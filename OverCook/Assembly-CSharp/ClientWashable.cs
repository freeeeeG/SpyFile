using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200062A RID: 1578
public class ClientWashable : ClientSynchroniserBase
{
	// Token: 0x06001DE8 RID: 7656 RVA: 0x00090FF5 File Offset: 0x0008F3F5
	public override EntityType GetEntityType()
	{
		return EntityType.Washable;
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x00090FFC File Offset: 0x0008F3FC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_washable = (Washable)synchronisedObject;
		this.m_duration = (float)this.m_washable.m_duration;
		this.m_durationMultiplier = this.m_washable.GetWashTimeMultiplier();
		if (this.m_washable.m_progressUIPrefab != null)
		{
			GameObject gameObject = GameUtils.InstantiateUIController(this.m_washable.m_progressUIPrefab.gameObject, "HoverIconCanvas");
			this.m_progressBar = gameObject.GetComponent<ProgressUIController>();
			this.m_progressBar.SetFollowTransform(base.transform, Vector3.zero);
		}
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x00091094 File Offset: 0x0008F494
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		WashableMessage washableMessage = (WashableMessage)serialisable;
		WashableMessage.MsgType msgType = washableMessage.m_msgType;
		if (msgType != WashableMessage.MsgType.Progress)
		{
			if (msgType == WashableMessage.MsgType.Rate)
			{
				this.m_rateOfChange = washableMessage.m_rate;
				this.m_progress = washableMessage.m_progress;
			}
		}
		else
		{
			this.m_progress = washableMessage.m_progress;
			this.m_duration = washableMessage.m_target;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000910FB File Offset: 0x0008F4FB
	public float ProgressPercent
	{
		get
		{
			return this.m_progress / this.m_duration;
		}
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x0009110A File Offset: 0x0008F50A
	private void Awake()
	{
		Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x00091124 File Offset: 0x0008F524
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (this.m_progressBar != null && this.m_progressBar.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.m_progressBar.gameObject);
		}
		this.m_progressBar = null;
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x00091190 File Offset: 0x0008F590
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_durationMultiplier = this.m_washable.GetWashTimeMultiplier();
		}
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000911C4 File Offset: 0x0008F5C4
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_rateOfChange != 0f)
		{
			float num = this.m_duration * (float)this.m_durationMultiplier;
			this.m_progress += this.m_rateOfChange * TimeManager.GetDeltaTime(base.gameObject.layer);
			this.m_progress = Mathf.Clamp(this.m_progress, 0f, num);
			if (this.m_progress >= 0f && this.m_progress < num)
			{
				this.UpdateUIProgressBar();
			}
		}
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x00091254 File Offset: 0x0008F654
	private void UpdateUIProgressBar()
	{
		if (this.m_progressBar != null)
		{
			float progress = Mathf.Clamp01(this.m_progress / (this.m_duration * (float)this.m_durationMultiplier));
			this.m_progressBar.SetProgress(progress);
		}
	}

	// Token: 0x04001710 RID: 5904
	private Washable m_washable;

	// Token: 0x04001711 RID: 5905
	private float m_progress;

	// Token: 0x04001712 RID: 5906
	private float m_rateOfChange;

	// Token: 0x04001713 RID: 5907
	private float m_duration;

	// Token: 0x04001714 RID: 5908
	private int m_durationMultiplier = 1;

	// Token: 0x04001715 RID: 5909
	private ProgressUIController m_progressBar;
}
