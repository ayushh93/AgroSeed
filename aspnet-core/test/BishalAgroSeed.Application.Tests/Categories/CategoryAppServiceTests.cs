﻿using Azure;
using BishalAgroSeed.Categories;
using BishalAgroSeed.Configurations;
using BishalAgroSeed.Dtos;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
        public async Task CreateAsync_ShouldThrowValidationException_WhenCategoryNameIsDuplicate()
        {
            // Arrange
            var exceptionMessage = "Duplicate Category Name!!";
            var input = new CreateUpdateCategoryDto { DisplayName = "ExistingCategory" };

            // Set up the mock repository to return true for the AnyAsync method:
            _mockRepository.AnyAsync(Arg.Any<Expression<Func<Category, bool>>>())
            .Returns(true);

            // Act
            var exception = await Assert.ThrowsAsync<AbpValidationException>(() => _categoryAppService.CreateAsync(input));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains(exceptionMessage, exception.Message);
        }
        [Fact]
        public async Task CreateAsync_ShouldCreateCategory_WhenCategoryNameIsUnique()
        {
            // Arrange
            var input = new CreateUpdateCategoryDto { DisplayName = "NewUniqueCategory", ParentId = null, IsActive = true };
            var category = new Category(new Guid(), "NewUniqueCategory", new Guid(), true);

            // Set up the mock repository to return false for the AnyAsync method:
            _mockRepository.AnyAsync(Arg.Any<Expression<Func<Category, bool>>>())
                .Returns(false);

            // Set up the mock repository for the InsertAsync method:
            _mockRepository.InsertAsync(Arg.Any<Category>()).Returns(category);

            // Act
            var result = await _categoryAppService.CreateAsync(input);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.DisplayName);
            Assert.Equal(input.DisplayName, result.DisplayName);
        }

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnFilteredDropdownDtos()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category(Guid.NewGuid(), "Category1", new Guid(), true),
                new Category(Guid.NewGuid(), "Category2", new Guid(), false),
                new Category(Guid.NewGuid(), "Category3", new Guid(), true),
                new Category(Guid.NewGuid(), "Category4", new Guid(), false),
                new Category(Guid.NewGuid(), "Category5", new Guid(), true),
                new Category(Guid.NewGuid(), "Category6", new Guid(), false)
            };

            var categoryFilter = new GetCategoryFilter { Id = categories[0].Id };

            var expectedResult = new List<DropdownDto> {
                new DropdownDto(categories[2].Id.ToString(), "Category3"),
                new DropdownDto(categories[4].Id.ToString(), "Category5"),
            };

            //Setup mock repository for GetQueryableAsync method to return categories as IQueryable 
            _mockRepository.GetQueryableAsync().Returns(categories.AsQueryable());

            //Act
            var result = await _categoryAppService.GetCategoriesAsync(categoryFilter);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedResult[i].Value, result[i].Value.ToLower());
                Assert.Equal(expectedResult[i].Name, result[i].Name);
            };
            // This failed
            //Assert.Equal(expectedResult, result); 
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnPagedCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category (Guid.NewGuid(), "ParentCategory1", new Guid(), true),
                new Category (Guid.NewGuid(), "ParentCategory2", new Guid(), true),
                new Category (Guid.NewGuid(), "ParentCategory3", new Guid(), true),
            };
            var childCategory = new Category(Guid.NewGuid(), "ChildCategory1", categories[0].Id, true);
            categories.Add(childCategory);

            PagedAndSortedResultRequestDto input = new PagedAndSortedResultRequestDto();
            input.SkipCount = 1;
            input.MaxResultCount = 5;

            var expectedResult= new List<CategoryDto>
            {
                new CategoryDto
                {
                    Id = categories[3].Id,
                    DisplayName = "ChildCategory1",
                    ParentId = categories[0].Id,
                    ParentName = "ParentCategory1",
                    IsActive = true
                },
                //new CategoryDto
                //{
                //    Id = categories[0].Id,
                //    DisplayName = "ParentCategory1",
                //    ParentId = null,
                //    IsActive = true
                //},
                new CategoryDto
                {
                    Id = categories[1].Id,
                    DisplayName = "ParentCategory2",
                    ParentId = null,
                    IsActive = true
                },
                new CategoryDto
                {
                    Id = categories[2].Id,
                    DisplayName = "ParentCategory3",
                    ParentId = null,
                    IsActive = true
                }                
            };

            //Setup mock repository for GetQueryableAsync method to return categories as IQueryable 
            _mockRepository.GetQueryableAsync().Returns(categories.AsQueryable());

            //Act
            var result = await _categoryAppService.GetListAsync(input);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Items);
            Assert.Equal(categories.Count,result.TotalCount);
            Assert.Equal(expectedResult.Count, result.Items.Count);
            for (int i = 0; i < result.Items.Count; i++)
            {
                Assert.Equal(expectedResult[i].Id, result.Items[i].Id);
                Assert.Equal(expectedResult[i].DisplayName, result.Items[i].DisplayName);
                Assert.Equal(expectedResult[i].DisplayName, result.Items[i].DisplayName);
                Assert.Equal(expectedResult[i].ParentId, result.Items[i].ParentId);
                Assert.Equal(expectedResult[i].ParentName, result.Items[i].ParentName);
                Assert.Equal(expectedResult[i].IsActive, result.Items[i].IsActive);
            };
            //This failed
            //Assert.Equal(expectedResult, result.Items);

        }
    }

}