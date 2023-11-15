using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
	// Token: 0x020015E5 RID: 5605
	[TaskCategory("Unity/Debug")]
	[TaskDescription("Draws a debug line")]
	public class DrawLine : Action
	{
		// Token: 0x06006B44 RID: 27460 RVA: 0x00133797 File Offset: 0x00131997
		public override TaskStatus OnUpdate()
		{
			Debug.DrawLine(this.start.Value, this.end.Value, this.color.Value, this.duration.Value, this.depthTest.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006B45 RID: 27461 RVA: 0x001337D8 File Offset: 0x001319D8
		public override void OnReset()
		{
			this.start = Vector3.zero;
			this.end = Vector3.zero;
			this.color = Color.white;
			this.duration = 0f;
			this.depthTest = true;
		}

		// Token: 0x040056FE RID: 22270
		[Tooltip("The start position")]
		public SharedVector3 start;

		// Token: 0x040056FF RID: 22271
		[Tooltip("The end position")]
		public SharedVector3 end;

		// Token: 0x04005700 RID: 22272
		[Tooltip("The color")]
		public SharedColor color = Color.white;

		// Token: 0x04005701 RID: 22273
		[Tooltip("Duration the line will be visible for in seconds.\nDefault: 0 means 1 frame.")]
		public SharedFloat duration;

		// Token: 0x04005702 RID: 22274
		[Tooltip("Whether the line should show through world geometry.")]
		public SharedBool depthTest = true;
	}
}
