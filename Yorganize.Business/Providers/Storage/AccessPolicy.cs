using System;

namespace Yorganize.Business.Providers.Storage
{
    [Flags]
    public enum AccessPolicy
    {
        // Summary:
        //     No access granted.
        None = 0,
        //
        // Summary:
        //     Read access granted.
        Read = 1,
        //
        // Summary:
        //     Write access granted.
        Write = 2,
        //
        // Summary:
        //     Delete access granted for blobs.
        Delete = 4,
        //
        // Summary:
        //     List access granted.
        List = 8,
    }
}
