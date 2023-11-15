using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
	// Token: 0x020015E6 RID: 5606
	[TaskCategory("Unity/Debug")]
	[TaskDescription("Draws a debug ray")]
	public class DrawRay : Action
	{
		// Token: 0x06006B47 RID: 27463 RVA: 0x00133855 File Offset: 0x00131A55
		public override TaskStatus OnUpdate()
		{
			Debug.DrawRay(this.start.Value, this.direction.Value, this.color.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x0013387E File Offset: 0x00131A7E
		public override void OnReset()
		{
			this.start = Vector3.zero;
			this.direction = Vector3.zero;
			this.color = Color.white;
		}

		// Token: 0x04005703 RID: 22275
		[Tooltip("The position")]
		public SharedVector3 start;

		// Token: 0x04005704 RID: 22276
		[Tooltip("The direction")]
		public SharedVector3 direction;

		// Token: 0x04005705 RID: 22277
		[Tooltip("The color")]
		public SharedColor color = Color.white;
	}
}
