using System;
using System.Collections.Generic;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008A7 RID: 2215
	public sealed class Rapidity : InscriptionInstance
	{
		// Token: 0x06002F18 RID: 12056 RVA: 0x0008D53D File Offset: 0x0008B73D
		protected override void Initialize()
		{
			this._step1Ability.Initialize(base.character);
			this._rapidityAttack.Initialize();
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x0008D55C File Offset: 0x0008B75C
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			if (this.keyword.isMaxStep)
			{
				if (!base.character.ability.Contains(this._rapidityAttack))
				{
					base.character.ability.Add(this._rapidityAttack);
				}
				return;
			}
			if (base.character.ability.Contains(this._rapidityAttack))
			{
				base.character.ability.Remove(this._rapidityAttack);
			}
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x0008D5D5 File Offset: 0x0008B7D5
		public override void Attach()
		{
			this._step1Ability.StartAttach();
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x0008D5E2 File Offset: 0x0008B7E2
		public override void Detach()
		{
			this._step1Ability.StopAttach();
		}

		// Token: 0x040026F3 RID: 9971
		[Header("2세트 효과")]
		[SerializeField]
		[Subcomponent(typeof(TriggerAbilityAttacher))]
		private TriggerAbilityAttacher _step1Ability;

		// Token: 0x040026F4 RID: 9972
		[Header("4세트 효과")]
		[SerializeField]
		private Rapidity.RapidityAttack _rapidityAttack;

		// Token: 0x020008A8 RID: 2216
		[Serializable]
		private sealed class RapidityAttack : Ability
		{
			// Token: 0x06002F1D RID: 12061 RVA: 0x0008D5EF File Offset: 0x0008B7EF
			public override void Initialize()
			{
				base.Initialize();
				this._onStep2Invoke.Initialize();
			}

			// Token: 0x06002F1E RID: 12062 RVA: 0x0008D602 File Offset: 0x0008B802
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Rapidity.RapidityAttack.Instance(owner, this);
			}

			// Token: 0x040026F5 RID: 9973
			[SerializeField]
			private ActionTypeBoolArray _triggerActionFilter;

			// Token: 0x040026F6 RID: 9974
			[SerializeField]
			private float _triggerInterval;

			// Token: 0x040026F7 RID: 9975
			[SerializeField]
			private int _triggerAttackCount;

			// Token: 0x040026F8 RID: 9976
			[SerializeField]
			private float _cooldownTime;

			// Token: 0x040026F9 RID: 9977
			[SerializeField]
			[Subcomponent(typeof(OperationInfo))]
			private OperationInfo.Subcomponents _onStep2Invoke;

			// Token: 0x020008A9 RID: 2217
			private sealed class Instance : AbilityInstance<Rapidity.RapidityAttack>
			{
				// Token: 0x17000A1A RID: 2586
				// (get) Token: 0x06002F20 RID: 12064 RVA: 0x0008D60B File Offset: 0x0008B80B
				public override float iconFillAmount
				{
					get
					{
						return this._remainCooldownTime / this.ability._cooldownTime;
					}
				}

				// Token: 0x06002F21 RID: 12065 RVA: 0x0008D61F File Offset: 0x0008B81F
				public Instance(Character owner, Rapidity.RapidityAttack ability) : base(owner, ability)
				{
					this._attackTimeHistory = new List<float>(ability._triggerAttackCount);
				}

				// Token: 0x06002F22 RID: 12066 RVA: 0x0008D648 File Offset: 0x0008B848
				protected override void OnAttach()
				{
					this.owner.onStartAction += this.HandleOnStartAction;
				}

				// Token: 0x06002F23 RID: 12067 RVA: 0x0008D661 File Offset: 0x0008B861
				protected override void OnDetach()
				{
					this.owner.onStartAction -= this.HandleOnStartAction;
				}

				// Token: 0x06002F24 RID: 12068 RVA: 0x0008D67A File Offset: 0x0008B87A
				public override void UpdateTime(float deltaTime)
				{
					base.UpdateTime(deltaTime);
					this._remainCooldownTime -= deltaTime;
				}

				// Token: 0x06002F25 RID: 12069 RVA: 0x0008D694 File Offset: 0x0008B894
				private void HandleOnStartAction(Characters.Actions.Action action)
				{
					if (!this.ability._triggerActionFilter[action.type])
					{
						return;
					}
					if (this._remainCooldownTime > 0f)
					{
						return;
					}
					this._topPoint = (this._topPoint + 1) % this._attackTimeHistory.Capacity;
					if (this._attackTimeHistory.Count < this._attackTimeHistory.Capacity)
					{
						this._attackTimeHistory.Add(Time.time);
					}
					if (this._attackTimeHistory.Count >= this._attackTimeHistory.Capacity)
					{
						this._bottomPoint = (this._bottomPoint + 1) % this._attackTimeHistory.Capacity;
						this._attackTimeHistory[this._topPoint] = Time.time;
						if (this._topPoint == this._bottomPoint)
						{
							return;
						}
						if (this._attackTimeHistory[this._topPoint] - this._attackTimeHistory[this._bottomPoint] > this.ability._triggerInterval)
						{
							return;
						}
						this._remainCooldownTime = this.ability._cooldownTime;
						this.owner.StartCoroutine(this.ability._onStep2Invoke.CRun(this.owner));
					}
				}

				// Token: 0x040026FA RID: 9978
				private List<float> _attackTimeHistory;

				// Token: 0x040026FB RID: 9979
				private int _topPoint = -1;

				// Token: 0x040026FC RID: 9980
				private int _bottomPoint = -1;

				// Token: 0x040026FD RID: 9981
				private float _remainCooldownTime;
			}
		}
	}
}
