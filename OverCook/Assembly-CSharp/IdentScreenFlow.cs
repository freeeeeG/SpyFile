using System;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020006BD RID: 1725
[RequireComponent(typeof(Animator))]
public class IdentScreenFlow : MonoBehaviour
{
	// Token: 0x060020A2 RID: 8354 RVA: 0x0009DA6C File Offset: 0x0009BE6C
	private void Start()
	{
		this.optionsData = new OptionsData();
		this.optionsData.OnAwake();
		this.optionsData.LoadFromSave();
		this.optionsData = null;
		this.m_skipButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
		this.m_animator = base.gameObject.RequestComponent<Animator>();
		this.m_SceneLoaderHelper = base.gameObject.AddComponent<SceneLoaderHelper>();
		this.m_SceneLoaderHelper.LoadLevelAsync(this.m_nextSceneName, false, LoadSceneMode.Single);
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x0009DAE4 File Offset: 0x0009BEE4
	private void Update()
	{
		if (!this.m_bSetOptions && GameUtils.GetGameMetaEnvironment() != null)
		{
			AudioManager x = GameUtils.RequestManager<AudioManager>();
			if (x != null)
			{
				this.optionsData = new OptionsData();
				this.optionsData.OnAwake();
				this.optionsData.LoadFromSave();
				this.optionsData = null;
				this.m_bSetOptions = true;
			}
		}
		if (this.m_skipButton.JustPressed() || InputManager.ActiveDevice.Action1.IsPressed)
		{
			this.m_animator.SetTrigger(IdentScreenFlow.m_iSkip);
		}
		if (this.m_ProgressBar != null)
		{
			if (this.m_SceneLoaderHelper != null)
			{
				this.m_ProgressBar.SetValue(this.m_SceneLoaderHelper.GetProgress());
			}
			else
			{
				this.m_ProgressBar.SetValue(0f);
			}
		}
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x0009DBCE File Offset: 0x0009BFCE
	public void ActivateNextScene()
	{
		this.m_SceneLoaderHelper.ActivateSceneWhenLoaded = true;
	}

	// Token: 0x04001908 RID: 6408
	[SerializeField]
	[SceneName]
	private string m_nextSceneName = "StartScreen";

	// Token: 0x04001909 RID: 6409
	private Animator m_animator;

	// Token: 0x0400190A RID: 6410
	private ILogicalButton m_skipButton;

	// Token: 0x0400190B RID: 6411
	private AsyncOperation m_async;

	// Token: 0x0400190C RID: 6412
	private OptionsData optionsData;

	// Token: 0x0400190D RID: 6413
	private SceneLoaderHelper m_SceneLoaderHelper;

	// Token: 0x0400190E RID: 6414
	public ProgressBarUI m_ProgressBar;

	// Token: 0x0400190F RID: 6415
	private static readonly int m_iSkip = Animator.StringToHash("Skip");

	// Token: 0x04001910 RID: 6416
	private bool m_bSetOptions;
}
