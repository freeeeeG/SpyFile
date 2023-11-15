using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000734 RID: 1844
public abstract class PlayerManagerShared<GamepadUserType> : Manager, IPlayerManager where GamepadUserType : GamepadUser
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06002330 RID: 9008 RVA: 0x000A7BE0 File Offset: 0x000A5FE0
	protected GameObject Dimmer
	{
		get
		{
			return this.m_dimmer;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06002331 RID: 9009 RVA: 0x000A7BE8 File Offset: 0x000A5FE8
	protected GameObject Rejoin
	{
		get
		{
			return this.m_rejoin;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06002332 RID: 9010 RVA: 0x000A7BF0 File Offset: 0x000A5FF0
	public AmbiControlsMappingData SidedAmbiMapping
	{
		get
		{
			return this.m_sidedAmbiMapping;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06002333 RID: 9011 RVA: 0x000A7BF8 File Offset: 0x000A5FF8
	public AmbiControlsMappingData UnsidedAmbiMapping
	{
		get
		{
			return this.m_unsidedAmbiMapping;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06002334 RID: 9012 RVA: 0x000A7C00 File Offset: 0x000A6000
	public static bool AcceptAndCancelButtonsInverted
	{
		get
		{
			return PlayerManagerShared<GamepadUserType>.c_AcceptInverted;
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000A7C08 File Offset: 0x000A6008
	private static void _AOT()
	{
		new EngagementSlot[0].Stringify<EngagementSlot>();
		ArrayUtils.AssembleString<EngagementSlot>(EngagementSlot.One, string.Empty);
		EngagementSlot[] array = new EngagementSlot[0];
		if (PlayerManagerShared<GamepadUserType>.<>f__mg$cache0 == null)
		{
			PlayerManagerShared<GamepadUserType>.<>f__mg$cache0 = new Generic<string, EngagementSlot, string>(ArrayUtils.AssembleString<EngagementSlot>);
		}
		array.Collapse(PlayerManagerShared<GamepadUserType>.<>f__mg$cache0);
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000A7C58 File Offset: 0x000A6058
	public virtual GamepadUser GetUser(EngagementSlot _slot)
	{
		GamepadUserType gamepadUserType = (GamepadUserType)((object)null);
		this.m_engagedPads.TryGetValue(_slot, out gamepadUserType);
		return gamepadUserType;
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000A7C81 File Offset: 0x000A6081
	public GamepadUser GetCachedPrimaryUser()
	{
		return this.m_cachedPrimaryUser;
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000A7C90 File Offset: 0x000A6090
	protected void ClearEngagement()
	{
		Dictionary<EngagementSlot, GamepadUserType> engagedPads = this.m_engagedPads;
		this.m_engagedPads = new Dictionary<EngagementSlot, GamepadUserType>();
		this.m_lostUsers.Clear();
		ServerUserSystem.UnlockEngagement();
		foreach (KeyValuePair<EngagementSlot, GamepadUserType> keyValuePair in engagedPads)
		{
			this.CallEngagementChangeCallback(keyValuePair.Key, keyValuePair.Value, null);
		}
		FastList<User> users = ServerUserSystem.m_Users;
		User.MachineID s_LocalMachineId = ServerUserSystem.s_LocalMachineId;
		User user = UserSystemUtils.FindUser(users, null, s_LocalMachineId, EngagementSlot.One, TeamID.Count, User.SplitStatus.Count);
		if (user != null)
		{
			ServerUserSystem.RemoveUser(user, true);
		}
		this.m_engagementRoutines.Clear();
		this.FinishLostPadEngagement();
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000A7D58 File Offset: 0x000A6158
	public virtual ControlPadInput.PadNum GetEngagementPad(out EngagmentCircumstances o_engagment)
	{
		o_engagment = null;
		ControlPadInput.PadNum result = ControlPadInput.PadNum.Count;
		foreach (KeyValuePair<ControlPadInput.PadNum, ILogicalButton> keyValuePair in this.m_engagementButtons)
		{
			if (keyValuePair.Value.JustPressed() && !this.m_engagedPads.ContainsKey((EngagementSlot)keyValuePair.Key))
			{
				result = keyValuePair.Key;
			}
		}
		return result;
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000A7DE4 File Offset: 0x000A61E4
	protected bool IsEngaged(EngagementSlot _slot)
	{
		return this.m_engagedPads.ContainsKey(_slot);
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000A7DF2 File Offset: 0x000A61F2
	public virtual bool IsBusy()
	{
		return this.m_engagementRoutines.Count > 0;
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000A7E02 File Offset: 0x000A6202
	public virtual bool IsWarningActive(PlayerWarning warning)
	{
		return warning == PlayerWarning.Disengaged && this.Rejoin.activeInHierarchy;
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000A7E20 File Offset: 0x000A6220
	public virtual bool IsEngagingSlot(EngagementSlot slot)
	{
		PlayerManagerShared<GamepadUserType>.EngagementRoutine engagementRoutine = this.m_engagementRoutines.Find((PlayerManagerShared<GamepadUserType>.EngagementRoutine x) => x.Slot == slot);
		return engagementRoutine != null;
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000A7E59 File Offset: 0x000A6259
	protected virtual void Awake()
	{
		this.m_dimmer.SetActive(false);
		this.m_rejoin.SetActive(false);
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000A7E74 File Offset: 0x000A6274
	protected virtual void Start()
	{
		for (int i = 0; i < PlayerInputLookup.GetSystemControllerMaximum(); i++)
		{
			ControlPadInput.PadNum pad = (ControlPadInput.PadNum)i;
			PlayerGameInput playerGameInput = new PlayerGameInput(pad, PadSide.Both, this.UnsidedAmbiMapping);
			ILogicalButton engagementButton = PlayerInputLookup.GetEngagementButton(playerGameInput);
			this.m_engagementButtons.Add((ControlPadInput.PadNum)i, engagementButton);
		}
	}

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06002340 RID: 9024 RVA: 0x000A7EBC File Offset: 0x000A62BC
	// (remove) Token: 0x06002341 RID: 9025 RVA: 0x000A7EF4 File Offset: 0x000A62F4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event VoidGeneric<EngagementSlot, GamepadUser, GamepadUser> m_engagementCallback = delegate(EngagementSlot _slot, GamepadUser _userBefore, GamepadUser _userNow)
	{
	};

	// Token: 0x06002342 RID: 9026 RVA: 0x000A7F2A File Offset: 0x000A632A
	protected void CallEngagementChangeCallback(EngagementSlot _s, GamepadUser _p, GamepadUser _n)
	{
		this.m_engagementCallback(_s, _p, _n);
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000A7F3A File Offset: 0x000A633A
	protected virtual bool CanEngage(EngagementSlot _e, ControlPadInput.PadNum _new, bool onlyAllowLostStickyProfiles)
	{
		return true;
	}

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06002344 RID: 9028 RVA: 0x000A7F3D File Offset: 0x000A633D
	// (remove) Token: 0x06002345 RID: 9029 RVA: 0x000A7F46 File Offset: 0x000A6346
	public event VoidGeneric<EngagementSlot, GamepadUser, GamepadUser> EngagementChangeCallback
	{
		add
		{
			this.m_engagementCallback += value;
		}
		remove
		{
			this.m_engagementCallback -= value;
		}
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000A7F4F File Offset: 0x000A634F
	protected void ClearRoutines()
	{
		this.m_engagementRoutines.Clear();
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000A7F5C File Offset: 0x000A635C
	protected void AddRoutine(string _routineName, EngagementSlot _slot, IEnumerator _routine)
	{
		this.m_engagementRoutines.Add(new PlayerManagerShared<GamepadUserType>.EngagementRoutine(_slot, _routine));
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000A7F70 File Offset: 0x000A6370
	protected virtual void Update()
	{
		while (this.m_engagementRoutines.Count > 0 && !this.m_engagementRoutines[0].Routine.MoveNext())
		{
			this.m_engagementRoutines.RemoveAt(0);
		}
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000A7FB0 File Offset: 0x000A63B0
	protected void AssignProfileToSlot(EngagementSlot _slot, GamepadUserType _newUser)
	{
		GamepadUserType gamepadUserType = this.m_engagedPads.SafeGet(_slot, (GamepadUserType)((object)null));
		this.m_engagedPads.SafeAdd(_slot, _newUser);
		if (this.m_lostUsers.Contains(_newUser))
		{
			this.m_lostUsers.Remove(_newUser);
		}
		if (_slot == EngagementSlot.One)
		{
			this.m_cachedPrimaryUser = _newUser;
		}
		this.CallEngagementChangeCallback(_slot, gamepadUserType, _newUser);
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000A801C File Offset: 0x000A641C
	protected void RemoveProfileToSlot(EngagementSlot _slot)
	{
		this.OnPadDisengage(_slot);
		GamepadUserType gamepadUserType = this.m_engagedPads[_slot];
		this.m_engagedPads.Remove(_slot);
		this.CallEngagementChangeCallback(_slot, gamepadUserType, null);
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000A8058 File Offset: 0x000A6458
	protected virtual void OnPadDisengage(EngagementSlot _slot)
	{
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000A805C File Offset: 0x000A645C
	protected void DisengageSlots(List<EngagementSlot> _slots)
	{
		for (int i = 0; i < _slots.Count; i++)
		{
			EngagementSlot engagementSlot = _slots[i];
			GamepadUserType gamepadUserType = this.m_engagedPads[engagementSlot];
			this.m_engagedPads.Remove(engagementSlot);
			this.CallEngagementChangeCallback(engagementSlot, gamepadUserType, null);
			if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
			{
				bool engagementsLocked = ServerUserSystem.EngagementsLocked;
			}
			if (gamepadUserType.StickyEngagement)
			{
				if (!this.m_lostUsers.Contains(gamepadUserType))
				{
					this.m_lostUsers.Add(gamepadUserType);
				}
				this.AddRoutine("LostEngagedPlayerRoutine", engagementSlot, this.LostEngagedPlayerRoutine(engagementSlot));
			}
		}
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000A810C File Offset: 0x000A650C
	protected EngagementSlot GetUnengagedSlot()
	{
		for (int i = 0; i < 4; i++)
		{
			EngagementSlot engagementSlot = (EngagementSlot)i;
			if (!this.IsEngaged(engagementSlot))
			{
				return engagementSlot;
			}
		}
		return EngagementSlot.Count;
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000A813C File Offset: 0x000A653C
	public virtual bool HasFreeEngagementSlot()
	{
		return this.GetUnengagedSlot() != EngagementSlot.Count;
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000A814C File Offset: 0x000A654C
	public virtual IEnumerator RunGameownerEngagement(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances)
	{
		bool itsRunnedMate = false;
		VoidGeneric<GamepadUser> finishedCall = delegate(GamepadUser _user)
		{
			itsRunnedMate = true;
		};
		this.StartGameownerEngagement(_engagingPadNum, _circumstances, finishedCall);
		while (!itsRunnedMate)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x000A8175 File Offset: 0x000A6575
	public virtual void StartGameownerEngagement(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, VoidGeneric<GamepadUser> _finishedCall)
	{
		this.StartPadEngagement(_engagingPadNum, _circumstances, _finishedCall);
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x000A8180 File Offset: 0x000A6580
	public virtual void StartPadEngagement(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, VoidGeneric<GamepadUser> _finishedCall)
	{
		EngagementSlot unengagedSlot = this.GetUnengagedSlot();
		this.AddRoutine("PadEngageRoutine", unengagedSlot, this.PadEngageRoutine(_engagingPadNum, _circumstances, unengagedSlot, _finishedCall));
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x000A81AC File Offset: 0x000A65AC
	protected IEnumerator LostEngagedPlayerRoutine(EngagementSlot _lostPad)
	{
		if (this.IsEngaged(_lostPad))
		{
			yield break;
		}
		yield return null;
		if (this.m_rejoinGamerNameText != null)
		{
			if (this.m_lostUsers.Count > 0)
			{
				GamepadUserType gamepadUserType = this.m_lostUsers[0];
				if (gamepadUserType.DisplayName.Length <= 20)
				{
					Text rejoinGamerNameText = this.m_rejoinGamerNameText;
					GamepadUserType gamepadUserType2 = this.m_lostUsers[0];
					rejoinGamerNameText.text = gamepadUserType2.DisplayName;
				}
				else
				{
					GamepadUserType gamepadUserType3 = this.m_lostUsers[0];
					string text = gamepadUserType3.DisplayName.Substring(0, 20) + "…";
					this.m_rejoinGamerNameText.text = text;
				}
			}
			else
			{
				this.m_rejoinGamerNameText.text = string.Empty;
			}
		}
		this.Dimmer.SetActive(true);
		this.Rejoin.SetActive(true);
		TimeManager timeManager = GameUtils.RequestManager<TimeManager>();
		this.m_lostEngagedPauseLayer = new TimeManager.PauseLayer?((!ConnectionStatus.IsInSession()) ? TimeManager.PauseLayer.System : TimeManager.PauseLayer.Network);
		if (timeManager != null)
		{
			timeManager.SetPaused(this.m_lostEngagedPauseLayer.Value, true, this);
		}
		T17EventSystem eventSystem = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
		if (eventSystem != null)
		{
			this.m_lostEngagedSuppressor = eventSystem.Disable(this);
		}
		T17EventSystemsManager.Instance.DisableAllEventSystemsExceptFor(null);
		GamepadUser engagedUser = null;
		while (engagedUser == null)
		{
			ControlPadInput.PadNum engagingPad = ControlPadInput.PadNum.Count;
			EngagmentCircumstances circumstances = null;
			while (engagingPad == ControlPadInput.PadNum.Count)
			{
				yield return null;
				if (this.GetUser(_lostPad) != null)
				{
					break;
				}
				engagingPad = this.GetEngagementPad(out circumstances);
				if (engagingPad != ControlPadInput.PadNum.Count && !this.CanEngage(_lostPad, engagingPad, true))
				{
					engagingPad = ControlPadInput.PadNum.Count;
				}
			}
			if (this.m_engagementButtons.ContainsKey(engagingPad))
			{
				while (this.m_engagementButtons[engagingPad].IsDown())
				{
					yield return null;
				}
			}
			IEnumerator login = this.PadEngageRoutine(engagingPad, circumstances, _lostPad, false, null);
			while (login.MoveNext())
			{
				yield return null;
			}
			engagedUser = this.GetUser(_lostPad);
		}
		this.FinishLostPadEngagement();
		yield break;
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000A81D0 File Offset: 0x000A65D0
	private void FinishLostPadEngagement()
	{
		T17EventSystemsManager.Instance.EnableAllEventSystems();
		if (this.m_lostEngagedSuppressor != null)
		{
			this.m_lostEngagedSuppressor.Release();
		}
		TimeManager timeManager = GameUtils.RequestManager<TimeManager>();
		if (timeManager != null && this.m_lostEngagedPauseLayer != null)
		{
			if (!TimeManager.IsPaused(this.m_lostEngagedPauseLayer.Value))
			{
				timeManager.SetPaused(this.m_lostEngagedPauseLayer.Value, true, this);
			}
			timeManager.SetPaused(this.m_lostEngagedPauseLayer.Value, false, this);
		}
		this.m_lostEngagedPauseLayer = null;
		this.Dimmer.SetActive(false);
		this.Rejoin.SetActive(false);
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000A8281 File Offset: 0x000A6681
	public virtual void DisengagePad(EngagementSlot _intendedSlot)
	{
		if (this.IsEngaged(_intendedSlot))
		{
			this.RemoveProfileToSlot(_intendedSlot);
		}
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000A8296 File Offset: 0x000A6696
	protected virtual IEnumerator PadEngageRoutine(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, EngagementSlot _intendedSlot, VoidGeneric<GamepadUser> _finishedCallback)
	{
		return this.PadEngageRoutine(_engagingPadNum, _circumstances, _intendedSlot, false, _finishedCallback);
	}

	// Token: 0x06002356 RID: 9046
	protected abstract IEnumerator PadEngageRoutine(ControlPadInput.PadNum _engagingPadNum, EngagmentCircumstances _circumstances, EngagementSlot _intendedSlot, bool _forceUserChoice, VoidGeneric<GamepadUser> _finishedCallback);

	// Token: 0x06002357 RID: 9047
	public abstract bool HasPlayer();

	// Token: 0x06002358 RID: 9048
	public abstract bool HasSavablePlayer();

	// Token: 0x06002359 RID: 9049 RVA: 0x000A82A4 File Offset: 0x000A66A4
	public virtual void ShowGamerCard(EngagementSlot slot)
	{
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000A82A6 File Offset: 0x000A66A6
	public virtual void ShowGamerCard(GamepadUser localUser)
	{
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000A82A8 File Offset: 0x000A66A8
	public virtual void ShowGamerCard(OnlineUserPlatformId onlineUser)
	{
	}

	// Token: 0x0600235C RID: 9052
	public abstract bool SupportsInvitesForAnyUser();

	// Token: 0x04001AD1 RID: 6865
	[SerializeField]
	[AssignChildRecursive("Dimmer", Editorbility.NonEditable)]
	private GameObject m_dimmer;

	// Token: 0x04001AD2 RID: 6866
	[SerializeField]
	[AssignChildRecursive("RejoinUI", Editorbility.NonEditable)]
	private GameObject m_rejoin;

	// Token: 0x04001AD3 RID: 6867
	[SerializeField]
	private Text m_rejoinGamerNameText;

	// Token: 0x04001AD4 RID: 6868
	[SerializeField]
	[AssignResource("SidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_sidedAmbiMapping;

	// Token: 0x04001AD5 RID: 6869
	[SerializeField]
	[AssignResource("UnsidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_unsidedAmbiMapping;

	// Token: 0x04001AD6 RID: 6870
	protected Dictionary<EngagementSlot, GamepadUserType> m_engagedPads = new Dictionary<EngagementSlot, GamepadUserType>(new PlayerManagerShared<GamepadUserType>.EngagementSlotComparer());

	// Token: 0x04001AD7 RID: 6871
	protected Dictionary<ControlPadInput.PadNum, ILogicalButton> m_engagementButtons = new Dictionary<ControlPadInput.PadNum, ILogicalButton>(new PlayerManagerShared<GamepadUserType>.PadComparer());

	// Token: 0x04001AD8 RID: 6872
	private List<PlayerManagerShared<GamepadUserType>.EngagementRoutine> m_engagementRoutines = new List<PlayerManagerShared<GamepadUserType>.EngagementRoutine>();

	// Token: 0x04001AD9 RID: 6873
	private TimeManager.PauseLayer? m_lostEngagedPauseLayer;

	// Token: 0x04001ADA RID: 6874
	private Suppressor m_lostEngagedSuppressor;

	// Token: 0x04001ADB RID: 6875
	protected List<GamepadUserType> m_lostUsers = new List<GamepadUserType>();

	// Token: 0x04001ADC RID: 6876
	private GamepadUserType m_cachedPrimaryUser;

	// Token: 0x04001ADD RID: 6877
	protected const int c_numEngagementSlots = 4;

	// Token: 0x04001ADE RID: 6878
	protected static bool c_AcceptInverted;

	// Token: 0x04001ADF RID: 6879
	public bool CanChangeSplitPads;

	// Token: 0x04001AE1 RID: 6881
	[CompilerGenerated]
	private static Generic<string, EngagementSlot, string> <>f__mg$cache0;

	// Token: 0x02000735 RID: 1845
	public class EngagementSlotComparer : IEqualityComparer<EngagementSlot>
	{
		// Token: 0x06002360 RID: 9056 RVA: 0x000A82B6 File Offset: 0x000A66B6
		public bool Equals(EngagementSlot x, EngagementSlot y)
		{
			return x == y;
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000A82BC File Offset: 0x000A66BC
		public int GetHashCode(EngagementSlot obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x02000736 RID: 1846
	public class PadComparer : IEqualityComparer<ControlPadInput.PadNum>
	{
		// Token: 0x06002363 RID: 9059 RVA: 0x000A82C7 File Offset: 0x000A66C7
		public bool Equals(ControlPadInput.PadNum x, ControlPadInput.PadNum y)
		{
			return x == y;
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000A82CD File Offset: 0x000A66CD
		public int GetHashCode(ControlPadInput.PadNum obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x02000737 RID: 1847
	private class EngagementRoutine
	{
		// Token: 0x06002365 RID: 9061 RVA: 0x000A82D0 File Offset: 0x000A66D0
		public EngagementRoutine(EngagementSlot _slot, IEnumerator _routine)
		{
			this.Slot = _slot;
			this.Routine = _routine;
		}

		// Token: 0x04001AE3 RID: 6883
		public EngagementSlot Slot;

		// Token: 0x04001AE4 RID: 6884
		public IEnumerator Routine;
	}
}
