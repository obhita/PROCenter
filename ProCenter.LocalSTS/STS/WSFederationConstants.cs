#region Licence Header
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
namespace ProCenter.LocalSTS.STS
{

    /// <summary>
    /// This is copied from System.Identity.WSFederationContants. It's poublic in 3.5 but become internal in 4.5.
    /// </summary>
    public static class WSFederationConstants
    {
        public const string Namespace = "http://docs.oasis-open.org/wsfed/federation/200706";

        public static class Actions
        {
            public const string Attribute = "wattr1.0";
            public const string Pseudonym = "wpseudo1.0";
            public const string SignIn = "wsignin1.0";
            public const string SignOut = "wsignout1.0";
            public const string SignOutCleanup = "wsignoutcleanup1.0";
        }

        public static class FaultCodeValues
        {
            public const string AlreadySignedIn = "AlreadySignedIn";
            public const string BadRequest = "BadRequest";
            public const string IssuerNameNotSupported = "IssuerNameNotSupported";
            public const string NeedFresherCredentials = "NeedFresherCredentials";
            public const string NoMatchInScope = "NoMatchInScope";
            public const string NoPseudonymInScope = "NoPseudonymInScope";
            public const string NotSignedIn = "NotSignedIn";
            public const string RstParameterNotAccepted = "RstParameterNotAccepted";
            public const string SpecificPolicy = "SpecificPolicy";
            public const string UnsupportedClaimsDialect = "UnsupportedClaimsDialect";
            public const string UnsupportedEncoding = "UnsupportedEncoding";
        }

        public static class Parameters
        {
            public const string Action = "wa";
            public const string Attribute = "wattr";
            public const string AttributePtr = "wattrptr";
            public const string AuthenticationType = "wauth";
            public const string Context = "wctx";
            public const string CurrentTime = "wct";
            public const string Encoding = "wencoding";
            public const string Federation = "wfed";
            public const string Freshness = "wfresh";
            public const string HomeRealm = "whr";
            public const string Policy = "wp";
            public const string Pseudonym = "wpseudo";
            public const string PseudonymPtr = "wpseudoptr";
            public const string Realm = "wtrealm";
            public const string Reply = "wreply";
            public const string Request = "wreq";
            public const string RequestPtr = "wreqptr";
            public const string Resource = "wres";
            public const string Result = "wresult";
            public const string ResultPtr = "wresultptr";
        }
    }
}