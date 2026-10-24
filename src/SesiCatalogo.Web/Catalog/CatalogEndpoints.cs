using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SesiCatalogo.Web.Data;

namespace SesiCatalogo.Web.Catalog;

public static class CatalogEndpoints
{
    public const string RateLimitPolicy = "catalog-api";

    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api").RequireRateLimiting(RateLimitPolicy);

        api.MapGet("/courses", ListCourses);
        api.MapGet("/courses/{id:int}", GetCourse);
        api.MapGet("/categories", ListCategories);
        api.MapGet("/modalities", ListModalities);
        api.MapGet("/statuses", ListStatuses);

        return app;
    }

    private static async Task<Results<Ok<CourseListResponse>, ValidationProblem>> ListCourses(
        [AsParameters] CourseFilter filter, CatalogDbContext db, CancellationToken cancellationToken)
    {
        var errors = filter.Validate();
        if (errors.Count > 0)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var query = db.Courses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = TextNormalizer.Normalize(filter.Search);
            query = query.Where(c => c.SearchText.Contains(term));
        }

        if (filter.CategoryId is int categoryId)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

        if (filter.ModalityId is int modalityId)
        {
            query = query.Where(c => c.ModalityId == modalityId);
        }

        if (filter.StatusId is int statusId)
        {
            query = query.Where(c => c.StatusId == statusId);
        }

        var items = await query
            .OrderBy(c => c.Id)
            .Select(c => new CourseSummary(
                c.Id,
                c.FullName,
                c.ShortName,
                c.Summary,
                new CategoryItem(c.Category.Id, c.Category.Name, c.Category.IconKey),
                new NamedItem(c.Modality.Id, c.Modality.Name),
                new NamedItem(c.Status.Id, c.Status.Name),
                c.WorkloadHours,
                c.ImagePath,
                c.ImageAlt,
                c.AvailableFrom))
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(new CourseListResponse(items.Count, items));
    }

    private static async Task<Results<Ok<CourseDetail>, ProblemHttpResult>> GetCourse(
        int id, CatalogDbContext db, CancellationToken cancellationToken)
    {
        var course = await db.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseDetail(
                c.Id,
                c.FullName,
                c.ShortName,
                c.Summary,
                c.Description,
                new CategoryItem(c.Category.Id, c.Category.Name, c.Category.IconKey),
                new NamedItem(c.Modality.Id, c.Modality.Name),
                new NamedItem(c.Status.Id, c.Status.Name),
                c.WorkloadHours,
                c.TargetAudience,
                c.ImagePath,
                c.ImageAlt,
                c.AvailableFrom,
                c.Modules
                    .OrderBy(m => m.Position)
                    .Select(m => new ModuleItem(
                        m.Position,
                        m.Title,
                        m.Lessons.OrderBy(l => l.Position).Select(l => new LessonItem(l.Position, l.Description)).ToList()))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);

        if (course is null)
        {
            return TypedResults.Problem(title: "Curso não encontrado.", statusCode: StatusCodes.Status404NotFound);
        }

        return TypedResults.Ok(course);
    }

    private static async Task<Ok<List<CategoryItem>>> ListCategories(CatalogDbContext db, CancellationToken cancellationToken) =>
        TypedResults.Ok(await db.Categories.AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CategoryItem(c.Id, c.Name, c.IconKey))
            .ToListAsync(cancellationToken));

    private static async Task<Ok<List<NamedItem>>> ListModalities(CatalogDbContext db, CancellationToken cancellationToken) =>
        TypedResults.Ok(await db.Modalities.AsNoTracking()
            .OrderBy(m => m.Id)
            .Select(m => new NamedItem(m.Id, m.Name))
            .ToListAsync(cancellationToken));

    private static async Task<Ok<List<NamedItem>>> ListStatuses(CatalogDbContext db, CancellationToken cancellationToken) =>
        TypedResults.Ok(await db.CourseStatuses.AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new NamedItem(s.Id, s.Name))
            .ToListAsync(cancellationToken));
}
