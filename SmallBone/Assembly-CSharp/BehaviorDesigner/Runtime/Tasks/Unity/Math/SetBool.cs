using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015B2 RID: 5554
	[TaskCategory("Unity/Math")]
	[TaskDescription("Sets a bool value")]
	[TaskIcon("Assets/Behavior Designer/Icon/SetBool.png")]
	public class SetBool : Action
	{
		// Token: 0x06006A99 RID: 27289 RVA: 0x00132587 File Offset: 0x00130787
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.boolValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x001325A0 File Offset: 0x001307A0
		public override void OnReset()
		{
			this.boolValue = false;
		}

		// Token: 0x04005678 RID: 22136
		[Tooltip("The bool value to set")]
		public SharedBool boolValue;

		// Token: 0x04005679 RID: 22137
		[Tooltip("The variable to store the result")]
		public SharedBool storeResult;
	}
}
