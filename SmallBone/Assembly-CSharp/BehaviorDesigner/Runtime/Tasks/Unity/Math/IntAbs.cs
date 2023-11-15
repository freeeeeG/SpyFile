using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A5 RID: 5541
	[TaskCategory("Unity/Math")]
	[TaskDescription("Stores the absolute value of the int.")]
	public class IntAbs : Action
	{
		// Token: 0x06006A79 RID: 27257 RVA: 0x00131FE4 File Offset: 0x001301E4
		public override TaskStatus OnUpdate()
		{
			this.intVariable.Value = Mathf.Abs(this.intVariable.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A7A RID: 27258 RVA: 0x00132002 File Offset: 0x00130202
		public override void OnReset()
		{
			this.intVariable = 0;
		}

		// Token: 0x0400564B RID: 22091
		[Tooltip("The int to return the absolute value of")]
		public SharedInt intVariable;
	}
}
