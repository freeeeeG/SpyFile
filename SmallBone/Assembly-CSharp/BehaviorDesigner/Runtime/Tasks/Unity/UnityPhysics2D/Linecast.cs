using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D
{
	// Token: 0x0200157C RID: 5500
	[TaskDescription("Returns success if there is any collider intersecting the line between start and end")]
	[TaskCategory("Unity/Physics2D")]
	public class Linecast : Action
	{
		// Token: 0x060069EA RID: 27114 RVA: 0x001308F2 File Offset: 0x0012EAF2
		public override TaskStatus OnUpdate()
		{
			if (!Physics2D.Linecast(this.startPosition.Value, this.endPosition.Value, this.layerMask))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x00130924 File Offset: 0x0012EB24
		public override void OnReset()
		{
			this.startPosition = Vector2.zero;
			this.endPosition = Vector2.zero;
			this.layerMask = -1;
		}

		// Token: 0x040055AC RID: 21932
		[Tooltip("The starting position of the linecast.")]
		public SharedVector2 startPosition;

		// Token: 0x040055AD RID: 21933
		[Tooltip("The ending position of the linecast.")]
		public SharedVector2 endPosition;

		// Token: 0x040055AE RID: 21934
		[Tooltip("Selectively ignore colliders.")]
		public LayerMask layerMask = -1;
	}
}
