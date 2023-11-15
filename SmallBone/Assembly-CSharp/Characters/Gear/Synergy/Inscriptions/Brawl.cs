using System;
using Characters.Abilities;
using Characters.Operations;
using Characters.Utils;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000876 RID: 2166
	public sealed class Brawl : InscriptionInstance
	{
		// Token: 0x06002D71 RID: 11633 RVA: 0x00002191 File Offset: 0x00000391
		protected override void Initialize()
		{
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x0008A015 File Offset: 0x00088215
		public override void Attach()
		{
			this._ability.inscriptionInstance = this;
			base.character.ability.Add(this._ability);
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x0008A03A File Offset: 0x0008823A
		public override void Detach()
		{
			base.character.ability.Remove(this._ability);
		}

		// Token: 0x04002605 RID: 9733
		[SerializeField]
		private Brawl.Instance _ability;

		// Token: 0x02000877 RID: 2167
		[Serializable]
		private class Instance : IAbility, IAbilityInstance
		{
			// Token: 0x17000996 RID: 2454
			// (get) Token: 0x06002D76 RID: 11638 RVA: 0x0008A053 File Offset: 0x00088253
			// (set) Token: 0x06002D77 RID: 11639 RVA: 0x0008A05B File Offset: 0x0008825B
			public float duration { get; set; }

			// Token: 0x17000997 RID: 2455
			// (get) Token: 0x06002D78 RID: 11640 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000998 RID: 2456
			// (get) Token: 0x06002D79 RID: 11641 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000999 RID: 2457
			// (get) Token: 0x06002D7A RID: 11642 RVA: 0x0008A064 File Offset: 0x00088264
			public Character owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x1700099A RID: 2458
			// (get) Token: 0x06002D7B RID: 11643 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x1700099B RID: 2459
			// (get) Token: 0x06002D7C RID: 11644 RVA: 0x0008A06C File Offset: 0x0008826C
			// (set) Token: 0x06002D7D RID: 11645 RVA: 0x0008A074 File Offset: 0x00088274
			public float remainTime { get; set; }

			// Token: 0x1700099C RID: 2460
			// (get) Token: 0x06002D7E RID: 11646 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700099D RID: 2461
			// (get) Token: 0x06002D7F RID: 11647 RVA: 0x0008A07D File Offset: 0x0008827D
			public Sprite icon
			{
				get
				{
					if (!this.inscriptionInstance.keyword.isMaxStep)
					{
						return null;
					}
					return this._icon;
				}
			}

			// Token: 0x1700099E RID: 2462
			// (get) Token: 0x06002D80 RID: 11648 RVA: 0x00071719 File Offset: 0x0006F919
			public float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x1700099F RID: 2463
			// (get) Token: 0x06002D81 RID: 11649 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009A0 RID: 2464
			// (get) Token: 0x06002D82 RID: 11650 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009A1 RID: 2465
			// (get) Token: 0x06002D83 RID: 11651 RVA: 0x0008A099 File Offset: 0x00088299
			public int iconStacks
			{
				get
				{
					return this._attackCount;
				}
			}

			// Token: 0x170009A2 RID: 2466
			// (get) Token: 0x06002D84 RID: 11652 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002D85 RID: 11653 RVA: 0x0008A0A1 File Offset: 0x000882A1
			public void Attach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x06002D86 RID: 11654 RVA: 0x0008A0CA File Offset: 0x000882CA
			public IAbilityInstance CreateInstance(Character owner)
			{
				this._owner = owner;
				this._operations.Initialize();
				this._enhanceOperations.Initialize();
				this._historyManager = new StackHistoryManager<Character>(this._historyCapacity);
				return this;
			}

			// Token: 0x06002D87 RID: 11655 RVA: 0x0008A0FB File Offset: 0x000882FB
			public void Detach()
			{
				if (this.owner == null)
				{
					return;
				}
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x06002D88 RID: 11656 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002D89 RID: 11657 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002D8A RID: 11658 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x06002D8B RID: 11659 RVA: 0x0008A134 File Offset: 0x00088334
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (!this._attackTypeFilter[gaveDamage.attackType])
				{
					return;
				}
				if (!this._motionTypeFilter[gaveDamage.motionType])
				{
					return;
				}
				if (!this._hitTargetTypeFilter[target.character.type])
				{
					return;
				}
				if (gaveDamage.key.Equals(this._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				if (this.inscriptionInstance.keyword.step < 1)
				{
					return;
				}
				Character character = target.character;
				this._historyManager.ClearIfExpired();
				if (!this._historyManager.IsElapsedFromLastTime(character, this._minimumCooldownTime, true))
				{
					return;
				}
				if (this._historyManager.TryAddStack(character, 1, this._maxHitPerUnit, this._hitIntervalPerUnit))
				{
					if (this.inscriptionInstance.keyword.isMaxStep)
					{
						this._attackCount++;
					}
					this.Attack(target);
				}
			}

			// Token: 0x06002D8C RID: 11660 RVA: 0x0008A218 File Offset: 0x00088418
			private void Attack(ITarget target)
			{
				this._positionInfo.Attach(target, this._targetPoint);
				if (this._attackCount >= this._cycle && this.inscriptionInstance.keyword.isMaxStep)
				{
					this._attackCount = 0;
					this.owner.StartCoroutine(this._enhanceOperations.CRun(this.owner));
					return;
				}
				this.owner.StartCoroutine(this._operations.CRun(this.owner));
			}

			// Token: 0x04002606 RID: 9734
			[Header("공통")]
			[SerializeField]
			private CharacterTypeBoolArray _hitTargetTypeFilter;

			// Token: 0x04002607 RID: 9735
			[SerializeField]
			private AttackTypeBoolArray _attackTypeFilter;

			// Token: 0x04002608 RID: 9736
			[SerializeField]
			private MotionTypeBoolArray _motionTypeFilter;

			// Token: 0x04002609 RID: 9737
			[SerializeField]
			[Header("2세트 효과")]
			private AttackDamage _attackDamage;

			// Token: 0x0400260A RID: 9738
			[SerializeField]
			private float _minimumCooldownTime;

			// Token: 0x0400260B RID: 9739
			[SerializeField]
			private float _hitIntervalPerUnit;

			// Token: 0x0400260C RID: 9740
			[SerializeField]
			private int _maxHitPerUnit;

			// Token: 0x0400260D RID: 9741
			[SerializeField]
			private string _attackKey;

			// Token: 0x0400260E RID: 9742
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _operations;

			// Token: 0x0400260F RID: 9743
			private StackHistoryManager<Character> _historyManager;

			// Token: 0x04002610 RID: 9744
			[Header("4세트 효과")]
			[SerializeField]
			private Sprite _icon;

			// Token: 0x04002611 RID: 9745
			[SerializeField]
			private int _cycle;

			// Token: 0x04002612 RID: 9746
			[SerializeField]
			private PositionInfo _positionInfo;

			// Token: 0x04002613 RID: 9747
			[SerializeField]
			private Transform _targetPoint;

			// Token: 0x04002614 RID: 9748
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _enhanceOperations;

			// Token: 0x04002615 RID: 9749
			private int _attackCount;

			// Token: 0x04002616 RID: 9750
			private readonly int _historyCapacity = 128;

			// Token: 0x04002619 RID: 9753
			internal InscriptionInstance inscriptionInstance;

			// Token: 0x0400261A RID: 9754
			private Character _owner;
		}
	}
}
