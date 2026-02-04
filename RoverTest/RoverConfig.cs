using Microsoft.Extensions.Configuration;

namespace RoverTest
{
    public sealed class RoverConfig
    {
        public string ApplicationLocatorBase { get; }
        public string DataLocatorBase { get; }

        private RoverConfig(
            string applicationLocatorBase,
            string dataLocatorBase)
        {
            ApplicationLocatorBase = applicationLocatorBase;
            DataLocatorBase = dataLocatorBase;
        }

        public static RoverConfig FromConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            var section = configuration.GetSection("Rover");

            if (!section.Exists())
                throw new InvalidOperationException("Missing 'Rover' configuration section.");

            var applicationBase = section["ApplicationLocatorBase"];
            var dataBase = section["DataLocatorBase"];

            if (string.IsNullOrWhiteSpace(applicationBase))
                throw new InvalidOperationException(
                    "'Rover:ApplicationLocatorBase' is missing or empty.");

            if (string.IsNullOrWhiteSpace(dataBase))
                throw new InvalidOperationException(
                    "'Rover:DataLocatorBase' is missing or empty.");

            return new RoverConfig(applicationBase, dataBase);
        }
    }
}