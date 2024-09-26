using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TLSecure.SecuredTypes.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct StructByte4
    {
        [FieldOffset(0)]
        internal int i;
        [FieldOffset(0)]
        internal float f;
    }
}