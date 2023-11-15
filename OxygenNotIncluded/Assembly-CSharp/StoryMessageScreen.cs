using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A7F RID: 2687
public class StoryMessageScreen : KScreen
{
	// Token: 0x170005FD RID: 1533
	// (set) Token: 0x060051B1 RID: 20913 RVA: 0x001D28D7 File Offset: 0x001D0AD7
	public string title
	{
		set
		{
			this.titleLabel.SetText(value);
		}
	}

	// Token: 0x170005FE RID: 1534
	// (set) Token: 0x060051B2 RID: 20914 RVA: 0x001D28E5 File Offset: 0x001D0AE5
	public string body
	{
		set
		{
			this.bodyLabel.SetText(value);
		}
	}

	// Token: 0x060051B3 RID: 20915 RVA: 0x001D28F3 File Offset: 0x001D0AF3
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x060051B4 RID: 20916 RVA: 0x001D28FA File Offset: 0x001D0AFA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryMessageScreen.HideInterface(true);
		CameraController.Instance.FadeOut(0.5f, 1f, null);
	}

	// Token: 0x060051B5 RID: 20917 RVA: 0x001D291D File Offset: 0x001D0B1D
	private IEnumerator ExpandPanel()
	{
		this.content.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(0.25f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		CameraController.Instance.FadeOut(0.5f, 1f, null);
		yield return null;
		yield break;
	}

	// Token: 0x060051B6 RID: 20918 RVA: 0x001D292C File Offset: 0x001D0B2C
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 0f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, -1f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.content.gameObject.SetActive(false);
		if (this.OnClose != null)
		{
			this.OnClose();
			this.OnClose = null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x060051B7 RID: 20919 RVA: 0x001D293C File Offset: 0x001D0B3C
	public static void HideInterface(bool hide)
	{
		SelectTool.Instance.Select(null, true);
		NotificationScreen.Instance.Show(!hide);
		OverlayMenu.Instance.Show(!hide);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(!hide);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(!hide);
		}
		ManagementMenu.Instance.Show(!hide);
		ToolMenu.Instance.Show(!hide);
		ToolMenu.Instance.PriorityScreen.Show(!hide);
		ColonyDiagnosticScreen.Instance.Show(!hide);
		PinnedResourcesPanel.Instance.Show(!hide);
		TopLeftControlScreen.Instance.Show(!hide);
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(!hide);
		}
		global::DateTime.Instance.Show(!hide);
		if (BuildWatermark.Instance != null)
		{
			BuildWatermark.Instance.Show(!hide);
		}
		PopFXManager.Instance.Show(!hide);
	}

	// Token: 0x060051B8 RID: 20920 RVA: 0x001D2A54 File Offset: 0x001D0C54
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x060051B9 RID: 20921 RVA: 0x001D2AAC File Offset: 0x001D0CAC
	protected override void OnActivate()
	{
		base.OnActivate();
		SelectTool.Instance.Select(null, false);
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		CameraController.Instance.DisableUserCameraControl = true;
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
		MusicManager.instance.PlaySong("Music_Victory_01_Message", false);
		base.StartCoroutine(this.ExpandPanel());
	}

	// Token: 0x060051BA RID: 20922 RVA: 0x001D2B60 File Offset: 0x001D0D60
	protected override void OnDeactivate()
	{
		base.IsActive();
		base.OnDeactivate();
		MusicManager.instance.StopSong("Music_Victory_01_Message", true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (this.restoreInterfaceOnClose)
		{
			CameraController.Instance.DisableUserCameraControl = false;
			CameraController.Instance.FadeIn(0f, 1f, null);
			StoryMessageScreen.HideInterface(false);
		}
	}

	// Token: 0x060051BB RID: 20923 RVA: 0x001D2BB9 File Offset: 0x001D0DB9
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			base.StartCoroutine(this.CollapsePanel());
		}
		e.Consumed = true;
	}

	// Token: 0x060051BC RID: 20924 RVA: 0x001D2BD8 File Offset: 0x001D0DD8
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x040035A3 RID: 13731
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x040035A4 RID: 13732
	[SerializeField]
	private Image bg;

	// Token: 0x040035A5 RID: 13733
	[SerializeField]
	private GameObject dialog;

	// Token: 0x040035A6 RID: 13734
	[SerializeField]
	private KButton button;

	// Token: 0x040035A7 RID: 13735
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x040035A8 RID: 13736
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x040035A9 RID: 13737
	[SerializeField]
	private LocText bodyLabel;

	// Token: 0x040035AA RID: 13738
	private const float expandedHeight = 300f;

	// Token: 0x040035AB RID: 13739
	[SerializeField]
	private GameObject content;

	// Token: 0x040035AC RID: 13740
	public bool restoreInterfaceOnClose = true;

	// Token: 0x040035AD RID: 13741
	public System.Action OnClose;

	// Token: 0x040035AE RID: 13742
	private bool startFade;
}
