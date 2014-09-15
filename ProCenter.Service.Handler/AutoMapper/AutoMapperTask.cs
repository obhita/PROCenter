// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

namespace ProCenter.Service.Handler.AutoMapper
{
    #region Using Statements

    using System;
    using System.Linq;

    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Primitives;
    using Pillar.Security.AccessControl;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.ReportsModule;
    using ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport;
    using ProCenter.Domain.ReportsModule.PatientScoreRangeReport;
    using ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport;
    using ProCenter.Domain.SecurityModule;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Message;
    using ProCenter.Service.Message.Organization;
    using ProCenter.Service.Message.Report;
    using ProCenter.Service.Message.Security;

    using global::AutoMapper;

    using LookupDto = ProCenter.Service.Message.Common.Lookups.LookupDto;
    using PatientDto = ProCenter.Service.Message.Patient.PatientDto;

    #endregion

    /// <summary>Task that configures automapper.</summary>
    public class AutoMapperTask : IOrderedBootstrapperTask
    {
        #region Public Properties

        /// <summary>
        ///     Gets the order.
        /// </summary>
        /// <value>
        ///     The order.
        /// </value>
        public int Order { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public void Execute ()
        {
            CreateLookupMappings ();
            CreatePatientMappings ();
            CreateAssessmetMappings ();
            CreateMessageMappings ();
            CreateOrganizationMappings ();
            CreateReportMapping ();
        }

        #endregion

        #region Methods

        private void CreateAssessmetMappings ()
        {
            Mapper.CreateMap<Score, ScoreDto> ();
            Mapper.CreateMap<ScoreItem, ScoreDto> ();

            Mapper.CreateMap<ReportModel, ReportModelDto> ();
            Mapper.CreateMap<ReportItem, ReportItemDto> ();
        }

        private void CreateLookupMappings ()
        {
            Mapper.CreateMap<Lookup, LookupDto> ()
                  .ForMember ( dest => dest.Code, opt => opt.MapFrom ( src => src.CodedConcept.Code ) )
                  .ForMember ( dest => dest.Name, opt => opt.MapFrom ( src => src.DisplayName ) );
        }

        private void CreateMessageMappings ()
        {
            Mapper.CreateMap<IMessage, IMessageDto> ()
                  .Include<WorkflowMessage, WorkflowMessageDto> ();
        }

        private void CreateOrganizationMappings ()
        {
            Mapper.CreateMap<Address, AddressDto> ()
                  .ForMember ( dest => dest.PostalCode, opt => opt.MapFrom ( src => src.PostalCode == null ? null : src.PostalCode.Code ) );
            Mapper.CreateMap<Phone, PhoneDto> ();

            Mapper.CreateMap<OrganizationAddress, OrganizationAddressDto> ()
                  .ForMember ( dest => dest.OriginalHash, opt => opt.MapFrom ( src => src.GetHashCode () ) );
            Mapper.CreateMap<OrganizationPhone, OrganizationPhoneDto> ()
                  .ForMember ( dest => dest.OriginalHash, opt => opt.MapFrom ( src => src.GetHashCode () ) );

            Mapper.CreateMap<Organization, OrganizationDto> ();
            Mapper.CreateMap<Organization, OrganizationSummaryDto> ()
                  .ForMember (
                      dest => dest.PrimaryAddress,
                      opt =>
                      opt.MapFrom (
                          src =>
                          ( src.OrganizationAddresses.FirstOrDefault ( a => a.IsPrimary ) ?? src.OrganizationAddresses.FirstOrDefault () ) == null
                              ? null
                              : ( src.OrganizationAddresses.FirstOrDefault ( a => a.IsPrimary ) ?? src.OrganizationAddresses.FirstOrDefault () ).Address ) )
                  .ForMember (
                      dest => dest.PrimaryPhone,
                      opt =>
                      opt.MapFrom (
                          src =>
                          ( src.OrganizationPhones.FirstOrDefault ( p => p.IsPrimary ) ?? src.OrganizationPhones.FirstOrDefault () ) == null
                              ? null
                              : ( src.OrganizationPhones.FirstOrDefault ( p => p.IsPrimary ) ?? src.OrganizationPhones.FirstOrDefault () ).Phone.Number ) );
            Mapper.CreateMap<Staff, StaffDto> ().ForMember ( dest => dest.Email, opt => opt.MapFrom ( src => src.Email.Address ) );
            Mapper.CreateMap<Permission, string> ().ConvertUsing ( src => src.Name ?? string.Empty );
            Mapper.CreateMap<Role, RoleDto> ();
            Mapper.CreateMap<Guid, RoleDto> ().ForMember ( dest => dest.Key, opt => opt.MapFrom ( src => src ) );
            Mapper.CreateMap<SystemAccount, SystemAccountDto> ()
                  .ForMember ( dest => dest.Email, opt => opt.MapFrom ( src => src.Email.Address ) )
                  .ForMember ( dest => dest.Roles, opt => opt.MapFrom ( src => src.RoleKeys ) );

            Mapper.CreateMap<Team, TeamSummaryDto> ();
            Mapper.CreateMap<Email, string> ()
                  .ConvertUsing ( email => email == null ? null : email.Address );
            Mapper.CreateMap<AssessmentReminder, AssessmentReminderDto> ();
        }

        private void CreatePatientMappings ()
        {
            Mapper.CreateMap<Patient, PatientDto> ();
        }

        private void CreateReportMapping ()
        {
            var lookupProvider = IoC.CurrentContainer.Resolve<ILookupProvider> ();
            Mapper.CreateMap<AssessmentScoreOverTimeParameters, AssessmentScoreOverTimeParametersDto>();
            Mapper.CreateMap<AssessmentScoreOverTimeParametersDto, AssessmentScoreOverTimeParameters>()
                  .ForMember(d => d.TimePeriod, s => s.MapFrom(so => so.TimePeriod.Code == null ? null : lookupProvider.Find<ReportTimePeriod>(so.TimePeriod.Code)));

            Mapper.CreateMap<PatientScoreRangeParameters, PatientScoreRangeParametersDto> ()
                  .ForMember ( d => d.Gender, s => s.MapFrom ( so => lookupProvider.Find<Gender> ( so.Gender ) ) );
            Mapper.CreateMap<PatientScoreRangeParametersDto, PatientScoreRangeParameters>()
                  .ForMember(d => d.Gender, s => s.MapFrom(so => so.Gender.Code == null ? null : lookupProvider.Find<Gender>(so.Gender.Code)))
                  .ForMember(d => d.TimePeriod, s => s.MapFrom(so => so.TimePeriod.Code == null ? null : lookupProvider.Find<ReportTimePeriod>(so.TimePeriod.Code)))
                  .ForMember(d => d.ParameterString, s => s.MapFrom(so => so.ToString()))
                  .ForMember(d => d.ScoreType, s => s.MapFrom(so => Mapper.Map<IScoreType> ( so.ScoreType  )));

            Mapper.CreateMap<NotCompletedAssessmentParameters, NotCompletedAssessmentParametersDto> ()
                  .ForMember ( d => d.Gender, s => s.MapFrom ( so => lookupProvider.Find<Gender> ( so.Gender ) ) );
            Mapper.CreateMap<NotCompletedAssessmentParametersDto, NotCompletedAssessmentParameters> ()
                  .ForMember ( d => d.Gender, s => s.MapFrom ( so => so.Gender.Code == null ? null : lookupProvider.Find<Gender> ( so.Gender.Code ) ) )
                  .ForMember ( d => d.TimePeriod, s => s.MapFrom ( so => so.TimePeriod.Code == null ? null : lookupProvider.Find<ReportTimePeriod> ( so.TimePeriod.Code ) ) )
                  .ForMember ( d => d.ParameterString, s => s.MapFrom ( so => so.ToString () ) );

            Mapper.CreateMap<PatientsWithSpecificResponseParameters, PatientsWithSpecificResponseParametersDto> ()
                  .ForMember ( d => d.Gender, s => s.MapFrom ( so => lookupProvider.Find<Gender> ( so.Gender ) ) )
                  .ForMember ( d => d.Responses, s => s.MapFrom ( so => so.QuestionResponses ) );
            Mapper.CreateMap<PatientsWithSpecificResponseParametersDto, PatientsWithSpecificResponseParameters> ()
                  .ForMember ( d => d.Gender, s => s.MapFrom ( so => so.Gender.Code == null ? null : lookupProvider.Find<Gender> ( so.Gender.Code ) ) )
                  .ForMember ( d => d.TimePeriod, s => s.MapFrom ( so => so.TimePeriod.Code == null ? null : lookupProvider.Find<ReportTimePeriod> ( so.TimePeriod.Code ) ) )
                  .ForMember ( d => d.QuestionResponses, s => s.MapFrom ( so => so.Responses ) )
                  .ForMember ( d => d.ParameterString, s => s.MapFrom ( so => so.ToString () ) );

            Mapper.CreateMap<IScoreTypeDto, IScoreType> ().ConvertUsing (
                source =>
                    {
                        var sourceType = source.GetType ();
                        var destinationType = Mapper.GetAllTypeMaps ().First ( typeMap => typeMap.SourceType == sourceType ).DestinationType;
                        var destination = Mapper.Map ( source, sourceType, destinationType );

                        return (IScoreType)destination;
                    } );
            Mapper.CreateMap<ScoreTypeIntDto, ScoreTypeInt> ();
            Mapper.CreateMap<ScoreTypeBooleanDto, ScoreTypeBoolean> ();
            Mapper.CreateMap<IScoreType, IScoreTypeDto> ().ConvertUsing (
                source =>
                    {
                        if ( source == null )
                        {
                            return null;
                        }
                        var sourceType = source.GetType ();
                        var destinationType = Mapper.GetAllTypeMaps ().First ( typeMap => typeMap.SourceType == sourceType ).DestinationType;
                        var destination = Mapper.Map ( source, sourceType, destinationType );

                        return (IScoreTypeDto)destination;
                    } );
            Mapper.CreateMap<ScoreTypeInt, ScoreTypeIntDto> ();
            Mapper.CreateMap<ScoreTypeBoolean, ScoreTypeBooleanDto> ();
            Mapper.CreateMap<ReportTemplate, ReportTemplateDto> ();
        }

        #endregion
    }
}