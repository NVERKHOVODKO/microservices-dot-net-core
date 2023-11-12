namespace TestApplication.DTO
{
    public class UserWithRolesDTO
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public List<string> RoleNames { get; set; }
    }
}