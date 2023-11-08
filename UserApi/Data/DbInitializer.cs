using ProjectX;
using TestApplication.Models;

public static class DbInitializer
{
    public static void Initialize(DataContext context)
    {
        context.Database.EnsureCreated();

        var roles = new List<RoleEntity>
        {
            new() { Id = Guid.NewGuid(), Role = "User" },
            new() { Id = Guid.NewGuid(), Role = "Admin" },
            new() { Id = Guid.NewGuid(), Role = "Support" },
            new() { Id = Guid.NewGuid(), Role = "SuperAdmin" }
        };

        if (!context.Roles.Any())
        {
            Console.WriteLine("Roles");
            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        var users = new List<UserEntity>
        {
            new()
            {
                Id = Guid.NewGuid(), Login = "name1", Password = "1", Salt = HashHandler.GenerateSalt(30),
                Email = "email1",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(), Login = "name2", Password = "2", Salt = HashHandler.GenerateSalt(30),
                Email = "email2",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(), Login = "name3", Password = "3", Salt = HashHandler.GenerateSalt(30),
                Email = "email3",
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            }
        };

        if (!context.Users.Any())
        {
            Console.WriteLine("Users");
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        if (!context.Roles.Any())
        {
            Console.WriteLine("Roles");
            context.Roles.AddRange(roles);
            context.SaveChanges();

            var userRoles = new List<UserRoleEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(), UserId = users[0].Id, RoleId = roles[0].Id, DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(), UserId = users[1].Id, RoleId = roles[1].Id, DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(), UserId = users[1].Id, RoleId = roles[3].Id, DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(), UserId = users[2].Id, RoleId = roles[1].Id, DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(), UserId = users[2].Id, RoleId = roles[2].Id, DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                }
            };
            Console.WriteLine("UserRoles");
            context.UserRoles.AddRange(userRoles);
            context.SaveChanges();
        }
    }
}