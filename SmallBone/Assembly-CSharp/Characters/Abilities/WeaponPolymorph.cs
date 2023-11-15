using System;
using Characters.Actions;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000ACA RID: 2762
	[Serializable]
	public class WeaponPolymorph : Ability
	{
		// Token: 0x060038B9 RID: 14521 RVA: 0x000A7348 File Offset: 0x000A5548
		public override void Initialize()
		{
			base.Initialize();
			this._exitingOperation.Initialize();
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x000A735B File Offset: 0x000A555B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new WeaponPolymorph.Instance(owner, this);
		}

		// Token: 0x04002D1E RID: 11550
		[SerializeField]
		[Header("Gauge")]
		private ValueGauge _gauge;

		// Token: 0x04002D1F RID: 11551
		[SerializeField]
		private float _gaugeLosingAmountPerSecond;

		// Token: 0x04002D20 RID: 11552
		[SerializeField]
		[Tooltip("비워두면 없는 걸로 처리됨")]
		[Header("Actions")]
		private Characters.Actions.Action _enteringAction;

		// Token: 0x04002D21 RID: 11553
		[SerializeField]
		[Header("Stop Condition")]
		private float _minGaugeValue;

		// Token: 0x04002D22 RID: 11554
		[CharacterOperation.SubcomponentAttribute]
		[Space]
		[SerializeField]
		private CharacterOperation.Subcomponents _exitingOperation;

		// Token: 0x02000ACB RID: 2763
		public class Instance : AbilityInstance<WeaponPolymorph>
		{
			// Token: 0x17000BCC RID: 3020
			// (get) Token: 0x060038BC RID: 14524 RVA: 0x00018EC5 File Offset: 0x000170C5
			public override int iconStacks
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000BCD RID: 3021
			// (get) Token: 0x060038BD RID: 14525 RVA: 0x00071719 File Offset: 0x0006F919
			public override float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x060038BE RID: 14526 RVA: 0x000A7364 File Offset: 0x000A5564
			public Instance(Character owner, WeaponPolymorph ability) : base(owner, ability)
			{
			}

			// Token: 0x060038BF RID: 14527 RVA: 0x000A7370 File Offset: 0x000A5570
			protected override void OnAttach()
			{
				this.ability._gauge.Add(this.ability._gauge.maxValue);
				if (this.ability._enteringAction != null)
				{
					this.ability._enteringAction.TryStart();
				}
			}

			// Token: 0x060038C0 RID: 14528 RVA: 0x000A73C1 File Offset: 0x000A55C1
			protected override void OnDetach()
			{
				this.ability._exitingOperation.Run(this.owner);
			}

			// Token: 0x060038C1 RID: 14529 RVA: 0x000A73DC File Offset: 0x000A55DC
			public override void UpdateTime(float deltaTime)
			{
				this.ability._gauge.Add(-this.ability._gaugeLosingAmountPerSecond * deltaTime);
				if (this.ability._gauge.currentValue > this.ability._minGaugeValue)
				{
					return;
				}
				this.owner.playerComponents.inventory.weapon.Unpolymorph();
			}
		}
	}
}
