using System.ComponentModel;

namespace ClientContracts
{
    public interface IMetaData
    {
        [DefaultValue(0)]
        int Version { get; }    
    }
}