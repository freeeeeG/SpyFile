using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A26 RID: 2598
	[Serializable]
	public class ExtraDamageToBack : Ability
	{
		// Token: 0x060036EB RID: 14059 RVA: 0x000A278D File Offset: 0x000A098D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ExtraDamageToBack.Instance(owner, this);
		}

		// Token: 0x04002BE5 RID: 11237
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002BE6 RID: 11238
		[SerializeField]
		private float _percent = 1f;

		// Token: 0x04002BE7 RID: 11239
		[SerializeField]
		private float _percentPoint;

		// Token: 0x04002BE8 RID: 11240
		[Header("Filter")]
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002BE9 RID: 11241
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x02000A27 RID: 2599
		public class Instance : AbilityInstance<ExtraDamageToBack>
		{
			// Token: 0x060036ED RID: 14061 RVA: 0x000A27A9 File Offset: 0x000A09A9
			public Instance(Character owner, ExtraDamageToBack ability) : base(owner, ability)
			{
			}

			// Token: 0x060036EE RID: 14062 RVA: 0x000A27B3 File Offset: 0x000A09B3
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060036EF RID: 14063 RVA: 0x000A27D2 File Offset: 0x000A09D2
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnOwnerGiveDamage));
			}

			// Token: 0x060036F0 RID: 14064 RVA: 0x000A27F4 File Offset: 0x000A09F4
			private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this.ability._motionTypeFilter[damage.motionType] || !this.ability._attackTypeFilter[damage.attackType])
				{
					return false;
				}
				if (!(target.character == null))
				{
					Movement movement = target.character.movement;
					if (movement == null || movement.config.type != Movement.Config.Type.Static)
					{
						int num = Math.Sign(damage.attacker.transform.position.x - target.transform.position.x);
						if ((num == 1 && target.character.lookingDirection == Character.LookingDirection.Right) || (num == -1 && target.character.lookingDirection == Character.LookingDirection.Left))
						{
							return false;
						}
						damage.percentMultiplier *= (double)this.ability._percent;
						damage.multiplier += (double)this.ability._percentPoint;
						this._remainCooldownTime = this.ability._cooldownTime;
						return false;
					}
				}
				return false;
			}

			// Token: 0x04002BEA RID: 11242
			private float _remainCooldownTime;
		}
	}
}
