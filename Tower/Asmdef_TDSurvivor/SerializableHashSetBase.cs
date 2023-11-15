using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000101 RID: 257
public abstract class SerializableHashSetBase
{
	// Token: 0x02000259 RID: 601
	public abstract class Storage
	{
	}

	// Token: 0x0200025A RID: 602
	protected class HashSet<TValue> : System.Collections.Generic.HashSet<TValue>
	{
		// Token: 0x06000E11 RID: 3601 RVA: 0x00035238 File Offset: 0x00033438
		public HashSet()
		{
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00035240 File Offset: 0x00033440
		public HashSet(ISet<TValue> set) : base(set)
		{
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00035249 File Offset: 0x00033449
		public HashSet(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
