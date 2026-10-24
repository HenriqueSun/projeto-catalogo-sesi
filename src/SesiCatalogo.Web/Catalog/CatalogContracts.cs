namespace SesiCatalogo.Web.Catalog;

public sealed record NamedItem(int Id, string Name);

public sealed record CategoryItem(int Id, string Name, string IconKey);

public sealed record CourseSummary(
    int Id,
    string FullName,
    string ShortName,
    string Summary,
    CategoryItem Category,
    NamedItem Modality,
    NamedItem Status,
    int WorkloadHours,
    string ImageUrl,
    string ImageAlt,
    DateOnly? AvailableFrom);

public sealed record CourseListResponse(int Total, IReadOnlyList<CourseSummary> Items);

public sealed record LessonItem(int Position, string Description);

public sealed record ModuleItem(int Position, string Title, IReadOnlyList<LessonItem> Lessons);

public sealed record CourseDetail(
    int Id,
    string FullName,
    string ShortName,
    string Summary,
    string Description,
    CategoryItem Category,
    NamedItem Modality,
    NamedItem Status,
    int WorkloadHours,
    string TargetAudience,
    string ImageUrl,
    string ImageAlt,
    DateOnly? AvailableFrom,
    IReadOnlyList<ModuleItem> Modules);

public sealed record CourseFilter(string? Search, int? CategoryId, int? ModalityId, int? StatusId)
{
    public const int MaxSearchLength = 100;

    public Dictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>();

        if (Search is { Length: > MaxSearchLength })
        {
            errors["search"] = [$"A pesquisa deve ter no máximo {MaxSearchLength} caracteres."];
        }

        AddIdError(errors, "categoryId", CategoryId);
        AddIdError(errors, "modalityId", ModalityId);
        AddIdError(errors, "statusId", StatusId);
        return errors;
    }

    private static void AddIdError(Dictionary<string, string[]> errors, string key, int? value)
    {
        if (value is < 1)
        {
            errors[key] = ["O identificador deve ser um número inteiro positivo."];
        }
    }
}
