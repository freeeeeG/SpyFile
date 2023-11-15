using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200097A RID: 2426
[AddComponentMenu("KMonoBehaviour/scripts/SmartReservoir")]
public class SmartReservoir : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x0600476F RID: 18287 RVA: 0x0019382E File Offset: 0x00191A2E
	public float PercentFull
	{
		get
		{
			return this.storage.MassStored() / this.storage.Capacity();
		}
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x00193847 File Offset: 0x00191A47
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SmartReservoir>(-801688580, SmartReservoir.OnLogicValueChangedDelegate);
		base.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCircuitDelegate);
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x00193871 File Offset: 0x00191A71
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SmartReservoir>(-905833192, SmartReservoir.OnCopySettingsDelegate);
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x0019388A File Offset: 0x00191A8A
	public void Sim200ms(float dt)
	{
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x00193894 File Offset: 0x00191A94
	private void UpdateLogicCircuit(object data)
	{
		float num = this.PercentFull * 100f;
		if (this.activated)
		{
			if (num >= (float)this.deactivateValue)
			{
				this.activated = false;
			}
		}
		else if (num <= (float)this.activateValue)
		{
			this.activated = true;
		}
		bool flag = this.activated;
		this.logicPorts.SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x001938F8 File Offset: 0x00191AF8
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == SmartReservoir.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x00193930 File Offset: 0x00191B30
	private void OnCopySettings(object data)
	{
		SmartReservoir component = ((GameObject)data).GetComponent<SmartReservoir>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x0019396A File Offset: 0x00191B6A
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06004777 RID: 18295 RVA: 0x0019398E File Offset: 0x00191B8E
	// (set) Token: 0x06004778 RID: 18296 RVA: 0x00193997 File Offset: 0x00191B97
	public float ActivateValue
	{
		get
		{
			return (float)this.deactivateValue;
		}
		set
		{
			this.deactivateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06004779 RID: 18297 RVA: 0x001939A8 File Offset: 0x00191BA8
	// (set) Token: 0x0600477A RID: 18298 RVA: 0x001939B1 File Offset: 0x00191BB1
	public float DeactivateValue
	{
		get
		{
			return (float)this.activateValue;
		}
		set
		{
			this.activateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x0600477B RID: 18299 RVA: 0x001939C2 File Offset: 0x00191BC2
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x0600477C RID: 18300 RVA: 0x001939C9 File Offset: 0x00191BC9
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x0600477D RID: 18301 RVA: 0x001939D0 File Offset: 0x00191BD0
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x0600477E RID: 18302 RVA: 0x001939D3 File Offset: 0x00191BD3
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x0600477F RID: 18303 RVA: 0x001939DF File Offset: 0x00191BDF
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06004780 RID: 18304 RVA: 0x001939EB File Offset: 0x00191BEB
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06004781 RID: 18305 RVA: 0x001939F7 File Offset: 0x00191BF7
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06004782 RID: 18306 RVA: 0x00193A03 File Offset: 0x00191C03
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04002F52 RID: 12114
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002F53 RID: 12115
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002F54 RID: 12116
	[Serialize]
	private int activateValue;

	// Token: 0x04002F55 RID: 12117
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x04002F56 RID: 12118
	[Serialize]
	private bool activated;

	// Token: 0x04002F57 RID: 12119
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04002F58 RID: 12120
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002F59 RID: 12121
	private MeterController logicMeter;

	// Token: 0x04002F5A RID: 12122
	public static readonly HashedString PORT_ID = "SmartReservoirLogicPort";

	// Token: 0x04002F5B RID: 12123
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002F5C RID: 12124
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002F5D RID: 12125
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
