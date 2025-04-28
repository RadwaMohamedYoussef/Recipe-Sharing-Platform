namespace Recipe_Sharing_Platform.DTOs
{
    public class RecipeResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ChefName { get; set; }
        public DateTime PublishedAt { get; set; }
        public List<string> Labels { get; set; }
    }
}
