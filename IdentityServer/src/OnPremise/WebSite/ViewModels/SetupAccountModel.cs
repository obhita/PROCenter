namespace Thinktecture.IdentityServer.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SetupAccountModel : SignInModel
    {
        public SetupAccountModel ()
        {
            
        }

        public SetupAccountModel (SignInModel signInModel)
        {
            UserName = signInModel.UserName;
            Password = signInModel.Password;
            ReturnUrl = signInModel.ReturnUrl;
            EnableSSO = signInModel.EnableSSO;
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.SetupAccountModel))]
        public string NewPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.SetupAccountModel))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string SecurityAnswer { get; set; }

        [Required]
        public string SecurityQuestion { get; set; }
    }
}