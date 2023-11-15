using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200063A RID: 1594
[AddComponentMenu("KMonoBehaviour/scripts/LogicOperationalController")]
public class LogicOperationalController : KMonoBehaviour
{
	// Token: 0x0600294A RID: 10570 RVA: 0x000DF094 File Offset: 0x000DD294
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, UI.LOGIC_PORTS.CONTROL_OPERATIONAL, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
		};
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000DF0D8 File Offset: 0x000DD2D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicOperationalController>(-801688580, LogicOperationalController.OnLogicValueChangedDelegate);
		if (LogicOperationalController.infoStatusItem == null)
		{
			LogicOperationalController.infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicOperationalController.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicOperationalController.ResolveInfoStatusItemString);
		}
		this.CheckWireState();
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000DF148 File Offset: 0x000DD348
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(LogicOperationalController.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000DF178 File Offset: 0x000DD378
	private LogicCircuitNetwork CheckWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		int value = (network != null) ? network.OutputValue : this.unNetworkedValue;
		this.operational.SetFlag(LogicOperationalController.LogicOperationalFlag, LogicCircuitNetwork.IsBitActive(0, value));
		return network;
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x000DF1B6 File Offset: 0x000DD3B6
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		return ((LogicOperationalController)data).operational.GetFlag(LogicOperationalController.LogicOperationalFlag) ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x000DF1E0 File Offset: 0x000DD3E0
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == LogicOperationalController.PORT_ID)
		{
			LogicCircuitNetwork logicCircuitNetwork = this.CheckWireState();
			base.GetComponent<KSelectable>().ToggleStatusItem(LogicOperationalController.infoStatusItem, logicCircuitNetwork != null, this);
		}
	}

	// Token: 0x0400183E RID: 6206
	public static readonly HashedString PORT_ID = "LogicOperational";

	// Token: 0x0400183F RID: 6207
	public int unNetworkedValue = 1;

	// Token: 0x04001840 RID: 6208
	public static readonly Operational.Flag LogicOperationalFlag = new Operational.Flag("LogicOperational", Operational.Flag.Type.Requirement);

	// Token: 0x04001841 RID: 6209
	private static StatusItem infoStatusItem;

	// Token: 0x04001842 RID: 6210
	[MyCmpGet]
	public Operational operational;

	// Token: 0x04001843 RID: 6211
	private static readonly EventSystem.IntraObjectHandler<LogicOperationalController> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicOperationalController>(delegate(LogicOperationalController component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
