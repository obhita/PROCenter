namespace ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport
{
    #region Using Statements

    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    using ProCenter.Common.Report;

    #endregion

    /// <summary>
    /// The NotCompletedAssessmentDataCollection class.
    /// </summary>
    public class NotCompletedAssessmentDataCollection : ArrayList, ITypedList
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties on
        ///     each item used to bind data.
        /// </summary>
        /// <param name="listAccessors">
        ///     An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in
        ///     the collection as bindable. This can be null.
        /// </param>
        /// <returns>
        ///     The <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties on each item
        ///     used to bind data.
        /// </returns>
        public PropertyDescriptorCollection GetItemProperties ( PropertyDescriptor[] listAccessors )
        {
            if (listAccessors != null && listAccessors.Length > 0)
            {
                var listAccessor = listAccessors[listAccessors.Length - 1];
                if (listAccessor.PropertyType == typeof(List<ReportString>))
                {
                    return TypeDescriptor.GetProperties(typeof(ReportString));
                }
            }
            return TypeDescriptor.GetProperties(typeof(NotCompletedAssessmentData));
        }

        /// <summary>
        ///     Returns the name of the list.
        /// </summary>
        /// <param name="listAccessors">
        ///     An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects, for which
        ///     the list name is returned. This can be null.
        /// </param>
        /// <returns>
        ///     The name of the list.
        /// </returns>
        public string GetListName ( PropertyDescriptor[] listAccessors )
        {
            return "NotCompletedAssessmentData";
        }

        #endregion
    }
}