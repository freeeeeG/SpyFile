using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015B1 RID: 5553
	[TaskDescription("Sets a random int value")]
	[TaskCategory("Unity/Math")]
	public class RandomInt : Action
	{
		// Token: 0x06006A96 RID: 27286 RVA: 0x001324F4 File Offset: 0x001306F4
		public override TaskStatus OnUpdate()
		{
			if (this.inclusive)
			{
				this.storeResult.Value = UnityEngine.Random.Range(this.min.Value, this.max.Value + 1);
			}
			else
			{
				this.storeResult.Value = UnityEngine.Random.Range(this.min.Value, this.max.Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x0013255A File Offset: 0x0013075A
		public override void OnReset()
		{
			this.min.Value = 0;
			this.max.Value = 0;
			this.inclusive = false;
			this.storeResult = 0;
		}

		// Token: 0x04005674 RID: 22132
		[Tooltip("The minimum amount")]
		public SharedInt min;

		// Token: 0x04005675 RID: 22133
		[Tooltip("The maximum amount")]
		public SharedInt max;

		// Token: 0x04005676 RID: 22134
		[Tooltip("Is the maximum value inclusive?")]
		public bool inclusive;

		// Token: 0x04005677 RID: 22135
		[Tooltip("The variable to store the result")]
		public SharedInt storeResult;
	}
}
