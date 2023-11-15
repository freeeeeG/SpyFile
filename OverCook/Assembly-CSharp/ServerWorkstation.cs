using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class ServerWorkstation : ServerSynchroniserBase
{
	// Token: 0x06001E33 RID: 7731 RVA: 0x000921D7 File Offset: 0x000905D7
	public override EntityType GetEntityType()
	{
		return EntityType.Workstation;
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x000921DC File Offset: 0x000905DC
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_workable = (Workstation)synchronisedObject;
		this.m_attachStation = base.gameObject.GetComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemovedItem));
		this.m_attachStation.RegisterAllowItemPickup(new Generic<bool>(this.CanPickupItem));
		this.m_interactable = base.gameObject.GetComponent<ServerInteractable>();
		this.m_interactable.RegisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnInteracterAdded), new ServerInteractable.EndInteractCallback(this.OnInteracterRemoved));
		this.m_interactable.SetInteractionSuppressed(true);
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x0009228C File Offset: 0x0009068C
	private void SynchroniseInteractionState(ServerWorkstation.Interacter _interactor, bool _active)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_interactor.m_object.gameObject);
		if (entry != null)
		{
			this.m_data.m_interacting = _active;
			this.m_data.m_interactor = entry;
			if (_active)
			{
				EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(this.m_item.gameObject);
				this.m_data.m_item = entry2;
			}
			else
			{
				this.m_data.m_item = null;
			}
			this.SendServerEvent(this.m_data);
		}
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x0009230D File Offset: 0x0009070D
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_item)
		{
			this.m_item.enabled = true;
		}
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x00092331 File Offset: 0x00090731
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_item)
		{
			this.m_item.enabled = false;
		}
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x00092358 File Offset: 0x00090758
	public override void OnDestroy()
	{
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterAllowItemPickup(new Generic<bool>(this.CanPickupItem));
			this.m_attachStation.UnregisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemovedItem));
		}
		if (this.m_interactable != null)
		{
			this.m_interactable.UnregisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnInteracterAdded), new ServerInteractable.EndInteractCallback(this.OnInteracterRemoved));
		}
		base.OnDestroy();
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x000923F8 File Offset: 0x000907F8
	private bool CanPickupItem()
	{
		return !base.enabled || !this.m_item || this.m_item.HasFinished() || this.m_item.GetProgress() == 0f;
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x00092449 File Offset: 0x00090849
	private bool InteractionIsSticky()
	{
		return this.m_interacters.Count == 0 || (this.m_item != null && !this.m_item.HasFinished());
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x00092480 File Offset: 0x00090880
	public override void UpdateSynchronising()
	{
		if (this.m_interacters.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.m_interacters.Count; i++)
		{
			ServerWorkstation.Interacter interacter = this.m_interacters[i];
			float deltaTime = TimeManager.GetDeltaTime(interacter.m_object);
			interacter.Update(deltaTime);
		}
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000924DC File Offset: 0x000908DC
	private void OnItemAdded(IAttachment _iHoldable)
	{
		ServerWorkableItem serverWorkableItem = _iHoldable.AccessGameObject().RequestComponent<ServerWorkableItem>();
		if (serverWorkableItem)
		{
			this.m_item = serverWorkableItem;
			this.m_interactable.SetInteractionSuppressed(false);
		}
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x00092514 File Offset: 0x00090914
	private void OnItemRemovedItem(IAttachment _iHoldable)
	{
		ServerWorkableItem exists = _iHoldable.AccessGameObject().RequestComponent<ServerWorkableItem>();
		if (exists)
		{
			this.m_interactable.SetInteractionSuppressed(true);
			this.m_item = null;
		}
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x0009254C File Offset: 0x0009094C
	private void OnInteracterAdded(GameObject _interacter, Vector2 _directionXZ)
	{
		AnimationEventData animationEventData = _interacter.RequireComponentRecursive<AnimationEventData>();
		float duration;
		float delay;
		animationEventData.GetTriggerData("Chop", "Impact", out duration, out delay);
		ServerWorkstation.Interacter interacter = new ServerWorkstation.Interacter(_interacter, delay, duration, delegate()
		{
			this.OnChop(_interacter.transform);
		});
		this.m_interacters.Add(interacter);
		this.m_interactable.SetStickyInteractionCallback(new Generic<bool>(this.InteractionIsSticky));
		this.SynchroniseInteractionState(interacter, true);
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000925D8 File Offset: 0x000909D8
	private void OnChop(Transform _interacter)
	{
		if (this.m_item)
		{
			Vector2 normalized = _interacter.forward.XZ().normalized;
			this.m_attachStation.RotateForDirection(normalized);
			this.m_item.DoWork(this.m_attachStation, _interacter.gameObject);
		}
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x0009262C File Offset: 0x00090A2C
	public void OnInteracterRemoved(GameObject _interacter)
	{
		ServerWorkstation.Interacter interacter = this.m_interacters.Find((ServerWorkstation.Interacter x) => x.m_object == _interacter);
		this.m_interacters.Remove(interacter);
		if (this.m_interacters.Count == 0)
		{
			this.m_interactable.SetStickyInteractionCallback(null);
		}
		this.SynchroniseInteractionState(interacter, false);
	}

	// Token: 0x04001746 RID: 5958
	private Workstation m_workable;

	// Token: 0x04001747 RID: 5959
	private WorkstationMessage m_data = new WorkstationMessage();

	// Token: 0x04001748 RID: 5960
	private List<ServerWorkstation.Interacter> m_interacters = new List<ServerWorkstation.Interacter>();

	// Token: 0x04001749 RID: 5961
	private ServerWorkableItem m_item;

	// Token: 0x0400174A RID: 5962
	private ServerAttachStation m_attachStation;

	// Token: 0x0400174B RID: 5963
	private ServerInteractable m_interactable;

	// Token: 0x02000635 RID: 1589
	private class Interacter
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x0009268F File Offset: 0x00090A8F
		public Interacter(GameObject _object, float _delay, float _duration, CallbackVoid _callback)
		{
			this.m_object = _object;
			this.m_callback = _callback;
			this.m_actionLength = _duration;
			this.m_actionTime = _delay;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000926B4 File Offset: 0x00090AB4
		public void Update(float _dt)
		{
			float num = this.m_actionTimer + _dt;
			if (this.m_actionTimer < this.m_actionTime && num >= this.m_actionTime)
			{
				this.m_callback();
			}
			if (num >= this.m_actionLength)
			{
				num -= this.m_actionLength;
			}
			this.m_actionTimer = num;
		}

		// Token: 0x0400174C RID: 5964
		public GameObject m_object;

		// Token: 0x0400174D RID: 5965
		public CallbackVoid m_callback;

		// Token: 0x0400174E RID: 5966
		private float m_actionLength;

		// Token: 0x0400174F RID: 5967
		private float m_actionTime;

		// Token: 0x04001750 RID: 5968
		private float m_actionTimer;
	}
}
