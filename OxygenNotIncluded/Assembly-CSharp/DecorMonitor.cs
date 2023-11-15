using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000874 RID: 2164
public class DecorMonitor : GameStateMachine<DecorMonitor, DecorMonitor.Instance>
{
	// Token: 0x06003F38 RID: 16184 RVA: 0x00160E90 File Offset: 0x0015F090
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleAttributeModifier("DecorSmoother", (DecorMonitor.Instance smi) => smi.GetDecorModifier(), (DecorMonitor.Instance smi) => true).Update("DecorSensing", delegate(DecorMonitor.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.NewDay, (DecorMonitor.Instance smi) => GameClock.Instance, delegate(DecorMonitor.Instance smi)
		{
			smi.OnNewDay();
		});
	}

	// Token: 0x040028F6 RID: 10486
	public static float MAXIMUM_DECOR_VALUE = 120f;

	// Token: 0x02001668 RID: 5736
	public new class Instance : GameStateMachine<DecorMonitor, DecorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008AA6 RID: 35494 RVA: 0x00314458 File Offset: 0x00312658
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.cycleTotalDecor = 2250f;
			this.amount = Db.Get().Amounts.Decor.Lookup(base.gameObject);
			this.modifier = new AttributeModifier(Db.Get().Amounts.Decor.deltaAttribute.Id, 1f, DUPLICANTS.NEEDS.DECOR.OBSERVED_DECOR, false, false, false);
		}

		// Token: 0x06008AA7 RID: 35495 RVA: 0x00314589 File Offset: 0x00312789
		public AttributeModifier GetDecorModifier()
		{
			return this.modifier;
		}

		// Token: 0x06008AA8 RID: 35496 RVA: 0x00314594 File Offset: 0x00312794
		public void Update(float dt)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell))
			{
				return;
			}
			float decorAtCell = GameUtil.GetDecorAtCell(cell);
			this.cycleTotalDecor += decorAtCell * dt;
			float value = 0f;
			float num = 4.1666665f;
			if (Mathf.Abs(decorAtCell - this.amount.value) > 0.5f)
			{
				if (decorAtCell > this.amount.value)
				{
					value = 3f * num;
				}
				else if (decorAtCell < this.amount.value)
				{
					value = -num;
				}
			}
			else
			{
				this.amount.value = decorAtCell;
			}
			this.modifier.SetValue(value);
		}

		// Token: 0x06008AA9 RID: 35497 RVA: 0x00314638 File Offset: 0x00312838
		public void OnNewDay()
		{
			this.yesterdaysTotalDecor = this.cycleTotalDecor;
			this.cycleTotalDecor = 0f;
			float totalValue = base.gameObject.GetAttributes().Add(Db.Get().Attributes.DecorExpectation).GetTotalValue();
			float num = this.yesterdaysTotalDecor / 600f;
			num += totalValue;
			Effects component = base.gameObject.GetComponent<Effects>();
			foreach (KeyValuePair<float, string> keyValuePair in this.effectLookup)
			{
				if (num < keyValuePair.Key)
				{
					component.Add(keyValuePair.Value, true);
					break;
				}
			}
		}

		// Token: 0x06008AAA RID: 35498 RVA: 0x003146FC File Offset: 0x003128FC
		public float GetTodaysAverageDecor()
		{
			return this.cycleTotalDecor / (GameClock.Instance.GetCurrentCycleAsPercentage() * 600f);
		}

		// Token: 0x06008AAB RID: 35499 RVA: 0x00314715 File Offset: 0x00312915
		public float GetYesterdaysAverageDecor()
		{
			return this.yesterdaysTotalDecor / 600f;
		}

		// Token: 0x04006B86 RID: 27526
		[Serialize]
		private float cycleTotalDecor;

		// Token: 0x04006B87 RID: 27527
		[Serialize]
		private float yesterdaysTotalDecor;

		// Token: 0x04006B88 RID: 27528
		private AmountInstance amount;

		// Token: 0x04006B89 RID: 27529
		private AttributeModifier modifier;

		// Token: 0x04006B8A RID: 27530
		private List<KeyValuePair<float, string>> effectLookup = new List<KeyValuePair<float, string>>
		{
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * -0.25f, "DecorMinus1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0f, "Decor0"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.25f, "Decor1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.5f, "Decor2"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.75f, "Decor3"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE, "Decor4"),
			new KeyValuePair<float, string>(float.MaxValue, "Decor5")
		};
	}
}
