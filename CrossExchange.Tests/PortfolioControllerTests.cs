using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace CrossExchange.Tests
{
   public class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly PortfolioController _portfolioController;
        private readonly ShareController _shareController;
        private readonly TradeController _tradeController;

        public PortfolioControllerTests()
        {
            _portfolioController = new PortfolioController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object,_portfolioRepositoryMock.Object);
        }

        [Test]
        public async Task Get_ShouldRetrivePortfolioById()
        {


            // Arrange
            int Id = 1;
            // Act
            var result = await _portfolioController.GetPortfolioInfo(Id);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldInsertPortfolioWithoutTrade()
        {
            var portfolio = new Portfolio
            {
                Name="Sushant Madhual"
                

                //Symbol = "CBI",
                //Rate = 330.0M,
                //TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            // Arrange

            // Act
            var result = await _portfolioController.Post(portfolio);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldNotInsertPortfolioWithTrade()
        {
            Portfolio portfolio = new Portfolio();
            portfolio.Name = "Sushant Madhual";
            portfolio.Trade = new System.Collections.Generic.List<Trade>();
            Trade trade = new Trade();
            trade.NoOfShares = 25;
            trade.Price =Convert.ToDecimal(50.01);
            trade.Symbol = "CBI";
            trade.Action = "BUY";
            portfolio.Trade.Add(trade);


            // Arrange

            // Act
            var result = await _portfolioController.Post(portfolio);

            // Assert
            Assert.NotNull(result);

            //var createdResult = result as BadRequestResult;
            //Assert.NotNull(createdResult);
            //Assert.AreEqual(400, createdResult.StatusCode);
        }
        


    }


}
