using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC077EC131E1");

            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07A6870605");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA90956645").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Books_ToTable");
        });


        modelBuilder.Entity<IdentityRole>()
            .HasData(
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "32fe658e-2cf4-44c2-a86c-771766fd7158",
            },
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = "a21a58d3-d387-4710-9f38-659c6c53a638",
            }
            );

        modelBuilder.Entity<ApiUser>()
           .HasData(
           new ApiUser
           {
               Id = "8182828c-3c9e-40eb-9751-0e43bdc33402",
               Email = "admin@bookstore.com",
               NormalizedEmail = "ADMIN@BOOKSTORE.COM",
               UserName = "admin",
               NormalizedUserName = "ADMIN",
               FirstName = "System",
               LastName = "Admin",
               PasswordHash = "AQAAAAIAAYagAAAAEL69nkno0DdRn85YvRHDgPT3Am0EBzl7+SKyXZCpCcfO+rlmgmMyRBWf5uLU4+pqeQ==",
               ConcurrencyStamp = "9a5dc9cb-d59c-4e55-9ded-88f6888085ae",
               SecurityStamp = "32f555f8-05d5-4ece-b4ae-99bc3ab9ed73"
           },
           new ApiUser
           {
               Id = "95bb6474-6e3d-46c1-a644-21397c83dc17",
               Email = "user@bookstore.com",
               NormalizedEmail = "USER@BOOKSTORE.COM",
               UserName = "user",
               NormalizedUserName = "USER",
               FirstName = "System",
               LastName = "User",
               PasswordHash = "AQAAAAIAAYagAAAAECZJ8C+Zr9Od4tL9J1nb+JKoPn9ryyza93lwIjvtZOuTCvOqhg7VGHnb2t0+Y7w6mQ==",
               ConcurrencyStamp = "2257c8eb-211a-4f12-be1c-78456fef12c1",
               SecurityStamp = "e516f4c6-9e0b-438e-aef8-3a1517997d76"
           }
           );

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "32fe658e-2cf4-44c2-a86c-771766fd7158",
                    UserId = "95bb6474-6e3d-46c1-a644-21397c83dc17"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "a21a58d3-d387-4710-9f38-659c6c53a638",
                    UserId = "8182828c-3c9e-40eb-9751-0e43bdc33402"
                }
            );


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
