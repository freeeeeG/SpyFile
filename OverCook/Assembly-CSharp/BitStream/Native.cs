using System;
using System.Runtime.InteropServices;

namespace BitStream
{
	// Token: 0x02000839 RID: 2105
	internal static class Native
	{
		// Token: 0x04002003 RID: 8195
		internal static readonly uint SizeOfInt = (uint)Marshal.SizeOf(typeof(int));

		// Token: 0x04002004 RID: 8196
		internal static readonly uint SizeOfUInt = (uint)Marshal.SizeOf(typeof(uint));

		// Token: 0x04002005 RID: 8197
		internal static readonly uint SizeOfUShort = (uint)Marshal.SizeOf(typeof(ushort));

		// Token: 0x04002006 RID: 8198
		internal static readonly uint SizeOfByte = (uint)Marshal.SizeOf(typeof(byte));

		// Token: 0x04002007 RID: 8199
		internal static readonly uint SizeOfFloat = (uint)Marshal.SizeOf(typeof(float));

		// Token: 0x04002008 RID: 8200
		internal static readonly uint SizeOfDouble = (uint)Marshal.SizeOf(typeof(double));

		// Token: 0x04002009 RID: 8201
		internal static readonly uint SizeOfGuid = (uint)Marshal.SizeOf(typeof(Guid));

		// Token: 0x0400200A RID: 8202
		internal static readonly uint SizeOfDecimal = (uint)Marshal.SizeOf(typeof(decimal));

		// Token: 0x0400200B RID: 8203
		internal const int BitsPerByte = 8;

		// Token: 0x0400200C RID: 8204
		internal const int BitsPerShort = 16;

		// Token: 0x0400200D RID: 8205
		internal const int BitsPerInt = 32;

		// Token: 0x0400200E RID: 8206
		internal const int BitsPerLong = 64;

		// Token: 0x0400200F RID: 8207
		internal const int BitsPerStringLength = 10;

		// Token: 0x04002010 RID: 8208
		internal const int MaxFloatToIntValue = 2147483583;
	}
}
