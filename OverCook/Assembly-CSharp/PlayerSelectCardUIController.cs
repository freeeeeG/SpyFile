using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B3E RID: 2878
public class PlayerSelectCardUIController : UIControllerBase
{
	// Token: 0x06003A65 RID: 14949 RVA: 0x00116109 File Offset: 0x00114509
	private void OnDestroy()
	{
		this.m_gamepadEngagementManager.SetCanDisconnect((EngagementSlot)this.m_actualPlayer, true);
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x0011611D File Offset: 0x0011451D
	public PlayerSelectCardUIController.State GetState()
	{
		return this.m_state;
	}

	// Token: 0x06003A67 RID: 14951 RVA: 0x00116128 File Offset: 0x00114528
	public void SetState(PlayerSelectCardUIController.State _state)
	{
		switch (_state)
		{
		case PlayerSelectCardUIController.State.PressStart:
			this.m_chef.enabled = false;
			this.m_chefArrows.enabled = false;
			this.m_chefAButton.gameObject.SetActive(false);
			this.m_colourArrows.enabled = false;
			this.m_colourAButton.gameObject.SetActive(false);
			this.m_backgroundImage.SetActive(false);
			this.m_pressStart.SetActive(true);
			this.m_gamepadEngagementManager.SetCanDisconnect((EngagementSlot)this.m_actualPlayer, true);
			break;
		case PlayerSelectCardUIController.State.SelectChef:
			this.m_chef.enabled = true;
			this.m_chefArrows.enabled = true;
			this.m_chefAButton.gameObject.SetActive(true);
			this.m_colourArrows.enabled = false;
			this.m_colourAButton.gameObject.SetActive(false);
			this.m_backgroundImage.SetActive(this.m_skipColourSelection);
			this.m_pressStart.SetActive(false);
			this.m_gamepadEngagementManager.SetCanDisconnect((EngagementSlot)this.m_actualPlayer, true);
			break;
		case PlayerSelectCardUIController.State.SelectColour:
			this.m_chef.enabled = true;
			this.m_chefArrows.enabled = false;
			this.m_chefAButton.gameObject.SetActive(false);
			this.m_colourArrows.enabled = true;
			this.m_colourAButton.gameObject.SetActive(true);
			this.m_backgroundImage.SetActive(true);
			this.m_pressStart.SetActive(false);
			this.m_gamepadEngagementManager.SetCanDisconnect((EngagementSlot)this.m_actualPlayer, false);
			break;
		case PlayerSelectCardUIController.State.Confirmed:
			this.m_chef.enabled = true;
			this.m_chefArrows.enabled = false;
			this.m_chefAButton.gameObject.SetActive(false);
			this.m_colourArrows.enabled = false;
			this.m_colourAButton.gameObject.SetActive(false);
			this.m_backgroundImage.SetActive(true);
			this.m_pressStart.SetActive(false);
			this.m_gamepadEngagementManager.SetCanDisconnect((EngagementSlot)this.m_actualPlayer, false);
			break;
		}
		this.m_state = _state;
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x00116330 File Offset: 0x00114730
	public void SetDeactivationOverride(Generic<bool> _callback)
	{
		this.m_deactivationOverride = _callback;
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x00116339 File Offset: 0x00114739
	public bool IsActive()
	{
		return this.m_assignedInput != null;
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x00116348 File Offset: 0x00114748
	private bool IsKeyBoard(PlayerGameInput _input)
	{
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		GamepadUser user = playerManager.GetUser((EngagementSlot)_input.Pad);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x00116380 File Offset: 0x00114780
	public void Activate(PlayerGameInput _input, int _colourOptions, GameSession.SelectedChefData _default = null, PlayerSelectCardUIController.State _state = PlayerSelectCardUIController.State.SelectChef)
	{
		this.m_assignedInput = _input;
		this.m_leftButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UILeft, _input);
		this.m_rightButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UIRight, _input);
		this.m_selectButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UISelect, _input);
		this.m_cancelButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UICancel, _input);
		ControlPadInput.Button? controlPadButton = PlayerButtonImage.GetControlPadButton<ControlPadInput.Button>(this.m_selectButton, ControllerIconLookup.DeviceContext.Pad);
		bool flag = this.IsKeyBoard(_input);
		for (int i = 0; i < this.m_buttonImages.Length; i++)
		{
			if (controlPadButton != null)
			{
				this.m_buttonImages[i].enabled = true;
				this.m_buttonImages[i].SetData(controlPadButton.Value, (!flag) ? ControllerIconLookup.DeviceContext.Pad : ControllerIconLookup.DeviceContext.Keyboard);
			}
			else
			{
				this.m_buttonImages[i].enabled = false;
			}
		}
		if (_colourOptions <= 1)
		{
			this.m_colourSelection = (int)this.m_actualPlayer;
			this.m_skipColourSelection = true;
			this.m_chefArrows.color = Color.white;
		}
		else
		{
			this.m_colourSelection = (int)(this.m_actualPlayer % (PlayerInputLookup.Player)_colourOptions);
			this.m_skipColourSelection = false;
		}
		this.m_colourCount = _colourOptions;
		this.SetColourSelection(this.m_colourSelection);
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		this.m_avatars = metaGameProgress.GetUnlockedAvatars();
		if (_default != null)
		{
			int num = this.m_avatars.FindIndex_Predicate((ChefAvatarData x) => x == _default.Character);
			if (num != -1)
			{
				this.SetChefSelection(num);
			}
		}
		if (this.m_chefSelection == -1)
		{
			this.SetChefSelection(UnityEngine.Random.Range(0, this.m_avatars.Length));
		}
		this.SetState(_state);
		base.enabled = true;
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x00116524 File Offset: 0x00114924
	public void SetChefSelection(GameSession.SelectedChefData _data)
	{
		if (this.m_state == PlayerSelectCardUIController.State.SelectChef)
		{
			int num = this.m_avatars.FindIndex_Predicate((ChefAvatarData x) => x == _data.Character);
			if (num != -1)
			{
				this.SetChefSelection(num);
			}
		}
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x00116570 File Offset: 0x00114970
	public void Deactivate()
	{
		this.m_gamepadEngagementManager.SetCanDisconnect((EngagementSlot)this.m_actualPlayer, true);
		this.m_assignedInput = null;
		this.SetState(PlayerSelectCardUIController.State.PressStart);
		base.enabled = false;
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x00116599 File Offset: 0x00114999
	public GameSession.SelectedChefData GetFullSelection()
	{
		if (this.m_state == PlayerSelectCardUIController.State.Confirmed)
		{
			return this.GetCurrentSelection();
		}
		return null;
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x001165B0 File Offset: 0x001149B0
	public GameSession.SelectedChefData GetCurrentSelection()
	{
		if (this.m_avatars.Length > 0)
		{
			ChefAvatarData chefAvatarData = this.m_avatars[this.m_chefSelection];
			ChefColourData colourData = this.m_avatarDirectory.Colours[this.m_colourSelection];
			return new GameSession.SelectedChefData(chefAvatarData, colourData);
		}
		return null;
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x001165F5 File Offset: 0x001149F5
	public PlayerInputLookup.Player GetActualPlayer()
	{
		return this.m_actualPlayer;
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x001165FD File Offset: 0x001149FD
	public PlayerGameInput GetAssignedInput()
	{
		return this.m_assignedInput;
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x00116605 File Offset: 0x00114A05
	private void Awake()
	{
		this.m_avatarDirectory = GameUtils.GetGameSession().Progress.GetAvatarDirectory();
		this.m_padSplitManager = GameUtils.RequireManager<PadSplitManager>();
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
		this.Deactivate();
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x00116638 File Offset: 0x00114A38
	private void Update()
	{
		if (TimeManager.IsPaused(base.gameObject) || this.m_padSplitManager.IsUIOpen())
		{
			this.m_leftButton.ClaimPressEvent();
			this.m_rightButton.ClaimPressEvent();
			this.m_cancelButton.ClaimPressEvent();
			this.m_selectButton.ClaimPressEvent();
			return;
		}
		PlayerSelectCardUIController.State state = this.m_state;
		if (state != PlayerSelectCardUIController.State.SelectChef)
		{
			if (state != PlayerSelectCardUIController.State.SelectColour)
			{
				if (state == PlayerSelectCardUIController.State.Confirmed)
				{
					this.UpdateConfirmed();
				}
			}
			else
			{
				this.UpdateColourSelect();
			}
		}
		else
		{
			this.UpdateChefSelect();
		}
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x001166D4 File Offset: 0x00114AD4
	private void UpdateChefSelect()
	{
		if (this.m_leftButton.JustPressed())
		{
			this.SetChefSelection(this.m_chefSelection - 1);
		}
		if (this.m_rightButton.JustPressed())
		{
			this.SetChefSelection(this.m_chefSelection + 1);
		}
		if (this.m_selectButton.JustPressed())
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.UISelect, base.gameObject.layer);
			if (this.m_skipColourSelection)
			{
				this.SetState(PlayerSelectCardUIController.State.Confirmed);
			}
			else
			{
				this.SetState(PlayerSelectCardUIController.State.SelectColour);
			}
		}
		if (this.m_cancelButton.JustPressed())
		{
			if (this.m_deactivationOverride != null)
			{
				if (this.m_deactivationOverride())
				{
					base.enabled = false;
				}
			}
			else
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.UIBack, base.gameObject.layer);
				this.Deactivate();
			}
		}
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x001167B0 File Offset: 0x00114BB0
	private void UpdateColourSelect()
	{
		if (this.m_leftButton.JustPressed())
		{
			this.SetColourSelection(this.m_colourSelection - 1);
		}
		if (this.m_rightButton.JustPressed())
		{
			this.SetColourSelection(this.m_colourSelection + 1);
		}
		if (this.m_selectButton.JustPressed())
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.UISelect, base.gameObject.layer);
			this.SetState(PlayerSelectCardUIController.State.Confirmed);
		}
		if (this.m_cancelButton.JustPressed())
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.UIBack, base.gameObject.layer);
			this.SetState(PlayerSelectCardUIController.State.SelectChef);
		}
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x00116850 File Offset: 0x00114C50
	private void UpdateConfirmed()
	{
		if (this.m_cancelButton.JustPressed())
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.UIBack, base.gameObject.layer);
			if (this.m_skipColourSelection)
			{
				this.SetState(PlayerSelectCardUIController.State.SelectChef);
			}
			else
			{
				this.SetState(PlayerSelectCardUIController.State.SelectColour);
			}
		}
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x001168A0 File Offset: 0x00114CA0
	private void SetColourSelection(int _selection)
	{
		if (this.m_colourCount > 0)
		{
			this.m_colourSelection = MathUtils.Wrap(_selection, 0, this.m_colourCount);
		}
		else
		{
			this.m_colourSelection = _selection;
		}
		ChefColourData chefColourData = this.m_avatarDirectory.Colours[this.m_colourSelection];
		Image[] array = this.m_backgroundImage.RequestComponentsRecursive<Image>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = chefColourData.UIColour;
		}
		this.m_colourArrows.color = chefColourData.UIColour;
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x0011692C File Offset: 0x00114D2C
	private void SetChefSelection(int _selection)
	{
		if (this.m_avatars.Length > 0)
		{
			this.m_chefSelection = MathUtils.Wrap(_selection, 0, this.m_avatars.Length);
			ChefAvatarData chefAvatarData = this.m_avatars[this.m_chefSelection];
		}
	}

	// Token: 0x04002F50 RID: 12112
	[SerializeField]
	private PlayerInputLookup.Player m_actualPlayer;

	// Token: 0x04002F51 RID: 12113
	[SerializeField]
	[AssignChild("Chef", Editorbility.NonEditable)]
	private Image m_chef;

	// Token: 0x04002F52 RID: 12114
	[SerializeField]
	[AssignChild("ChefArrows", Editorbility.NonEditable)]
	private Image m_chefArrows;

	// Token: 0x04002F53 RID: 12115
	[SerializeField]
	[AssignChild("ChefAButton", Editorbility.NonEditable)]
	private Image m_chefAButton;

	// Token: 0x04002F54 RID: 12116
	[SerializeField]
	[AssignChild("Image", Editorbility.NonEditable)]
	private GameObject m_backgroundImage;

	// Token: 0x04002F55 RID: 12117
	[SerializeField]
	[AssignChild("ColourArrows", Editorbility.NonEditable)]
	private Image m_colourArrows;

	// Token: 0x04002F56 RID: 12118
	[SerializeField]
	[AssignChild("ColourAButton", Editorbility.NonEditable)]
	private Image m_colourAButton;

	// Token: 0x04002F57 RID: 12119
	[SerializeField]
	[AssignChild("PressStart", Editorbility.NonEditable)]
	private GameObject m_pressStart;

	// Token: 0x04002F58 RID: 12120
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	private ButtonImage[] m_buttonImages;

	// Token: 0x04002F59 RID: 12121
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002F5A RID: 12122
	private PlayerGameInput m_assignedInput;

	// Token: 0x04002F5B RID: 12123
	private AvatarDirectoryData m_avatarDirectory;

	// Token: 0x04002F5C RID: 12124
	private Generic<bool> m_deactivationOverride;

	// Token: 0x04002F5D RID: 12125
	private PadSplitManager m_padSplitManager;

	// Token: 0x04002F5E RID: 12126
	private ILogicalButton m_leftButton;

	// Token: 0x04002F5F RID: 12127
	private ILogicalButton m_rightButton;

	// Token: 0x04002F60 RID: 12128
	private ILogicalButton m_selectButton;

	// Token: 0x04002F61 RID: 12129
	private ILogicalButton m_cancelButton;

	// Token: 0x04002F62 RID: 12130
	private PlayerSelectCardUIController.State m_state;

	// Token: 0x04002F63 RID: 12131
	private ChefAvatarData[] m_avatars;

	// Token: 0x04002F64 RID: 12132
	private int m_colourCount;

	// Token: 0x04002F65 RID: 12133
	private int m_chefSelection = -1;

	// Token: 0x04002F66 RID: 12134
	private int m_colourSelection = -1;

	// Token: 0x04002F67 RID: 12135
	private bool m_skipColourSelection;

	// Token: 0x02000B3F RID: 2879
	public enum State
	{
		// Token: 0x04002F69 RID: 12137
		PressStart,
		// Token: 0x04002F6A RID: 12138
		SelectChef,
		// Token: 0x04002F6B RID: 12139
		SelectColour,
		// Token: 0x04002F6C RID: 12140
		Confirmed
	}
}
