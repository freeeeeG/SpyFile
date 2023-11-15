using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A6C RID: 2668
public class LoadingOverlay : KModalScreen
{
	// Token: 0x060050E5 RID: 20709 RVA: 0x001CB842 File Offset: 0x001C9A42
	protected override void OnPrefabInit()
	{
		this.pause = false;
		this.fadeIn = false;
		base.OnPrefabInit();
	}

	// Token: 0x060050E6 RID: 20710 RVA: 0x001CB858 File Offset: 0x001C9A58
	private void Update()
	{
		if (!this.loadNextFrame && this.showLoad)
		{
			this.loadNextFrame = true;
			this.showLoad = false;
			return;
		}
		if (this.loadNextFrame)
		{
			this.loadNextFrame = false;
			this.loadCb();
		}
	}

	// Token: 0x060050E7 RID: 20711 RVA: 0x001CB893 File Offset: 0x001C9A93
	public static void DestroyInstance()
	{
		LoadingOverlay.instance = null;
	}

	// Token: 0x060050E8 RID: 20712 RVA: 0x001CB89C File Offset: 0x001C9A9C
	public static void Load(System.Action cb)
	{
		GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
		if (LoadingOverlay.instance == null)
		{
			LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, (GameScreenManager.Instance == null) ? gameObject : GameScreenManager.Instance.ssOverlayCanvas, false);
			LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.LOADING);
		}
		if (GameScreenManager.Instance != null)
		{
			LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
		}
		else
		{
			LoadingOverlay.instance.transform.SetParent(gameObject.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
			if (MainMenu.Instance != null)
			{
				MainMenu.Instance.StopAmbience();
			}
		}
		LoadingOverlay.instance.loadCb = cb;
		LoadingOverlay.instance.showLoad = true;
		LoadingOverlay.instance.Activate();
	}

	// Token: 0x060050E9 RID: 20713 RVA: 0x001CB9C8 File Offset: 0x001C9BC8
	public static void Clear()
	{
		if (LoadingOverlay.instance != null)
		{
			LoadingOverlay.instance.Deactivate();
		}
	}

	// Token: 0x040034F0 RID: 13552
	private bool loadNextFrame;

	// Token: 0x040034F1 RID: 13553
	private bool showLoad;

	// Token: 0x040034F2 RID: 13554
	private System.Action loadCb;

	// Token: 0x040034F3 RID: 13555
	private static LoadingOverlay instance;
}
