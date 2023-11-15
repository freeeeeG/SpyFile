using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002B3 RID: 691
	public struct InputRange
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x00043148 File Offset: 0x00041548
		private InputRange(float value0, float value1, InputRangeType type)
		{
			this.Value0 = value0;
			this.Value1 = value1;
			this.Type = type;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0004315F File Offset: 0x0004155F
		public InputRange(InputRangeType type)
		{
			this.Value0 = InputRange.TypeToRange[(int)type].Value0;
			this.Value1 = InputRange.TypeToRange[(int)type].Value1;
			this.Type = type;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00043194 File Offset: 0x00041594
		public bool Includes(float value)
		{
			return !this.Excludes(value);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x000431A0 File Offset: 0x000415A0
		public bool Excludes(float value)
		{
			return this.Type == InputRangeType.None || value < Mathf.Min(this.Value0, this.Value1) || value > Mathf.Max(this.Value0, this.Value1);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x000431E0 File Offset: 0x000415E0
		public static float Remap(float value, InputRange sourceRange, InputRange targetRange)
		{
			if (sourceRange.Excludes(value))
			{
				return 0f;
			}
			float t = Mathf.InverseLerp(sourceRange.Value0, sourceRange.Value1, value);
			return Mathf.Lerp(targetRange.Value0, targetRange.Value1, t);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0004322C File Offset: 0x0004162C
		internal static float Remap(float value, InputRangeType sourceRangeType, InputRangeType targetRangeType)
		{
			InputRange sourceRange = InputRange.TypeToRange[(int)sourceRangeType];
			InputRange targetRange = InputRange.TypeToRange[(int)targetRangeType];
			return InputRange.Remap(value, sourceRange, targetRange);
		}

		// Token: 0x04000A8C RID: 2700
		public static readonly InputRange None = new InputRange(0f, 0f, InputRangeType.None);

		// Token: 0x04000A8D RID: 2701
		public static readonly InputRange MinusOneToOne = new InputRange(-1f, 1f, InputRangeType.MinusOneToOne);

		// Token: 0x04000A8E RID: 2702
		public static readonly InputRange ZeroToOne = new InputRange(0f, 1f, InputRangeType.ZeroToOne);

		// Token: 0x04000A8F RID: 2703
		public static readonly InputRange ZeroToMinusOne = new InputRange(0f, -1f, InputRangeType.ZeroToMinusOne);

		// Token: 0x04000A90 RID: 2704
		public static readonly InputRange ZeroToNegativeInfinity = new InputRange(0f, float.NegativeInfinity, InputRangeType.ZeroToNegativeInfinity);

		// Token: 0x04000A91 RID: 2705
		public static readonly InputRange ZeroToPositiveInfinity = new InputRange(0f, float.PositiveInfinity, InputRangeType.ZeroToPositiveInfinity);

		// Token: 0x04000A92 RID: 2706
		public static readonly InputRange Everything = new InputRange(float.NegativeInfinity, float.PositiveInfinity, InputRangeType.Everything);

		// Token: 0x04000A93 RID: 2707
		private static readonly InputRange[] TypeToRange = new InputRange[]
		{
			InputRange.None,
			InputRange.MinusOneToOne,
			InputRange.ZeroToOne,
			InputRange.ZeroToMinusOne,
			InputRange.ZeroToNegativeInfinity,
			InputRange.ZeroToPositiveInfinity,
			InputRange.Everything
		};

		// Token: 0x04000A94 RID: 2708
		public readonly float Value0;

		// Token: 0x04000A95 RID: 2709
		public readonly float Value1;

		// Token: 0x04000A96 RID: 2710
		public readonly InputRangeType Type;
	}
}
