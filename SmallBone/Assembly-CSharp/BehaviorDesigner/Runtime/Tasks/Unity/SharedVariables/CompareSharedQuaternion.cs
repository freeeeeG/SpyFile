using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x0200152F RID: 5423
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedQuaternion : Conditional
	{
		// Token: 0x060068E8 RID: 26856 RVA: 0x0012EA68 File Offset: 0x0012CC68
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x0012EA98 File Offset: 0x0012CC98
		public override void OnReset()
		{
			this.variable = Quaternion.identity;
			this.compareTo = Quaternion.identity;
		}

		// Token: 0x040054CF RID: 21711
		[Tooltip("The first variable to compare")]
		public SharedQuaternion variable;

		// Token: 0x040054D0 RID: 21712
		[Tooltip("The variable to compare to")]
		public SharedQuaternion compareTo;
	}
}
