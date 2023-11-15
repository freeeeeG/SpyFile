using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015AC RID: 5548
	[TaskCategory("Unity/Math")]
	[TaskDescription("Is the int a positive value?")]
	public class IsIntPositive : Conditional
	{
		// Token: 0x06006A88 RID: 27272 RVA: 0x0013230D File Offset: 0x0013050D
		public override TaskStatus OnUpdate()
		{
			if (this.intVariable.Value <= 0)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x00132320 File Offset: 0x00130520
		public override void OnReset()
		{
			this.intVariable = 0;
		}

		// Token: 0x04005666 RID: 22118
		[Tooltip("The int to check if positive")]
		public SharedInt intVariable;
	}
}
