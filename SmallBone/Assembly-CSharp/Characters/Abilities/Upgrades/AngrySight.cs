using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000ACD RID: 2765
	[Serializable]
	public sealed class AngrySight : Ability
	{
		// Token: 0x060038C3 RID: 14531 RVA: 0x000A7447 File Offset: 0x000A5647
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AngrySight.Instance(owner, this);
		}

		// Token: 0x04002D23 RID: 11555
		[SerializeField]
		private SoundInfo _attachAudioClipInfo;

		// Token: 0x04002D24 RID: 11556
		[SerializeField]
		private SoundInfo _detachAudioClipInfo;

		// Token: 0x04002D25 RID: 11557
		[SerializeField]
		private float _damagePercentMultiplier = 1f;

		// Token: 0x04002D26 RID: 11558
		[SerializeField]
		private float _distance;

		// Token: 0x04002D27 RID: 11559
		[SerializeField]
		private float _angryTime;

		// Token: 0x02000ACE RID: 2766
		public sealed class Instance : AbilityInstance<AngrySight>
		{
			// Token: 0x17000BCE RID: 3022
			// (get) Token: 0x060038C5 RID: 14533 RVA: 0x000A7463 File Offset: 0x000A5663
			public override Sprite icon
			{
				get
				{
					if (!this._buffAttached)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000BCF RID: 3023
			// (get) Token: 0x060038C6 RID: 14534 RVA: 0x000A7475 File Offset: 0x000A5675
			public override float iconFillAmount
			{
				get
				{
					return this._remainAngryTime / this.ability._angryTime;
				}
			}

			// Token: 0x060038C7 RID: 14535 RVA: 0x000A7489 File Offset: 0x000A5689
			public Instance(Character owner, AngrySight ability) : base(owner, ability)
			{
			}

			// Token: 0x060038C8 RID: 14536 RVA: 0x000A7494 File Offset: 0x000A5694
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(0, new TakeDamageDelegate(this.HandleOnTakeDamage));
				this.owner.onGiveDamage.Add(0, new GiveDamageDelegate(this.HandleOnGiveDamage));
				this._spotLight = this.owner.GetComponent<CharacterSpotLight>();
			}

			// Token: 0x060038C9 RID: 14537 RVA: 0x000A74F4 File Offset: 0x000A56F4
			private bool HandleOnTakeDamage(ref Damage damage)
			{
				if (damage.attackType == Damage.AttackType.None || damage.amount < 1.0)
				{
					return false;
				}
				this._remainAngryTime = this.ability._angryTime;
				if (!this._buffAttached)
				{
					this.DetachInvulnerable();
					this._buffAttached = true;
					return true;
				}
				this._buffAttached = true;
				return false;
			}

			// Token: 0x060038CA RID: 14538 RVA: 0x000A754C File Offset: 0x000A574C
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!this._buffAttached)
				{
					return false;
				}
				damage.percentMultiplier *= (double)this.ability._damagePercentMultiplier;
				return false;
			}

			// Token: 0x060038CB RID: 14539 RVA: 0x000A756F File Offset: 0x000A576F
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainAngryTime -= deltaTime;
				if (this._remainAngryTime <= 0f)
				{
					if (this._buffAttached)
					{
						this.Down();
					}
					this._buffAttached = false;
				}
			}

			// Token: 0x060038CC RID: 14540 RVA: 0x000A75A8 File Offset: 0x000A57A8
			private void DetachInvulnerable()
			{
				this.owner.invulnerable.Detach(this);
				this._spotLight.Activate();
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachAudioClipInfo, this.owner.transform.position);
			}

			// Token: 0x060038CD RID: 14541 RVA: 0x000A75F8 File Offset: 0x000A57F8
			private void Down()
			{
				this.owner.invulnerable.Attach(this);
				this._spotLight.Deactivate();
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._detachAudioClipInfo, this.owner.transform.position);
			}

			// Token: 0x060038CE RID: 14542 RVA: 0x000A7648 File Offset: 0x000A5848
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.HandleOnTakeDamage));
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				this.owner.invulnerable.Detach(this);
				this._spotLight.Deactivate();
			}

			// Token: 0x04002D28 RID: 11560
			private float _remainAngryTime;

			// Token: 0x04002D29 RID: 11561
			private bool _buffAttached;

			// Token: 0x04002D2A RID: 11562
			private CharacterSpotLight _spotLight;
		}
	}
}
