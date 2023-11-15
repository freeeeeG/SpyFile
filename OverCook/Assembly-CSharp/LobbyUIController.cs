using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;

// Token: 0x02000B35 RID: 2869
[ExecutionDependency(typeof(PlayerSelectCardUIController))]
public class LobbyUIController : UIControllerBase
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06003A2A RID: 14890 RVA: 0x001151A8 File Offset: 0x001135A8
	// (remove) Token: 0x06003A2B RID: 14891 RVA: 0x001151DC File Offset: 0x001135DC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event VoidGeneric<bool> OpenCloseCallback;

	// Token: 0x06003A2C RID: 14892 RVA: 0x00115210 File Offset: 0x00113610
	protected void OnEnable()
	{
		LobbyUIController.IsOpen = true;
		LobbyUIController.OpenCloseCallback(LobbyUIController.IsOpen);
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.UpdateLobbyForEngagement));
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x00115247 File Offset: 0x00113647
	protected void OnDisable()
	{
		LobbyUIController.IsOpen = false;
		LobbyUIController.OpenCloseCallback(LobbyUIController.IsOpen);
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.UpdateLobbyForEngagement));
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x0011527E File Offset: 0x0011367E
	public virtual void SetSceneData(GameSession.GameType _gameType, SceneDirectoryData.SceneDirectoryEntry _sceneDirectoryEntry, GameProgress.GameProgressData.LevelProgress _levelProgress)
	{
		this.m_sceneDirectoryEntry = _sceneDirectoryEntry;
		this.m_progressData = _levelProgress;
	}

	// Token: 0x06003A2F RID: 14895 RVA: 0x0011528E File Offset: 0x0011368E
	public void RegisterForCompletedMessage(CallbackVoid _callback)
	{
		this.m_onCompleted = (CallbackVoid)Delegate.Combine(this.m_onCompleted, _callback);
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x001152A7 File Offset: 0x001136A7
	public void RegisterForCanceledMessage(CallbackVoid _callback)
	{
		this.m_onCancelled = (CallbackVoid)Delegate.Combine(this.m_onCancelled, _callback);
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x001152C0 File Offset: 0x001136C0
	public void SetInitialAvatars(Dictionary<PlayerInputLookup.Player, LobbyUIController.AvatarCardData> _cardData)
	{
		for (int i = 0; i < this.m_playerSelectCards.Length; i++)
		{
			PlayerGameInput assignedInput = this.m_playerSelectCards[i].GetAssignedInput();
			if (assignedInput != null && assignedInput.Pad == ControlPadInput.PadNum.One)
			{
				this.m_playerSelectCards[i].SetDeactivationOverride(new Generic<bool>(this.Exit));
			}
			else
			{
				this.m_playerSelectCards[i].SetDeactivationOverride(new Generic<bool>(this.False));
			}
			if (_cardData.ContainsKey((PlayerInputLookup.Player)i) && this.m_playerSelectCards[i].IsActive())
			{
				LobbyUIController.AvatarCardData avatarCardData = _cardData[(PlayerInputLookup.Player)i];
				this.m_playerSelectCards[i].SetChefSelection(avatarCardData.Chef);
			}
		}
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x00115378 File Offset: 0x00113778
	public Dictionary<PlayerInputLookup.Player, LobbyUIController.AvatarCardData> GetSelectedAvatars()
	{
		Dictionary<PlayerInputLookup.Player, LobbyUIController.AvatarCardData> dictionary = new Dictionary<PlayerInputLookup.Player, LobbyUIController.AvatarCardData>();
		int num = 0;
		for (int i = 0; i < this.m_playerSelectCards.Length; i++)
		{
			GameSession.SelectedChefData fullSelection = this.m_playerSelectCards[i].GetFullSelection();
			PlayerGameInput assignedInput = this.m_playerSelectCards[i].GetAssignedInput();
			if (fullSelection != null)
			{
				dictionary.Add((PlayerInputLookup.Player)num, new LobbyUIController.AvatarCardData(assignedInput, fullSelection));
				num++;
			}
		}
		return dictionary;
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x001153E0 File Offset: 0x001137E0
	private PlayerGameInput GetInputForID(int _id)
	{
		GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
		return baseInputConfig.GetInputData((PlayerInputLookup.Player)_id);
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x001153FC File Offset: 0x001137FC
	protected virtual void Awake()
	{
		if (GameUtils.GetGameSession().TypeSettings.Type == GameSession.GameType.Competitive)
		{
			this.m_colourOptions = 2;
		}
		else
		{
			this.m_colourOptions = 0;
		}
		this.m_playerSelectCards = base.gameObject.RequestComponentsRecursive<PlayerSelectCardUIController>();
		Array.Sort<PlayerSelectCardUIController>(this.m_playerSelectCards, (PlayerSelectCardUIController x, PlayerSelectCardUIController y) => x.GetActualPlayer().CompareTo(y.GetActualPlayer()));
		for (int i = 0; i < 4; i++)
		{
		}
		this.m_readyButton = base.transform.FindChildRecursive("ReadyButton").gameObject.RequireComponent<Image>();
		this.m_readyButton.gameObject.SetActive(false);
		this.m_playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.UpdateLobbyForEngagement();
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChange;
		this.m_playerStartButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x001154E3 File Offset: 0x001138E3
	private void OnDestroy()
	{
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChange;
	}

	// Token: 0x06003A36 RID: 14902 RVA: 0x001154FC File Offset: 0x001138FC
	private void OnEngagementChange(EngagementSlot _slot, GamepadUser _prev, GamepadUser _new)
	{
		if (_new == null && _prev != null)
		{
			this.DeactivateEntriesForPad((ControlPadInput.PadNum)_slot);
		}
		this.UpdateLobbyForEngagement();
	}

	// Token: 0x06003A37 RID: 14903 RVA: 0x00115524 File Offset: 0x00113924
	private void DeactivateEntriesForPad(ControlPadInput.PadNum _pad)
	{
		for (int i = 0; i < this.m_playerSelectCards.Length; i++)
		{
			if (this.m_playerSelectCards[i].IsActive())
			{
				PlayerGameInput assignedInput = this.m_playerSelectCards[i].GetAssignedInput();
				if (assignedInput.Pad == _pad)
				{
					this.m_playerSelectCards[i].Deactivate();
				}
			}
		}
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x00115584 File Offset: 0x00113984
	public static PlayerGameInput GetEngagedPlayerGameInput(IPlayerManager _iPlayerManager, int _index)
	{
		GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
		if (baseInputConfig != null)
		{
			GameInputConfig.ConfigEntry configEntry = baseInputConfig.m_playerConfigs.TryAtIndex(_index, null);
			if (configEntry != null)
			{
				EngagementSlot pad = (EngagementSlot)configEntry.Pad;
				if (_iPlayerManager.GetUser(pad) != null)
				{
					return new PlayerGameInput(configEntry.Pad, configEntry.Side, configEntry.AmbiControlsMapping);
				}
			}
		}
		return null;
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x001155E3 File Offset: 0x001139E3
	private bool Matching(PlayerGameInput _c1, PlayerGameInput _c2)
	{
		if (_c1 != null && _c2 != null)
		{
			return _c1.Pad == _c2.Pad && _c1.Side == _c2.Side;
		}
		return _c1 == _c2;
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x00115618 File Offset: 0x00113A18
	private void UpdateLobbyForEngagement()
	{
		LobbyUIController.AvatarCardData[] array = new LobbyUIController.AvatarCardData[this.m_playerSelectCards.Length];
		PlayerSelectCardUIController.State[] array2 = new PlayerSelectCardUIController.State[array.Length];
		for (int i = 0; i < this.m_playerSelectCards.Length; i++)
		{
			if (this.m_playerSelectCards[i].IsActive())
			{
				array[i] = new LobbyUIController.AvatarCardData(this.m_playerSelectCards[i].GetAssignedInput(), this.m_playerSelectCards[i].GetCurrentSelection());
				array2[i] = this.m_playerSelectCards[i].GetState();
			}
			else
			{
				array[i] = null;
			}
		}
		GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
		for (int j = 0; j < this.m_playerSelectCards.Length; j++)
		{
			PlayerGameInput newInputPlayer = LobbyUIController.GetEngagedPlayerGameInput(this.m_playerManager, j);
			if (this.m_playerSelectCards[j].IsActive() && !this.Matching(this.m_playerSelectCards[j].GetAssignedInput(), newInputPlayer))
			{
				this.m_playerSelectCards[j].Deactivate();
			}
			if (newInputPlayer != null && !this.m_playerSelectCards[j].IsActive())
			{
				int num = array.FindIndex_Predicate((LobbyUIController.AvatarCardData x) => x != null && this.Matching(x.PlayerInput, newInputPlayer));
				if (num == -1)
				{
					this.m_playerSelectCards[j].Activate(newInputPlayer, this.m_colourOptions, null, PlayerSelectCardUIController.State.SelectChef);
				}
				else
				{
					LobbyUIController.AvatarCardData avatarCardData = array[num];
					this.m_playerSelectCards[j].Activate(newInputPlayer, this.m_colourOptions, avatarCardData.Chef, array2[num]);
				}
			}
		}
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x001157B4 File Offset: 0x00113BB4
	private bool IsReadyForStart()
	{
		LobbyUIController.<IsReadyForStart>c__AnonStorey2 <IsReadyForStart>c__AnonStorey = new LobbyUIController.<IsReadyForStart>c__AnonStorey2();
		if (!this.AllCardsReady(out <IsReadyForStart>c__AnonStorey.chefData))
		{
			return false;
		}
		if (GameUtils.GetGameSession().TypeSettings.Type != GameSession.GameType.Competitive)
		{
			return this.m_sceneDirectoryEntry.GetSceneVarient(<IsReadyForStart>c__AnonStorey.chefData.Length) != null;
		}
		if (<IsReadyForStart>c__AnonStorey.chefData.Length != 2 && <IsReadyForStart>c__AnonStorey.chefData.Length != 4)
		{
			return false;
		}
		ChefColourData[] colourData = <IsReadyForStart>c__AnonStorey.chefData.ConvertAll((GameSession.SelectedChefData x) => x.Colour);
		int[] array = colourData.ConvertAll((ChefColourData x) => colourData.FindAll(new Predicate<ChefColourData>(x.Equals)).Length);
		return array.FindIndex_Predicate((int x) => x != <IsReadyForStart>c__AnonStorey.chefData.Length / 2) == -1;
	}

	// Token: 0x06003A3C RID: 14908 RVA: 0x00115890 File Offset: 0x00113C90
	protected virtual void Update()
	{
		bool flag = this.IsReadyForStart();
		if (flag && !this.m_readyButton.isActiveAndEnabled)
		{
			this.m_playerStartButton.ClaimPressEvent();
			this.m_playerStartButton.ClaimReleaseEvent();
		}
		this.m_readyButton.gameObject.SetActive(flag);
		if (this.m_playerStartButton.JustPressed() && this.m_readyButton.isActiveAndEnabled)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.UISelect, base.gameObject.layer);
			this.m_onCompleted();
		}
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x0011591F File Offset: 0x00113D1F
	private bool False()
	{
		return false;
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x00115922 File Offset: 0x00113D22
	private bool Exit()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIBack, base.gameObject.layer);
		this.m_onCancelled();
		return true;
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x00115944 File Offset: 0x00113D44
	private bool IsAssignedToCard(PlayerGameInput _player)
	{
		for (int i = 0; i < this.m_playerSelectCards.Length; i++)
		{
			PlayerSelectCardUIController playerSelectCardUIController = this.m_playerSelectCards[i];
			PlayerGameInput assignedInput = playerSelectCardUIController.GetAssignedInput();
			if (assignedInput != null && assignedInput.Pad == _player.Pad)
			{
				if (assignedInput.Side == PadSide.Both)
				{
					return true;
				}
				if (assignedInput.Side == _player.Side)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x001159B4 File Offset: 0x00113DB4
	private bool AllCardsReady(out GameSession.SelectedChefData[] o_chefData)
	{
		o_chefData = new GameSession.SelectedChefData[0];
		for (int i = 0; i < this.m_playerSelectCards.Length; i++)
		{
			if (this.m_playerSelectCards[i].GetAssignedInput() != null)
			{
				GameSession.SelectedChefData fullSelection = this.m_playerSelectCards[i].GetFullSelection();
				if (fullSelection == null)
				{
					return false;
				}
				ArrayUtils.PushBack<GameSession.SelectedChefData>(ref o_chefData, fullSelection);
			}
		}
		return true;
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x00115A12 File Offset: 0x00113E12
	// Note: this type is marked as 'beforefieldinit'.
	static LobbyUIController()
	{
		LobbyUIController.OpenCloseCallback = delegate(bool _b)
		{
		};
		LobbyUIController.IsOpen = false;
	}

	// Token: 0x04002F24 RID: 12068
	private ILogicalButton m_playerStartButton;

	// Token: 0x04002F25 RID: 12069
	private IPlayerManager m_playerManager;

	// Token: 0x04002F26 RID: 12070
	private CallbackVoid m_onCompleted = delegate()
	{
	};

	// Token: 0x04002F27 RID: 12071
	private CallbackVoid m_onCancelled = delegate()
	{
	};

	// Token: 0x04002F28 RID: 12072
	private Image m_readyButton;

	// Token: 0x04002F29 RID: 12073
	private int m_colourOptions;

	// Token: 0x04002F2A RID: 12074
	protected PlayerSelectCardUIController[] m_playerSelectCards = new PlayerSelectCardUIController[0];

	// Token: 0x04002F2B RID: 12075
	protected SceneDirectoryData.SceneDirectoryEntry m_sceneDirectoryEntry;

	// Token: 0x04002F2C RID: 12076
	protected GameProgress.GameProgressData.LevelProgress m_progressData;

	// Token: 0x04002F2E RID: 12078
	public static bool IsOpen;

	// Token: 0x02000B36 RID: 2870
	public class AvatarCardData
	{
		// Token: 0x06003A47 RID: 14919 RVA: 0x00115A66 File Offset: 0x00113E66
		public AvatarCardData(PlayerGameInput _playerInput, GameSession.SelectedChefData _chef)
		{
			this.PlayerInput = _playerInput;
			this.Chef = _chef;
		}

		// Token: 0x04002F33 RID: 12083
		public PlayerGameInput PlayerInput;

		// Token: 0x04002F34 RID: 12084
		public GameSession.SelectedChefData Chef;
	}
}
