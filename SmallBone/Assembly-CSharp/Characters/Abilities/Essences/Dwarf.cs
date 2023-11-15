using System;
using Characters.Gear.Quintessences;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BDC RID: 3036
	[Serializable]
	public class Dwarf : Ability
	{
		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003E74 RID: 15988 RVA: 0x000B5933 File Offset: 0x000B3B33
		// (set) Token: 0x06003E75 RID: 15989 RVA: 0x000B593B File Offset: 0x000B3B3B
		public int attackCount { get; set; }

		// Token: 0x06003E76 RID: 15990 RVA: 0x000B5944 File Offset: 0x000B3B44
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Dwarf.Instance(owner, this);
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x000B5950 File Offset: 0x000B3B50
		public void AddAttackCount(Character owner)
		{
			int attackCount = this.attackCount;
			this.attackCount = attackCount + 1;
			if (this.attackCount >= this._attackCountToPromote)
			{
				this._dwarfEssence.ChangeOnInventory(this._essenceToPromote.Instantiate());
				this._awakeningEffect.Spawn(owner.transform.position, owner, 0f, 1f);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._awakeningSound, owner.transform.position);
			}
		}

		// Token: 0x04003034 RID: 12340
		[SerializeField]
		private int _attackCountToPromote;

		// Token: 0x04003035 RID: 12341
		[SerializeField]
		private Quintessence _dwarfEssence;

		// Token: 0x04003036 RID: 12342
		[SerializeField]
		private Quintessence _essenceToPromote;

		// Token: 0x04003037 RID: 12343
		[SerializeField]
		private EffectInfo _awakeningEffect;

		// Token: 0x04003038 RID: 12344
		[SerializeField]
		private SoundInfo _awakeningSound;

		// Token: 0x04003039 RID: 12345
		public string _attackKey;

		// Token: 0x02000BDD RID: 3037
		public class Instance : AbilityInstance<Dwarf>
		{
			// Token: 0x17000D3B RID: 3387
			// (get) Token: 0x06003E79 RID: 15993 RVA: 0x000B59CF File Offset: 0x000B3BCF
			public override int iconStacks
			{
				get
				{
					return this.ability.attackCount;
				}
			}

			// Token: 0x06003E7A RID: 15994 RVA: 0x000B59DC File Offset: 0x000B3BDC
			public Instance(Character owner, Dwarf ability) : base(owner, ability)
			{
			}

			// Token: 0x06003E7B RID: 15995 RVA: 0x000B59E6 File Offset: 0x000B3BE6
			protected override void OnAttach()
			{
				Character owner = this.ability._dwarfEssence.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003E7C RID: 15996 RVA: 0x000B5A19 File Offset: 0x000B3C19
			protected override void OnDetach()
			{
				Character owner = this.ability._dwarfEssence.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x06003E7D RID: 15997 RVA: 0x000B5A4C File Offset: 0x000B3C4C
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (gaveDamage.motionType != Damage.MotionType.Quintessence)
				{
					return;
				}
				if (target.character == null)
				{
					return;
				}
				if (target.character.type == Character.Type.Dummy)
				{
					return;
				}
				if (!gaveDamage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				this.ability.AddAttackCount(this.owner);
			}
		}
	}
}
