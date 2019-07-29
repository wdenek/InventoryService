using AutoFixture;
using FluentAssertions;
using InventoryService.Controllers;
using InventoryService.Mappers;
using InventoryService.Models;
using InventoryService.Queries;
using InventoryService.UnitTest.TestTools;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BestelService.UnitTest
{
    [TestClass]
    public class ProductControllerTests
    {
        private Fixture _fixture;
        private Mock<IMediator> _mediatorMock;
        private ProductController _sut;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _mediatorMock = new Mock<IMediator>();
            _sut = new ProductController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task ProductController_Get_ReturnsCorrectProducts()
        {
            // Arrange
            var expectedProducts = _fixture.CreateMany<Product>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(expectedProducts);

            // Act
            var resultBroden = await _sut.Get();

            // Assert
            Assert.IsNotNull(resultBroden);
            Assert.IsInstanceOfType(resultBroden, typeof(IEnumerable<Product>));
            Assert.AreEqual(expectedProducts, resultBroden);
        }

        [TestMethod]
        public async Task ProductController_GetById_ReturnsCorrectItem()
        {
            int expectedProductId = 12;

            // Arrange
            
            // Approach 1: make use of GetHashCode and Equals function in the GetProductQuery
            _mediatorMock.Setup(m => m.Send(new GetProductQuery(expectedProductId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Product(expectedProductId, "Wit"));

            // Approach 2: make use of It.Is<GetProductQuery>
            _mediatorMock.Setup(m => m.Send(It.Is<GetProductQuery>(q => q.Id == expectedProductId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Product(expectedProductId, "Wit"));
            
            // Act
            var result = await _sut.Get(expectedProductId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedProductId, result.Id);
        }

        [TestMethod]
        public async Task ProductController_SearchBad_ProductsReturned()
        {
            // Arrange
            var request = _fixture.Create<SearchProductsRequest>();
            var expectedProducts = _fixture.CreateMany<Product>();

            _mediatorMock
                .Setup(m => m.Send(It.Is<SearchProductsQuery>(
                    qry => qry.Name == request.Name &&
                        qry.Description == request.Description &&
                        qry.Category == request.Category &&
                        qry.IsInStock == request.IsInStock),
                    default)
                )
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.SearchBad(request);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [TestMethod]
        public async Task ProductController_Search_FluentAssertions_ProductsReturned()
        {
            // Arrange
            var request = _fixture.Create<SearchProductsRequest>();
            var expectedQuery = request.ToQuery();
            var expectedProducts = _fixture.CreateMany<Product>();

            _mediatorMock.Setup(m => m.Send(Its.EquivalentTo(expectedQuery), default))
                         .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.Search(request);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [TestMethod]
        public async Task ProductController_Search_FluentAssertions_InvalidRequest_ShouldFail()
        {
            // Arrange
            var invalidRequest = _fixture.Create<SearchProductsRequest>();
            var expectedQuery = _fixture.Create<SearchProductsQuery>();
            var expectedProducts = _fixture.CreateMany<Product>();

            _mediatorMock.Setup(m => m.Send(Its.EquivalentTo(expectedQuery), default))
                         .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.Search(invalidRequest);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }
    }
}
