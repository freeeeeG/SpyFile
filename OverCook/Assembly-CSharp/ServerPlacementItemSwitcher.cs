using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class ServerPlacementItemSwitcher : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x0600192E RID: 6446 RVA: 0x0007F738 File Offset: 0x0007DB38
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_placementItemSwitcher = (PlacementItemSwitcher)synchronisedObject;
		this.m_attachStation = base.GetComponent<ServerAttachStation>();
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x0007F759 File Offset: 0x0007DB59
	public override EntityType GetEntityType()
	{
		return EntityType.PickupItemSwitcher;
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x0007F760 File Offset: 0x0007DB60
	public void OnTrigger(string _trigger)
	{
		if (this.m_placementItemSwitcher.enabled && _trigger == this.m_placementItemSwitcher.m_switchTrigger)
		{
			this.m_currentItemPrefabIndex++;
			this.m_currentItemPrefabIndex %= this.m_placementItemSwitcher.m_ingredients.Length;
			this.m_message.m_itemIndex = this.m_currentItemPrefabIndex;
			this.SendServerEvent(this.m_message);
		}
	}

	// Token: 0x04001422 RID: 5154
	private PlacementItemSwitcher m_placementItemSwitcher;

	// Token: 0x04001423 RID: 5155
	private ServerAttachStation m_attachStation;

	// Token: 0x04001424 RID: 5156
	private int m_currentItemPrefabIndex;

	// Token: 0x04001425 RID: 5157
	private PickupItemSwitcherMessage m_message = new PickupItemSwitcherMessage();
}
