namespace Recipe_Sharing_Platform.DTOs
{
    public class RecipeCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public List<int> LabelIds { get; set; }
        public IFormFile Image { get; set; }
    }
}
