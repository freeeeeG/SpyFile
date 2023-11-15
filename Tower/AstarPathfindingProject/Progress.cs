using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000016 RID: 22
	public struct Progress
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x000087C1 File Offset: 0x000069C1
		public Progress(float progress, string description)
		{
			this.progress = progress;
			this.description = description;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000087D1 File Offset: 0x000069D1
		public Progress MapTo(float min, float max, string prefix = null)
		{
			return new Progress(Mathf.Lerp(min, max, this.progress), prefix + this.description);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000087F1 File Offset: 0x000069F1
		public override string ToString()
		{
			return this.progress.ToString("0.0") + " " + this.description;
		}

		// Token: 0x040000F6 RID: 246
		public readonly float progress;

		// Token: 0x040000F7 RID: 247
		public readonly string description;
	}
}
