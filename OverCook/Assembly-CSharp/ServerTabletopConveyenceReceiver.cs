using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000597 RID: 1431
public class ServerTabletopConveyenceReceiver : ServerSynchroniserBase, IConveyenceReceiver
{
	// Token: 0x06001B24 RID: 6948 RVA: 0x00086DD4 File Offset: 0x000851D4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_attachStation = base.gameObject.RequireComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowPlacement));
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x00086E37 File Offset: 0x00085237
	public bool IsReceiving()
	{
		return this.m_receiving;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x00086E3F File Offset: 0x0008523F
	private bool AllowPlacement(GameObject _object, PlacementContext _context)
	{
		return !this.m_receiving;
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x00086E4A File Offset: 0x0008524A
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_itemJustRemoved)
		{
			this.m_itemJustRemoved = false;
			this.RefreshConveyTo();
		}
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x00086E6C File Offset: 0x0008526C
	private void OnItemAdded(IAttachment _attachment)
	{
		this.m_item = _attachment;
		ServerLimitedQuantityItem component = this.m_item.AccessGameObject().GetComponent<ServerLimitedQuantityItem>();
		if (null != component)
		{
			component.RegisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		this.m_itemJustRemoved = false;
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x00086EC4 File Offset: 0x000852C4
	private void OnItemRemoved(IAttachment _attachment)
	{
		ServerLimitedQuantityItem component = this.m_item.AccessGameObject().GetComponent<ServerLimitedQuantityItem>();
		if (null != component)
		{
			component.UnregisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		this.m_item = null;
		this.m_itemJustRemoved = true;
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x00086F1C File Offset: 0x0008531C
	private void OnAttachmentDestroyed(GameObject toDeDestroyed)
	{
		IAttachment attachment = toDeDestroyed.RequestInterface<IAttachment>();
		if (this.m_item != null && attachment == this.m_item)
		{
			this.OnItemRemoved(attachment);
		}
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x00086F4E File Offset: 0x0008534E
	public void InformStartingConveyToMe()
	{
		this.m_receiving = true;
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x00086F62 File Offset: 0x00085362
	public void InformEndingConveyToMe()
	{
		this.m_receiving = false;
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x00086F78 File Offset: 0x00085378
	public IEnumerator ConveyToMe(ServerConveyorStation _priorConveyor, IAttachment _object)
	{
		ServerAttachStation attachStation = _priorConveyor.gameObject.RequireComponent<ServerAttachStation>();
		ServerAttachStation attachStation2 = this.m_attachStation;
		Transform attachPoint = attachStation.GetAttachPoint(_object.AccessGameObject());
		Transform attachPoint2 = attachStation2.GetAttachPoint(_object.AccessGameObject());
		float prop = 0f;
		float speed = _priorConveyor.GetConveySpeed();
		bool aborted = false;
		ServerAttachStation.OnItemAdded addedDuringConveying = delegate(IAttachment _iHoldable)
		{
			aborted = true;
		};
		ServerAttachStation.OnItemRemoved removedDuringConveying = delegate(IAttachment _iHoldable)
		{
			aborted = true;
		};
		attachStation2.RegisterOnItemAdded(addedDuringConveying);
		attachStation.RegisterOnItemRemoved(removedDuringConveying);
		do
		{
			prop = Mathf.Clamp01(prop + TimeManager.GetDeltaTime(base.gameObject) * speed);
			yield return null;
		}
		while (prop < 0.5f && !aborted);
		attachStation2.UnregisterOnItemAdded(addedDuringConveying);
		attachStation.UnregisterOnItemRemoved(removedDuringConveying);
		if (aborted)
		{
			yield break;
		}
		_priorConveyor.TakeResponsibilityForItem();
		attachStation2.AddItem(_object.AccessGameObject(), _object.AccessGameObject().transform.forward.XZ(), default(PlacementContext));
		aborted = false;
		attachStation2.RegisterOnItemRemoved(removedDuringConveying);
		do
		{
			prop = Mathf.Clamp01(prop + TimeManager.GetDeltaTime(base.gameObject) * speed);
			yield return null;
		}
		while (prop < 1f && !aborted);
		attachStation2.UnregisterOnItemRemoved(removedDuringConveying);
		yield break;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x00086FA4 File Offset: 0x000853A4
	public bool CanConveyTo(IAttachment _itemToConvey)
	{
		if (_itemToConvey != null)
		{
			return !this.m_itemJustRemoved && !this.m_receiving && this.m_attachStation.CanAttachToSelf(_itemToConvey.AccessGameObject(), default(PlacementContext)) && !this.m_allowConveyToCallbacks.CallForResult(false);
		}
		return this.m_item == null && !this.m_itemJustRemoved && !this.m_receiving && !this.m_allowConveyToCallbacks.CallForResult(false);
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x00087032 File Offset: 0x00085432
	public void RegisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
		this.m_refreshedConveyToCallback = (CallbackVoid)Delegate.Combine(this.m_refreshedConveyToCallback, _callback);
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x0008704B File Offset: 0x0008544B
	public void UnregisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
		this.m_refreshedConveyToCallback = (CallbackVoid)Delegate.Remove(this.m_refreshedConveyToCallback, _callback);
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x00087064 File Offset: 0x00085464
	public void RefreshConveyTo()
	{
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x00087071 File Offset: 0x00085471
	public void RegisterAllowConveyToCallback(Generic<bool> _allowConveyCallback)
	{
		this.m_allowConveyToCallbacks.Add(_allowConveyCallback);
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x0008707F File Offset: 0x0008547F
	public void UnregisterAllowConveyToCallback(Generic<bool> _allowConveyCallback)
	{
		this.m_allowConveyToCallbacks.Remove(_allowConveyCallback);
	}

	// Token: 0x04001562 RID: 5474
	private ServerAttachStation m_attachStation;

	// Token: 0x04001563 RID: 5475
	private bool m_receiving;

	// Token: 0x04001564 RID: 5476
	private bool m_itemJustRemoved;

	// Token: 0x04001565 RID: 5477
	private IAttachment m_item;

	// Token: 0x04001566 RID: 5478
	private CallbackVoid m_refreshedConveyToCallback = delegate()
	{
	};

	// Token: 0x04001567 RID: 5479
	private List<Generic<bool>> m_allowConveyToCallbacks = new List<Generic<bool>>();
}
