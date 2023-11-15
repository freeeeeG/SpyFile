using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200072C RID: 1836
public class PadSplitManager : Manager
{
	// Token: 0x060022EC RID: 8940 RVA: 0x000A7582 File Offset: 0x000A5982
	public bool IsUIOpen()
	{
		return this.m_padSplitCoroutine != null;
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000A7590 File Offset: 0x000A5990
	private void Awake()
	{
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		this.m_metaGame = GameUtils.GetMetaGameProgress();
		this.m_gameDebugConfig = GameUtils.GetDebugConfig();
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000A75CC File Offset: 0x000A59CC
	private void Start()
	{
		this.m_padSplitOpenButtons = new ILogicalButton[4];
		for (int i = 0; i < this.m_padSplitOpenButtons.Length; i++)
		{
			ControlPadInput.PadNum pad = (ControlPadInput.PadNum)i;
			PlayerGameInput playerGameInput = new PlayerGameInput(pad, PadSide.Both, this.m_unsidedMappingData);
			ILogicalButton fixedButton = PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.Curse, playerGameInput);
			this.m_padSplitOpenButtons[i] = fixedButton;
		}
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000A7620 File Offset: 0x000A5A20
	private bool InWorldMap(string _sceneName)
	{
		if (_sceneName.Contains("_Map"))
		{
			return true;
		}
		GameSession gameSession = GameUtils.GetGameSession();
		return gameSession != null && _sceneName.Equals(gameSession.TypeSettings.WorldMapScene);
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000A7665 File Offset: 0x000A5A65
	private bool CanOpenPadSplit()
	{
		return false;
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000A7668 File Offset: 0x000A5A68
	private bool ShouldClosePadSplit()
	{
		string name = SceneManager.GetActiveScene().name;
		return this.AnyOpenPressed() || !this.InWorldMap(name);
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000A769C File Offset: 0x000A5A9C
	private void Update()
	{
		if (this.m_padSplitCoroutine == null)
		{
			bool flag = this.CanOpenPadSplit();
			this.m_padSplitPromptUI.SetBool(PadSplitManager.m_Open, flag);
			if (flag && this.AnyOpenPressed())
			{
				this.m_padSplitCoroutine = this.RunPadSplit();
			}
		}
		else
		{
			this.m_padSplitPromptUI.SetBool(PadSplitManager.m_Open, false);
			this.m_padSplitCoroutine.MoveNext();
			if (this.ShouldClosePadSplit())
			{
				this.EndPadSplit();
			}
		}
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000A771C File Offset: 0x000A5B1C
	private bool AnyOpenPressed()
	{
		for (int i = 0; i < 4; i++)
		{
			ILogicalButton logicalButton = this.m_padSplitOpenButtons[i];
			if (logicalButton.JustPressed())
			{
				EngagementSlot slot = (EngagementSlot)i;
				GamepadUser user = this.m_playerManager.GetUser(slot);
				if (user != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000A7770 File Offset: 0x000A5B70
	private IEnumerator RunPadSplit()
	{
		TimeManager timeManager = GameUtils.RequireManager<TimeManager>();
		timeManager.SetPaused(TimeManager.PauseLayer.Main, true, this);
		GameObject multiChefUI = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_multiChefUIPrefab.gameObject);
		MultiChefUIController multiChefUIController = multiChefUI.RequireComponent<MultiChefUIController>();
		this.m_menuSession = new PadSplitManager.MenuSession(multiChefUIController);
		multiChefUIController.OnExitCallback += this.EndPadSplit;
		for (;;)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000A778C File Offset: 0x000A5B8C
	private void EndPadSplit()
	{
		if (this.m_menuSession != null)
		{
			if (this.m_menuSession.MultiChefUI != null)
			{
				UnityEngine.Object.Destroy(this.m_menuSession.MultiChefUI.gameObject);
			}
			this.m_menuSession = null;
		}
		TimeManager timeManager = GameUtils.RequireManager<TimeManager>();
		timeManager.SetPaused(TimeManager.PauseLayer.Main, false, this);
		this.m_padSplitCoroutine = null;
		GameUtils.RequireManager<SaveManager>().SaveMetaProgress(null);
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000A77F7 File Offset: 0x000A5BF7
	private void OnEngagementChanged(EngagementSlot _slot, GamepadUser _userBefore, GamepadUser _userAfter)
	{
		if (_userAfter == null && _userBefore != null && !_userBefore.StickyEngagement)
		{
			this.ClearSplitForSlot(_slot);
		}
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000A7824 File Offset: 0x000A5C24
	private void ClearSplitForSlot(EngagementSlot _slot)
	{
		GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
		GameInputConfig.ConfigEntry[] collection = baseInputConfig.m_playerConfigs.AllRemoved_Predicate((GameInputConfig.ConfigEntry x) => x.Pad == (ControlPadInput.PadNum)_slot);
		List<GameInputConfig.ConfigEntry> list = new List<GameInputConfig.ConfigEntry>(collection);
		PadSplitManager.FixupConfigList(list, this.m_unsidedMappingData);
		GameInputConfig baseInputConfig2 = new GameInputConfig(list.ToArray());
		PlayerInputLookup.SetBaseInputConfig(baseInputConfig2);
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000A7884 File Offset: 0x000A5C84
	public static void FixupConfigList(List<GameInputConfig.ConfigEntry> _configList, AmbiControlsMappingData _unsidedMappingData)
	{
		int i;
		for (i = 0; i < 4; i++)
		{
			if (_configList.FindIndex((GameInputConfig.ConfigEntry x) => x.Player == (PlayerInputLookup.Player)i) == -1)
			{
				int j;
				for (j = 0; j < 4; j++)
				{
					if (_configList.FindIndex((GameInputConfig.ConfigEntry x) => x.Pad == (ControlPadInput.PadNum)j) == -1)
					{
						PlayerInputLookup.Player l = (PlayerInputLookup.Player)i;
						ControlPadInput.PadNum k = (ControlPadInput.PadNum)j;
						User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
						_configList.Add(new GameInputConfig.ConfigEntry(l, k, PadSide.Both, s_LocalMachineId, _unsidedMappingData));
						break;
					}
				}
			}
		}
		int p;
		for (p = 0; p < 4; p++)
		{
			List<GameInputConfig.ConfigEntry> list = _configList.FindAll((GameInputConfig.ConfigEntry x) => x.Pad == (ControlPadInput.PadNum)p);
			if (list.Count == 1)
			{
				list[0].UIHandedness = list[0].Side;
			}
		}
	}

	// Token: 0x04001ABE RID: 6846
	[SerializeField]
	[AssignResource("SidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_sidedMappingData;

	// Token: 0x04001ABF RID: 6847
	[SerializeField]
	[AssignResource("UnsidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_unsidedMappingData;

	// Token: 0x04001AC0 RID: 6848
	[SerializeField]
	private Animator m_padSplitPromptUI;

	// Token: 0x04001AC1 RID: 6849
	[SerializeField]
	private MultiChefUIController m_multiChefUIPrefab;

	// Token: 0x04001AC2 RID: 6850
	private GameDebugConfig m_gameDebugConfig;

	// Token: 0x04001AC3 RID: 6851
	private PadSplitManager.MenuSession m_menuSession;

	// Token: 0x04001AC4 RID: 6852
	private PlayerManager m_playerManager;

	// Token: 0x04001AC5 RID: 6853
	private MetaGameProgress m_metaGame;

	// Token: 0x04001AC6 RID: 6854
	private ILogicalButton[] m_padSplitOpenButtons;

	// Token: 0x04001AC7 RID: 6855
	private IEnumerator m_padSplitCoroutine;

	// Token: 0x04001AC8 RID: 6856
	private static int m_Open = Animator.StringToHash("Open");

	// Token: 0x0200072D RID: 1837
	private class MenuSession
	{
		// Token: 0x060022FA RID: 8954 RVA: 0x000A79C8 File Offset: 0x000A5DC8
		public MenuSession(MultiChefUIController _multiChefUI)
		{
			this.MultiChefUI = _multiChefUI;
		}

		// Token: 0x04001AC9 RID: 6857
		public MultiChefUIController MultiChefUI;
	}
}
