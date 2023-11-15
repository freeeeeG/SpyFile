using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200153E RID: 5438
	[TaskDescription("Sets the SharedObjectList variable to the specified object. Returns Success.")]
	[TaskCategory("Unity/SharedVariable")]
	public class SetSharedObjectList : Action
	{
		// Token: 0x06006915 RID: 26901 RVA: 0x0012EF49 File Offset: 0x0012D149
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x0012EF62 File Offset: 0x0012D162
		public override void OnReset()
		{
			this.targetValue = null;
			this.targetVariable = null;
		}

		// Token: 0x040054EE RID: 21742
		[Tooltip("The value to set the SharedObjectList to.")]
		public SharedObjectList targetValue;

		// Token: 0x040054EF RID: 21743
		[RequiredField]
		[Tooltip("The SharedObjectList to set")]
		public SharedObjectList targetVariable;
	}
}
