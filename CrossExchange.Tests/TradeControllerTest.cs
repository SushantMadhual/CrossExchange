using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace CrossExchange.Tests
{
   public class TradeControllerTest
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly PortfolioController _portfolioController;
        private readonly ShareController _shareController;
        private readonly TradeController _tradeController;

        public TradeControllerTest()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portfolioRepositoryMock.Object);

        }

        [Test]
        public async Task Get_ShouldRetriveAllTradingById()
        {


            // Arrange
            int Id = 1;
            // Act
            var result = await _tradeController.GetAllTradings(Id);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldInsertTrade()
        {
            var tradeModel = new TradeModel
            {
                Action="BUY",
                NoOfShares=50,
                PortfolioId=1,
                Symbol="REL"
            };

            // Arrange

            // Act
            var result = await _tradeController.Post(tradeModel);

            // Assert
            Assert.NotNull(result);

            //var createdResult = result as CreatedResult;
            //Assert.NotNull(createdResult);
            //Assert.AreEqual(201, createdResult.StatusCode);
        }
    }
}
