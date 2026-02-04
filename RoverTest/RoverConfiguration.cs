using Microsoft.Extensions.Configuration;

namespace RoverTest
{
    public static class RoverConfiguration
    {
        private static IConfigurationRoot Build()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.rover.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        public static RoverConfig Load()
        {
            var configRoot = Build();
            return RoverConfig.FromConfiguration(configRoot);
        }
    }
}