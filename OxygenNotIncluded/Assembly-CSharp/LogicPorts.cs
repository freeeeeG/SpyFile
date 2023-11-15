using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000850 RID: 2128
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicPorts")]
public class LogicPorts : KMonoBehaviour, IGameObjectEffectDescriptor, IRenderEveryTick
{
	// Token: 0x06003E20 RID: 15904 RVA: 0x00159537 File Offset: 0x00157737
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x00159548 File Offset: 0x00157748
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.isPhysical = (base.GetComponent<BuildingComplete>() != null);
		if (!this.isPhysical && base.GetComponent<BuildingUnderConstruction>() == null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.OnOverlayChanged(OverlayScreen.Instance.mode);
			this.CreateVisualizers();
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		if (this.isPhysical)
		{
			this.UpdateMissingWireIcon();
			this.CreatePhysicalPorts(false);
			return;
		}
		this.CreateVisualizers();
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x001595EC File Offset: 0x001577EC
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.DestroyVisualizers();
		if (this.isPhysical)
		{
			this.DestroyPhysicalPorts();
		}
		base.OnCleanUp();
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x00159639 File Offset: 0x00157839
	public void RenderEveryTick(float dt)
	{
		this.CreateVisualizers();
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x00159641 File Offset: 0x00157841
	public void HackRefreshVisualizers()
	{
		this.CreateVisualizers();
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x0015964C File Offset: 0x0015784C
	private void CreateVisualizers()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool flag = num != this.cell;
		this.cell = num;
		if (!flag)
		{
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				Orientation orientation = component.GetOrientation();
				flag = (orientation != this.orientation);
				this.orientation = orientation;
			}
		}
		if (!flag)
		{
			return;
		}
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				LogicPortVisualizer logicPortVisualizer = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
				this.outputPorts.Add(logicPortVisualizer);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer);
			}
		}
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				LogicPortVisualizer logicPortVisualizer2 = new LogicPortVisualizer(this.GetActualCell(port2.cellOffset), port2.spriteType);
				this.inputPorts.Add(logicPortVisualizer2);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer2);
			}
		}
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x0015979C File Offset: 0x0015799C
	private void DestroyVisualizers()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement elem in this.outputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem);
			}
		}
		if (this.inputPorts != null)
		{
			foreach (ILogicUIElement elem2 in this.inputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem2);
			}
		}
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x00159854 File Offset: 0x00157A54
	private void CreatePhysicalPorts(bool forceCreate = false)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell && !forceCreate)
		{
			return;
		}
		this.cell = num;
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port info = this.outputPortInfo[i];
				LogicEventSender logicEventSender = new LogicEventSender(info.id, this.GetActualCell(info.cellOffset), delegate(int new_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.outputPorts.Add(logicEventSender);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventSender);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventSender.GetLogicUICell(), logicEventSender, true);
			}
			if (this.serializedOutputValues != null && this.serializedOutputValues.Length == this.outputPorts.Count)
			{
				for (int j = 0; j < this.outputPorts.Count; j++)
				{
					(this.outputPorts[j] as LogicEventSender).SetValue(this.serializedOutputValues[j]);
				}
			}
		}
		this.serializedOutputValues = null;
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int k = 0; k < this.inputPortInfo.Length; k++)
			{
				LogicPorts.Port info = this.inputPortInfo[k];
				LogicEventHandler logicEventHandler = new LogicEventHandler(this.GetActualCell(info.cellOffset), delegate(int new_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.inputPorts.Add(logicEventHandler);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventHandler);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventHandler.GetLogicUICell(), logicEventHandler, true);
			}
		}
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x00159A7C File Offset: 0x00157C7C
	private bool ShowMissingWireIcon()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		if (this.outputPortInfo != null)
		{
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				if (port.requiresConnection)
				{
					int portCell = this.GetPortCell(port.id);
					if (logicCircuitManager.GetNetworkForCell(portCell) == null)
					{
						return true;
					}
				}
			}
		}
		if (this.inputPortInfo != null)
		{
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				if (port2.requiresConnection)
				{
					int portCell2 = this.GetPortCell(port2.id);
					if (logicCircuitManager.GetNetworkForCell(portCell2) == null)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x00159B2F File Offset: 0x00157D2F
	public void OnMove()
	{
		this.DestroyPhysicalPorts();
		this.CreatePhysicalPorts(false);
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x00159B3E File Offset: 0x00157D3E
	private void OnLogicNetworkConnectionChanged(int cell, bool connected)
	{
		this.UpdateMissingWireIcon();
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x00159B46 File Offset: 0x00157D46
	private void UpdateMissingWireIcon()
	{
		LogicCircuitManager.ToggleNoWireConnected(this.ShowMissingWireIcon(), base.gameObject);
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x00159B5C File Offset: 0x00157D5C
	private void DestroyPhysicalPorts()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement logicUIElement in this.outputPorts)
			{
				ILogicEventSender logicEventSender = (ILogicEventSender)logicUIElement;
				Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventSender.GetLogicCell(), logicEventSender, true);
			}
		}
		if (this.inputPorts != null)
		{
			for (int i = 0; i < this.inputPorts.Count; i++)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[i] as LogicEventHandler;
				if (logicEventHandler != null)
				{
					Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventHandler.GetLogicCell(), logicEventHandler, true);
				}
			}
		}
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x00159C18 File Offset: 0x00157E18
	private void OnLogicValueChanged(HashedString port_id, int new_value)
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

	// Token: 0x06003E2E RID: 15918 RVA: 0x00159C64 File Offset: 0x00157E64
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x00159CA0 File Offset: 0x00157EA0
	public bool TryGetPortAtCell(int cell, out LogicPorts.Port port, out bool isInput)
	{
		foreach (LogicPorts.Port port2 in this.inputPortInfo)
		{
			if (this.GetActualCell(port2.cellOffset) == cell)
			{
				port = port2;
				isInput = true;
				return true;
			}
		}
		foreach (LogicPorts.Port port3 in this.outputPortInfo)
		{
			if (this.GetActualCell(port3.cellOffset) == cell)
			{
				port = port3;
				isInput = false;
				return true;
			}
		}
		port = default(LogicPorts.Port);
		isInput = false;
		return false;
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x00159D28 File Offset: 0x00157F28
	public void SendSignal(HashedString port_id, int new_value)
	{
		if (this.outputPortInfo != null && this.outputPorts == null)
		{
			this.CreatePhysicalPorts(true);
		}
		foreach (ILogicUIElement logicUIElement in this.outputPorts)
		{
			LogicEventSender logicEventSender = (LogicEventSender)logicUIElement;
			if (logicEventSender.ID == port_id)
			{
				logicEventSender.SetValue(new_value);
				break;
			}
		}
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x00159DA8 File Offset: 0x00157FA8
	public int GetPortCell(HashedString port_id)
	{
		foreach (LogicPorts.Port port in this.inputPortInfo)
		{
			if (port.id == port_id)
			{
				return this.GetActualCell(port.cellOffset);
			}
		}
		foreach (LogicPorts.Port port2 in this.outputPortInfo)
		{
			if (port2.id == port_id)
			{
				return this.GetActualCell(port2.cellOffset);
			}
		}
		return -1;
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x00159E28 File Offset: 0x00158028
	public int GetInputValue(HashedString port_id)
	{
		int num = 0;
		while (num < this.inputPortInfo.Length && this.inputPorts != null)
		{
			if (this.inputPortInfo[num].id == port_id)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[num] as LogicEventHandler;
				if (logicEventHandler == null)
				{
					return 0;
				}
				return logicEventHandler.Value;
			}
			else
			{
				num++;
			}
		}
		return 0;
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x00159E88 File Offset: 0x00158088
	public int GetOutputValue(HashedString port_id)
	{
		for (int i = 0; i < this.outputPorts.Count; i++)
		{
			LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
			if (logicEventSender == null)
			{
				return 0;
			}
			if (logicEventSender.ID == port_id)
			{
				return logicEventSender.GetLogicValue();
			}
		}
		return 0;
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x00159ED8 File Offset: 0x001580D8
	public bool IsPortConnected(HashedString port_id)
	{
		int portCell = this.GetPortCell(port_id);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x00159F00 File Offset: 0x00158100
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			base.enabled = true;
			this.CreateVisualizers();
			return;
		}
		base.enabled = false;
		this.DestroyVisualizers();
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x00159F2C File Offset: 0x0015812C
	public LogicWire.BitDepth GetConnectedWireBitDepth(HashedString port_id)
	{
		LogicWire.BitDepth result = LogicWire.BitDepth.NumRatings;
		int portCell = this.GetPortCell(port_id);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component = gameObject.GetComponent<LogicWire>();
			if (component != null)
			{
				result = component.MaxBitDepth;
			}
		}
		return result;
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x00159F74 File Offset: 0x00158174
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		LogicPorts component = go.GetComponent<LogicPorts>();
		if (component != null)
		{
			if (component.inputPortInfo != null && component.inputPortInfo.Length != 0)
			{
				Descriptor item = new Descriptor(UI.LOGIC_PORTS.INPUT_PORTS, UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item);
				foreach (LogicPorts.Port port in component.inputPortInfo)
				{
					string tooltip = string.Format(UI.LOGIC_PORTS.INPUT_PORT_TOOLTIP, port.activeDescription, port.inactiveDescription);
					item = new Descriptor(port.description, tooltip, Descriptor.DescriptorType.Effect, false);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			if (component.outputPortInfo != null && component.outputPortInfo.Length != 0)
			{
				Descriptor item2 = new Descriptor(UI.LOGIC_PORTS.OUTPUT_PORTS, UI.LOGIC_PORTS.OUTPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item2);
				foreach (LogicPorts.Port port2 in component.outputPortInfo)
				{
					string tooltip2 = string.Format(UI.LOGIC_PORTS.OUTPUT_PORT_TOOLTIP, port2.activeDescription, port2.inactiveDescription);
					item2 = new Descriptor(port2.description, tooltip2, Descriptor.DescriptorType.Effect, false);
					item2.IncreaseIndent();
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x06003E38 RID: 15928 RVA: 0x0015A0DC File Offset: 0x001582DC
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.isPhysical && this.outputPorts != null)
		{
			this.serializedOutputValues = new int[this.outputPorts.Count];
			for (int i = 0; i < this.outputPorts.Count; i++)
			{
				LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
				this.serializedOutputValues[i] = logicEventSender.GetLogicValue();
			}
		}
	}

	// Token: 0x06003E39 RID: 15929 RVA: 0x0015A145 File Offset: 0x00158345
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedOutputValues = null;
	}

	// Token: 0x04002860 RID: 10336
	[SerializeField]
	public LogicPorts.Port[] outputPortInfo;

	// Token: 0x04002861 RID: 10337
	[SerializeField]
	public LogicPorts.Port[] inputPortInfo;

	// Token: 0x04002862 RID: 10338
	public List<ILogicUIElement> outputPorts;

	// Token: 0x04002863 RID: 10339
	public List<ILogicUIElement> inputPorts;

	// Token: 0x04002864 RID: 10340
	private int cell = -1;

	// Token: 0x04002865 RID: 10341
	private Orientation orientation = Orientation.NumRotations;

	// Token: 0x04002866 RID: 10342
	[Serialize]
	private int[] serializedOutputValues;

	// Token: 0x04002867 RID: 10343
	private bool isPhysical;

	// Token: 0x02001622 RID: 5666
	[Serializable]
	public struct Port
	{
		// Token: 0x060089CB RID: 35275 RVA: 0x00311F6A File Offset: 0x0031016A
		public Port(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon, LogicPortSpriteType sprite_type, bool display_custom_name = false)
		{
			this.id = id;
			this.cellOffset = cell_offset;
			this.description = description;
			this.activeDescription = activeDescription;
			this.inactiveDescription = inactiveDescription;
			this.requiresConnection = show_wire_missing_icon;
			this.spriteType = sprite_type;
			this.displayCustomName = display_custom_name;
		}

		// Token: 0x060089CC RID: 35276 RVA: 0x00311FA9 File Offset: 0x003101A9
		public static LogicPorts.Port InputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Input, display_custom_name);
		}

		// Token: 0x060089CD RID: 35277 RVA: 0x00311FBB File Offset: 0x003101BB
		public static LogicPorts.Port OutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Output, display_custom_name);
		}

		// Token: 0x060089CE RID: 35278 RVA: 0x00311FCD File Offset: 0x003101CD
		public static LogicPorts.Port RibbonInputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonInput, display_custom_name);
		}

		// Token: 0x060089CF RID: 35279 RVA: 0x00311FDF File Offset: 0x003101DF
		public static LogicPorts.Port RibbonOutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonOutput, display_custom_name);
		}

		// Token: 0x04006AC3 RID: 27331
		public HashedString id;

		// Token: 0x04006AC4 RID: 27332
		public CellOffset cellOffset;

		// Token: 0x04006AC5 RID: 27333
		public string description;

		// Token: 0x04006AC6 RID: 27334
		public string activeDescription;

		// Token: 0x04006AC7 RID: 27335
		public string inactiveDescription;

		// Token: 0x04006AC8 RID: 27336
		public bool requiresConnection;

		// Token: 0x04006AC9 RID: 27337
		public LogicPortSpriteType spriteType;

		// Token: 0x04006ACA RID: 27338
		public bool displayCustomName;
	}
}
