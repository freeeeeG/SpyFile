using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using Team17.Online;

// Token: 0x0200072F RID: 1839
public class PCPlayerManager : PlayerManagerShared<PCPlayerManager.PCPlayerProfile>
{
	// Token: 0x0600230D RID: 8973 RVA: 0x000A87F9 File Offset: 0x000A6BF9
	protected override void Awake()
	{
		WindowsAccessibility.ToggleAccessibilityShortcutKeys(false);
		base.Awake();
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000A8807 File Offset: 0x000A6C07
	protected virtual void OnDestroy()
	{
		WindowsAccessibility.ToggleAccessibilityShortcutKeys(true);
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000A8810 File Offset: 0x000A6C10
	protected override bool CanEngage(EngagementSlot _e, ControlPadInput.PadNum _new, bool onlyAllowLostStickyProfiles)
	{
		if (PCPadInputProvider.IsKeyboard(_new) && !this.CanChangeSplitPads)
		{
			GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
			GameInputConfig.ConfigEntry[] array = baseInputConfig.m_playerConfigs.FindAll((GameInputConfig.ConfigEntry x) => x.Pad == (ControlPadInput.PadNum)_e);
			if (array.Length > 1)
			{
				return false;
			}
			if (array.Length == 1 && array[0].Side != PadSide.Both)
			{
				return false;
			}
		}
		return base.CanEngage(_e, _new, onlyAllowLostStickyProfiles);
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000A8890 File Offset: 0x000A6C90
	public override bool HasPlayer()
	{
		return base.IsEngaged(EngagementSlot.One);
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000A8899 File Offset: 0x000A6C99
	public override bool HasSavablePlayer()
	{
		return this.HasPlayer();
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000A88A4 File Offset: 0x000A6CA4
	protected void BootstrapAwake()
	{
		EngagementSlot engagementSlot = EngagementSlot.One;
		for (int i = 0; i < PlayerInputLookup.GetSystemControllerMaximum(); i++)
		{
			ControlPadInput.PadNum padNum = (ControlPadInput.PadNum)i;
			bool flag = Singleton<PCPadInputProvider>.Get().IsPadAttached(padNum);
			if ((engagementSlot == EngagementSlot.One || flag) && engagementSlot != EngagementSlot.Count)
			{
				this.EngagePadToSlot(padNum, engagementSlot);
				engagementSlot++;
			}
		}
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000A88F8 File Offset: 0x000A6CF8
	protected override IEnumerator PadEngageRoutine(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, EngagementSlot _intendedSlot, bool _forcePlayerChoice, VoidGeneric<GamepadUser> _finishedCallback)
	{
		PCPlayerManager.PCPlayerProfile param = this.EngagePadToSlot(_engagingPadNum, _intendedSlot);
		if (_finishedCallback != null)
		{
			_finishedCallback(param);
		}
		yield break;
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000A892C File Offset: 0x000A6D2C
	protected virtual PCPlayerManager.PCPlayerProfile EngagePadToSlot(ControlPadInput.PadNum _engagingPadNum, EngagementSlot _intendedSlot)
	{
		PlayerActionSet input = PCPadInputProvider.EngagePad(_engagingPadNum, (ControlPadInput.PadNum)_intendedSlot);
		PCPlayerManager.PCPlayerProfile pcplayerProfile = new PCPlayerManager.PCPlayerProfile(input);
		if (this.m_lostUsers.Count > 0 && this.m_lostUsers[0].UID != pcplayerProfile.UID)
		{
			PCPadInputProvider.DisengagePad((ControlPadInput.PadNum)_intendedSlot);
			return null;
		}
		pcplayerProfile.StickyEngagement = (_intendedSlot == EngagementSlot.One);
		base.AssignProfileToSlot(_intendedSlot, pcplayerProfile);
		return pcplayerProfile;
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000A8995 File Offset: 0x000A6D95
	protected override void OnPadDisengage(EngagementSlot _slot)
	{
		PCPadInputProvider.DisengagePad((ControlPadInput.PadNum)_slot);
		base.OnPadDisengage(_slot);
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000A89A4 File Offset: 0x000A6DA4
	public override void ShowGamerCard(GamepadUser localUser)
	{
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000A89A6 File Offset: 0x000A6DA6
	public override void ShowGamerCard(OnlineUserPlatformId onlineUser)
	{
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x000A89A8 File Offset: 0x000A6DA8
	protected override void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			EngagementSlot engagementSlot = (EngagementSlot)i;
			if (base.IsEngaged(engagementSlot))
			{
				ControlPadInput.PadNum pad = (ControlPadInput.PadNum)i;
				if (!PlayerInputLookup.IsPadAttached(pad))
				{
					base.DisengageSlots(new List<EngagementSlot>(new EngagementSlot[]
					{
						engagementSlot
					}));
				}
			}
		}
		base.Update();
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000A89FD File Offset: 0x000A6DFD
	public override bool SupportsInvitesForAnyUser()
	{
		return false;
	}

	// Token: 0x02000730 RID: 1840
	public class PCPlayerProfile : GamepadUser
	{
		// Token: 0x0600231A RID: 8986 RVA: 0x000A8A00 File Offset: 0x000A6E00
		public PCPlayerProfile(PlayerActionSet _input)
		{
			this.m_actionSet = _input;
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x000A8A0F File Offset: 0x000A6E0F
		public override string UID
		{
			get
			{
				return (this.m_actionSet.Device == null) ? "Keyboard" : this.m_actionSet.Device.Meta;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000A8A3B File Offset: 0x000A6E3B
		public override GamepadUser.ControlTypeEnum ControlType
		{
			get
			{
				if (this.m_actionSet != null && this.m_actionSet.Device == null)
				{
					return GamepadUser.ControlTypeEnum.Keyboard;
				}
				return GamepadUser.ControlTypeEnum.Pad;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x000A8A5C File Offset: 0x000A6E5C
		public override string DisplayName
		{
			get
			{
				if (this.m_actionSet == null)
				{
					return string.Empty;
				}
				if (this.m_actionSet.Device != null)
				{
					return Localization.Get("StartScreen.Engagement.ControllerDevice", new LocToken[0]);
				}
				return Localization.Get("StartScreen.Engagement.KeyboardDevice", new LocToken[0]);
			}
		}

		// Token: 0x04001ACA RID: 6858
		private PlayerActionSet m_actionSet;
	}
}
