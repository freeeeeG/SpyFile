using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000BB0 RID: 2992
public class OptionsMenuScreen : KModalButtonMenu
{
	// Token: 0x06005D56 RID: 23894 RVA: 0x00222334 File Offset: 0x00220534
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepMenuOpen = true;
		this.buttons = new List<KButtonMenu.ButtonInfo>
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GRAPHICS, global::Action.NumActions, new UnityAction(this.OnGraphicsOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.AUDIO, global::Action.NumActions, new UnityAction(this.OnAudioOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GAME, global::Action.NumActions, new UnityAction(this.OnGameOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.METRICS, global::Action.NumActions, new UnityAction(this.OnMetrics), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.FEEDBACK, global::Action.NumActions, new UnityAction(this.OnFeedback), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.CREDITS, global::Action.NumActions, new UnityAction(this.OnCredits), null, null)
		};
		this.closeButton.onClick += this.Deactivate;
		this.backButton.onClick += this.Deactivate;
	}

	// Token: 0x06005D57 RID: 23895 RVA: 0x00222479 File Offset: 0x00220679
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.OPTIONS_SCREEN.TITLE);
		this.backButton.transform.SetAsLastSibling();
	}

	// Token: 0x06005D58 RID: 23896 RVA: 0x002224A8 File Offset: 0x002206A8
	protected override void OnActivate()
	{
		base.OnActivate();
		foreach (GameObject gameObject in this.buttonObjects)
		{
		}
	}

	// Token: 0x06005D59 RID: 23897 RVA: 0x002224D4 File Offset: 0x002206D4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005D5A RID: 23898 RVA: 0x002224F6 File Offset: 0x002206F6
	private void OnGraphicsOptions()
	{
		base.ActivateChildScreen(this.graphicsOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06005D5B RID: 23899 RVA: 0x0022250A File Offset: 0x0022070A
	private void OnAudioOptions()
	{
		base.ActivateChildScreen(this.audioOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06005D5C RID: 23900 RVA: 0x0022251E File Offset: 0x0022071E
	private void OnGameOptions()
	{
		base.ActivateChildScreen(this.gameOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06005D5D RID: 23901 RVA: 0x00222532 File Offset: 0x00220732
	private void OnMetrics()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x06005D5E RID: 23902 RVA: 0x00222546 File Offset: 0x00220746
	public void ShowMetricsScreen()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x06005D5F RID: 23903 RVA: 0x0022255A File Offset: 0x0022075A
	private void OnFeedback()
	{
		base.ActivateChildScreen(this.feedbackScreenPrefab.gameObject);
	}

	// Token: 0x06005D60 RID: 23904 RVA: 0x0022256E File Offset: 0x0022076E
	private void OnCredits()
	{
		base.ActivateChildScreen(this.creditsScreenPrefab.gameObject);
	}

	// Token: 0x06005D61 RID: 23905 RVA: 0x00222582 File Offset: 0x00220782
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x04003ECF RID: 16079
	[SerializeField]
	private GameOptionsScreen gameOptionsScreenPrefab;

	// Token: 0x04003ED0 RID: 16080
	[SerializeField]
	private AudioOptionsScreen audioOptionsScreenPrefab;

	// Token: 0x04003ED1 RID: 16081
	[SerializeField]
	private GraphicsOptionsScreen graphicsOptionsScreenPrefab;

	// Token: 0x04003ED2 RID: 16082
	[SerializeField]
	private CreditsScreen creditsScreenPrefab;

	// Token: 0x04003ED3 RID: 16083
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003ED4 RID: 16084
	[SerializeField]
	private MetricsOptionsScreen metricsScreenPrefab;

	// Token: 0x04003ED5 RID: 16085
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x04003ED6 RID: 16086
	[SerializeField]
	private LocText title;

	// Token: 0x04003ED7 RID: 16087
	[SerializeField]
	private KButton backButton;
}
