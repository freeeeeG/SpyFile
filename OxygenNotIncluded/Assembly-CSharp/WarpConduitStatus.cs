using System;
using UnityEngine;

// Token: 0x020006B6 RID: 1718
public static class WarpConduitStatus
{
	// Token: 0x06002EC0 RID: 11968 RVA: 0x000F6F0C File Offset: 0x000F510C
	public static void UpdateWarpConduitsOperational(GameObject sender, GameObject receiver)
	{
		object obj = sender != null && sender.GetComponent<Activatable>().IsActivated;
		bool flag = receiver != null && receiver.GetComponent<Activatable>().IsActivated;
		object obj2 = obj;
		bool value = (obj2 & flag) != null;
		int num = 0;
		if (obj2 != null)
		{
			num++;
		}
		if (flag)
		{
			num++;
		}
		if (sender != null)
		{
			sender.GetComponent<Operational>().SetFlag(WarpConduitStatus.warpConnectedFlag, value);
			if (num != 2)
			{
				sender.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
				sender.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, num);
			}
			else
			{
				sender.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
			}
		}
		if (receiver != null)
		{
			receiver.GetComponent<Operational>().SetFlag(WarpConduitStatus.warpConnectedFlag, value);
			if (num != 2)
			{
				receiver.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
				receiver.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, num);
				return;
			}
			receiver.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
		}
	}

	// Token: 0x04001BB0 RID: 7088
	public static readonly Operational.Flag warpConnectedFlag = new Operational.Flag("warp_conduit_connected", Operational.Flag.Type.Requirement);
}
