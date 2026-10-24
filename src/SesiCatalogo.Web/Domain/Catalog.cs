namespace SesiCatalogo.Web.Domain;

public sealed class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string IconKey { get; set; }
}

public sealed class Modality
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public sealed class CourseStatus
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public sealed class Course
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string ShortName { get; set; }
    public required string Summary { get; set; }
    public required string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int ModalityId { get; set; }
    public Modality Modality { get; set; } = null!;
    public int StatusId { get; set; }
    public CourseStatus Status { get; set; } = null!;
    public int WorkloadHours { get; set; }
    public required string TargetAudience { get; set; }
    public required string ImagePath { get; set; }
    public required string ImageAlt { get; set; }
    public DateOnly? AvailableFrom { get; set; }

    // Accent- and case-free copy of FullName + ShortName, so search works the same on any database provider.
    public required string SearchText { get; set; }

    public List<CourseModule> Modules { get; set; } = [];
}

public sealed class CourseModule
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int Position { get; set; }
    public required string Title { get; set; }
    public List<Lesson> Lessons { get; set; } = [];
}

public sealed class Lesson
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public int Position { get; set; }
    public required string Description { get; set; }
}
