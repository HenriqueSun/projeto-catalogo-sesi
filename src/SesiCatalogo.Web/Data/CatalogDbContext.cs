using Microsoft.EntityFrameworkCore;
using SesiCatalogo.Web.Domain;

namespace SesiCatalogo.Web.Data;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Modality> Modalities => Set<Modality>();
    public DbSet<CourseStatus> CourseStatuses => Set<CourseStatus>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseModule> CourseModules => Set<CourseModule>();
    public DbSet<Lesson> Lessons => Set<Lesson>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(60);
            entity.Property(c => c.IconKey).HasMaxLength(40);
            entity.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<Modality>(entity =>
        {
            entity.Property(m => m.Name).HasMaxLength(40);
            entity.HasIndex(m => m.Name).IsUnique();
        });

        modelBuilder.Entity<CourseStatus>(entity =>
        {
            entity.Property(s => s.Name).HasMaxLength(40);
            entity.HasIndex(s => s.Name).IsUnique();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable(t => t.HasCheckConstraint("CK_Courses_WorkloadHours", "\"WorkloadHours\" > 0"));
            entity.Property(c => c.FullName).HasMaxLength(150);
            entity.Property(c => c.ShortName).HasMaxLength(40);
            entity.Property(c => c.Summary).HasMaxLength(300);
            entity.Property(c => c.Description).HasMaxLength(2000);
            entity.Property(c => c.TargetAudience).HasMaxLength(300);
            entity.Property(c => c.ImagePath).HasMaxLength(200);
            entity.Property(c => c.ImageAlt).HasMaxLength(200);
            entity.Property(c => c.SearchText).HasMaxLength(250);
            entity.HasIndex(c => c.ShortName).IsUnique();

            entity.HasOne(c => c.Category).WithMany().HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.Modality).WithMany().HasForeignKey(c => c.ModalityId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.Status).WithMany().HasForeignKey(c => c.StatusId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(c => c.Modules).WithOne().HasForeignKey(m => m.CourseId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CourseModule>(entity =>
        {
            entity.Property(m => m.Title).HasMaxLength(150);
            entity.HasIndex(m => new { m.CourseId, m.Position }).IsUnique();
            entity.HasMany(m => m.Lessons).WithOne().HasForeignKey(l => l.ModuleId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.Property(l => l.Description).HasMaxLength(300);
            entity.HasIndex(l => new { l.ModuleId, l.Position }).IsUnique();
        });

        CatalogSeed.Apply(modelBuilder);
    }
}
