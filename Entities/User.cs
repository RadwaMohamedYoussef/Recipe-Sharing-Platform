namespace Recipe_Sharing_Platform.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public bool IsChef { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}

