using System;
using Characters.Abilities;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000879 RID: 2169
	public abstract class CooldownAbility : Ability
	{
		// Token: 0x0400261D RID: 9757
		[SerializeField]
		internal float _buffDuration;

		// Token: 0x0400261E RID: 9758
		[SerializeField]
		internal float _cooldownTime;

		// Token: 0x0400261F RID: 9759
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		internal CharacterOperation.Subcomponents _onAttached;

		// Token: 0x04002620 RID: 9760
		[CharacterOperation.SubcomponentAttribute]
		internal CharacterOperation.Subcomponents _onDetached;

		// Token: 0x04002621 RID: 9761
		[SerializeField]
		internal EffectInfo _buffLoopEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0200087A RID: 2170
		public abstract class CooldownAbilityInstance : AbilityInstance<CooldownAbility>
		{
			// Token: 0x170009A6 RID: 2470
			// (get) Token: 0x06002D96 RID: 11670 RVA: 0x0008A3BA File Offset: 0x000885BA
			public override Sprite icon
			{
				get
				{
					if (this._remainBuffTime <= 0f && this._remainCooldownTime <= 0f)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x170009A7 RID: 2471
			// (get) Token: 0x06002D97 RID: 11671 RVA: 0x0008A3E0 File Offset: 0x000885E0
			public override float iconFillAmount
			{
				get
				{
					if (this._buffAttached)
					{
						return 1f - this._remainBuffTime / this.ability._buffDuration;
					}
					if (this._remainCooldownTime > 0f)
					{
						return 1f - this._remainCooldownTime / this.ability._cooldownTime;
					}
					return base.iconFillAmount;
				}
			}

			// Token: 0x06002D98 RID: 11672 RVA: 0x0008A43A File Offset: 0x0008863A
			public CooldownAbilityInstance(Character owner, CooldownAbility ability) : base(owner, ability)
			{
			}

			// Token: 0x06002D99 RID: 11673 RVA: 0x0008A444 File Offset: 0x00088644
			protected void ChangeIconFillToBuffTime()
			{
				base.iconFillInversed = false;
				base.iconFillFlipped = false;
			}

			// Token: 0x06002D9A RID: 11674 RVA: 0x0008A454 File Offset: 0x00088654
			protected void ChangeIconFillToCooldownTime()
			{
				base.iconFillInversed = true;
				base.iconFillFlipped = true;
			}

			// Token: 0x06002D9B RID: 11675 RVA: 0x0008A464 File Offset: 0x00088664
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._remainBuffTime < 0f)
				{
					this._remainCooldownTime -= deltaTime;
					if (this._remainCooldownTime < 0f)
					{
						this.ChangeIconFillToBuffTime();
					}
				}
				this._remainBuffTime -= deltaTime;
				if (this._buffAttached && this._remainBuffTime < 0f)
				{
					this.OnDetachBuff();
				}
			}

			// Token: 0x06002D9C RID: 11676 RVA: 0x0008A4D0 File Offset: 0x000886D0
			protected virtual void OnAttachBuff()
			{
				this._remainBuffTime = this.ability._buffDuration;
				this._remainCooldownTime = this.ability._cooldownTime;
				this.ChangeIconFillToBuffTime();
				this._buffAttached = true;
				this._loopEffect = ((this.ability._buffLoopEffect == null) ? null : this.ability._buffLoopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
			}

			// Token: 0x06002D9D RID: 11677 RVA: 0x0008A552 File Offset: 0x00088752
			protected virtual void OnDetachBuff()
			{
				this.ChangeIconFillToCooldownTime();
				if (this._loopEffect != null)
				{
					this._loopEffect.Stop();
					this._loopEffect = null;
				}
				this._buffAttached = false;
			}

			// Token: 0x04002622 RID: 9762
			protected float _remainCooldownTime;

			// Token: 0x04002623 RID: 9763
			protected float _remainBuffTime;

			// Token: 0x04002624 RID: 9764
			protected bool _buffAttached;

			// Token: 0x04002625 RID: 9765
			protected EffectPoolInstance _loopEffect;
		}
	}
}
