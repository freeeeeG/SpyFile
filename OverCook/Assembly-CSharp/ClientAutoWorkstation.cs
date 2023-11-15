using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class ClientAutoWorkstation : ClientSynchroniserBase
{
	// Token: 0x06001A60 RID: 6752 RVA: 0x0008428D File Offset: 0x0008268D
	public override EntityType GetEntityType()
	{
		return EntityType.AutoWorkstation;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x00084294 File Offset: 0x00082694
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_autoWorkstation = (AutoWorkstation)synchronisedObject;
		this.m_interactable = base.gameObject.RequestComponent<ClientInteractable>();
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(true);
		}
		this.m_attachStations = base.gameObject.RequestComponentsInImmediateChildren<ClientAttachStation>();
		this.m_workableItems = new ClientWorkableItem[this.m_attachStations.Length];
		for (int i = 0; i < this.m_attachStations.Length; i++)
		{
			int index = i;
			this.m_attachStations[i].RegisterOnItemAdded(delegate(IClientAttachment x)
			{
				this.OnItemAdded(x, index);
			});
			this.m_attachStations[i].RegisterOnItemRemoved(delegate(IClientAttachment x)
			{
				this.OnItemRemoved(x, index);
			});
			this.m_attachStations[i].RegisterAllowItemPickup(() => this.CanPickupItem(index));
			this.m_attachStations[i].RegisterAllowItemPlacement((GameObject x, PlacementContext y) => this.CanPlaceItem(x, y, index));
		}
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x00084390 File Offset: 0x00082790
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		AutoWorkstationMessage autoWorkstationMessage = (AutoWorkstationMessage)serialisable;
		if (autoWorkstationMessage.m_working)
		{
			Array.Resize<ClientWorkableItem>(ref this.m_workableItems, autoWorkstationMessage.m_items.Length);
			for (int i = 0; i < autoWorkstationMessage.m_items.Length; i++)
			{
				if (autoWorkstationMessage.m_items[i] != null && autoWorkstationMessage.m_items[i].m_GameObject != null)
				{
					this.m_workableItems[i] = autoWorkstationMessage.m_items[i].m_GameObject.RequireComponent<ClientWorkableItem>();
				}
			}
			this.StartWorking();
		}
		else
		{
			this.StopWorking();
		}
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x0008442C File Offset: 0x0008282C
	protected override void OnDestroy()
	{
		for (int i = 0; i < this.m_attachStations.Length; i++)
		{
			ClientAttachStation clientAttachStation = this.m_attachStations[i];
		}
		base.OnDestroy();
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x00084464 File Offset: 0x00082864
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
			if (this.m_interactable != null && this.m_interactable.InteractorCount() != 0)
			{
				this.m_workAction.Reset();
			}
			else
			{
				this.StopWorking();
			}
		}
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000844DC File Offset: 0x000828DC
	private bool CanPickupItem(int _stationIndex)
	{
		if (base.enabled && this.m_workAction == null)
		{
			ClientAttachStation clientAttachStation = this.m_attachStations[_stationIndex];
			if (clientAttachStation != null && clientAttachStation.HasItem())
			{
				ClientWorkableItem clientWorkableItem = this.m_workableItems[_stationIndex];
				if (clientWorkableItem != null)
				{
					return clientWorkableItem.HasFinished() || clientWorkableItem.GetProgress() == 0f;
				}
			}
		}
		return true;
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x00084551 File Offset: 0x00082951
	private bool CanPlaceItem(GameObject _item, PlacementContext _context, int _stationIndex)
	{
		return _context.m_source != PlacementContext.Source.Player || !this.m_autoWorkstation.enabled || this.m_workAction == null;
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x0008457E File Offset: 0x0008297E
	private void OnItemAdded(IClientAttachment _iHoldable, int _stationIndex)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(false);
		}
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000845A0 File Offset: 0x000829A0
	private void OnItemRemoved(IClientAttachment _iHoldable, int _stationIndex)
	{
		ClientWorkableItem x = _iHoldable.AccessGameObject().RequestComponent<ClientWorkableItem>();
		if (x != null)
		{
			this.m_workableItems[_stationIndex] = null;
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
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x00084638 File Offset: 0x00082A38
	private bool InteractionIsSticky()
	{
		for (int i = 0; i < this.m_workableItems.Length; i++)
		{
			if (this.m_workableItems[i] != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x00084674 File Offset: 0x00082A74
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
		TriggerCallback triggerCallback = base.gameObject.RequestComponent<TriggerCallback>();
		if (triggerCallback != null)
		{
			AnimationEventData animationEventData = base.gameObject.RequireComponentRecursive<AnimationEventData>();
			if (flag)
			{
				float duration;
				float delay;
				animationEventData.GetTriggerData("Chop", "Impact", out duration, out delay);
				this.m_workAction = new ClientAutoWorkstation.WorkAction(triggerCallback, delay, duration, new TriggerCallback.Callback(this.OnChop));
			}
			else
			{
				float duration;
				float delay;
				animationEventData.GetTriggerData("Jam", "Impact", out duration, out delay);
				this.m_workAction = new ClientAutoWorkstation.WorkAction(triggerCallback, delay, duration, new TriggerCallback.Callback(this.OnJam));
			}
			triggerCallback.RegisterCallback("Impact", this.m_workAction.Method);
		}
		Animator animator = base.gameObject.RequestComponentInImmediateChildren<Animator>();
		if (animator != null)
		{
			animator.SetBool("IsChopping", true);
			animator.SetBool("IsJammed", !flag);
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000847BC File Offset: 0x00082BBC
	private void StopWorking()
	{
		if (this.m_workAction != null)
		{
			TriggerCallback triggerCallback = base.gameObject.RequestComponent<TriggerCallback>();
			if (triggerCallback != null)
			{
				triggerCallback.UnregisterCallback("Impact", this.m_workAction.Method);
			}
			this.m_workAction = null;
		}
		for (int i = 0; i < this.m_workableItems.Length; i++)
		{
			this.m_workableItems[i] = null;
		}
		Animator animator = base.gameObject.RequestComponentInImmediateChildren<Animator>();
		if (animator != null)
		{
			animator.SetBool("IsChopping", false);
			animator.SetBool("IsJammed", false);
		}
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x0008485C File Offset: 0x00082C5C
	private void OnChop()
	{
		OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
		int num = this.m_attachStations.Length;
		for (int i = 0; i < num; i++)
		{
			ClientAttachStation station = this.m_attachStations[i];
			ClientWorkableItem clientWorkableItem = this.m_workableItems[i];
			if (clientWorkableItem != null)
			{
				clientWorkableItem.DoWork(station, base.gameObject, this.m_autoWorkstation.m_choppingPower);
			}
			if (overcookedAchievementManager != null)
			{
				overcookedAchievementManager.IncStat(702, 1f, ControlPadInput.PadNum.One);
			}
		}
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000848E0 File Offset: 0x00082CE0
	private void OnJam()
	{
	}

	// Token: 0x040014DF RID: 5343
	private AutoWorkstation m_autoWorkstation;

	// Token: 0x040014E0 RID: 5344
	private ClientAutoWorkstation.WorkAction m_workAction;

	// Token: 0x040014E1 RID: 5345
	private ClientInteractable m_interactable;

	// Token: 0x040014E2 RID: 5346
	private ClientAttachStation[] m_attachStations = new ClientAttachStation[0];

	// Token: 0x040014E3 RID: 5347
	private ClientWorkableItem[] m_workableItems = new ClientWorkableItem[0];

	// Token: 0x02000576 RID: 1398
	private class WorkAction
	{
		// Token: 0x06001A6E RID: 6766 RVA: 0x000848E2 File Offset: 0x00082CE2
		public WorkAction(TriggerCallback _component, float _delay, float _duration, TriggerCallback.Callback _method)
		{
			this.Component = _component;
			this.Method = _method;
			this.m_actionLength = _duration;
			this.m_actionTime = _delay;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00084908 File Offset: 0x00082D08
		public void Update(float _dt)
		{
			float num = this.m_actionTimer + _dt;
			if (this.m_actionTimer < this.m_actionTime && num >= this.m_actionTime)
			{
				this.Method();
			}
			this.m_actionTimer = num;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0008494D File Offset: 0x00082D4D
		public void Reset()
		{
			this.m_actionTimer = 0f;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0008495A File Offset: 0x00082D5A
		public bool IsFinished()
		{
			return this.m_actionTimer >= this.m_actionLength;
		}

		// Token: 0x040014E4 RID: 5348
		public TriggerCallback Component;

		// Token: 0x040014E5 RID: 5349
		public TriggerCallback.Callback Method;

		// Token: 0x040014E6 RID: 5350
		private float m_actionLength;

		// Token: 0x040014E7 RID: 5351
		private float m_actionTime;

		// Token: 0x040014E8 RID: 5352
		private float m_actionTimer;
	}
}
