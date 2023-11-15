using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000A4E RID: 2638
public class BossLevelOutroFlowroutine : CutsceneOutroFlowroutineBase, CampaignFlowController.IOutroFlowSceneProvider
{
	// Token: 0x06003424 RID: 13348 RVA: 0x000F5065 File Offset: 0x000F3465
	public void SetOutroDirectors(CutsceneController _successDirector, CutsceneController _failureDirector)
	{
		this.m_successCutscene = (_successDirector ?? this.m_successCutscene);
		this.m_failureCutscene = (_failureDirector ?? this.m_failureCutscene);
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x000F5090 File Offset: 0x000F3490
	protected override void Setup(CampaignFlowController.OutroData _setupData)
	{
		this.m_succeeded = (_setupData.StarsAwarded > 0);
		if (this.m_successCutscene != null)
		{
			this.m_successCutscene.gameObject.SetActive(false);
		}
		if (this.m_failureCutscene != null)
		{
			this.m_failureCutscene.gameObject.SetActive(false);
		}
		base.Setup(_setupData);
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x000F50F8 File Offset: 0x000F34F8
	protected override IEnumerator Run()
	{
		IEnumerator cutsceneRoutine = base.Run();
		while (cutsceneRoutine.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000F5113 File Offset: 0x000F3513
	protected override CutsceneController GetCutsceneController()
	{
		return (!this.m_succeeded) ? this.m_failureCutscene : this.m_successCutscene;
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x000F5134 File Offset: 0x000F3534
	public string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen)
	{
		if (this.m_succeeded)
		{
			o_loadState = GameState.LoadKitchen;
			o_loadEndState = GameState.NotSet;
			o_useLoadingScreen = false;
			return this.m_creditsScene;
		}
		o_loadState = GameState.LoadKitchen;
		o_loadEndState = GameState.RunKitchen;
		o_useLoadingScreen = true;
		return SceneManager.GetActiveScene().name;
	}

	// Token: 0x040029D9 RID: 10713
	[SerializeField]
	private CutsceneController m_successCutscene;

	// Token: 0x040029DA RID: 10714
	[SerializeField]
	private CutsceneController m_failureCutscene;

	// Token: 0x040029DB RID: 10715
	[SerializeField]
	[SceneName]
	private string m_creditsScene = string.Empty;

	// Token: 0x040029DC RID: 10716
	private bool m_succeeded;
}
