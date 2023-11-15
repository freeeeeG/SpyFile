using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001541 RID: 5441
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedString variable to the specified object. Returns Success.")]
	public class SetSharedString : Action
	{
		// Token: 0x0600691E RID: 26910 RVA: 0x0012EFFD File Offset: 0x0012D1FD
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x0012F016 File Offset: 0x0012D216
		public override void OnReset()
		{
			this.targetValue = "";
			this.targetVariable = "";
		}

		// Token: 0x040054F4 RID: 21748
		[Tooltip("The value to set the SharedString to")]
		public SharedString targetValue;

		// Token: 0x040054F5 RID: 21749
		[RequiredField]
		[Tooltip("The SharedString to set")]
		public SharedString targetVariable;
	}
}
