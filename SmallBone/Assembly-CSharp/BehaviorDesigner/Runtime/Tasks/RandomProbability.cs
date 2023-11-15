using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014C8 RID: 5320
	[TaskDescription("The random probability task will return success when the random probability is below the succeed probability. It will otherwise return failure.")]
	public class RandomProbability : Conditional
	{
		// Token: 0x06006773 RID: 26483 RVA: 0x0012B524 File Offset: 0x00129724
		public override void OnAwake()
		{
			if (this.useSeed.Value)
			{
				UnityEngine.Random.InitState(this.seed.Value);
			}
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x0012B543 File Offset: 0x00129743
		public override TaskStatus OnUpdate()
		{
			if (UnityEngine.Random.value < this.successProbability.Value)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x06006775 RID: 26485 RVA: 0x0012B55A File Offset: 0x0012975A
		public override void OnReset()
		{
			this.successProbability = 0.5f;
			this.seed = 0;
			this.useSeed = false;
		}

		// Token: 0x0400537A RID: 21370
		[Tooltip("The chance that the task will return success")]
		public SharedFloat successProbability = 0.5f;

		// Token: 0x0400537B RID: 21371
		[Tooltip("Seed the random number generator to make things easier to debug")]
		public SharedInt seed;

		// Token: 0x0400537C RID: 21372
		[Tooltip("Do we want to use the seed?")]
		public SharedBool useSeed;
	}
}
