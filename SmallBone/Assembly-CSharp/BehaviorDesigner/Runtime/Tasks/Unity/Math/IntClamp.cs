using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A6 RID: 5542
	[TaskDescription("Clamps the int between two values.")]
	[TaskCategory("Unity/Math")]
	public class IntClamp : Action
	{
		// Token: 0x06006A7C RID: 27260 RVA: 0x00132010 File Offset: 0x00130210
		public override TaskStatus OnUpdate()
		{
			this.intVariable.Value = Mathf.Clamp(this.intVariable.Value, this.minValue.Value, this.maxValue.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A7D RID: 27261 RVA: 0x00132044 File Offset: 0x00130244
		public override void OnReset()
		{
			this.intVariable = 0;
			this.minValue = 0;
			this.maxValue = 0;
		}

		// Token: 0x0400564C RID: 22092
		[Tooltip("The int to clamp")]
		public SharedInt intVariable;

		// Token: 0x0400564D RID: 22093
		[Tooltip("The maximum value of the int")]
		public SharedInt minValue;

		// Token: 0x0400564E RID: 22094
		[Tooltip("The maximum value of the int")]
		public SharedInt maxValue;
	}
}
