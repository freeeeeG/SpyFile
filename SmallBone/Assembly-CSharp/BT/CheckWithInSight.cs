using System;
using BT.SharedValues;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BT
{
	// Token: 0x02001416 RID: 5142
	public sealed class CheckWithInSight : Node
	{
		// Token: 0x06006520 RID: 25888 RVA: 0x00124D2C File Offset: 0x00122F2C
		protected override NodeState UpdateDeltatime(Context context)
		{
			if (this._owner == null)
			{
				this._owner = context.Get<Character>(Key.OwnerCharacter);
			}
			Character character = this.FindTarget();
			if (character == null)
			{
				return NodeState.Fail;
			}
			context.Set<Character>(Key.Target, new SharedValue<Character>(character));
			return NodeState.Success;
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x00124D7C File Offset: 0x00122F7C
		private Character FindTarget()
		{
			CheckWithInSight._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._owner.gameObject));
			return TargetFinder.FindClosestTarget(CheckWithInSight._overlapper, this._range, this._owner.collider);
		}

		// Token: 0x0400516D RID: 20845
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x0400516E RID: 20846
		[SerializeField]
		private Collider2D _range;

		// Token: 0x0400516F RID: 20847
		private Character _owner;

		// Token: 0x04005170 RID: 20848
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(31);
	}
}
