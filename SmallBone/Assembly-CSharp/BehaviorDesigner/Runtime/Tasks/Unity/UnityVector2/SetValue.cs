using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x02001500 RID: 5376
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Sets the value of the Vector2.")]
	public class SetValue : Action
	{
		// Token: 0x0600683C RID: 26684 RVA: 0x0012D1F8 File Offset: 0x0012B3F8
		public override TaskStatus OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Value.Value;
			return TaskStatus.Success;
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x0012D211 File Offset: 0x0012B411
		public override void OnReset()
		{
			this.vector2Value = Vector2.zero;
			this.vector2Variable = Vector2.zero;
		}

		// Token: 0x0400542B RID: 21547
		[Tooltip("The Vector2 to get the values of")]
		public SharedVector2 vector2Value;

		// Token: 0x0400542C RID: 21548
		[Tooltip("The Vector2 to set the values of")]
		public SharedVector2 vector2Variable;
	}
}
