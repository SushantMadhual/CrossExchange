using System;
using System.Threading.Tasks;
using CrossExchange.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CrossExchange.Tests
{

    public class MiddlewareTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MiddlewareTest()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Test]
        public async Task Index_Get_ReturnsIndexHtmlPage()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            HttpStatusCode responseMessage = response.StatusCode;
            Assert.AreEqual(HttpStatusCode.NotFound, responseMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert.Contains("<title>Home Page - BlogPlayground</title>", responseString);
            Assert.IsEmpty(responseString);
        }

       

    }


   
}
