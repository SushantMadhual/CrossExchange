﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CrossExchange.Controller
{
    [Route("api/Portfolio")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioRepository _portfolioRepository { get; set; }

        public PortfolioController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("{portFolioid}")]
        public async Task<IActionResult> GetPortfolioInfo([FromRoute]int portFolioid)
        {
            var portfolio = _portfolioRepository.GetAll().Where(x => x.Id.Equals(portFolioid));
            
            return Ok(portfolio);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Portfolio value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //BUG Fixed - User can not start trade before register.
            if (value.Trade != null || value.Trade?.Count > 0)
            {
                var result = new BadRequestObjectResult(new { message = "User start trading after profile creation" });
                return result;
                //return BadRequest(value);
            }
            await _portfolioRepository.InsertAsync(value);


            return Created($"Portfolio/{value.Id}", value);
        }

    }
}
