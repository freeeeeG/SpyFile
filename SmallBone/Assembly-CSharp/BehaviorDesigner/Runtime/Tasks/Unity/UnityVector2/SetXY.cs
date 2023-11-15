using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x02001501 RID: 5377
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Sets the X and Y values of the Vector2.")]
	public class SetXY : Action
	{
		// Token: 0x0600683F RID: 26687 RVA: 0x0012D234 File Offset: 0x0012B434
		public override TaskStatus OnUpdate()
		{
			Vector2 value = this.vector2Variable.Value;
			if (!this.xValue.IsNone)
			{
				value.x = this.xValue.Value;
			}
			if (!this.yValue.IsNone)
			{
				value.y = this.yValue.Value;
			}
			this.vector2Variable.Value = value;
			return TaskStatus.Success;
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x0012D298 File Offset: 0x0012B498
		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.xValue = 0f;
			this.yValue = 0f;
		}

		// Token: 0x0400542D RID: 21549
		[Tooltip("The Vector2 to set the values of")]
		public SharedVector2 vector2Variable;

		// Token: 0x0400542E RID: 21550
		[Tooltip("The X value. Set to None to have the value ignored")]
		public SharedFloat xValue;

		// Token: 0x0400542F RID: 21551
		[Tooltip("The Y value. Set to None to have the value ignored")]
		public SharedFloat yValue;
	}
}
