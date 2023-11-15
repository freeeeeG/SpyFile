using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015B4 RID: 5556
	[TaskDescription("Sets an int value")]
	[TaskCategory("Unity/Math")]
	public class SetInt : Action
	{
		// Token: 0x06006A9F RID: 27295 RVA: 0x001325E9 File Offset: 0x001307E9
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.intValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x00132602 File Offset: 0x00130802
		public override void OnReset()
		{
			this.intValue = 0;
			this.storeResult = 0;
		}

		// Token: 0x0400567C RID: 22140
		[Tooltip("The int value to set")]
		public SharedInt intValue;

		// Token: 0x0400567D RID: 22141
		[Tooltip("The variable to store the result")]
		public SharedInt storeResult;
	}
}
