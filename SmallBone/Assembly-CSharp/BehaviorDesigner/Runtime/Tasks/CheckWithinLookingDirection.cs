using System;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A2 RID: 5282
	[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
	public sealed class CheckWithinLookingDirection : Conditional
	{
		// Token: 0x06006702 RID: 26370 RVA: 0x00129EEC File Offset: 0x001280EC
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
			ContactFilter2D contactFilter = default(ContactFilter2D);
			contactFilter.SetLayerMask(this._layerMask);
			if (this._rayDistance <= 0f)
			{
				this._rayDistance = this._ownerValue.collider.bounds.extents.x + this._skinWidth;
			}
			this._rayCaster = new RayCaster
			{
				contactFilter = contactFilter
			};
		}

		// Token: 0x06006703 RID: 26371 RVA: 0x00129F68 File Offset: 0x00128168
		public override TaskStatus OnUpdate()
		{
			this._rayCaster.origin = this.transform.position;
			Vector2 offset = this._ownerValue.collider.offset;
			if (this.yOffset == 0f)
			{
				RayCaster rayCaster = this._rayCaster;
				rayCaster.origin.y = rayCaster.origin.y + offset.y;
			}
			else
			{
				RayCaster rayCaster2 = this._rayCaster;
				rayCaster2.origin.y = rayCaster2.origin.y + this.yOffset;
			}
			if (!this._reverseLookingDirection)
			{
				this._rayCaster.direction = ((this._ownerValue.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right);
			}
			else
			{
				this._rayCaster.direction = ((this._ownerValue.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left);
			}
			this._rayCaster.distance = this._rayDistance;
			foreach (RaycastHit2D raycastHit2D in this._rayCaster.Cast())
			{
				if (!(raycastHit2D.collider.gameObject == this._ownerValue.gameObject))
				{
					return TaskStatus.Success;
				}
			}
			return TaskStatus.Failure;
		}

		// Token: 0x040052E3 RID: 21219
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040052E4 RID: 21220
		[SerializeField]
		private float _rayDistance;

		// Token: 0x040052E5 RID: 21221
		[SerializeField]
		private LayerMask _layerMask;

		// Token: 0x040052E6 RID: 21222
		[SerializeField]
		private bool _reverseLookingDirection;

		// Token: 0x040052E7 RID: 21223
		[SerializeField]
		private float yOffset;

		// Token: 0x040052E8 RID: 21224
		private float _skinWidth = 0.1f;

		// Token: 0x040052E9 RID: 21225
		private RayCaster _rayCaster;

		// Token: 0x040052EA RID: 21226
		private Character _ownerValue;
	}
}
