using System;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A3 RID: 5283
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
	public sealed class CheckWithinSight : Conditional
	{
		// Token: 0x06006706 RID: 26374 RVA: 0x0012A0A0 File Offset: 0x001282A0
		public override void OnAwake()
		{
			this._targetLayer = new TargetLayer(0, false, true, false, false);
			this._overlapper = new NonAllocOverlapper(31);
			this._ownerValue = this._owner.Value;
			this._rangeValue = this._range.Value;
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x0012A0F4 File Offset: 0x001282F4
		public override TaskStatus OnUpdate()
		{
			Character character = this.FindTarget();
			if (character == null)
			{
				return TaskStatus.Failure;
			}
			this._target.SetValue(character);
			return TaskStatus.Success;
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x0012A120 File Offset: 0x00128320
		private Character FindTarget()
		{
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._ownerValue.gameObject));
			return TargetFinder.FindClosestTarget(this._overlapper, this._rangeValue, this._ownerValue.collider);
		}

		// Token: 0x040052EB RID: 21227
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040052EC RID: 21228
		[SerializeField]
		private SharedCollider _range;

		// Token: 0x040052ED RID: 21229
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040052EE RID: 21230
		private TargetLayer _targetLayer;

		// Token: 0x040052EF RID: 21231
		private NonAllocOverlapper _overlapper;

		// Token: 0x040052F0 RID: 21232
		private Character _ownerValue;

		// Token: 0x040052F1 RID: 21233
		private Collider2D _rangeValue;
	}
}
