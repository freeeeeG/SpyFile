using System;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B0D RID: 2829
	[Serializable]
	public sealed class ThornsArmor : Ability
	{
		// Token: 0x0600398E RID: 14734 RVA: 0x000A9C71 File Offset: 0x000A7E71
		public override void Initialize()
		{
			base.Initialize();
			this._onHit.Initialize();
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x000A9C84 File Offset: 0x000A7E84
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ThornsArmor.Instance(owner, this);
		}

		// Token: 0x04002DA2 RID: 11682
		[SerializeField]
		private SoundInfo _attachAudioClipInfo;

		// Token: 0x04002DA3 RID: 11683
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x04002DA4 RID: 11684
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002DA5 RID: 11685
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x04002DA6 RID: 11686
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onHit;

		// Token: 0x04002DA7 RID: 11687
		[SerializeField]
		private float _minDamage;

		// Token: 0x04002DA8 RID: 11688
		[SerializeField]
		private float _cooldownTime = 1f;

		// Token: 0x04002DA9 RID: 11689
		[SerializeField]
		private CustomFloat _damage;

		// Token: 0x02000B0E RID: 2830
		public sealed class Instance : AbilityInstance<ThornsArmor>
		{
			// Token: 0x06003991 RID: 14737 RVA: 0x000A9CA0 File Offset: 0x000A7EA0
			public Instance(Character owner, ThornsArmor ability) : base(owner, ability)
			{
			}

			// Token: 0x06003992 RID: 14738 RVA: 0x000A9CAC File Offset: 0x000A7EAC
			protected override void OnAttach()
			{
				this._remainCooldownTime = this.ability._cooldownTime;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachAudioClipInfo, this.owner.transform.position);
				this.owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
			}

			// Token: 0x06003993 RID: 14739 RVA: 0x000A9D0C File Offset: 0x000A7F0C
			private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				Character character = tookDamage.attacker.character;
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				if (character == null)
				{
					return;
				}
				if (!this.ability._characterType[character.type])
				{
					return;
				}
				if (!this.ability._motionType[tookDamage.motionType])
				{
					return;
				}
				if (!this.ability._attackType[tookDamage.attackType])
				{
					return;
				}
				if (damageDealt < (double)this.ability._minDamage)
				{
					return;
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				float value = this.ability._damage.value;
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage((double)value, MMMaths.RandomPointWithinBounds(character.collider.bounds));
				character.health.TakeHealth((double)value);
				character.StartCoroutine(this.ability._onHit.CRun(character));
			}

			// Token: 0x06003994 RID: 14740 RVA: 0x000A9E01 File Offset: 0x000A8001
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06003995 RID: 14741 RVA: 0x000A9E18 File Offset: 0x000A8018
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
			}

			// Token: 0x04002DAA RID: 11690
			private float _remainCooldownTime;
		}
	}
}
