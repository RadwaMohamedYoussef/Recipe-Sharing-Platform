namespace Recipe_Sharing_Platform.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string ImagePath { get; set; }
        public int ChefId { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public User Chef { get; set; }
        public ICollection<RecipeLabel> RecipeLabels { get; set; }
    }
}
