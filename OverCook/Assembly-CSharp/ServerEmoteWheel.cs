using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A89 RID: 2697
public class ServerEmoteWheel : ServerSynchroniserBase
{
	// Token: 0x0600354F RID: 13647 RVA: 0x000F86DA File Offset: 0x000F6ADA
	protected virtual void Awake()
	{
		this.m_emoteWheel = base.gameObject.RequireComponent<EmoteWheel>();
		Mailbox.Server.RegisterForMessageType(MessageType.EmoteWheel, new OrderedMessageReceivedCallback(this.ProcessClientMessage));
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x000F8708 File Offset: 0x000F6B08
	protected void ProcessClientMessage(IOnlineMultiplayerSessionUserId _sender, Serialisable _serialisable)
	{
		EmoteWheelMessage emoteWheelMessage = _serialisable as EmoteWheelMessage;
		if (emoteWheelMessage.m_player == this.m_emoteWheel.m_player && emoteWheelMessage.m_player != PlayerInputLookup.Player.Count && emoteWheelMessage.m_forUI == this.m_emoteWheel.ForUI)
		{
			this.StartEmote(_sender, emoteWheelMessage);
		}
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x000F875D File Offset: 0x000F6B5D
	protected virtual void Update()
	{
		if (this.m_emoteWheel.ForUI && ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x000F878C File Offset: 0x000F6B8C
	protected void StartEmote(IOnlineMultiplayerSessionUserId _sender, EmoteWheelMessage _message)
	{
		EmoteWheelOption emoteWheelOption = this.m_emoteWheel.m_emoteWheelOptions.m_options[_message.m_emoteIdx];
		if ((emoteWheelOption.m_type == EmoteWheelOption.EmoteType.Animation || emoteWheelOption.m_type == EmoteWheelOption.EmoteType.Both) && emoteWheelOption.m_triggerForCode)
		{
			if (this.m_emoteWheel.m_codeTriggerTarget != null)
			{
				this.m_emoteWheel.m_codeTriggerTarget.SendTrigger(emoteWheelOption.m_animTrigger);
			}
			else
			{
				base.gameObject.SendTrigger(emoteWheelOption.m_animTrigger);
			}
		}
		this.m_message.InitialiseStartEmote(_message.m_emoteIdx, this.m_emoteWheel.m_player, _message.m_forUI);
		ServerMessenger.EmoteWheelMessage(this.m_message);
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x000F8844 File Offset: 0x000F6C44
	public override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Server.UnregisterForMessageType(MessageType.EmoteWheel, new OrderedMessageReceivedCallback(this.ProcessClientMessage));
	}

	// Token: 0x04002AC0 RID: 10944
	private EmoteWheel m_emoteWheel;

	// Token: 0x04002AC1 RID: 10945
	private EmoteWheelMessage m_message = new EmoteWheelMessage();
}
