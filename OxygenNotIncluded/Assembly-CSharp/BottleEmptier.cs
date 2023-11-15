using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005A0 RID: 1440
[SerializationConfig(MemberSerialization.OptIn)]
public class BottleEmptier : StateMachineComponent<BottleEmptier.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600231E RID: 8990 RVA: 0x000C05B3 File Offset: 0x000BE7B3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.Subscribe<BottleEmptier>(493375141, BottleEmptier.OnRefreshUserMenuDelegate);
		base.Subscribe<BottleEmptier>(-905833192, BottleEmptier.OnCopySettingsDelegate);
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000C05E8 File Offset: 0x000BE7E8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000C05EB File Offset: 0x000BE7EB
	private void OnChangeAllowManualPumpingStationFetching()
	{
		this.allowManualPumpingStationFetching = !this.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000C0608 File Offset: 0x000BE808
	private void OnRefreshUserMenu(object data)
	{
		if (this.isGasEmptier)
		{
			KIconButtonMenu.ButtonInfo button = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.TOOLTIP, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 0.4f);
			return;
		}
		KIconButtonMenu.ButtonInfo button2 = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button2, 0.4f);
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000C073C File Offset: 0x000BE93C
	private void OnCopySettings(object data)
	{
		BottleEmptier component = ((GameObject)data).GetComponent<BottleEmptier>();
		this.allowManualPumpingStationFetching = component.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x04001410 RID: 5136
	public float emptyRate = 10f;

	// Token: 0x04001411 RID: 5137
	[Serialize]
	public bool allowManualPumpingStationFetching;

	// Token: 0x04001412 RID: 5138
	public bool isGasEmptier;

	// Token: 0x04001413 RID: 5139
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001414 RID: 5140
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200122D RID: 4653
	public class StatesInstance : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.GameInstance
	{
		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06007C16 RID: 31766 RVA: 0x002DFD0F File Offset: 0x002DDF0F
		// (set) Token: 0x06007C17 RID: 31767 RVA: 0x002DFD17 File Offset: 0x002DDF17
		public MeterController meter { get; private set; }

		// Token: 0x06007C18 RID: 31768 RVA: 0x002DFD20 File Offset: 0x002DDF20
		public StatesInstance(BottleEmptier smi) : base(smi)
		{
			TreeFilterable component = base.master.GetComponent<TreeFilterable>();
			component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_arrow",
				"meter_scale"
			});
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
			base.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x002DFDCC File Offset: 0x002DDFCC
		public void CreateChore()
		{
			HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
			Tag[] forbidden_tags;
			if (!base.master.allowManualPumpingStationFetching)
			{
				forbidden_tags = new Tag[]
				{
					GameTags.LiquidSource,
					GameTags.GasSource
				};
			}
			else
			{
				forbidden_tags = new Tag[0];
			}
			Storage component = base.GetComponent<Storage>();
			this.chore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, component, component.Capacity(), tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, null, true, null, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x06007C1A RID: 31770 RVA: 0x002DFE51 File Offset: 0x002DE051
		public void CancelChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Storage Changed");
				this.chore = null;
			}
		}

		// Token: 0x06007C1B RID: 31771 RVA: 0x002DFE72 File Offset: 0x002DE072
		public void RefreshChore()
		{
			this.GoTo(base.sm.unoperational);
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x002DFE85 File Offset: 0x002DE085
		private void OnFilterChanged(HashSet<Tag> tags)
		{
			this.RefreshChore();
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x002DFE90 File Offset: 0x002DE090
		private void OnStorageChange(object data)
		{
			Storage component = base.GetComponent<Storage>();
			this.meter.SetPositionPercent(Mathf.Clamp01(component.RemainingCapacity() / component.capacityKg));
		}

		// Token: 0x06007C1E RID: 31774 RVA: 0x002DFEC1 File Offset: 0x002DE0C1
		private void OnOnlyFetchMarkedItemsSettingChanged(object data)
		{
			this.RefreshChore();
		}

		// Token: 0x06007C1F RID: 31775 RVA: 0x002DFECC File Offset: 0x002DE0CC
		public void StartMeter()
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), firstPrimaryElement.Element.substance.colour);
			this.meter.SetSymbolTint(new KAnimHashedString("water1"), firstPrimaryElement.Element.substance.colour);
			base.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("leak_ceiling"), firstPrimaryElement.Element.substance.colour);
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x002DFF60 File Offset: 0x002DE160
		private PrimaryElement GetFirstPrimaryElement()
		{
			Storage component = base.GetComponent<Storage>();
			for (int i = 0; i < component.Count; i++)
			{
				GameObject gameObject = component[i];
				if (!(gameObject == null))
				{
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					if (!(component2 == null))
					{
						return component2;
					}
				}
			}
			return null;
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x002DFFAC File Offset: 0x002DE1AC
		public void Emit(float dt)
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			Storage component = base.GetComponent<Storage>();
			float num = Mathf.Min(firstPrimaryElement.Mass, base.master.emptyRate * dt);
			if (num <= 0f)
			{
				return;
			}
			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			component.ConsumeAndGetDisease(prefabTag, num, out num2, out diseaseInfo, out temperature);
			Vector3 position = base.transform.GetPosition();
			position.y += 1.8f;
			bool flag = base.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
			position.x += (flag ? -0.2f : 0.2f);
			int num3 = Grid.PosToCell(position) + (flag ? -1 : 1);
			if (Grid.Solid[num3])
			{
				num3 += (flag ? 1 : -1);
			}
			Element element = firstPrimaryElement.Element;
			ushort idx = element.idx;
			if (element.IsLiquid)
			{
				FallingWater.instance.AddParticle(num3, idx, num2, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
				return;
			}
			SimMessages.ModifyCell(num3, idx, temperature, num2, diseaseInfo.idx, diseaseInfo.count, SimMessages.ReplaceType.None, false, -1);
		}

		// Token: 0x04005EB1 RID: 24241
		private FetchChore chore;
	}

	// Token: 0x0200122E RID: 4654
	public class States : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier>
	{
		// Token: 0x06007C22 RID: 31778 RVA: 0x002E00DC File Offset: 0x002DE2DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waitingfordelivery;
			this.statusItem = new StatusItem("BottleEmptier", "", "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveStringCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.NAME;
				}
				return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.NAME;
			};
			this.statusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					if (bottleEmptier.isGasEmptier)
					{
						return BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.TOOLTIP;
					}
					return BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.TOOLTIP;
				}
				else
				{
					if (bottleEmptier.isGasEmptier)
					{
						return BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.TOOLTIP;
					}
					return BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.TOOLTIP;
				}
			};
			this.root.ToggleStatusItem(this.statusItem, (BottleEmptier.StatesInstance smi) => smi.master);
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.emptying, (BottleEmptier.StatesInstance smi) => !smi.GetComponent<Storage>().IsEmpty()).Enter("CreateChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CreateChore();
			}).Exit("CancelChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CancelChore();
			}).PlayAnim("on");
			this.emptying.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().IsEmpty()).Enter("StartMeter", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.StartMeter();
			}).Update("Emit", delegate(BottleEmptier.StatesInstance smi, float dt)
			{
				smi.Emit(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x04005EB3 RID: 24243
		private StatusItem statusItem;

		// Token: 0x04005EB4 RID: 24244
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State unoperational;

		// Token: 0x04005EB5 RID: 24245
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State waitingfordelivery;

		// Token: 0x04005EB6 RID: 24246
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State emptying;
	}
}
