using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200153A RID: 5434
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedGameObject variable to the specified object. Returns Success.")]
	public class SetSharedGameObject : Action
	{
		// Token: 0x06006909 RID: 26889 RVA: 0x0012EE5C File Offset: 0x0012D05C
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = ((this.targetValue.Value != null || this.valueCanBeNull.Value) ? this.targetValue.Value : this.gameObject);
			return TaskStatus.Success;
		}

		// Token: 0x0600690A RID: 26890 RVA: 0x0012EEA8 File Offset: 0x0012D0A8
		public override void OnReset()
		{
			this.valueCanBeNull = false;
			this.targetValue = null;
			this.targetVariable = null;
		}

		// Token: 0x040054E5 RID: 21733
		[Tooltip("The value to set the SharedGameObject to. If null the variable will be set to the current GameObject")]
		public SharedGameObject targetValue;

		// Token: 0x040054E6 RID: 21734
		[Tooltip("The SharedGameObject to set")]
		[RequiredField]
		public SharedGameObject targetVariable;

		// Token: 0x040054E7 RID: 21735
		[Tooltip("Can the target value be null?")]
		public SharedBool valueCanBeNull;
	}
}
