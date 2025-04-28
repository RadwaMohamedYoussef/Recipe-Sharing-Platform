using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipe_Sharing_Platform.Data;
using Recipe_Sharing_Platform.DTOs;
using Recipe_Sharing_Platform.Entities;
using Recipe_Sharing_Platform.Helpers;
using Recipe_Sharing_Platform.Services;
using System.Security.Claims;

namespace Recipe_Sharing_Platform.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipes(GetRecipesDto dto )
        {
            var recipes = await _recipeService.GetRecipes(dto);
            return Ok(recipes);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromForm] RecipeCreateDto dto)
        {
            var isChef = User.Claims.FirstOrDefault(c => c.Type == "IsChef")?.Value;
            if (isChef != "True") return Forbid();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _recipeService.CreateRecipe(dto, userId);

            return Ok();
        }
    }
}


