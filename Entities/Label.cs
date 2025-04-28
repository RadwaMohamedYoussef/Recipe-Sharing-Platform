namespace Recipe_Sharing_Platform.Entities
{
    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RecipeLabel> RecipeLabels { get; set; }
    }
}
