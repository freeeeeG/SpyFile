using System;
using Klei.AI;
using STRINGS;

// Token: 0x0200072D RID: 1837
public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
	// Token: 0x06003278 RID: 12920 RVA: 0x0010BF5E File Offset: 0x0010A15E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState), UpdateRate.SIM_1000ms, true);
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x0010BF84 File Offset: 0x0010A184
	private static bool IsConfined(OvercrowdingMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.Burrowed) && !smi.HasTag(GameTags.Creatures.Digger) && (smi.cavity == null || smi.cavity.numCells < smi.def.spaceRequiredPerCreature);
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x0010BFD4 File Offset: 0x0010A1D4
	private static bool IsFutureOvercrowded(OvercrowdingMonitor.Instance smi)
	{
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return false;
		}
		if (smi.cavity != null)
		{
			int num = smi.cavity.creatures.Count + smi.cavity.eggs.Count;
			return num != 0 && smi.cavity.eggs.Count != 0 && smi.cavity.numCells / num < smi.def.spaceRequiredPerCreature;
		}
		return false;
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x0010C050 File Offset: 0x0010A250
	private static int CalculateOvercrowdedModifer(OvercrowdingMonitor.Instance smi)
	{
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return 0;
		}
		if (smi.cavity == null)
		{
			return 0;
		}
		FishOvercrowdingMonitor.Instance smi2 = smi.GetSMI<FishOvercrowdingMonitor.Instance>();
		if (smi2 != null)
		{
			int fishCount = smi2.fishCount;
			if (fishCount <= 0)
			{
				return 0;
			}
			int num = smi2.cellCount / smi.def.spaceRequiredPerCreature;
			if (num < smi.cavity.creatures.Count)
			{
				return -5 - (fishCount - (num + 1));
			}
			return 0;
		}
		else
		{
			if (smi.cavity.creatures.Count <= 1)
			{
				return 0;
			}
			int num2 = smi.cavity.numCells / smi.def.spaceRequiredPerCreature;
			if (num2 < smi.cavity.creatures.Count)
			{
				return -5 - (smi.cavity.creatures.Count - (num2 + 1));
			}
			return 0;
		}
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x0010C118 File Offset: 0x0010A318
	private static bool IsOvercrowded(OvercrowdingMonitor.Instance smi)
	{
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return false;
		}
		FishOvercrowdingMonitor.Instance smi2 = smi.GetSMI<FishOvercrowdingMonitor.Instance>();
		if (smi2 == null)
		{
			return smi.cavity != null && smi.cavity.creatures.Count > 1 && smi.cavity.numCells / smi.cavity.creatures.Count < smi.def.spaceRequiredPerCreature;
		}
		int fishCount = smi2.fishCount;
		if (fishCount > 0)
		{
			return smi2.cellCount / fishCount < smi.def.spaceRequiredPerCreature;
		}
		int cell = Grid.PosToCell(smi);
		return Grid.IsValidCell(cell) && !Grid.IsLiquid(cell);
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x0010C1C0 File Offset: 0x0010A3C0
	private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
	{
		OvercrowdingMonitor.UpdateCavity(smi, dt);
		bool flag = OvercrowdingMonitor.IsConfined(smi);
		bool flag2 = OvercrowdingMonitor.IsOvercrowded(smi);
		bool flag3 = !smi.isBaby && OvercrowdingMonitor.IsFutureOvercrowded(smi);
		KPrefabID component = smi.gameObject.GetComponent<KPrefabID>();
		Effect effect = smi.isFish ? smi.fishOvercrowdedEffect : smi.overcrowdedEffect;
		component.SetTag(GameTags.Creatures.Confined, flag);
		component.SetTag(GameTags.Creatures.Overcrowded, flag2);
		component.SetTag(GameTags.Creatures.Expecting, flag3);
		if (!smi.isFish)
		{
			smi.overcrowdedModifier.SetValue((float)OvercrowdingMonitor.CalculateOvercrowdedModifer(smi));
		}
		else
		{
			smi.fishOvercrowdedModifier.SetValue((float)OvercrowdingMonitor.CalculateOvercrowdedModifer(smi));
		}
		OvercrowdingMonitor.SetEffect(smi, smi.stuckEffect, flag);
		OvercrowdingMonitor.SetEffect(smi, effect, !flag && flag2);
		OvercrowdingMonitor.SetEffect(smi, smi.futureOvercrowdedEffect, !flag && flag3);
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x0010C294 File Offset: 0x0010A494
	private static void SetEffect(OvercrowdingMonitor.Instance smi, Effect effect, bool set)
	{
		Effects component = smi.GetComponent<Effects>();
		if (set)
		{
			component.Add(effect, false);
			return;
		}
		component.Remove(effect);
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x0010C2BC File Offset: 0x0010A4BC
	private static void UpdateCavity(OvercrowdingMonitor.Instance smi, float dt)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
		if (cavityForCell != smi.cavity)
		{
			KPrefabID component = smi.GetComponent<KPrefabID>();
			if (smi.cavity != null)
			{
				if (smi.HasTag(GameTags.Egg))
				{
					smi.cavity.RemoveFromCavity(component, smi.cavity.eggs);
				}
				else
				{
					smi.cavity.RemoveFromCavity(component, smi.cavity.creatures);
				}
				Game.Instance.roomProber.UpdateRoom(cavityForCell);
			}
			smi.cavity = cavityForCell;
			if (smi.cavity != null)
			{
				if (smi.HasTag(GameTags.Egg))
				{
					smi.cavity.eggs.Add(component);
				}
				else
				{
					smi.cavity.creatures.Add(component);
				}
				Game.Instance.roomProber.UpdateRoom(smi.cavity);
			}
		}
	}

	// Token: 0x04001E4E RID: 7758
	public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;

	// Token: 0x020014B7 RID: 5303
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006637 RID: 26167
		public int spaceRequiredPerCreature;
	}

	// Token: 0x020014B8 RID: 5304
	public new class Instance : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x060085B6 RID: 34230 RVA: 0x00306B6C File Offset: 0x00304D6C
		public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def) : base(master, def)
		{
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
			FishOvercrowdingMonitor.Def def3 = master.gameObject.GetDef<FishOvercrowdingMonitor.Def>();
			this.isFish = (def3 != null);
			this.futureOvercrowdedEffect = new Effect("FutureOvercrowded", CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.futureOvercrowdedEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, true, false, true));
			this.overcrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.overcrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, false);
			this.overcrowdedEffect.Add(this.overcrowdedModifier);
			this.fishOvercrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.FISHTOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.fishOvercrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, false);
			this.fishOvercrowdedEffect.Add(this.fishOvercrowdedModifier);
			this.stuckEffect = new Effect("Confined", CREATURES.MODIFIERS.CONFINED.NAME, CREATURES.MODIFIERS.CONFINED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.stuckEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.CONFINED.NAME, false, false, true));
			this.stuckEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.CONFINED.NAME, true, false, true));
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x060085B7 RID: 34231 RVA: 0x00306DDC File Offset: 0x00304FDC
		protected override void OnCleanUp()
		{
			if (this.cavity == null)
			{
				return;
			}
			KPrefabID component = base.master.GetComponent<KPrefabID>();
			if (base.HasTag(GameTags.Egg))
			{
				this.cavity.RemoveFromCavity(component, this.cavity.eggs);
				return;
			}
			this.cavity.RemoveFromCavity(component, this.cavity.creatures);
		}

		// Token: 0x060085B8 RID: 34232 RVA: 0x00306E3A File Offset: 0x0030503A
		public void RoomRefreshUpdateCavity()
		{
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x04006638 RID: 26168
		public CavityInfo cavity;

		// Token: 0x04006639 RID: 26169
		public bool isBaby;

		// Token: 0x0400663A RID: 26170
		public bool isFish;

		// Token: 0x0400663B RID: 26171
		public Effect futureOvercrowdedEffect;

		// Token: 0x0400663C RID: 26172
		public Effect overcrowdedEffect;

		// Token: 0x0400663D RID: 26173
		public AttributeModifier overcrowdedModifier;

		// Token: 0x0400663E RID: 26174
		public Effect fishOvercrowdedEffect;

		// Token: 0x0400663F RID: 26175
		public AttributeModifier fishOvercrowdedModifier;

		// Token: 0x04006640 RID: 26176
		public Effect stuckEffect;
	}
}
