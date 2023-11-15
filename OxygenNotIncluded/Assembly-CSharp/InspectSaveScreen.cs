using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A69 RID: 2665
public class InspectSaveScreen : KModalScreen
{
	// Token: 0x06005096 RID: 20630 RVA: 0x001C8D4D File Offset: 0x001C6F4D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += this.CloseScreen;
		this.deleteSaveBtn.onClick += this.DeleteSave;
	}

	// Token: 0x06005097 RID: 20631 RVA: 0x001C8D83 File Offset: 0x001C6F83
	private void CloseScreen()
	{
		LoadScreen.Instance.Show(true);
		this.Show(false);
	}

	// Token: 0x06005098 RID: 20632 RVA: 0x001C8D97 File Offset: 0x001C6F97
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			this.buttonPool.ClearAll();
			this.buttonFileMap.Clear();
		}
	}

	// Token: 0x06005099 RID: 20633 RVA: 0x001C8DBC File Offset: 0x001C6FBC
	public void SetTarget(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogError("The directory path provided is empty.");
			this.Show(false);
			return;
		}
		if (!Directory.Exists(path))
		{
			global::Debug.LogError("The directory provided does not exist.");
			this.Show(false);
			return;
		}
		if (this.buttonPool == null)
		{
			this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
		}
		this.currentPath = path;
		List<string> list = (from filename in Directory.GetFiles(path)
		where Path.GetExtension(filename).ToLower() == ".sav"
		orderby File.GetLastWriteTime(filename) descending
		select filename).ToList<string>();
		string text = list[0];
		if (File.Exists(text))
		{
			this.mainSaveBtn.gameObject.SetActive(true);
			this.AddNewSave(this.mainSaveBtn, text);
		}
		else
		{
			this.mainSaveBtn.gameObject.SetActive(false);
		}
		if (list.Count > 1)
		{
			for (int i = 1; i < list.Count; i++)
			{
				this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[i]);
			}
		}
		this.Show(true);
	}

	// Token: 0x0600509A RID: 20634 RVA: 0x001C8EF4 File Offset: 0x001C70F4
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600509B RID: 20635 RVA: 0x001C8F84 File Offset: 0x001C7184
	private void DeleteSave()
	{
		if (string.IsNullOrEmpty(this.currentPath))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, delegate
		{
			string[] files = Directory.GetFiles(this.currentPath);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			Directory.Delete(this.currentPath);
			this.CloseScreen();
		});
	}

	// Token: 0x0600509C RID: 20636 RVA: 0x001C8FBA File Offset: 0x001C71BA
	private void AddNewSave(KButton btn, string file)
	{
	}

	// Token: 0x0600509D RID: 20637 RVA: 0x001C8FBC File Offset: 0x001C71BC
	private void ButtonClicked(KButton btn)
	{
		LoadingOverlay.Load(delegate
		{
			this.Load(this.buttonFileMap[btn]);
		});
	}

	// Token: 0x0600509E RID: 20638 RVA: 0x001C8FE1 File Offset: 0x001C71E1
	private void Load(string filename)
	{
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		App.LoadScene("backend");
		this.Deactivate();
	}

	// Token: 0x0600509F RID: 20639 RVA: 0x001C900B File Offset: 0x001C720B
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.CloseScreen();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x040034CA RID: 13514
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040034CB RID: 13515
	[SerializeField]
	private KButton mainSaveBtn;

	// Token: 0x040034CC RID: 13516
	[SerializeField]
	private KButton backupBtnPrefab;

	// Token: 0x040034CD RID: 13517
	[SerializeField]
	private KButton deleteSaveBtn;

	// Token: 0x040034CE RID: 13518
	[SerializeField]
	private GameObject buttonGroup;

	// Token: 0x040034CF RID: 13519
	private UIPool<KButton> buttonPool;

	// Token: 0x040034D0 RID: 13520
	private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();

	// Token: 0x040034D1 RID: 13521
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x040034D2 RID: 13522
	private string currentPath = "";
}
