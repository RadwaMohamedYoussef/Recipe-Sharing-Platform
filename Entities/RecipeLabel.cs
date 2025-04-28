namespace Recipe_Sharing_Platform.Entities
{
    public class RecipeLabel
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int LabelId { get; set; }
        public Label Label { get; set; }
    }
}
