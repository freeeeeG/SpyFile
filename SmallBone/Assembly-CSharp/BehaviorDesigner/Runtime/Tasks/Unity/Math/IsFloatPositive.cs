using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015AB RID: 5547
	[TaskDescription("Is the float a positive value?")]
	[TaskCategory("Unity/Math")]
	public class IsFloatPositive : Conditional
	{
		// Token: 0x06006A85 RID: 27269 RVA: 0x001322E4 File Offset: 0x001304E4
		public override TaskStatus OnUpdate()
		{
			if (this.floatVariable.Value <= 0f)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x001322FB File Offset: 0x001304FB
		public override void OnReset()
		{
			this.floatVariable = 0f;
		}

		// Token: 0x04005665 RID: 22117
		[Tooltip("The float to check if positive")]
		public SharedFloat floatVariable;
	}
}
