using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
namespace CrossExchange.Tests
{
    public class DBContextTest
    {
        //Protfolio 
        private void InitDbContext(ExchangeContext context)
        {

            context.Portfolios.Add(new Portfolio { Name = "Test10", Trade = null });
            context.Portfolios.Add(new Portfolio { Name = "Test11", Trade = null });
            

            context.Shares.Add(new HourlyShareRate { Rate = Convert.ToDecimal(50.00), Symbol = "TES", TimeStamp = DateTime.Now });
            context.Shares.Add(new HourlyShareRate { Rate = Convert.ToDecimal(50.10), Symbol = "SBI", TimeStamp = DateTime.Now });

            context.Trades.Add(new Trade { Action="BUY",NoOfShares=50,PortfolioId=1,Price=Convert.ToDecimal(60.10),Symbol="SAG" });
            context.Trades.Add(new Trade { Action = "SELL", NoOfShares = 500, PortfolioId = 2, Price = Convert.ToDecimal(600.10), Symbol = "ABG" });
            context.SaveChanges();
        }
        [Test]
        public void GetList_ShouldReturnCorrectCountTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new PortfolioRepository(context);

            // Act
            var result = repo.GetAll();
            var count = result.CountAsync();
            // Assert
            Assert.AreEqual(2, Convert.ToInt32(count.Result));
        }

        [Test]
        public void GetList_ShouldInsertCorrectly()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio1");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new PortfolioRepository(context);

            // Act
            var result = repo.InsertAsync(new Portfolio { Id = 20, Name = "15", Trade = null });
            var count = result.IsCompleted;
            //// Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count);
        }

        [Test]
        public void GetList_ShouldUpdateCorrectlyTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio2");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new PortfolioRepository(context);

            // Act
            var result = repo.UpdateAsync(new Portfolio { Id = 20, Name = "15", Trade = null });
            var count = result.IsCompleted;
            //// Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count);
        }

        //Share
        [Test]
        public void GetList_ShouldReturnCorrectShareCountTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio4");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new ShareRepository(context);

            // Act
            var result = repo.Query();
            var count = result.CountAsync();
            // Assert
            Assert.AreEqual(2, Convert.ToInt32(count.Result));
        }

        [Test]
        public void GetList_ShouldInsertShareCorrectly()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio5");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new ShareRepository(context);
            var rate = Convert.ToDecimal(50.00);
            // Act
            var result = repo.InsertAsync(new HourlyShareRate { Rate= rate, Symbol="TES",TimeStamp=DateTime.Now});
            var count = result.IsCompleted;
            //// Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count);
        }

        [Test]
        public void GetList_ShouldUpdateCorrectlyShareTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio6");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new ShareRepository(context);
            var rate = Convert.ToDecimal(50.00);
            // Act
            var result = repo.UpdateAsync(new HourlyShareRate { Rate = rate, Symbol = "TES", TimeStamp = DateTime.Now });
            var count = result.IsCompleted;
            //// Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count);
        }

        //Trade
        [Test]
        public void Query_ShouldReturnCorrectTradeCountTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio7");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new TradeRepository(context);

            // Act
            var result = repo.Query();
            var count = result.CountAsync();
            // Assert
            Assert.AreEqual(2, Convert.ToInt32(count.Result));
        }

        [Test]
        public void Insert_ShouldInsertTradeTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio8");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new TradeRepository(context);
            var rate = Convert.ToDecimal(50.00);
            // Act
            var result = repo.InsertAsync(new Trade { Action = "SELL", NoOfShares = 500, PortfolioId = 2, Price = Convert.ToDecimal(600.10), Symbol = "ABG" });
            var count = result.IsCompleted;
            //// Assert
            Assert.IsNotNull(count);
            Assert.IsTrue(count);
        }

        [Test]
        public void Update_ShouldUpdateTradeTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ExchangeContext>();
            builder.UseInMemoryDatabase(databaseName:
                     "TestPortfolio9");

            var context = new ExchangeContext(builder.Options);
            InitDbContext(context);

            var repo = new TradeRepository(context);
            var rate = Convert.ToDecimal(50.00);
            // Act
            var result = repo.UpdateAsync(new Trade { Action = "SELL", NoOfShares = 500, PortfolioId = 2, Price = Convert.ToDecimal(600.10), Symbol = "ABG" });
            var count = result.IsCompleted;
            //// Asserttrat
            Assert.IsNotNull(count);
            Assert.IsTrue(count);
        }
    }
}
