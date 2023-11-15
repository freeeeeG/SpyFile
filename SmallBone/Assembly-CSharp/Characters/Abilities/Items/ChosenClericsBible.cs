using System;
using Characters.Operations;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CA0 RID: 3232
	[Serializable]
	public sealed class ChosenClericsBible : Ability
	{
		// Token: 0x060041B3 RID: 16819 RVA: 0x000BF001 File Offset: 0x000BD201
		public override void Initialize()
		{
			base.Initialize();
			this._onHealed.Initialize();
			this._onAttachBuff.Initialize();
			this._onDetachBuff.Initialize();
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x000BF02A File Offset: 0x000BD22A
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChosenClericsBible.Instance(owner, this);
		}

		// Token: 0x04003254 RID: 12884
		[SerializeField]
		private Transform _wingParent;

		// Token: 0x04003255 RID: 12885
		[SerializeField]
		private Animator _animator;

		// Token: 0x04003256 RID: 12886
		[Header("회복 필요 조건")]
		[SerializeField]
		private CharacterTypeBoolArray _targetFilter;

		// Token: 0x04003257 RID: 12887
		[SerializeField]
		private int _killCount;

		// Token: 0x04003258 RID: 12888
		[SerializeField]
		private float _healAmount;

		// Token: 0x04003259 RID: 12889
		[SerializeField]
		[Header("회복 시")]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onHealed;

		// Token: 0x0400325A RID: 12890
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onAttachBuff;

		// Token: 0x0400325B RID: 12891
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onDetachBuff;

		// Token: 0x0400325C RID: 12892
		[SerializeField]
		private Stat.Values _buff;

		// Token: 0x0400325D RID: 12893
		[SerializeField]
		private float _buffDuration;

		// Token: 0x0400325E RID: 12894
		[SerializeField]
		private float _onAttachBuffAttackInterval = 1f;

		// Token: 0x02000CA1 RID: 3233
		public sealed class Instance : AbilityInstance<ChosenClericsBible>
		{
			// Token: 0x17000DC8 RID: 3528
			// (get) Token: 0x060041B6 RID: 16822 RVA: 0x000BF046 File Offset: 0x000BD246
			public override Sprite icon
			{
				get
				{
					if (!this._buffAttached)
					{
						return base.icon;
					}
					return null;
				}
			}

			// Token: 0x17000DC9 RID: 3529
			// (get) Token: 0x060041B7 RID: 16823 RVA: 0x000BF058 File Offset: 0x000BD258
			public override int iconStacks
			{
				get
				{
					return this.ability._killCount - this._remainKillCount;
				}
			}

			// Token: 0x060041B8 RID: 16824 RVA: 0x000BF06C File Offset: 0x000BD26C
			public Instance(Character owner, ChosenClericsBible ability) : base(owner, ability)
			{
			}

			// Token: 0x060041B9 RID: 16825 RVA: 0x000BF078 File Offset: 0x000BD278
			protected override void OnAttach()
			{
				this.ability._animator.transform.SetParent(this.owner.attachWithFlip.transform);
				this._remainKillCount = this.ability._killCount;
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
				this.owner.health.onHealed += this.HandleOnHealed;
			}

			// Token: 0x060041BA RID: 16826 RVA: 0x000BF0FE File Offset: 0x000BD2FE
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainBuffTime -= deltaTime;
				if (this._remainBuffTime <= 0f && this._buffAttached)
				{
					this.DetachBuff();
				}
				this.UpdateAttachedTime(deltaTime);
			}

			// Token: 0x060041BB RID: 16827 RVA: 0x000BF138 File Offset: 0x000BD338
			private void UpdateAttachedTime(float deltatime)
			{
				if (!this._buffAttached)
				{
					return;
				}
				this._remainAttackTime -= deltatime;
				if (this._remainAttackTime > 0f)
				{
					return;
				}
				this._remainAttackTime = this.ability._onAttachBuffAttackInterval;
				this._cOnAttachBuffOperations.Stop();
				this._cOnAttachBuffOperations = this.owner.StartCoroutineWithReference(this.ability._onAttachBuff.CRun(this.owner));
			}

			// Token: 0x060041BC RID: 16828 RVA: 0x000BF1B0 File Offset: 0x000BD3B0
			private void HandleOnKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._targetFilter[character.type])
				{
					return;
				}
				this._remainKillCount--;
				if (this._remainKillCount <= 0)
				{
					this._remainKillCount = this.ability._killCount;
					this.owner.StartCoroutine(this.ability._onHealed.CRun(this.owner));
					this.owner.health.Heal((double)this.ability._healAmount, true);
				}
			}

			// Token: 0x060041BD RID: 16829 RVA: 0x000BF250 File Offset: 0x000BD450
			private void AttachBuff()
			{
				if (!this._buffAttached)
				{
					this.ability._animator.gameObject.SetActive(true);
					this.ability._animator.Play(ChosenClericsBible.Instance._activateHash);
					this.owner.stat.AttachOrUpdateValues(this.ability._buff);
					this._remainAttackTime = this.ability._onAttachBuffAttackInterval;
				}
				this._remainBuffTime = this.ability._buffDuration;
				this._buffAttached = true;
			}

			// Token: 0x060041BE RID: 16830 RVA: 0x000BF2D4 File Offset: 0x000BD4D4
			private void DetachBuff()
			{
				this.ability._animator.gameObject.SetActive(false);
				this.owner.stat.DetachValues(this.ability._buff);
				this.owner.StartCoroutine(this.ability._onDetachBuff.CRun(this.owner));
				this._buffAttached = false;
			}

			// Token: 0x060041BF RID: 16831 RVA: 0x000BF33B File Offset: 0x000BD53B
			private void HandleOnHealed(double healed, double overHealed)
			{
				this.AttachBuff();
			}

			// Token: 0x060041C0 RID: 16832 RVA: 0x000BF344 File Offset: 0x000BD544
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
				this.owner.health.onHealed -= this.HandleOnHealed;
				if (this.ability._wingParent == null)
				{
					this.ability._animator.transform.SetParent(Map.Instance.transform);
				}
				else
				{
					this.ability._animator.transform.SetParent(this.owner.attachWithFlip.transform);
				}
				this.DetachBuff();
			}

			// Token: 0x0400325F RID: 12895
			private static readonly int _activateHash = Animator.StringToHash("Start");

			// Token: 0x04003260 RID: 12896
			private int _remainKillCount;

			// Token: 0x04003261 RID: 12897
			private float _remainBuffTime;

			// Token: 0x04003262 RID: 12898
			private bool _buffAttached;

			// Token: 0x04003263 RID: 12899
			private float _remainAttackTime;

			// Token: 0x04003264 RID: 12900
			private CoroutineReference _cOnAttachBuffOperations;
		}
	}
}
