using Microsoft.EntityFrameworkCore;
using Recipe_Sharing_Platform.Data;
using Recipe_Sharing_Platform.DTOs;
using Recipe_Sharing_Platform.Entities;
using Recipe_Sharing_Platform.Helpers;

namespace Recipe_Sharing_Platform.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RecipeService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<object> GetRecipes(GetRecipesDto dto)
        {
            var query = _context.Recipes
                .Include(r => r.RecipeLabels).ThenInclude(rl => rl.Label)
                .Include(r => r.Chef)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.keyword))
                query = query.Where(r => r.Title.Contains( dto.keyword) || r.Description.Contains( dto.keyword));

            if (dto. chefId.HasValue)
                query = query.Where(r => r.ChefId == dto.chefId.Value);

            if (dto. labelId.HasValue)
                query = query.Where(r => r.RecipeLabels.Any(rl => rl.LabelId == dto.labelId.Value));

            if (dto. fromDate.HasValue)
                query = query.Where(r => r.PublishedAt >= dto.fromDate.Value);

            if (dto.toDate.HasValue)
                query = query.Where(r => r.PublishedAt <= dto.toDate.Value);

            var totalItems = await query.CountAsync();
            var recipes = await query.Skip((dto.pageNumber - 1) * dto.pageSize).Take(dto.pageSize).ToListAsync();

            return new
            {
                currentPage = dto.pageNumber,
                totalPages = (int)Math.Ceiling(totalItems / (double)dto. pageSize),
                recipes = recipes.Select(r => new RecipeResponseDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    ChefName = r.Chef.Username,
                    PublishedAt = r.PublishedAt,
                    Labels = r.RecipeLabels.Select(rl => rl.Label.Name).ToList()
                })
            };
        }

        public async Task CreateRecipe(RecipeCreateDto dto, int userId)
        {
            var uploadPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "recipes");
            var imageName = await ImageHelper.SaveResizedImageAsync(dto.Image, uploadPath);

            var recipe = new Recipe
            {
                Title = dto.Title,
                Description = dto.Description,
                Ingredients = dto.Ingredients,
                Instructions = dto.Instructions,
                ImagePath = $"/uploads/recipes/{imageName}",
                ChefId = userId
            };

            recipe.RecipeLabels = dto.LabelIds.Select(id => new RecipeLabel { LabelId = id }).ToList();

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }
            public async Task AddLabelAsync(string labelName)
    {
        var label = new Label { Name = labelName };
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();
    }
    }
    }
