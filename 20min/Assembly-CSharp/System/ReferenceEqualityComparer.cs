using System;
using System.Collections.Generic;

namespace System
{
	// Token: 0x0200004F RID: 79
	public class ReferenceEqualityComparer : EqualityComparer<object>
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x00015854 File Offset: 0x00013A54
		public override bool Equals(object x, object y)
		{
			return x == y;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001585A File Offset: 0x00013A5A
		public override int GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}
	}
}
