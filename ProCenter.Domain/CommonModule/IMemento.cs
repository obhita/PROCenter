namespace ProCenter.Domain.CommonModule
{
    using System;

    /// <summary>
    /// Interface for common aggregate memento.
    /// </summary>
    public interface IMemento
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; set; }
    }
}