using System;
using System.Collections;
using Team17.Online;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007BE RID: 1982
	public class HordeRatingUIController : UIControllerBase
	{
		// Token: 0x060025F7 RID: 9719 RVA: 0x000B41B0 File Offset: 0x000B25B0
		private void Awake()
		{
			this.m_animator = base.gameObject.RequireComponent<Animator>();
			this.m_focusPlayersButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIResultsToggleProfile, PlayerInputLookup.Player.One);
			this.m_backButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
			this.m_uiPlayers.AllowSettingFocus = false;
			this.m_waitingForPlayers.SetActive(false);
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000B4204 File Offset: 0x000B2604
		private void Start()
		{
			this.UpdateLegend();
			if (T17EventSystemsManager.Instance != null)
			{
				T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
				if (eventSystemForEngagementSlot != null)
				{
					eventSystemForEngagementSlot.SetSelectedGameObject(null);
					eventSystemForEngagementSlot.SetSelectedGameObject(null);
				}
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000B4248 File Offset: 0x000B2648
		private void UpdateLegend()
		{
			if (!ConnectionStatus.IsHost() && ConnectionStatus.IsInSession())
			{
				this.m_legendText.SetLocalisedTextCatchAll(this.m_LegendText_Emote_NoRestart);
			}
			else if (!UserSystemUtils.AnySplitPadUsers())
			{
				this.m_legendText.SetLocalisedTextCatchAll((ClientGameSetup.Mode != GameMode.Campaign) ? this.m_LegendText_Emote_NoRestart : this.m_LegendText_Emote_Restart);
			}
			else
			{
				this.m_legendText.SetLocalisedTextCatchAll((ClientGameSetup.Mode != GameMode.Campaign) ? this.m_LegendText_NoEmote_NoRestart : this.m_LegendText_NoEmote_Restart);
			}
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000B42DC File Offset: 0x000B26DC
		public void SetScoreData(object _scoreData)
		{
			HordeRatingUIController.ScoreData scoreData = (HordeRatingUIController.ScoreData)_scoreData;
			GameSession gameSession = GameUtils.GetGameSession();
			SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
			int levelID = GameUtils.GetLevelID();
			GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(levelID);
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[levelID];
			SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = gameSession.LevelSettings.SceneDirectoryVarientEntry;
			this.m_levelTitleText.SetLocalisedTextCatchAll(sceneDirectoryEntry.Label);
			this.m_health.Value = Mathf.RoundToInt(scoreData.m_health * 100f);
			this.m_money.Value = scoreData.m_moneyEarned;
			this.m_enemies.Value = scoreData.m_enemiesDefeated;
			if (this.m_uiPlayers != null)
			{
				GamepadUser user = GameUtils.RequestManager<PlayerManager>().GetUser(EngagementSlot.One);
				this.m_uiPlayers.Show(user, null, null, true);
			}
			if (this.m_onionKingAnimator != null)
			{
				this.m_onionKingAnimator.SetFloat(HordeRatingUIController.m_iScoreAnimHash, scoreData.m_health);
				this.m_onionKingAnimator.Update(0f);
			}
			if (this.m_kevinAnimator != null)
			{
				this.m_kevinAnimator.SetFloat(HordeRatingUIController.m_iScoreAnimHash, scoreData.m_health);
				this.m_kevinAnimator.Update(0f);
			}
			base.StartCoroutine(this.TickUpScore(scoreData));
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000B4430 File Offset: 0x000B2830
		private IEnumerator TickUpScore(HordeRatingUIController.ScoreData _scoreData)
		{
			this.m_healthBar.Value = 0f;
			while (!this.m_animator.GetBool("Ready"))
			{
				yield return null;
			}
			this.m_animator.SetBool(HordeRatingUIController.m_iHealthBarAnimHash, true);
			this.m_tickUpProgress = 0f;
			while (this.m_tickUpProgress < 1f)
			{
				this.m_tickUpProgress = Mathf.Min(this.m_tickUpProgress + Time.deltaTime / 2f, 1f);
				this.m_healthBar.Value = _scoreData.m_health * this.m_tickUpProgress;
				this.m_animator.SetInteger(HordeRatingUIController.m_iHealthAnimHash, Mathf.RoundToInt(_scoreData.m_health * 100f * this.m_tickUpProgress));
				yield return null;
			}
			this.m_animator.SetBool(HordeRatingUIController.m_iHealthBarAnimHash, false);
			yield break;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000B4452 File Offset: 0x000B2852
		public bool HasAnimationSettled()
		{
			return !this.m_animator.GetBool(HordeRatingUIController.m_iHealthBarAnimHash);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000B4468 File Offset: 0x000B2868
		public bool AllowedToSkip()
		{
			if (T17EventSystemsManager.Instance != null)
			{
				T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
				if (eventSystemForEngagementSlot != null)
				{
					return eventSystemForEngagementSlot.currentSelectedGameObject == null;
				}
			}
			return true;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000B44A5 File Offset: 0x000B28A5
		public bool AllowedToRestart()
		{
			return !this.m_focusedOnPlayers;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000B44B0 File Offset: 0x000B28B0
		private void Update()
		{
			if (this.m_uiPlayers != null)
			{
				if (this.m_focusPlayersButton.JustPressed())
				{
					this.m_focusPlayersButton.ClaimPressEvent();
					if (T17EventSystemsManager.Instance != null)
					{
						T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
						if (eventSystemForEngagementSlot != null && eventSystemForEngagementSlot.currentSelectedGameObject == null)
						{
							this.m_focusedOnPlayers = true;
							this.m_uiPlayers.FocusOnFirstPlayer(true);
							this.m_legendText.SetLocalisedTextCatchAll(HordeRatingUIController.m_focusedLegendText);
						}
					}
				}
				if (this.m_backButton.JustPressed())
				{
					this.m_backButton.ClaimPressEvent();
					if (T17EventSystemsManager.Instance != null)
					{
						T17EventSystem eventSystemForEngagementSlot2 = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
						if (eventSystemForEngagementSlot2 != null)
						{
							if (eventSystemForEngagementSlot2.currentSelectedGameObject != null)
							{
								this.m_uiPlayers.CloseAllPlayerMenus();
								eventSystemForEngagementSlot2.SetSelectedGameObject(null);
								eventSystemForEngagementSlot2.SetSelectedGameObject(null);
								this.UpdateLegend();
							}
							else
							{
								this.m_focusedOnPlayers = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x04001DDE RID: 7646
		[SerializeField]
		private T17Text m_levelTitleText;

		// Token: 0x04001DDF RID: 7647
		[SerializeField]
		private T17Text m_legendText;

		// Token: 0x04001DE0 RID: 7648
		[SerializeField]
		private UIPlayerRootMenu m_uiPlayers;

		// Token: 0x04001DE1 RID: 7649
		[SerializeField]
		public GameObject m_waitingForPlayers;

		// Token: 0x04001DE2 RID: 7650
		[SerializeField]
		private ProgressBarUI m_healthBar;

		// Token: 0x04001DE3 RID: 7651
		[SerializeField]
		private DisplayIntUIController m_health;

		// Token: 0x04001DE4 RID: 7652
		[SerializeField]
		private DisplayIntUIController m_money;

		// Token: 0x04001DE5 RID: 7653
		[SerializeField]
		private DisplayIntUIController m_enemies;

		// Token: 0x04001DE6 RID: 7654
		[SerializeField]
		private Animator m_onionKingAnimator;

		// Token: 0x04001DE7 RID: 7655
		[SerializeField]
		private Animator m_kevinAnimator;

		// Token: 0x04001DE8 RID: 7656
		[SerializeField]
		[Range(0.001f, 10f)]
		private float m_tickUpDuration = 1f;

		// Token: 0x04001DE9 RID: 7657
		private float m_tickUpProgress;

		// Token: 0x04001DEA RID: 7658
		private Animator m_animator;

		// Token: 0x04001DEB RID: 7659
		public string m_LegendText_Emote_NoRestart = "Text.Menu.Legend03";

		// Token: 0x04001DEC RID: 7660
		public string m_LegendText_NoEmote_NoRestart = "Text.Menu.Legend03NoEmote";

		// Token: 0x04001DED RID: 7661
		public string m_LegendText_Emote_Restart = "Text.Menu.Legend03Restart";

		// Token: 0x04001DEE RID: 7662
		public string m_LegendText_NoEmote_Restart = "Text.Menu.Legend03RestartNoEmote";

		// Token: 0x04001DEF RID: 7663
		private static readonly string m_focusedLegendText = "Text.Menu.RoundResultsCancel";

		// Token: 0x04001DF0 RID: 7664
		private static readonly string m_healthLocalisationTag = "Horde.Health";

		// Token: 0x04001DF1 RID: 7665
		private static readonly int m_iHealthAnimHash = Animator.StringToHash("Health");

		// Token: 0x04001DF2 RID: 7666
		private static readonly int m_iHealthBarAnimHash = Animator.StringToHash("DoProgress");

		// Token: 0x04001DF3 RID: 7667
		private static readonly int m_iScoreAnimHash = Animator.StringToHash("Score");

		// Token: 0x04001DF4 RID: 7668
		private ILogicalButton m_focusPlayersButton;

		// Token: 0x04001DF5 RID: 7669
		private ILogicalButton m_backButton;

		// Token: 0x04001DF6 RID: 7670
		private bool m_focusedOnPlayers;

		// Token: 0x04001DF7 RID: 7671
		private const float c_timeForHealthBar = 2f;

		// Token: 0x020007BF RID: 1983
		public struct ScoreData
		{
			// Token: 0x04001DF8 RID: 7672
			public float m_health;

			// Token: 0x04001DF9 RID: 7673
			public int m_moneyEarned;

			// Token: 0x04001DFA RID: 7674
			public int m_enemiesDefeated;
		}
	}
}
