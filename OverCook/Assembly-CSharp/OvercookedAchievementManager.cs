using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000729 RID: 1833
public class OvercookedAchievementManager : SteamAchievementManager
{
	// Token: 0x060022D4 RID: 8916 RVA: 0x000A70BB File Offset: 0x000A54BB
	protected override void Awake()
	{
		base.Awake();
		this.SetupStatSystem();
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000A70CC File Offset: 0x000A54CC
	private void SetupStatSystem()
	{
		this.m_StatSystem.Clear();
		for (int i = 0; i < this.m_StatsTracking.Length; i++)
		{
			this.m_StatSystem.Setup(this.m_StatsTracking[i]);
		}
		this.m_StatSystem.SetupDone();
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000A711C File Offset: 0x000A551C
	public override void Init()
	{
		if (!this.m_initialised)
		{
			this.m_initialised = true;
			this.m_StatSystem.LoadStats();
			Mailbox.Client.RegisterForMessageType(MessageType.Achievement, new OrderedMessageReceivedCallback(this.OnAchievementRecieved));
			this.m_StatSystem.RegisterTrophyProgress(new VoidGeneric<int, float>(this.OnTrophyProgress));
			this.m_StatSystem.RegisterTrophyUnlock(new VoidGeneric<int>(this.OnTrophyUnlock));
			base.Init();
		}
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000A7194 File Offset: 0x000A5594
	public override void Unload()
	{
		base.Unload();
		this.m_initialised = false;
		Mailbox.Client.UnregisterForMessageType(MessageType.Achievement, new OrderedMessageReceivedCallback(this.OnAchievementRecieved));
		this.m_StatSystem.UnregisterTrophyProgress(new VoidGeneric<int, float>(this.OnTrophyProgress));
		this.m_StatSystem.UnregisterTrophyUnlock(new VoidGeneric<int>(this.OnTrophyUnlock));
		this.SetupStatSystem();
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000A71FA File Offset: 0x000A55FA
	protected override void OnDestroy()
	{
		this.Unload();
		base.OnDestroy();
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000A7208 File Offset: 0x000A5608
	private void OnTrophyProgress(int trophyId, float progress)
	{
		this.SetProgress(trophyId, progress);
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000A7212 File Offset: 0x000A5612
	private void OnTrophyUnlock(int trophyId)
	{
		this.Unlock(trophyId);
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000A721B File Offset: 0x000A561B
	public void IncStat(int ID, float value, ControlPadInput.PadNum pad)
	{
		this.m_StatSystem.IncStat(ID, value, pad);
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000A722B File Offset: 0x000A562B
	public void AddIDStat(int ID, int itemID, ControlPadInput.PadNum pad)
	{
		this.m_StatSystem.AddIDStat(ID, itemID, pad);
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x000A723C File Offset: 0x000A563C
	private void OnAchievementRecieved(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		AchievementMessage achievementMessage = (AchievementMessage)message;
		GameObject gameObject = EntitySerialisationRegistry.GetEntry(achievementMessage.m_Header.m_uEntityID).m_GameObject;
		if (gameObject != null)
		{
			PlayerIDProvider playerIDProvider = gameObject.RequestComponent<PlayerIDProvider>();
			if (playerIDProvider != null)
			{
				ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(playerIDProvider.GetID());
				this.IncStat(achievementMessage.m_statId, (float)achievementMessage.m_increment, padForPlayer);
			}
		}
	}

	// Token: 0x04001AB6 RID: 6838
	[SerializeField]
	private StatsTracking[] m_StatsTracking;

	// Token: 0x04001AB7 RID: 6839
	private bool m_initialised;
}
