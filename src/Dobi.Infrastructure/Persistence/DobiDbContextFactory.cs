using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence
{
    public class DobiDbContextFactory : IDesignTimeDbContextFactory<DobiDbContext>
    {
        public DobiDbContext CreateDbContext(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var apiProjectPath = Directory.Exists(Path.Combine(currentDirectory, "src", "Dobi.Api"))
                ? Path.Combine(currentDirectory, "src", "Dobi.Api")
                : currentDirectory;

            var appSettingsPath = Path.Combine(apiProjectPath, "appsettings.json");
            var appSettingsDevelopmentPath = Path.Combine(apiProjectPath, "appsettings.Development.json");

            if (!File.Exists(appSettingsPath) && !File.Exists(appSettingsDevelopmentPath))
            {
                throw new InvalidOperationException(
                    $"Could not find appsettings.json or appsettings.Development.json. Current directory: {currentDirectory}. Resolved API path: {apiProjectPath}");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string 'DefaultConnection' was not found. Current directory: {currentDirectory}. Resolved API path: {apiProjectPath}");
            }

            var optionsBuilder = new DbContextOptionsBuilder<DobiDbContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new DobiDbContext(optionsBuilder.Options);
        }
    }
}
