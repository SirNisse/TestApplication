using Microsoft.EntityFrameworkCore;
using TestApplication.Models;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TestApplication.DAL;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext()
    {

    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
    }

    public new DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<AttachmentLibrary> AttachmentLibraries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.AttachmentLibrary)
            .WithMany(al => al.Attachments)
            .HasForeignKey(a => a.AttachmentLibraryId);

        modelBuilder.Entity<AttachmentLibrary>()
            .HasOne(al => al.Project)
            .WithMany(m => m.AttachmentLibraries)
            .HasForeignKey(al => al.ProjectId);


        string ADMIN_ID = "87a1532f-21b5-4454-a7dc-c9fd816c18ed";
        string ROLE_ID = "4fc03322-7480-4d47-bd1d-a76e1657ba3f";

        modelBuilder.Entity<IdentityRole>()
            .HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "User",
                NormalizedName = "USER"
            });
             
        modelBuilder.Entity<IdentityUser>()
            .HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = "sysadmin",
                NormalizedUserName = "SYSADMIN",
                PasswordHash = "AQAAAAIAAYagAAAAELWhMtasyn+5XDibQslsQsC3UOyBUvcQbvsnf2jdDPycnbRKRcxLAI8/oKRux1DGBQ==", // Snöyra1#
                Email = "simon.dahlstrom@hotmail.com",
                NormalizedEmail = "simon.dahlstrom@hotmail.com",
                EmailConfirmed = true
            });

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasData(new IdentityUserRole<string>
            {                
                UserId = ADMIN_ID,
                RoleId = ROLE_ID
            });

        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {     
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=test;Trusted_Connection=True;TrustServerCertificate=True");
    }

}
