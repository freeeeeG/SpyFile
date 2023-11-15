using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000077 RID: 119
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0001EE74 File Offset: 0x0001D274
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040001EB RID: 491
		public readonly string name;

		// Token: 0x040001EC RID: 492
		public bool dirty;
	}
}
