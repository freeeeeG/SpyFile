using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200153B RID: 5435
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Sets the SharedGameObjectList variable to the specified object. Returns Success.")]
	public class SetSharedGameObjectList : Action
	{
		// Token: 0x0600690C RID: 26892 RVA: 0x0012EEC4 File Offset: 0x0012D0C4
		public override TaskStatus OnUpdate()
		{
			this.targetVariable.Value = this.targetValue.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600690D RID: 26893 RVA: 0x0012EEDD File Offset: 0x0012D0DD
		public override void OnReset()
		{
			this.targetValue = null;
			this.targetVariable = null;
		}

		// Token: 0x040054E8 RID: 21736
		[Tooltip("The value to set the SharedGameObjectList to.")]
		public SharedGameObjectList targetValue;

		// Token: 0x040054E9 RID: 21737
		[RequiredField]
		[Tooltip("The SharedGameObjectList to set")]
		public SharedGameObjectList targetVariable;
	}
}
