using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009B8 RID: 2488
	[Serializable]
	public class AirAndGroundAttackDamage : Ability
	{
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06003520 RID: 13600 RVA: 0x0009D688 File Offset: 0x0009B888
		public float groundPercent
		{
			get
			{
				return this._groundPercent;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x0009D690 File Offset: 0x0009B890
		public float airPercent
		{
			get
			{
				return this._airPercent;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x0009D698 File Offset: 0x0009B898
		public float groundPercentPoint
		{
			get
			{
				return this._groundPercentPoint;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06003523 RID: 13603 RVA: 0x0009D6A0 File Offset: 0x0009B8A0
		public float airPercentPoint
		{
			get
			{
				return this._airPercentPoint;
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x0009D6A8 File Offset: 0x0009B8A8
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AirAndGroundAttackDamage.Instance(owner, this);
		}

		// Token: 0x04002ACD RID: 10957
		[SerializeField]
		private float _groundPercent = 1f;

		// Token: 0x04002ACE RID: 10958
		[SerializeField]
		private float _airPercent = 1f;

		// Token: 0x04002ACF RID: 10959
		[SerializeField]
		private float _groundPercentPoint;

		// Token: 0x04002AD0 RID: 10960
		[SerializeField]
		private float _airPercentPoint;

		// Token: 0x020009B9 RID: 2489
		public class Instance : AbilityInstance<AirAndGroundAttackDamage>
		{
			// Token: 0x06003526 RID: 13606 RVA: 0x0009D6CF File Offset: 0x0009B8CF
			internal Instance(Character owner, AirAndGroundAttackDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x06003527 RID: 13607 RVA: 0x0009D6D9 File Offset: 0x0009B8D9
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06003528 RID: 13608 RVA: 0x0009D6F8 File Offset: 0x0009B8F8
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}

			// Token: 0x06003529 RID: 13609 RVA: 0x0009D718 File Offset: 0x0009B918
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (((character != null) ? character.health : null) == null)
				{
					return false;
				}
				damage.percentMultiplier *= (double)(this.owner.movement.isGrounded ? this.ability.groundPercent : this.ability.airPercent);
				damage.multiplier += (double)(this.owner.movement.isGrounded ? this.ability.groundPercentPoint : this.ability.airPercentPoint);
				return false;
			}
		}
	}
}
