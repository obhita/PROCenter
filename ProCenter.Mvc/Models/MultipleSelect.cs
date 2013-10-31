namespace ProCenter.Mvc.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class MultipleSelect
    {
        public string[] SelectedKeys { get; set; }

        public IEnumerable<SelectListItem> Items { get; set; }
    }
}