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

            // This is your Approach 2 wich I wouldn't use.
            // Currently it's still sort of readable but with more complex mapping logic this would be horrible.
            // You would also have to verify all different mapping cases if the mapping is more complex with conditional logic.
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

            // Something like this would be my preferred approach.
            // The ToQuery mapping method is unit tested separately and reused in this test.
            // I've created a helper method to uses FluentAssertions to compare the
            // expectedObject with the object that is passed into Send.
            _mediatorMock.Setup(m => m.Send(Its.EquivalentTo(expectedQuery), default))
                         .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.Search(request);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [TestMethod]
        public async Task ProductController_Search_FluentAssertionsMetExtensionMethod_ProductsReturned()
        {
            // Arrange
            var request = _fixture.Create<SearchProductsRequest>();
            var expectedQuery = request.ToQuery();
            var expectedProducts = _fixture.CreateMany<Product>();

            // Same approach as above but with an extension method.
            // AsExpectedObject() should be called within the Setup or it doesn't work.
            _mediatorMock.Setup(m => m.Send(expectedQuery.AsExpectedObject(), default))
                         .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.Search(request);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [TestMethod]
        public async Task ProductController_Search_FluentAssertions_InvalidRequest_DoesNotReturnProducts()
        {
            // Arrange
            var invalidRequest = _fixture.Create<SearchProductsRequest>();
            var expectedQuery = _fixture.Create<SearchProductsQuery>();
            var expectedProducts = _fixture.CreateMany<Product>();

            // Example that shows how a failure looks liek.
            // See the Output of the test for the details on why the comparison failed.
            _mediatorMock.Setup(m => m.Send(Its.EquivalentTo(expectedQuery), default))
                         .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.Search(invalidRequest);

            // Assert
            result.Should().BeEmpty(); //empty because command doesn't match request in setup
        }

        [TestMethod]
        public async Task ProductController_Search_FluentAssertions_DifferenceBetweenCommandAndRequestButPropertyIsIgnored_ProductsReturned()
        {
            // Arrange
            var request = _fixture.Create<SearchProductsRequest>();
            var expectedQuery = request.ToQuery();
            expectedQuery.Description = "Different value as request";
            var expectedProducts = _fixture.CreateMany<Product>();

            _mediatorMock
                .Setup(m => m.Send(
                    Its.EquivalentTo(expectedQuery, options => options.Excluding(qry => qry.Description)),
                    default)
                )
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.Search(request);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }
    }
}
