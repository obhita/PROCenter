#region License Header
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
#endregion
namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Pillar.Common.DataTransferObject;

    #endregion

    [MetadataType(typeof (KeyedDataTransferObjectMetadata))]
    public class KeyedDataTransferObject : KeyedDataTransferObject<Guid>, IKeyedDataTransferObject
    {
        private IList<DataErrorInfo> _dataErrorInfoCollection = new List<DataErrorInfo>();

        /// <summary>
        ///     Gets the data error information collection.
        /// </summary>
        public IEnumerable<DataErrorInfo> DataErrorInfoCollection
        {
            get { return _dataErrorInfoCollection; }
            internal set
            {
                if (value != null)
                {
                    _dataErrorInfoCollection = new List<DataErrorInfo>(value);
                }
            }
        }

        /// <summary>
        ///     Adds the data error information.
        /// </summary>
        /// <param name="dataErrorInfo">The data error information.</param>
        public void AddDataErrorInfo(DataErrorInfo dataErrorInfo)
        {
            if (dataErrorInfo.DataErrorInfoType != DataErrorInfoType.ObjectLevel)
            {
                foreach (var propertyName in dataErrorInfo.Properties)
                {
                    ValidatePropertyExists(propertyName);
                }
            }

            _dataErrorInfoCollection.Add(dataErrorInfo);
        }

        /// <summary>
        ///     Clears all data error information.
        /// </summary>
        public void ClearAllDataErrorInfo()
        {
            RemoveDataErrorInfo(_dataErrorInfoCollection);
        }

        /// <summary>
        ///     Removes the data error information.
        /// </summary>
        /// <param name="propertyName">Name of the property which has erroneous data.</param>
        public void RemoveDataErrorInfo(string propertyName)
        {
            // what if the property is null, empty, or doesn't exist?
            if (!string.IsNullOrEmpty(propertyName))
            {
                ValidatePropertyExists(propertyName);
            }

            var dataErrorInfoList = GetDataErrorInfos(propertyName);

            RemoveDataErrorInfo(dataErrorInfoList);
        }

        #region Methods

        private IEnumerable<DataErrorInfo> GetDataErrorInfos(string propertyName)
        {
            IList<DataErrorInfo> list;

            if (string.IsNullOrEmpty(propertyName))
            {
                list = _dataErrorInfoCollection.Where(
                    p => p.DataErrorInfoType == DataErrorInfoType.ObjectLevel).ToList();
            }
            else
            {
                list = _dataErrorInfoCollection.Where(
                    p => p.Properties != null && p.Properties.Contains(propertyName)
                    ).ToList();
            }

            return list;
        }

        private void RemoveDataErrorInfo(IEnumerable<DataErrorInfo> dataErrorInfoList)
        {
            if (dataErrorInfoList != null)
            {
                ISet<string> propertyNames = new HashSet<string>();
                IList<DataErrorInfo> iterationList = new List<DataErrorInfo>(dataErrorInfoList);
                foreach (var dataErrorInfo in iterationList)
                {
                    _dataErrorInfoCollection.Remove(dataErrorInfo);

                    if (dataErrorInfo.DataErrorInfoType == DataErrorInfoType.ObjectLevel)
                    {
                        propertyNames.Add(string.Empty);
                    }
                    else
                    {
                        foreach (var name in dataErrorInfo.Properties)
                        {
                            propertyNames.Add(name);
                        }
                    }
                }
            }
        }

        private void ValidatePropertyExists(string propertyName)
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop == null)
            {
                throw new ArgumentException("Property not found on type: " + GetType(), propertyName);
            }
        }

        #endregion

        /// <summary>
        ///     Removes the data error info.
        /// </summary>
        /// <param name="dataErrorInfo">The data error info.</param>
        public void RemoveDataErrorInfo(DataErrorInfo dataErrorInfo)
        {
            ISet<string> propertyNames = new HashSet<string>();
            _dataErrorInfoCollection.Remove(dataErrorInfo);
            if (dataErrorInfo.DataErrorInfoType == DataErrorInfoType.ObjectLevel)
            {
                propertyNames.Add(string.Empty);
            }
            else
            {
                foreach (var name in dataErrorInfo.Properties)
                {
                    propertyNames.Add(name);
                }
            }
        }

        public class KeyedDataTransferObjectMetadata
        {
            [HiddenInput(DisplayValue = false)]
            [Display(AutoGenerateField = false, Name = " ")]
            public long Key { get; set; }

            [ScaffoldColumn(false)]
            public IEnumerable<DataErrorInfo> DataErrorInfoCollection { get; set; }
        }
    }
}