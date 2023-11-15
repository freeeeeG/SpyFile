using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015B3 RID: 5555
	[TaskIcon("Assets/Behavior Designer/Icon/SetFloat.png")]
	[TaskCategory("Unity/Math")]
	[TaskDescription("Sets a float value")]
	public class SetFloat : Action
	{
		// Token: 0x06006A9C RID: 27292 RVA: 0x001325AE File Offset: 0x001307AE
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.floatValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x001325C7 File Offset: 0x001307C7
		public override void OnReset()
		{
			this.floatValue = 0f;
			this.storeResult = 0f;
		}

		// Token: 0x0400567A RID: 22138
		[Tooltip("The float value to set")]
		public SharedFloat floatValue;

		// Token: 0x0400567B RID: 22139
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;
	}
}
