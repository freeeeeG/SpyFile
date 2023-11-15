using System;
using System.IO;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
public class SaveScreen : KModalScreen
{
	// Token: 0x06005180 RID: 20864 RVA: 0x001CF77C File Offset: 0x001CD97C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.oldSaveButtonPrefab.gameObject.SetActive(false);
		this.newSaveButton.onClick += this.OnClickNewSave;
		this.closeButton.onClick += this.Deactivate;
	}

	// Token: 0x06005181 RID: 20865 RVA: 0x001CF7D0 File Offset: 0x001CD9D0
	protected override void OnCmpEnable()
	{
		foreach (SaveLoader.SaveFileEntry saveFileEntry in SaveLoader.GetAllColonyFiles(true, SearchOption.TopDirectoryOnly))
		{
			this.AddExistingSaveFile(saveFileEntry.path);
		}
		SpeedControlScreen.Instance.Pause(true, false);
	}

	// Token: 0x06005182 RID: 20866 RVA: 0x001CF838 File Offset: 0x001CDA38
	protected override void OnDeactivate()
	{
		SpeedControlScreen.Instance.Unpause(true);
		base.OnDeactivate();
	}

	// Token: 0x06005183 RID: 20867 RVA: 0x001CF84C File Offset: 0x001CDA4C
	private void AddExistingSaveFile(string filename)
	{
		KButton kbutton = Util.KInstantiateUI<KButton>(this.oldSaveButtonPrefab.gameObject, this.oldSavesRoot.gameObject, true);
		HierarchyReferences component = kbutton.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("Title").GetComponent<LocText>();
		TMP_Text component3 = component.GetReference<RectTransform>("Date").GetComponent<LocText>();
		System.DateTime lastWriteTime = File.GetLastWriteTime(filename);
		component2.text = string.Format("{0}", Path.GetFileNameWithoutExtension(filename));
		component3.text = string.Format("{0:H:mm:ss}" + Localization.GetFileDateFormat(0), lastWriteTime);
		kbutton.onClick += delegate()
		{
			this.Save(filename);
		};
	}

	// Token: 0x06005184 RID: 20868 RVA: 0x001CF908 File Offset: 0x001CDB08
	public static string GetValidSaveFilename(string filename)
	{
		string text = ".sav";
		if (Path.GetExtension(filename).ToLower() != text)
		{
			filename += text;
		}
		return filename;
	}

	// Token: 0x06005185 RID: 20869 RVA: 0x001CF938 File Offset: 0x001CDB38
	public void Save(string filename)
	{
		filename = SaveScreen.GetValidSaveFilename(filename);
		if (File.Exists(filename))
		{
			ScreenPrefabs.Instance.ConfirmDoAction(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
			}, base.transform.parent);
			return;
		}
		this.DoSave(filename);
	}

	// Token: 0x06005186 RID: 20870 RVA: 0x001CF9C0 File Offset: 0x001CDBC0
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			this.Deactivate();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, null, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x06005187 RID: 20871 RVA: 0x001CFA78 File Offset: 0x001CDC78
	public void OnClickNewSave()
	{
		FileNameDialog fileNameDialog = (FileNameDialog)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.FileNameDialog.gameObject, base.transform.parent.gameObject);
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath != null)
		{
			string text = SaveLoader.GetOriginalSaveFileName(activeSaveFilePath);
			text = Path.GetFileNameWithoutExtension(text);
			fileNameDialog.SetTextAndSelect(text);
		}
		fileNameDialog.onConfirm = delegate(string filename)
		{
			filename = Path.Combine(SaveLoader.GetActiveSaveColonyFolder(), filename);
			this.Save(filename);
		};
	}

	// Token: 0x06005188 RID: 20872 RVA: 0x001CFAE4 File Offset: 0x001CDCE4
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		e.Consumed = true;
	}

	// Token: 0x06005189 RID: 20873 RVA: 0x001CFAFC File Offset: 0x001CDCFC
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x0400357E RID: 13694
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400357F RID: 13695
	[SerializeField]
	private KButton newSaveButton;

	// Token: 0x04003580 RID: 13696
	[SerializeField]
	private KButton oldSaveButtonPrefab;

	// Token: 0x04003581 RID: 13697
	[SerializeField]
	private Transform oldSavesRoot;
}
