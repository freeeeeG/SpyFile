using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067E RID: 1662
public abstract class CutsceneOutroFlowroutineBase : CampaignFlowController.OutroFlowroutine
{
	// Token: 0x06001FE7 RID: 8167 RVA: 0x0009B1CC File Offset: 0x000995CC
	protected override void Setup(CampaignFlowController.OutroData _setupData)
	{
		if (this.m_data.TimesUpUIPrefab != null)
		{
			this.m_timesUpUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_data.TimesUpUIPrefab);
			this.m_timesUpUIInstance.SetActive(false);
		}
		CutsceneController cutsceneController = this.GetCutsceneController();
		this.m_cutsceneController = cutsceneController.gameObject.RequireComponent<ClientCutsceneController>();
		CutsceneController.SetupData setupData = new CutsceneController.SetupData();
		setupData.skippable = true;
		setupData.postplaybackUIEnabled = false;
		this.m_cutsceneRoutine = this.m_cutsceneController.StartCutscene(setupData);
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x0009B250 File Offset: 0x00099650
	protected override IEnumerator Run()
	{
		int layerID = LayerMask.NameToLayer("UI");
		IEnumerator timerRoutine = null;
		if (this.m_timesUpUIInstance != null)
		{
			this.m_timesUpUIInstance.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.TimesUp, this.m_timesUpUIInstance.layer);
			timerRoutine = CoroutineUtils.TimerRoutine(this.m_data.TimesUpUILifetime, layerID);
			while (timerRoutine.MoveNext())
			{
				yield return null;
			}
			UnityEngine.Object.Destroy(this.m_timesUpUIInstance);
		}
		if (this.m_cutsceneController != null)
		{
			this.m_cutsceneController.gameObject.SetActive(true);
			while (this.m_cutsceneRoutine != null && this.m_cutsceneRoutine.MoveNext())
			{
				yield return this.m_cutsceneRoutine.Current;
			}
			this.m_cutsceneController.Shutdown();
			this.m_cutsceneController.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x0009B26B File Offset: 0x0009966B
	protected override void Shutdown()
	{
	}

	// Token: 0x06001FEA RID: 8170
	protected abstract CutsceneController GetCutsceneController();

	// Token: 0x04001845 RID: 6213
	[SerializeField]
	private LevelOutroFlowroutineData m_data;

	// Token: 0x04001846 RID: 6214
	private ClientCutsceneController m_cutsceneController;

	// Token: 0x04001847 RID: 6215
	private IEnumerator m_cutsceneRoutine;

	// Token: 0x04001848 RID: 6216
	private GameObject m_timesUpUIInstance;
}
