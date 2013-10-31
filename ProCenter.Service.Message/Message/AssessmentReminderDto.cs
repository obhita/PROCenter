namespace ProCenter.Service.Message.Message
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Common;
    using Domain.MessageModule;

    #endregion

    public class AssessmentReminderDto : KeyedDataTransferObject, IMessageDto
    {
        [ScaffoldColumn(false)]
        public Guid? OrganizationKey { get; set; }

        [Display(Name = "Patient")]
        [Required]
        public Guid? PatientKey { get; set; }

        [ScaffoldColumn(false)]
        public string PatientFirstName { get; set; }

        [ScaffoldColumn(false)]
        public string PatientLastName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Guid? CreatedByStaffKey { get; set; }

        [Display(Name = "Assessment")]
        [Required]
        public Guid? AssessmentDefinitionKey { get; set; }

        [ScaffoldColumn(false)]
        public string AssessmentName { get; set; }

        [ScaffoldColumn(false)]
        public string AssessmentCode { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Assessment Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Start { get; set; }

        [Display(Name = "Reminder")]
        [Required]
        public double ReminderTime { get; set; }

        [Display(Name = " ")]
        [Required]
        public AssessmentReminderUnit ReminderUnit { get; set; }

        [Display(Name = "Email to send to:")]
        public string SendToEmail { get; set; }

        [ScaffoldColumn(false)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public AssessmentReminderStatus Status { get; set; }

        [Display(Name = "Can Self Administer")]
        [ScaffoldColumn(false)]
        public bool ForSelfAdministration { get; set; }
    }
}