﻿using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BE0 RID: 3040
public class ScenariosMenu : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x0600602E RID: 24622 RVA: 0x00238F18 File Offset: 0x00237118
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.RebuildScreen();
	}

	// Token: 0x0600602F RID: 24623 RVA: 0x00238F9C File Offset: 0x0023719C
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.RebuildUGCButtons();
	}

	// Token: 0x06006030 RID: 24624 RVA: 0x00239000 File Offset: 0x00237200
	private void RebuildUGCButtons()
	{
		ListPool<SteamUGCService.Mod, ScenariosMenu>.PooledList pooledList = ListPool<SteamUGCService.Mod, ScenariosMenu>.Allocate();
		bool flag = pooledList.Count > 0;
		this.noScenariosText.gameObject.SetActive(!flag);
		this.contentRoot.gameObject.SetActive(flag);
		bool flag2 = true;
		if (pooledList.Count != 0)
		{
			for (int i = 0; i < pooledList.Count; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.ugcButtonPrefab, this.ugcContainer, false);
				gameObject.name = pooledList[i].title + "_button";
				gameObject.gameObject.SetActive(true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Title").SetText(pooledList[i].title);
				Texture2D previewImage = pooledList[i].previewImage;
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				KButton component2 = gameObject.GetComponent<KButton>();
				int index = i;
				PublishedFileId_t item = pooledList[index].fileId;
				component2.onClick += delegate()
				{
					this.ShowDetails(item);
				};
				component2.onDoubleClick += delegate()
				{
					this.LoadScenario(item);
				};
				this.buttons.Add(gameObject);
				if (item == this.activeItem)
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.HideDetails();
		}
		pooledList.Recycle();
	}

	// Token: 0x06006031 RID: 24625 RVA: 0x002391AC File Offset: 0x002373AC
	private void LoadScenario(PublishedFileId_t item)
	{
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		DebugUtil.LogArgs(new object[]
		{
			"LoadScenario",
			text,
			num,
			num2
		});
		System.DateTime dateTime;
		byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, new string[]
		{
			".sav"
		}, out dateTime, false);
		string text2 = Path.Combine(SaveLoader.GetSavePrefix(), "scenario.sav");
		File.WriteAllBytes(text2, bytesFromZip);
		SaveLoader.SetActiveSaveFilePath(text2);
		Time.timeScale = 0f;
		App.LoadScene("backend");
	}

	// Token: 0x06006032 RID: 24626 RVA: 0x0023923D File Offset: 0x0023743D
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x06006033 RID: 24627 RVA: 0x00239274 File Offset: 0x00237474
	private void ShowDetails(PublishedFileId_t item)
	{
		this.activeItem = item;
		SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(item);
		if (mod != null)
		{
			this.scenarioTitle.text = mod.title;
			this.scenarioDetails.text = mod.description;
		}
		this.loadScenarioButton.onClick += delegate()
		{
			this.LoadScenario(item);
		};
		this.detailsRoot.gameObject.SetActive(true);
	}

	// Token: 0x06006034 RID: 24628 RVA: 0x002392FF File Offset: 0x002374FF
	private void HideDetails()
	{
		this.detailsRoot.gameObject.SetActive(false);
	}

	// Token: 0x06006035 RID: 24629 RVA: 0x00239312 File Offset: 0x00237512
	protected override void OnActivate()
	{
		base.OnActivate();
		SteamUGCService.Instance.AddClient(this);
		this.HideDetails();
	}

	// Token: 0x06006036 RID: 24630 RVA: 0x0023932B File Offset: 0x0023752B
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		SteamUGCService.Instance.RemoveClient(this);
	}

	// Token: 0x06006037 RID: 24631 RVA: 0x0023933E File Offset: 0x0023753E
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=scenario");
	}

	// Token: 0x06006038 RID: 24632 RVA: 0x0023934A File Offset: 0x0023754A
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		this.RebuildScreen();
	}

	// Token: 0x04004176 RID: 16758
	public const string TAG_SCENARIO = "scenario";

	// Token: 0x04004177 RID: 16759
	public KButton textButton;

	// Token: 0x04004178 RID: 16760
	public KButton dismissButton;

	// Token: 0x04004179 RID: 16761
	public KButton closeButton;

	// Token: 0x0400417A RID: 16762
	public KButton workshopButton;

	// Token: 0x0400417B RID: 16763
	public KButton loadScenarioButton;

	// Token: 0x0400417C RID: 16764
	[Space]
	public GameObject ugcContainer;

	// Token: 0x0400417D RID: 16765
	public GameObject ugcButtonPrefab;

	// Token: 0x0400417E RID: 16766
	public LocText noScenariosText;

	// Token: 0x0400417F RID: 16767
	public RectTransform contentRoot;

	// Token: 0x04004180 RID: 16768
	public RectTransform detailsRoot;

	// Token: 0x04004181 RID: 16769
	public LocText scenarioTitle;

	// Token: 0x04004182 RID: 16770
	public LocText scenarioDetails;

	// Token: 0x04004183 RID: 16771
	private PublishedFileId_t activeItem;

	// Token: 0x04004184 RID: 16772
	private List<GameObject> buttons = new List<GameObject>();
}
