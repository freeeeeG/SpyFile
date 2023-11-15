using System;
using BT.SharedValues;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BT
{
	// Token: 0x02001415 RID: 5141
	public sealed class CheckWithInMasterSight : Node
	{
		// Token: 0x0600651C RID: 25884 RVA: 0x00124C6C File Offset: 0x00122E6C
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

		// Token: 0x0600651D RID: 25885 RVA: 0x00124CBC File Offset: 0x00122EBC
		private Character FindTarget()
		{
			CheckWithInMasterSight._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._owner.gameObject));
			return TargetFinder.FindClosestTarget(CheckWithInMasterSight._overlapper, this._minion.leader.player.transform.position, this._range);
		}

		// Token: 0x04005168 RID: 20840
		[SerializeField]
		private Minion _minion;

		// Token: 0x04005169 RID: 20841
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x0400516A RID: 20842
		[SerializeField]
		private float _range;

		// Token: 0x0400516B RID: 20843
		private Character _owner;

		// Token: 0x0400516C RID: 20844
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(31);
	}
}
