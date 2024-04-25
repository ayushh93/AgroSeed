using Azure;
using BishalAgroSeed.Categories;
using BishalAgroSeed.Configurations;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;
using Xunit;

namespace BishalAgroSeed.Application.Tests.Categories
{
    public class CategoryAppServiceTests
    {
        private readonly CategoryAppService _categoryAppService;
        private readonly IRepository<Category, Guid> _mockRepository;

        public CategoryAppServiceTests()
        {
            _mockRepository = Substitute.For<IRepository<Category, Guid>>();
            _categoryAppService = new CategoryAppService(_mockRepository);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowAbpValidationException_WhenCategoryNameIsDuplicate()
        {
            // Arrange
            var exceptionMessage = "Duplicate Category Name!!";
            var input = new CreateUpdateCategoryDto { DisplayName = "ExistingCategory" };

            //code to return fake response for AnyAsync method: 
                _mockRepository.AnyAsync(Arg.Any<Expression<Func<Category, bool>>>())
            .Returns(true);

            // Act
            var exception = await Assert.ThrowsAsync<AbpValidationException>(() => _categoryAppService.CreateAsync(input));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains(exceptionMessage, exception.Message);
        }

        

    }

}
