using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class ServerAutoDestructEmptyPlateStack : ServerSynchroniserBase, ISurfacePlacementNotified
{
	// Token: 0x06001CD2 RID: 7378 RVA: 0x0008D548 File Offset: 0x0008B948
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_autoDestruct = (AutoDestructEmptyPlateStack)synchronisedObject;
		this.m_plateStack = base.gameObject.RequestComponent<ServerPlateStackBase>();
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x0008D570 File Offset: 0x0008B970
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (!this.ActiveAndEnabled || this.m_autoDestruct == null || !this.m_autoDestruct.enabled)
		{
			return;
		}
		if (this.m_plateStack == null || this.m_plateStack.gameObject == null)
		{
			return;
		}
		if (this.m_plateStack.GetSize() <= 0)
		{
			if (this.m_attachStation != null)
			{
				this.m_attachStation.TakeItem();
			}
			NetworkUtils.DestroyObjectsRecursive(this.m_plateStack.gameObject);
		}
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x0008D616 File Offset: 0x0008BA16
	public void OnSurfacePlacement(ServerAttachStation _station)
	{
		this.m_attachStation = _station;
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x0008D61F File Offset: 0x0008BA1F
	public void OnSurfaceDeplacement(ServerAttachStation _station)
	{
		this.m_attachStation = null;
	}

	// Token: 0x04001672 RID: 5746
	private AutoDestructEmptyPlateStack m_autoDestruct;

	// Token: 0x04001673 RID: 5747
	private ServerPlateStackBase m_plateStack;

	// Token: 0x04001674 RID: 5748
	private ServerAttachStation m_attachStation;
}
