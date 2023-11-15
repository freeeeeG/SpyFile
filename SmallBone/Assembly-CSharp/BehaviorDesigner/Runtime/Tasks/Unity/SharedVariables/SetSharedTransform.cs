using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001542 RID: 5442
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedTransform variable to the specified object. Returns Success.")]
	public class SetSharedTransform : Action
	{
		// Token: 0x06006921 RID: 26913 RVA: 0x0012F038 File Offset: 0x0012D238
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = ((this.targetValue.Value != null) ? this.targetValue.Value : this.transform);
			return TaskStatus.Success;
		}

		// Token: 0x06006922 RID: 26914 RVA: 0x0012F06C File Offset: 0x0012D26C
		public override void OnReset()
		{
			this.targetValue = null;
			this.targetVariable = null;
		}

		// Token: 0x040054F6 RID: 21750
		[Tooltip("The value to set the SharedTransform to. If null the variable will be set to the current Transform")]
		public SharedTransform targetValue;

		// Token: 0x040054F7 RID: 21751
		[RequiredField]
		[Tooltip("The SharedTransform to set")]
		public SharedTransform targetVariable;
	}
}
