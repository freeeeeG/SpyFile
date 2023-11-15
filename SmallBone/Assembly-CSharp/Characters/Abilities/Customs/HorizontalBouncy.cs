using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D5A RID: 3418
	[Serializable]
	public class HorizontalBouncy : Ability
	{
		// Token: 0x060044F6 RID: 17654 RVA: 0x000C85A1 File Offset: 0x000C67A1
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new HorizontalBouncy.Instance(owner, this);
		}

		// Token: 0x0400348A RID: 13450
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);

		// Token: 0x02000D5B RID: 3419
		public class Instance : AbilityInstance<HorizontalBouncy>
		{
			// Token: 0x060044F7 RID: 17655 RVA: 0x000C85AA File Offset: 0x000C67AA
			public Instance(Character owner, HorizontalBouncy ability) : base(owner, ability)
			{
			}

			// Token: 0x060044F8 RID: 17656 RVA: 0x000C85B4 File Offset: 0x000C67B4
			protected override void OnAttach()
			{
				this.owner.movement.controller.collisionState.rightCollisionDetector.OnEnter += this.Flip;
				this.owner.movement.controller.collisionState.leftCollisionDetector.OnEnter += this.Flip;
			}

			// Token: 0x060044F9 RID: 17657 RVA: 0x000C8618 File Offset: 0x000C6818
			protected override void OnDetach()
			{
				this.owner.movement.controller.collisionState.rightCollisionDetector.OnEnter -= this.Flip;
				this.owner.movement.controller.collisionState.leftCollisionDetector.OnEnter -= this.Flip;
			}

			// Token: 0x060044FA RID: 17658 RVA: 0x000C867C File Offset: 0x000C687C
			private void Flip(RaycastHit2D hit)
			{
				if ((8 & hit.collider.gameObject.layer) == 0)
				{
					return;
				}
				this.owner.ForceToLookAt((this.owner.lookingDirection == Character.LookingDirection.Left) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
				this.owner.movement.push.ApplyKnockback(this.owner, this.ability._pushInfo);
			}
		}
	}
}
