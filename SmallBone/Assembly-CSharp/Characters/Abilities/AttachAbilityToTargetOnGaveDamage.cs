using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009CE RID: 2510
	[Serializable]
	public class AttachAbilityToTargetOnGaveDamage : Ability
	{
		// Token: 0x06003566 RID: 13670 RVA: 0x0009E880 File Offset: 0x0009CA80
		public override void Initialize()
		{
			base.Initialize();
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0009E893 File Offset: 0x0009CA93
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AttachAbilityToTargetOnGaveDamage.Instance(owner, this);
		}

		// Token: 0x04002B04 RID: 11012
		[SerializeField]
		private SoundInfo _attachAudioClipInfo;

		// Token: 0x04002B05 RID: 11013
		[SerializeField]
		private bool _needCritical;

		// Token: 0x04002B06 RID: 11014
		[SerializeField]
		private CharacterTypeBoolArray _targetFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true
		});

		// Token: 0x04002B07 RID: 11015
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04002B08 RID: 11016
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04002B09 RID: 11017
		[SerializeField]
		private string _attackKey;

		// Token: 0x04002B0A RID: 11018
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _abilityComponent;

		// Token: 0x020009CF RID: 2511
		public class Instance : AbilityInstance<AttachAbilityToTargetOnGaveDamage>
		{
			// Token: 0x06003569 RID: 13673 RVA: 0x0009E8C0 File Offset: 0x0009CAC0
			public Instance(Character owner, AttachAbilityToTargetOnGaveDamage ability) : base(owner, ability)
			{
			}

			// Token: 0x0600356A RID: 13674 RVA: 0x0009E8CA File Offset: 0x0009CACA
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x0600356B RID: 13675 RVA: 0x0009E8F3 File Offset: 0x0009CAF3
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}

			// Token: 0x0600356C RID: 13676 RVA: 0x0009E91C File Offset: 0x0009CB1C
			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (target.character == null || target.character.health.dead)
				{
					return;
				}
				if (this.ability._needCritical && !gaveDamage.critical)
				{
					return;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !gaveDamage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (!this.ability._targetFilter[target.character.type] || !this.ability._motionTypeFilter[gaveDamage.motionType] || !this.ability._attackTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				target.character.ability.Add(this.ability._abilityComponent.ability);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachAudioClipInfo, this.owner.transform.position);
			}
		}
	}
}
