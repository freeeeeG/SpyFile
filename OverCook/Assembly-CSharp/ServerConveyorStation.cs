using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public class ServerConveyorStation : ServerSynchroniserBase
{
	// Token: 0x06001A77 RID: 6775 RVA: 0x00084AF8 File Offset: 0x00082EF8
	public override EntityType GetEntityType()
	{
		return EntityType.ConveyorStation;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x00084AFC File Offset: 0x00082EFC
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_attachStation = base.gameObject.RequireComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_selfReceiver = base.gameObject.RequireComponent<ServerTabletopConveyenceReceiver>();
		this.m_selfReceiver.RegisterRefreshedConveyToCallback(new CallbackVoid(this.RefreshStartConveying));
		this.m_pendingAdjacentUpdate = true;
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x00084B77 File Offset: 0x00082F77
	public void RegisterConveyStateChangedCallback(CallbackBool _callback)
	{
		this.m_conveyStateChanged = (CallbackBool)Delegate.Combine(this.m_conveyStateChanged, _callback);
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x00084B90 File Offset: 0x00082F90
	public void UnregisterConveyStateChangedCallback(CallbackBool _callback)
	{
		this.m_conveyStateChanged = (CallbackBool)Delegate.Remove(this.m_conveyStateChanged, _callback);
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x00084BA9 File Offset: 0x00082FA9
	public void RegisterAllowConveyCallback(Generic<bool> _allowConveyCallback)
	{
		this.m_allowConveyCallbacks.Add(_allowConveyCallback);
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x00084BB7 File Offset: 0x00082FB7
	public void UnregisterAllowConveyCallback(Generic<bool> _allowConveyCallback)
	{
		this.m_allowConveyCallbacks.Remove(_allowConveyCallback);
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x00084BC6 File Offset: 0x00082FC6
	private void Awake()
	{
		this.m_conveyor = base.gameObject.RequireComponent<ConveyorStation>();
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x00084BDC File Offset: 0x00082FDC
	protected override void OnEnable()
	{
		base.OnEnable();
		StaticGridLocation staticGridLocation = base.gameObject.RequireComponent<StaticGridLocation>();
		this.m_gridManager = staticGridLocation.AccessGridManager;
		this.m_gridIndex = staticGridLocation.GridIndex;
		this.UpdateAdjacentReceiver();
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x00084C19 File Offset: 0x00083019
	public override void UpdateSynchronising()
	{
		if (this.m_pendingAdjacentUpdate)
		{
			this.m_pendingAdjacentUpdate = false;
			this.UpdateAdjacentReceiver();
		}
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x00084C34 File Offset: 0x00083034
	private void OnItemAdded(IAttachment _attachment)
	{
		this.m_item = _attachment;
		this.m_itemBeingDestroyed = false;
		LimitedQuantityItem limitedQuantityItem = this.m_item.AccessGameObject().RequestComponent<LimitedQuantityItem>();
		if (limitedQuantityItem != null)
		{
			limitedQuantityItem.RegisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.ImpendingDestruction));
		}
		this.m_responsibleForItem = this.m_item;
		base.StartCoroutine(this.DelayRefreshStartConveying());
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x00084C97 File Offset: 0x00083097
	private void ImpendingDestruction(GameObject _destroying)
	{
		this.m_itemBeingDestroyed = true;
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x00084CA0 File Offset: 0x000830A0
	private IEnumerator DelayRefreshStartConveying()
	{
		yield return null;
		this.RefreshStartConveying();
		this.m_selfReceiver.RefreshConveyTo();
		yield break;
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x00084CBC File Offset: 0x000830BC
	private void OnItemRemoved(IAttachment _attachment)
	{
		IAttachment item = this.m_item;
		IAttachment responsibleForItem = this.m_responsibleForItem;
		LimitedQuantityItem limitedQuantityItem = this.m_item.AccessGameObject().RequestComponent<LimitedQuantityItem>();
		if (limitedQuantityItem != null)
		{
			limitedQuantityItem.UnregisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.ImpendingDestruction));
		}
		this.m_itemBeingDestroyed = false;
		this.m_item = null;
		this.m_responsibleForItem = null;
		if (responsibleForItem != null)
		{
			this.EndConveyence();
		}
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x00084D27 File Offset: 0x00083127
	public void UpdateAdjacentReceiver()
	{
		if (null != this.m_gridManager)
		{
			this.SetAdjacentReceiver(this.GetAdjacentReceiver());
		}
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x00084D48 File Offset: 0x00083148
	private void SetAdjacentReceiver(IConveyenceReceiver _receiver)
	{
		if (this.m_adjacentReceiver != _receiver)
		{
			if (this.m_adjacentReceiver != null)
			{
				this.m_adjacentReceiver.UnregisterRefreshedConveyToCallback(new CallbackVoid(this.RefreshStartConveying));
			}
			this.m_adjacentReceiver = _receiver;
			if (this.m_adjacentReceiver != null)
			{
				this.m_adjacentReceiver.RegisterRefreshedConveyToCallback(new CallbackVoid(this.RefreshStartConveying));
			}
			base.StartCoroutine(this.DelayRefreshStartConveying());
		}
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x00084DB9 File Offset: 0x000831B9
	public bool CanConveyToAdjacent()
	{
		return this.m_adjacentReceiver != null && this.m_adjacentReceiver.CanConveyTo(this.m_item) && !this.m_allowConveyCallbacks.CallForResult(false);
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x00084DEE File Offset: 0x000831EE
	public bool IsConveying()
	{
		return this.m_conveying;
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x00084DF6 File Offset: 0x000831F6
	private void SetIsConveying(bool _isConveying)
	{
		this.m_conveying = _isConveying;
		this.m_conveyStateChanged(this.m_conveying);
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x00084E10 File Offset: 0x00083210
	private void RefreshStartConveying()
	{
		if (this.CanConveyToAdjacent() && this.m_item != null && !this.m_itemBeingDestroyed && !this.m_selfReceiver.IsReceiving() && !this.m_conveying)
		{
			this.SetIsConveying(true);
			this.m_adjacentReceiver.InformStartingConveyToMe();
			this.m_data.m_receiverEntityID = (this.m_adjacentReceiver as ServerSynchroniserBase).GetEntityId();
			this.m_data.m_itemEntityID = (this.m_item as ServerSynchroniserBase).GetEntityId();
			float num = ClientTime.Time();
			this.m_data.m_arriveTime = num + 1f / this.GetConveySpeed();
			this.SendServerEvent(this.m_data);
			base.StartCoroutine(this.ConveyTo(this.m_adjacentReceiver));
		}
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x00084EE0 File Offset: 0x000832E0
	public IEnumerator ConveyTo(IConveyenceReceiver _receiver)
	{
		this.m_conveyingAwayRoutine = _receiver.ConveyToMe(this, this.m_item);
		yield return base.StartCoroutine(this.m_conveyingAwayRoutine);
		this.EndConveyence();
		yield break;
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x00084F04 File Offset: 0x00083304
	public void EndConveyence()
	{
		if (this.m_conveyingAwayRoutine != null)
		{
			base.StopCoroutine(this.m_conveyingAwayRoutine);
		}
		this.SetIsConveying(false);
		if (this.m_adjacentReceiver != null)
		{
			this.m_adjacentReceiver.InformEndingConveyToMe();
		}
		this.RefreshStartConveying();
		this.m_conveyingAwayRoutine = null;
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x00084F52 File Offset: 0x00083352
	public GameObject TakeResponsibilityForItem()
	{
		this.m_responsibleForItem = null;
		return this.m_attachStation.TakeItem();
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x00084F68 File Offset: 0x00083368
	public IConveyenceReceiver GetAdjacentReceiver()
	{
		GridIndex nextGridIndex = this.GetNextGridIndex();
		GameObject gridOccupant = this.m_gridManager.GetGridOccupant(nextGridIndex);
		if (gridOccupant != null)
		{
			return gridOccupant.RequestInterface<IConveyenceReceiver>();
		}
		return null;
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x00084FA0 File Offset: 0x000833A0
	public GridIndex GetNextGridIndex()
	{
		int num = (int)Mathf.Round(-base.transform.right.x);
		int num2 = (int)Mathf.Round(-base.transform.right.z);
		ConveyorStation.XZDirection conveyanceDirectionXZ = this.m_conveyor.m_conveyanceDirectionXZ;
		if (conveyanceDirectionXZ == ConveyorStation.XZDirection.Leftwards)
		{
			return new GridIndex(this.m_gridIndex.X - num, this.m_gridIndex.Y, this.m_gridIndex.Z - num2);
		}
		if (conveyanceDirectionXZ != ConveyorStation.XZDirection.Rightwards)
		{
			return new GridIndex(int.MaxValue, int.MaxValue, int.MaxValue);
		}
		return new GridIndex(this.m_gridIndex.X + num, this.m_gridIndex.Y, this.m_gridIndex.Z + num2);
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x0008506E File Offset: 0x0008346E
	public float GetConveySpeed()
	{
		return this.m_conveyor.m_conveySpeed;
	}

	// Token: 0x040014F2 RID: 5362
	private ConveyorStation m_conveyor;

	// Token: 0x040014F3 RID: 5363
	private ConveyorStationMessage m_data = new ConveyorStationMessage();

	// Token: 0x040014F4 RID: 5364
	private ServerTabletopConveyenceReceiver m_selfReceiver;

	// Token: 0x040014F5 RID: 5365
	private ServerAttachStation m_attachStation;

	// Token: 0x040014F6 RID: 5366
	private GridManager m_gridManager;

	// Token: 0x040014F7 RID: 5367
	private GridIndex m_gridIndex;

	// Token: 0x040014F8 RID: 5368
	private IConveyenceReceiver m_adjacentReceiver;

	// Token: 0x040014F9 RID: 5369
	private CallbackBool m_conveyStateChanged = delegate(bool _conveying)
	{
	};

	// Token: 0x040014FA RID: 5370
	private List<Generic<bool>> m_allowConveyCallbacks = new List<Generic<bool>>();

	// Token: 0x040014FB RID: 5371
	private bool m_pendingAdjacentUpdate;

	// Token: 0x040014FC RID: 5372
	private IAttachment m_responsibleForItem;

	// Token: 0x040014FD RID: 5373
	private bool m_conveying;

	// Token: 0x040014FE RID: 5374
	private IAttachment m_item;

	// Token: 0x040014FF RID: 5375
	private bool m_itemBeingDestroyed;

	// Token: 0x04001500 RID: 5376
	private IEnumerator m_conveyingAwayRoutine;
}
