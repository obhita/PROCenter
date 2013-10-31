namespace ProCenter.Mvc.Infrastructure.Permission
{
    #region Using Statements

    using Pillar.Security.AccessControl;

    #endregion

    /// <summary>
    ///     Patient Permissions
    /// </summary>
    public static class PatientPermission
    {
        #region Static Fields

        /// <summary>
        ///     The patient edit permission
        /// </summary>
        public static Permission PatientEditPermission = new Permission {Name = "patientmodule/patientedit"};

        /// <summary>
        ///     The patient view permission
        /// </summary>
        public static Permission PatientViewPermission = new Permission { Name = "patientmodule/patientview" };

        #endregion
    }
}