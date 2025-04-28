namespace Recipe_Sharing_Platform.DTOs
{
    public class GetRecipesDto
    {
        public string keyword { get; set; }
        public int? chefId { get; set; }
        public int? labelId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
