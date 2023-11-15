using System;
using UnityEngine;

// Token: 0x0200078A RID: 1930
public class TutorialOutroFlowroutine : CutsceneOutroFlowroutineBase, CampaignFlowController.IOutroFlowSceneProvider
{
	// Token: 0x06002554 RID: 9556 RVA: 0x000B0DAC File Offset: 0x000AF1AC
	protected void Awake()
	{
		if (this.m_cutscene != null)
		{
			this.m_cutscene.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000B0DD0 File Offset: 0x000AF1D0
	protected override CutsceneController GetCutsceneController()
	{
		return this.m_cutscene;
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000B0DD8 File Offset: 0x000AF1D8
	public string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen)
	{
		o_loadState = GameState.LoadKitchen;
		o_loadEndState = GameState.RunKitchen;
		o_useLoadingScreen = true;
		return this.m_nextScene;
	}

	// Token: 0x04001CEA RID: 7402
	[SerializeField]
	private CutsceneController m_cutscene;

	// Token: 0x04001CEB RID: 7403
	[SerializeField]
	[SceneName]
	private string m_nextScene = string.Empty;
}
