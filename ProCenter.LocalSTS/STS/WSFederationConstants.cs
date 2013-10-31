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