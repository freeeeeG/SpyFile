using System;
using System.Collections.Generic;
using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BCC RID: 3020
public class ReportErrorDialog : MonoBehaviour
{
	// Token: 0x06005EDC RID: 24284 RVA: 0x0022D2B8 File Offset: 0x0022B4B8
	private void Start()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(true);
		}
		this.StackTrace.SetActive(false);
		this.CrashLabel.text = ((this.mode == ReportErrorDialog.Mode.SubmitError) ? UI.CRASHSCREEN.TITLE : UI.CRASHSCREEN.TITLE_MODS);
		this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
		this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
		if (this.mode == ReportErrorDialog.Mode.DisableMods)
		{
			this.BuildModsList();
		}
		this.submitButton.gameObject.SetActive(this.submitAction != null);
		this.submitButton.onClick += this.OnSelect_SUBMIT;
		this.moreInfoButton.onClick += this.OnSelect_MOREINFO;
		this.continueGameButton.gameObject.SetActive(this.continueAction != null);
		this.continueGameButton.onClick += this.OnSelect_CONTINUE;
		this.quitButton.onClick += this.OnSelect_QUIT;
		this.messageInputField.text = UI.CRASHSCREEN.BODY;
		KCrashReporter.onCrashReported += this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress += this.UpdateProgressBar;
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x0022D414 File Offset: 0x0022B614
	private void BuildModsList()
	{
		DebugUtil.Assert(Global.Instance != null && Global.Instance.modManager != null);
		Manager mod_mgr = Global.Instance.modManager;
		List<Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
		allCrashableMods.Sort((Mod x, Mod y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace));
		foreach (Mod mod in allCrashableMods)
		{
			if (mod.foundInStackTrace && mod.label.distribution_platform != Label.DistributionPlatform.Dev)
			{
				mod_mgr.EnableMod(mod.label, false, this);
			}
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject, false);
			LocText reference = hierarchyReferences.GetReference<LocText>("Title");
			reference.text = mod.title;
			reference.color = (mod.foundInStackTrace ? Color.red : Color.white);
			MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
			toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
			Label mod_label = mod.label;
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				bool flag = !mod_mgr.IsModEnabled(mod_label);
				toggle.ChangeState(flag ? 1 : 0);
				mod_mgr.EnableMod(mod_label, flag, this);
			}));
			toggle.GetComponent<ToolTip>().OnToolTip = (() => mod_mgr.IsModEnabled(mod_label) ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
			hierarchyReferences.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005EDE RID: 24286 RVA: 0x0022D5E8 File Offset: 0x0022B7E8
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x06005EDF RID: 24287 RVA: 0x0022D5F0 File Offset: 0x0022B7F0
	private void OnDestroy()
	{
		if (KCrashReporter.terminateOnError)
		{
			App.Quit();
		}
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(false);
		}
		KCrashReporter.onCrashReported -= this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress -= this.UpdateProgressBar;
	}

	// Token: 0x06005EE0 RID: 24288 RVA: 0x0022D642 File Offset: 0x0022B842
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_QUIT();
		}
	}

	// Token: 0x06005EE1 RID: 24289 RVA: 0x0022D653 File Offset: 0x0022B853
	public void PopupSubmitErrorDialog(string stackTrace, System.Action onSubmit, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.SubmitError;
		this.m_stackTrace = stackTrace;
		this.submitAction = onSubmit;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x06005EE2 RID: 24290 RVA: 0x0022D679 File Offset: 0x0022B879
	public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.DisableMods;
		this.m_stackTrace = stackTrace;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x0022D698 File Offset: 0x0022B898
	public void OnSelect_MOREINFO()
	{
		this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
		this.StackTrace.SetActive(true);
		this.moreInfoButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
		this.moreInfoButton.ClearOnClick();
		this.moreInfoButton.onClick += this.OnSelect_COPYTOCLIPBOARD;
	}

	// Token: 0x06005EE4 RID: 24292 RVA: 0x0022D703 File Offset: 0x0022B903
	public void OnSelect_COPYTOCLIPBOARD()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.m_stackTrace + "\nBuild: " + BuildWatermark.GetBuildText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x06005EE5 RID: 24293 RVA: 0x0022D730 File Offset: 0x0022B930
	public void OnSelect_SUBMIT()
	{
		this.submitButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.REPORTING;
		this.submitButton.GetComponent<KButton>().isInteractable = false;
		this.Submit();
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x0022D763 File Offset: 0x0022B963
	public void OnSelect_QUIT()
	{
		if (this.quitAction != null)
		{
			this.quitAction();
		}
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x0022D778 File Offset: 0x0022B978
	public void OnSelect_CONTINUE()
	{
		if (this.continueAction != null)
		{
			this.continueAction();
		}
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x0022D790 File Offset: 0x0022B990
	public void OpenRefMessage(bool success)
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(false);
		this.referenceMessage.SetActive(true);
		this.messageText.text = (success ? UI.CRASHSCREEN.THANKYOU : UI.CRASHSCREEN.UPLOAD_FAILED);
		this.m_crashSubmitted = success;
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x0022D7EC File Offset: 0x0022B9EC
	public void OpenUploadingMessagee()
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(true);
		this.referenceMessage.SetActive(false);
		this.progressBar.fillAmount = 0f;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0f, GameUtil.TimeSlice.None));
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x0022D857 File Offset: 0x0022BA57
	public void OnSelect_MESSAGE()
	{
		if (!this.m_crashSubmitted)
		{
			Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}
	}

	// Token: 0x06005EEB RID: 24299 RVA: 0x0022D86B File Offset: 0x0022BA6B
	public string UserMessage()
	{
		return this.messageInputField.text;
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x0022D878 File Offset: 0x0022BA78
	private void Submit()
	{
		this.submitAction();
		this.OpenUploadingMessagee();
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x0022D88B File Offset: 0x0022BA8B
	public void UpdateProgressBar(float progress)
	{
		this.progressBar.fillAmount = progress;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x04004010 RID: 16400
	private System.Action submitAction;

	// Token: 0x04004011 RID: 16401
	private System.Action quitAction;

	// Token: 0x04004012 RID: 16402
	private System.Action continueAction;

	// Token: 0x04004013 RID: 16403
	public KInputTextField messageInputField;

	// Token: 0x04004014 RID: 16404
	[Header("Message")]
	public GameObject referenceMessage;

	// Token: 0x04004015 RID: 16405
	public LocText messageText;

	// Token: 0x04004016 RID: 16406
	[Header("Upload Progress")]
	public GameObject uploadInProgress;

	// Token: 0x04004017 RID: 16407
	public Image progressBar;

	// Token: 0x04004018 RID: 16408
	public LocText progressText;

	// Token: 0x04004019 RID: 16409
	private string m_stackTrace;

	// Token: 0x0400401A RID: 16410
	private bool m_crashSubmitted;

	// Token: 0x0400401B RID: 16411
	[SerializeField]
	private KButton submitButton;

	// Token: 0x0400401C RID: 16412
	[SerializeField]
	private KButton moreInfoButton;

	// Token: 0x0400401D RID: 16413
	[SerializeField]
	private KButton quitButton;

	// Token: 0x0400401E RID: 16414
	[SerializeField]
	private KButton continueGameButton;

	// Token: 0x0400401F RID: 16415
	[SerializeField]
	private LocText CrashLabel;

	// Token: 0x04004020 RID: 16416
	[SerializeField]
	private GameObject CrashDescription;

	// Token: 0x04004021 RID: 16417
	[SerializeField]
	private GameObject ModsInfo;

	// Token: 0x04004022 RID: 16418
	[SerializeField]
	private GameObject StackTrace;

	// Token: 0x04004023 RID: 16419
	[SerializeField]
	private GameObject modEntryPrefab;

	// Token: 0x04004024 RID: 16420
	[SerializeField]
	private Transform modEntryParent;

	// Token: 0x04004025 RID: 16421
	private ReportErrorDialog.Mode mode;

	// Token: 0x02001B0E RID: 6926
	private enum Mode
	{
		// Token: 0x04007B95 RID: 31637
		SubmitError,
		// Token: 0x04007B96 RID: 31638
		DisableMods
	}
}
