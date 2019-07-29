using AutoFixture;
using InventoryService.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using InventoryService.Mappers;
using InventoryService.Models;
using FluentAssertions;

namespace InventoryService.UnitTest.Mappers
{
    [TestClass]
    public class SearchProductsRequestMapperTests
    {
        [TestMethod]
        public void ToQuery_ValidRequest_QueryReturned()
        {
            // Arrange
            var request = new Fixture().Create<SearchProductsRequest>();

            // Act
            var result = request.ToQuery();

            // Assert
            result.Should().BeEquivalentTo(request);
        }
    }
}
