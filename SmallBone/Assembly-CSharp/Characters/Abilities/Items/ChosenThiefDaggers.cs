using System;
using System.Collections.Generic;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CA6 RID: 3238
	[Serializable]
	public sealed class ChosenThiefDaggers : Ability
	{
		// Token: 0x060041D3 RID: 16851 RVA: 0x000BF7E9 File Offset: 0x000BD9E9
		public override void Initialize()
		{
			base.Initialize();
			this._ownerAbility.Initialize();
			this._targetAbility.Initialize();
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x000BF807 File Offset: 0x000BDA07
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChosenThiefDaggers.Instance(owner, this);
		}

		// Token: 0x04003274 RID: 12916
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _ownerAbility;

		// Token: 0x04003275 RID: 12917
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _targetAbility;

		// Token: 0x02000CA7 RID: 3239
		public sealed class Instance : AbilityInstance<ChosenThiefDaggers>
		{
			// Token: 0x060041D6 RID: 16854 RVA: 0x000BF810 File Offset: 0x000BDA10
			public Instance(Character owner, ChosenThiefDaggers ability) : base(owner, ability)
			{
				this._targets = new HashSet<Character>();
			}

			// Token: 0x060041D7 RID: 16855 RVA: 0x000BF828 File Offset: 0x000BDA28
			protected override void OnAttach()
			{
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.LevelManager_onMapLoaded;
			}

			// Token: 0x060041D8 RID: 16856 RVA: 0x000BF898 File Offset: 0x000BDA98
			private void LevelManager_onMapLoaded()
			{
				this._targets.Clear();
			}

			// Token: 0x060041D9 RID: 16857 RVA: 0x000BF8A8 File Offset: 0x000BDAA8
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this._targets.Contains(character))
				{
					return;
				}
				this._targets.Remove(character);
				if (damageDealt == 0.0)
				{
					return;
				}
				this.owner.ability.Add(this.ability._ownerAbility.ability);
				foreach (AbilityComponent abilityComponent in this.ability._targetAbility.components)
				{
					character.ability.Add(abilityComponent.ability);
				}
			}

			// Token: 0x060041DA RID: 16858 RVA: 0x000BF948 File Offset: 0x000BDB48
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (target.character == null)
				{
					return false;
				}
				if (target.character.health.percent < 1.0)
				{
					return false;
				}
				if (!this._targets.Contains(target.character))
				{
					this._targets.Add(target.character);
				}
				return false;
			}

			// Token: 0x060041DB RID: 16859 RVA: 0x000BF9A8 File Offset: 0x000BDBA8
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				Singleton<Service>.Instance.levelManager.onMapLoaded -= this.LevelManager_onMapLoaded;
				this.owner.ability.Remove(this.ability._ownerAbility.ability);
			}

			// Token: 0x04003276 RID: 12918
			private HashSet<Character> _targets;
		}
	}
}
