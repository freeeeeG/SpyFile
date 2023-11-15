using System;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BD6 RID: 3030
	[Serializable]
	public sealed class SpectorOwner : Ability
	{
		// Token: 0x06003E64 RID: 15972 RVA: 0x000B578B File Offset: 0x000B398B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new SpectorOwner.Instance(owner, this);
		}

		// Token: 0x04003030 RID: 12336
		[SerializeField]
		private string _attackKey;

		// Token: 0x02000BD7 RID: 3031
		public sealed class Instance : AbilityInstance<SpectorOwner>
		{
			// Token: 0x06003E66 RID: 15974 RVA: 0x000B5794 File Offset: 0x000B3994
			public Instance(Character owner, SpectorOwner ability) : base(owner, ability)
			{
			}

			// Token: 0x06003E67 RID: 15975 RVA: 0x000B57A0 File Offset: 0x000B39A0
			protected override void OnAttach()
			{
				this._inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.quintessence;
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x06003E68 RID: 15976 RVA: 0x000B57F8 File Offset: 0x000B39F8
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x06003E69 RID: 15977 RVA: 0x000B5824 File Offset: 0x000B3A24
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (!gaveDamage.key.Equals(this.ability._attackKey))
				{
					return;
				}
				if (target.character.health.dead)
				{
					this._inventory.items[0].cooldown.time.remainTime = 0f;
				}
			}

			// Token: 0x04003031 RID: 12337
			private QuintessenceInventory _inventory;
		}
	}
}
