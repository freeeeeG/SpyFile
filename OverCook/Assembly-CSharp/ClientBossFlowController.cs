using System;
using GameModes;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000643 RID: 1603
public class ClientBossFlowController : ClientDynamicFlowController
{
	// Token: 0x06001E82 RID: 7810 RVA: 0x00095986 File Offset: 0x00093D86
	protected override void Awake()
	{
		base.Awake();
		Mailbox.Client.RegisterForMessageType(MessageType.BossLevel, new OrderedMessageReceivedCallback(this.OnBossLevelMessage));
		this.m_gameSession = GameUtils.GetGameSession();
		this.m_gameModeKind = this.m_gameSession.GameModeKind;
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x000959C2 File Offset: 0x00093DC2
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.BossLevel, new OrderedMessageReceivedCallback(this.OnBossLevelMessage));
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x000959E4 File Offset: 0x00093DE4
	protected void OnBossLevelMessage(IOnlineMultiplayerSessionUserId _sessionId, Serialisable _serialisable)
	{
		BossLevelMessage bossLevelMessage = (BossLevelMessage)_serialisable;
		this.m_bHasSucceeded = (this.m_gameModeKind != Kind.Practice);
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x00095A0C File Offset: 0x00093E0C
	protected override ClientOrderControllerBase BuildOrderController(RecipeFlowGUI _recipeUI)
	{
		ClientBossOrderController clientBossOrderController = new ClientBossOrderController(_recipeUI);
		clientBossOrderController.SetRoundTimer(base.RoundTimer);
		return clientBossOrderController;
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x00095A2D File Offset: 0x00093E2D
	protected override void FinaliseRoundTimer()
	{
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x00095A30 File Offset: 0x00093E30
	protected override int GetStarRating(int _points)
	{
		if (DebugManager.Instance.GetOption("Skip Level 4 star"))
		{
			return 4;
		}
		if (DebugManager.Instance.GetOption("Skip Level 1 star") || DebugManager.Instance.GetOption("Skip Level 2 star") || DebugManager.Instance.GetOption("Skip Level 3 star"))
		{
			return 3;
		}
		if (DebugManager.Instance.GetOption("Skip Level"))
		{
			return 0;
		}
		bool flag = this.m_gameSession.Progress.SaveData.IsNGPEnabledForLevel(GameUtils.GetLevelID()) && this.m_gameSession.Progress.SaveData.NewGamePlusDialogShown;
		int num = (!flag) ? 3 : 4;
		return (!this.m_bHasSucceeded) ? 0 : num;
	}

	// Token: 0x04001772 RID: 6002
	private bool m_bHasSucceeded;

	// Token: 0x04001773 RID: 6003
	private Kind m_gameModeKind;

	// Token: 0x04001774 RID: 6004
	private GameSession m_gameSession;
}
