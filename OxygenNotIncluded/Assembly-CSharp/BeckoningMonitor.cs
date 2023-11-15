using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000709 RID: 1801
public class BeckoningMonitor : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
	// Token: 0x06003188 RID: 12680 RVA: 0x00107748 File Offset: 0x00105948
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeckoningMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (BeckoningMonitor.Instance smi) => smi.IsReadyToBeckon(), null).Update(delegate(BeckoningMonitor.Instance smi, float dt)
		{
			smi.UpdateBlockedStatusItem();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001460 RID: 5216
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008472 RID: 33906 RVA: 0x0030299F File Offset: 0x00300B9F
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
		}

		// Token: 0x0400654A RID: 25930
		public float caloriesPerCycle;

		// Token: 0x0400654B RID: 25931
		public string effectId = "MooWellFed";
	}

	// Token: 0x02001461 RID: 5217
	public new class Instance : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameInstance
	{
		// Token: 0x06008474 RID: 33908 RVA: 0x003029D8 File Offset: 0x00300BD8
		public Instance(IStateMachineTarget master, BeckoningMonitor.Def def) : base(master, def)
		{
			this.beckoning = Db.Get().Amounts.Beckoning.Lookup(base.gameObject);
		}

		// Token: 0x06008475 RID: 33909 RVA: 0x00302A04 File Offset: 0x00300C04
		private bool IsSpaceVisible()
		{
			int num = Grid.PosToCell(this);
			return Grid.IsValidCell(num) && Grid.ExposedToSunlight[num] > 0;
		}

		// Token: 0x06008476 RID: 33910 RVA: 0x00302A30 File Offset: 0x00300C30
		private bool IsBeckoningAvailable()
		{
			return base.smi.beckoning.value >= base.smi.beckoning.GetMax();
		}

		// Token: 0x06008477 RID: 33911 RVA: 0x00302A57 File Offset: 0x00300C57
		public bool IsReadyToBeckon()
		{
			return this.IsBeckoningAvailable() && this.IsSpaceVisible();
		}

		// Token: 0x06008478 RID: 33912 RVA: 0x00302A6C File Offset: 0x00300C6C
		public void UpdateBlockedStatusItem()
		{
			bool flag = this.IsSpaceVisible();
			if (!flag && this.IsBeckoningAvailable() && this.beckoningBlockedHandle == Guid.Empty)
			{
				this.beckoningBlockedHandle = this.kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked, null);
				return;
			}
			if (flag)
			{
				this.beckoningBlockedHandle = this.kselectable.RemoveStatusItem(this.beckoningBlockedHandle, false);
			}
		}

		// Token: 0x06008479 RID: 33913 RVA: 0x00302ADC File Offset: 0x00300CDC
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x0400654C RID: 25932
		private AmountInstance beckoning;

		// Token: 0x0400654D RID: 25933
		[MyCmpGet]
		private Effects effects;

		// Token: 0x0400654E RID: 25934
		[MyCmpGet]
		public KSelectable kselectable;

		// Token: 0x0400654F RID: 25935
		private Guid beckoningBlockedHandle;
	}
}
