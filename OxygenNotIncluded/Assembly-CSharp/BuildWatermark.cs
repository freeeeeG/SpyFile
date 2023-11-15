using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class BuildWatermark : KScreen
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002964 File Offset: 0x00000B64
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildWatermark.Instance = this;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002972 File Offset: 0x00000B72
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshText();
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002980 File Offset: 0x00000B80
	public static string GetBuildText()
	{
		string text = DistributionPlatform.Initialized ? (LaunchInitializer.BuildPrefix() + "-") : "??-";
		if (Application.isEditor)
		{
			text += "<EDITOR>";
		}
		else
		{
			text += 577063U.ToString();
			if (DistributionPlatform.Initialized)
			{
				text = text + "-" + DlcManager.GetActiveContentLetters();
			}
			else
			{
				text += "-?";
			}
			if (DebugHandler.enabled)
			{
				text += "D";
			}
		}
		return text;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002A10 File Offset: 0x00000C10
	public void RefreshText()
	{
		bool flag = true;
		bool flag2 = DistributionPlatform.Initialized && DistributionPlatform.Inst.IsArchiveBranch;
		string buildText = BuildWatermark.GetBuildText();
		this.button.ClearOnClick();
		if (flag)
		{
			this.textDisplay.SetText(string.Format(UI.DEVELOPMENTBUILDS.WATERMARK, buildText));
			this.toolTip.ClearMultiStringTooltip();
		}
		else
		{
			this.textDisplay.SetText(string.Format(UI.DEVELOPMENTBUILDS.TESTING_WATERMARK, buildText));
			this.toolTip.SetSimpleTooltip(UI.DEVELOPMENTBUILDS.TESTING_TOOLTIP);
			if (this.interactable)
			{
				this.button.onClick += this.ShowTestingMessage;
			}
		}
		foreach (GameObject gameObject in this.archiveIcons)
		{
			gameObject.SetActive(flag && flag2);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002B08 File Offset: 0x00000D08
	private void ShowTestingMessage()
	{
		Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, Global.Instance.globalCanvas, true).PopupConfirmDialog(UI.DEVELOPMENTBUILDS.TESTING_MESSAGE, delegate
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}, delegate
		{
		}, null, null, UI.DEVELOPMENTBUILDS.TESTING_MESSAGE_TITLE, UI.DEVELOPMENTBUILDS.TESTING_MORE_INFO, null, null);
	}

	// Token: 0x04000021 RID: 33
	public bool interactable = true;

	// Token: 0x04000022 RID: 34
	public LocText textDisplay;

	// Token: 0x04000023 RID: 35
	public ToolTip toolTip;

	// Token: 0x04000024 RID: 36
	public KButton button;

	// Token: 0x04000025 RID: 37
	public List<GameObject> archiveIcons;

	// Token: 0x04000026 RID: 38
	public static BuildWatermark Instance;
}
