using System;
using UnityEngine;

// Token: 0x0200059A RID: 1434
public class TabletopConveyenceWindReceiver : TabletopWindReceiver
{
	// Token: 0x06001B43 RID: 6979 RVA: 0x000876C0 File Offset: 0x00085AC0
	protected override void Start()
	{
		this.m_receiver = base.gameObject.RequestInterface<IConveyenceReceiver>();
		this.m_conveyor = base.gameObject.RequestComponent<ServerConveyorStation>();
		if (this.m_conveyor != null)
		{
			this.m_conveyor.RegisterAllowConveyCallback(new Generic<bool>(this.AllowConveying));
		}
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x00087718 File Offset: 0x00085B18
	protected override bool ShouldDetach(GameObject _object)
	{
		return (!(this.m_conveyor != null) || !this.m_conveyor.IsConveying()) && (this.m_receiver == null || !this.m_receiver.IsReceiving()) && base.ShouldDetach(_object);
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x0008776C File Offset: 0x00085B6C
	private bool AllowConveying()
	{
		return !base.IsTooMuchWind(base.GetVelocity());
	}

	// Token: 0x0400156D RID: 5485
	private ServerConveyorStation m_conveyor;

	// Token: 0x0400156E RID: 5486
	private IConveyenceReceiver m_receiver;
}
