// Guids.cs
// MUST match guids.h
using System;

namespace FEI.GhostDocHelper
{
    static class GuidList
    {
        public const string guidGhostDocHelperPkgString = "05a08abf-0900-46fc-b4e6-9c7bdb9a2457";
        public const string guidGhostDocHelperCmdSetString = "3f579f62-9b1c-45d2-b9b0-3ee73ce93dee";
        public const string guidGhostDocPackageProjectCmdSetString = "3f579f62-9b1c-45d2-b9b0-3ee73ce93de1";
        public const string guidGhostDocPackageFolderCmdSetString = "3f579f62-9b1c-45d2-b9b0-3ee73ce93de2";

        public static readonly Guid guidGhostDocHelperCmdSet = new Guid(guidGhostDocHelperCmdSetString);
        public static readonly Guid guidGhostDocPackageProjectCmdSet = new Guid(guidGhostDocPackageProjectCmdSetString);
        public static readonly Guid guidGhostDocPackageFolderCmdSet = new Guid(guidGhostDocPackageFolderCmdSetString);
    };
}