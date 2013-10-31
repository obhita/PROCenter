namespace ProCenter.Service.Handler.AutoMapper
{
    #region Using Statements

    using System.Linq;
    using System;
    using Domain.AssessmentModule;
    using Domain.CommonModule;
    using Domain.CommonModule.Lookups;
    using Domain.MessageModule;
    using Domain.OrganizationModule;
    using Domain.PatientModule;
    using Domain.SecurityModule;
    using Infrastructure.Service;
    using Pillar.Common.Bootstrapper;
    using Pillar.Domain.Primitives;
    using Pillar.Security.AccessControl;
    using ProCenter.Common;
    using Service.Message.Assessment;
    using Service.Message.Common;
    using Service.Message.Common.Lookups;
    using Service.Message.Message;
    using Service.Message.Organization;
    using Service.Message.Patient;
    using Service.Message.Report;
    using Service.Message.Security;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Task that configures automapper
    /// </summary>
    public class AutoMapperTask : IOrderedBootstrapperTask
    {
        public int Order { get; private set; }

        #region Public Methods and Operators

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public void Execute()
        {
            CreateLookupMappings();
            CreatePatientMappings();
            CreateAssessmetMappings();
            CreateMessageMappings();
            CreateOrganizationMappings ();
        }

        private void CreateOrganizationMappings ()
        {
            Mapper.CreateMap<Address, AddressDto>()
                .ForMember ( dest => dest.PostalCode, opt => opt.MapFrom ( src => src.PostalCode == null ? null : src.PostalCode.Code ) );
            Mapper.CreateMap<Phone, PhoneDto> ();

            Mapper.CreateMap<OrganizationAddress, OrganizationAddressDto> ()
                .ForMember ( dest => dest.OriginalHash, opt => opt.MapFrom ( src => src.GetHashCode () ) );
            Mapper.CreateMap<OrganizationPhone, OrganizationPhoneDto>()
                .ForMember(dest => dest.OriginalHash, opt => opt.MapFrom(src => src.GetHashCode()));

            Mapper.CreateMap<Organization, OrganizationDto> ();
            Mapper.CreateMap<Organization, OrganizationSummaryDto> ()
                  .ForMember ( dest => dest.PrimaryAddress,
                               opt =>
                               opt.MapFrom (
                                            src =>
                                            ( src.OrganizationAddresses.FirstOrDefault ( a => a.IsPrimary ) ?? src.OrganizationAddresses.FirstOrDefault () ) == null
                                                ? null
                                                : ( src.OrganizationAddresses.FirstOrDefault ( a => a.IsPrimary ) ?? src.OrganizationAddresses.FirstOrDefault () ).Address ) )
                  .ForMember ( dest => dest.PrimaryPhone,
                               opt =>
                               opt.MapFrom (
                                            src =>
                                            ( src.OrganizationPhones.FirstOrDefault ( p => p.IsPrimary ) ?? src.OrganizationPhones.FirstOrDefault () ) == null
                                                ? null
                                                : ( src.OrganizationPhones.FirstOrDefault ( p => p.IsPrimary ) ?? src.OrganizationPhones.FirstOrDefault () ).Phone.Number ) );
            Mapper.CreateMap<Staff, StaffDto>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
            Mapper.CreateMap<Permission, string>().ConvertUsing(src => src.Name ?? string.Empty);
            Mapper.CreateMap<Role, RoleDto>();
            Mapper.CreateMap<Guid, RoleDto>().ForMember(dest => dest.Key, opt => opt.MapFrom(src => src));
            Mapper.CreateMap<SystemAccount, SystemAccountDto>()
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
                  .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleKeys));

            Mapper.CreateMap<Team, TeamSummaryDto>();
            Mapper.CreateMap<Email,string>()
                .ConvertUsing ( email => email == null ? null : email.Address );
            Mapper.CreateMap<AssessmentReminder, AssessmentReminderDto>();
        }

        private void CreateMessageMappings()
        {
            Mapper.CreateMap<IMessage, IMessageDto>()
                .Include<WorkflowMessage,WorkflowMessageDto>();
        }

        private void CreateAssessmetMappings()
        {
            Mapper.CreateMap<Score, ScoreDto>();
            Mapper.CreateMap<ScoreItem, ScoreDto>();

            Mapper.CreateMap<ReportModel, ReportModelDto>();
            Mapper.CreateMap<ReportItem, ReportItemDto>();
        }

        #endregion

        #region Methods

        private void CreateLookupMappings()
        {
            Mapper.CreateMap<Lookup, LookupDto>()
                  .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CodedConcept.Code))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName));
        }

        private void CreatePatientMappings()
        {
            Mapper.CreateMap<Patient, PatientDto>();
        }


        #endregion
    }
}