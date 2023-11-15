using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class ServerCannon : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06001433 RID: 5171 RVA: 0x0006E500 File Offset: 0x0006C900
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannon = (Cannon)synchronisedObject;
		Cannon cannon = this.m_cannon;
		cannon.EndCannonRoutine = (GenericVoid<GameObject>)Delegate.Combine(cannon.EndCannonRoutine, new GenericVoid<GameObject>(this.EndCannonRoutine));
		this.m_handlers = this.m_cannon.GetComponents<IServerCannonHandler>();
		this.m_pilotRotation = base.gameObject.RequestComponentRecursive<PilotRotation>();
		this.m_buttonInteractable = this.m_cannon.m_button.RequireComponent<ServerInteractable>();
		this.m_buttonInteractable.RegisterTriggerCallbacks(new ServerInteractable.BeginInteractCallback(this.ButtonInteractCallback));
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x0006E598 File Offset: 0x0006C998
	public override void UpdateSynchronising()
	{
		if (this.m_loadedObject != null && !this.m_readyToLaunch && this.m_readyToLaunchCallback != null)
		{
			this.m_readyToLaunch = this.m_readyToLaunchCallback();
			if (this.m_readyToLaunch && this.m_cannon.m_button != null && !string.IsNullOrEmpty(this.m_cannon.m_enableTrigger))
			{
				this.m_cannon.m_button.SendTrigger(this.m_cannon.m_enableTrigger);
			}
		}
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x0006E62E File Offset: 0x0006CA2E
	public override EntityType GetEntityType()
	{
		return EntityType.Cannon;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x0006E632 File Offset: 0x0006CA32
	public void ButtonInteractCallback(GameObject _interacter, Vector2 _directionXZ)
	{
		if (this.m_loadedObject != null && _interacter.RequestComponents<PlayerIDProvider>() != null && !this.m_flying)
		{
			ServerMessenger.Achievement(_interacter, 802, 1);
		}
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x0006E668 File Offset: 0x0006CA68
	public void Load(GameObject _obj, GenericVoid<bool> EndInteractionCallback)
	{
		this.m_readyToLaunch = false;
		this.m_loadedObject = _obj;
		this.OnInteractionEnd = EndInteractionCallback;
		IServerCannonHandler handler = this.GetHandler(_obj);
		if (handler != null)
		{
			handler.Load(_obj);
		}
		this.m_message.m_loadedObject = _obj;
		this.m_message.m_state = CannonMessage.CannonState.Load;
		this.SendServerEvent(this.m_message);
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x0006E6C4 File Offset: 0x0006CAC4
	public void Unload(GameObject _obj)
	{
		IServerCannonHandler handler = this.GetHandler(_obj);
		if (handler != null)
		{
			handler.Unload(_obj);
		}
		_obj.transform.SetPositionAndRotation(this.m_cannon.m_exitPoint.position, this.m_cannon.m_exitPoint.rotation);
		this.EndInteraction(true);
		this.m_message.m_state = CannonMessage.CannonState.Unload;
		this.m_message.m_loadedObject = _obj;
		this.SendServerEvent(this.m_message);
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x0006E73C File Offset: 0x0006CB3C
	public IServerCannonHandler GetHandler(GameObject _obj)
	{
		for (int i = 0; i < this.m_handlers.Length; i++)
		{
			if (this.m_handlers[i].CanHandle(_obj))
			{
				return this.m_handlers[i];
			}
		}
		return null;
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0006E780 File Offset: 0x0006CB80
	public void EndCannonRoutine(GameObject _obj)
	{
		this.m_flying = false;
		IServerCannonHandler handler = this.GetHandler(_obj);
		if (handler != null)
		{
			handler.ExitCannonRoutine(_obj);
		}
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x0006E7AC File Offset: 0x0006CBAC
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_cannon.m_launchTrigger && !this.m_flying)
		{
			this.EndInteraction(false);
			this.m_message.m_state = CannonMessage.CannonState.Launched;
			this.m_flying = true;
			this.m_message.m_loadedObject = this.m_loadedObject;
			if (this.m_pilotRotation != null)
			{
				this.m_message.m_angle = this.m_pilotRotation.m_transformToRotate.eulerAngles.y;
			}
			this.SendServerEvent(this.m_message);
		}
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x0006E845 File Offset: 0x0006CC45
	public bool IsFlying()
	{
		return this.m_flying;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x0006E850 File Offset: 0x0006CC50
	private void EndInteraction(bool enableComponents)
	{
		if (this.m_cannon.m_button != null && !string.IsNullOrEmpty(this.m_cannon.m_disableTrigger))
		{
			this.m_cannon.m_button.SendTrigger(this.m_cannon.m_disableTrigger);
		}
		if (this.OnInteractionEnd != null)
		{
			this.OnInteractionEnd(enableComponents);
			this.OnInteractionEnd = null;
		}
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x0006E8C1 File Offset: 0x0006CCC1
	public void SetReadyToLaunchCallback(Generic<bool> _callback)
	{
		this.m_readyToLaunchCallback = _callback;
	}

	// Token: 0x04000FAF RID: 4015
	private Cannon m_cannon;

	// Token: 0x04000FB0 RID: 4016
	private CannonMessage m_message = new CannonMessage();

	// Token: 0x04000FB1 RID: 4017
	private bool m_flying;

	// Token: 0x04000FB2 RID: 4018
	private IServerCannonHandler[] m_handlers;

	// Token: 0x04000FB3 RID: 4019
	private GenericVoid<bool> OnInteractionEnd;

	// Token: 0x04000FB4 RID: 4020
	private GameObject m_loadedObject;

	// Token: 0x04000FB5 RID: 4021
	private PilotRotation m_pilotRotation;

	// Token: 0x04000FB6 RID: 4022
	private Generic<bool> m_readyToLaunchCallback;

	// Token: 0x04000FB7 RID: 4023
	private bool m_readyToLaunch;

	// Token: 0x04000FB8 RID: 4024
	private ServerInteractable m_buttonInteractable;
}
