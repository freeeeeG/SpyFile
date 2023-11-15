using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015CB RID: 5579
	[TaskDescription("Stores the raw value of the specified axis and stores it in a float.")]
	[TaskCategory("Unity/Input")]
	public class GetAxisRaw : Action
	{
		// Token: 0x06006AF6 RID: 27382 RVA: 0x00133104 File Offset: 0x00131304
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

		// Token: 0x06006AF7 RID: 27383 RVA: 0x0013314A File Offset: 0x0013134A
		public override void OnReset()
		{
			this.axisName = "";
			this.multiplier = 1f;
			this.storeResult = 0f;
		}

		// Token: 0x040056CE RID: 22222
		[Tooltip("The name of the axis")]
		public SharedString axisName;

		// Token: 0x040056CF RID: 22223
		[Tooltip("Axis values are in the range -1 to 1. Use the multiplier to set a larger range")]
		public SharedFloat multiplier;

		// Token: 0x040056D0 RID: 22224
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
