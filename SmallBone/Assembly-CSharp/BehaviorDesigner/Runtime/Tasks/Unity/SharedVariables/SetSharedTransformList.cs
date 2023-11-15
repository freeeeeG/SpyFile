using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001543 RID: 5443
	[TaskDescription("Sets the SharedTransformList variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedTransformList : Action
	{
		// Token: 0x06006924 RID: 26916 RVA: 0x0012F07C File Offset: 0x0012D27C
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x0012F095 File Offset: 0x0012D295
		public override void OnReset()
		{
			this.targetValue = null;
			this.targetVariable = null;
		}

		// Token: 0x040054F8 RID: 21752
		[Tooltip("The value to set the SharedTransformList to.")]
		public SharedTransformList targetValue;

		// Token: 0x040054F9 RID: 21753
		[Tooltip("The SharedTransformList to set")]
		[RequiredField]
		public SharedTransformList targetVariable;
	}
}
