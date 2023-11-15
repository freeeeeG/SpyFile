using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class CameraManager : Singleton<CameraManager>
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x000161FE File Offset: 0x000143FE
	public Camera MainCamera
	{
		get
		{
			return this.mainCamera;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00016206 File Offset: 0x00014406
	public Camera UICamera
	{
		get
		{
			return this.uiCamera;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001620E File Offset: 0x0001440E
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600058D RID: 1421 RVA: 0x00016216 File Offset: 0x00014416
	public Camera PhotoCamera
	{
		get
		{
			return this.photoCamera;
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00016220 File Offset: 0x00014420
	private void OnEnable()
	{
		EventMgr.Register<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, new Action<GameSceneReferenceHandler>(this.OnUpdateEnvSceneBindings));
		EventMgr.Register<Vector3, bool>(eGameEvents.MoveCameraToOffset, new Action<Vector3, bool>(this.OnMoveCameraToOffset));
		EventMgr.Register<CameraManager.eCameraShakeStrength>(eGameEvents.RequestCameraShake, new Action<CameraManager.eCameraShakeStrength>(this.OnRequestCameraShake));
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00016274 File Offset: 0x00014474
	private void OnDisable()
	{
		EventMgr.Remove<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, new Action<GameSceneReferenceHandler>(this.OnUpdateEnvSceneBindings));
		EventMgr.Remove<Vector3, bool>(eGameEvents.MoveCameraToOffset, new Action<Vector3, bool>(this.OnMoveCameraToOffset));
		EventMgr.Remove<CameraManager.eCameraShakeStrength>(eGameEvents.RequestCameraShake, new Action<CameraManager.eCameraShakeStrength>(this.OnRequestCameraShake));
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x000162C6 File Offset: 0x000144C6
	private void OnMoveCameraToOffset(Vector3 offset, bool isImmediate)
	{
		this.mainCamera.GetComponent<CameraController>().SetCameraPositionByOffset(offset, isImmediate);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x000162DA File Offset: 0x000144DA
	private void OnUpdateEnvSceneBindings(GameSceneReferenceHandler refHandler)
	{
		this.OnMoveCameraToOffset(refHandler.InitialCameraOffset, true);
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x000162EC File Offset: 0x000144EC
	private void OnRequestCameraShake(CameraManager.eCameraShakeStrength strength)
	{
		switch (strength)
		{
		case CameraManager.eCameraShakeStrength.Weak:
			this.ShakeCamera(0.05f, 0.005f, 0f);
			return;
		case CameraManager.eCameraShakeStrength.Normal:
			this.ShakeCamera(0.1f, 0.005f, 0f);
			return;
		case CameraManager.eCameraShakeStrength.Strong:
			this.ShakeCamera(0.25f, 0.005f, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0001634D File Offset: 0x0001454D
	private void OnDestroy()
	{
		if (this.photoCameraController != null)
		{
			this.photoCameraController.ClearPhotos();
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00016368 File Offset: 0x00014568
	public void OverrideCamera(CameraManager.eCameraType type, Camera camera)
	{
		if (type == CameraManager.eCameraType.MAIN_CAMERA)
		{
			this.mainCamera = camera;
			this.shakeCamera_Main = camera.transform.parent.gameObject.GetComponent<ShakeCamera>();
			return;
		}
		if (type != CameraManager.eCameraType.UI_CAMERA)
		{
			return;
		}
		this.uiCamera = camera;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0001639C File Offset: 0x0001459C
	public static void SwitchCamera(CameraManager.eVirtualCameraType type)
	{
		Singleton<CameraManager>.Instance.switchCamera(type);
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x000163AC File Offset: 0x000145AC
	private void switchCamera(CameraManager.eVirtualCameraType type)
	{
		if (this.currentCamera != null)
		{
			this.currentCamera.gameObject.SetActive(false);
		}
		this.dic_VCameras[type].gameObject.SetActive(true);
		this.currentCamera = this.dic_VCameras[type];
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00016404 File Offset: 0x00014604
	public Vector3 Calculate2DPosFrom3DPos(Vector3 worldPos)
	{
		Vector3 result = this.uiCamera.ViewportToWorldPoint(this.mainCamera.WorldToViewportPoint(worldPos));
		result.z = this.uiCamera.transform.position.z + this.canvas.planeDistance;
		return result;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00016452 File Offset: 0x00014652
	public Vector3 CalculateScreenPos(Vector3 worldPos)
	{
		return this.uiCamera.WorldToScreenPoint(worldPos);
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00016460 File Offset: 0x00014660
	public Vector3 ScreenPosToUIPos(Vector3 screenPos)
	{
		Vector3 result = this.uiCamera.ScreenToWorldPoint(screenPos);
		result.z = this.uiCamera.transform.position.z + this.canvas.planeDistance;
		return result;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x000164A3 File Offset: 0x000146A3
	public Vector3 CalculateViewportPos(Vector3 worldPos)
	{
		return this.uiCamera.WorldToViewportPoint(worldPos);
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x000164B4 File Offset: 0x000146B4
	public Vector3 EnsureUIStaysInLRBorder(Vector3 worldPos, float width)
	{
		Vector3 b = Vector3.right * 0.5f * width;
		Vector3 vector = this.uiCamera.WorldToViewportPoint(worldPos - b);
		if (vector.x < 0f)
		{
			vector.x = 0f;
			Vector3 vector2 = this.uiCamera.ViewportToWorldPoint(vector) + b;
			vector2.z = this.uiCamera.transform.position.z + this.canvas.planeDistance;
			Debug.DrawLine(worldPos, vector2, Color.red, 3f);
			return vector2;
		}
		Vector3 vector3 = this.uiCamera.WorldToViewportPoint(worldPos + b);
		if (vector3.x > 1f)
		{
			vector3.x = 1f;
			Vector3 vector2 = this.uiCamera.ViewportToWorldPoint(vector3) - b;
			vector2.z = this.uiCamera.transform.position.z + this.canvas.planeDistance;
			Debug.DrawLine(worldPos, vector2, Color.red, 3f);
			return vector2;
		}
		return worldPos;
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x000165D0 File Offset: 0x000147D0
	public bool IsInScreen(Vector3 worldPos, float marginOffset = 0f)
	{
		Vector3 vector = this.mainCamera.WorldToViewportPoint(worldPos);
		return vector.x >= 0f - marginOffset && vector.x <= 1f + marginOffset && vector.y >= 0f - marginOffset && vector.y <= 1f + marginOffset;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0001662B File Offset: 0x0001482B
	public Vector3 GetMouseWorldPos()
	{
		return this.MousePosToWorldPos(Input.mousePosition);
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00016638 File Offset: 0x00014838
	public Vector3 MousePosToWorldPos(Vector3 mousePos)
	{
		return this.uiCamera.ScreenToWorldPoint(mousePos);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00016648 File Offset: 0x00014848
	public void ShakeCamera(float intensity, float decay, float delay = 0f)
	{
		if (this.cameraShakeIntensitySetting == -1f)
		{
			this.cameraShakeIntensitySetting = (float)PlayerPrefs.GetInt("GAME_CAMERA_SHAKE", 100) / 100f;
		}
		if (Singleton<GameStateController>.Instance.IsCurrentState(eGameState.EDIT_MODE))
		{
			return;
		}
		if (this.cameraShakeIntensitySetting <= 0f)
		{
			return;
		}
		if (delay <= 0f)
		{
			this.shakeCamera_Main.DoShake(intensity * this.cameraShakeIntensitySetting, decay);
			return;
		}
		base.StartCoroutine(this.CR_ShakeCamera(intensity * this.cameraShakeIntensitySetting, decay, delay));
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x000166CB File Offset: 0x000148CB
	private IEnumerator CR_ShakeCamera(float intensity, float decay, float delay)
	{
		Camera camera = this.mainCamera;
		yield return new WaitForSeconds(delay);
		if (camera == this.mainCamera)
		{
			this.shakeCamera_Main.DoShake(intensity, decay);
		}
		yield break;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x000166EF File Offset: 0x000148EF
	public void TakePhoto()
	{
		this.photoCameraController.TakePhoto();
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x000166FC File Offset: 0x000148FC
	public int GetPhotoCount()
	{
		return this.photoCameraController.GetPhotoCount();
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00016709 File Offset: 0x00014909
	public void ClearPhotos()
	{
		this.photoCameraController.ClearPhotos();
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00016716 File Offset: 0x00014916
	public RenderTexture GetPhoto(int index)
	{
		return this.photoCameraController.GetPhoto(index);
	}

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	private Camera mainCamera;

	// Token: 0x0400050D RID: 1293
	[SerializeField]
	private Camera uiCamera;

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	private Canvas canvas;

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private Camera photoCamera;

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private PhotoCameraController photoCameraController;

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private CameraManager.VCamDictionary dic_VCameras;

	// Token: 0x04000512 RID: 1298
	private CinemachineVirtualCamera currentCamera;

	// Token: 0x04000513 RID: 1299
	private ShakeCamera shakeCamera_Main;

	// Token: 0x04000514 RID: 1300
	private float cameraShakeIntensitySetting = -1f;

	// Token: 0x0200024C RID: 588
	[Serializable]
	public class VCamDictionary : SerializableDictionary<CameraManager.eVirtualCameraType, CinemachineVirtualCamera>
	{
	}

	// Token: 0x0200024D RID: 589
	public enum eCameraType
	{
		// Token: 0x04000B48 RID: 2888
		MAIN_CAMERA,
		// Token: 0x04000B49 RID: 2889
		UI_CAMERA
	}

	// Token: 0x0200024E RID: 590
	public enum eVirtualCameraType
	{
		// Token: 0x04000B4B RID: 2891
		NONE,
		// Token: 0x04000B4C RID: 2892
		TITLE,
		// Token: 0x04000B4D RID: 2893
		LEVEL_UP,
		// Token: 0x04000B4E RID: 2894
		INGAME
	}

	// Token: 0x0200024F RID: 591
	public enum eCameraShakeStrength
	{
		// Token: 0x04000B50 RID: 2896
		Weak,
		// Token: 0x04000B51 RID: 2897
		Normal,
		// Token: 0x04000B52 RID: 2898
		Strong
	}
}
