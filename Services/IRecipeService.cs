using Recipe_Sharing_Platform.DTOs;

namespace Recipe_Sharing_Platform.Services
{
    public interface IRecipeService
    {
        Task<object> GetRecipes(GetRecipesDto dto);
        Task CreateRecipe(RecipeCreateDto dto, int userId);
    }
}
