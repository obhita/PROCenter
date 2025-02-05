﻿#region License Header

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

namespace ProCenter.ReadSideService
{
    #region Using Statements

    using System.Configuration;

    using Pillar.Common.Utility;

    #endregion

    /// <summary>The connection string configuration provider class.</summary>
    public class ConnectionStringConfigurationProvider : IConnectionStringConfigurationProvider
    {
        #region Fields

        private readonly ConnectionStringSettingsCollection _connectionStrings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringConfigurationProvider"/> class.
        /// </summary>
        /// <param name="connectionStrings">The connection strings.</param>
        public ConnectionStringConfigurationProvider ( ConnectionStringSettingsCollection connectionStrings )
        {
            _connectionStrings = connectionStrings;
            Check.IsNotNull ( connectionStrings, "connectionStrings is required." );
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the connection string.</summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns>A connection string.</returns>
        public string GetConnectionString ( string connectionStringName )
        {
            return _connectionStrings[connectionStringName].ConnectionString;
        }

        #endregion
    }
}