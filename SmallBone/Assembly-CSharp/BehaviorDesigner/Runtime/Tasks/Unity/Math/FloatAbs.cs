using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x0200159F RID: 5535
	[TaskCategory("Unity/Math")]
	[TaskDescription("Stores the absolute value of the float.")]
	public class FloatAbs : Action
	{
		// Token: 0x06006A6D RID: 27245 RVA: 0x00131CBA File Offset: 0x0012FEBA
		public override TaskStatus OnUpdate()
		{
			this.floatVariable.Value = Mathf.Abs(this.floatVariable.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A6E RID: 27246 RVA: 0x00131CD8 File Offset: 0x0012FED8
		public override void OnReset()
		{
			this.floatVariable = 0f;
		}

		// Token: 0x04005631 RID: 22065
		[Tooltip("The float to return the absolute value of")]
		public SharedFloat floatVariable;
	}
}
