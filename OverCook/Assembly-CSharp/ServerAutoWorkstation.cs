using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class ServerAutoWorkstation : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06001A48 RID: 6728 RVA: 0x000839CE File Offset: 0x00081DCE
	public override EntityType GetEntityType()
	{
		return EntityType.AutoWorkstation;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x000839D4 File Offset: 0x00081DD4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_autoWorkstation = (AutoWorkstation)synchronisedObject;
		this.m_interactable = base.gameObject.GetComponent<ServerInteractable>();
		if (this.m_interactable != null)
		{
			this.m_interactable.RegisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnInteracterAdded), new ServerInteractable.EndInteractCallback(this.OnInteracterRemoved));
			this.m_interactable.SetInteractionSuppressed(true);
		}
		this.m_attachStations = base.gameObject.RequestComponentsInImmediateChildren<ServerAttachStation>();
		this.m_workableItems = new ServerWorkableItem[this.m_attachStations.Length];
		for (int i = 0; i < this.m_attachStations.Length; i++)
		{
			int index = i;
			this.m_attachStations[i].RegisterOnItemAdded(delegate(IAttachment x)
			{
				this.OnItemAdded(x, index);
			});
			this.m_attachStations[i].RegisterOnItemRemoved(delegate(IAttachment x)
			{
				this.OnItemRemovedItem(x, index);
			});
			this.m_attachStations[i].RegisterAllowItemPickup(() => this.CanPickupItem(index));
			this.m_attachStations[i].RegisterAllowItemPlacement((GameObject x, PlacementContext y) => this.CanPlaceItem(x, y, index));
		}
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x00083AF4 File Offset: 0x00081EF4
	private void SynchroniseInteractionState(bool _active)
	{
		this.m_data.m_working = _active;
		if (_active)
		{
			Array.Resize<EntitySerialisationEntry>(ref this.m_data.m_items, this.m_workableItems.Length);
			for (int i = 0; i < this.m_workableItems.Length; i++)
			{
				if (this.m_workableItems[i] != null)
				{
					EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_workableItems[i].gameObject);
					this.m_data.m_items[i] = entry;
				}
				else
				{
					this.m_data.m_items[i] = null;
				}
			}
		}
		else
		{
			for (int j = 0; j < this.m_data.m_items.Length; j++)
			{
				this.m_data.m_items[j] = null;
			}
		}
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x00083BC8 File Offset: 0x00081FC8
	public override void OnDestroy()
	{
		for (int i = 0; i < this.m_attachStations.Length; i++)
		{
			ServerAttachStation serverAttachStation = this.m_attachStations[i];
		}
		if (this.m_interactable != null)
		{
			this.m_interactable.UnregisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnInteracterAdded), new ServerInteractable.EndInteractCallback(this.OnInteracterRemoved));
		}
		base.OnDestroy();
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x00083C34 File Offset: 0x00082034
	protected override void OnEnable()
	{
		base.OnEnable();
		for (int i = 0; i < this.m_workableItems.Length; i++)
		{
			if (this.m_workableItems[i] != null)
			{
				this.m_workableItems[i].enabled = true;
			}
		}
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x00083C84 File Offset: 0x00082084
	protected override void OnDisable()
	{
		base.OnDisable();
		for (int i = 0; i < this.m_workableItems.Length; i++)
		{
			if (this.m_workableItems[i] != null)
			{
				this.m_workableItems[i].enabled = false;
			}
		}
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x00083CD4 File Offset: 0x000820D4
	public override void UpdateSynchronising()
	{
		if (this.m_workAction == null)
		{
			return;
		}
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		this.m_workAction.Update(deltaTime);
		if (this.m_workAction.IsFinished())
		{
			if (this.m_interacters.Count != 0)
			{
				this.m_workAction.Reset();
			}
			else
			{
				this.StopWorking();
			}
		}
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x00083D3C File Offset: 0x0008213C
	private void StartWorking()
	{
		bool flag = true;
		for (int i = 0; i < this.m_attachStations.Length; i++)
		{
			if (this.m_workableItems[i] == null && this.m_attachStations[i] != null && this.m_attachStations[i].HasItem())
			{
				flag = false;
				break;
			}
		}
		AnimationEventData animationEventData = base.gameObject.RequireComponentRecursive<AnimationEventData>();
		if (flag)
		{
			float duration;
			float delay;
			animationEventData.GetTriggerData("Chop", "Impact", out duration, out delay);
			this.m_workAction = new ServerAutoWorkstation.WorkAction(delay, duration, new CallbackVoid(this.OnChop));
		}
		else
		{
			float duration;
			float delay;
			animationEventData.GetTriggerData("Jam", "Impact", out duration, out delay);
			this.m_workAction = new ServerAutoWorkstation.WorkAction(delay, duration, new CallbackVoid(this.OnJam));
		}
		this.SynchroniseInteractionState(true);
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x00083E20 File Offset: 0x00082220
	private void StopWorking()
	{
		this.m_workAction = null;
		this.SynchroniseInteractionState(false);
		if (this.m_autoWorkstation.m_workFinishedTrigger != string.Empty)
		{
			if (this.m_autoWorkstation.m_workFinishedTarget != null)
			{
				this.m_autoWorkstation.m_workFinishedTarget.SendTrigger(this.m_autoWorkstation.m_workFinishedTrigger);
			}
			else
			{
				base.gameObject.SendTrigger(this.m_autoWorkstation.m_workFinishedTrigger);
			}
		}
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x00083EA4 File Offset: 0x000822A4
	private void OnChop()
	{
		int num = this.m_attachStations.Length;
		for (int i = 0; i < num; i++)
		{
			ServerAttachStation station = this.m_attachStations[i];
			ServerWorkableItem serverWorkableItem = this.m_workableItems[i];
			if (serverWorkableItem != null)
			{
				serverWorkableItem.DoWork(station, base.gameObject, this.m_autoWorkstation.m_choppingPower);
			}
		}
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x00083F02 File Offset: 0x00082302
	private void OnJam()
	{
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x00083F04 File Offset: 0x00082304
	private bool CanPickupItem(int _stationIndex)
	{
		if (this.m_autoWorkstation.enabled && this.m_workAction != null)
		{
			ServerAttachStation serverAttachStation = this.m_attachStations[_stationIndex];
			if (serverAttachStation != null && serverAttachStation.HasItem())
			{
				ServerWorkableItem serverWorkableItem = this.m_workableItems[_stationIndex];
				if (serverWorkableItem != null)
				{
					return serverWorkableItem.HasFinished() || serverWorkableItem.GetProgress() == 0f;
				}
			}
		}
		return true;
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x00083F7E File Offset: 0x0008237E
	private bool CanPlaceItem(GameObject _item, PlacementContext _context, int _stationIndex)
	{
		return _context.m_source != PlacementContext.Source.Player || !this.m_autoWorkstation.enabled || this.m_workAction == null;
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x00083FAC File Offset: 0x000823AC
	private void OnItemAdded(IAttachment _iHoldable, int _stationIndex)
	{
		ServerWorkableItem serverWorkableItem = _iHoldable.AccessGameObject().RequestComponent<ServerWorkableItem>();
		if (serverWorkableItem != null)
		{
			this.m_workableItems[_stationIndex] = serverWorkableItem;
		}
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(false);
		}
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x00083FF8 File Offset: 0x000823F8
	private void OnItemRemovedItem(IAttachment _iHoldable, int _stationIndex)
	{
		ServerWorkableItem x = _iHoldable.AccessGameObject().RequestComponent<ServerWorkableItem>();
		if (x != null)
		{
			this.m_workableItems[_stationIndex] = null;
		}
		int num = 0;
		for (int i = 0; i < this.m_attachStations.Length; i++)
		{
			if (this.m_attachStations[i] != null && this.m_attachStations[i].HasItem())
			{
				num++;
			}
		}
		if (num == 0 && this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(true);
		}
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x00084090 File Offset: 0x00082490
	private bool InteractionIsSticky()
	{
		for (int i = 0; i < this.m_workableItems.Length; i++)
		{
			if (this.m_workableItems[i] != null && !this.m_workableItems[i].HasFinished())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000840DE File Offset: 0x000824DE
	private void OnInteracterAdded(GameObject _interacter, Vector2 _directionXZ)
	{
		if (this.m_interacters.Count == 0)
		{
			this.StartWorking();
			this.m_interactable.SetStickyInteractionCallback(new Generic<bool>(this.InteractionIsSticky));
		}
		this.m_interacters.Add(_interacter);
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x00084119 File Offset: 0x00082519
	public void OnInteracterRemoved(GameObject _interacter)
	{
		this.m_interacters.Remove(_interacter);
		if (this.m_interacters.Count == 0)
		{
			this.m_interactable.SetStickyInteractionCallback(null);
		}
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x00084144 File Offset: 0x00082544
	public void OnTrigger(string _trigger)
	{
		if (this.m_autoWorkstation.enabled && this.m_autoWorkstation.m_workTrigger == _trigger && this.m_workAction == null)
		{
			this.StartWorking();
		}
	}

	// Token: 0x040014D4 RID: 5332
	private AutoWorkstation m_autoWorkstation;

	// Token: 0x040014D5 RID: 5333
	private AutoWorkstationMessage m_data = new AutoWorkstationMessage();

	// Token: 0x040014D6 RID: 5334
	private ServerAutoWorkstation.WorkAction m_workAction;

	// Token: 0x040014D7 RID: 5335
	private ServerInteractable m_interactable;

	// Token: 0x040014D8 RID: 5336
	private ServerAttachStation[] m_attachStations = new ServerAttachStation[0];

	// Token: 0x040014D9 RID: 5337
	private ServerWorkableItem[] m_workableItems = new ServerWorkableItem[0];

	// Token: 0x040014DA RID: 5338
	private List<GameObject> m_interacters = new List<GameObject>();

	// Token: 0x02000574 RID: 1396
	private class WorkAction
	{
		// Token: 0x06001A5B RID: 6747 RVA: 0x00084190 File Offset: 0x00082590
		public WorkAction(float _delay, float _duration, CallbackVoid _callback)
		{
			this.m_callback = _callback;
			this.m_actionLength = _duration;
			this.m_actionTime = _delay;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000841B0 File Offset: 0x000825B0
		public void Update(float _dt)
		{
			float num = this.m_actionTimer + _dt;
			if (this.m_actionTimer < this.m_actionTime && num >= this.m_actionTime)
			{
				this.m_callback();
			}
			this.m_actionTimer = num;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000841F5 File Offset: 0x000825F5
		public void Reset()
		{
			this.m_actionTimer = 0f;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00084202 File Offset: 0x00082602
		public bool IsFinished()
		{
			return this.m_actionTimer >= this.m_actionLength;
		}

		// Token: 0x040014DB RID: 5339
		public CallbackVoid m_callback;

		// Token: 0x040014DC RID: 5340
		private float m_actionLength;

		// Token: 0x040014DD RID: 5341
		private float m_actionTime;

		// Token: 0x040014DE RID: 5342
		private float m_actionTimer;
	}
}
