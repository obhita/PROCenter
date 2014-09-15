using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources.SignInModel))]
        public string UserName { get; set; }

        public string ReturnUrl { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }
    }
}