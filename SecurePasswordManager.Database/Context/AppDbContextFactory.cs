using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SecurePasswordManager.Database.Context;

namespace SecurePasswordManager.Database;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Указываем временную базу данных только для нужд генерации миграций.
        // На работу реального приложения это не повлияет.
        optionsBuilder.UseSqlite("Data Source=design_time_migrations.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}
