using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002AF RID: 687
	public class InputControlMapping
	{
		// Token: 0x06000D2C RID: 3372 RVA: 0x00042FAC File Offset: 0x000413AC
		public float MapValue(float value)
		{
			if (this.Raw)
			{
				value *= this.Scale;
				value = ((!this.SourceRange.Excludes(value)) ? value : 0f);
			}
			else
			{
				value = Mathf.Clamp(value * this.Scale, -1f, 1f);
				value = InputRange.Remap(value, this.SourceRange, this.TargetRange);
			}
			if (this.Invert)
			{
				value = -value;
			}
			return value;
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x0004302E File Offset: 0x0004142E
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x0004305C File Offset: 0x0004145C
		public string Handle
		{
			get
			{
				return (!string.IsNullOrEmpty(this.handle)) ? this.handle : this.Target.ToString();
			}
			set
			{
				this.handle = value;
			}
		}

		// Token: 0x04000A29 RID: 2601
		public InputControlSource Source;

		// Token: 0x04000A2A RID: 2602
		public InputControlType Target;

		// Token: 0x04000A2B RID: 2603
		public bool Invert;

		// Token: 0x04000A2C RID: 2604
		public float Scale = 1f;

		// Token: 0x04000A2D RID: 2605
		public bool Raw;

		// Token: 0x04000A2E RID: 2606
		public bool IgnoreInitialZeroValue;

		// Token: 0x04000A2F RID: 2607
		public float Sensitivity = 1f;

		// Token: 0x04000A30 RID: 2608
		public float LowerDeadZone;

		// Token: 0x04000A31 RID: 2609
		public float UpperDeadZone = 1f;

		// Token: 0x04000A32 RID: 2610
		public InputRange SourceRange = InputRange.MinusOneToOne;

		// Token: 0x04000A33 RID: 2611
		public InputRange TargetRange = InputRange.MinusOneToOne;

		// Token: 0x04000A34 RID: 2612
		private string handle;
	}
}
