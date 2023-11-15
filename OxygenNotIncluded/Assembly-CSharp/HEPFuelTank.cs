using System;
using STRINGS;
using UnityEngine;

// Token: 0x020007F1 RID: 2033
public class HEPFuelTank : KMonoBehaviour, IFuelTank, IUserControlledCapacity
{
	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x060039C9 RID: 14793 RVA: 0x00142648 File Offset: 0x00140848
	public IStorage Storage
	{
		get
		{
			return this.hepStorage;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x060039CA RID: 14794 RVA: 0x00142650 File Offset: 0x00140850
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x060039CB RID: 14795 RVA: 0x00142658 File Offset: 0x00140858
	public void DEBUG_FillTank()
	{
		this.hepStorage.Store(this.hepStorage.RemainingCapacity());
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x060039CC RID: 14796 RVA: 0x00142671 File Offset: 0x00140871
	// (set) Token: 0x060039CD RID: 14797 RVA: 0x0014267E File Offset: 0x0014087E
	public float UserMaxCapacity
	{
		get
		{
			return this.hepStorage.capacity;
		}
		set
		{
			this.hepStorage.capacity = value;
			base.Trigger(-795826715, this);
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x060039CE RID: 14798 RVA: 0x00142698 File Offset: 0x00140898
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x060039CF RID: 14799 RVA: 0x0014269F File Offset: 0x0014089F
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x060039D0 RID: 14800 RVA: 0x001426A7 File Offset: 0x001408A7
	public float AmountStored
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x060039D1 RID: 14801 RVA: 0x001426B4 File Offset: 0x001408B4
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x060039D2 RID: 14802 RVA: 0x001426B7 File Offset: 0x001408B7
	public LocString CapacityUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x001426C0 File Offset: 0x001408C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		this.m_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.m_meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<HEPFuelTank>(-795826715, HEPFuelTank.OnStorageChangedDelegate);
		base.Subscribe<HEPFuelTank>(-1837862626, HEPFuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x00142769 File Offset: 0x00140969
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HEPFuelTank>(-905833192, HEPFuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x00142782 File Offset: 0x00140982
	private void OnStorageChange(object data)
	{
		this.m_meter.SetPositionPercent(this.hepStorage.Particles / Mathf.Max(1f, this.hepStorage.capacity));
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x001427B0 File Offset: 0x001409B0
	private void OnCopySettings(object data)
	{
		HEPFuelTank component = ((GameObject)data).GetComponent<HEPFuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x04002682 RID: 9858
	[MyCmpReq]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04002683 RID: 9859
	public float physicalFuelCapacity;

	// Token: 0x04002684 RID: 9860
	private MeterController m_meter;

	// Token: 0x04002685 RID: 9861
	public bool consumeFuelOnLand;

	// Token: 0x04002686 RID: 9862
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002687 RID: 9863
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002688 RID: 9864
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnCopySettings(data);
	});
}
