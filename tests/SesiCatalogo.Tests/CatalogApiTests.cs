using System.Net;
using System.Net.Http.Json;
using SesiCatalogo.Web.Catalog;

namespace SesiCatalogo.Tests;

public sealed class CatalogApiTests(CatalogApiFactory factory) : IClassFixture<CatalogApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task<CourseListResponse> GetCourses(string query = "")
    {
        var response = await _client.GetAsync($"/api/courses{query}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<CourseListResponse>())!;
    }

    [Fact]
    public async Task Seed_meets_the_minimum_catalog_requirements()
    {
        var courses = await GetCourses();
        var categories = await _client.GetFromJsonAsync<List<CategoryItem>>("/api/categories");
        var modalities = await _client.GetFromJsonAsync<List<NamedItem>>("/api/modalities");
        var statuses = await _client.GetFromJsonAsync<List<NamedItem>>("/api/statuses");

        Assert.True(courses.Total >= 6);
        Assert.True(categories!.Count >= 3);
        Assert.True(modalities!.Count >= 2);
        Assert.True(statuses!.Count >= 3);

        Assert.True(courses.Items.Select(c => c.Category.Id).Distinct().Count() >= 3);
        Assert.True(courses.Items.Select(c => c.Modality.Id).Distinct().Count() >= 2);
        Assert.True(courses.Items.Select(c => c.Status.Id).Distinct().Count() >= 3);
        Assert.True(courses.Items.Select(c => c.WorkloadHours).Distinct().Count() > 1);
    }

    [Fact]
    public async Task List_total_matches_number_of_items()
    {
        var courses = await GetCourses();

        Assert.Equal(courses.Items.Count, courses.Total);
    }

    [Theory]
    [InlineData("matematica")]
    [InlineData("MATEMÁTICA")]
    [InlineData("  Matemática  ")]
    public async Task Search_ignores_case_accents_and_surrounding_spaces(string term)
    {
        var courses = await GetCourses($"?search={Uri.EscapeDataString(term)}");

        Assert.NotEmpty(courses.Items);
        Assert.All(courses.Items, c => Assert.Contains("Matemática", c.FullName));
    }

    [Fact]
    public async Task Search_also_matches_short_name()
    {
        var courses = await GetCourses("?search=lin-ingtecnico");

        var course = Assert.Single(courses.Items);
        Assert.Equal("LIN-INGTECNICO", course.ShortName);
    }

    [Fact]
    public async Task Filters_restrict_results_by_category_modality_and_status()
    {
        var byCategory = await GetCourses("?categoryId=3");
        var byModality = await GetCourses("?modalityId=2");
        var byStatus = await GetCourses("?statusId=1");

        Assert.NotEmpty(byCategory.Items);
        Assert.All(byCategory.Items, c => Assert.Equal(3, c.Category.Id));
        Assert.NotEmpty(byModality.Items);
        Assert.All(byModality.Items, c => Assert.Equal(2, c.Modality.Id));
        Assert.NotEmpty(byStatus.Items);
        Assert.All(byStatus.Items, c => Assert.Equal(1, c.Status.Id));
    }

    [Fact]
    public async Task Search_and_filters_combine_as_intersection()
    {
        var all = await GetCourses();
        var expected = all.Items
            .Where(c => c.FullName.Contains("Metalurgia") && c.Category.Id == 2 && c.Modality.Id == 2)
            .Select(c => c.Id)
            .ToList();

        var combined = await GetCourses("?search=metalurgia&categoryId=2&modalityId=2");

        Assert.NotEmpty(expected);
        Assert.Equal(expected, combined.Items.Select(c => c.Id).ToList());
    }

    [Fact]
    public async Task No_matches_returns_empty_list_with_zero_total()
    {
        var courses = await GetCourses("?search=curso-inexistente-xyz");

        Assert.Equal(0, courses.Total);
        Assert.Empty(courses.Items);
    }

    [Theory]
    [InlineData("?categoryId=0")]
    [InlineData("?statusId=-1")]
    [InlineData("?modalityId=abc")]
    public async Task Invalid_filter_values_return_bad_request(string query)
    {
        var response = await _client.GetAsync($"/api/courses{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_longer_than_limit_returns_validation_problem()
    {
        var response = await _client.GetAsync($"/api/courses?search={new string('a', CourseFilter.MaxSearchLength + 1)}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Detail_returns_all_course_fields_with_ordered_modules_and_lessons()
    {
        var course = await _client.GetFromJsonAsync<CourseDetail>("/api/courses/1");

        Assert.NotNull(course);
        Assert.False(string.IsNullOrWhiteSpace(course.FullName));
        Assert.False(string.IsNullOrWhiteSpace(course.ShortName));
        Assert.False(string.IsNullOrWhiteSpace(course.Description));
        Assert.False(string.IsNullOrWhiteSpace(course.TargetAudience));
        Assert.False(string.IsNullOrWhiteSpace(course.ImageUrl));
        Assert.True(course.WorkloadHours > 0);
        Assert.Equal(10, course.Modules.Count);
        Assert.Equal(Enumerable.Range(1, course.Modules.Count), course.Modules.Select(m => m.Position));
        Assert.All(course.Modules, m => Assert.Equal(Enumerable.Range(1, m.Lessons.Count), m.Lessons.Select(l => l.Position)));
    }

    [Fact]
    public async Task Unknown_course_returns_not_found_problem()
    {
        var response = await _client.GetAsync("/api/courses/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Api_is_read_only()
    {
        var response = await _client.PostAsJsonAsync("/api/courses", new { fullName = "Novo curso" });

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task Responses_include_security_headers()
    {
        var response = await _client.GetAsync("/api/courses");

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Contains("default-src 'self'", response.Headers.GetValues("Content-Security-Policy").Single());
    }

    [Fact]
    public async Task Front_end_pages_are_served()
    {
        foreach (var page in new[] { "/", "/cursos.html", "/curso.html" })
        {
            var response = await _client.GetAsync(page);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        }
    }
}
