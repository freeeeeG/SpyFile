using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class ServerCannonSessionInteractable : ServerSessionInteractable
{
	// Token: 0x06001469 RID: 5225 RVA: 0x0006F9A4 File Offset: 0x0006DDA4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannon = synchronisedObject.gameObject.RequireComponent<ServerCannon>();
		this.m_placementInteractable = base.gameObject.RequireComponent<ServerPlacementInteractable>();
		this.m_placementInteractable.RegisterCanInteractCallback(new Generic<bool, GameObject>(this.CanInteract));
		this.m_placementInteractable.RegisterTriggerCallback(new VoidGeneric<GameObject, Vector2>(this.StartSession));
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x0006FA0A File Offset: 0x0006DE0A
	protected override bool CanInteract(GameObject _interacter)
	{
		return !this.m_cannon.IsFlying();
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x0006FA1A File Offset: 0x0006DE1A
	protected override void StartSession(GameObject _interacter, Vector2 _directionXZ)
	{
		this.m_placementInteractable.SetInteractionSurpressed(true);
		base.StartSession(_interacter, _directionXZ);
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x0006FA30 File Offset: 0x0006DE30
	protected override ServerSessionInteractable.SessionBase BuildSession(GameObject _interacter)
	{
		this.m_session = new ServerCannonSessionInteractable.UserSession(this, _interacter, this.m_cannon);
		return this.m_session;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x0006FA4B File Offset: 0x0006DE4B
	public void Exit()
	{
		this.m_session.OnSessionEnded();
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x0006FA58 File Offset: 0x0006DE58
	protected override void OnSessionEnded()
	{
		this.m_placementInteractable.SetInteractionSurpressed(false);
		base.OnSessionEnded();
	}

	// Token: 0x04000FCA RID: 4042
	private ServerCannonSessionInteractable.UserSession m_session;

	// Token: 0x04000FCB RID: 4043
	private ServerCannon m_cannon;

	// Token: 0x04000FCC RID: 4044
	private ServerPlacementInteractable m_placementInteractable;

	// Token: 0x02000453 RID: 1107
	private class UserSession : ServerSessionInteractable.SessionBase
	{
		// Token: 0x0600146F RID: 5231 RVA: 0x0006FA6C File Offset: 0x0006DE6C
		public UserSession(ServerCannonSessionInteractable _self, GameObject _avatar, ServerCannon _cannon) : base(_self, _avatar)
		{
			this.m_avatar = _avatar;
			this.m_cannon = _cannon;
			this.m_cannon.Load(_avatar, new GenericVoid<bool>(this.InteractionEnded));
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0006FA9C File Offset: 0x0006DE9C
		public override void Update()
		{
			if (base.PlayerControls != null)
			{
				base.PlayerControls.ControlScheme.IsUseJustReleased();
			}
			if ((base.PlayerControls == null || base.PlayerControls.ControlScheme.m_dashButton.JustPressed()) && !this.m_cannon.IsFlying())
			{
				this.m_cannon.Unload(this.m_avatar);
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0006FB18 File Offset: 0x0006DF18
		public override void OnDestroyChefMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			DestroyChefMessage destroyChefMessage = (DestroyChefMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(destroyChefMessage.m_Chef.m_Header.m_uEntityID);
			if (entry != null && entry.m_GameObject != null)
			{
				PlayerControls y = entry.m_GameObject.RequireComponent<PlayerControls>();
				if (base.PlayerControls == y)
				{
					this.m_cannon.Unload(entry.m_GameObject);
				}
			}
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0006FB87 File Offset: 0x0006DF87
		public void InteractionEnded(bool enableComponents)
		{
			if (enableComponents)
			{
				this.OnSessionEnded();
			}
			else
			{
				this.m_running = false;
				Mailbox.Client.UnregisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.OnDestroyChefMessageReceived));
			}
		}

		// Token: 0x04000FCD RID: 4045
		private ServerCannon m_cannon;

		// Token: 0x04000FCE RID: 4046
		private GameObject m_avatar;
	}
}
