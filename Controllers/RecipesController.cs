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
        private static readonly Dictionary<string, string> CityTimeZoneMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    // Middle East
    { "riyadh", "Arab Standard Time" },
    { "cairo", "Egypt Standard Time" },
    { "dubai", "Arabian Standard Time" },
    { "amman", "Jordan Standard Time" },
    { "beirut", "Middle East Standard Time" },
    { "baghdad", "Arabic Standard Time" },
    { "doha", "Arab Standard Time" },
    { "manama", "Arab Standard Time" },
    { "muscat", "Gulf Standard Time" },
    { "kuwait", "Arab Standard Time" },
    { "jerusalem", "Israel Standard Time" },
    { "tehran", "Iran Standard Time" },
    
    // Africa
    { "lagos", "W. Central Africa Standard Time" },
    { "nairobi", "E. Africa Standard Time" },
    { "johannesburg", "South Africa Standard Time" },
    { "addis ababa", "E. Africa Standard Time" },
    { "algiers", "W. Central Africa Standard Time" },
    { "casablanca", "Morocco Standard Time" },
    
    // Europe
    { "london", "GMT Standard Time" },
    { "paris", "Romance Standard Time" },
    { "berlin", "W. Europe Standard Time" },
    { "madrid", "Romance Standard Time" },
    { "rome", "W. Europe Standard Time" },
    { "amsterdam", "W. Europe Standard Time" },
    { "vienna", "W. Europe Standard Time" },
    { "oslo", "W. Europe Standard Time" },
    { "stockholm", "W. Europe Standard Time" },
    { "helsinki", "FLE Standard Time" },
    { "athens", "GTB Standard Time" },
    { "istanbul", "Turkey Standard Time" },
    { "moscow", "Russian Standard Time" },
    
    // Asia
    { "karachi", "Pakistan Standard Time" },
    { "delhi", "India Standard Time" },
    { "kolkata", "India Standard Time" },
    { "mumbai", "India Standard Time" },
    { "dhaka", "Bangladesh Standard Time" },
    { "bangkok", "SE Asia Standard Time" },
    { "jakarta", "SE Asia Standard Time" },
    { "manila", "Singapore Standard Time" },
    { "kuala lumpur", "Singapore Standard Time" },
    { "singapore", "Singapore Standard Time" },
    { "beijing", "China Standard Time" },
    { "shanghai", "China Standard Time" },
    { "hong kong", "China Standard Time" },
    { "tokyo", "Tokyo Standard Time" },
    { "seoul", "Korea Standard Time" },

    // Australia & Oceania
    { "sydney", "AUS Eastern Standard Time" },
    { "melbourne", "AUS Eastern Standard Time" },
    { "brisbane", "E. Australia Standard Time" },
    { "perth", "W. Australia Standard Time" },
    { "auckland", "New Zealand Standard Time" },
    { "wellington", "New Zealand Standard Time" },
    { "suva", "Fiji Standard Time" },

    // North America
    { "new york", "Eastern Standard Time" },
    { "washington", "Eastern Standard Time" },
    { "toronto", "Eastern Standard Time" },
    { "chicago", "Central Standard Time" },
    { "dallas", "Central Standard Time" },
    { "denver", "Mountain Standard Time" },
    { "phoenix", "US Mountain Standard Time" },
    { "los angeles", "Pacific Standard Time" },
    { "vancouver", "Pacific Standard Time" },
    { "mexico city", "Central Standard Time (Mexico)" },

    // South America
    { "sao paulo", "E. South America Standard Time" },
    { "buenos aires", "Argentina Standard Time" },
    { "bogota", "SA Pacific Standard Time" },
    { "lima", "SA Pacific Standard Time" },
    { "caracas", "Venezuela Standard Time" },
    { "santiago", "Pacific SA Standard Time" },

    // Others
    { "honolulu", "Hawaiian Standard Time" },
    { "anchorage", "Alaskan Standard Time" },
    { "reykjavik", "Greenwich Standard Time" },
    { "kathmandu", "Nepal Standard Time" },
    { "yangon", "Myanmar Standard Time" }
};

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
        [HttpGet("{city}")]
        public IActionResult GetTimeInfo(string city)
        {
            if (!CityTimeZoneMap.TryGetValue(city, out var timeZoneId))
            {
                return NotFound(new { Message = "City not recognized. Please use a valid city name." });
            }

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

                var result = new
                {
                    City = city,
                    TimeZoneId = tz.Id,
                    TimeZoneDisplayName = tz.DisplayName,
                    LocalTime = localTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    UtcOffset = tz.BaseUtcOffset.ToString(),
                    IsDaylightSavingTime = tz.IsDaylightSavingTime(localTime)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Unexpected error", Details = ex.Message });
            }
        }

    }
}


