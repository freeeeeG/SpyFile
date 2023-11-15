using System;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B5 RID: 5301
	[TaskDescription("Collider가 해당 layerMask를 가진 충돌체의 바운드로부터 포함되어있는지")]
	public sealed class IsContainBounds : Conditional
	{
		// Token: 0x0600672D RID: 26413 RVA: 0x0012AAFA File Offset: 0x00128CFA
		public override void OnAwake()
		{
			this._overlapper = new NonAllocOverlapper(31);
			this._overlapper.contactFilter.SetLayerMask(this._layerMask);
			this._boundsValue = this._bounds.Value;
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x0012AB30 File Offset: 0x00128D30
		public override TaskStatus OnUpdate()
		{
			foreach (Collider2D collider2D in this._overlapper.OverlapCollider(this._boundsValue).results)
			{
				if (collider2D.bounds.min.x <= this._boundsValue.bounds.min.x && collider2D.bounds.min.y <= this._boundsValue.bounds.min.y && collider2D.bounds.max.x >= this._boundsValue.bounds.max.x && collider2D.bounds.max.y >= this._boundsValue.bounds.max.y)
				{
					return TaskStatus.Success;
				}
			}
			return TaskStatus.Failure;
		}

		// Token: 0x04005338 RID: 21304
		[SerializeField]
		private SharedCollider _bounds;

		// Token: 0x04005339 RID: 21305
		[SerializeField]
		private LayerMask _layerMask;

		// Token: 0x0400533A RID: 21306
		private Collider2D _boundsValue;

		// Token: 0x0400533B RID: 21307
		private NonAllocOverlapper _overlapper;
	}
}
