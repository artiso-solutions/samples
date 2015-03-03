using System.ComponentModel;

namespace Contracts
{
    /// <summary>
    /// meta data used for MEF to identify exports
    /// </summary>
    public interface IMetaData
    {
        /// <summary>
        /// Gets the version of the MEF export.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DefaultValue(0)]
        int Version { get; }    
    }
}