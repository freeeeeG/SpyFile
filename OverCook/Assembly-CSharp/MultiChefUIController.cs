using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B2D RID: 2861
[ExecutionDependency(typeof(MultiChefPadUIController))]
public class MultiChefUIController : UIControllerBase
{
	// Token: 0x14000037 RID: 55
	// (add) Token: 0x060039F8 RID: 14840 RVA: 0x00113D60 File Offset: 0x00112160
	// (remove) Token: 0x060039F9 RID: 14841 RVA: 0x00113D98 File Offset: 0x00112198
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid OnExitCallback = delegate()
	{
	};

	// Token: 0x060039FA RID: 14842 RVA: 0x00113DD0 File Offset: 0x001121D0
	private void Awake()
	{
		for (int i = 0; i < this.m_padUI.Length; i++)
		{
			this.m_padUIDictionary.Add(this.m_padUI[i].PadNum, this.m_padUI[i]);
			this.m_padUI[i].Reinitialise();
		}
		for (int j = 0; j < this.m_padUI.Length; j++)
		{
			this.m_padUI[j].CanAddPlayerQuery = new Generic<bool, int>(this.CanAddPlayer);
			this.m_padUI[j].OnSplitStateChange += this.OnSplitChange;
		}
		ILogicalButton uibutton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
		ILogicalButton uibutton2 = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart, PlayerInputLookup.Player.One);
		this.m_completeButton = new ComboLogicalButton(new ILogicalButton[]
		{
			uibutton,
			uibutton2
		}, ComboLogicalButton.ComboType.Or);
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x00113E9A File Offset: 0x0011229A
	private void Update()
	{
		if (this.m_completeButton.JustPressed() && !TimeManager.IsPaused(TimeManager.PauseLayer.System))
		{
			this.OnExitCallback();
		}
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x00113EC4 File Offset: 0x001122C4
	public bool CanAddPlayer(int _numToAdd)
	{
		int num = 0;
		IEnumerator enumerator = this.IteratePadPlayerAssignment().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return num + _numToAdd <= this.m_padUIDictionary.Count;
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x00113F38 File Offset: 0x00112338
	private IEnumerable IteratePadPlayerAssignment()
	{
		int playerId = 0;
		for (int i = 0; i < this.m_padUIDictionary.Count; i++)
		{
			ControlPadInput.PadNum padNum = (ControlPadInput.PadNum)i;
			MultiChefPadUIController padUIController = this.m_padUIDictionary[padNum];
			IEnumerator enumerator = padUIController.IterateSides().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					PadSide side = (PadSide)obj;
					yield return new MultiChefUIController.PadPlayerInfo((PlayerInputLookup.Player)playerId, padNum, side);
					playerId++;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		yield break;
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x00113F5C File Offset: 0x0011235C
	private void OnSplitChange()
	{
		List<GameInputConfig.ConfigEntry> list = new List<GameInputConfig.ConfigEntry>();
		User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
		IEnumerator enumerator = this.IteratePadPlayerAssignment().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				MultiChefUIController.PadPlayerInfo padPlayerInfo = (MultiChefUIController.PadPlayerInfo)obj;
				MultiChefPadUIController multiChefPadUIController = this.m_padUIDictionary[padPlayerInfo.Pad];
				multiChefPadUIController.AssignPlayer(padPlayerInfo.PadSide, padPlayerInfo.PlayerId);
				AmbiControlsMappingData mappingData = (padPlayerInfo.PadSide != PadSide.Both) ? this.m_sidedMappingData : this.m_unsidedMappingData;
				list.Add(new GameInputConfig.ConfigEntry(padPlayerInfo.PlayerId, padPlayerInfo.Pad, padPlayerInfo.PadSide, s_LocalMachineId, mappingData));
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		PadSplitManager.FixupConfigList(list, this.m_unsidedMappingData);
		GameInputConfig baseInputConfig = new GameInputConfig(list.ToArray());
		PlayerInputLookup.SetBaseInputConfig(baseInputConfig);
	}

	// Token: 0x04002EF0 RID: 12016
	[SerializeField]
	[AssignComponentRecursive(Editorbility.NonEditable)]
	private MultiChefPadUIController[] m_padUI = new MultiChefPadUIController[4];

	// Token: 0x04002EF1 RID: 12017
	[SerializeField]
	[AssignResource("SidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_sidedMappingData;

	// Token: 0x04002EF2 RID: 12018
	[SerializeField]
	[AssignResource("UnsidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_unsidedMappingData;

	// Token: 0x04002EF3 RID: 12019
	private ILogicalButton m_completeButton;

	// Token: 0x04002EF4 RID: 12020
	private Dictionary<ControlPadInput.PadNum, MultiChefPadUIController> m_padUIDictionary = new Dictionary<ControlPadInput.PadNum, MultiChefPadUIController>();

	// Token: 0x02000B2E RID: 2862
	private class PadPlayerInfo
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x00114052 File Offset: 0x00112452
		public PadPlayerInfo(PlayerInputLookup.Player _p, ControlPadInput.PadNum _n, PadSide _s)
		{
			this.PlayerId = _p;
			this.Pad = _n;
			this.PadSide = _s;
		}

		// Token: 0x04002EF6 RID: 12022
		public PlayerInputLookup.Player PlayerId;

		// Token: 0x04002EF7 RID: 12023
		public ControlPadInput.PadNum Pad;

		// Token: 0x04002EF8 RID: 12024
		public PadSide PadSide;
	}
}
