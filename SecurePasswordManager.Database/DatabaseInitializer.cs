using Microsoft.EntityFrameworkCore;
using SecurePasswordManager.Database.Context;

namespace SecurePasswordManager.Database;

public class DatabaseInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();
    }
}
