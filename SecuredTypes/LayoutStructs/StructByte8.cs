using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TLSecure.SecuredTypes.Structs
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct StructByte8
	{
		[FieldOffset(0)]
		internal long l;
		[FieldOffset(0)]
		internal double d;
	}
}