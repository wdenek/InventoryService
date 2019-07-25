using AutoFixture;
using InventoryService.Controllers;
using InventoryService.Models;
using InventoryService.Queries;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;

namespace BestelService.UnitTest
{
    [TestClass]
    public class ProductControllerTests
    {
        private Fixture _fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void ProductController_Get_ReturnsCorrectProducts()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var expectedProducts = _fixture.CreateMany<Product>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(expectedProducts);

            var sut = new ProductController(mediatorMock.Object);

            // Act
            var resultBroden = sut.Get().Result;

            // Assert
            Assert.IsNotNull(resultBroden);
            Assert.IsInstanceOfType(resultBroden, typeof(IEnumerable<Product>));
            Assert.AreEqual(expectedProducts, resultBroden);
        }

        [TestMethod]
        public void ProductController_GetById_ReturnsCorrectItem()
        {
            int expectedProductId = 12;

            // Arrange
            var mediatorMock = new Mock<IMediator>();

            // Approach 1: make use of GetHashCode and Equals function in the GetProductQuery
            mediatorMock.Setup(m => m.Send(new GetProductQuery(expectedProductId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Product(expectedProductId, "Wit"));

            // Approach 2: make use of It.Is<GetProductQuery>
            mediatorMock.Setup(m => m.Send(It.Is<GetProductQuery>(q => q.Id == expectedProductId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Product(expectedProductId, "Wit"));
            
            var sut = new ProductController(mediatorMock.Object);

            // Act
            var result = sut.Get(expectedProductId).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProductId, result.Id);
        }
    }
}
