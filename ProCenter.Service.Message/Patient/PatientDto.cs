namespace ProCenter.Service.Message.Patient
{
    #region

    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Common.Lookups;
    using Primitive;
    using Security;

    #endregion

    public class PatientDto : KeyedDataTransferObject
    {
        [Display(Name = " ")]
        [Required]
        public PersonName Name { get; set; }

        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public LookupDto Gender { get; set; }

        public LookupDto Ethnicity { get; set; }

        public LookupDto Religion { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Unique Identifier")]
        [Editable(false)]
        public string UniqueIdentifier { get; set; }


        [ScaffoldColumn(false)]
        public SystemAccountDto SystemAccount { get; set; }

        [ScaffoldColumn(false)]
        public Guid OrganizationKey { get; set; }

        [ScaffoldColumn(false)]
        public bool HasAccount
        {
            get { return SystemAccount != null; }
        }
    }
}