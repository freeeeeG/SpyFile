using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020000FC RID: 252
public abstract class SerializableDictionaryBase
{
	// Token: 0x02000256 RID: 598
	public abstract class Storage
	{
	}

	// Token: 0x02000257 RID: 599
	protected class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>
	{
		// Token: 0x06000E0C RID: 3596 RVA: 0x0003520D File Offset: 0x0003340D
		public Dictionary()
		{
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00035215 File Offset: 0x00033415
		public Dictionary(IDictionary<TKey, TValue> dict) : base(dict)
		{
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0003521E File Offset: 0x0003341E
		public Dictionary(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
