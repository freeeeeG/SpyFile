using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A0 RID: 5536
	[TaskCategory("Unity/Math")]
	[TaskDescription("Clamps the float between two values.")]
	public class FloatClamp : Action
	{
		// Token: 0x06006A70 RID: 27248 RVA: 0x00131CEA File Offset: 0x0012FEEA
		public override TaskStatus OnUpdate()
		{
			this.floatVariable.Value = Mathf.Clamp(this.floatVariable.Value, this.minValue.Value, this.maxValue.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A71 RID: 27249 RVA: 0x00131D1E File Offset: 0x0012FF1E
		public override void OnReset()
		{
			this.floatVariable = 0f;
			this.minValue = 0f;
			this.maxValue = 0f;
		}

		// Token: 0x04005632 RID: 22066
		[Tooltip("The float to clamp")]
		public SharedFloat floatVariable;

		// Token: 0x04005633 RID: 22067
		[Tooltip("The maximum value of the float")]
		public SharedFloat minValue;

		// Token: 0x04005634 RID: 22068
		[Tooltip("The maximum value of the float")]
		public SharedFloat maxValue;
	}
}
