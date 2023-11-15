using System;
using Characters.Player;
using Data;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Token: 0x0200000E RID: 14
public class CameraController : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000029 RID: 41 RVA: 0x00002D36 File Offset: 0x00000F36
	// (set) Token: 0x0600002A RID: 42 RVA: 0x00002D3E File Offset: 0x00000F3E
	public float trackSpeed
	{
		get
		{
			return this._trackSpeed;
		}
		set
		{
			this._trackSpeed = value;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600002B RID: 43 RVA: 0x00002D47 File Offset: 0x00000F47
	public Vector3 delta
	{
		get
		{
			if (!this.pause)
			{
				return this._delta;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600002C RID: 44 RVA: 0x00002D5D File Offset: 0x00000F5D
	public PixelPerfectCamera pixelPerfectcamera
	{
		get
		{
			return this._pixelPerfectCamera;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00002D65 File Offset: 0x00000F65
	public Camera camera
	{
		get
		{
			return this._camera;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600002E RID: 46 RVA: 0x00002D6D File Offset: 0x00000F6D
	public float zoom
	{
		get
		{
			return this._pixelPerfectCamera.zoom;
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002D7C File Offset: 0x00000F7C
	private void Awake()
	{
		this._camera.transparencySortMode = TransparencySortMode.Orthographic;
		this._position = base.transform.position;
		this._moveEase = new EasingFunction(this._moveEaseMethod);
		this._zoomEase = new EasingFunction(this._zoomEaseMethod);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002DC8 File Offset: 0x00000FC8
	public void Update()
	{
		if (this.pause)
		{
			this._position = base.transform.position;
			return;
		}
		Vector3 vector;
		if (this._targetToTrack)
		{
			if (this._playerCameraController == null)
			{
				vector = Vector3.Lerp(this._position, this._targetToTrack.position + this._offset, Time.unscaledDeltaTime * this._trackSpeed);
			}
			else
			{
				vector = Vector3.Lerp(this._position, this._playerCameraController.trackPosition, Time.unscaledDeltaTime * this._playerCameraController.trackSpeed);
			}
		}
		else
		{
			this._moveTime += Time.unscaledDeltaTime * this._moveSpeed;
			if (this._moveTime >= 1f)
			{
				vector = this._targetPosition;
			}
			else
			{
				float t = this._moveEase.function(0f, 1f, this._moveTime);
				vector = Vector3.LerpUnclamped(this._position, this._targetPosition, t);
			}
		}
		if (this._pixelPerfectCamera != null)
		{
			this._zoomTime += Time.unscaledDeltaTime * this._zoomSpeed;
			if (this._zoomTime >= 1f)
			{
				this._pixelPerfectCamera.zoom = this._targetZoom;
			}
			else
			{
				this._pixelPerfectCamera.zoom = this._zoomEase.function(this._formerZoom, this._targetZoom, this._zoomTime);
			}
		}
		vector.z = this._position.z;
		Vector3 position = this._position;
		CameraZone cameraZone = this.zone;
		this._position = ((cameraZone != null) ? cameraZone.GetClampedPosition(this._camera, vector) : vector);
		this._delta = this._position - position;
		this._timeToNextShake -= Time.deltaTime;
		if (this._timeToNextShake < 0f)
		{
			this._shakeAmount = UnityEngine.Random.insideUnitSphere * this.shake.value * GameData.Settings.cameraShakeIntensity;
			this._shakeAmount *= 2f;
			this._timeToNextShake = 0.016666668f;
		}
		base.transform.position = this._position + this._shakeAmount;
		this.shake.Update();
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003014 File Offset: 0x00001214
	public void StartTrack(Transform target)
	{
		this._playerCameraController = target.GetComponent<PlayerCameraController>();
		this._targetToTrack = target;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003029 File Offset: 0x00001229
	public void StopTrack()
	{
		this._targetToTrack = null;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003034 File Offset: 0x00001234
	public void Move(Vector3 position)
	{
		position += this._offset;
		position.z = this._position.z;
		this._position = (this._targetPosition = position);
		this._moveTime = 0f;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000307C File Offset: 0x0000127C
	public void Zoom(float percent, float zoomSpeed = 1f)
	{
		this._targetZoom = percent;
		this._zoomSpeed = zoomSpeed;
		this._zoomTime = 0f;
		this._formerZoom = this._pixelPerfectCamera.zoom;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000030A8 File Offset: 0x000012A8
	public void RenderEndingScene()
	{
		this._playerCameraController.RenderDeathCamera();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x000030B5 File Offset: 0x000012B5
	public void Shake(float amount, float duration)
	{
		this.shake.Attach(this, amount, duration);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x000030C5 File Offset: 0x000012C5
	public void UpdateCameraPosition()
	{
		this._position = this._camera.gameObject.transform.position;
	}

	// Token: 0x04000020 RID: 32
	private const int _shakeBaseFps = 60;

	// Token: 0x04000021 RID: 33
	private const float _maxShakeInterval = 0.016666668f;

	// Token: 0x04000022 RID: 34
	[NonSerialized]
	public bool pause;

	// Token: 0x04000023 RID: 35
	[SerializeField]
	[GetComponent]
	private Camera _camera;

	// Token: 0x04000024 RID: 36
	[GetComponent]
	[SerializeField]
	private PixelPerfectCamera _pixelPerfectCamera;

	// Token: 0x04000025 RID: 37
	[SerializeField]
	private Vector3 _offset;

	// Token: 0x04000026 RID: 38
	[SerializeField]
	private EasingFunction.Method _moveEaseMethod = EasingFunction.Method.Linear;

	// Token: 0x04000027 RID: 39
	[SerializeField]
	private float _moveSpeed = 1f;

	// Token: 0x04000028 RID: 40
	private float _moveTime;

	// Token: 0x04000029 RID: 41
	private EasingFunction _moveEase;

	// Token: 0x0400002A RID: 42
	[SerializeField]
	private EasingFunction.Method _zoomEaseMethod = EasingFunction.Method.Linear;

	// Token: 0x0400002B RID: 43
	[SerializeField]
	private float _zoomSpeed = 1f;

	// Token: 0x0400002C RID: 44
	private float _zoomTime;

	// Token: 0x0400002D RID: 45
	private float _formerZoom = 1f;

	// Token: 0x0400002E RID: 46
	private float _targetZoom = 1f;

	// Token: 0x0400002F RID: 47
	private EasingFunction _zoomEase;

	// Token: 0x04000030 RID: 48
	[SerializeField]
	private float _trackSpeed = 1f;

	// Token: 0x04000031 RID: 49
	private Transform _targetToTrack;

	// Token: 0x04000032 RID: 50
	private Vector3 _targetPosition;

	// Token: 0x04000033 RID: 51
	private Vector3 _position;

	// Token: 0x04000034 RID: 52
	private Vector3 _delta;

	// Token: 0x04000035 RID: 53
	private Vector3 _shakeAmount;

	// Token: 0x04000036 RID: 54
	private float _timeToNextShake;

	// Token: 0x04000037 RID: 55
	private PlayerCameraController _playerCameraController;

	// Token: 0x04000038 RID: 56
	public readonly MaxOnlyTimedFloats shake = new MaxOnlyTimedFloats();

	// Token: 0x04000039 RID: 57
	public CameraZone zone;
}
