namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Agatha.Common;

    #endregion

    public class SaveAssessmentItemRequest : Request
    {
        public Guid Key { get; set; }

        public ItemDto Item { get; set; }
    }
}