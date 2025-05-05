using JWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWT.Data
{
    public class ProductDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public ProductDbContext()
        {
            
        }
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var user1 = new AppUser 
            {
                Id = 1,
                UserName = "superUser",
                NormalizedUserName = "SUPERUSER",
                Email = "super@user.com",
                NormalizedEmail = "SUPER@USER.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var user2 = new AppUser
            {
                Id = 2,
                UserName = "cevdo",
                NormalizedUserName = "CEVDO",
                Email = "cevdo@user.com",
                NormalizedEmail = "CEVDO@USER.COM",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
            user1.PasswordHash = passwordHasher.HashPassword(user1, "Super@123");
            user2.PasswordHash = passwordHasher.HashPassword(user2, "cEVDO@123");
            builder.Entity<AppUser>().HasData(user1, user2);

            builder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "T-shirt",
                    Description = "Pamuklu",
                    Price = 400
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Gomlek",
                    Description = "Kısa kollu",
                    Price = 600
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Kazak",
                    Description = "Yun V yaka",
                    Price = 1200
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Ceket",
                    Description = "Deri siyah",
                    Price = 4000
                }
            );
        }
    }
}
