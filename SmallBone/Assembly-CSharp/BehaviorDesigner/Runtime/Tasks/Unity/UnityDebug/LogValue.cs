using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
	// Token: 0x020015E8 RID: 5608
	[TaskDescription("Log a variable value.")]
	[TaskCategory("Unity/Debug")]
	public class LogValue : Action
	{
		// Token: 0x06006B4F RID: 27471 RVA: 0x00133A55 File Offset: 0x00131C55
		public override TaskStatus OnUpdate()
		{
			Debug.Log(this.variable.Value.value.GetValue());
			return TaskStatus.Success;
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x00133A72 File Offset: 0x00131C72
		public override void OnReset()
		{
			this.variable = null;
		}

		// Token: 0x0400570C RID: 22284
		[Tooltip("The variable to output")]
		public SharedGenericVariable variable;
	}
}
