﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000923 RID: 2339
[AddComponentMenu("KMonoBehaviour/scripts/Timelapser")]
public class Timelapser : KMonoBehaviour
{
	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060043CA RID: 17354 RVA: 0x0017BFAB File Offset: 0x0017A1AB
	public bool CapturingTimelapseScreenshot
	{
		get
		{
			return this.screenshotActive;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x060043CB RID: 17355 RVA: 0x0017BFB3 File Offset: 0x0017A1B3
	// (set) Token: 0x060043CC RID: 17356 RVA: 0x0017BFBB File Offset: 0x0017A1BB
	public Texture2D freezeTexture { get; private set; }

	// Token: 0x060043CD RID: 17357 RVA: 0x0017BFC4 File Offset: 0x0017A1C4
	protected override void OnPrefabInit()
	{
		this.RefreshRenderTextureSize(null);
		Game.Instance.Subscribe(75424175, new Action<object>(this.RefreshRenderTextureSize));
		this.freezeCamera = CameraController.Instance.timelapseFreezeCamera;
		if (this.CycleTimeToScreenshot() > 0f)
		{
			this.OnNewDay(null);
		}
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
		this.OnResize();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		base.StartCoroutine(this.Render());
	}

	// Token: 0x060043CE RID: 17358 RVA: 0x0017C06D File Offset: 0x0017A26D
	private void OnResize()
	{
		if (this.freezeTexture != null)
		{
			UnityEngine.Object.Destroy(this.freezeTexture);
		}
		this.freezeTexture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.ARGB32, false);
	}

	// Token: 0x060043CF RID: 17359 RVA: 0x0017C0AC File Offset: 0x0017A2AC
	private void RefreshRenderTextureSize(object data = null)
	{
		if (this.previewScreenshot)
		{
			if (this.bufferRenderTexture != null)
			{
				this.bufferRenderTexture.DestroyRenderTexture();
			}
			this.bufferRenderTexture = new RenderTexture(this.previewScreenshotResolution.x, this.previewScreenshotResolution.y, 32, RenderTextureFormat.ARGB32);
			this.bufferRenderTexture.name = "Timelapser.PreviewScreenshot";
			return;
		}
		if (this.timelapseUserEnabled)
		{
			if (this.bufferRenderTexture != null)
			{
				this.bufferRenderTexture.DestroyRenderTexture();
			}
			this.bufferRenderTexture = new RenderTexture(SaveGame.Instance.TimelapseResolution.x, SaveGame.Instance.TimelapseResolution.y, 32, RenderTextureFormat.ARGB32);
			this.bufferRenderTexture.name = "Timelapser.Timelapse";
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x060043D0 RID: 17360 RVA: 0x0017C16C File Offset: 0x0017A36C
	private bool timelapseUserEnabled
	{
		get
		{
			return SaveGame.Instance.TimelapseResolution.x > 0;
		}
	}

	// Token: 0x060043D1 RID: 17361 RVA: 0x0017C180 File Offset: 0x0017A380
	private void OnNewDay(object data = null)
	{
		DebugUtil.LogWarningArgs(new object[]
		{
			this.worldsToScreenshot.Count == 0,
			"Timelapse.OnNewDay but worldsToScreenshot is not empty"
		});
		int cycle = GameClock.Instance.GetCycle();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
			{
				if (worldContainer.DiscoveryTimestamp + (float)cycle > (float)this.timelapseScreenshotCycles[this.timelapseScreenshotCycles.Length - 1])
				{
					if (worldContainer.DiscoveryTimestamp + (float)(cycle % 10) == 0f)
					{
						this.screenshotToday = true;
						this.worldsToScreenshot.Add(worldContainer.id);
					}
				}
				else
				{
					for (int i = 0; i < this.timelapseScreenshotCycles.Length; i++)
					{
						if ((int)worldContainer.DiscoveryTimestamp + cycle == this.timelapseScreenshotCycles[i])
						{
							this.screenshotToday = true;
							this.worldsToScreenshot.Add(worldContainer.id);
						}
					}
				}
			}
		}
	}

	// Token: 0x060043D2 RID: 17362 RVA: 0x0017C2A4 File Offset: 0x0017A4A4
	private void Update()
	{
		if (this.screenshotToday)
		{
			if (this.CycleTimeToScreenshot() <= 0f || GameClock.Instance.GetCycle() == 0)
			{
				if (!this.timelapseUserEnabled)
				{
					this.screenshotToday = false;
					this.worldsToScreenshot.Clear();
					return;
				}
				if (!PlayerController.Instance.CanDrag())
				{
					CameraController.Instance.ForcePanningState(false);
					this.screenshotToday = false;
					this.SaveScreenshot();
					return;
				}
			}
		}
		else
		{
			this.screenshotToday = (!this.screenshotPending && this.worldsToScreenshot.Count > 0);
		}
	}

	// Token: 0x060043D3 RID: 17363 RVA: 0x0017C331 File Offset: 0x0017A531
	private float CycleTimeToScreenshot()
	{
		return 300f - GameClock.Instance.GetTime() % 600f;
	}

	// Token: 0x060043D4 RID: 17364 RVA: 0x0017C349 File Offset: 0x0017A549
	private IEnumerator Render()
	{
		for (;;)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			if (this.screenshotPending)
			{
				int num = this.previewScreenshot ? ClusterManager.Instance.GetStartWorld().id : this.worldsToScreenshot[0];
				if (!this.freezeCamera.enabled)
				{
					this.freezeTexture.ReadPixels(new Rect(0f, 0f, (float)Camera.main.pixelWidth, (float)Camera.main.pixelHeight), 0, 0);
					this.freezeTexture.Apply();
					this.freezeCamera.gameObject.GetComponent<FillRenderTargetEffect>().SetFillTexture(this.freezeTexture);
					this.freezeCamera.enabled = true;
					this.screenshotActive = true;
					this.RefreshRenderTextureSize(null);
					DebugHandler.SetTimelapseMode(true, num);
					this.SetPostionAndOrtho(num);
					this.activeOverlay = OverlayScreen.Instance.mode;
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
				}
				else
				{
					this.RenderAndPrint(num);
					if (!this.previewScreenshot)
					{
						this.worldsToScreenshot.Remove(num);
					}
					this.freezeCamera.enabled = false;
					DebugHandler.SetTimelapseMode(false, 0);
					this.screenshotPending = false;
					this.previewScreenshot = false;
					this.screenshotActive = false;
					this.debugScreenShot = false;
					this.previewSaveGamePath = "";
					OverlayScreen.Instance.ToggleOverlay(this.activeOverlay, false);
				}
			}
		}
		yield break;
	}

	// Token: 0x060043D5 RID: 17365 RVA: 0x0017C358 File Offset: 0x0017A558
	public void InitialScreenshot()
	{
		this.worldsToScreenshot.Add(ClusterManager.Instance.GetStartWorld().id);
		this.SaveScreenshot();
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x0017C37A File Offset: 0x0017A57A
	private void SaveScreenshot()
	{
		this.screenshotPending = true;
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x0017C383 File Offset: 0x0017A583
	public void SaveColonyPreview(string saveFileName)
	{
		this.previewSaveGamePath = saveFileName;
		this.previewScreenshot = true;
		this.SaveScreenshot();
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x0017C39C File Offset: 0x0017A59C
	private void SetPostionAndOrtho(int world_id)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(world_id);
		if (world == null)
		{
			return;
		}
		float num = 0f;
		Camera overlayCamera = CameraController.Instance.overlayCamera;
		this.camSize = overlayCamera.orthographicSize;
		this.camPosition = CameraController.Instance.transform.position;
		if (!world.IsStartWorld)
		{
			CameraController.Instance.OrthographicSize = (float)(world.WorldSize.y / 2);
			CameraController.Instance.SetPosition(new Vector3((float)(world.WorldOffset.x + world.WorldSize.x / 2), (float)(world.WorldOffset.y + world.WorldSize.y / 2), CameraController.Instance.transform.position.z));
			return;
		}
		GameObject telepad = GameUtil.GetTelepad(world_id);
		if (telepad == null)
		{
			return;
		}
		Vector3 position = telepad.transform.GetPosition();
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			Vector3 position2 = buildingComplete.transform.GetPosition();
			float num2 = (float)this.bufferRenderTexture.width / (float)this.bufferRenderTexture.height;
			Vector3 vector = position - position2;
			num = Mathf.Max(new float[]
			{
				num,
				vector.x / num2,
				vector.y
			});
		}
		num += 10f;
		num = Mathf.Max(num, 18f);
		CameraController.Instance.OrthographicSize = num;
		CameraController.Instance.SetPosition(new Vector3(telepad.transform.position.x, telepad.transform.position.y, CameraController.Instance.transform.position.z));
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x0017C588 File Offset: 0x0017A788
	private void RenderAndPrint(int world_id)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(world_id);
		if (world == null)
		{
			return;
		}
		if (world.IsStartWorld)
		{
			GameObject telepad = GameUtil.GetTelepad(world.id);
			if (telepad == null)
			{
				global::Debug.Log("No telepad present, aborting screenshot.");
				return;
			}
			Vector3 position = telepad.transform.position;
			position.z = CameraController.Instance.transform.position.z;
			CameraController.Instance.SetPosition(position);
		}
		else
		{
			CameraController.Instance.SetPosition(new Vector3((float)(world.WorldOffset.x + world.WorldSize.x / 2), (float)(world.WorldOffset.y + world.WorldSize.y / 2), CameraController.Instance.transform.position.z));
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this.bufferRenderTexture;
		CameraController.Instance.RenderForTimelapser(ref this.bufferRenderTexture);
		this.WriteToPng(this.bufferRenderTexture, world_id);
		CameraController.Instance.OrthographicSize = this.camSize;
		CameraController.Instance.SetPosition(this.camPosition);
		RenderTexture.active = active;
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x0017C6B0 File Offset: 0x0017A8B0
	public void WriteToPng(RenderTexture renderTex, int world_id = -1)
	{
		Texture2D texture2D = new Texture2D(renderTex.width, renderTex.height, TextureFormat.ARGB32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTex.width, (float)renderTex.height), 0, 0);
		texture2D.Apply();
		byte[] bytes = texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
		if (!Directory.Exists(Util.RootFolder()))
		{
			Directory.CreateDirectory(Util.RootFolder());
		}
		string text = Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string path = RetireColonyUtility.StripInvalidCharacters(SaveGame.Instance.BaseName);
		if (!this.previewScreenshot)
		{
			string text2 = Path.Combine(text, path);
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string text3 = text2;
			if (world_id >= 0)
			{
				string name = ClusterManager.Instance.GetWorld(world_id).GetComponent<ClusterGridEntity>().Name;
				text3 = Path.Combine(text3, world_id.ToString("D5"));
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
				}
				text3 = Path.Combine(text3, name);
			}
			else
			{
				text3 = Path.Combine(text3, path);
			}
			DebugUtil.LogArgs(new object[]
			{
				"Saving screenshot to",
				text3
			});
			string format = "0000.##";
			text3 = text3 + "_cycle_" + GameClock.Instance.GetCycle().ToString(format);
			if (this.debugScreenShot)
			{
				text3 = string.Concat(new string[]
				{
					text3,
					"_",
					System.DateTime.Now.Day.ToString(),
					"-",
					System.DateTime.Now.Month.ToString(),
					"_",
					System.DateTime.Now.Hour.ToString(),
					"-",
					System.DateTime.Now.Minute.ToString(),
					"-",
					System.DateTime.Now.Second.ToString()
				});
			}
			File.WriteAllBytes(text3 + ".png", bytes);
			return;
		}
		string text4 = this.previewSaveGamePath;
		text4 = Path.ChangeExtension(text4, ".png");
		DebugUtil.LogArgs(new object[]
		{
			"Saving screenshot to",
			text4
		});
		File.WriteAllBytes(text4, bytes);
	}

	// Token: 0x04002CF0 RID: 11504
	private bool screenshotActive;

	// Token: 0x04002CF1 RID: 11505
	private bool screenshotPending;

	// Token: 0x04002CF2 RID: 11506
	private bool previewScreenshot;

	// Token: 0x04002CF3 RID: 11507
	private string previewSaveGamePath = "";

	// Token: 0x04002CF4 RID: 11508
	private bool screenshotToday;

	// Token: 0x04002CF5 RID: 11509
	private List<int> worldsToScreenshot = new List<int>();

	// Token: 0x04002CF6 RID: 11510
	private HashedString activeOverlay;

	// Token: 0x04002CF7 RID: 11511
	private Camera freezeCamera;

	// Token: 0x04002CF8 RID: 11512
	private RenderTexture bufferRenderTexture;

	// Token: 0x04002CFA RID: 11514
	private Vector3 camPosition;

	// Token: 0x04002CFB RID: 11515
	private float camSize;

	// Token: 0x04002CFC RID: 11516
	private bool debugScreenShot;

	// Token: 0x04002CFD RID: 11517
	private Vector2Int previewScreenshotResolution = new Vector2Int(Grid.WidthInCells * 2, Grid.HeightInCells * 2);

	// Token: 0x04002CFE RID: 11518
	private const int DEFAULT_SCREENSHOT_INTERVAL = 10;

	// Token: 0x04002CFF RID: 11519
	private int[] timelapseScreenshotCycles = new int[]
	{
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		13,
		14,
		15,
		16,
		17,
		18,
		19,
		20,
		21,
		22,
		23,
		24,
		25,
		26,
		27,
		28,
		29,
		30,
		31,
		32,
		33,
		34,
		35,
		36,
		37,
		38,
		39,
		40,
		41,
		42,
		43,
		44,
		45,
		46,
		47,
		48,
		49,
		50,
		55,
		60,
		65,
		70,
		75,
		80,
		85,
		90,
		95,
		100,
		110,
		120,
		130,
		140,
		150,
		160,
		170,
		180,
		190,
		200,
		210,
		220,
		230,
		240,
		250,
		260,
		270,
		280,
		290,
		200,
		310,
		320,
		330,
		340,
		350,
		360,
		370,
		380,
		390,
		400,
		410,
		420,
		430,
		440,
		450,
		460,
		470,
		480,
		490,
		500
	};
}
