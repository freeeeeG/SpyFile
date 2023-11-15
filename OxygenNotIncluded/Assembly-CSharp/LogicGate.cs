using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000630 RID: 1584
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGate : LogicGateBase, ILogicEventSender, ILogicNetworkConnection
{
	// Token: 0x0600288B RID: 10379 RVA: 0x000DB078 File Offset: 0x000D9278
	protected override void OnSpawn()
	{
		this.inputOne = new LogicEventHandler(base.InputCellOne, new Action<int>(this.UpdateState), null, LogicPortSpriteType.Input);
		if (base.RequiresTwoInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		else if (base.RequiresFourInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputThree = new LogicEventHandler(base.InputCellThree, new Action<int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputFour = new LogicEventHandler(base.InputCellFour, new Action<int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		if (base.RequiresControlInputs)
		{
			this.controlOne = new LogicEventHandler(base.ControlCellOne, new Action<int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
			this.controlTwo = new LogicEventHandler(base.ControlCellTwo, new Action<int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
		}
		if (base.RequiresFourOutputs)
		{
			this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
			this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
			this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
			this.outputTwoSender = new LogicEventSender(LogicGateBase.OUTPUT_TWO_PORT_ID, base.OutputCellTwo, delegate(int new_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_TWO_PORT_ID, new_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputThreeSender = new LogicEventSender(LogicGateBase.OUTPUT_THREE_PORT_ID, base.OutputCellThree, delegate(int new_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_THREE_PORT_ID, new_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputFourSender = new LogicEventSender(LogicGateBase.OUTPUT_FOUR_PORT_ID, base.OutputCellFour, delegate(int new_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_FOUR_PORT_ID, new_value);
				}
			}, null, LogicPortSpriteType.Output);
		}
		base.Subscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate);
		base.Subscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate);
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || !component.IsBroken)
		{
			this.Connect();
		}
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000DB265 File Offset: 0x000D9465
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.Disconnect();
		base.Unsubscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000DB29C File Offset: 0x000D949C
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000DB2A4 File Offset: 0x000D94A4
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000DB2AC File Offset: 0x000D94AC
	private void Connect()
	{
		if (!this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = true;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.AddToNetworks(outputCellOne, this, true);
			this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.Output);
			logicCircuitManager.AddVisElem(this.outputOne);
			if (base.RequiresFourOutputs)
			{
				this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.AddVisElem(this.outputTwo);
				this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.AddVisElem(this.outputThree);
				this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.AddVisElem(this.outputFour);
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.AddToNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.AddVisElem(this.inputOne);
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.AddToNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.AddToNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
				logicCircuitSystem.AddToNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.AddVisElem(this.inputThree);
				logicCircuitSystem.AddToNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.AddVisElem(this.inputFour);
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.AddToNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.AddVisElem(this.controlOne);
				logicCircuitSystem.AddToNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.AddVisElem(this.controlTwo);
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000DB4A8 File Offset: 0x000D96A8
	private void Disconnect()
	{
		if (this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = false;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.RemoveFromNetworks(outputCellOne, this, true);
			logicCircuitManager.RemoveVisElem(this.outputOne);
			this.outputOne = null;
			if (base.RequiresFourOutputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.RemoveVisElem(this.outputTwo);
				this.outputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.RemoveVisElem(this.outputThree);
				this.outputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.RemoveVisElem(this.outputFour);
				this.outputFour = null;
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.RemoveFromNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.RemoveVisElem(this.inputOne);
			this.inputOne = null;
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.RemoveFromNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.RemoveVisElem(this.inputThree);
				this.inputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.RemoveVisElem(this.inputFour);
				this.inputFour = null;
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.RemoveVisElem(this.controlOne);
				this.controlOne = null;
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.RemoveVisElem(this.controlTwo);
				this.controlTwo = null;
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000DB6AC File Offset: 0x000D98AC
	private void UpdateState(int new_value)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int value = this.inputOne.Value;
		int num = (this.inputTwo != null) ? this.inputTwo.Value : 0;
		int num2 = (this.inputThree != null) ? this.inputThree.Value : 0;
		int num3 = (this.inputFour != null) ? this.inputFour.Value : 0;
		int value2 = (this.controlOne != null) ? this.controlOne.Value : 0;
		int value3 = (this.controlTwo != null) ? this.controlTwo.Value : 0;
		if (base.RequiresFourInputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			if (this.op == LogicGateBase.Op.Multiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value2))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueOne = num;
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					this.outputValueOne = num2;
				}
				else
				{
					this.outputValueOne = num3;
				}
			}
		}
		if (base.RequiresFourOutputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			this.outputValueTwo = 0;
			this.outputTwoSender.SetValue(0);
			this.outputValueThree = 0;
			this.outputThreeSender.SetValue(0);
			this.outputValueFour = 0;
			this.outputFourSender.SetValue(0);
			if (this.op == LogicGateBase.Op.Demultiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value3))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueTwo = value;
						this.outputTwoSender.SetValue(value);
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					this.outputValueThree = value;
					this.outputThreeSender.SetValue(value);
				}
				else
				{
					this.outputValueFour = value;
					this.outputFourSender.SetValue(value);
				}
			}
		}
		switch (this.op)
		{
		case LogicGateBase.Op.And:
			this.outputValueOne = (value & num);
			break;
		case LogicGateBase.Op.Or:
			this.outputValueOne = (value | num);
			break;
		case LogicGateBase.Op.Not:
		{
			LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
			int inputCellOne = base.InputCellOne;
			GameObject gameObject = Grid.Objects[inputCellOne, 31];
			if (gameObject != null)
			{
				LogicWire component = gameObject.GetComponent<LogicWire>();
				if (component != null)
				{
					bitDepth = component.MaxBitDepth;
				}
			}
			if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
			{
				uint num4 = (uint)value;
				num4 = ~num4;
				num4 &= 15U;
				this.outputValueOne = (int)num4;
			}
			else
			{
				this.outputValueOne = ((value == 0) ? 1 : 0);
			}
			break;
		}
		case LogicGateBase.Op.Xor:
			this.outputValueOne = (value ^ num);
			break;
		case LogicGateBase.Op.CustomSingle:
			this.outputValueOne = this.GetCustomValue(value, num);
			break;
		}
		this.RefreshAnimation();
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000DB940 File Offset: 0x000D9B40
	private void OnAdditionalOutputsLogicValueChanged(HashedString port_id, int new_value)
	{
		if (base.gameObject != null)
		{
			base.gameObject.Trigger(-801688580, new LogicValueChanged
			{
				portID = port_id,
				newValue = new_value
			});
		}
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000DB989 File Offset: 0x000D9B89
	public virtual void LogicTick()
	{
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000DB98B File Offset: 0x000D9B8B
	protected virtual int GetCustomValue(int val1, int val2)
	{
		return val1;
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000DB990 File Offset: 0x000D9B90
	public int GetPortValue(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.inputOne.Value;
		case LogicGateBase.PortId.InputTwo:
			if (base.RequiresTwoInputs || base.RequiresFourInputs)
			{
				return this.inputTwo.Value;
			}
			return 0;
		case LogicGateBase.PortId.InputThree:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputThree.Value;
		case LogicGateBase.PortId.InputFour:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputFour.Value;
		case LogicGateBase.PortId.OutputOne:
			return this.outputValueOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.outputValueTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.outputValueThree;
		case LogicGateBase.PortId.OutputFour:
			return this.outputValueFour;
		case LogicGateBase.PortId.ControlOne:
			return this.controlOne.Value;
		case LogicGateBase.PortId.ControlTwo:
			return this.controlTwo.Value;
		default:
			return this.outputValueOne;
		}
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x000DBA60 File Offset: 0x000D9C60
	public bool GetPortConnected(LogicGateBase.PortId port)
	{
		if ((port == LogicGateBase.PortId.InputTwo && !base.RequiresTwoInputs && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputThree && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputFour && !base.RequiresFourInputs))
		{
			return false;
		}
		int cell = base.PortCell(port);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(cell) != null;
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x000DBAB6 File Offset: 0x000D9CB6
	public void SetPortDescriptions(LogicGate.LogicGateDescriptions descriptions)
	{
		this.descriptions = descriptions;
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x000DBAC0 File Offset: 0x000D9CC0
	public LogicGate.LogicGateDescriptions.Description GetPortDescription(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresTwoInputs && !base.RequiresFourInputs)
			{
				return LogicGate.INPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.INPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.InputTwo:
			if (this.descriptions.inputTwo == null)
			{
				return LogicGate.INPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.inputTwo;
		case LogicGateBase.PortId.InputThree:
			if (this.descriptions.inputThree == null)
			{
				return LogicGate.INPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.inputThree;
		case LogicGateBase.PortId.InputFour:
			if (this.descriptions.inputFour == null)
			{
				return LogicGate.INPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.inputFour;
		case LogicGateBase.PortId.OutputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresFourOutputs)
			{
				return LogicGate.OUTPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.OUTPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.OutputTwo:
			if (this.descriptions.outputTwo == null)
			{
				return LogicGate.OUTPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.outputTwo;
		case LogicGateBase.PortId.OutputThree:
			if (this.descriptions.outputThree == null)
			{
				return LogicGate.OUTPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.outputThree;
		case LogicGateBase.PortId.OutputFour:
			if (this.descriptions.outputFour == null)
			{
				return LogicGate.OUTPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.outputFour;
		case LogicGateBase.PortId.ControlOne:
			if (this.descriptions.controlOne == null)
			{
				return LogicGate.CONTROL_ONE_DESCRIPTION;
			}
			return this.descriptions.controlOne;
		case LogicGateBase.PortId.ControlTwo:
			if (this.descriptions.controlTwo == null)
			{
				return LogicGate.CONTROL_TWO_DESCRIPTION;
			}
			return this.descriptions.controlTwo;
		default:
			return this.descriptions.outputOne;
		}
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000DBC65 File Offset: 0x000D9E65
	public int GetLogicValue()
	{
		return this.outputValueOne;
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x000DBC6D File Offset: 0x000D9E6D
	public int GetLogicCell()
	{
		return this.GetLogicUICell();
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x000DBC75 File Offset: 0x000D9E75
	public int GetLogicUICell()
	{
		return base.OutputCellOne;
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000DBC7D File Offset: 0x000D9E7D
	public bool IsLogicInput()
	{
		return false;
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x000DBC80 File Offset: 0x000D9E80
	private LogicEventHandler GetInputFromControlValue(int val)
	{
		switch (val)
		{
		case 1:
			return this.inputTwo;
		case 2:
			return this.inputThree;
		case 3:
			return this.inputFour;
		}
		return this.inputOne;
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x000DBCB5 File Offset: 0x000D9EB5
	private void ShowSymbolConditionally(bool showAnything, bool active, KBatchedAnimController kbac, KAnimHashedString ifTrue, KAnimHashedString ifFalse)
	{
		if (!showAnything)
		{
			kbac.SetSymbolVisiblity(ifTrue, false);
			kbac.SetSymbolVisiblity(ifFalse, false);
			return;
		}
		kbac.SetSymbolVisiblity(ifTrue, active);
		kbac.SetSymbolVisiblity(ifFalse, !active);
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x000DBCE2 File Offset: 0x000D9EE2
	private void TintSymbolConditionally(bool tintAnything, bool condition, KBatchedAnimController kbac, KAnimHashedString symbol, Color ifTrue, Color ifFalse)
	{
		if (tintAnything)
		{
			kbac.SetSymbolTint(symbol, condition ? ifTrue : ifFalse);
			return;
		}
		kbac.SetSymbolTint(symbol, Color.white);
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x000DBD06 File Offset: 0x000D9F06
	private void SetBloomSymbolShowing(bool showing, KBatchedAnimController kbac, KAnimHashedString symbol, KAnimHashedString bloomSymbol)
	{
		kbac.SetSymbolVisiblity(bloomSymbol, showing);
		kbac.SetSymbolVisiblity(symbol, !showing);
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x000DBD1C File Offset: 0x000D9F1C
	protected void RefreshAnimation()
	{
		if (this.cleaningUp)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.op == LogicGateBase.Op.Multiplexer)
		{
			int num = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value) * 2;
			if (this.lastAnimState != num)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num;
			LogicEventHandler inputFromControlValue = this.GetInputFromControlValue(num);
			KAnimHashedString[] array = LogicGate.multiplexerSymbolPaths[num];
			LogicCircuitNetwork logicCircuitNetwork = Game.Instance.logicCircuitSystem.GetNetworkForCell(inputFromControlValue.GetLogicCell()) as LogicCircuitNetwork;
			UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellOne);
			UtilityNetwork networkForCell2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellTwo);
			UtilityNetwork networkForCell3 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellThree);
			UtilityNetwork networkForCell4 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellFour);
			this.ShowSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL_BLM_RED, LogicGate.INPUT2_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL_BLM_RED, LogicGate.INPUT3_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL_BLM_RED, LogicGate.INPUT4_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL_BLM_RED, LogicGate.OUTPUT1_SYMBOL_BLM_GRN);
			this.TintSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(Game.Instance.logicCircuitSystem.GetNetworkForCell(base.OutputCellOne) != null && logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			for (int i = 0; i < LogicGate.multiplexerSymbols.Length; i++)
			{
				KAnimHashedString symbol = LogicGate.multiplexerSymbols[i];
				KAnimHashedString kanimHashedString = LogicGate.multiplexerBloomSymbols[i];
				bool flag = Array.IndexOf<KAnimHashedString>(array, kanimHashedString) != -1 && logicCircuitNetwork != null;
				this.SetBloomSymbolShowing(flag, component, symbol, kanimHashedString);
				if (flag)
				{
					component.SetSymbolTint(kanimHashedString, (inputFromControlValue.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			return;
		}
		if (this.op == LogicGateBase.Op.Demultiplexer)
		{
			int num2 = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) * 2 + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value);
			if (this.lastAnimState != num2)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num2;
			KAnimHashedString[] array2 = LogicGate.demultiplexerSymbolPaths[num2];
			LogicCircuitNetwork logicCircuitNetwork2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(this.inputOne.GetLogicCell()) as LogicCircuitNetwork;
			for (int j = 0; j < LogicGate.demultiplexerSymbols.Length; j++)
			{
				KAnimHashedString symbol2 = LogicGate.demultiplexerSymbols[j];
				KAnimHashedString kanimHashedString2 = LogicGate.demultiplexerBloomSymbols[j];
				bool flag2 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString2) != -1 && logicCircuitNetwork2 != null;
				this.SetBloomSymbolShowing(flag2, component, symbol2, kanimHashedString2);
				if (flag2)
				{
					component.SetSymbolTint(kanimHashedString2, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			this.ShowSymbolConditionally(logicCircuitNetwork2 != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			if (logicCircuitNetwork2 != null)
			{
				component.SetSymbolTint(LogicGate.INPUT1_SYMBOL_BLOOM, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
			}
			int[] array3 = new int[]
			{
				base.OutputCellOne,
				base.OutputCellTwo,
				base.OutputCellThree,
				base.OutputCellFour
			};
			for (int k = 0; k < LogicGate.demultiplexerOutputSymbols.Length; k++)
			{
				KAnimHashedString kanimHashedString3 = LogicGate.demultiplexerOutputSymbols[k];
				bool flag3 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString3) == -1 || this.inputOne.Value == 0;
				UtilityNetwork networkForCell5 = Game.Instance.logicCircuitSystem.GetNetworkForCell(array3[k]);
				this.TintSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, kanimHashedString3, this.inactiveTintColor, this.activeTintColor);
				this.ShowSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, LogicGate.demultiplexerOutputRedSymbols[k], LogicGate.demultiplexerOutputGreenSymbols[k]);
			}
			return;
		}
		if (this.op == LogicGateBase.Op.And || this.op == LogicGateBase.Op.Xor || this.op == LogicGateBase.Op.Not || this.op == LogicGateBase.Op.Or)
		{
			int outputCellOne = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				int num3 = this.inputOne.Value * 2 + this.inputTwo.Value;
				if (this.lastAnimState != num3)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = num3;
					return;
				}
			}
			else
			{
				int value = this.inputOne.Value;
				if (this.lastAnimState != value)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = value;
					return;
				}
			}
		}
		else
		{
			int outputCellOne2 = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne2) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				component.Play("on_" + (this.inputOne.Value + this.inputTwo.Value * 2 + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play("on_" + (this.inputOne.Value + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x000DC521 File Offset: 0x000DA721
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
	}

	// Token: 0x04001789 RID: 6025
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE
	};

	// Token: 0x0400178A RID: 6026
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_INACTIVE
	};

	// Token: 0x0400178B RID: 6027
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_INACTIVE
	};

	// Token: 0x0400178C RID: 6028
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_INACTIVE
	};

	// Token: 0x0400178D RID: 6029
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_INACTIVE
	};

	// Token: 0x0400178E RID: 6030
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x0400178F RID: 6031
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x04001790 RID: 6032
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_INACTIVE
	};

	// Token: 0x04001791 RID: 6033
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_INACTIVE
	};

	// Token: 0x04001792 RID: 6034
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_INACTIVE
	};

	// Token: 0x04001793 RID: 6035
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_ONE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE
	};

	// Token: 0x04001794 RID: 6036
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE
	};

	// Token: 0x04001795 RID: 6037
	private LogicGate.LogicGateDescriptions descriptions;

	// Token: 0x04001796 RID: 6038
	private LogicEventSender[] additionalOutputs;

	// Token: 0x04001797 RID: 6039
	private const bool IS_CIRCUIT_ENDPOINT = true;

	// Token: 0x04001798 RID: 6040
	private bool connected;

	// Token: 0x04001799 RID: 6041
	protected bool cleaningUp;

	// Token: 0x0400179A RID: 6042
	private int lastAnimState = -1;

	// Token: 0x0400179B RID: 6043
	[Serialize]
	protected int outputValueOne;

	// Token: 0x0400179C RID: 6044
	[Serialize]
	protected int outputValueTwo;

	// Token: 0x0400179D RID: 6045
	[Serialize]
	protected int outputValueThree;

	// Token: 0x0400179E RID: 6046
	[Serialize]
	protected int outputValueFour;

	// Token: 0x0400179F RID: 6047
	private LogicEventHandler inputOne;

	// Token: 0x040017A0 RID: 6048
	private LogicEventHandler inputTwo;

	// Token: 0x040017A1 RID: 6049
	private LogicEventHandler inputThree;

	// Token: 0x040017A2 RID: 6050
	private LogicEventHandler inputFour;

	// Token: 0x040017A3 RID: 6051
	private LogicPortVisualizer outputOne;

	// Token: 0x040017A4 RID: 6052
	private LogicPortVisualizer outputTwo;

	// Token: 0x040017A5 RID: 6053
	private LogicPortVisualizer outputThree;

	// Token: 0x040017A6 RID: 6054
	private LogicPortVisualizer outputFour;

	// Token: 0x040017A7 RID: 6055
	private LogicEventSender outputTwoSender;

	// Token: 0x040017A8 RID: 6056
	private LogicEventSender outputThreeSender;

	// Token: 0x040017A9 RID: 6057
	private LogicEventSender outputFourSender;

	// Token: 0x040017AA RID: 6058
	private LogicEventHandler controlOne;

	// Token: 0x040017AB RID: 6059
	private LogicEventHandler controlTwo;

	// Token: 0x040017AC RID: 6060
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x040017AD RID: 6061
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x040017AE RID: 6062
	private static KAnimHashedString INPUT1_SYMBOL = "input1";

	// Token: 0x040017AF RID: 6063
	private static KAnimHashedString INPUT2_SYMBOL = "input2";

	// Token: 0x040017B0 RID: 6064
	private static KAnimHashedString INPUT3_SYMBOL = "input3";

	// Token: 0x040017B1 RID: 6065
	private static KAnimHashedString INPUT4_SYMBOL = "input4";

	// Token: 0x040017B2 RID: 6066
	private static KAnimHashedString OUTPUT1_SYMBOL = "output1";

	// Token: 0x040017B3 RID: 6067
	private static KAnimHashedString OUTPUT2_SYMBOL = "output2";

	// Token: 0x040017B4 RID: 6068
	private static KAnimHashedString OUTPUT3_SYMBOL = "output3";

	// Token: 0x040017B5 RID: 6069
	private static KAnimHashedString OUTPUT4_SYMBOL = "output4";

	// Token: 0x040017B6 RID: 6070
	private static KAnimHashedString INPUT1_SYMBOL_BLM_RED = "input1_red_bloom";

	// Token: 0x040017B7 RID: 6071
	private static KAnimHashedString INPUT1_SYMBOL_BLM_GRN = "input1_green_bloom";

	// Token: 0x040017B8 RID: 6072
	private static KAnimHashedString INPUT2_SYMBOL_BLM_RED = "input2_red_bloom";

	// Token: 0x040017B9 RID: 6073
	private static KAnimHashedString INPUT2_SYMBOL_BLM_GRN = "input2_green_bloom";

	// Token: 0x040017BA RID: 6074
	private static KAnimHashedString INPUT3_SYMBOL_BLM_RED = "input3_red_bloom";

	// Token: 0x040017BB RID: 6075
	private static KAnimHashedString INPUT3_SYMBOL_BLM_GRN = "input3_green_bloom";

	// Token: 0x040017BC RID: 6076
	private static KAnimHashedString INPUT4_SYMBOL_BLM_RED = "input4_red_bloom";

	// Token: 0x040017BD RID: 6077
	private static KAnimHashedString INPUT4_SYMBOL_BLM_GRN = "input4_green_bloom";

	// Token: 0x040017BE RID: 6078
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_RED = "output1_red_bloom";

	// Token: 0x040017BF RID: 6079
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_GRN = "output1_green_bloom";

	// Token: 0x040017C0 RID: 6080
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_RED = "output2_red_bloom";

	// Token: 0x040017C1 RID: 6081
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_GRN = "output2_green_bloom";

	// Token: 0x040017C2 RID: 6082
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_RED = "output3_red_bloom";

	// Token: 0x040017C3 RID: 6083
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_GRN = "output3_green_bloom";

	// Token: 0x040017C4 RID: 6084
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_RED = "output4_red_bloom";

	// Token: 0x040017C5 RID: 6085
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_GRN = "output4_green_bloom";

	// Token: 0x040017C6 RID: 6086
	private static KAnimHashedString LINE_LEFT_1_SYMBOL = "line_left_1";

	// Token: 0x040017C7 RID: 6087
	private static KAnimHashedString LINE_LEFT_2_SYMBOL = "line_left_2";

	// Token: 0x040017C8 RID: 6088
	private static KAnimHashedString LINE_LEFT_3_SYMBOL = "line_left_3";

	// Token: 0x040017C9 RID: 6089
	private static KAnimHashedString LINE_LEFT_4_SYMBOL = "line_left_4";

	// Token: 0x040017CA RID: 6090
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL = "line_right_1";

	// Token: 0x040017CB RID: 6091
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL = "line_right_2";

	// Token: 0x040017CC RID: 6092
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL = "line_right_3";

	// Token: 0x040017CD RID: 6093
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL = "line_right_4";

	// Token: 0x040017CE RID: 6094
	private static KAnimHashedString FLIPPER_1_SYMBOL = "flipper1";

	// Token: 0x040017CF RID: 6095
	private static KAnimHashedString FLIPPER_2_SYMBOL = "flipper2";

	// Token: 0x040017D0 RID: 6096
	private static KAnimHashedString FLIPPER_3_SYMBOL = "flipper3";

	// Token: 0x040017D1 RID: 6097
	private static KAnimHashedString INPUT_SYMBOL = "input";

	// Token: 0x040017D2 RID: 6098
	private static KAnimHashedString OUTPUT_SYMBOL = "output";

	// Token: 0x040017D3 RID: 6099
	private static KAnimHashedString INPUT1_SYMBOL_BLOOM = "input1_bloom";

	// Token: 0x040017D4 RID: 6100
	private static KAnimHashedString INPUT2_SYMBOL_BLOOM = "input2_bloom";

	// Token: 0x040017D5 RID: 6101
	private static KAnimHashedString INPUT3_SYMBOL_BLOOM = "input3_bloom";

	// Token: 0x040017D6 RID: 6102
	private static KAnimHashedString INPUT4_SYMBOL_BLOOM = "input4_bloom";

	// Token: 0x040017D7 RID: 6103
	private static KAnimHashedString OUTPUT1_SYMBOL_BLOOM = "output1_bloom";

	// Token: 0x040017D8 RID: 6104
	private static KAnimHashedString OUTPUT2_SYMBOL_BLOOM = "output2_bloom";

	// Token: 0x040017D9 RID: 6105
	private static KAnimHashedString OUTPUT3_SYMBOL_BLOOM = "output3_bloom";

	// Token: 0x040017DA RID: 6106
	private static KAnimHashedString OUTPUT4_SYMBOL_BLOOM = "output4_bloom";

	// Token: 0x040017DB RID: 6107
	private static KAnimHashedString LINE_LEFT_1_SYMBOL_BLOOM = "line_left_1_bloom";

	// Token: 0x040017DC RID: 6108
	private static KAnimHashedString LINE_LEFT_2_SYMBOL_BLOOM = "line_left_2_bloom";

	// Token: 0x040017DD RID: 6109
	private static KAnimHashedString LINE_LEFT_3_SYMBOL_BLOOM = "line_left_3_bloom";

	// Token: 0x040017DE RID: 6110
	private static KAnimHashedString LINE_LEFT_4_SYMBOL_BLOOM = "line_left_4_bloom";

	// Token: 0x040017DF RID: 6111
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL_BLOOM = "line_right_1_bloom";

	// Token: 0x040017E0 RID: 6112
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL_BLOOM = "line_right_2_bloom";

	// Token: 0x040017E1 RID: 6113
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL_BLOOM = "line_right_3_bloom";

	// Token: 0x040017E2 RID: 6114
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL_BLOOM = "line_right_4_bloom";

	// Token: 0x040017E3 RID: 6115
	private static KAnimHashedString FLIPPER_1_SYMBOL_BLOOM = "flipper1_bloom";

	// Token: 0x040017E4 RID: 6116
	private static KAnimHashedString FLIPPER_2_SYMBOL_BLOOM = "flipper2_bloom";

	// Token: 0x040017E5 RID: 6117
	private static KAnimHashedString FLIPPER_3_SYMBOL_BLOOM = "flipper3_bloom";

	// Token: 0x040017E6 RID: 6118
	private static KAnimHashedString INPUT_SYMBOL_BLOOM = "input_bloom";

	// Token: 0x040017E7 RID: 6119
	private static KAnimHashedString OUTPUT_SYMBOL_BLOOM = "output_bloom";

	// Token: 0x040017E8 RID: 6120
	private static KAnimHashedString[][] multiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		}
	};

	// Token: 0x040017E9 RID: 6121
	private static KAnimHashedString[] multiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_LEFT_3_SYMBOL,
		LogicGate.LINE_LEFT_4_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.FLIPPER_1_SYMBOL,
		LogicGate.FLIPPER_2_SYMBOL,
		LogicGate.FLIPPER_3_SYMBOL,
		LogicGate.OUTPUT_SYMBOL
	};

	// Token: 0x040017EA RID: 6122
	private static KAnimHashedString[] multiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_1_SYMBOL_BLOOM,
		LogicGate.FLIPPER_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_3_SYMBOL_BLOOM,
		LogicGate.OUTPUT_SYMBOL_BLOOM
	};

	// Token: 0x040017EB RID: 6123
	private static KAnimHashedString[][] demultiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.OUTPUT1_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.OUTPUT2_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT3_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM,
			LogicGate.OUTPUT4_SYMBOL
		}
	};

	// Token: 0x040017EC RID: 6124
	private static KAnimHashedString[] demultiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL,
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.LINE_RIGHT_3_SYMBOL,
		LogicGate.LINE_RIGHT_4_SYMBOL
	};

	// Token: 0x040017ED RID: 6125
	private static KAnimHashedString[] demultiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM
	};

	// Token: 0x040017EE RID: 6126
	private static KAnimHashedString[] demultiplexerOutputSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL,
		LogicGate.OUTPUT2_SYMBOL,
		LogicGate.OUTPUT3_SYMBOL,
		LogicGate.OUTPUT4_SYMBOL
	};

	// Token: 0x040017EF RID: 6127
	private static KAnimHashedString[] demultiplexerOutputRedSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_RED,
		LogicGate.OUTPUT2_SYMBOL_BLM_RED,
		LogicGate.OUTPUT3_SYMBOL_BLM_RED,
		LogicGate.OUTPUT4_SYMBOL_BLM_RED
	};

	// Token: 0x040017F0 RID: 6128
	private static KAnimHashedString[] demultiplexerOutputGreenSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT2_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT3_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT4_SYMBOL_BLM_GRN
	};

	// Token: 0x040017F1 RID: 6129
	private Color activeTintColor = new Color(0.5411765f, 0.9882353f, 0.29803923f);

	// Token: 0x040017F2 RID: 6130
	private Color inactiveTintColor = Color.red;

	// Token: 0x020012FB RID: 4859
	public class LogicGateDescriptions
	{
		// Token: 0x0400613C RID: 24892
		public LogicGate.LogicGateDescriptions.Description inputOne;

		// Token: 0x0400613D RID: 24893
		public LogicGate.LogicGateDescriptions.Description inputTwo;

		// Token: 0x0400613E RID: 24894
		public LogicGate.LogicGateDescriptions.Description inputThree;

		// Token: 0x0400613F RID: 24895
		public LogicGate.LogicGateDescriptions.Description inputFour;

		// Token: 0x04006140 RID: 24896
		public LogicGate.LogicGateDescriptions.Description outputOne;

		// Token: 0x04006141 RID: 24897
		public LogicGate.LogicGateDescriptions.Description outputTwo;

		// Token: 0x04006142 RID: 24898
		public LogicGate.LogicGateDescriptions.Description outputThree;

		// Token: 0x04006143 RID: 24899
		public LogicGate.LogicGateDescriptions.Description outputFour;

		// Token: 0x04006144 RID: 24900
		public LogicGate.LogicGateDescriptions.Description controlOne;

		// Token: 0x04006145 RID: 24901
		public LogicGate.LogicGateDescriptions.Description controlTwo;

		// Token: 0x020020EE RID: 8430
		public class Description
		{
			// Token: 0x04009373 RID: 37747
			public string name;

			// Token: 0x04009374 RID: 37748
			public string active;

			// Token: 0x04009375 RID: 37749
			public string inactive;
		}
	}
}
