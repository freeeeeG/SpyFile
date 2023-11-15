using System;
using FX;
using Singletons;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C76 RID: 3190
	[Serializable]
	public class StatBonusPerHeal : Ability
	{
		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x000BD2E3 File Offset: 0x000BB4E3
		// (set) Token: 0x06004124 RID: 16676 RVA: 0x000BD2EB File Offset: 0x000BB4EB
		private int stack
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = math.min(value, this._maxStack);
				if (this._instance == null)
				{
					return;
				}
				this._instance.UpdateStack();
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004125 RID: 16677 RVA: 0x000BD313 File Offset: 0x000BB513
		public int maxStack
		{
			get
			{
				return this._maxStack;
			}
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x000BD31B File Offset: 0x000BB51B
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x000BD338 File Offset: 0x000BB538
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new StatBonusPerHeal.Instance(owner, this);
		}

		// Token: 0x040031EE RID: 12782
		[SerializeField]
		private EffectInfo _buffLoopEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x040031EF RID: 12783
		[SerializeField]
		private SoundInfo _buffAttachAudioClipInfo;

		// Token: 0x040031F0 RID: 12784
		[SerializeField]
		private float _buffDuration;

		// Token: 0x040031F1 RID: 12785
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031F2 RID: 12786
		[SerializeField]
		private float _healAmountPerStack = 1f;

		// Token: 0x040031F3 RID: 12787
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040031F4 RID: 12788
		[SerializeField]
		private bool _healed = true;

		// Token: 0x040031F5 RID: 12789
		[SerializeField]
		private bool _overhealed;

		// Token: 0x040031F6 RID: 12790
		private StatBonusPerHeal.Instance _instance;

		// Token: 0x040031F7 RID: 12791
		private int _stack;

		// Token: 0x02000C77 RID: 3191
		public class Instance : AbilityInstance<StatBonusPerHeal>
		{
			// Token: 0x17000DB8 RID: 3512
			// (get) Token: 0x06004129 RID: 16681 RVA: 0x000BD381 File Offset: 0x000BB581
			public override Sprite icon
			{
				get
				{
					if (!this._attachedBuff)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000DB9 RID: 3513
			// (get) Token: 0x0600412A RID: 16682 RVA: 0x000BD393 File Offset: 0x000BB593
			public override float iconFillAmount
			{
				get
				{
					return this._remainBuffDuration / this.ability._buffDuration;
				}
			}

			// Token: 0x17000DBA RID: 3514
			// (get) Token: 0x0600412B RID: 16683 RVA: 0x000BD3A7 File Offset: 0x000BB5A7
			public override int iconStacks
			{
				get
				{
					return this.ability.stack;
				}
			}

			// Token: 0x0600412C RID: 16684 RVA: 0x000BD3B4 File Offset: 0x000BB5B4
			public Instance(Character owner, StatBonusPerHeal ability) : base(owner, ability)
			{
				base.iconFillInversed = true;
			}

			// Token: 0x0600412D RID: 16685 RVA: 0x000BD3C5 File Offset: 0x000BB5C5
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this.owner.health.onHealed += this.HandleOnHealed;
			}

			// Token: 0x0600412E RID: 16686 RVA: 0x000BD3FC File Offset: 0x000BB5FC
			private void HandleOnHealed(double healed, double overHealed)
			{
				this._remainBuffDuration = this.ability._buffDuration;
				double num = 0.0;
				if (this.ability._healed)
				{
					num += healed;
				}
				if (this.ability._overhealed)
				{
					num += overHealed;
				}
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._buffAttachAudioClipInfo, this.owner.transform.position);
				int num2 = Mathf.CeilToInt((float)num / this.ability._healAmountPerStack);
				if (this._attachedBuff)
				{
					if (num2 >= this.ability.stack)
					{
						this.ability.stack = num2;
						return;
					}
				}
				else
				{
					this.AttachBuff();
					this.ability.stack = num2;
				}
			}

			// Token: 0x0600412F RID: 16687 RVA: 0x000BD4B5 File Offset: 0x000BB6B5
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.owner.health.onHealed -= this.HandleOnHealed;
			}

			// Token: 0x06004130 RID: 16688 RVA: 0x000BD4E9 File Offset: 0x000BB6E9
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainBuffDuration -= deltaTime;
				if (this._remainBuffDuration < 0f && this._attachedBuff)
				{
					this.DetachBuff();
				}
			}

			// Token: 0x06004131 RID: 16689 RVA: 0x000BD51C File Offset: 0x000BB71C
			private void AttachBuff()
			{
				this._attachedBuff = true;
				this._buffLoopEffectInstance = ((this.ability._buffLoopEffect != null) ? this.ability._buffLoopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f) : null);
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x06004132 RID: 16690 RVA: 0x000BD58C File Offset: 0x000BB78C
			private void DetachBuff()
			{
				this._attachedBuff = false;
				if (this._buffLoopEffectInstance != null)
				{
					this._buffLoopEffectInstance.Stop();
				}
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004133 RID: 16691 RVA: 0x000BD5C4 File Offset: 0x000BB7C4
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability.stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031F8 RID: 12792
			private EffectPoolInstance _buffLoopEffectInstance;

			// Token: 0x040031F9 RID: 12793
			private Stat.Values _stat;

			// Token: 0x040031FA RID: 12794
			private float _remainBuffDuration;

			// Token: 0x040031FB RID: 12795
			private bool _attachedBuff;
		}
	}
}
