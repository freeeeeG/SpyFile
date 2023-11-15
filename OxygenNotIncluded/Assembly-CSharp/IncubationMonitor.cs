using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000727 RID: 1831
public class IncubationMonitor : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>
{
	// Token: 0x0600325C RID: 12892 RVA: 0x0010B40C File Offset: 0x0010960C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubating;
		this.root.Enter(delegate(IncubationMonitor.Instance smi)
		{
			smi.OnOperationalChanged(null);
		}).Enter(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Add(smi);
		}).Exit(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Remove(smi);
		});
		this.incubating.PlayAnim("idle", KAnim.PlayMode.Loop).Transition(this.hatching_pre, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.IsReadyToHatch), UpdateRate.SIM_1000ms).TagTransition(GameTags.Entombed, this.entombed, false).ParamTransition<bool>(this.isSuppressed, this.suppressed, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsTrue).ToggleEffect((IncubationMonitor.Instance smi) => smi.incubatingEffect);
		this.entombed.TagTransition(GameTags.Entombed, this.incubating, true);
		this.suppressed.ToggleEffect((IncubationMonitor.Instance smi) => this.suppressedEffect).ParamTransition<bool>(this.isSuppressed, this.incubating, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsFalse).TagTransition(GameTags.Entombed, this.entombed, false).Transition(this.not_viable, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.NoLongerViable), UpdateRate.SIM_1000ms);
		this.hatching_pre.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DropSelfFromStorage)).PlayAnim("hatching_pre").OnAnimQueueComplete(this.hatching_pst);
		this.hatching_pst.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnBaby)).PlayAnim("hatching_pst").OnAnimQueueComplete(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.not_viable.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnGenericEgg)).GoTo(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.suppressedEffect = new Effect("IncubationSuppressed", CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.suppressedEffect.Add(new AttributeModifier(Db.Get().Amounts.Viability.deltaAttribute.Id, -0.016666668f, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, false, false, true));
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x0010B685 File Offset: 0x00109885
	private static bool IsReadyToHatch(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.incubation.value >= smi.incubation.GetMax();
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x0010B6B8 File Offset: 0x001098B8
	private static void SpawnBaby(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.spawnedCreature), position);
		gameObject.SetActive(true);
		gameObject.GetSMI<AnimInterruptMonitor.Instance>().Play("hatching_pst", KAnim.PlayMode.Once);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		Db.Get().Amounts.Wildness.Copy(gameObject, smi.gameObject);
		if (smi.incubator != null)
		{
			smi.incubator.StoreBaby(gameObject);
		}
		IncubationMonitor.SpawnShell(smi);
		SaveLoader.Instance.saveManager.Unregister(smi.GetComponent<SaveLoadRoot>());
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x0010B7AD File Offset: 0x001099AD
	private static bool NoLongerViable(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.viability.value <= smi.viability.GetMin();
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x0010B7E0 File Offset: 0x001099E0
	private static GameObject SpawnShell(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EggShell"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x0010B834 File Offset: 0x00109A34
	private static GameObject SpawnEggInnards(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RawEgg"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x0010B888 File Offset: 0x00109A88
	private static void SpawnGenericEgg(IncubationMonitor.Instance smi)
	{
		IncubationMonitor.SpawnShell(smi);
		GameObject gameObject = IncubationMonitor.SpawnEggInnards(smi);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06003263 RID: 12899 RVA: 0x0010B8F1 File Offset: 0x00109AF1
	private static void DeleteSelf(IncubationMonitor.Instance smi)
	{
		smi.gameObject.DeleteObject();
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x0010B900 File Offset: 0x00109B00
	private static void DropSelfFromStorage(IncubationMonitor.Instance smi)
	{
		if (!smi.sm.inIncubator.Get(smi))
		{
			Storage storage = smi.GetStorage();
			if (storage)
			{
				storage.Drop(smi.gameObject, true);
			}
			smi.gameObject.AddTag(GameTags.StoredPrivate);
		}
	}

	// Token: 0x04001E35 RID: 7733
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter incubatorIsActive;

	// Token: 0x04001E36 RID: 7734
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter inIncubator;

	// Token: 0x04001E37 RID: 7735
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter isSuppressed;

	// Token: 0x04001E38 RID: 7736
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State incubating;

	// Token: 0x04001E39 RID: 7737
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State entombed;

	// Token: 0x04001E3A RID: 7738
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State suppressed;

	// Token: 0x04001E3B RID: 7739
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pre;

	// Token: 0x04001E3C RID: 7740
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pst;

	// Token: 0x04001E3D RID: 7741
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State not_viable;

	// Token: 0x04001E3E RID: 7742
	private Effect suppressedEffect;

	// Token: 0x020014A8 RID: 5288
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600856E RID: 34158 RVA: 0x00305C24 File Offset: 0x00303E24
		public override void Configure(GameObject prefab)
		{
			List<string> initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
			initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
			initialAmounts.Add(Db.Get().Amounts.Incubation.Id);
			initialAmounts.Add(Db.Get().Amounts.Viability.Id);
		}

		// Token: 0x04006607 RID: 26119
		public float baseIncubationRate;

		// Token: 0x04006608 RID: 26120
		public Tag spawnedCreature;
	}

	// Token: 0x020014A9 RID: 5289
	public new class Instance : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.GameInstance
	{
		// Token: 0x06008570 RID: 34160 RVA: 0x00305C94 File Offset: 0x00303E94
		public Instance(IStateMachineTarget master, IncubationMonitor.Def def) : base(master, def)
		{
			this.incubation = Db.Get().Amounts.Incubation.Lookup(base.gameObject);
			Action<object> handler = new Action<object>(this.OnStore);
			master.Subscribe(856640610, handler);
			master.Subscribe(1309017699, handler);
			Action<object> handler2 = new Action<object>(this.OnOperationalChanged);
			master.Subscribe(1628751838, handler2);
			master.Subscribe(960378201, handler2);
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
			this.viability = Db.Get().Amounts.Viability.Lookup(base.gameObject);
			this.viability.value = this.viability.GetMax();
			float value = def.baseIncubationRate;
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				value = 33.333332f;
			}
			AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_INCUBATION_RATE.NAME, false, false, true);
			this.incubatingEffect = new Effect("Incubating", CREATURES.MODIFIERS.INCUBATING.NAME, CREATURES.MODIFIERS.INCUBATING.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
			this.incubatingEffect.Add(modifier);
		}

		// Token: 0x06008571 RID: 34161 RVA: 0x00305E12 File Offset: 0x00304012
		public Storage GetStorage()
		{
			if (!(base.transform.parent != null))
			{
				return null;
			}
			return base.transform.parent.GetComponent<Storage>();
		}

		// Token: 0x06008572 RID: 34162 RVA: 0x00305E3C File Offset: 0x0030403C
		public void OnStore(object data)
		{
			Storage storage = data as Storage;
			bool stored = storage || (data != null && (bool)data);
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06008573 RID: 34163 RVA: 0x00305E84 File Offset: 0x00304084
		public void OnOperationalChanged(object data = null)
		{
			bool stored = base.gameObject.HasTag(GameTags.Stored);
			Storage storage = this.GetStorage();
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06008574 RID: 34164 RVA: 0x00305EC4 File Offset: 0x003040C4
		private void UpdateIncubationState(bool stored, EggIncubator incubator)
		{
			this.incubator = incubator;
			base.smi.sm.inIncubator.Set(incubator != null, base.smi, false);
			bool value = stored && !incubator;
			base.smi.sm.isSuppressed.Set(value, base.smi, false);
			Operational operational = incubator ? incubator.GetComponent<Operational>() : null;
			bool value2 = incubator && (operational == null || operational.IsOperational);
			base.smi.sm.incubatorIsActive.Set(value2, base.smi, false);
		}

		// Token: 0x06008575 RID: 34165 RVA: 0x00305F78 File Offset: 0x00304178
		public void ApplySongBuff()
		{
			base.GetComponent<Effects>().Add("EggSong", true);
		}

		// Token: 0x06008576 RID: 34166 RVA: 0x00305F8C File Offset: 0x0030418C
		public bool HasSongBuff()
		{
			return base.GetComponent<Effects>().HasEffect("EggSong");
		}

		// Token: 0x04006609 RID: 26121
		public AmountInstance incubation;

		// Token: 0x0400660A RID: 26122
		public AmountInstance wildness;

		// Token: 0x0400660B RID: 26123
		public AmountInstance viability;

		// Token: 0x0400660C RID: 26124
		public EggIncubator incubator;

		// Token: 0x0400660D RID: 26125
		public Effect incubatingEffect;
	}
}
