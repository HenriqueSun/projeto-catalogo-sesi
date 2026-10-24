using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SesiCatalogo.Web.Data;

public static class DatabaseInitializer
{
    public static string ResolvePath(IConfiguration configuration, IHostEnvironment environment)
    {
        var configuredPath = configuration["Database:Path"] ?? Path.Combine("data", "catalog.db");
        return Path.GetFullPath(configuredPath, environment.ContentRootPath);
    }

    public static string ConnectionString(string databasePath, SqliteOpenMode mode) =>
        new SqliteConnectionStringBuilder { DataSource = databasePath, Mode = mode }.ToString();

    // The app itself only gets a read-only connection; this write connection exists solely to apply migrations at startup.
    public static void Migrate(string databasePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);

        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlite(ConnectionString(databasePath, SqliteOpenMode.ReadWriteCreate))
            .Options;

        using var db = new CatalogDbContext(options);
        db.Database.Migrate();
    }
}
