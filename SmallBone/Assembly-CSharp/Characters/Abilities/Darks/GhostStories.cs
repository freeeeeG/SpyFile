using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BB5 RID: 2997
	[Serializable]
	public sealed class GhostStories : Ability
	{
		// Token: 0x06003DC4 RID: 15812 RVA: 0x000B38F0 File Offset: 0x000B1AF0
		public override void Initialize()
		{
			base.Initialize();
			this._onTeleport.Initialize();
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x000B3903 File Offset: 0x000B1B03
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GhostStories.Instance(owner, this);
		}

		// Token: 0x04002FC0 RID: 12224
		[SerializeField]
		private CharacterTypeBoolArray _attackerFilter;

		// Token: 0x04002FC1 RID: 12225
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _onTeleport;

		// Token: 0x04002FC2 RID: 12226
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x02000BB6 RID: 2998
		public sealed class Instance : AbilityInstance<GhostStories>
		{
			// Token: 0x06003DC7 RID: 15815 RVA: 0x000B390C File Offset: 0x000B1B0C
			public Instance(Character owner, GhostStories ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DC8 RID: 15816 RVA: 0x000B3916 File Offset: 0x000B1B16
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x06003DC9 RID: 15817 RVA: 0x000B392D File Offset: 0x000B1B2D
			protected override void OnAttach()
			{
				this.owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
			}

			// Token: 0x06003DCA RID: 15818 RVA: 0x000B394C File Offset: 0x000B1B4C
			private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				Character character = tookDamage.attacker.character;
				if (character == null || !this.ability._attackerFilter[character.type])
				{
					return;
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				this.owner.StartCoroutine(this.CTeleport());
			}

			// Token: 0x06003DCB RID: 15819 RVA: 0x000B39B8 File Offset: 0x000B1BB8
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
			}

			// Token: 0x06003DCC RID: 15820 RVA: 0x000B39D6 File Offset: 0x000B1BD6
			private IEnumerator CTeleport()
			{
				yield return Chronometer.global.WaitForSeconds(1f);
				this.ability._onTeleport.Run(this.owner);
				yield break;
			}

			// Token: 0x04002FC3 RID: 12227
			private float _remainCooldownTime;
		}
	}
}
