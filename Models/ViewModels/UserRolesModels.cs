namespace BibliotecaWebApplicationMvc.Models.ViewModels
{
    public class UserRolesModels
    {
        public string? UserId { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}
