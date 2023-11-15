using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002B5 RID: 693
	public class OneAxisInputControl : InputControlBase
	{
		// Token: 0x06000D41 RID: 3393 RVA: 0x00041E5C File Offset: 0x0004025C
		internal void CommitWithSides(InputControl negativeSide, InputControl positiveSide, ulong updateTick, float deltaTime)
		{
			base.LowerDeadZone = Mathf.Max(negativeSide.LowerDeadZone, positiveSide.LowerDeadZone);
			base.UpperDeadZone = Mathf.Min(negativeSide.UpperDeadZone, positiveSide.UpperDeadZone);
			this.Raw = (negativeSide.Raw || positiveSide.Raw);
			float value = Utility.ValueFromSides(negativeSide.RawValue, positiveSide.RawValue);
			base.CommitWithValue(value, updateTick, deltaTime);
		}
	}
}
