using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000682 RID: 1666
public class LevelIntroFlowroutine : IntroFlowroutineBase
{
	// Token: 0x06001FFC RID: 8188 RVA: 0x0009AAA7 File Offset: 0x00098EA7
	protected virtual void Awake()
	{
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x0009AAAC File Offset: 0x00098EAC
	public override void Setup(CallbackVoid _startRoundCallback)
	{
		this.m_startRoundCallback = _startRoundCallback;
		if (this.m_data.ReadyUIPrefab != null)
		{
			this.m_readyUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_data.ReadyUIPrefab);
			this.m_readyUIInstance.SetActive(false);
		}
		if (this.m_data.GoUIPrefab != null)
		{
			this.m_goUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_data.GoUIPrefab);
			this.m_goUIInstance.SetActive(false);
		}
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x0009AB30 File Offset: 0x00098F30
	public override IEnumerator Run()
	{
		TimeManager timeManager = GameUtils.RequireManager<TimeManager>();
		timeManager.SetPaused(TimeManager.PauseLayer.Main, true, this);
		if (this.m_data.TutorialPopup.CanSpawn() && ClientGameSetup.Mode == GameMode.Campaign)
		{
			ClientTutorialPopupController controller = base.gameObject.RequireComponent<ClientTutorialPopupController>();
			IEnumerator tutorialRoutine = controller.ShowTutorial(this.m_data.TutorialPopup, new Generic<IEnumerator, GameObject>(this.TutorialDismissRoutine));
			while (tutorialRoutine.MoveNext())
			{
				yield return null;
			}
			controller.Shutdown();
		}
		int layerID = LayerMask.NameToLayer("UI");
		IEnumerator timerRoutine = CoroutineUtils.TimerRoutine(this.m_data.ReadyDelay, layerID);
		while (timerRoutine.MoveNext())
		{
			yield return null;
		}
		if (this.m_readyUIInstance != null)
		{
			this.m_readyUIInstance.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.ReadyIntro, this.m_goUIInstance.layer);
			timerRoutine = CoroutineUtils.TimerRoutine(this.m_data.ReadyUILifetime, layerID);
			while (timerRoutine.MoveNext())
			{
				yield return null;
			}
			UnityEngine.Object.Destroy(this.m_readyUIInstance);
		}
		if (!ConnectionStatus.IsInSession())
		{
			PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
			while (playerManager.IsWarningActive(PlayerWarning.Disengaged))
			{
				yield return null;
			}
		}
		timeManager.SetPaused(TimeManager.PauseLayer.Main, false, this);
		this.m_startRoundCallback();
		if (this.m_goUIInstance)
		{
			this.m_goUIInstance.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.LevelGo, this.m_goUIInstance.layer);
			timerRoutine = CoroutineUtils.TimerRoutine(this.m_data.GOUILifetime, layerID);
			while (timerRoutine.MoveNext())
			{
				yield return null;
			}
			UnityEngine.Object.Destroy(this.m_goUIInstance);
		}
		yield break;
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x0009AB4C File Offset: 0x00098F4C
	private IEnumerator TutorialDismissRoutine(GameObject _ui)
	{
		ILogicalButton rawSkipButton = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart, PadSide.Both);
		TimedLogicalButton skipButton = new TimedLogicalButton(rawSkipButton, TimedLogicalButton.Condition.HeldLonger, 1f);
		if (_ui != null)
		{
			TimedInputUIController timedInputUIController = _ui.RequestComponentRecursive<TimedInputUIController>();
			timedInputUIController.SetDisplayInput(skipButton, 1f);
		}
		IEnumerator autoDismissTimer = CoroutineUtils.TimerRoutine(15f, LayerMask.NameToLayer("UI"));
		while (autoDismissTimer.MoveNext() && !skipButton.JustPressed())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001857 RID: 6231
	private const float c_tutorialAutoDismissTime = 15f;

	// Token: 0x04001858 RID: 6232
	private const float c_dismissTutorialHoldDuration = 1f;

	// Token: 0x04001859 RID: 6233
	[SerializeField]
	private LevelIntroFlowroutineData m_data;

	// Token: 0x0400185A RID: 6234
	private GameObject m_readyUIInstance;

	// Token: 0x0400185B RID: 6235
	private GameObject m_goUIInstance;

	// Token: 0x0400185C RID: 6236
	private CallbackVoid m_startRoundCallback = delegate()
	{
	};
}
