using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000487 RID: 1159
[AddComponentMenu("KMonoBehaviour/scripts/CameraController")]
public class CameraController : KMonoBehaviour, IInputHandler
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06001999 RID: 6553 RVA: 0x00086B63 File Offset: 0x00084D63
	public string handlerName
	{
		get
		{
			return base.gameObject.name;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x0600199A RID: 6554 RVA: 0x00086B70 File Offset: 0x00084D70
	// (set) Token: 0x0600199B RID: 6555 RVA: 0x00086B94 File Offset: 0x00084D94
	public float OrthographicSize
	{
		get
		{
			if (!(this.baseCamera == null))
			{
				return this.baseCamera.orthographicSize;
			}
			return 0f;
		}
		set
		{
			for (int i = 0; i < this.cameras.Count; i++)
			{
				this.cameras[i].orthographicSize = value;
			}
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600199C RID: 6556 RVA: 0x00086BC9 File Offset: 0x00084DC9
	// (set) Token: 0x0600199D RID: 6557 RVA: 0x00086BD1 File Offset: 0x00084DD1
	public KInputHandler inputHandler { get; set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600199E RID: 6558 RVA: 0x00086BDA File Offset: 0x00084DDA
	// (set) Token: 0x0600199F RID: 6559 RVA: 0x00086BE2 File Offset: 0x00084DE2
	public float targetOrthographicSize { get; private set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060019A0 RID: 6560 RVA: 0x00086BEB File Offset: 0x00084DEB
	// (set) Token: 0x060019A1 RID: 6561 RVA: 0x00086BF3 File Offset: 0x00084DF3
	public bool isTargetPosSet { get; set; }

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00086BFC File Offset: 0x00084DFC
	// (set) Token: 0x060019A3 RID: 6563 RVA: 0x00086C04 File Offset: 0x00084E04
	public Vector3 targetPos { get; private set; }

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060019A4 RID: 6564 RVA: 0x00086C0D File Offset: 0x00084E0D
	// (set) Token: 0x060019A5 RID: 6565 RVA: 0x00086C15 File Offset: 0x00084E15
	public bool ignoreClusterFX { get; private set; }

	// Token: 0x060019A6 RID: 6566 RVA: 0x00086C1E File Offset: 0x00084E1E
	public void ToggleClusterFX()
	{
		this.ignoreClusterFX = !this.ignoreClusterFX;
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00086C30 File Offset: 0x00084E30
	protected override void OnForcedCleanUp()
	{
		GameInputManager inputManager = Global.GetInputManager();
		if (inputManager == null)
		{
			return;
		}
		inputManager.usedMenus.Remove(this);
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060019A8 RID: 6568 RVA: 0x00086C54 File Offset: 0x00084E54
	public int cameraActiveCluster
	{
		get
		{
			if (ClusterManager.Instance == null)
			{
				return 255;
			}
			return ClusterManager.Instance.activeWorldId;
		}
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x00086C74 File Offset: 0x00084E74
	public void GetWorldCamera(out Vector2I worldOffset, out Vector2I worldSize)
	{
		WorldContainer worldContainer = null;
		if (ClusterManager.Instance != null)
		{
			worldContainer = ClusterManager.Instance.activeWorld;
		}
		if (!this.ignoreClusterFX && worldContainer != null)
		{
			worldOffset = worldContainer.WorldOffset;
			worldSize = worldContainer.WorldSize;
			return;
		}
		worldOffset = new Vector2I(0, 0);
		worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060019AA RID: 6570 RVA: 0x00086CE7 File Offset: 0x00084EE7
	// (set) Token: 0x060019AB RID: 6571 RVA: 0x00086CEF File Offset: 0x00084EEF
	public bool DisableUserCameraControl
	{
		get
		{
			return this.userCameraControlDisabled;
		}
		set
		{
			this.userCameraControlDisabled = value;
			if (this.userCameraControlDisabled)
			{
				this.panning = false;
				this.panLeft = false;
				this.panRight = false;
				this.panUp = false;
				this.panDown = false;
			}
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060019AC RID: 6572 RVA: 0x00086D23 File Offset: 0x00084F23
	// (set) Token: 0x060019AD RID: 6573 RVA: 0x00086D2A File Offset: 0x00084F2A
	public static CameraController Instance { get; private set; }

	// Token: 0x060019AE RID: 6574 RVA: 0x00086D32 File Offset: 0x00084F32
	public static void DestroyInstance()
	{
		CameraController.Instance = null;
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x00086D3A File Offset: 0x00084F3A
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.mrt.ToggleColouredOverlayView(enabled);
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x00086D48 File Offset: 0x00084F48
	protected override void OnPrefabInit()
	{
		global::Util.Reset(base.transform);
		base.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -100f));
		this.targetOrthographicSize = this.maxOrthographicSize;
		CameraController.Instance = this;
		this.DisableUserCameraControl = false;
		this.baseCamera = this.CopyCamera(Camera.main, "baseCamera");
		this.mrt = this.baseCamera.gameObject.AddComponent<MultipleRenderTarget>();
		this.mrt.onSetupComplete += this.OnMRTSetupComplete;
		this.baseCamera.gameObject.AddComponent<LightBufferCompositor>();
		this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
		this.baseCamera.transform.parent = base.transform;
		global::Util.Reset(this.baseCamera.transform);
		int mask = LayerMask.GetMask(new string[]
		{
			"PlaceWithDepth",
			"Overlay"
		});
		int mask2 = LayerMask.GetMask(new string[]
		{
			"Construction"
		});
		this.baseCamera.cullingMask &= ~mask;
		this.baseCamera.cullingMask |= mask2;
		this.baseCamera.tag = "Untagged";
		this.baseCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_LitTex";
		this.infraredCamera = this.CopyCamera(this.baseCamera, "Infrared");
		this.infraredCamera.cullingMask = 0;
		this.infraredCamera.clearFlags = CameraClearFlags.Color;
		this.infraredCamera.depth = this.baseCamera.depth - 1f;
		this.infraredCamera.transform.parent = base.transform;
		this.infraredCamera.gameObject.AddComponent<Infrared>();
		if (SimDebugView.Instance != null)
		{
			this.simOverlayCamera = this.CopyCamera(this.baseCamera, "SimOverlayCamera");
			this.simOverlayCamera.cullingMask = LayerMask.GetMask(new string[]
			{
				"SimDebugView"
			});
			this.simOverlayCamera.clearFlags = CameraClearFlags.Color;
			this.simOverlayCamera.depth = this.baseCamera.depth + 1f;
			this.simOverlayCamera.transform.parent = base.transform;
			this.simOverlayCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_SimDebugViewTex";
		}
		this.overlayCamera = Camera.main;
		this.overlayCamera.name = "Overlay";
		this.overlayCamera.cullingMask = (mask | mask2);
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.overlayCamera.transform.parent = base.transform;
		this.overlayCamera.depth = this.baseCamera.depth + 3f;
		this.overlayCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayCamera.transform.localRotation = Quaternion.identity;
		this.overlayCamera.renderingPath = RenderingPath.Forward;
		this.overlayCamera.allowHDR = false;
		this.overlayCamera.tag = "Untagged";
		this.overlayCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = this.baseCamera;
		ColorCorrectionLookup component = this.overlayCamera.GetComponent<ColorCorrectionLookup>();
		component.Convert(this.dayColourCube, "");
		component.Convert2(this.nightColourCube, "");
		this.cameras.Add(this.overlayCamera);
		this.lightBufferCamera = this.CopyCamera(this.overlayCamera, "Light Buffer");
		this.lightBufferCamera.clearFlags = CameraClearFlags.Color;
		this.lightBufferCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"Lights"
		});
		this.lightBufferCamera.depth = this.baseCamera.depth - 1f;
		this.lightBufferCamera.transform.parent = base.transform;
		this.lightBufferCamera.transform.SetLocalPosition(Vector3.zero);
		this.lightBufferCamera.rect = new Rect(0f, 0f, 1f, 1f);
		LightBuffer lightBuffer = this.lightBufferCamera.gameObject.AddComponent<LightBuffer>();
		lightBuffer.Material = this.LightBufferMaterial;
		lightBuffer.CircleMaterial = this.LightCircleOverlay;
		lightBuffer.ConeMaterial = this.LightConeOverlay;
		this.overlayNoDepthCamera = this.CopyCamera(this.overlayCamera, "overlayNoDepth");
		int mask3 = LayerMask.GetMask(new string[]
		{
			"Overlay",
			"Place"
		});
		this.baseCamera.cullingMask &= ~mask3;
		this.overlayNoDepthCamera.clearFlags = CameraClearFlags.Depth;
		this.overlayNoDepthCamera.cullingMask = mask3;
		this.overlayNoDepthCamera.transform.parent = base.transform;
		this.overlayNoDepthCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayNoDepthCamera.depth = this.baseCamera.depth + 4f;
		this.overlayNoDepthCamera.tag = "MainCamera";
		this.overlayNoDepthCamera.gameObject.AddComponent<NavPathDrawer>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RangeVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<SkyVisibilityVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<ScannerNetworkVisualizerEffect>();
		this.uiCamera = this.CopyCamera(this.overlayCamera, "uiCamera");
		this.uiCamera.clearFlags = CameraClearFlags.Depth;
		this.uiCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"UI"
		});
		this.uiCamera.transform.parent = base.transform;
		this.uiCamera.transform.SetLocalPosition(Vector3.zero);
		this.uiCamera.depth = this.baseCamera.depth + 5f;
		if (Game.Instance != null)
		{
			this.timelapseFreezeCamera = this.CopyCamera(this.uiCamera, "timelapseFreezeCamera");
			this.timelapseFreezeCamera.depth = this.uiCamera.depth + 3f;
			this.timelapseFreezeCamera.gameObject.AddComponent<FillRenderTargetEffect>();
			this.timelapseFreezeCamera.enabled = false;
			Camera camera = CameraController.CloneCamera(this.overlayCamera, "timelapseCamera");
			Timelapser timelapser = camera.gameObject.AddComponent<Timelapser>();
			camera.transparencySortMode = TransparencySortMode.Orthographic;
			camera.depth = this.baseCamera.depth + 2f;
			Game.Instance.timelapser = timelapser;
		}
		if (GameScreenManager.Instance != null)
		{
			for (int i = 0; i < this.uiCameraTargets.Count; i++)
			{
				GameScreenManager.Instance.SetCamera(this.uiCameraTargets[i], this.uiCamera);
			}
			this.infoText = GameScreenManager.Instance.screenshotModeCanvas.GetComponentInChildren<LocText>();
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
		this.SetSpeedFromPrefs(null);
		Game.Instance.Subscribe(75424175, new Action<object>(this.SetSpeedFromPrefs));
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x00087462 File Offset: 0x00085662
	private void SetSpeedFromPrefs(object data = null)
	{
		this.keyPanningSpeed = Mathf.Clamp(0.1f, KPlayerPrefs.GetFloat("CameraSpeed"), 2f);
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x00087484 File Offset: 0x00085684
	public int GetCursorCell()
	{
		Vector3 rhs = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		Vector3 vector = Vector3.Max(ClusterManager.Instance.activeWorld.minimumBounds, rhs);
		vector = Vector3.Min(ClusterManager.Instance.activeWorld.maximumBounds, vector);
		return Grid.PosToCell(vector);
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000874DD File Offset: 0x000856DD
	public static Camera CloneCamera(Camera camera, string name)
	{
		Camera camera2 = new GameObject
		{
			name = name
		}.AddComponent<Camera>();
		camera2.CopyFrom(camera);
		return camera2;
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000874F8 File Offset: 0x000856F8
	private Camera CopyCamera(Camera camera, string name)
	{
		Camera camera2 = CameraController.CloneCamera(camera, name);
		this.cameras.Add(camera2);
		return camera2;
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x0008751A File Offset: 0x0008571A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Restore();
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x00087528 File Offset: 0x00085728
	public static void SetDefaultCameraSpeed()
	{
		KPlayerPrefs.SetFloat("CameraSpeed", 1f);
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060019B7 RID: 6583 RVA: 0x00087539 File Offset: 0x00085739
	// (set) Token: 0x060019B8 RID: 6584 RVA: 0x00087541 File Offset: 0x00085741
	public Coroutine activeFadeRoutine { get; private set; }

	// Token: 0x060019B9 RID: 6585 RVA: 0x0008754A File Offset: 0x0008574A
	public void FadeOut(float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 0f, targetPercentage, speed, null));
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x0008757B File Offset: 0x0008577B
	public void FadeIn(float targetPercentage = 0f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 1f, targetPercentage, speed, callback));
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000875AC File Offset: 0x000857AC
	public void ActiveWorldStarWipe(int id, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, false, default(Vector3), 10f, callback);
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000875D0 File Offset: 0x000857D0
	public void ActiveWorldStarWipe(int id, Vector3 position, float forceOrthgraphicSize = 10f, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, true, position, forceOrthgraphicSize, callback);
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000875E0 File Offset: 0x000857E0
	private void ActiveWorldStarWipe(int id, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action callback)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		if (ClusterManager.Instance.activeWorldId != id)
		{
			DetailsScreen.Instance.DeselectAndClose();
			this.activeFadeRoutine = base.StartCoroutine(this.SwapToWorldFade(id, useForcePosition, forcePosition, forceOrthgraphicSize, callback));
			return;
		}
		ManagementMenu.Instance.CloseAll();
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, 8f, true);
			if (callback != null)
			{
				callback();
			}
		}
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x0008765B File Offset: 0x0008585B
	private IEnumerator SwapToWorldFade(int worldId, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action newWorldCallback)
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot);
		ClusterManager.Instance.UpdateWorldReverbSnapshot(worldId);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 0f, 1f, 3f, null));
		ClusterManager.Instance.SetActiveWorld(worldId);
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, forceOrthgraphicSize, false);
			CameraController.Instance.SetPosition(forcePosition);
		}
		if (newWorldCallback != null)
		{
			newWorldCallback();
		}
		ManagementMenu.Instance.CloseAll();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 1f, 0f, 3f, null));
		yield break;
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x0008768F File Offset: 0x0008588F
	public void SetWorldInteractive(bool state)
	{
		GameScreenManager.Instance.fadePlaneFront.raycastTarget = !state;
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000876A4 File Offset: 0x000858A4
	private IEnumerator FadeWithBlack(bool fadeUI, float startBlackPercent, float targetBlackPercent, float speed = 1f, System.Action callback = null)
	{
		Image fadePlane = fadeUI ? GameScreenManager.Instance.fadePlaneFront : GameScreenManager.Instance.fadePlaneBack;
		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.unscaledDeltaTime * speed;
			float a = MathUtil.ReRange(percent, 0f, 1f, startBlackPercent, targetBlackPercent);
			fadePlane.color = new Color(0f, 0f, 0f, a);
			yield return SequenceUtil.WaitForNextFrame;
		}
		fadePlane.color = new Color(0f, 0f, 0f, targetBlackPercent);
		if (callback != null)
		{
			callback();
		}
		this.activeFadeRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x000876D8 File Offset: 0x000858D8
	public void EnableFreeCamera(bool enable)
	{
		this.FreeCameraEnabled = enable;
		this.SetInfoText("Screenshot Mode (ESC to exit)");
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000876EC File Offset: 0x000858EC
	private static bool WithinInputField()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null)
		{
			return false;
		}
		bool result = false;
		if (current.currentSelectedGameObject != null && (current.currentSelectedGameObject.GetComponent<KInputTextField>() != null || current.currentSelectedGameObject.GetComponent<InputField>() != null))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060019C3 RID: 6595 RVA: 0x00087744 File Offset: 0x00085944
	private bool IsMouseOverGameWindow
	{
		get
		{
			return 0f <= Input.mousePosition.x && 0f <= Input.mousePosition.y && (float)Screen.width >= Input.mousePosition.x && (float)Screen.height >= Input.mousePosition.y;
		}
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x0008779C File Offset: 0x0008599C
	private void SetInfoText(string text)
	{
		this.infoText.text = text;
		Color color = this.infoText.color;
		color.a = 0.5f;
		this.infoText.color = color;
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000877DC File Offset: 0x000859DC
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (SaveGame.Instance != null && SaveGame.Instance.GetComponent<UserNavigation>().Handle(e))
		{
			return;
		}
		if (!this.ChangeWorldInput(e))
		{
			if (e.TryConsume(global::Action.TogglePause))
			{
				SpeedControlScreen.Instance.TogglePause(false);
			}
			else if (e.TryConsume(global::Action.ZoomIn) && this.IsMouseOverGameWindow)
			{
				float a = this.targetOrthographicSize * (1f / this.zoomFactor);
				this.targetOrthographicSize = Mathf.Max(a, this.minOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.ZoomOut) && this.IsMouseOverGameWindow)
			{
				float a2 = this.targetOrthographicSize * this.zoomFactor;
				this.targetOrthographicSize = Mathf.Min(a2, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
			{
				this.panning = true;
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (this.FreeCameraEnabled && e.TryConsume(global::Action.CinemaCamEnable))
			{
				this.cinemaCamEnabled = !this.cinemaCamEnabled;
				DebugUtil.LogArgs(new object[]
				{
					"Cinema Cam Enabled ",
					this.cinemaCamEnabled
				});
				this.SetInfoText(this.cinemaCamEnabled ? "Cinema Cam Enabled" : "Cinema Cam Disabled");
			}
			else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				if (e.TryConsume(global::Action.CinemaToggleLock))
				{
					this.cinemaToggleLock = !this.cinemaToggleLock;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Lock ",
						this.cinemaToggleLock
					});
					this.SetInfoText(this.cinemaToggleLock ? "Cinema Input Lock ON" : "Cinema Input Lock OFF");
				}
				else if (e.TryConsume(global::Action.CinemaToggleEasing))
				{
					this.cinemaToggleEasing = !this.cinemaToggleEasing;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Easing ",
						this.cinemaToggleEasing
					});
					this.SetInfoText(this.cinemaToggleEasing ? "Cinema Easing ON" : "Cinema Easing OFF");
				}
				else if (e.TryConsume(global::Action.CinemaUnpauseOnMove))
				{
					this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Unpause Next Move ",
						this.cinemaUnpauseNextMove
					});
					this.SetInfoText(this.cinemaUnpauseNextMove ? "Cinema Unpause Next Move ON" : "Cinema Unpause Next Move OFF");
				}
				else if (e.TryConsume(global::Action.CinemaPanLeft))
				{
					this.cinemaPanLeft = (!this.cinemaToggleLock || !this.cinemaPanLeft);
					this.cinemaPanRight = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanRight))
				{
					this.cinemaPanRight = (!this.cinemaToggleLock || !this.cinemaPanRight);
					this.cinemaPanLeft = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanUp))
				{
					this.cinemaPanUp = (!this.cinemaToggleLock || !this.cinemaPanUp);
					this.cinemaPanDown = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanDown))
				{
					this.cinemaPanDown = (!this.cinemaToggleLock || !this.cinemaPanDown);
					this.cinemaPanUp = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomIn))
				{
					this.cinemaZoomIn = (!this.cinemaToggleLock || !this.cinemaZoomIn);
					this.cinemaZoomOut = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomOut))
				{
					this.cinemaZoomOut = (!this.cinemaToggleLock || !this.cinemaZoomOut);
					this.cinemaZoomIn = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedPlus))
				{
					this.cinemaZoomSpeed++;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedMinus))
				{
					this.cinemaZoomSpeed--;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
			}
			else if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
			}
			else if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
			}
			else if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
			}
			else if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
			}
		}
		if (!e.Consumed && OverlayMenu.Instance != null)
		{
			OverlayMenu.Instance.OnKeyDown(e);
		}
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x00087D38 File Offset: 0x00085F38
	public bool ChangeWorldInput(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return true;
		}
		int num = -1;
		if (e.TryConsume(global::Action.SwitchActiveWorld1))
		{
			num = 0;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld2))
		{
			num = 1;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld3))
		{
			num = 2;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld4))
		{
			num = 3;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld5))
		{
			num = 4;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld6))
		{
			num = 5;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld7))
		{
			num = 6;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld8))
		{
			num = 7;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld9))
		{
			num = 8;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld10))
		{
			num = 9;
		}
		if (num != -1)
		{
			List<int> discoveredAsteroidIDsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
			if (num < discoveredAsteroidIDsSorted.Count && num >= 0)
			{
				num = discoveredAsteroidIDsSorted[num];
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (world != null && world.IsDiscovered && ClusterManager.Instance.activeWorldId != world.id)
				{
					ManagementMenu.Instance.CloseClusterMap();
					this.ActiveWorldStarWipe(world.id, null);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x00087E70 File Offset: 0x00086070
	public void OnKeyUp(KButtonEvent e)
	{
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
		{
			this.panning = false;
			return;
		}
		if (this.FreeCameraEnabled && this.cinemaCamEnabled)
		{
			if (e.TryConsume(global::Action.CinemaPanLeft))
			{
				this.cinemaPanLeft = (this.cinemaToggleLock && this.cinemaPanLeft);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanRight))
			{
				this.cinemaPanRight = (this.cinemaToggleLock && this.cinemaPanRight);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanUp))
			{
				this.cinemaPanUp = (this.cinemaToggleLock && this.cinemaPanUp);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanDown))
			{
				this.cinemaPanDown = (this.cinemaToggleLock && this.cinemaPanDown);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomIn))
			{
				this.cinemaZoomIn = (this.cinemaToggleLock && this.cinemaZoomIn);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomOut))
			{
				this.cinemaZoomOut = (this.cinemaToggleLock && this.cinemaZoomOut);
				return;
			}
		}
		else
		{
			if (e.TryConsume(global::Action.CameraHome))
			{
				this.CameraGoHome(2f);
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
			}
		}
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x0008800B File Offset: 0x0008620B
	public void ForcePanningState(bool state)
	{
		this.panning = false;
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x00088014 File Offset: 0x00086214
	public void CameraGoHome(float speed = 2f)
	{
		GameObject activeTelepad = GameUtil.GetActiveTelepad();
		if (activeTelepad != null && ClusterUtil.ActiveWorldHasPrinter())
		{
			Vector3 pos = new Vector3(activeTelepad.transform.GetPosition().x, activeTelepad.transform.GetPosition().y + 1f, base.transform.GetPosition().z);
			this.SetTargetPos(pos, 10f, true);
			this.SetOverrideZoomSpeed(speed);
		}
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x00088088 File Offset: 0x00086288
	public void CameraGoTo(Vector3 pos, float speed = 2f, bool playSound = true)
	{
		pos.z = base.transform.GetPosition().z;
		this.SetTargetPos(pos, 10f, playSound);
		this.SetOverrideZoomSpeed(speed);
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x000880B8 File Offset: 0x000862B8
	public void SnapTo(Vector3 pos)
	{
		this.ClearFollowTarget();
		pos.z = -100f;
		this.targetPos = Vector3.zero;
		this.isTargetPosSet = false;
		base.transform.SetPosition(pos);
		this.keyPanDelta = Vector3.zero;
		this.OrthographicSize = this.targetOrthographicSize;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x0008810D File Offset: 0x0008630D
	public void SnapTo(Vector3 pos, float orthographicSize)
	{
		this.targetOrthographicSize = orthographicSize;
		this.SnapTo(pos);
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x0008811D File Offset: 0x0008631D
	public void SetOverrideZoomSpeed(float tempZoomSpeed)
	{
		this.overrideZoomSpeed = tempZoomSpeed;
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x00088128 File Offset: 0x00086328
	public void SetTargetPos(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		if ((int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			this.targetOrthographicSize = 20f;
			this.ActiveWorldStarWipe((int)Grid.WorldIdx[num], pos, 10f, delegate()
			{
				this.targetPos = pos;
				this.isTargetPosSet = true;
				this.OrthographicSize = orthographic_size + 5f;
				this.targetOrthographicSize = orthographic_size;
			});
		}
		else
		{
			this.targetPos = pos;
			this.isTargetPosSet = true;
			this.targetOrthographicSize = orthographic_size;
		}
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x00088230 File Offset: 0x00086430
	public void SetTargetPosForWorldChange(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		this.targetPos = pos;
		this.isTargetPosSet = true;
		this.targetOrthographicSize = orthographic_size;
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
		this.SetPosition(pos);
		this.OrthographicSize = orthographic_size;
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000882D4 File Offset: 0x000864D4
	public void SetMaxOrthographicSize(float size)
	{
		this.maxOrthographicSize = size;
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000882DD File Offset: 0x000864DD
	public void SetPosition(Vector3 pos)
	{
		base.transform.SetPosition(pos);
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000882EC File Offset: 0x000864EC
	public IEnumerator DoCinematicZoom(float targetOrthographicSize)
	{
		this.cinemaCamEnabled = true;
		this.FreeCameraEnabled = true;
		this.targetOrthographicSize = targetOrthographicSize;
		while (targetOrthographicSize - this.OrthographicSize >= 0.001f)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OrthographicSize = targetOrthographicSize;
		this.FreeCameraEnabled = false;
		this.cinemaCamEnabled = false;
		yield break;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x00088304 File Offset: 0x00086504
	private Vector3 PointUnderCursor(Vector3 mousePos, Camera cam)
	{
		Ray ray = cam.ScreenPointToRay(mousePos);
		Vector3 direction = ray.direction;
		Vector3 b = direction * Mathf.Abs(cam.transform.GetPosition().z / direction.z);
		return ray.origin + b;
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x00088354 File Offset: 0x00086554
	private void CinemaCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		Vector3 localPosition = base.transform.GetLocalPosition();
		float num = Mathf.Pow((float)this.cinemaZoomSpeed, 3f);
		if (this.cinemaZoomIn)
		{
			this.overrideZoomSpeed = -num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else if (this.cinemaZoomOut)
		{
			this.overrideZoomSpeed = num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else
		{
			this.overrideZoomSpeed = 0f;
		}
		if (this.cinemaToggleEasing)
		{
			this.cinemaZoomVelocity += (this.overrideZoomSpeed - this.cinemaZoomVelocity) * this.cinemaEasing;
		}
		else
		{
			this.cinemaZoomVelocity = this.overrideZoomSpeed;
		}
		if (this.cinemaZoomVelocity != 0f)
		{
			this.OrthographicSize = main.orthographicSize + this.cinemaZoomVelocity * unscaledDeltaTime * (main.orthographicSize / 20f);
			this.targetOrthographicSize = main.orthographicSize;
		}
		float num2 = num / TuningData<CameraController.Tuning>.Get().cinemaZoomToFactor;
		float num3 = this.keyPanningSpeed / 20f * main.orthographicSize;
		float num4 = num3 * (num / TuningData<CameraController.Tuning>.Get().cinemaPanToFactor);
		if (!this.isTargetPosSet && this.targetOrthographicSize != main.orthographicSize)
		{
			float t = Mathf.Min(num2 * unscaledDeltaTime, 0.1f);
			this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		}
		Vector3 b = Vector3.zero;
		if (this.isTargetPosSet)
		{
			float num5 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetZoomEasingFactor;
			float num6 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetPanEasingFactor;
			float num7 = this.targetOrthographicSize - main.orthographicSize;
			Vector3 vector = this.targetPos - localPosition;
			float num8;
			float num9;
			if (!this.cinemaToggleEasing)
			{
				num8 = num2 * unscaledDeltaTime;
				num9 = num4 * unscaledDeltaTime;
			}
			else
			{
				DebugUtil.LogArgs(new object[]
				{
					"Min zoom of:",
					num2 * unscaledDeltaTime,
					Mathf.Abs(num7) * num5 * unscaledDeltaTime
				});
				num8 = Mathf.Min(num2 * unscaledDeltaTime, Mathf.Abs(num7) * num5 * unscaledDeltaTime);
				DebugUtil.LogArgs(new object[]
				{
					"Min pan of:",
					num4 * unscaledDeltaTime,
					vector.magnitude * num6 * unscaledDeltaTime
				});
				num9 = Mathf.Min(num4 * unscaledDeltaTime, vector.magnitude * num6 * unscaledDeltaTime);
			}
			float num10;
			if (Mathf.Abs(num7) < num8)
			{
				num10 = num7;
			}
			else
			{
				num10 = Mathf.Sign(num7) * num8;
			}
			if (vector.magnitude < num9)
			{
				b = vector;
			}
			else
			{
				b = vector.normalized * num9;
			}
			if (Mathf.Abs(num10) < 0.001f && b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				num10 = num7;
				b = vector;
			}
			this.OrthographicSize = main.orthographicSize + num10 * (main.orthographicSize / 20f);
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else
		{
			float num11 = num / TuningData<CameraController.Tuning>.Get().cinemaPanFactor;
			Vector3 zero = Vector3.zero;
			if (this.cinemaPanLeft)
			{
				this.ClearFollowTarget();
				zero.x = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanRight)
			{
				this.ClearFollowTarget();
				zero.x = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanUp)
			{
				this.ClearFollowTarget();
				zero.y = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanDown)
			{
				this.ClearFollowTarget();
				zero.y = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaToggleEasing)
			{
				this.keyPanDelta += (zero - this.keyPanDelta) * this.cinemaEasing;
			}
			else
			{
				this.keyPanDelta = zero;
			}
		}
		Vector3 vector2 = localPosition + b + b2 + this.keyPanDelta * unscaledDeltaTime;
		if (this.followTarget != null)
		{
			vector2.x = this.followTargetPos.x;
			vector2.y = this.followTargetPos.y;
		}
		vector2.z = -100f;
		if ((double)(vector2 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector2);
		}
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x0008881C File Offset: 0x00086A1C
	private void NormalCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		this.smoothDt = this.smoothDt * 2f / 3f + unscaledDeltaTime / 3f;
		float num = (this.overrideZoomSpeed != 0f) ? this.overrideZoomSpeed : this.zoomSpeed;
		Vector3 localPosition = base.transform.GetLocalPosition();
		Vector3 vector = (this.overrideZoomSpeed != 0f) ? new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f) : KInputManager.GetMousePos();
		Vector3 position = this.PointUnderCursor(vector, main);
		Vector3 position2 = main.ScreenToViewportPoint(vector);
		float num2 = this.keyPanningSpeed / 20f * main.orthographicSize;
		num2 *= Mathf.Min(unscaledDeltaTime / 0.016666666f, 10f);
		float t = num * Mathf.Min(this.smoothDt, 0.3f);
		this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		base.transform.SetLocalPosition(localPosition);
		Vector3 vector2 = main.WorldToViewportPoint(position);
		position2.z = vector2.z;
		Vector3 b = main.ViewportToWorldPoint(vector2) - main.ViewportToWorldPoint(position2);
		if (this.isTargetPosSet)
		{
			b = Vector3.Lerp(localPosition, this.targetPos, num * this.smoothDt) - localPosition;
			if (b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				b = this.targetPos - localPosition;
			}
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
		}
		Vector3 vector3 = localPosition + b + b2;
		if (this.panning)
		{
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else if (!this.DisableUserCameraControl)
		{
			if (this.panLeft)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panRight)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panUp)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panDown)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				Vector2 vector4 = num2 * KInputManager.steamInputInterpreter.GetSteamCameraMovement();
				if (Mathf.Abs(vector4.x) > Mathf.Epsilon || Mathf.Abs(vector4.y) > Mathf.Epsilon)
				{
					this.ClearFollowTarget();
					this.isTargetPosSet = false;
					this.overrideZoomSpeed = 0f;
				}
				this.keyPanDelta += new Vector3(vector4.x, vector4.y, 0f);
			}
			Vector3 vector5 = new Vector3(Mathf.Lerp(0f, this.keyPanDelta.x, this.smoothDt * this.keyPanningEasing), Mathf.Lerp(0f, this.keyPanDelta.y, this.smoothDt * this.keyPanningEasing), 0f);
			this.keyPanDelta -= vector5;
			vector3.x += vector5.x;
			vector3.y += vector5.y;
		}
		if (this.followTarget != null)
		{
			vector3.x = this.followTargetPos.x;
			vector3.y = this.followTargetPos.y;
		}
		vector3.z = -100f;
		if ((double)(vector3 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector3);
		}
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x00088C68 File Offset: 0x00086E68
	private void Update()
	{
		if (Game.Instance == null || !Game.Instance.timelapser.CapturingTimelapseScreenshot)
		{
			if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				this.CinemaCamUpdate();
			}
			else
			{
				this.NormalCamUpdate();
			}
		}
		if (this.infoText != null && this.infoText.color.a > 0f)
		{
			Color color = this.infoText.color;
			color.a = Mathf.Max(0f, this.infoText.color.a - Time.unscaledDeltaTime * 0.5f);
			this.infoText.color = color;
		}
		this.ConstrainToWorld();
		Vector3 vector = this.PointUnderCursor(KInputManager.GetMousePos(), Camera.main);
		Shader.SetGlobalVector("_WorldCameraPos", new Vector4(base.transform.GetPosition().x, base.transform.GetPosition().y, base.transform.GetPosition().z, Camera.main.orthographicSize));
		Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector.x, vector.y, 0f, 0f));
		this.VisibleArea.Update();
		this.soundCuller = SoundCuller.CreateCuller();
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x00088DB8 File Offset: 0x00086FB8
	private Vector3 GetFollowPos()
	{
		if (this.followTarget != null)
		{
			Vector3 result = this.followTarget.transform.GetPosition();
			KAnimControllerBase component = this.followTarget.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				result = component.GetWorldPivot();
			}
			return result;
		}
		return Vector3.zero;
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x00088E08 File Offset: 0x00087008
	public static float GetHighestVisibleCell_Height(byte worldID = 255)
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Camera main = Camera.main;
		float orthographicSize = main.orthographicSize;
		main.orthographicSize = 20f;
		Ray ray = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		Vector3 vector2 = CameraController.Instance.transform.GetPosition() - ray.origin;
		main.orthographicSize = orthographicSize;
		if (ClusterManager.Instance != null)
		{
			WorldContainer worldContainer = (worldID == byte.MaxValue) ? ClusterManager.Instance.activeWorld : ClusterManager.Instance.GetWorld((int)worldID);
			worldContainer.minimumBounds * Grid.CellSizeInMeters;
			vector = worldContainer.maximumBounds * Grid.CellSizeInMeters;
			new Vector2((float)worldContainer.Width, (float)worldContainer.Height) * Grid.CellSizeInMeters;
		}
		return vector.y * 1.1f + 20f + vector2.y;
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x00088F10 File Offset: 0x00087110
	private void ConstrainToWorld()
	{
		if (Game.Instance != null && Game.Instance.IsLoading())
		{
			return;
		}
		if (this.FreeCameraEnabled)
		{
			return;
		}
		Camera main = Camera.main;
		Ray ray = main.ViewportPointToRay(Vector3.zero + Vector3.one * 0.33f);
		Ray ray2 = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector3 point2 = ray2.GetPoint(distance2);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Vector2 vector3 = vector2;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			vector = activeWorld.minimumBounds * Grid.CellSizeInMeters;
			vector2 = activeWorld.maximumBounds * Grid.CellSizeInMeters;
			vector3 = new Vector2((float)activeWorld.Width, (float)activeWorld.Height) * Grid.CellSizeInMeters;
		}
		if (point2.x - point.x > vector3.x || point2.y - point.y > vector3.y)
		{
			return;
		}
		Vector3 b = base.transform.GetPosition() - ray.origin;
		Vector3 vector4 = point;
		vector4.x = Mathf.Max(vector.x, vector4.x);
		vector4.y = Mathf.Max(vector.y * Grid.CellSizeInMeters, vector4.y);
		ray.origin = vector4;
		ray.direction = -ray.direction;
		vector4 = ray.GetPoint(distance);
		base.transform.SetPosition(vector4 + b);
		b = base.transform.GetPosition() - ray2.origin;
		vector4 = point2;
		vector4.x = Mathf.Min(vector2.x, vector4.x);
		vector4.y = Mathf.Min(vector2.y * 1.1f, vector4.y);
		ray2.origin = vector4;
		ray2.direction = -ray2.direction;
		vector4 = ray2.GetPoint(distance2);
		Vector3 position = vector4 + b;
		position.z = -100f;
		base.transform.SetPosition(position);
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x000891B0 File Offset: 0x000873B0
	public void Save(BinaryWriter writer)
	{
		writer.Write(base.transform.GetPosition());
		writer.Write(base.transform.localScale);
		writer.Write(base.transform.rotation);
		writer.Write(this.targetOrthographicSize);
		CameraSaveData.position = base.transform.GetPosition();
		CameraSaveData.localScale = base.transform.localScale;
		CameraSaveData.rotation = base.transform.rotation;
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x0008922C File Offset: 0x0008742C
	private void Restore()
	{
		if (CameraSaveData.valid)
		{
			int cell = Grid.PosToCell(CameraSaveData.position);
			if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell))
			{
				global::Debug.LogWarning("Resetting Camera Position... camera was saved in an undiscovered area of the map.");
				this.CameraGoHome(2f);
				return;
			}
			base.transform.SetPosition(CameraSaveData.position);
			base.transform.localScale = CameraSaveData.localScale;
			base.transform.rotation = CameraSaveData.rotation;
			this.targetOrthographicSize = Mathf.Clamp(CameraSaveData.orthographicsSize, this.minOrthographicSize, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
			this.SnapTo(base.transform.GetPosition());
		}
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000892E6 File Offset: 0x000874E6
	private void OnMRTSetupComplete(Camera cam)
	{
		this.cameras.Add(cam);
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x000892F4 File Offset: 0x000874F4
	public bool IsAudibleSound(Vector2 pos)
	{
		return this.soundCuller.IsAudible(pos);
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x00089304 File Offset: 0x00087504
	public bool IsAudibleSound(Vector3 pos, EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.soundCuller.IsAudible(pos, eventReferencePath);
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x0008932F File Offset: 0x0008752F
	public bool IsAudibleSound(Vector3 pos, HashedString sound_path)
	{
		return this.soundCuller.IsAudible(pos, sound_path);
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x00089343 File Offset: 0x00087543
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		return this.soundCuller.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x00089354 File Offset: 0x00087554
	public bool IsVisiblePos(Vector3 pos)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		return visibleArea.Min <= pos && pos <= visibleArea.Max;
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x0008938F File Offset: 0x0008758F
	protected override void OnCleanUp()
	{
		CameraController.Instance = null;
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x00089398 File Offset: 0x00087598
	public void SetFollowTarget(Transform follow_target)
	{
		this.ClearFollowTarget();
		if (follow_target == null)
		{
			return;
		}
		this.followTarget = follow_target;
		this.OrthographicSize = 6f;
		this.targetOrthographicSize = 6f;
		Vector3 followPos = this.GetFollowPos();
		this.followTargetPos = new Vector3(followPos.x, followPos.y, base.transform.GetPosition().z);
		base.transform.SetPosition(this.followTargetPos);
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-1506069671, null);
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x00089428 File Offset: 0x00087628
	public void ClearFollowTarget()
	{
		if (this.followTarget == null)
		{
			return;
		}
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-485480405, null);
		this.followTarget = null;
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x00089458 File Offset: 0x00087658
	public void UpdateFollowTarget()
	{
		if (this.followTarget != null)
		{
			Vector3 followPos = this.GetFollowPos();
			Vector2 a = new Vector2(base.transform.GetLocalPosition().x, base.transform.GetLocalPosition().y);
			byte b = Grid.WorldIdx[Grid.PosToCell(followPos)];
			if (ClusterManager.Instance.activeWorldId != (int)b)
			{
				Transform transform = this.followTarget;
				this.SetFollowTarget(null);
				ClusterManager.Instance.SetActiveWorld((int)b);
				this.SetFollowTarget(transform);
				return;
			}
			Vector2 vector = Vector2.Lerp(a, followPos, Time.unscaledDeltaTime * 25f);
			this.followTargetPos = new Vector3(vector.x, vector.y, base.transform.GetLocalPosition().z);
		}
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x00089524 File Offset: 0x00087724
	public void RenderForTimelapser(ref RenderTexture tex)
	{
		this.RenderCameraForTimelapse(this.baseCamera, ref tex, this.timelapseCameraCullingMask, -1f);
		CameraClearFlags clearFlags = this.overlayCamera.clearFlags;
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.RenderCameraForTimelapse(this.overlayCamera, ref tex, this.timelapseOverlayCameraCullingMask, -1f);
		this.overlayCamera.clearFlags = clearFlags;
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x00089588 File Offset: 0x00087788
	private void RenderCameraForTimelapse(Camera cam, ref RenderTexture tex, LayerMask mask, float overrideAspect = -1f)
	{
		int cullingMask = cam.cullingMask;
		RenderTexture targetTexture = cam.targetTexture;
		cam.targetTexture = tex;
		cam.aspect = (float)tex.width / (float)tex.height;
		if (overrideAspect != -1f)
		{
			cam.aspect = overrideAspect;
		}
		if (mask != -1)
		{
			cam.cullingMask = mask;
		}
		cam.Render();
		cam.ResetAspect();
		cam.cullingMask = cullingMask;
		cam.targetTexture = targetTexture;
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x00089602 File Offset: 0x00087802
	private void CheckMoveUnpause()
	{
		if (this.cinemaCamEnabled && this.cinemaUnpauseNextMove)
		{
			this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
		}
	}

	// Token: 0x04000E2C RID: 3628
	public const float DEFAULT_MAX_ORTHO_SIZE = 20f;

	// Token: 0x04000E2D RID: 3629
	public const float MAX_Y_SCALE = 1.1f;

	// Token: 0x04000E2E RID: 3630
	public LocText infoText;

	// Token: 0x04000E2F RID: 3631
	private const float FIXED_Z = -100f;

	// Token: 0x04000E31 RID: 3633
	public bool FreeCameraEnabled;

	// Token: 0x04000E32 RID: 3634
	public float zoomSpeed;

	// Token: 0x04000E33 RID: 3635
	public float minOrthographicSize;

	// Token: 0x04000E34 RID: 3636
	public float zoomFactor;

	// Token: 0x04000E35 RID: 3637
	public float keyPanningSpeed;

	// Token: 0x04000E36 RID: 3638
	public float keyPanningEasing;

	// Token: 0x04000E37 RID: 3639
	public Texture2D dayColourCube;

	// Token: 0x04000E38 RID: 3640
	public Texture2D nightColourCube;

	// Token: 0x04000E39 RID: 3641
	public Material LightBufferMaterial;

	// Token: 0x04000E3A RID: 3642
	public Material LightCircleOverlay;

	// Token: 0x04000E3B RID: 3643
	public Material LightConeOverlay;

	// Token: 0x04000E3C RID: 3644
	public Transform followTarget;

	// Token: 0x04000E3D RID: 3645
	public Vector3 followTargetPos;

	// Token: 0x04000E3E RID: 3646
	public GridVisibleArea VisibleArea = new GridVisibleArea();

	// Token: 0x04000E40 RID: 3648
	private float maxOrthographicSize = 20f;

	// Token: 0x04000E41 RID: 3649
	private float overrideZoomSpeed;

	// Token: 0x04000E42 RID: 3650
	private bool panning;

	// Token: 0x04000E43 RID: 3651
	private const float MaxEdgePaddingPercent = 0.33f;

	// Token: 0x04000E44 RID: 3652
	private Vector3 keyPanDelta;

	// Token: 0x04000E47 RID: 3655
	[SerializeField]
	private LayerMask timelapseCameraCullingMask;

	// Token: 0x04000E48 RID: 3656
	[SerializeField]
	private LayerMask timelapseOverlayCameraCullingMask;

	// Token: 0x04000E4A RID: 3658
	private bool userCameraControlDisabled;

	// Token: 0x04000E4B RID: 3659
	private bool panLeft;

	// Token: 0x04000E4C RID: 3660
	private bool panRight;

	// Token: 0x04000E4D RID: 3661
	private bool panUp;

	// Token: 0x04000E4E RID: 3662
	private bool panDown;

	// Token: 0x04000E50 RID: 3664
	[NonSerialized]
	public Camera baseCamera;

	// Token: 0x04000E51 RID: 3665
	[NonSerialized]
	public Camera overlayCamera;

	// Token: 0x04000E52 RID: 3666
	[NonSerialized]
	public Camera overlayNoDepthCamera;

	// Token: 0x04000E53 RID: 3667
	[NonSerialized]
	public Camera uiCamera;

	// Token: 0x04000E54 RID: 3668
	[NonSerialized]
	public Camera lightBufferCamera;

	// Token: 0x04000E55 RID: 3669
	[NonSerialized]
	public Camera simOverlayCamera;

	// Token: 0x04000E56 RID: 3670
	[NonSerialized]
	public Camera infraredCamera;

	// Token: 0x04000E57 RID: 3671
	[NonSerialized]
	public Camera timelapseFreezeCamera;

	// Token: 0x04000E58 RID: 3672
	[SerializeField]
	private List<GameScreenManager.UIRenderTarget> uiCameraTargets;

	// Token: 0x04000E59 RID: 3673
	public List<Camera> cameras = new List<Camera>();

	// Token: 0x04000E5A RID: 3674
	private MultipleRenderTarget mrt;

	// Token: 0x04000E5B RID: 3675
	public SoundCuller soundCuller;

	// Token: 0x04000E5C RID: 3676
	private bool cinemaCamEnabled;

	// Token: 0x04000E5D RID: 3677
	private bool cinemaToggleLock;

	// Token: 0x04000E5E RID: 3678
	private bool cinemaToggleEasing;

	// Token: 0x04000E5F RID: 3679
	private bool cinemaUnpauseNextMove;

	// Token: 0x04000E60 RID: 3680
	private bool cinemaPanLeft;

	// Token: 0x04000E61 RID: 3681
	private bool cinemaPanRight;

	// Token: 0x04000E62 RID: 3682
	private bool cinemaPanUp;

	// Token: 0x04000E63 RID: 3683
	private bool cinemaPanDown;

	// Token: 0x04000E64 RID: 3684
	private bool cinemaZoomIn;

	// Token: 0x04000E65 RID: 3685
	private bool cinemaZoomOut;

	// Token: 0x04000E66 RID: 3686
	private int cinemaZoomSpeed = 10;

	// Token: 0x04000E67 RID: 3687
	private float cinemaEasing = 0.05f;

	// Token: 0x04000E68 RID: 3688
	private float cinemaZoomVelocity;

	// Token: 0x04000E6A RID: 3690
	private float smoothDt;

	// Token: 0x02001104 RID: 4356
	public class Tuning : TuningData<CameraController.Tuning>
	{
		// Token: 0x04005B03 RID: 23299
		public float maxOrthographicSizeDebug;

		// Token: 0x04005B04 RID: 23300
		public float cinemaZoomFactor = 100f;

		// Token: 0x04005B05 RID: 23301
		public float cinemaPanFactor = 50f;

		// Token: 0x04005B06 RID: 23302
		public float cinemaZoomToFactor = 100f;

		// Token: 0x04005B07 RID: 23303
		public float cinemaPanToFactor = 50f;

		// Token: 0x04005B08 RID: 23304
		public float targetZoomEasingFactor = 400f;

		// Token: 0x04005B09 RID: 23305
		public float targetPanEasingFactor = 100f;
	}
}
