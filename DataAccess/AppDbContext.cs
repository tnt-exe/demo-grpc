using Microsoft.EntityFrameworkCore;

namespace GrpcService1.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe"
                },
                new Employee
                {
                    EmployeeId = 2,
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Category 1"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Category 2"
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductName = "Product 1",
                    CategoryId = 1
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "Product 2",
                    CategoryId = 2
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "Product 3",
                    CategoryId = 1
                },
                new Product
                {
                    ProductId = 4,
                    ProductName = "Product 4",
                    CategoryId = 2
                }
            );
        }
    }
}
