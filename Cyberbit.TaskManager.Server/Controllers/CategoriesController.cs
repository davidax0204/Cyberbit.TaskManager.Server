using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models;
using Cyberbit.TaskManager.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoriesBl _categoriesBl;
        private readonly IAutoMapperService _autoMapper;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoriesBl categoriesBl, IAutoMapperService autoMapper)
        {
            _logger = logger;
            _categoriesBl = categoriesBl;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var allCategories = await _categoriesBl.GetAllCategories();
            if (allCategories == null)
            {
                var errMsg = "Failed to get all Categories";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }
            var allCategoriesDto = _autoMapper.Mapper.Map<List<CategoryDto>>(allCategories);
            return allCategoriesDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                var errMsg = $"Input Category dto model is invalid";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var Category = _autoMapper.Mapper.Map<Category>(categoryDto);
            var addedCategory = await _categoriesBl.AddCategory(Category);
            if (addedCategory == null)
            {
                var errMsg = $"Failed to add Category. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var addedCategoryDto = _autoMapper.Mapper.Map<CategoryDto>(addedCategory);
            return addedCategoryDto;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<CategoryDto>> UpdateCategory([FromRoute] int id, [FromBody] CategoryDto categoryDto)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to update Category with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            if (id != categoryDto.Id)
            {
                var errMsg = $"Failed to update Category. Id in url and in dto are mismatched";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var category = _autoMapper.Mapper.Map<Category>(categoryDto);
            Category updatedCategory = null;
            try
            {
                updatedCategory = await _categoriesBl.UpdateCategory(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to updated Category '{id}'");
            }

            if (updatedCategory == null)
            {
                var errMsg = $"Failed to update Category with id '{id}'. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var updatedCategoryDto = _autoMapper.Mapper.Map<CategoryDto>(updatedCategory);
            return updatedCategoryDto;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<CategoryDto>> DeleteCategory([FromRoute] int id)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to delete Category with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            Category Category = null;
            try
            {
                Category = await _categoriesBl.DeleteCategory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to delete Category '{id}'");
            }

            if (Category == null)
            {
                var errMsg = $"Failed to delete Category with id '{id}'. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var deletedCategoryDto = _autoMapper.Mapper.Map<CategoryDto>(Category);
            return deletedCategoryDto;
        }

    }
}
