using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200153C RID: 5436
	[TaskDescription("Sets the SharedInt variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedInt : Action
	{
		// Token: 0x0600690F RID: 26895 RVA: 0x0012EEED File Offset: 0x0012D0ED
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x0012EF06 File Offset: 0x0012D106
		public override void OnReset()
		{
			this.targetValue = 0;
			this.targetVariable = 0;
		}

		// Token: 0x040054EA RID: 21738
		[Tooltip("The value to set the SharedInt to")]
		public SharedInt targetValue;

		// Token: 0x040054EB RID: 21739
		[Tooltip("The SharedInt to set")]
		[RequiredField]
		public SharedInt targetVariable;
	}
}
