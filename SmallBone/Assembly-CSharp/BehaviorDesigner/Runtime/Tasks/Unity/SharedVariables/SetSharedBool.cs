using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001537 RID: 5431
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedBool variable to the specified object. Returns Success.")]
	public class SetSharedBool : Action
	{
		// Token: 0x06006900 RID: 26880 RVA: 0x0012EDB2 File Offset: 0x0012CFB2
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x0012EDCB File Offset: 0x0012CFCB
		public override void OnReset()
		{
			this.targetValue = false;
			this.targetVariable = false;
		}

		// Token: 0x040054DF RID: 21727
		[Tooltip("The value to set the SharedBool to")]
		public SharedBool targetValue;

		// Token: 0x040054E0 RID: 21728
		[RequiredField]
		[Tooltip("The SharedBool to set")]
		public SharedBool targetVariable;
	}
}
