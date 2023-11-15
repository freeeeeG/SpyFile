using System;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000055 RID: 85
public class CameraController : MonoBehaviour
{
	// Token: 0x060001CC RID: 460 RVA: 0x00007C6C File Offset: 0x00005E6C
	private void Start()
	{
		if (this.camera == null)
		{
			this.camera = base.GetComponent<Camera>();
		}
		if (this.camera.orthographic)
		{
			this.camDefaultOrthoSize = this.camera.orthographicSize;
			this.camTargetOrthoSize = this.camDefaultOrthoSize;
		}
		else
		{
			this.camDefaultFOV = this.camera.fieldOfView;
			this.camTargetFOV = this.camDefaultFOV;
		}
		this.startPos = this.camera.transform.position;
		this.targetCameraLerpPosition = this.camera.transform.position;
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00007D08 File Offset: 0x00005F08
	public void OverrideFOV(float fov)
	{
		this.camTargetFOV = fov;
		this.camDefaultFOV = fov;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00007D18 File Offset: 0x00005F18
	public void SetCameraPositionByOffset(Vector3 offset, bool isImmediate)
	{
		if (isImmediate)
		{
			this.camera.transform.position = this.startPos + offset;
		}
		this.targetCameraLerpPosition = this.startPos + offset;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00007D4C File Offset: 0x00005F4C
	private void OnEnable()
	{
		EventMgr.Register<int, float>(eGameEvents.UI_ShowStageAnnounce, new Action<int, float>(this.OnShowStageAnnounce));
		if (this.doOverrideMainCamera)
		{
			if (this.camera == null)
			{
				this.camera = base.GetComponent<Camera>();
			}
			Singleton<CameraManager>.Instance.OverrideCamera(CameraManager.eCameraType.MAIN_CAMERA, this.camera);
		}
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00007DA4 File Offset: 0x00005FA4
	private void OnDisable()
	{
		EventMgr.Remove<int, float>(eGameEvents.UI_ShowStageAnnounce, new Action<int, float>(this.OnShowStageAnnounce));
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00007DBE File Offset: 0x00005FBE
	private void OnShowStageAnnounce(int index, float duration)
	{
		this.camera.fieldOfView = 90f;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00007DD0 File Offset: 0x00005FD0
	private void Update()
	{
		if (Singleton<GameStateController>.Instance.IsCurrentState(eGameState.PAUSE_GAME))
		{
			return;
		}
		this.isMouseInWindow = RefiUtilities.IsMouseInsideWindow();
		if (this.camera.orthographic)
		{
			if (this.isMouseInWindow && Input.mouseScrollDelta != Vector2.zero)
			{
				this.camTargetOrthoSize += -1f * Input.mouseScrollDelta.y;
				this.camTargetOrthoSize = Mathf.Clamp(this.camTargetOrthoSize, this.camOrthoSizeRange.x, this.camOrthoSizeRange.y);
			}
			if (Mathf.Abs(this.camera.orthographicSize - this.camTargetOrthoSize) > 0.01f)
			{
				this.camera.orthographicSize = Mathf.Lerp(this.camera.orthographicSize, this.camTargetOrthoSize, this.screenScaleLerpRate * Time.deltaTime);
			}
			else
			{
				this.camera.orthographicSize = this.camTargetOrthoSize;
			}
		}
		else
		{
			if (this.isMouseInWindow && Input.mouseScrollDelta != Vector2.zero)
			{
				this.camTargetFOV += -1f * Input.mouseScrollDelta.y;
				this.camTargetFOV = Mathf.Clamp(this.camTargetFOV, this.camPerspectiveFOVScaleRange.x, this.camPerspectiveFOVScaleRange.y);
			}
			if (Mathf.Abs(this.camera.orthographicSize - this.camTargetOrthoSize) > 0.01f)
			{
				this.camera.fieldOfView = Mathf.Lerp(this.camera.fieldOfView, this.camTargetFOV, this.screenScaleLerpRate * Time.deltaTime);
			}
			else
			{
				this.camera.fieldOfView = this.camTargetFOV;
			}
		}
		if (this.isMouseInWindow)
		{
			if (this.camera.orthographic)
			{
				if (Input.GetMouseButtonDown(1))
				{
					if (!LeanTouch.PointOverGui(Input.mousePosition))
					{
						this.isMovingCamera = true;
						this.lastRightClickOrigin = this.camera.ScreenToWorldPoint(Input.mousePosition);
					}
				}
				else if (Input.GetMouseButton(1))
				{
					if (this.isMovingCamera)
					{
						this.rightClickDiff = this.camera.ScreenToWorldPoint(Input.mousePosition) - this.camera.transform.position;
						this.targetCameraLerpPosition = this.lastRightClickOrigin - this.rightClickDiff;
						this.targetCameraLerpPosition = this.LimitPositionInRange(this.targetCameraLerpPosition);
					}
				}
				else if (Input.GetMouseButtonUp(1))
				{
					this.isMovingCamera = false;
				}
			}
			else if (Input.GetMouseButtonDown(1))
			{
				if (!LeanTouch.PointOverGui(Input.mousePosition))
				{
					this.isMovingCamera = true;
					this.cameraDistanceToGround = this.GetDistanceToPlane(Vector3.zero, Vector3.up);
					this.lastRightClickOrigin = this.camera.ScreenToWorldPoint(Input.mousePosition.WithZ(this.cameraDistanceToGround));
				}
			}
			else if (Input.GetMouseButton(1))
			{
				if (this.isMovingCamera)
				{
					this.cameraDistanceToGround = this.GetDistanceToPlane(Vector3.zero, Vector3.up);
					this.rightClickDiff = this.camera.ScreenToWorldPoint(Input.mousePosition.WithZ(this.cameraDistanceToGround)) - this.camera.transform.position;
					float y = this.camera.transform.position.y;
					this.targetCameraLerpPosition = this.lastRightClickOrigin - this.rightClickDiff;
					this.targetCameraLerpPosition = this.targetCameraLerpPosition.WithY(y);
					this.targetCameraLerpPosition = this.LimitPositionInRange(this.targetCameraLerpPosition);
				}
			}
			else if (Input.GetMouseButtonUp(1))
			{
				this.isMovingCamera = false;
			}
		}
		this.camera.transform.position = Vector3.Lerp(this.camera.transform.position, this.targetCameraLerpPosition, this.positionLerpSpeed * Time.unscaledDeltaTime);
		Vector3 vector = Vector3.zero;
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			vector += Vector3.forward;
			vector += Vector3.right;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			vector += Vector3.back;
			vector += Vector3.left;
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			vector += Vector3.forward;
			vector += Vector3.left;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			vector += Vector3.back;
			vector += Vector3.right;
		}
		if (vector != Vector3.zero)
		{
			this.targetCameraLerpPosition += vector * Time.deltaTime * 10f;
			this.targetCameraLerpPosition = this.LimitPositionInRange(this.targetCameraLerpPosition);
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000082BC File Offset: 0x000064BC
	private Vector3 LimitPositionInRange(Vector3 pos)
	{
		Vector3 vector = pos - this.startPos;
		if (vector.magnitude > this.rangeLimit)
		{
			pos = this.startPos + vector.normalized * this.rangeLimit;
		}
		return pos;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00008308 File Offset: 0x00006508
	private float GetDistanceToPlane(Vector3 planePosition, Vector3 planeNormal)
	{
		Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
		float result = 0f;
		if (new Plane(planeNormal, planePosition).Raycast(ray, out result))
		{
			return result;
		}
		return 0f;
	}

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private bool doOverrideMainCamera = true;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private float rangeLimit;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	[FormerlySerializedAs("orthoLerpRate")]
	private float screenScaleLerpRate = 3f;

	// Token: 0x0400015B RID: 347
	[SerializeField]
	private float positionLerpSpeed = 5f;

	// Token: 0x0400015C RID: 348
	[Header("[Ortho]")]
	[SerializeField]
	private Vector2 camOrthoSizeRange;

	// Token: 0x0400015D RID: 349
	private float camDefaultOrthoSize = -1f;

	// Token: 0x0400015E RID: 350
	private float camTargetOrthoSize = -1f;

	// Token: 0x0400015F RID: 351
	[Header("[Perspective]")]
	[SerializeField]
	private Vector2 camPerspectiveFOVScaleRange;

	// Token: 0x04000160 RID: 352
	private float camDefaultFOV = -1f;

	// Token: 0x04000161 RID: 353
	private float camTargetFOV = -1f;

	// Token: 0x04000162 RID: 354
	private Camera camera;

	// Token: 0x04000163 RID: 355
	private Vector3 lastRightClickOrigin;

	// Token: 0x04000164 RID: 356
	private Vector3 rightClickDiff;

	// Token: 0x04000165 RID: 357
	private float cameraDistanceToGround;

	// Token: 0x04000166 RID: 358
	private Vector3 targetCameraLerpPosition;

	// Token: 0x04000167 RID: 359
	private Vector3 startPos;

	// Token: 0x04000168 RID: 360
	private Vector3 lastFramePosition;

	// Token: 0x04000169 RID: 361
	private bool isMovingCamera;

	// Token: 0x0400016A RID: 362
	private bool isMouseInWindow;
}
