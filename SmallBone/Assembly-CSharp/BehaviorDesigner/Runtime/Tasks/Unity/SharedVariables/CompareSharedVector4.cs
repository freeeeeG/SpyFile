using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
	// Token: 0x02001536 RID: 5430
	[TaskCategory("Unity/SharedVariable")]
	[TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
	public class CompareSharedVector4 : Conditional
	{
		// Token: 0x060068FD RID: 26877 RVA: 0x0012ED60 File Offset: 0x0012CF60
		public override TaskStatus OnUpdate()
		{
			if (!this.variable.Value.Equals(this.compareTo.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x0012ED90 File Offset: 0x0012CF90
		public override void OnReset()
		{
			this.variable = Vector4.zero;
			this.compareTo = Vector4.zero;
		}

		// Token: 0x040054DD RID: 21725
		[Tooltip("The first variable to compare")]
		public SharedVector4 variable;

		// Token: 0x040054DE RID: 21726
		[Tooltip("The variable to compare to")]
		public SharedVector4 compareTo;
	}
}
