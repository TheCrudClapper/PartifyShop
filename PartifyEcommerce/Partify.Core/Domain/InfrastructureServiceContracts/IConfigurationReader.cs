namespace CSOS.Core.Domain.InfrastructureServiceContracts
{
    /// <summary>
    /// Class for reading env variables from config files
    /// </summary>
    public interface IConfigurationReader
    {
        public string DefaultProductImage { get; set; }
    }
}

