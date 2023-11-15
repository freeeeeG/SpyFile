using System;

namespace InControl
{
	// Token: 0x020002B7 RID: 695
	[Serializable]
	public class InControlException : Exception
	{
		// Token: 0x06000D64 RID: 3428 RVA: 0x00043386 File Offset: 0x00041786
		public InControlException()
		{
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0004338E File Offset: 0x0004178E
		public InControlException(string message) : base(message)
		{
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00043397 File Offset: 0x00041797
		public InControlException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
