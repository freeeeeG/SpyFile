using System;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
	// Token: 0x02001654 RID: 5716
	public sealed class CheckWithinSkulHead : Conditional
	{
		// Token: 0x06006CF7 RID: 27895 RVA: 0x001376EC File Offset: 0x001358EC
		public override TaskStatus OnUpdate()
		{
			NonAllocOverlapper nonAllocOverlapper = new NonAllocOverlapper(31);
			nonAllocOverlapper.contactFilter.SetLayerMask(this._layerMask);
			if (nonAllocOverlapper.OverlapCollider(this._range.Value).results.Count == 0)
			{
				return TaskStatus.Failure;
			}
			foreach (Collider2D collider2D in nonAllocOverlapper.OverlapCollider(this._range.Value).results)
			{
				if (collider2D.GetComponent<DroppedSkulHead>())
				{
					this._storeSkulHead.SetValue(collider2D.transform);
					return TaskStatus.Success;
				}
			}
			return TaskStatus.Failure;
		}

		// Token: 0x040058BA RID: 22714
		[SerializeField]
		private SharedCollider _range;

		// Token: 0x040058BB RID: 22715
		private LayerMask _layerMask = 134217728;

		// Token: 0x040058BC RID: 22716
		[SerializeField]
		private SharedTransform _storeSkulHead;
	}
}
