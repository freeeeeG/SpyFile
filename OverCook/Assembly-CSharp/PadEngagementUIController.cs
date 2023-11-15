using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B3D RID: 2877
public class PadEngagementUIController : UIControllerBase
{
	// Token: 0x06003A5D RID: 14941 RVA: 0x00115DE0 File Offset: 0x001141E0
	private void Awake()
	{
		this.m_restoreFullPadSprite = this.m_fullPad.PcSprite;
		this.m_restoreLeftPadSprite = this.m_leftPad.PcSprite;
		this.m_restoreRightPadSprite = this.m_rightPad.PcSprite;
		this.m_iPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_iPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.RefreshCosmetics));
		if (PlayerInputLookup.IsAwake())
		{
			this.RefreshCosmetics();
		}
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x00115E72 File Offset: 0x00114272
	private void OnDestroy()
	{
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Remove(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.RefreshCosmetics));
		this.m_iPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x00115EAB File Offset: 0x001142AB
	private void OnEngagementChanged(EngagementSlot _e, GamepadUser _before, GamepadUser _new)
	{
		if (PlayerInputLookup.IsAwake())
		{
			this.RefreshCosmetics();
		}
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x00115EC0 File Offset: 0x001142C0
	private void RefreshCosmetics()
	{
		this.m_fullPad.gameObject.SetActive(false);
		this.m_leftPad.gameObject.SetActive(false);
		this.m_rightPad.gameObject.SetActive(false);
		this.m_profileName.gameObject.SetActive(false);
		this.m_background.enabled = true;
		GamepadUser user = this.m_iPlayerManager.GetUser(this.m_engagementSlot);
		if (user != null)
		{
			GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
			GameInputConfig.ConfigEntry[] array = baseInputConfig.m_playerConfigs.FindAll((GameInputConfig.ConfigEntry x) => x.Pad == (ControlPadInput.PadNum)this.m_engagementSlot);
			for (int i = 0; i < array.Length; i++)
			{
				PadSide uihandedness = array[i].UIHandedness;
				Image image = this.GetImage(uihandedness);
				image.gameObject.SetActive(true);
				ChefColourData chefColourData = this.m_avatarDirectory.Colours[(int)array[i].Player];
				image.color = chefColourData.PadUIColour;
			}
			this.m_profileName.gameObject.SetActive(true);
			this.m_profileName.text = this.Ellipsify(user.DisplayName);
			bool flag = user.ControlType == GamepadUser.ControlTypeEnum.Pad;
			this.m_fullPad.sprite = ((!flag) ? this.m_keyboardBothSprite : this.m_restoreFullPadSprite);
			this.m_leftPad.sprite = ((!flag) ? this.m_keyboardLeftSprite : this.m_restoreLeftPadSprite);
			this.m_rightPad.sprite = ((!flag) ? this.m_keyboardRightSprite : this.m_restoreRightPadSprite);
			this.m_background.enabled = flag;
		}
		else
		{
			this.m_background.enabled = false;
			this.m_fullPad.gameObject.SetActive(true);
			this.m_fullPad.color = this.m_greyedOutColour;
		}
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x0011608E File Offset: 0x0011448E
	private Image GetImage(PadSide _side)
	{
		if (_side == PadSide.Both)
		{
			return this.m_fullPad;
		}
		if (_side == PadSide.Left)
		{
			return this.m_leftPad;
		}
		if (_side != PadSide.Right)
		{
			return null;
		}
		return this.m_rightPad;
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x001160BF File Offset: 0x001144BF
	private string Ellipsify(string _input)
	{
		if (_input.Length <= 16)
		{
			return _input;
		}
		return _input.Substring(0, 16) + "...";
	}

	// Token: 0x04002F40 RID: 12096
	[SerializeField]
	private EngagementSlot m_engagementSlot;

	// Token: 0x04002F41 RID: 12097
	[SerializeField]
	[AssignComponent(Editorbility.Editable)]
	private Image m_background;

	// Token: 0x04002F42 RID: 12098
	[SerializeField]
	[AssignChild("FullPad", Editorbility.NonEditable)]
	private PlatformDependentImage m_fullPad;

	// Token: 0x04002F43 RID: 12099
	[SerializeField]
	[AssignChild("LeftPad", Editorbility.NonEditable)]
	private PlatformDependentImage m_leftPad;

	// Token: 0x04002F44 RID: 12100
	[SerializeField]
	[AssignChild("RightPad", Editorbility.NonEditable)]
	private PlatformDependentImage m_rightPad;

	// Token: 0x04002F45 RID: 12101
	[SerializeField]
	[AssignChild("ProfileName", Editorbility.NonEditable)]
	private Text m_profileName;

	// Token: 0x04002F46 RID: 12102
	[SerializeField]
	[AssignResource("MainAvatarDirectory", Editorbility.NonEditable)]
	private AvatarDirectoryData m_avatarDirectory;

	// Token: 0x04002F47 RID: 12103
	[SerializeField]
	private Sprite m_keyboardBothSprite;

	// Token: 0x04002F48 RID: 12104
	[SerializeField]
	private Sprite m_keyboardLeftSprite;

	// Token: 0x04002F49 RID: 12105
	[SerializeField]
	private Sprite m_keyboardRightSprite;

	// Token: 0x04002F4A RID: 12106
	[SerializeField]
	private Sprite m_nxProControllerSprite;

	// Token: 0x04002F4B RID: 12107
	[SerializeField]
	private Color m_greyedOutColour = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04002F4C RID: 12108
	private Sprite m_restoreFullPadSprite;

	// Token: 0x04002F4D RID: 12109
	private Sprite m_restoreLeftPadSprite;

	// Token: 0x04002F4E RID: 12110
	private Sprite m_restoreRightPadSprite;

	// Token: 0x04002F4F RID: 12111
	private IPlayerManager m_iPlayerManager;
}
