using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015B0 RID: 5552
	[TaskDescription("Sets a random float value")]
	[TaskCategory("Unity/Math")]
	public class RandomFloat : Action
	{
		// Token: 0x06006A93 RID: 27283 RVA: 0x00132450 File Offset: 0x00130650
		public override TaskStatus OnUpdate()
		{
			if (this.inclusive)
			{
				this.storeResult.Value = UnityEngine.Random.Range(this.min.Value, this.max.Value);
			}
			else
			{
				this.storeResult.Value = UnityEngine.Random.Range(this.min.Value, this.max.Value - 1E-05f);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A94 RID: 27284 RVA: 0x001324BA File Offset: 0x001306BA
		public override void OnReset()
		{
			this.min.Value = 0f;
			this.max.Value = 0f;
			this.inclusive = false;
			this.storeResult = 0f;
		}

		// Token: 0x04005670 RID: 22128
		[Tooltip("The minimum amount")]
		public SharedFloat min;

		// Token: 0x04005671 RID: 22129
		[Tooltip("The maximum amount")]
		public SharedFloat max;

		// Token: 0x04005672 RID: 22130
		[Tooltip("Is the maximum value inclusive?")]
		public bool inclusive;

		// Token: 0x04005673 RID: 22131
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
