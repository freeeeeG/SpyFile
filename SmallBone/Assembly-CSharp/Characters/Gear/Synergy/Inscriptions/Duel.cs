using System;
using System.Collections;
using Characters.Abilities;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200087C RID: 2172
	public sealed class Duel : InscriptionInstance
	{
		// Token: 0x06002DA7 RID: 11687 RVA: 0x0008A770 File Offset: 0x00088970
		protected override void Initialize()
		{
			this._currentInstance = new Duel.StatBonus(this._effect);
			this._currentInstance.statPerStack.values[0].value = 1.0 + (double)this._statValuePerStack * 0.01;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0008A7C0 File Offset: 0x000889C0
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			this._currentInstance.maxStack = this._maxStackByStep[this.keyword.step];
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x0008A7DF File Offset: 0x000889DF
		public override void Attach()
		{
			Character character = base.character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0008A808 File Offset: 0x00088A08
		public override void Detach()
		{
			Character character = base.character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x0008A834 File Offset: 0x00088A34
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (this.keyword.step < 1)
			{
				return;
			}
			if (target.character == null)
			{
				return;
			}
			if (gaveDamage.attackType == Damage.AttackType.None || gaveDamage.attackType == Damage.AttackType.Additional)
			{
				return;
			}
			if (!this._characterTypes[target.character.type])
			{
				return;
			}
			if (!this._targetLayer.Evaluate(base.character.gameObject).Contains(target.character.gameObject.layer))
			{
				return;
			}
			if (this._currentInstance.attached && this._currentInstance.owner != null && this._currentInstance.owner.liveAndActive)
			{
				if (this._currentInstance.owner == target.character)
				{
					this._currentInstance.AddStack();
				}
				return;
			}
			target.character.ability.Add(this._currentInstance);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._attachSound, target.transform.position);
		}

		// Token: 0x04002629 RID: 9769
		[SerializeField]
		[Header("2세트 효과")]
		private TargetLayer _targetLayer;

		// Token: 0x0400262A RID: 9770
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x0400262B RID: 9771
		[Tooltip("혹시 단계별 이펙트를 바꾸고 싶을 경우 Duel 애니메이터의 Transition과 Parameter를 수정하면 됨")]
		[SerializeField]
		private EffectInfo _effect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400262C RID: 9772
		[SerializeField]
		private SoundInfo _attachSound;

		// Token: 0x0400262D RID: 9773
		[Range(0f, 100f)]
		[SerializeField]
		private float _statValuePerStack;

		// Token: 0x0400262E RID: 9774
		[SerializeField]
		private int[] _maxStackByStep;

		// Token: 0x0400262F RID: 9775
		private Duel.StatBonus _currentInstance;

		// Token: 0x0200087D RID: 2173
		private class StatBonus : IAbility, IAbilityInstance
		{
			// Token: 0x170009AB RID: 2475
			// (get) Token: 0x06002DAD RID: 11693 RVA: 0x0008A95B File Offset: 0x00088B5B
			public Character owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x170009AC RID: 2476
			// (get) Token: 0x06002DAE RID: 11694 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170009AD RID: 2477
			// (get) Token: 0x06002DAF RID: 11695 RVA: 0x0008A963 File Offset: 0x00088B63
			// (set) Token: 0x06002DB0 RID: 11696 RVA: 0x0008A96B File Offset: 0x00088B6B
			public float remainTime { get; set; }

			// Token: 0x170009AE RID: 2478
			// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x0008A974 File Offset: 0x00088B74
			// (set) Token: 0x06002DB2 RID: 11698 RVA: 0x0008A97C File Offset: 0x00088B7C
			public bool attached { get; private set; }

			// Token: 0x170009AF RID: 2479
			// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x0008A985 File Offset: 0x00088B85
			// (set) Token: 0x06002DB4 RID: 11700 RVA: 0x0008A98D File Offset: 0x00088B8D
			public Sprite icon { get; set; }

			// Token: 0x170009B0 RID: 2480
			// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x00071719 File Offset: 0x0006F919
			public float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x170009B1 RID: 2481
			// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillInversed
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009B2 RID: 2482
			// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool iconFillFlipped
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009B3 RID: 2483
			// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconStacks
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170009B4 RID: 2484
			// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009B5 RID: 2485
			// (get) Token: 0x06002DBA RID: 11706 RVA: 0x0008A996 File Offset: 0x00088B96
			// (set) Token: 0x06002DBB RID: 11707 RVA: 0x0008A99E File Offset: 0x00088B9E
			public float duration { get; set; }

			// Token: 0x170009B6 RID: 2486
			// (get) Token: 0x06002DBC RID: 11708 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x170009B7 RID: 2487
			// (get) Token: 0x06002DBD RID: 11709 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002DBE RID: 11710 RVA: 0x0008A9A8 File Offset: 0x00088BA8
			public StatBonus(EffectInfo effect)
			{
				this._effect = effect;
			}

			// Token: 0x06002DBF RID: 11711 RVA: 0x0008AA1A File Offset: 0x00088C1A
			public IAbilityInstance CreateInstance(Character owner)
			{
				this._owner = owner;
				return this;
			}

			// Token: 0x06002DC0 RID: 11712 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002DC1 RID: 11713 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x06002DC2 RID: 11714 RVA: 0x0008AA24 File Offset: 0x00088C24
			public void Refresh()
			{
				this.AddStack();
			}

			// Token: 0x06002DC3 RID: 11715 RVA: 0x0008AA2C File Offset: 0x00088C2C
			void IAbilityInstance.Attach()
			{
				this.attached = true;
				this._effectInstance = ((this._effect == null) ? null : this._effect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
				this._stacks = 1;
				this._owner.stat.AttachValues(this._stat);
				this.UpdateStack();
				this._owner.StartCoroutine(this.CUpdateAnimation());
			}

			// Token: 0x06002DC4 RID: 11716 RVA: 0x0008AAB4 File Offset: 0x00088CB4
			void IAbilityInstance.Detach()
			{
				this.attached = false;
				if (this._effectInstance != null)
				{
					this._effectInstance.Stop();
					this._effectInstance = null;
				}
				this._stacks = 0;
				this._owner.stat.DetachValues(this._stat);
				this._owner.StopCoroutine(this.CUpdateAnimation());
			}

			// Token: 0x06002DC5 RID: 11717 RVA: 0x0008AB16 File Offset: 0x00088D16
			public void AddStack()
			{
				if (this._stacks == this.maxStack)
				{
					return;
				}
				this._stacks++;
				this._effectInstance.animator.SetInteger("Stacks", this._stacks);
				this.UpdateStack();
			}

			// Token: 0x06002DC6 RID: 11718 RVA: 0x0008AB58 File Offset: 0x00088D58
			private void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.statPerStack.values[i].GetStackedValue((double)this._stacks);
				}
				this._owner.stat.SetNeedUpdate();
			}

			// Token: 0x06002DC7 RID: 11719 RVA: 0x0008ABB8 File Offset: 0x00088DB8
			private IEnumerator CUpdateAnimation()
			{
				bool update = false;
				for (;;)
				{
					yield return null;
					if (update)
					{
						this._effectInstance.animator.SetInteger("Stacks", this._stacks);
						update = false;
					}
					if (!this._owner.attach.gameObject.activeSelf)
					{
						update = true;
					}
				}
				yield break;
			}

			// Token: 0x04002630 RID: 9776
			[NonSerialized]
			public Stat.Values statPerStack = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.0)
			});

			// Token: 0x04002631 RID: 9777
			[NonSerialized]
			public int maxStack;

			// Token: 0x04002632 RID: 9778
			private Stat.Values _stat = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.0)
			});

			// Token: 0x04002633 RID: 9779
			private Character _owner;

			// Token: 0x04002634 RID: 9780
			private int _stacks;

			// Token: 0x04002635 RID: 9781
			private readonly EffectInfo _effect;

			// Token: 0x04002636 RID: 9782
			private EffectPoolInstance _effectInstance;
		}
	}
}
