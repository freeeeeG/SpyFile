using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001539 RID: 5433
	[TaskDescription("Sets the SharedFloat variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedFloat : Action
	{
		// Token: 0x06006906 RID: 26886 RVA: 0x0012EE20 File Offset: 0x0012D020
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x0012EE39 File Offset: 0x0012D039
		public override void OnReset()
		{
			this.targetValue = 0f;
			this.targetVariable = 0f;
		}

		// Token: 0x040054E3 RID: 21731
		[Tooltip("The value to set the SharedFloat to")]
		public SharedFloat targetValue;

		// Token: 0x040054E4 RID: 21732
		[RequiredField]
		[Tooltip("The SharedFloat to set")]
		public SharedFloat targetVariable;
	}
}
