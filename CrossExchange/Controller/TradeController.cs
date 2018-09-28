using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CrossExchange.Controller
{
    [Route("api/Trade")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }


        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portFolioid)
        {
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));
            return Ok(trade);
        }



        /*************************************************************************************************************************************
        For a given portfolio, with all the registered shares you need to do a trade which could be either a BUY or SELL trade. 
        For a particular trade keep following conditions in mind:
		BUY:
        a) The rate at which the shares will be bought will be the latest price in the database.
		b) The share specified should be a registered one otherwise it should be considered a bad request. 
		c) The Portfolio of the user should also be registered otherwise it should be considered a bad request. 
                
        SELL:
        a) The share should be there in the portfolio of the customer.
		b) The Portfolio of the user should be registered otherwise it should be considered a bad request. 
		c) The rate at which the shares will be sold will be the latest price in the database.
        d) The number of shares should be sufficient so that it can be sold. 
        Hint: You need to group the total shares bought and sold of a particular share and see the difference to figure out if there are sufficient quantities available for SELL. 

        *************************************************************************************************************************************/

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TradeModel model)

        {
            Trade createTrade = new Trade();
            createTrade.PortfolioId = model.PortfolioId;
            createTrade.Symbol = model.Symbol;
            createTrade.NoOfShares = model.NoOfShares;
            createTrade.Action = model.Action;

            int availableShares = 0;
            var portfolio = _portfolioRepository.GetAll().Where(x => x.Id.Equals(model.PortfolioId)).FirstOrDefault();
            
            //Task c(Buy) and d(Sell)
            if (portfolio==default(Portfolio) )
                return BadRequest(model);

            var shares = _shareRepository.Query().Where(x => x.Symbol.Equals(model.Symbol)).ToList();
      
            //Task b(Buy) 
            if (shares == null || shares?.Count == 0)
                return BadRequest(model);

            // Task a(Buy)
            var latestprice =  _shareRepository.Query().Where(x => x.Symbol.Equals(model.Symbol)).OrderByDescending(x=>x.TimeStamp).FirstOrDefault();
            var rate = latestprice?.Rate;
            createTrade.Price =(createTrade.NoOfShares* rate.Value);

            
           
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(model.PortfolioId) && x.Symbol.Equals(model.Symbol));
            if (trade != null)
            {
                var totalBuy = trade.Where(x => x.Action.Equals("BUY")).Sum(x => x.NoOfShares);
                var totalSell = trade.Where(x => x.Action.Equals("SELL")).Sum(x => x.NoOfShares);
                availableShares = totalBuy - totalSell;
            }
            
            //Task d(Sell)
            if(model.Action=="SELL" && availableShares<model.NoOfShares)
            {
                return BadRequest(model);
            }

            await _tradeRepository.InsertAsync(createTrade);

            return Created("Trade", createTrade);
        }
        
    }
}
