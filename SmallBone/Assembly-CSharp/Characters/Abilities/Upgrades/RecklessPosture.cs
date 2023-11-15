using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B01 RID: 2817
	[Serializable]
	public sealed class RecklessPosture : Ability
	{
		// Token: 0x06003965 RID: 14693 RVA: 0x000A94A0 File Offset: 0x000A76A0
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RecklessPosture.Instance(owner, this);
		}

		// Token: 0x04002D8B RID: 11659
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04002D8C RID: 11660
		[SerializeField]
		private EffectInfo _buffLoopEffect;

		// Token: 0x04002D8D RID: 11661
		[SerializeField]
		private SoundInfo _buffAttachAudioClipInfo;

		// Token: 0x04002D8E RID: 11662
		[SerializeField]
		private float _buffDuration;

		// Token: 0x04002D8F RID: 11663
		[SerializeField]
		private Stat.Values _baseStat;

		// Token: 0x04002D90 RID: 11664
		[SerializeField]
		private int _multiplierWhenHit;

		// Token: 0x02000B02 RID: 2818
		public sealed class Instance : AbilityInstance<RecklessPosture>
		{
			// Token: 0x06003967 RID: 14695 RVA: 0x000A94A9 File Offset: 0x000A76A9
			public Instance(Character owner, RecklessPosture ability) : base(owner, ability)
			{
			}

			// Token: 0x06003968 RID: 14696 RVA: 0x000A94B4 File Offset: 0x000A76B4
			protected override void OnAttach()
			{
				this._stat = this.ability._baseStat.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
			}

			// Token: 0x06003969 RID: 14697 RVA: 0x000A9509 File Offset: 0x000A7709
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainBuffTime -= deltaTime;
				if (this._buffAttached && this._remainBuffTime <= 0f)
				{
					this.ResetStat();
				}
			}

			// Token: 0x0600396A RID: 14698 RVA: 0x000A953B File Offset: 0x000A773B
			private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (!this.ability._attackTypeFilter[tookDamage.attackType])
				{
					return;
				}
				this.MultiplyStat();
			}

			// Token: 0x0600396B RID: 14699 RVA: 0x000A955C File Offset: 0x000A775C
			private void ResetStat()
			{
				this._buffAttached = false;
				if (this._loopEffect != null)
				{
					this._loopEffect.Stop();
					this._loopEffect = null;
				}
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._baseStat.values[i].value;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600396C RID: 14700 RVA: 0x000A95E4 File Offset: 0x000A77E4
			private void MultiplyStat()
			{
				this._remainBuffTime = this.ability._buffDuration;
				this._buffAttached = true;
				this._loopEffect = ((this.ability._buffLoopEffect == null) ? null : this.ability._buffLoopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._baseStat.values[i].GetStackedValue((double)this.ability._multiplierWhenHit);
				}
				this.owner.stat.SetNeedUpdate();
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._buffAttachAudioClipInfo, this.owner.transform.position);
			}

			// Token: 0x0600396D RID: 14701 RVA: 0x000A96D2 File Offset: 0x000A78D2
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x04002D91 RID: 11665
			private EffectPoolInstance _loopEffect;

			// Token: 0x04002D92 RID: 11666
			private Stat.Values _stat;

			// Token: 0x04002D93 RID: 11667
			private bool _buffAttached;

			// Token: 0x04002D94 RID: 11668
			private float _remainBuffTime;
		}
	}
}
