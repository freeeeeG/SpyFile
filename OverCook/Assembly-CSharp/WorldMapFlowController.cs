using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000BE1 RID: 3041
[ExecutionDependency(typeof(WorldMapCamera))]
public class WorldMapFlowController : Manager
{
	// Token: 0x06003E31 RID: 15921 RVA: 0x00129D1F File Offset: 0x0012811F
	public SceneDirectoryData GetSceneDirectory()
	{
		return this.m_sceneDirectory;
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x00129D28 File Offset: 0x00128128
	public bool IsLevelUnlocked(PortalMapNode _node)
	{
		GameProgress progress = GameUtils.GetGameSession().Progress;
		return _node.LevelIndex != -1 && (_node.ForceUnlocked || DebugManager.Instance.GetOption("Unlock all levels") || progress.SaveData.IsLevelUnlocked(_node.LevelIndex, true));
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x00129D84 File Offset: 0x00128184
	public bool IsSwitchSwitched(SwitchMapNode _node)
	{
		GameProgress progress = GameUtils.GetGameSession().Progress;
		if (_node.SwitchID <= 0)
		{
			return false;
		}
		if (DebugManager.Instance.GetOption("Raise all ramps"))
		{
			return true;
		}
		GameProgress.GameProgressData.SwitchState switchState = progress.GetSwitchState(_node.SwitchID);
		return switchState.Activated || _node.IsSwitchedDueToCompletion();
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x00129DE8 File Offset: 0x001281E8
	private void Awake()
	{
		UserSystemUtils.BuildGameInputConfig();
	}

	// Token: 0x040031EC RID: 12780
	[SerializeField]
	private SceneDirectoryData m_sceneDirectory;

	// Token: 0x040031ED RID: 12781
	[SerializeField]
	public AmbiControlsMappingData m_sidedAmbiMapping;

	// Token: 0x040031EE RID: 12782
	[SerializeField]
	public AmbiControlsMappingData m_unsidedAmbiMapping;

	// Token: 0x040031EF RID: 12783
	[SerializeField]
	public WorldMapFlowController.RevealSequenceData m_unfoldSequenceData = new WorldMapFlowController.RevealSequenceData();

	// Token: 0x040031F0 RID: 12784
	[SerializeField]
	[AssignResource("WorldMapInfoPopup_NewGamePlus", Editorbility.Editable)]
	public WorldMapInfoPopup m_newGamePlusDialogPrefab;

	// Token: 0x040031F1 RID: 12785
	[SerializeField]
	[AssignResource("WorldMapInfoPopup_PractiseMode", Editorbility.Editable)]
	public WorldMapInfoPopup m_practiceModeDialogPrefab;

	// Token: 0x040031F2 RID: 12786
	[SerializeField]
	[AssignResource("WorldMapInfoPopup_HordeMode", Editorbility.Editable)]
	public WorldMapInfoPopup m_hordeModeDialogPrefab;

	// Token: 0x02000BE2 RID: 3042
	[Serializable]
	public class RevealSequenceData
	{
		// Token: 0x040031F3 RID: 12787
		public float TimePerNode = 1f;

		// Token: 0x040031F4 RID: 12788
		public float IdealTransitTime = 1f;

		// Token: 0x040031F5 RID: 12789
		public float MinMoveSpeed = 10f;

		// Token: 0x040031F6 RID: 12790
		public float MaxMoveSpeed = 50f;

		// Token: 0x040031F7 RID: 12791
		public float AccelerationTime = 1f;
	}
}
