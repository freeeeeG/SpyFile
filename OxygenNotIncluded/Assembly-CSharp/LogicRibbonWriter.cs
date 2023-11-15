using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200063F RID: 1599
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonWriter")]
public class LogicRibbonWriter : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x060029A6 RID: 10662 RVA: 0x000E0043 File Offset: 0x000DE243
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonWriter>(-905833192, LogicRibbonWriter.OnCopySettingsDelegate);
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000E005C File Offset: 0x000DE25C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonWriter>(-801688580, LogicRibbonWriter.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000E00B8 File Offset: 0x000DE2B8
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonWriter.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x000E00F8 File Offset: 0x000DE2F8
	private void OnCopySettings(object data)
	{
		LogicRibbonWriter component = ((GameObject)data).GetComponent<LogicRibbonWriter>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x000E0128 File Offset: 0x000DE328
	private void UpdateLogicCircuit()
	{
		int new_value = this.currentValue << this.selectedBit;
		base.GetComponent<LogicPorts>().SendSignal(LogicRibbonWriter.OUTPUT_PORT_ID, new_value);
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x000E0157 File Offset: 0x000DE357
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x000E0160 File Offset: 0x000DE360
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x000E01A0 File Offset: 0x000DE3A0
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000E01E0 File Offset: 0x000DE3E0
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x000E01EF File Offset: 0x000DE3EF
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x000E01F7 File Offset: 0x000DE3F7
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060029B1 RID: 10673 RVA: 0x000E01FF File Offset: 0x000DE3FF
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_TITLE";
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060029B2 RID: 10674 RVA: 0x000E0206 File Offset: 0x000DE406
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_DESCRIPTION;
		}
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x000E0212 File Offset: 0x000DE412
	public bool SideScreenDisplayWriterDescription()
	{
		return true;
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000E0215 File Offset: 0x000DE415
	public bool SideScreenDisplayReaderDescription()
	{
		return false;
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x000E0218 File Offset: 0x000DE418
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000E0264 File Offset: 0x000DE464
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonWriter.INPUT_PORT_ID);
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x000E0290 File Offset: 0x000DE490
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonWriter.OUTPUT_PORT_ID);
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x000E02BC File Offset: 0x000DE4BC
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		int num = 0;
		if (inputNetwork)
		{
			num++;
			this.kbac.SetSymbolTint(LogicRibbonWriter.INPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetInputValue()) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num += 4;
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0400186C RID: 6252
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonWriterInput");

	// Token: 0x0400186D RID: 6253
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonWriterOutput");

	// Token: 0x0400186E RID: 6254
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400186F RID: 6255
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001870 RID: 6256
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001871 RID: 6257
	private LogicPorts ports;

	// Token: 0x04001872 RID: 6258
	public int bitDepth = 4;

	// Token: 0x04001873 RID: 6259
	[Serialize]
	public int selectedBit;

	// Token: 0x04001874 RID: 6260
	[Serialize]
	private int currentValue;

	// Token: 0x04001875 RID: 6261
	private KBatchedAnimController kbac;

	// Token: 0x04001876 RID: 6262
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04001877 RID: 6263
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04001878 RID: 6264
	private static KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04001879 RID: 6265
	private static KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x0400187A RID: 6266
	private static KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x0400187B RID: 6267
	private static KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x0400187C RID: 6268
	private static KAnimHashedString INPUT_SYMBOL = "input_light_bloom";
}
