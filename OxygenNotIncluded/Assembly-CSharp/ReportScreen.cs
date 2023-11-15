using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BCD RID: 3021
public class ReportScreen : KScreen
{
	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06005EEF RID: 24303 RVA: 0x0022D8C8 File Offset: 0x0022BAC8
	// (set) Token: 0x06005EF0 RID: 24304 RVA: 0x0022D8CF File Offset: 0x0022BACF
	public static ReportScreen Instance { get; private set; }

	// Token: 0x06005EF1 RID: 24305 RVA: 0x0022D8D7 File Offset: 0x0022BAD7
	public static void DestroyInstance()
	{
		ReportScreen.Instance = null;
	}

	// Token: 0x06005EF2 RID: 24306 RVA: 0x0022D8E0 File Offset: 0x0022BAE0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ReportScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.prevButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day - 1);
		};
		this.nextButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day + 1);
		};
		this.summaryButton.onClick += delegate()
		{
			RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
		};
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06005EF3 RID: 24307 RVA: 0x0022D982 File Offset: 0x0022BB82
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005EF4 RID: 24308 RVA: 0x0022D98A File Offset: 0x0022BB8A
	protected override void OnShow(bool bShow)
	{
		base.OnShow(bShow);
		if (ReportManager.Instance != null)
		{
			this.currentReport = ReportManager.Instance.TodaysReport;
		}
	}

	// Token: 0x06005EF5 RID: 24309 RVA: 0x0022D9B0 File Offset: 0x0022BBB0
	public void SetTitle(string title)
	{
		this.title.text = title;
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x0022D9BE File Offset: 0x0022BBBE
	public override void ScreenUpdate(bool b)
	{
		base.ScreenUpdate(b);
		this.Refresh();
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x0022D9D0 File Offset: 0x0022BBD0
	private void Refresh()
	{
		global::Debug.Assert(this.currentReport != null);
		if (this.currentReport.day == ReportManager.Instance.TodaysReport.day)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_TODAY, this.currentReport.day));
		}
		else if (this.currentReport.day == ReportManager.Instance.TodaysReport.day - 1)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_YESTERDAY, this.currentReport.day));
		}
		else
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day));
		}
		bool flag = this.currentReport.day < ReportManager.Instance.TodaysReport.day;
		this.nextButton.isInteractable = flag;
		if (flag)
		{
			this.nextButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day + 1);
			this.nextButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.nextButton.GetComponent<ToolTip>().enabled = false;
		}
		flag = (this.currentReport.day > 1);
		this.prevButton.isInteractable = flag;
		if (flag)
		{
			this.prevButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day - 1);
			this.prevButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.prevButton.GetComponent<ToolTip>().enabled = false;
		}
		this.AddSpacer(0);
		int num = 1;
		foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in ReportManager.Instance.ReportGroups)
		{
			ReportManager.ReportEntry entry = this.currentReport.GetEntry(keyValuePair.Key);
			if (num != keyValuePair.Value.group)
			{
				num = keyValuePair.Value.group;
				this.AddSpacer(num);
			}
			bool flag2 = entry.accumulate != 0f || keyValuePair.Value.reportIfZero;
			if (keyValuePair.Value.isHeader)
			{
				this.CreateHeader(keyValuePair.Value);
			}
			else if (flag2)
			{
				this.CreateOrUpdateLine(entry, keyValuePair.Value, flag2);
			}
		}
	}

	// Token: 0x06005EF8 RID: 24312 RVA: 0x0022DC6C File Offset: 0x0022BE6C
	public void ShowReport(int day)
	{
		this.currentReport = ReportManager.Instance.FindReport(day);
		global::Debug.Assert(this.currentReport != null, "Can't find report for day: " + day.ToString());
		this.Refresh();
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x0022DCA4 File Offset: 0x0022BEA4
	private GameObject AddSpacer(int group)
	{
		GameObject gameObject;
		if (this.lineItems.ContainsKey(group.ToString()))
		{
			gameObject = this.lineItems[group.ToString()];
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.lineItemSpacer, this.contentFolder, false);
			gameObject.name = "Spacer" + group.ToString();
			this.lineItems[group.ToString()] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005EFA RID: 24314 RVA: 0x0022DD24 File Offset: 0x0022BF24
	private GameObject CreateHeader(ReportManager.ReportGroup reportGroup)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (gameObject == null)
		{
			gameObject = Util.KInstantiateUI(this.lineItemHeader, this.contentFolder, true);
			gameObject.name = "LineItemHeader" + this.lineItems.Count.ToString();
			this.lineItems[reportGroup.stringKey] = gameObject;
		}
		gameObject.SetActive(true);
		gameObject.GetComponent<ReportScreenHeader>().SetMainEntry(reportGroup);
		return gameObject;
	}

	// Token: 0x06005EFB RID: 24315 RVA: 0x0022DDAC File Offset: 0x0022BFAC
	private GameObject CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (!is_line_active)
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if (gameObject == null)
			{
				gameObject = Util.KInstantiateUI(this.lineItem, this.contentFolder, true);
				gameObject.name = "LineItem" + this.lineItems.Count.ToString();
				this.lineItems[reportGroup.stringKey] = gameObject;
			}
			gameObject.SetActive(true);
			gameObject.GetComponent<ReportScreenEntry>().SetMainEntry(entry, reportGroup);
		}
		return gameObject;
	}

	// Token: 0x06005EFC RID: 24316 RVA: 0x0022DE52 File Offset: 0x0022C052
	private void OnClickClose()
	{
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Close", false));
		this.Show(false);
	}

	// Token: 0x04004026 RID: 16422
	[SerializeField]
	private LocText title;

	// Token: 0x04004027 RID: 16423
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004028 RID: 16424
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04004029 RID: 16425
	[SerializeField]
	private KButton nextButton;

	// Token: 0x0400402A RID: 16426
	[SerializeField]
	private KButton summaryButton;

	// Token: 0x0400402B RID: 16427
	[SerializeField]
	private GameObject lineItem;

	// Token: 0x0400402C RID: 16428
	[SerializeField]
	private GameObject lineItemSpacer;

	// Token: 0x0400402D RID: 16429
	[SerializeField]
	private GameObject lineItemHeader;

	// Token: 0x0400402E RID: 16430
	[SerializeField]
	private GameObject contentFolder;

	// Token: 0x0400402F RID: 16431
	private Dictionary<string, GameObject> lineItems = new Dictionary<string, GameObject>();

	// Token: 0x04004030 RID: 16432
	private ReportManager.DailyReport currentReport;
}
