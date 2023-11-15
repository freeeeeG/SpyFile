using System;
using Characters.Actions;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A2C RID: 2604
	[Serializable]
	public sealed class GetDeflect : Ability
	{
		// Token: 0x060036F8 RID: 14072 RVA: 0x000A293D File Offset: 0x000A0B3D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GetDeflect.Instance(owner, this);
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x000A2948 File Offset: 0x000A0B48
		private bool OnOwnerTakeDamage(ref Damage damage)
		{
			if (damage.attackType == Damage.AttackType.Projectile)
			{
				if (this._deflectAction != null)
				{
					this._deflectAction.TryStart();
				}
				IProjectile projectile = damage.attacker.projectile;
				if (projectile != null)
				{
					if (projectile.GetComponent<SkulHeadToTeleport>() != null)
					{
						return true;
					}
					if (this._deflectedProjectile != null)
					{
						this._deflectedProjectile.reusable.Spawn(this._deflectedTransformValue.position, true).GetComponent<DeflectedProjectile>().Deflect((this._owner.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left, projectile.GetComponentInChildren<SpriteRenderer>().sprite, projectile.transform.localScale, projectile.speed);
					}
					projectile.Despawn();
				}
				return true;
			}
			return false;
		}

		// Token: 0x04002BEB RID: 11243
		[SerializeField]
		private Character _owner;

		// Token: 0x04002BEC RID: 11244
		[SerializeField]
		private Characters.Actions.Action _deflectAction;

		// Token: 0x04002BED RID: 11245
		[SerializeField]
		private DeflectedProjectile _deflectedProjectile;

		// Token: 0x04002BEE RID: 11246
		[SerializeField]
		private Transform _deflectedTransformValue;

		// Token: 0x02000A2D RID: 2605
		public sealed class Instance : AbilityInstance<GetDeflect>
		{
			// Token: 0x060036FB RID: 14075 RVA: 0x000A2A15 File Offset: 0x000A0C15
			public Instance(Character owner, GetDeflect ability) : base(owner, ability)
			{
			}

			// Token: 0x060036FC RID: 14076 RVA: 0x000A2A1F File Offset: 0x000A0C1F
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.ability.OnOwnerTakeDamage));
			}

			// Token: 0x060036FD RID: 14077 RVA: 0x000A2A4C File Offset: 0x000A0C4C
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.ability.OnOwnerTakeDamage));
			}
		}
	}
}
