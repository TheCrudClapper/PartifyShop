using CSOS.UI.Helpers.Contracts;
using Microsoft.Extensions.Configuration;

namespace CSOS.UI.Helpers
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public string DefaultProductImage { get; set; } = null!;
        public ConfigurationReader(IConfiguration configuration)
        {
            _configuration = configuration;
            GetDefaultProductImage();
        }

        private void GetDefaultProductImage()
        {
            DefaultProductImage = _configuration.GetValue<string>("ProjectVariables:DefaultProductImage")
                                   ?? "/images/no-image.png";
        }
    }
}

