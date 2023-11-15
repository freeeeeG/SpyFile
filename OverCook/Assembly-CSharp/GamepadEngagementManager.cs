using System;
using InControl;
using Team17.Online;
using UnityEngine;

// Token: 0x020006F8 RID: 1784
[ExecutionDependency(typeof(InControlManager))]
public class GamepadEngagementManager : Manager
{
	// Token: 0x060021CD RID: 8653 RVA: 0x000A343B File Offset: 0x000A183B
	public void SetCanDisconnect(EngagementSlot _slot, bool _canDisconnect)
	{
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x000A3440 File Offset: 0x000A1840
	private void Awake()
	{
		this.m_iPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_iPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		this.m_hasProfile = this.m_iPlayerManager.HasPlayer();
		PlayerInputLookup.OnRegenerateControls = (CallbackVoid)Delegate.Combine(PlayerInputLookup.OnRegenerateControls, new CallbackVoid(this.GatherButtons));
		if (PlayerInputLookup.IsAwake())
		{
			this.GatherButtons();
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x060021CF RID: 8655 RVA: 0x000A34B0 File Offset: 0x000A18B0
	// (set) Token: 0x060021D0 RID: 8656 RVA: 0x000A34B8 File Offset: 0x000A18B8
	public bool CanManuallyChangeEngagement
	{
		get
		{
			return this.m_canManuallyChangeEngagement;
		}
		set
		{
			this.m_canManuallyChangeEngagement = value;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000A34C1 File Offset: 0x000A18C1
	public SuppressionController Suppressor
	{
		get
		{
			return this.m_manualEngagementSuppressor;
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000A34CC File Offset: 0x000A18CC
	private void GatherButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			PlayerGameInput playerGameInput = new PlayerGameInput((ControlPadInput.PadNum)i, PadSide.Both, this.m_unsidedAmbiMapping);
			this.m_cancelButtons[i] = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UICancel, playerGameInput);
		}
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000A3509 File Offset: 0x000A1909
	private void OnDestroy()
	{
		this.m_iPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000A3524 File Offset: 0x000A1924
	private void OnEngagementChanged(EngagementSlot _e, GamepadUser _prev, GamepadUser _new)
	{
		if (_e == EngagementSlot.One)
		{
			if (_prev != null && _new == null)
			{
				this.m_hasProfile = false;
			}
			if (_new != null && _prev == null)
			{
				this.m_hasProfile = true;
			}
		}
		PlayerGameInput playerGameInput = new PlayerGameInput((ControlPadInput.PadNum)_e, PadSide.Both, this.m_unsidedAmbiMapping);
		this.m_cancelButtons[(int)_e] = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UICancel, playerGameInput);
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000A3594 File Offset: 0x000A1994
	private void Update()
	{
		if (this.m_hasProfile && !this.m_iPlayerManager.IsBusy() && !T17DialogBoxManager.HasAnyOpenDialogs() && this.m_engagementCooldownTimer <= 0f && ClientGameSetup.Mode == GameMode.OnlineKitchen && this.m_canManuallyChangeEngagement && !this.m_manualEngagementSuppressor.IsSuppressed())
		{
			if (ClientUserSystem.m_Users.Count < 4)
			{
				EngagmentCircumstances circumstances = null;
				ControlPadInput.PadNum engagementPad = this.m_iPlayerManager.GetEngagementPad(out circumstances);
				if (engagementPad != ControlPadInput.PadNum.Count && engagementPad != ControlPadInput.PadNum.One && this.m_iPlayerManager.HasFreeEngagementSlot())
				{
					this.m_iPlayerManager.StartPadEngagement(engagementPad, circumstances, null);
					this.m_engagementCooldownTimer = this.kEngagementCooldownTime;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.m_cancelButtons[i].JustPressed())
				{
					GamepadUser user = this.m_iPlayerManager.GetUser((EngagementSlot)i);
					if (user != null && !user.StickyEngagement)
					{
						this.m_iPlayerManager.DisengagePad((EngagementSlot)i);
						this.m_engagementCooldownTimer = this.kEngagementCooldownTime;
					}
				}
			}
		}
		if (this.m_engagementCooldownTimer > 0f)
		{
			this.m_engagementCooldownTimer -= Time.deltaTime;
		}
		if (this.m_manualEngagementSuppressor != null)
		{
			this.m_manualEngagementSuppressor.UpdateSuppressors();
		}
	}

	// Token: 0x04001A11 RID: 6673
	[SerializeField]
	[AssignResource("SidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_sidedAmbiMapping;

	// Token: 0x04001A12 RID: 6674
	[SerializeField]
	[AssignResource("UnsidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_unsidedAmbiMapping;

	// Token: 0x04001A13 RID: 6675
	private IPlayerManager m_iPlayerManager;

	// Token: 0x04001A14 RID: 6676
	private ILogicalButton[] m_cancelButtons = new ILogicalButton[4];

	// Token: 0x04001A15 RID: 6677
	private bool m_hasProfile;

	// Token: 0x04001A16 RID: 6678
	private readonly float kEngagementCooldownTime = 0.5f;

	// Token: 0x04001A17 RID: 6679
	private float m_engagementCooldownTimer;

	// Token: 0x04001A18 RID: 6680
	private bool m_canManuallyChangeEngagement;

	// Token: 0x04001A19 RID: 6681
	private SuppressionController m_manualEngagementSuppressor = new SuppressionController();
}
