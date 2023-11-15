using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B8F RID: 2959
public class MetricsOptionsScreen : KModalScreen
{
	// Token: 0x06005BED RID: 23533 RVA: 0x0021A823 File Offset: 0x00218A23
	private bool IsSettingsDirty()
	{
		return this.disableDataCollection != KPrivacyPrefs.instance.disableDataCollection;
	}

	// Token: 0x06005BEE RID: 23534 RVA: 0x0021A83A File Offset: 0x00218A3A
	public override void OnKeyDown(KButtonEvent e)
	{
		if ((e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)) && !this.IsSettingsDirty())
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005BEF RID: 23535 RVA: 0x0021A864 File Offset: 0x00218A64
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.disableDataCollection = KPrivacyPrefs.instance.disableDataCollection;
		this.title.SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
		GameObject gameObject = this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.OnClickToggle();
		};
		this.enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
		this.dismissButton.onClick += delegate()
		{
			if (this.IsSettingsDirty())
			{
				this.ApplySettingsAndDoRestart();
				return;
			}
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.descriptionButton.onClick.AddListener(delegate()
		{
			App.OpenWebURL("https://www.kleientertainment.com/privacy-policy");
		});
		this.Refresh();
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x0021A968 File Offset: 0x00218B68
	private void OnClickToggle()
	{
		this.disableDataCollection = !this.disableDataCollection;
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(this.disableDataCollection);
		this.Refresh();
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x0021A9A4 File Offset: 0x00218BA4
	private void ApplySettingsAndDoRestart()
	{
		KPrivacyPrefs.instance.disableDataCollection = this.disableDataCollection;
		KPrivacyPrefs.Save();
		KPlayerPrefs.SetString("DisableDataCollection", KPrivacyPrefs.instance.disableDataCollection ? "yes" : "no");
		KPlayerPrefs.Save();
		ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
		App.instance.Restart();
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x0021AA38 File Offset: 0x00218C38
	private void Refresh()
	{
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").transform.GetChild(0).gameObject.SetActive(!this.disableDataCollection);
		this.closeButton.isInteractable = !this.IsSettingsDirty();
		this.restartWarningText.gameObject.SetActive(this.IsSettingsDirty());
		if (this.IsSettingsDirty())
		{
			this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.RESTART_BUTTON;
			return;
		}
		this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.DONE_BUTTON;
	}

	// Token: 0x04003DE7 RID: 15847
	public LocText title;

	// Token: 0x04003DE8 RID: 15848
	public KButton dismissButton;

	// Token: 0x04003DE9 RID: 15849
	public KButton closeButton;

	// Token: 0x04003DEA RID: 15850
	public GameObject enableButton;

	// Token: 0x04003DEB RID: 15851
	public Button descriptionButton;

	// Token: 0x04003DEC RID: 15852
	public LocText restartWarningText;

	// Token: 0x04003DED RID: 15853
	private bool disableDataCollection;
}
