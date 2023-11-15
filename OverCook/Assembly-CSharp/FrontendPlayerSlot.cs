using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000ABD RID: 2749
public class FrontendPlayerSlot : MonoBehaviour
{
	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06003719 RID: 14105 RVA: 0x001032D8 File Offset: 0x001016D8
	public GamepadUser CurrentGamepadUser
	{
		get
		{
			return this.m_CurrentgamepadUser;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600371A RID: 14106 RVA: 0x001032E0 File Offset: 0x001016E0
	public PadSide CurrentPadSide
	{
		get
		{
			return this.m_CurrentpadSide;
		}
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x001032E8 File Offset: 0x001016E8
	private void Awake()
	{
		if (this.m_SlotButton == null)
		{
			this.m_SlotButton = base.GetComponent<T17Button>();
		}
		if (this.m_ControllerTypeImage == null)
		{
		}
		if (this.m_SlotButtonImage == null && this.m_SlotButton != null)
		{
			this.m_SlotButtonImage = this.m_SlotButton.GetComponent<T17Image>();
		}
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		if (null != this.m_GamerpicImage && null != this.m_GamerpicImage.sprite)
		{
			this.m_gamerPicDefaultSprite = this.m_GamerpicImage.sprite;
		}
		this.m_isInitialised = true;
		if (PlayerInputLookup.IsAwake())
		{
			this.RefreshCosmetics();
		}
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x00103407 File Offset: 0x00101807
	private void OnDestroy()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x00103440 File Offset: 0x00101840
	private void OnEngagementChanged(EngagementSlot _param1, GamepadUser _param2, GamepadUser _param3)
	{
		if (PlayerInputLookup.IsAwake())
		{
			this.RefreshCosmetics();
		}
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x00103452 File Offset: 0x00101852
	public void OnUsersChanged()
	{
		this.RefreshCosmetics();
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x0010345A File Offset: 0x0010185A
	public void OnConnectionModeUpdated()
	{
		this.RefreshCosmetics();
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x00103462 File Offset: 0x00101862
	public void SelectSlot()
	{
		this.m_isSelected = true;
		this.RefreshCosmetics();
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x00103471 File Offset: 0x00101871
	public void DeselectSlot()
	{
		this.m_isSelected = false;
		this.RefreshCosmetics();
	}

	// Token: 0x06003722 RID: 14114 RVA: 0x00103480 File Offset: 0x00101880
	private void RefreshCosmetics()
	{
		if (!this.m_isInitialised)
		{
			return;
		}
		this.m_userIndex = (int)this.m_EngagementSlot;
		bool flag = !ConnectionStatus.IsInSession() || ConnectionStatus.IsHost();
		FastList<User> users = ClientUserSystem.m_Users;
		if (this.m_userIndex < users.Count)
		{
			this.SetSlotEnabledForUser(users._items[this.m_userIndex]);
		}
		else if (this.m_userIndex == users.Count && flag)
		{
			this.SetSlotEnabledForAdd();
		}
		else
		{
			this.SetSlotDisabled();
		}
	}

	// Token: 0x06003723 RID: 14115 RVA: 0x00103510 File Offset: 0x00101910
	private void SetSlotDisabled()
	{
		this.m_CurrentpadSide = PadSide.Both;
		this.m_CurrentgamepadUser = null;
		if (this.m_AddPlayerImage != null)
		{
			this.m_AddPlayerImage.gameObject.SetActive(false);
		}
		if (this.m_SlotButton != null)
		{
			this.m_SlotButton.interactable = false;
		}
		if (ConnectionStatus.IsHost())
		{
			if (this.m_SlotDisabledHosting != null)
			{
				this.m_SlotDisabledHosting.SetActive(true);
			}
			if (this.m_SlotDisabledImage != null)
			{
				this.m_SlotDisabledImage.gameObject.SetActive(false);
			}
		}
		else
		{
			if (this.m_SlotDisabledImage != null)
			{
				this.m_SlotDisabledImage.gameObject.SetActive(true);
			}
			if (this.m_SlotDisabledHosting != null)
			{
				this.m_SlotDisabledHosting.SetActive(false);
			}
		}
		this.SetUIColourForUser(null);
		this.SetControllerIconForUser(null);
		this.SetGamerpicForUser(null, false);
		this.SetSlotSelectedHighlight(false, false);
		this.SetNameTextForUser(null);
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x0010361C File Offset: 0x00101A1C
	private void SetSlotEnabledForAdd()
	{
		this.m_CurrentpadSide = PadSide.Both;
		this.m_CurrentgamepadUser = null;
		if (this.m_AddPlayerImage != null)
		{
			this.m_AddPlayerImage.gameObject.SetActive(true);
		}
		if (this.m_SlotDisabledImage != null)
		{
			this.m_SlotDisabledImage.gameObject.SetActive(false);
		}
		if (this.m_SlotDisabledHosting != null)
		{
			this.m_SlotDisabledHosting.SetActive(false);
		}
		if (this.m_SlotButton != null)
		{
			this.m_SlotButton.interactable = true;
			this.m_SlotButton.image = this.m_AddPlayerImage;
			this.m_SlotButton.m_bShowTooltip = true;
			this.m_SlotButton.m_bLocalizeTooltip = true;
			this.m_SlotButton.m_TooltipTag = this.m_AddPlayerTooltip;
		}
		this.SetUIColourForUser(null);
		this.SetControllerIconForUser(null);
		this.SetGamerpicForUser(null, false);
		this.SetSlotSelectedHighlight(true, false);
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x00103710 File Offset: 0x00101B10
	private void SetSlotEnabledForUser(User user)
	{
		if (this.m_SlotDisabledImage != null)
		{
			this.m_SlotDisabledImage.gameObject.SetActive(false);
		}
		if (this.m_SlotDisabledHosting != null)
		{
			this.m_SlotDisabledHosting.SetActive(false);
		}
		if (this.m_AddPlayerImage != null)
		{
			this.m_AddPlayerImage.gameObject.SetActive(false);
		}
		if (this.m_SlotButton != null)
		{
			this.m_SlotButton.interactable = true;
			this.m_SlotButton.image = this.m_frameImage;
			this.m_SlotButton.m_bShowTooltip = false;
		}
		if (user == null)
		{
			return;
		}
		if (this.m_SlotButton != null)
		{
			this.m_SlotButton.m_bShowTooltip = true;
			this.m_SlotButton.m_bLocalizeTooltip = true;
			if (user.IsLocal)
			{
				if (user.Engagement == EngagementSlot.One && user.Split != User.SplitStatus.SplitPadGuest)
				{
					this.m_SlotButton.m_TooltipTag = this.m_HostTooltip;
				}
				else
				{
					this.m_SlotButton.m_TooltipTag = this.m_LocalTooltip;
				}
			}
			else if (user.Engagement == EngagementSlot.One)
			{
				this.m_SlotButton.m_TooltipTag = this.m_HostTooltip;
			}
			else
			{
				this.m_SlotButton.m_TooltipTag = this.m_ClientTooltip;
			}
		}
		if (this.m_IPlayerManager != null)
		{
			this.m_CurrentgamepadUser = this.m_IPlayerManager.GetUser(user.Engagement);
		}
		this.SetUIColourForUser(user);
		this.SetControllerIconForUser(user);
		this.SetGamerpicForUser(user, false);
		this.SetSlotSelectedHighlight(true, true);
		this.SetNameTextForUser(user);
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x001038AF File Offset: 0x00101CAF
	private string Ellipsify(string _input)
	{
		if (_input.Length <= 16)
		{
			return _input;
		}
		return _input.Substring(0, 16) + "...";
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x001038D4 File Offset: 0x00101CD4
	private void SetUIColourForUser(User user)
	{
		Color color = this.m_DisabledSlotColour;
		Color lhs = this.m_GreyedOutColour;
		if (user != null && user.Colour != 7U && user.Colour < (uint)this.m_AvatarDirectory.Colours.Length)
		{
			ChefColourData chefColourData = this.m_AvatarDirectory.Colours[(int)user.Colour];
			color = chefColourData.PadUIColour;
			lhs = chefColourData.PadBarColour;
		}
		if (this.m_ControllerTypeImage != null)
		{
			this.m_ControllerTypeImage.color = color;
		}
		if (this.m_SlotButtonImage != null)
		{
			this.m_SlotButtonImage.color = color;
		}
		if (this.m_userNameText != null)
		{
			this.m_userNameText.color = color;
		}
		if (lhs == this.m_GreyedOutColour)
		{
		}
	}

	// Token: 0x06003728 RID: 14120 RVA: 0x001039A4 File Offset: 0x00101DA4
	private void SetControllerIconForUser(User user)
	{
		if (this.m_ControllerTypeImage == null)
		{
			return;
		}
		if (user == null)
		{
			this.m_ControllerTypeImage.gameObject.SetActive(false);
			return;
		}
		if (!user.IsLocal)
		{
			if (this.m_remotePlayerIcon != null)
			{
				this.m_ControllerTypeImage.gameObject.SetActive(true);
				this.m_ControllerTypeImage.sprite = this.m_remotePlayerIcon;
			}
			return;
		}
		if (this.m_CurrentgamepadUser != null)
		{
			this.m_ControllerTypeImage.gameObject.SetActive(true);
			this.m_CurrentpadSide = user.PadSide;
			Sprite image = this.m_ControllerSprites.GetImage(this.m_CurrentpadSide, this.m_CurrentgamepadUser.ControlType);
			this.m_ControllerTypeImage.sprite = image;
		}
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x00103A74 File Offset: 0x00101E74
	private void SetGamerpicForUser(User user, bool _force = false)
	{
		if (null == this.m_GamerpicImage)
		{
			return;
		}
		if (user != null)
		{
			Vector3 localScale = new Vector3(1f, 1f, 1f);
			Texture2D avatarImage = ClientUserSystem.GetAvatarImage(user);
			if (null == avatarImage)
			{
				this.m_GamerpicImage.sprite = this.m_gamerPicDefaultSprite;
			}
			else if (_force || avatarImage != this.m_lastAvatarImage)
			{
				this.m_GamerpicImage.sprite = Sprite.Create(avatarImage, new Rect(0f, 0f, (float)avatarImage.width, (float)avatarImage.height), new Vector2(0f, 0f), 100f, 1U, SpriteMeshType.FullRect);
			}
			this.m_lastAvatarImage = avatarImage;
			localScale.y = -1f;
			this.m_GamerpicImage.gameObject.transform.localScale = localScale;
			this.m_GamerpicImage.gameObject.SetActive(true);
		}
		else
		{
			this.m_GamerpicImage.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x00103B84 File Offset: 0x00101F84
	private void SetSlotSelectedHighlight(bool slotEnabled, bool slotHasUser)
	{
		bool flag = slotEnabled && slotHasUser;
		bool flag2 = slotEnabled && !slotHasUser;
		if (this.m_frameImage != null)
		{
			this.m_frameImage.gameObject.SetActive(flag && !this.m_isSelected);
		}
		if (this.m_selectedFrameImage != null)
		{
			this.m_selectedFrameImage.gameObject.SetActive(flag && this.m_isSelected);
		}
		if (this.m_AddPlayerImage != null)
		{
			this.m_AddPlayerImage.gameObject.SetActive(flag2 && !this.m_isSelected);
		}
		if (this.m_selectedAddPlayerImage != null)
		{
			this.m_selectedAddPlayerImage.gameObject.SetActive(flag2 && this.m_isSelected);
		}
	}

	// Token: 0x0600372B RID: 14123 RVA: 0x00103C70 File Offset: 0x00102070
	private void SetNameTextForUser(User user)
	{
		if (this.m_userNameText == null)
		{
			return;
		}
		this.m_userNameText.gameObject.SetActive(user != null);
		if (user != null)
		{
			this.m_userNameText.SetNonLocalizedText(user.DisplayName);
		}
	}

	// Token: 0x04002C36 RID: 11318
	public EngagementSlot m_EngagementSlot;

	// Token: 0x04002C37 RID: 11319
	public Color m_DisabledSlotColour = Color.gray;

	// Token: 0x04002C38 RID: 11320
	public T17Image m_SlotDisabledImage;

	// Token: 0x04002C39 RID: 11321
	public GameObject m_SlotDisabledHosting;

	// Token: 0x04002C3A RID: 11322
	public T17Image m_AddPlayerImage;

	// Token: 0x04002C3B RID: 11323
	public T17Image m_selectedAddPlayerImage;

	// Token: 0x04002C3C RID: 11324
	public T17Button m_SlotButton;

	// Token: 0x04002C3D RID: 11325
	public T17Image m_ControllerTypeImage;

	// Token: 0x04002C3E RID: 11326
	public T17Image m_GamerpicImage;

	// Token: 0x04002C3F RID: 11327
	public T17Image m_frameImage;

	// Token: 0x04002C40 RID: 11328
	public T17Image m_selectedFrameImage;

	// Token: 0x04002C41 RID: 11329
	public Sprite m_remotePlayerIcon;

	// Token: 0x04002C42 RID: 11330
	public T17Text m_userNameText;

	// Token: 0x04002C43 RID: 11331
	[SerializeField]
	private string m_HostTooltip;

	// Token: 0x04002C44 RID: 11332
	[SerializeField]
	private string m_ClientTooltip;

	// Token: 0x04002C45 RID: 11333
	[SerializeField]
	private string m_LocalTooltip;

	// Token: 0x04002C46 RID: 11334
	[SerializeField]
	private string m_AddPlayerTooltip;

	// Token: 0x04002C47 RID: 11335
	private bool m_isSelected;

	// Token: 0x04002C48 RID: 11336
	private bool m_isInitialised;

	// Token: 0x04002C49 RID: 11337
	private Texture2D m_lastAvatarImage;

	// Token: 0x04002C4A RID: 11338
	private GamepadUser m_CurrentgamepadUser;

	// Token: 0x04002C4B RID: 11339
	private PadSide m_CurrentpadSide = PadSide.Both;

	// Token: 0x04002C4C RID: 11340
	[AssignResource("Frontend_ControllerTypeSprites", Editorbility.NonEditable)]
	public ControllerTypeSprites m_ControllerSprites;

	// Token: 0x04002C4D RID: 11341
	[AssignResource("MainAvatarDirectory", Editorbility.NonEditable)]
	public AvatarDirectoryData m_AvatarDirectory;

	// Token: 0x04002C4E RID: 11342
	private T17Image m_SlotButtonImage;

	// Token: 0x04002C4F RID: 11343
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04002C50 RID: 11344
	private Color m_GreyedOutColour = new Color(0.5f, 0.5f, 0.5f);

	// Token: 0x04002C51 RID: 11345
	private int m_userIndex;

	// Token: 0x04002C52 RID: 11346
	private Sprite m_gamerPicDefaultSprite;
}
