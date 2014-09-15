using System.Collections.Generic;

namespace ProCenter.Service.Message.Assessment
{
    /// <summary>
    /// The Group class.
    /// </summary>
    public class Group : Item
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<IItem> Items { get; set; }
    }
}
