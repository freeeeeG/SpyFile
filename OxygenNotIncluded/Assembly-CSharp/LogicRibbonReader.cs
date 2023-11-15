using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200063E RID: 1598
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonReader")]
public class LogicRibbonReader : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06002991 RID: 10641 RVA: 0x000DFB10 File Offset: 0x000DDD10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonReader>(-801688580, LogicRibbonReader.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000DFB6C File Offset: 0x000DDD6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonReader>(-905833192, LogicRibbonReader.OnCopySettingsDelegate);
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x000DFB88 File Offset: 0x000DDD88
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonReader.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x000DFBC8 File Offset: 0x000DDDC8
	private void OnCopySettings(object data)
	{
		LogicRibbonReader component = ((GameObject)data).GetComponent<LogicRibbonReader>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x000DFBF8 File Offset: 0x000DDDF8
	private void UpdateLogicCircuit()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
		int portCell = component.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component2 = gameObject.GetComponent<LogicWire>();
			if (component2 != null)
			{
				bitDepth = component2.MaxBitDepth;
			}
		}
		if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
		{
			int num = this.currentValue >> this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, num);
		}
		else
		{
			int num = this.currentValue & 1 << this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, (num > 0) ? 1 : 0);
		}
		this.UpdateVisuals();
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x000DFCA1 File Offset: 0x000DDEA1
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x000DFCA9 File Offset: 0x000DDEA9
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x000DFCB8 File Offset: 0x000DDEB8
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x000DFCC0 File Offset: 0x000DDEC0
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x0600299A RID: 10650 RVA: 0x000DFCC8 File Offset: 0x000DDEC8
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_TITLE";
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x0600299B RID: 10651 RVA: 0x000DFCCF File Offset: 0x000DDECF
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_DESCRIPTION;
		}
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x000DFCDB File Offset: 0x000DDEDB
	public bool SideScreenDisplayWriterDescription()
	{
		return false;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x000DFCDE File Offset: 0x000DDEDE
	public bool SideScreenDisplayReaderDescription()
	{
		return true;
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x000DFCE4 File Offset: 0x000DDEE4
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x000DFD30 File Offset: 0x000DDF30
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonReader.INPUT_PORT_ID);
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x000DFD5C File Offset: 0x000DDF5C
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonReader.OUTPUT_PORT_ID);
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x000DFD88 File Offset: 0x000DDF88
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x000DFDC8 File Offset: 0x000DDFC8
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060029A3 RID: 10659 RVA: 0x000DFE08 File Offset: 0x000DE008
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		this.GetInputValue();
		int num = 0;
		if (inputNetwork)
		{
			num += 4;
			this.kbac.SetSymbolTint(this.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num++;
			this.kbac.SetSymbolTint(this.OUTPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetOutputValue()) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0400185B RID: 6235
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonReaderInput");

	// Token: 0x0400185C RID: 6236
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonReaderOutput");

	// Token: 0x0400185D RID: 6237
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400185E RID: 6238
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400185F RID: 6239
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001860 RID: 6240
	private KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04001861 RID: 6241
	private KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04001862 RID: 6242
	private KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04001863 RID: 6243
	private KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04001864 RID: 6244
	private KAnimHashedString OUTPUT_SYMBOL = "output_light_bloom";

	// Token: 0x04001865 RID: 6245
	private KBatchedAnimController kbac;

	// Token: 0x04001866 RID: 6246
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04001867 RID: 6247
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04001868 RID: 6248
	private LogicPorts ports;

	// Token: 0x04001869 RID: 6249
	public int bitDepth = 4;

	// Token: 0x0400186A RID: 6250
	[Serialize]
	public int selectedBit;

	// Token: 0x0400186B RID: 6251
	[Serialize]
	private int currentValue;
}
