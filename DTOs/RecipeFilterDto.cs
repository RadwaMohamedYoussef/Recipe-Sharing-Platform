namespace Recipe_Sharing_Platform.DTOs
{
    public class RecipeFilterDto
    {
        public string Keyword { get; set; }
        public int? ChefId { get; set; }
        public int? LabelId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
