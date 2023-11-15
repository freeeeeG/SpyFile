using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000078 RID: 120
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x06000315 RID: 789 RVA: 0x0001EE83 File Offset: 0x0001D283
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x040001ED RID: 493
		public readonly float min;
	}
}
