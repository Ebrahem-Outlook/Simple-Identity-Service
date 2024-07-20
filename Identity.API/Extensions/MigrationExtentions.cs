using Identity.API.Database;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Extensions;

public static class MigrationExtentions
{
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
}
