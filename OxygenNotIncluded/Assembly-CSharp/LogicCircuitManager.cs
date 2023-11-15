using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

// Token: 0x0200084C RID: 2124
public class LogicCircuitManager
{
	// Token: 0x06003DE8 RID: 15848 RVA: 0x0015849C File Offset: 0x0015669C
	public LogicCircuitManager(UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduit_system)
	{
		this.conduitSystem = conduit_system;
		this.timeSinceBridgeRefresh = 0f;
		this.elapsedTime = 0f;
		for (int i = 0; i < 2; i++)
		{
			this.bridgeGroups[i] = new List<LogicUtilityNetworkLink>();
		}
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x001584FC File Offset: 0x001566FC
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x00158508 File Offset: 0x00156708
	private void Refresh(float dt)
	{
		if (this.conduitSystem.IsDirty)
		{
			this.conduitSystem.Update();
			LogicCircuitNetwork.logicSoundRegister.Clear();
			this.PropagateSignals(true);
			this.elapsedTime = 0f;
		}
		else if (SpeedControlScreen.Instance != null && !SpeedControlScreen.Instance.IsPaused)
		{
			this.elapsedTime += dt;
			this.timeSinceBridgeRefresh += dt;
			while (this.elapsedTime > LogicCircuitManager.ClockTickInterval)
			{
				this.elapsedTime -= LogicCircuitManager.ClockTickInterval;
				this.PropagateSignals(false);
				if (this.onLogicTick != null)
				{
					this.onLogicTick();
				}
			}
			if (this.timeSinceBridgeRefresh > LogicCircuitManager.BridgeRefreshInterval)
			{
				this.UpdateCircuitBridgeLists();
				this.timeSinceBridgeRefresh = 0f;
			}
		}
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			this.CheckCircuitOverloaded(dt, logicCircuitNetwork.id, logicCircuitNetwork.GetBitsUsed());
		}
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x0015863C File Offset: 0x0015683C
	private void PropagateSignals(bool force_send_events)
	{
		IList<UtilityNetwork> networks = Game.Instance.logicCircuitSystem.GetNetworks();
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			((LogicCircuitNetwork)utilityNetwork).UpdateLogicValue();
		}
		foreach (UtilityNetwork utilityNetwork2 in networks)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork2;
			logicCircuitNetwork.SendLogicEvents(force_send_events, logicCircuitNetwork.id);
		}
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x001586D8 File Offset: 0x001568D8
	public LogicCircuitNetwork GetNetworkForCell(int cell)
	{
		return this.conduitSystem.GetNetworkForCell(cell) as LogicCircuitNetwork;
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x001586EB File Offset: 0x001568EB
	public void AddVisElem(ILogicUIElement elem)
	{
		this.uiVisElements.Add(elem);
		if (this.onElemAdded != null)
		{
			this.onElemAdded(elem);
		}
	}

	// Token: 0x06003DEE RID: 15854 RVA: 0x0015870D File Offset: 0x0015690D
	public void RemoveVisElem(ILogicUIElement elem)
	{
		if (this.onElemRemoved != null)
		{
			this.onElemRemoved(elem);
		}
		this.uiVisElements.Remove(elem);
	}

	// Token: 0x06003DEF RID: 15855 RVA: 0x00158730 File Offset: 0x00156930
	public ReadOnlyCollection<ILogicUIElement> GetVisElements()
	{
		return this.uiVisElements.AsReadOnly();
	}

	// Token: 0x06003DF0 RID: 15856 RVA: 0x0015873D File Offset: 0x0015693D
	public static void ToggleNoWireConnected(bool show_missing_wire, GameObject go)
	{
		go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoLogicWireConnected, show_missing_wire, null);
	}

	// Token: 0x06003DF1 RID: 15857 RVA: 0x0015875C File Offset: 0x0015695C
	private void CheckCircuitOverloaded(float dt, int id, int bits_used)
	{
		UtilityNetwork networkByID = Game.Instance.logicCircuitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)networkByID;
			if (logicCircuitNetwork != null)
			{
				logicCircuitNetwork.UpdateOverloadTime(dt, bits_used);
			}
		}
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x0015878F File Offset: 0x0015698F
	public void Connect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Add(bridge);
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x001587A4 File Offset: 0x001569A4
	public void Disconnect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Remove(bridge);
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x001587BC File Offset: 0x001569BC
	private void UpdateCircuitBridgeLists()
	{
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			if (this.updateEvenBridgeGroups)
			{
				if (logicCircuitNetwork.id % 2 == 0)
				{
					logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
				}
			}
			else if (logicCircuitNetwork.id % 2 == 1)
			{
				logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
			}
		}
		this.updateEvenBridgeGroups = !this.updateEvenBridgeGroups;
	}

	// Token: 0x0400283B RID: 10299
	public static float ClockTickInterval = 0.1f;

	// Token: 0x0400283C RID: 10300
	private float elapsedTime;

	// Token: 0x0400283D RID: 10301
	private UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduitSystem;

	// Token: 0x0400283E RID: 10302
	private List<ILogicUIElement> uiVisElements = new List<ILogicUIElement>();

	// Token: 0x0400283F RID: 10303
	public static float BridgeRefreshInterval = 1f;

	// Token: 0x04002840 RID: 10304
	private List<LogicUtilityNetworkLink>[] bridgeGroups = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04002841 RID: 10305
	private bool updateEvenBridgeGroups;

	// Token: 0x04002842 RID: 10306
	private float timeSinceBridgeRefresh;

	// Token: 0x04002843 RID: 10307
	public System.Action onLogicTick;

	// Token: 0x04002844 RID: 10308
	public Action<ILogicUIElement> onElemAdded;

	// Token: 0x04002845 RID: 10309
	public Action<ILogicUIElement> onElemRemoved;

	// Token: 0x0200161F RID: 5663
	private struct Signal
	{
		// Token: 0x060089C5 RID: 35269 RVA: 0x00311F2C File Offset: 0x0031012C
		public Signal(int cell, int value)
		{
			this.cell = cell;
			this.value = value;
		}

		// Token: 0x04006ABC RID: 27324
		public int cell;

		// Token: 0x04006ABD RID: 27325
		public int value;
	}
}
