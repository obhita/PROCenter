namespace ProCenter.Mvc.Infrastructure.Extension
{
    #region Using Statements

    using ProCenter.Service.Message.Patient;

    #endregion

    public static class PatientDtoExtensions
    {
        public static string FullName(this PatientDto patientDto)
        {
            return string.Format("{1}, {0}", patientDto.Name.FirstName, patientDto.Name.LastName);
        }
    }
}