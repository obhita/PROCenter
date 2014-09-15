namespace ProCenter.Service.Message.Assessment
{
    /// <summary>
    /// The Item class.
    /// </summary>
    public class Question : Item
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>
        /// The name of the parent.
        /// </value>
        public string ParentName { get; set; }
    }
}
