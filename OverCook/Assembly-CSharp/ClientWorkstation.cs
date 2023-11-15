using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class ClientWorkstation : ClientSynchroniserBase
{
	// Token: 0x06001E44 RID: 7748 RVA: 0x0009275C File Offset: 0x00090B5C
	public override EntityType GetEntityType()
	{
		return EntityType.Workstation;
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x00092760 File Offset: 0x00090B60
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_workstation = (Workstation)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<ClientInteractable>();
		this.m_interactable.SetInteractionSuppressed(true);
		this.m_attachStation = base.gameObject.GetComponent<ClientAttachStation>();
		this.m_attachStation.RegisterAllowItemPickup(new Generic<bool>(this.CanPickupItem));
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		if (this.m_workstation.m_chopPFX != null)
		{
			AttachStation component = base.gameObject.GetComponent<AttachStation>();
			GameObject gameObject = this.m_workstation.m_chopPFX.InstantiateOnParent(base.transform, false);
			gameObject.transform.localPosition = component.GetAttachPoint(gameObject).localPosition;
			this.m_chopPFXInstance = gameObject.RequireComponent<ParticleSystem>();
			this.m_chopPFXInstance.Stop();
		}
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x00092854 File Offset: 0x00090C54
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		WorkstationMessage workstationMessage = (WorkstationMessage)serialisable;
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(workstationMessage.m_interactorHeader.m_uEntityID);
		GameObject interacter = (entry == null) ? null : entry.m_GameObject;
		if (workstationMessage.m_interacting)
		{
			GameObject gameObject = EntitySerialisationRegistry.GetEntry(workstationMessage.m_itemHeader.m_uEntityID).m_GameObject;
			this.StartWorking(interacter, gameObject.RequireComponent<ClientWorkableItem>());
		}
		else
		{
			this.StopWorking(interacter);
		}
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x000928C8 File Offset: 0x00090CC8
	protected override void OnDestroy()
	{
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterAllowItemPickup(new Generic<bool>(this.CanPickupItem));
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		}
		base.OnDestroy();
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x00092934 File Offset: 0x00090D34
	private void StartWorking(GameObject _interacter, ClientWorkableItem _item)
	{
		this.m_item = _item;
		TriggerCallback callbackComponent = _interacter.RequestComponent<TriggerCallback>();
		if (callbackComponent != null)
		{
			AnimationEventData animationEventData = _interacter.RequireComponentRecursive<AnimationEventData>();
			float duration;
			float delay;
			animationEventData.GetTriggerData("Chop", "Impact", out duration, out delay);
			if (this.m_chopPFXInstance != null)
			{
				this.m_chopPFXInstance.transform.localPosition = this.m_attachStation.GetAttachPoint(_item.gameObject).localPosition;
				this.m_chopPFXInstance.Play();
			}
			ClientWorkstation.Interacter interacter = new ClientWorkstation.Interacter(callbackComponent, delay, duration, delegate()
			{
				this.OnChop(callbackComponent.transform);
			});
			callbackComponent.RegisterCallback(this.m_workstation.m_chopTrigger, interacter.Method);
			this.m_interacters.Add(interacter);
			this.m_interactable.SetStickyInteractionCallback(new Generic<bool>(this.InteractionIsSticky));
		}
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x00092A2C File Offset: 0x00090E2C
	public bool InteractionIsSticky()
	{
		return this.m_interacters.Count == 0 || this.m_item != null;
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x00092A4D File Offset: 0x00090E4D
	public bool IsBeingUsed()
	{
		return this.m_interacters.Count > 0;
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x00092A60 File Offset: 0x00090E60
	private void OnChop(Transform _interacter)
	{
		if (this.m_item)
		{
			this.m_item.DoWork(this.m_attachStation, _interacter.gameObject);
			PlayerIDProvider playerIDProvider = _interacter.gameObject.RequestComponent<PlayerIDProvider>();
			if (playerIDProvider != null)
			{
				GameUtils.TriggerNXRumble(playerIDProvider.GetID(), GameOneShotAudioTag.Chop);
			}
		}
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x00092AB8 File Offset: 0x00090EB8
	public override void UpdateSynchronising()
	{
		if (this.m_interacters.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.m_interacters.Count; i++)
		{
			ClientWorkstation.Interacter interacter = this.m_interacters[i];
			float deltaTime = TimeManager.GetDeltaTime(interacter.Component.gameObject);
			interacter.Update(deltaTime);
		}
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x00092B18 File Offset: 0x00090F18
	public void StopWorking(GameObject _interacter)
	{
		TriggerCallback callbackComponent = (!(_interacter != null)) ? null : _interacter.RequestComponent<TriggerCallback>();
		ClientWorkstation.Interacter interacter = this.m_interacters.Find((ClientWorkstation.Interacter x) => x.Component == callbackComponent);
		interacter.Component.UnregisterCallback(this.m_workstation.m_chopTrigger, interacter.Method);
		this.m_interacters.Remove(interacter);
		if (this.m_chopPFXInstance != null)
		{
			this.m_chopPFXInstance.Stop();
		}
		if (this.m_interacters.Count == 0)
		{
			this.m_item = null;
			this.m_interactable.SetStickyInteractionCallback(null);
		}
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x00092BCC File Offset: 0x00090FCC
	private bool CanPickupItem()
	{
		return !base.enabled || !this.m_item || this.m_item.HasFinished() || this.m_item.GetProgress() == 0f;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x00092C20 File Offset: 0x00091020
	private void OnItemAdded(IClientAttachment _iHoldable)
	{
		ClientWorkableItem exists = _iHoldable.AccessGameObject().RequestComponent<ClientWorkableItem>();
		if (exists)
		{
			this.m_interactable.SetInteractionSuppressed(false);
		}
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x00092C50 File Offset: 0x00091050
	private void OnItemRemoved(IClientAttachment _iHoldable)
	{
		ClientWorkableItem exists = _iHoldable.AccessGameObject().RequestComponent<ClientWorkableItem>();
		if (exists)
		{
			this.m_interactable.SetInteractionSuppressed(true);
		}
	}

	// Token: 0x04001751 RID: 5969
	private Workstation m_workstation;

	// Token: 0x04001752 RID: 5970
	private ClientInteractable m_interactable;

	// Token: 0x04001753 RID: 5971
	private ClientAttachStation m_attachStation;

	// Token: 0x04001754 RID: 5972
	private ParticleSystem m_chopPFXInstance;

	// Token: 0x04001755 RID: 5973
	private List<ClientWorkstation.Interacter> m_interacters = new List<ClientWorkstation.Interacter>();

	// Token: 0x04001756 RID: 5974
	private ClientWorkableItem m_item;

	// Token: 0x02000637 RID: 1591
	private class Interacter
	{
		// Token: 0x06001E51 RID: 7761 RVA: 0x00092C80 File Offset: 0x00091080
		public Interacter(TriggerCallback _component, float _delay, float _duration, TriggerCallback.Callback _method)
		{
			this.Component = _component;
			this.Method = _method;
			this.m_actionLength = _duration;
			this.m_actionTime = _delay;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00092CA8 File Offset: 0x000910A8
		public void Update(float _dt)
		{
			float num = this.m_actionTimer + _dt;
			if (this.m_actionTimer < this.m_actionTime && num >= this.m_actionTime)
			{
				this.Method();
			}
			if (num >= this.m_actionLength)
			{
				num -= this.m_actionLength;
			}
			this.m_actionTimer = num;
		}

		// Token: 0x04001757 RID: 5975
		public TriggerCallback Component;

		// Token: 0x04001758 RID: 5976
		public TriggerCallback.Callback Method;

		// Token: 0x04001759 RID: 5977
		private float m_actionLength;

		// Token: 0x0400175A RID: 5978
		private float m_actionTime;

		// Token: 0x0400175B RID: 5979
		private float m_actionTimer;
	}
}
