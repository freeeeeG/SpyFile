using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Weapons.Fighter
{
	// Token: 0x02000C11 RID: 3089
	[Serializable]
	public sealed class ChallengerMarkPassive : Ability
	{
		// Token: 0x06003F7E RID: 16254 RVA: 0x000B83C4 File Offset: 0x000B65C4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChallengerMarkPassive.Instance(owner, this);
		}

		// Token: 0x040030E2 RID: 12514
		[SerializeField]
		private ChallengerMarkPassiveComponent _component;

		// Token: 0x040030E3 RID: 12515
		[SerializeField]
		private int _maxCount = 1;

		// Token: 0x040030E4 RID: 12516
		[SerializeField]
		private float _findTargetRadius;

		// Token: 0x040030E5 RID: 12517
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x040030E6 RID: 12518
		private NonAllocOverlapper _overalpper = new NonAllocOverlapper(32);

		// Token: 0x02000C12 RID: 3090
		public class Instance : AbilityInstance<ChallengerMarkPassive>
		{
			// Token: 0x06003F80 RID: 16256 RVA: 0x000B83E9 File Offset: 0x000B65E9
			public Instance(Character owner, ChallengerMarkPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06003F81 RID: 16257 RVA: 0x000B83F4 File Offset: 0x000B65F4
			protected override void OnAttach()
			{
				if (this.ability._component.Count() >= this.ability._maxCount)
				{
					base.Detach();
					return;
				}
				this.ability._component.Add(this);
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x06003F82 RID: 16258 RVA: 0x000B8454 File Offset: 0x000B6654
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
				this.ability._component.Remove(this);
				if (this.owner.health.dead)
				{
					this.FindNext();
				}
			}

			// Token: 0x06003F83 RID: 16259 RVA: 0x000B84A8 File Offset: 0x000B66A8
			private void FindNext()
			{
				this.ability._overalpper.contactFilter.SetLayerMask(1024);
				Character closestTarget = this.GetClosestTarget(this.ability._overalpper.OverlapCircle(this.owner.transform.position, this.ability._findTargetRadius).GetComponents<Target>(true), this.owner.transform.position);
				if (closestTarget != null)
				{
					closestTarget.ability.Add(this.ability);
				}
			}

			// Token: 0x06003F84 RID: 16260 RVA: 0x000B8544 File Offset: 0x000B6744
			private Character GetClosestTarget(List<Target> results, Vector2 center)
			{
				if (results.Count == 0)
				{
					return null;
				}
				if (results.Count == 1)
				{
					if (results[0].character == this.owner)
					{
						return null;
					}
					return results[0].character;
				}
				else
				{
					float num = float.MaxValue;
					int index = 0;
					for (int i = 1; i < results.Count; i++)
					{
						if (!(results[i].character == this.owner))
						{
							Collider2D collider = results[i].collider;
							if (results[i].character != null)
							{
								collider = results[i].character.collider;
							}
							float num2 = Vector2.Distance(center, collider.transform.position);
							if (num > num2)
							{
								index = i;
								num = num2;
							}
						}
					}
					if (results[index].character == this.owner)
					{
						return null;
					}
					return results[index].character;
				}
			}
		}
	}
}
