using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000079 RID: 121
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0001EE92 File Offset: 0x0001D292
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x040001EE RID: 494
		public readonly string method;
	}
}
