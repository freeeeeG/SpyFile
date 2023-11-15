using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B2B RID: 2859
public class MultiChefPadUIController : UIControllerBase
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x060039E1 RID: 14817 RVA: 0x00113558 File Offset: 0x00111958
	// (remove) Token: 0x060039E2 RID: 14818 RVA: 0x00113590 File Offset: 0x00111990
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid OnSplitStateChange = delegate()
	{
	};

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x060039E3 RID: 14819 RVA: 0x001135C6 File Offset: 0x001119C6
	public bool IsPadActive
	{
		get
		{
			return this.Slot == EngagementSlot.One || this.m_playerManager.GetUser(this.Slot) != null;
		}
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x060039E4 RID: 14820 RVA: 0x001135ED File Offset: 0x001119ED
	private EngagementSlot Slot
	{
		get
		{
			return (EngagementSlot)this.m_padNum;
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x060039E5 RID: 14821 RVA: 0x001135F5 File Offset: 0x001119F5
	public ControlPadInput.PadNum PadNum
	{
		get
		{
			return this.m_padNum;
		}
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x00113600 File Offset: 0x00111A00
	public void Reinitialise()
	{
		GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
		GameInputConfig.ConfigEntry[] array = baseInputConfig.m_playerConfigs.FindAll((GameInputConfig.ConfigEntry x) => x.Pad == this.m_padNum);
		if (array.Length == 0 || !this.IsPadActive)
		{
			this.SetState(MultiChefPadUIController.State.NotThere);
		}
		else if (array.Length == 1)
		{
			PadSide uihandedness = array[0].UIHandedness;
			this.AssignPlayer(uihandedness, array[0].Player);
			if (uihandedness == PadSide.Both)
			{
				this.SetState(MultiChefPadUIController.State.Combined);
				return;
			}
			if (uihandedness == PadSide.Left)
			{
				this.SetState(MultiChefPadUIController.State.LeftOnly);
				return;
			}
			if (uihandedness == PadSide.Right)
			{
				this.SetState(MultiChefPadUIController.State.RightOnly);
				return;
			}
		}
		else
		{
			foreach (GameInputConfig.ConfigEntry configEntry in array)
			{
				this.AssignPlayer(configEntry.Side, configEntry.Player);
			}
			this.SetState(MultiChefPadUIController.State.Split);
		}
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x001136D8 File Offset: 0x00111AD8
	public void AssignPlayer(PadSide _side, PlayerInputLookup.Player _player)
	{
		this.GetTextForSide(_side).text = ((int)(_player + 1)).ToString();
		ChefColourData chefColourData = this.m_avatarDirectory.Colours[(int)_player];
		this.GetPadImage(_side).color = chefColourData.PadUIColour;
		this.GetNumberCircle(_side).color = chefColourData.UIColour;
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x00113734 File Offset: 0x00111B34
	private Text GetTextForSide(PadSide _side)
	{
		if (_side == PadSide.Both)
		{
			return this.m_bothNumber;
		}
		if (_side == PadSide.Left)
		{
			return this.m_leftNumber;
		}
		if (_side != PadSide.Right)
		{
			return null;
		}
		return this.m_rightNumber;
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x00113765 File Offset: 0x00111B65
	private Image GetPadImage(PadSide _side)
	{
		if (_side == PadSide.Left)
		{
			return this.m_leftPadImage;
		}
		if (_side == PadSide.Right)
		{
			return this.m_rightPadImage;
		}
		if (_side != PadSide.Both)
		{
			return null;
		}
		return this.m_bothPadImage;
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x00113796 File Offset: 0x00111B96
	private Image GetNumberCircle(PadSide _side)
	{
		if (_side == PadSide.Left)
		{
			return this.m_leftNumberCircle;
		}
		if (_side == PadSide.Right)
		{
			return this.m_rightNumberCircle;
		}
		if (_side != PadSide.Both)
		{
			return null;
		}
		return this.m_bothNumberCircle;
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x001137C8 File Offset: 0x00111BC8
	private void Awake()
	{
		PlayerGameInput playerGameInput = new PlayerGameInput(this.m_padNum, PadSide.Both, this.m_sidedMappingData);
		this.m_splitButtonL = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UILeft, playerGameInput);
		this.m_splitButtonR = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UIRight, playerGameInput);
		this.m_playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_padSplitManager = GameUtils.RequireManager<PadSplitManager>();
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x0011381C File Offset: 0x00111C1C
	private bool MonitorPadAttached()
	{
		bool isPadActive = this.IsPadActive;
		if (this.m_state != MultiChefPadUIController.State.NotThere && !isPadActive)
		{
			this.SetState(MultiChefPadUIController.State.NotThere);
		}
		if (isPadActive && this.m_state == MultiChefPadUIController.State.NotThere)
		{
			if (this.CanAddPlayerQuery(1))
			{
				this.SetState(MultiChefPadUIController.State.Combined);
			}
			else if (!this.m_padAtttached)
			{
				this.m_animator.SetTrigger(MultiChefPadUIController.m_iMaxChefsWarning);
			}
		}
		this.m_padAtttached = isPadActive;
		return this.m_state != MultiChefPadUIController.State.NotThere;
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x001138A4 File Offset: 0x00111CA4
	private bool IsKeyboard()
	{
		GamepadUser user = this.m_playerManager.GetUser(this.Slot);
		return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x001138DC File Offset: 0x00111CDC
	private void Update()
	{
		bool flag = this.IsKeyboard();
		bool flag2 = this.MonitorPadAttached();
		this.m_arrows.gameObject.SetActive(flag2);
		if (flag2)
		{
			if (this.m_splitButtonL.JustPressed())
			{
				this.MoveState(-1);
			}
			if (this.m_splitButtonR.JustPressed())
			{
				this.MoveState(1);
			}
		}
		if (this.m_padImageBoth == null)
		{
			this.m_padImageBoth = this.m_bothPadImage.sprite;
			this.m_padImageL = this.m_leftPadImage.sprite;
			this.m_padImageR = this.m_rightPadImage.sprite;
		}
		else
		{
			this.m_bothPadImage.sprite = ((!flag) ? this.m_padImageBoth : this.m_keyboardBothImage);
			this.m_leftPadImage.sprite = ((!flag) ? this.m_padImageL : this.m_keyboardLeftImage);
			this.m_rightPadImage.sprite = ((!flag) ? this.m_padImageR : this.m_keyboardRightImage);
			this.m_noneImage.enabled = !flag;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x060039EF RID: 14831 RVA: 0x001139FA File Offset: 0x00111DFA
	// (set) Token: 0x060039F0 RID: 14832 RVA: 0x00113A04 File Offset: 0x00111E04
	private int StateId
	{
		get
		{
			return this.m_state - MultiChefPadUIController.State.LeftOnly;
		}
		set
		{
			int intervalMin = 1;
			int num = 4;
			MultiChefPadUIController.State state = (MultiChefPadUIController.State)MathUtils.Wrap(value + 1, intervalMin, num + 1);
			this.SetState(state);
		}
	}

	// Token: 0x060039F1 RID: 14833 RVA: 0x00113A2C File Offset: 0x00111E2C
	private void MoveState(int _progression)
	{
		int intervalMin = 1;
		int num = 4;
		int num2 = MathUtils.Wrap((int)(this.m_state + _progression), intervalMin, num + 1);
		MultiChefPadUIController.State state = (MultiChefPadUIController.State)num2;
		if (state == MultiChefPadUIController.State.Split && !this.CanAddPlayerQuery(1))
		{
			this.m_animator.SetTrigger(MultiChefPadUIController.m_iMaxChefsWarning);
			return;
		}
		this.SetState(state);
	}

	// Token: 0x060039F2 RID: 14834 RVA: 0x00113A84 File Offset: 0x00111E84
	private void SetState(MultiChefPadUIController.State _state)
	{
		this.m_animator.SetBool(MultiChefPadUIController.m_iPadConnected, _state != MultiChefPadUIController.State.NotThere);
		this.m_animator.SetBool(MultiChefPadUIController.m_iHasSplitLeft, _state == MultiChefPadUIController.State.LeftOnly || _state == MultiChefPadUIController.State.Split);
		this.m_animator.SetBool(MultiChefPadUIController.m_iHasSplitRight, _state == MultiChefPadUIController.State.RightOnly || _state == MultiChefPadUIController.State.Split);
		this.m_state = _state;
		this.OnSplitStateChange();
	}

	// Token: 0x060039F3 RID: 14835 RVA: 0x00113AF8 File Offset: 0x00111EF8
	public IEnumerable IterateSides()
	{
		switch (this.m_state)
		{
		case MultiChefPadUIController.State.NotThere:
			yield break;
		case MultiChefPadUIController.State.LeftOnly:
			yield return PadSide.Left;
			break;
		case MultiChefPadUIController.State.Combined:
			yield return PadSide.Both;
			break;
		case MultiChefPadUIController.State.RightOnly:
			yield return PadSide.Right;
			break;
		case MultiChefPadUIController.State.Split:
			yield return PadSide.Left;
			yield return PadSide.Right;
			break;
		}
		yield break;
	}

	// Token: 0x04002EC7 RID: 11975
	[SerializeField]
	private ControlPadInput.PadNum m_padNum;

	// Token: 0x04002EC8 RID: 11976
	[SerializeField]
	[AssignChildRecursive("BothNumber", Editorbility.NonEditable)]
	private Text m_bothNumber;

	// Token: 0x04002EC9 RID: 11977
	[SerializeField]
	[AssignChildRecursive("LeftNumber", Editorbility.NonEditable)]
	private Text m_leftNumber;

	// Token: 0x04002ECA RID: 11978
	[SerializeField]
	[AssignChildRecursive("RightNumber", Editorbility.NonEditable)]
	private Text m_rightNumber;

	// Token: 0x04002ECB RID: 11979
	[SerializeField]
	[AssignChildRecursive("PadImage", Editorbility.NonEditable)]
	private Image m_noneImage;

	// Token: 0x04002ECC RID: 11980
	[SerializeField]
	[AssignChildRecursive("PadImageL", Editorbility.NonEditable)]
	private Image m_leftPadImage;

	// Token: 0x04002ECD RID: 11981
	[SerializeField]
	[AssignChildRecursive("PadImageR", Editorbility.NonEditable)]
	private Image m_rightPadImage;

	// Token: 0x04002ECE RID: 11982
	[SerializeField]
	[AssignChildRecursive("PadImageB", Editorbility.NonEditable)]
	private Image m_bothPadImage;

	// Token: 0x04002ECF RID: 11983
	[SerializeField]
	[AssignChildRecursive("NumberCircleL", Editorbility.NonEditable)]
	private Image m_leftNumberCircle;

	// Token: 0x04002ED0 RID: 11984
	[SerializeField]
	[AssignChildRecursive("NumberCircleR", Editorbility.NonEditable)]
	private Image m_rightNumberCircle;

	// Token: 0x04002ED1 RID: 11985
	[SerializeField]
	[AssignChildRecursive("NumberCircleB", Editorbility.NonEditable)]
	private Image m_bothNumberCircle;

	// Token: 0x04002ED2 RID: 11986
	[SerializeField]
	[AssignResource("MainAvatarDirectory", Editorbility.NonEditable)]
	private AvatarDirectoryData m_avatarDirectory;

	// Token: 0x04002ED3 RID: 11987
	[SerializeField]
	[AssignResource("SidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_sidedMappingData;

	// Token: 0x04002ED4 RID: 11988
	[SerializeField]
	[AssignChildRecursive("Arrows", Editorbility.NonEditable)]
	private Image m_arrows;

	// Token: 0x04002ED5 RID: 11989
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Animator m_animator;

	// Token: 0x04002ED6 RID: 11990
	[SerializeField]
	private Sprite m_keyboardBothImage;

	// Token: 0x04002ED7 RID: 11991
	[SerializeField]
	private Sprite m_keyboardLeftImage;

	// Token: 0x04002ED8 RID: 11992
	[SerializeField]
	private Sprite m_keyboardRightImage;

	// Token: 0x04002EDA RID: 11994
	public Generic<bool, int> CanAddPlayerQuery;

	// Token: 0x04002EDB RID: 11995
	private PadSplitManager m_padSplitManager;

	// Token: 0x04002EDC RID: 11996
	private IPlayerManager m_playerManager;

	// Token: 0x04002EDD RID: 11997
	private ILogicalButton m_splitButtonL;

	// Token: 0x04002EDE RID: 11998
	private ILogicalButton m_splitButtonR;

	// Token: 0x04002EDF RID: 11999
	private Sprite m_padImageBoth;

	// Token: 0x04002EE0 RID: 12000
	private Sprite m_padImageL;

	// Token: 0x04002EE1 RID: 12001
	private Sprite m_padImageR;

	// Token: 0x04002EE2 RID: 12002
	private bool m_padAtttached;

	// Token: 0x04002EE3 RID: 12003
	private MultiChefPadUIController.State m_state;

	// Token: 0x04002EE4 RID: 12004
	private static readonly int m_iPadConnected = Animator.StringToHash("PadConnected");

	// Token: 0x04002EE5 RID: 12005
	private static readonly int m_iHasSplitLeft = Animator.StringToHash("HasSplitLeft");

	// Token: 0x04002EE6 RID: 12006
	private static readonly int m_iHasSplitRight = Animator.StringToHash("HasSplitRight");

	// Token: 0x04002EE7 RID: 12007
	private static readonly int m_iMaxChefsWarning = Animator.StringToHash("MaxChefsWarning");

	// Token: 0x02000B2C RID: 2860
	private enum State
	{
		// Token: 0x04002EEA RID: 12010
		NotThere,
		// Token: 0x04002EEB RID: 12011
		LeftOnly,
		// Token: 0x04002EEC RID: 12012
		Combined,
		// Token: 0x04002EED RID: 12013
		RightOnly,
		// Token: 0x04002EEE RID: 12014
		Split
	}
}
