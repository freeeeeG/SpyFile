using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public abstract class ServerHazardBase : ServerSynchroniserBase, IHandleGridTransfer
{
	// Token: 0x06001603 RID: 5635 RVA: 0x00075113 File Offset: 0x00073513
	protected virtual void Awake()
	{
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x00075115 File Offset: 0x00073515
	protected virtual void Start()
	{
		this.m_gridLocation = base.gameObject.RequireInterface<IGridLocation>();
		this.m_gridManager = this.m_gridLocation.AccessGridManager;
	}

	// Token: 0x06001605 RID: 5637
	public abstract void HandleTransfer(GridIndex _index, GameObject _object);

	// Token: 0x06001606 RID: 5638 RVA: 0x00075139 File Offset: 0x00073539
	public bool CanHandleTransfer(GridIndex _index, GameObject _object)
	{
		return true;
	}

	// Token: 0x040010A2 RID: 4258
	private GridManager m_gridManager;

	// Token: 0x040010A3 RID: 4259
	private IGridLocation m_gridLocation;
}
