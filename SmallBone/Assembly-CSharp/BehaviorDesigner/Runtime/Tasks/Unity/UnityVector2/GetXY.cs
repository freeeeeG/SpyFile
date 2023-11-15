using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F9 RID: 5369
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the X and Y values of the Vector2.")]
	public class GetXY : Action
	{
		// Token: 0x0600682A RID: 26666 RVA: 0x0012CEF3 File Offset: 0x0012B0F3
		public override TaskStatus OnUpdate()
		{
			this.storeX.Value = this.vector2Variable.Value.x;
			this.storeY.Value = this.vector2Variable.Value.y;
			return TaskStatus.Success;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x0012CF2C File Offset: 0x0012B12C
		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.storeX = (this.storeY = 0f);
		}

		// Token: 0x04005413 RID: 21523
		[Tooltip("The Vector2 to get the values of")]
		public SharedVector2 vector2Variable;

		// Token: 0x04005414 RID: 21524
		[RequiredField]
		[Tooltip("The X value")]
		public SharedFloat storeX;

		// Token: 0x04005415 RID: 21525
		[RequiredField]
		[Tooltip("The Y value")]
		public SharedFloat storeY;
	}
}
