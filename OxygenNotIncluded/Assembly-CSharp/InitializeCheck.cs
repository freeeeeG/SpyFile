using System;
using System.IO;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000806 RID: 2054
public class InitializeCheck : MonoBehaviour
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06003ADF RID: 15071 RVA: 0x00146EAC File Offset: 0x001450AC
	// (set) Token: 0x06003AE0 RID: 15072 RVA: 0x00146EB3 File Offset: 0x001450B3
	public static InitializeCheck.SavePathIssue savePathState { get; private set; }

	// Token: 0x06003AE1 RID: 15073 RVA: 0x00146EBC File Offset: 0x001450BC
	private void Awake()
	{
		this.CheckForSavePathIssue();
		if (InitializeCheck.savePathState == InitializeCheck.SavePathIssue.Ok && !KCrashReporter.hasCrash)
		{
			AudioMixer.Create();
			App.LoadScene("frontend");
			return;
		}
		Canvas cmp = base.gameObject.AddComponent<Canvas>();
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);
		Camera camera = base.gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = 200f;
		camera.backgroundColor = Color.black;
		camera.clearFlags = CameraClearFlags.Color;
		camera.nearClipPlane = 0f;
		global::Debug.Log("Cannot initialize filesystem. [" + InitializeCheck.savePathState.ToString() + "]");
		Localization.Initialize();
		GameObject.Find("BootCanvas").SetActive(false);
		this.ShowFileErrorDialogs();
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x00146F95 File Offset: 0x00145195
	private GameObject CreateUIRoot()
	{
		return Util.KInstantiate(this.rootCanvasPrefab, null, "CanvasRoot");
	}

	// Token: 0x06003AE3 RID: 15075 RVA: 0x00146FA8 File Offset: 0x001451A8
	private void ShowErrorDialog(string msg)
	{
		GameObject parent = this.CreateUIRoot();
		Util.KInstantiateUI<ConfirmDialogScreen>(this.confirmDialogScreen.gameObject, parent, true).PopupConfirmDialog(msg, new System.Action(this.Quit), null, null, null, null, null, null, this.sadDupe);
	}

	// Token: 0x06003AE4 RID: 15076 RVA: 0x00146FEC File Offset: 0x001451EC
	private void ShowFileErrorDialogs()
	{
		string text = null;
		switch (InitializeCheck.savePathState)
		{
		case InitializeCheck.SavePathIssue.WriteTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.SpaceTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.WorldGenFilesFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FILES, WorldGen.WORLDGEN_SAVE_FILENAME + "\n" + WorldGen.GetSIMSaveFilename(-1));
			break;
		}
		if (text != null)
		{
			this.ShowErrorDialog(text);
		}
	}

	// Token: 0x06003AE5 RID: 15077 RVA: 0x00147074 File Offset: 0x00145274
	private void CheckForSavePathIssue()
	{
		if (this.test_issue != InitializeCheck.SavePathIssue.Ok)
		{
			InitializeCheck.savePathState = this.test_issue;
			return;
		}
		string savePrefix = SaveLoader.GetSavePrefix();
		InitializeCheck.savePathState = InitializeCheck.SavePathIssue.Ok;
		try
		{
			SaveLoader.GetSavePrefixAndCreateFolder();
			using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testFile, FileMode.Create, FileAccess.Write))
			{
				new BinaryWriter(fileStream);
				fileStream.Close();
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WriteTestFail;
			goto IL_E7;
		}
		using (FileStream fileStream2 = File.Open(savePrefix + InitializeCheck.testSave, FileMode.Create, FileAccess.Write))
		{
			try
			{
				fileStream2.SetLength(15000000L);
				new BinaryWriter(fileStream2);
				fileStream2.Close();
			}
			catch
			{
				fileStream2.Close();
				InitializeCheck.savePathState = InitializeCheck.SavePathIssue.SpaceTestFail;
				goto IL_E7;
			}
		}
		try
		{
			using (File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Append))
			{
			}
			using (File.Open(WorldGen.GetSIMSaveFilename(-1), FileMode.Append))
			{
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WorldGenFilesFail;
		}
		IL_E7:
		try
		{
			if (File.Exists(savePrefix + InitializeCheck.testFile))
			{
				File.Delete(savePrefix + InitializeCheck.testFile);
			}
			if (File.Exists(savePrefix + InitializeCheck.testSave))
			{
				File.Delete(savePrefix + InitializeCheck.testSave);
			}
		}
		catch
		{
		}
	}

	// Token: 0x06003AE6 RID: 15078 RVA: 0x00147218 File Offset: 0x00145418
	private void Quit()
	{
		global::Debug.Log("Quitting...");
		App.Quit();
	}

	// Token: 0x040026FF RID: 9983
	private static readonly string testFile = "testfile";

	// Token: 0x04002700 RID: 9984
	private static readonly string testSave = "testsavefile";

	// Token: 0x04002701 RID: 9985
	public Canvas rootCanvasPrefab;

	// Token: 0x04002702 RID: 9986
	public ConfirmDialogScreen confirmDialogScreen;

	// Token: 0x04002703 RID: 9987
	public Sprite sadDupe;

	// Token: 0x04002704 RID: 9988
	private InitializeCheck.SavePathIssue test_issue;

	// Token: 0x020015E3 RID: 5603
	public enum SavePathIssue
	{
		// Token: 0x040069D2 RID: 27090
		Ok,
		// Token: 0x040069D3 RID: 27091
		WriteTestFail,
		// Token: 0x040069D4 RID: 27092
		SpaceTestFail,
		// Token: 0x040069D5 RID: 27093
		WorldGenFilesFail
	}
}
