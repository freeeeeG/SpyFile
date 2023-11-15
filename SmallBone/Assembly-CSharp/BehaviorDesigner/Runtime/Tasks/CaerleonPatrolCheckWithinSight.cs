using System;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001461 RID: 5217
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
	public sealed class CaerleonPatrolCheckWithinSight : Conditional
	{
		// Token: 0x060065E8 RID: 26088 RVA: 0x001264F4 File Offset: 0x001246F4
		public override void OnAwake()
		{
			this._targetLayer = new TargetLayer(0, false, true, false, false);
			this._overlapper = new NonAllocOverlapper(31);
			CaerleonPatrolCheckWithinSight._reusableCaster.contactFilter.SetLayerMask(this._blockLayerMask);
			this._ownerValue = this._owner.Value;
			this._rangeValue = this._range.Value;
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x0012655C File Offset: 0x0012475C
		public override TaskStatus OnUpdate()
		{
			Character character = this.FindTarget();
			if (character == null)
			{
				return TaskStatus.Failure;
			}
			float distance = Mathf.Abs(character.transform.position.x - this._ownerValue.transform.position.x);
			Vector3 position = this._ownerValue.transform.position;
			if (CaerleonPatrolCheckWithinSight._reusableCaster.RayCast(position, (this._ownerValue.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right, distance).results.Count > 0)
			{
				return TaskStatus.Failure;
			}
			if (character.stealth.value)
			{
				return TaskStatus.Failure;
			}
			this._target.SetValue(character);
			return TaskStatus.Success;
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x00126610 File Offset: 0x00124810
		private Character FindTarget()
		{
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._ownerValue.gameObject));
			return TargetFinder.FindClosestTarget(this._overlapper, this._rangeValue, this._ownerValue.collider);
		}

		// Token: 0x040051CF RID: 20943
		private static readonly NonAllocCaster _reusableCaster = new NonAllocCaster(15);

		// Token: 0x040051D0 RID: 20944
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040051D1 RID: 20945
		[SerializeField]
		private SharedCollider _range;

		// Token: 0x040051D2 RID: 20946
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040051D3 RID: 20947
		private TargetLayer _targetLayer;

		// Token: 0x040051D4 RID: 20948
		private NonAllocOverlapper _overlapper;

		// Token: 0x040051D5 RID: 20949
		private Character _ownerValue;

		// Token: 0x040051D6 RID: 20950
		private Collider2D _rangeValue;

		// Token: 0x040051D7 RID: 20951
		[SerializeField]
		private LayerMask _blockLayerMask = 8;
	}
}
