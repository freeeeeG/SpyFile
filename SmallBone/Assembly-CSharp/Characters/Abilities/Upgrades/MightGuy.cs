using System;
using System.Linq;
using Characters.Actions;
using Characters.Operations;
using FX;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AEF RID: 2799
	[Serializable]
	public sealed class MightGuy : Ability
	{
		// Token: 0x06003932 RID: 14642 RVA: 0x000A8A93 File Offset: 0x000A6C93
		public override void Initialize()
		{
			base.Initialize();
			this._onLoseHealth.Initialize();
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000A8AA6 File Offset: 0x000A6CA6
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new MightGuy.Instance(owner, this);
		}

		// Token: 0x04002D66 RID: 11622
		[SerializeField]
		private int _loseHealthAmount;

		// Token: 0x04002D67 RID: 11623
		[SerializeField]
		private int _freeDashCountDefault;

		// Token: 0x04002D68 RID: 11624
		[SerializeField]
		private int _freeDashCountInBossMap;

		// Token: 0x04002D69 RID: 11625
		[SerializeField]
		private EffectInfo _debuffLoopEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04002D6A RID: 11626
		[SerializeField]
		private SoundInfo _debuffAttachAudioClipInfo;

		// Token: 0x04002D6B RID: 11627
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onLoseHealth;

		// Token: 0x02000AF0 RID: 2800
		public sealed class Instance : AbilityInstance<MightGuy>
		{
			// Token: 0x17000BD9 RID: 3033
			// (get) Token: 0x06003935 RID: 14645 RVA: 0x000A8AC9 File Offset: 0x000A6CC9
			public override int iconStacks
			{
				get
				{
					if (this._remainFreeDash > 0)
					{
						return this._remainFreeDash;
					}
					return 0;
				}
			}

			// Token: 0x17000BDA RID: 3034
			// (get) Token: 0x06003936 RID: 14646 RVA: 0x000A8ADC File Offset: 0x000A6CDC
			public override float iconFillAmount
			{
				get
				{
					return (float)((this._remainFreeDash <= 0) ? 1 : 0);
				}
			}

			// Token: 0x06003937 RID: 14647 RVA: 0x000A8AEC File Offset: 0x000A6CEC
			public Instance(Character owner, MightGuy ability) : base(owner, ability)
			{
			}

			// Token: 0x06003938 RID: 14648 RVA: 0x000A8AF8 File Offset: 0x000A6CF8
			protected override void OnAttach()
			{
				Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.HandleOnMapLoaded;
				this._remainFreeDash = this.ability._freeDashCountDefault;
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x06003939 RID: 14649 RVA: 0x000A8B48 File Offset: 0x000A6D48
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Dash)
				{
					return;
				}
				this._remainFreeDash--;
				if (this._remainFreeDash == 0)
				{
					this._loopEffect = this.ability._debuffLoopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
					PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._debuffAttachAudioClipInfo, this.owner.transform.position);
				}
				if (this._remainFreeDash < 0)
				{
					this.owner.health.TakeHealth((double)this.ability._loseHealthAmount);
					Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage((double)this.ability._loseHealthAmount, this.owner.transform.position);
					this.owner.StartCoroutine(this.ability._onLoseHealth.CRun(this.owner));
				}
			}

			// Token: 0x0600393A RID: 14650 RVA: 0x000A8C48 File Offset: 0x000A6E48
			private void HandleOnMapLoaded(Map old, Map @new)
			{
				if (this._loopEffect != null)
				{
					this._loopEffect.Stop();
					this._loopEffect = null;
				}
				if (@new.waveContainer.GetAllEnemies().Any((Character character) => character.type == Character.Type.Adventurer || character.type == Character.Type.Boss))
				{
					this._remainFreeDash = this.ability._freeDashCountInBossMap;
					return;
				}
				this._remainFreeDash = this.ability._freeDashCountDefault;
			}

			// Token: 0x0600393B RID: 14651 RVA: 0x000A8CCC File Offset: 0x000A6ECC
			protected override void OnDetach()
			{
				if (this._loopEffect != null)
				{
					this._loopEffect.Stop();
					this._loopEffect = null;
				}
				Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.HandleOnMapLoaded;
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x04002D6C RID: 11628
			private EffectPoolInstance _loopEffect;

			// Token: 0x04002D6D RID: 11629
			private int _remainFreeDash;
		}
	}
}
