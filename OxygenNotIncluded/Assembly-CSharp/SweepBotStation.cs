using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200069C RID: 1692
[AddComponentMenu("KMonoBehaviour/scripts/SweepBotStation")]
public class SweepBotStation : KMonoBehaviour
{
	// Token: 0x06002D9B RID: 11675 RVA: 0x000F1EE7 File Offset: 0x000F00E7
	public void SetStorages(Storage botMaterialStorage, Storage sweepStorage)
	{
		this.botMaterialStorage = botMaterialStorage;
		this.sweepStorage = sweepStorage;
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x000F1EF7 File Offset: 0x000F00F7
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
		base.Subscribe<SweepBotStation>(-592767678, SweepBotStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x000F1F11 File Offset: 0x000F0111
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, false);
	}

	// Token: 0x06002D9E RID: 11678 RVA: 0x000F1F2C File Offset: 0x000F012C
	protected override void OnSpawn()
	{
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		else
		{
			StorageUnloadMonitor.Instance smi = this.sweepBot.Get().GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			this.RefreshSweepBotSubscription();
		}
		this.UpdateMeter();
		this.UpdateNameDisplay();
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x000F1FEC File Offset: 0x000F01EC
	private void RequestNewSweepBot(object data = null)
	{
		if (this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS) == null)
		{
			FetchList2 fetchList = new FetchList2(this.botMaterialStorage, Db.Get().ChoreTypes.Fetch);
			fetchList.Add(GameTags.RefinedMetal, null, SweepBotConfig.MASS, Operational.State.None);
			fetchList.Submit(null, true);
			return;
		}
		this.MakeNewSweepBot(null);
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x000F2054 File Offset: 0x000F0254
	private void MakeNewSweepBot(object data = null)
	{
		if (this.newSweepyHandle.IsValid)
		{
			return;
		}
		if (this.botMaterialStorage.GetAmountAvailable(GameTags.RefinedMetal) < SweepBotConfig.MASS)
		{
			return;
		}
		PrimaryElement primaryElement = this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS);
		if (primaryElement == null)
		{
			return;
		}
		SimHashes sweepBotMaterial = primaryElement.ElementID;
		primaryElement.Mass -= SweepBotConfig.MASS;
		this.UpdateMeter();
		this.newSweepyHandle = GameScheduler.Instance.Schedule("MakeSweepy", 2f, delegate(object obj)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("SweepBot"), Grid.CellToPos(Grid.CellRight(Grid.PosToCell(this.gameObject))), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			this.sweepBot = new Ref<KSelectable>(gameObject.GetComponent<KSelectable>());
			if (!string.IsNullOrEmpty(this.storedName))
			{
				this.sweepBot.Get().GetComponent<UserNameable>().SetName(this.storedName);
			}
			this.UpdateNameDisplay();
			StorageUnloadMonitor.Instance smi = gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			this.sweepBot.Get().GetComponent<PrimaryElement>().ElementID = sweepBotMaterial;
			this.RefreshSweepBotSubscription();
			this.newSweepyHandle.ClearScheduler();
		}, null, null);
		base.GetComponent<KBatchedAnimController>().Play("newsweepy", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x000F2120 File Offset: 0x000F0320
	private void RefreshSweepBotSubscription()
	{
		if (this.refreshSweepbotHandle != -1)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
			this.sweepBot.Get().Unsubscribe(this.sweepBotNameChangeHandle);
		}
		this.refreshSweepbotHandle = this.sweepBot.Get().Subscribe(1969584890, new Action<object>(this.RequestNewSweepBot));
		this.sweepBotNameChangeHandle = this.sweepBot.Get().Subscribe(1102426921, new Action<object>(this.UpdateStoredName));
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x000F21B0 File Offset: 0x000F03B0
	private void UpdateStoredName(object data)
	{
		this.storedName = (string)data;
		this.UpdateNameDisplay();
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x000F21C4 File Offset: 0x000F03C4
	private void UpdateNameDisplay()
	{
		if (string.IsNullOrEmpty(this.storedName))
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, ROBOTS.MODELS.SWEEPBOT.NAME));
		}
		else
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, this.storedName));
		}
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x000F222F File Offset: 0x000F042F
	public void DockRobot(bool docked)
	{
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, docked);
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x000F2244 File Offset: 0x000F0444
	public void StartCharging()
	{
		base.GetComponent<KBatchedAnimController>().Queue("sleep_pre", KAnim.PlayMode.Once, 1f, 0f);
		base.GetComponent<KBatchedAnimController>().Queue("sleep_idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x000F2291 File Offset: 0x000F0491
	public void StopCharging()
	{
		base.GetComponent<KBatchedAnimController>().Play("sleep_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.UpdateNameDisplay();
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x000F22BC File Offset: 0x000F04BC
	protected override void OnCleanUp()
	{
		if (this.newSweepyHandle.IsValid)
		{
			this.newSweepyHandle.ClearScheduler();
		}
		if (this.refreshSweepbotHandle != -1 && this.sweepBot.Get() != null)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
		}
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x000F2314 File Offset: 0x000F0514
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x000F234C File Offset: 0x000F054C
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component.currentFrame >= component.GetCurrentNumFrames())
		{
			base.GetComponent<KBatchedAnimController>().Play("remove", KAnim.PlayMode.Once, 1f, 0f);
		}
		for (int i = 0; i < this.sweepStorage.Count; i++)
		{
			this.sweepStorage[i].GetComponent<Clearable>().MarkForClear(false, true);
		}
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x000F23E4 File Offset: 0x000F05E4
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		if (component.Flags.ContainsValue(false))
		{
			component.SetActive(false, false);
		}
		else
		{
			component.SetActive(true, false);
		}
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x000F243A File Offset: 0x000F063A
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.sweepStorage.Capacity() - this.sweepStorage.storageFullMargin;
	}

	// Token: 0x06002DAC RID: 11692 RVA: 0x000F2453 File Offset: 0x000F0653
	private float GetAmountStored()
	{
		return this.sweepStorage.MassStored();
	}

	// Token: 0x04001AEF RID: 6895
	[Serialize]
	public Ref<KSelectable> sweepBot;

	// Token: 0x04001AF0 RID: 6896
	[Serialize]
	public string storedName;

	// Token: 0x04001AF1 RID: 6897
	private static readonly Operational.Flag dockedRobot = new Operational.Flag("dockedRobot", Operational.Flag.Type.Functional);

	// Token: 0x04001AF2 RID: 6898
	private MeterController meter;

	// Token: 0x04001AF3 RID: 6899
	[SerializeField]
	private Storage botMaterialStorage;

	// Token: 0x04001AF4 RID: 6900
	[SerializeField]
	private Storage sweepStorage;

	// Token: 0x04001AF5 RID: 6901
	private SchedulerHandle newSweepyHandle;

	// Token: 0x04001AF6 RID: 6902
	private static readonly EventSystem.IntraObjectHandler<SweepBotStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SweepBotStation>(delegate(SweepBotStation component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001AF7 RID: 6903
	private int refreshSweepbotHandle = -1;

	// Token: 0x04001AF8 RID: 6904
	private int sweepBotNameChangeHandle = -1;
}
