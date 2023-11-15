using System;
using GameModes;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
public class WorldMapOverlayUIController : UIControllerBase
{
	// Token: 0x060035AB RID: 13739 RVA: 0x000FAF04 File Offset: 0x000F9304
	private void Start()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_starCountUIRoot.SetActive(gameSession.GameModeKind == Kind.Campaign);
		this.m_gameModeUIText.SetLocalisedTextCatchAll(this.m_gameModeUIData.m_gameModes[(int)gameSession.GameModeKind].m_nameLocalisationKey);
		this.m_starCountUIText.text = gameSession.Progress.GetStarTotal().ToString().PadLeft(3, '0');
		GameSession gameSession2 = gameSession;
		gameSession2.OnGameModeSessionConfigChanged = (OnSessionConfigChanged)Delegate.Combine(gameSession2.OnGameModeSessionConfigChanged, new OnSessionConfigChanged(this.OnGameModeSessionConfigChanged));
		this.m_playerManager = GameUtils.RequestManagerInterface<IPlayerManager>();
		this.m_changeModeButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIChangeMode, PlayerInputLookup.Player.One);
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x000FAFB8 File Offset: 0x000F93B8
	private void OnDestroy()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		GameSession gameSession2 = gameSession;
		gameSession2.OnGameModeSessionConfigChanged = (OnSessionConfigChanged)Delegate.Remove(gameSession2.OnGameModeSessionConfigChanged, new OnSessionConfigChanged(this.OnGameModeSessionConfigChanged));
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x000FAFF0 File Offset: 0x000F93F0
	private void Update()
	{
		if ((ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession()) && !T17DialogBoxManager.HasAnyOpenDialogs() && !this.m_playerManager.IsWarningActive(PlayerWarning.Disengaged) && this.m_changeModeButton.JustPressed() && T17InGameFlow.Instance != null && T17InGameFlow.Instance.m_Rootmenu.GetCurrentOpenMenu() == null)
		{
			this.m_changeModeButton.ClaimPressEvent();
			this.m_changeModeButton.ClaimReleaseEvent();
			T17InGameFlow.Instance.m_Rootmenu.OpenInGameMenu(InGameRootMenu.IngameMenuTypeToOpen.GameMode, -1);
		}
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x000FB08E File Offset: 0x000F948E
	private void OnGameModeSessionConfigChanged(SessionConfig config)
	{
		this.m_starCountUIRoot.SetActive(config.m_kind == Kind.Campaign);
		this.m_gameModeUIText.SetLocalisedTextCatchAll(this.m_gameModeUIData.m_gameModes[(int)config.m_kind].m_nameLocalisationKey);
	}

	// Token: 0x04002B2E RID: 11054
	[SerializeField]
	[AssignResource("GameModeUIData", Editorbility.Editable)]
	private GameModeUIData m_gameModeUIData;

	// Token: 0x04002B2F RID: 11055
	[Header("Mode UI")]
	[SerializeField]
	private T17Text m_gameModeUIText;

	// Token: 0x04002B30 RID: 11056
	[Header("Star UI")]
	[SerializeField]
	private GameObject m_starCountUIRoot;

	// Token: 0x04002B31 RID: 11057
	[SerializeField]
	private T17Text m_starCountUIText;

	// Token: 0x04002B32 RID: 11058
	private IPlayerManager m_playerManager;

	// Token: 0x04002B33 RID: 11059
	private ILogicalButton m_changeModeButton;
}
