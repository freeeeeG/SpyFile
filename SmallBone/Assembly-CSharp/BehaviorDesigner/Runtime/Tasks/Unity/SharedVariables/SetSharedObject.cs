using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200153D RID: 5437
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedObject variable to the specified object. Returns Success.")]
	public class SetSharedObject : Action
	{
		// Token: 0x06006912 RID: 26898 RVA: 0x0012EF20 File Offset: 0x0012D120
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x0012EF39 File Offset: 0x0012D139
		public override void OnReset()
		{
			this.targetValue = null;
			this.targetVariable = null;
		}

		// Token: 0x040054EC RID: 21740
		[Tooltip("The value to set the SharedObject to")]
		public SharedObject targetValue;

		// Token: 0x040054ED RID: 21741
		[Tooltip("The SharedTransform to set")]
		[RequiredField]
		public SharedObject targetVariable;
	}
}
