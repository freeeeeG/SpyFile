using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015CA RID: 5578
	[TaskCategory("Unity/Input")]
	[TaskDescription("Stores the value of the specified axis and stores it in a float.")]
	public class GetAxis : Action
	{
		// Token: 0x06006AF3 RID: 27379 RVA: 0x0013308C File Offset: 0x0013128C
		public override TaskStatus OnUpdate()
		{
			float num = Input.GetAxis(this.axisName.Value);
			if (!this.multiplier.IsNone)
			{
				num *= this.multiplier.Value;
			}
			this.storeResult.Value = num;
			return TaskStatus.Success;
		}

		// Token: 0x06006AF4 RID: 27380 RVA: 0x001330D2 File Offset: 0x001312D2
		public override void OnReset()
		{
			this.axisName = "";
			this.multiplier = 1f;
			this.storeResult = 0f;
		}

		// Token: 0x040056CB RID: 22219
		[Tooltip("The name of the axis")]
		public SharedString axisName;

		// Token: 0x040056CC RID: 22220
		[Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range")]
		public SharedFloat multiplier;

		// Token: 0x040056CD RID: 22221
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
