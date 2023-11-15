using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200017B RID: 379
public class ScaleAndMove : IGameSystem
{
	// Token: 0x060009A7 RID: 2471 RVA: 0x00019A54 File Offset: 0x00017C54
	public void Initialize(MainUI mainUI)
	{
		this.m_MainUI = mainUI;
		this.cam = base.GetComponent<Camera>();
		this.camTr = this.cam.transform;
		this.CamViewPos = this.cam.transform.position;
		this.camInitPos = this.cam.transform.position;
		this.CamInitialSize = 6f;
		this.oldPosition = this.cam.transform.position;
		Input.multiTouchEnabled = true;
		this.MoveTurorial = false;
		this.SizeTutorial = false;
		this.StartCamAnim();
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00019AEC File Offset: 0x00017CEC
	private void StartCamAnim()
	{
		this.cam.DOOrthoSize(this.CamInitialSize, 2f);
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00019B05 File Offset: 0x00017D05
	private IEnumerator ShakeCor()
	{
		Time.timeScale = 0.3f;
		this.cam.DOShakePosition(this.shakeDuration, this.shakeStrength, 10, 90f, true);
		yield return new WaitForSecondsRealtime(1.2f);
		Time.timeScale = (float)GameRes.GameSpeed;
		yield break;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00019B14 File Offset: 0x00017D14
	public void ShakeCam()
	{
		base.StartCoroutine(this.ShakeCor());
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00019B24 File Offset: 0x00017D24
	public override void GameUpdate()
	{
		this.DesktopInput();
		this.TutorialCounter();
		this.BackgroundUpdate();
		base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, this.minX, this.maxX), Mathf.Clamp(base.transform.position.y, this.minY, this.maxY), base.transform.position.z);
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00019BA5 File Offset: 0x00017DA5
	private IEnumerator MoveCor()
	{
		this.CanControl = false;
		this.MoveTurorial = false;
		this.cam.transform.DOMove(new Vector3(0f, 0f, this.cam.transform.position.z), 1f, false);
		this.cam.DOOrthoSize(this.CamInitialSize, 1f);
		yield return new WaitForSeconds(1f);
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.MouseMove);
		yield break;
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00019BB4 File Offset: 0x00017DB4
	private void TutorialCounter()
	{
		if (this.MoveTurorial && Vector2.Distance(base.transform.position, this.oldPosition) > 4f)
		{
			base.StartCoroutine(this.MoveCor());
		}
		if (this.SizeTutorial && Mathf.Abs(this.cam.orthographicSize - this.CamInitialSize) > 1f)
		{
			this.SizeTutorial = false;
			this.cam.transform.DOMove(new Vector3(0f, 0f, this.cam.transform.position.z), 1f, false);
			this.cam.DOOrthoSize(this.CamInitialSize, 1f);
			Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.WheelMove);
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00019C8C File Offset: 0x00017E8C
	private void RTSView()
	{
		if (!this.CanControl)
		{
			return;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			this.cam.orthographicSize = Mathf.Clamp(this.cam.orthographicSize, this.minmum, this.maximum);
			this.cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * this.scrollSpeed;
		}
		this.speedHorizon = Vector3.zero;
		this.speedVertical = Vector3.zero;
		this.speed = Vector3.zero;
		Vector3 vector = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		if (vector.x < 0.01f && base.transform.localPosition.x > this.minX)
		{
			this.speedHorizon = Vector3.left * (float)this.slideSpeed * Time.deltaTime;
			return;
		}
		if (vector.x > 0.99f && base.transform.localPosition.x < this.maxX)
		{
			this.speedHorizon = Vector3.right * (float)this.slideSpeed * Time.deltaTime;
			return;
		}
		if (vector.y < 0.01f && base.transform.localPosition.y > this.minY)
		{
			this.speedVertical = Vector3.down * (float)this.slideSpeed * Time.deltaTime;
			return;
		}
		if (vector.y > 0.99f && base.transform.localPosition.y < this.maxY)
		{
			this.speedVertical = Vector3.up * (float)this.slideSpeed * Time.deltaTime;
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00019E4C File Offset: 0x0001804C
	private void DesktopInput()
	{
		if (!this.CanControl)
		{
			return;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			this.cam.orthographicSize = Mathf.Clamp(this.cam.orthographicSize, this.minmum, this.maximum);
			this.cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * this.scrollSpeed;
		}
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, StaticData.GetSelectLayer);
			if (raycastHit2D.collider != null)
			{
				raycastHit2D.collider.GetComponent<TileBase>().TileDown();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1f, StaticData.GetSelectLayer);
			if (raycastHit2D2.collider != null)
			{
				raycastHit2D2.collider.GetComponent<TileBase>().TileUp();
			}
		}
		if (DraggingActions.DraggingThis == null && Input.GetMouseButton(0))
		{
			this.Difference = this.cam.ScreenToWorldPoint(Input.mousePosition) - this.cam.transform.position;
			if (!this.Drag)
			{
				this.Drag = true;
				this.Origin = this.cam.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else
		{
			this.Drag = false;
		}
		if (this.Drag)
		{
			Vector3 b = this.Origin - this.Difference;
			Vector3 position = Vector3.Lerp(this.cam.transform.position, b, this.SmoothSpeed);
			this.cam.transform.position = position;
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0001A02C File Offset: 0x0001822C
	private void MobileInput()
	{
		if (Input.touchCount <= 0)
		{
			return;
		}
		if (Input.touchCount == 1)
		{
			if (Input.touches[0].phase == TouchPhase.Began)
			{
				this.m_ScreenPos = Input.touches[0].position;
				return;
			}
			if (Input.touches[0].phase == TouchPhase.Moved)
			{
				this.cam.transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x, -Input.touches[0].deltaPosition.y, 0f) * Time.deltaTime * 0.1f);
			}
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0001A0E4 File Offset: 0x000182E4
	public void LocatePos(Vector2 pos)
	{
		Vector3 endValue = new Vector3(pos.x, pos.y, this.cam.transform.position.z);
		this.cam.transform.DOMove(endValue, 0.5f, false);
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0001A134 File Offset: 0x00018334
	private void BackgroundUpdate()
	{
		Vector2 vector = (this.CamViewPos - this.camTr.position) * this.parallaxScale;
		for (int i = 0; i < this.backGrounds.Length; i++)
		{
			float x = this.backGrounds[i].position.x + vector.x * ((float)i * this.parallaxReductionFactor + 1f);
			float y = this.backGrounds[i].position.y + vector.y * ((float)i * this.parallaxReductionFactor + 1f);
			Vector3 b = new Vector3(x, y, this.backGrounds[i].position.z);
			this.backGrounds[i].position = Vector3.Lerp(this.backGrounds[i].position, b, this.smoothing * Time.deltaTime);
		}
		this.CamViewPos = this.camTr.position;
	}

	// Token: 0x040004ED RID: 1261
	private MainUI m_MainUI;

	// Token: 0x040004EE RID: 1262
	private Vector2 m_ScreenPos;

	// Token: 0x040004EF RID: 1263
	private Vector3 oldPosition;

	// Token: 0x040004F0 RID: 1264
	private Vector3 Origin;

	// Token: 0x040004F1 RID: 1265
	private Vector3 Difference;

	// Token: 0x040004F2 RID: 1266
	private float SmoothSpeed = 0.6f;

	// Token: 0x040004F3 RID: 1267
	private bool Drag;

	// Token: 0x040004F4 RID: 1268
	private Camera cam;

	// Token: 0x040004F5 RID: 1269
	private int slideSpeed = 6;

	// Token: 0x040004F6 RID: 1270
	private float scrollSpeed = 2.5f;

	// Token: 0x040004F7 RID: 1271
	private float maximum = 12f;

	// Token: 0x040004F8 RID: 1272
	private float minmum = 3f;

	// Token: 0x040004F9 RID: 1273
	private float maxY = 11f;

	// Token: 0x040004FA RID: 1274
	private float minY = -11f;

	// Token: 0x040004FB RID: 1275
	private float minX = -11f;

	// Token: 0x040004FC RID: 1276
	private float maxX = 11f;

	// Token: 0x040004FD RID: 1277
	private Vector2 CamMovement;

	// Token: 0x040004FE RID: 1278
	private Vector3 camInitPos;

	// Token: 0x040004FF RID: 1279
	private Vector3 CamViewPos;

	// Token: 0x04000500 RID: 1280
	private float CamInitialSize;

	// Token: 0x04000501 RID: 1281
	public bool MoveTurorial;

	// Token: 0x04000502 RID: 1282
	public bool SizeTutorial;

	// Token: 0x04000503 RID: 1283
	public bool CanControl = true;

	// Token: 0x04000504 RID: 1284
	private Vector3 speedHorizon = Vector3.zero;

	// Token: 0x04000505 RID: 1285
	private Vector3 speedVertical = Vector3.zero;

	// Token: 0x04000506 RID: 1286
	private Vector3 speed = Vector3.zero;

	// Token: 0x04000507 RID: 1287
	[Header("视差控制")]
	[SerializeField]
	private Transform[] backGrounds;

	// Token: 0x04000508 RID: 1288
	[SerializeField]
	private float parallaxScale;

	// Token: 0x04000509 RID: 1289
	[SerializeField]
	private float parallaxReductionFactor;

	// Token: 0x0400050A RID: 1290
	[SerializeField]
	private float smoothing;

	// Token: 0x0400050B RID: 1291
	[Header("摄像机晃动")]
	[SerializeField]
	private float shakeDuration;

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	private float shakeStrength;

	// Token: 0x0400050D RID: 1293
	private Transform camTr;
}
