using System;
using Characters;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class CameraTargetChanger : MonoBehaviour
{
	// Token: 0x06000039 RID: 57 RVA: 0x00003149 File Offset: 0x00001349
	private void Awake()
	{
		this._trackSpeedCached = Scene<GameBase>.instance.cameraController.trackSpeed;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003160 File Offset: 0x00001360
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character component = collision.GetComponent<Character>();
		if (component == null)
		{
			return;
		}
		if (component.type != Character.Type.Player)
		{
			return;
		}
		this.EnableCameraZone();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003190 File Offset: 0x00001390
	private void OnTriggerExit2D(Collider2D collision)
	{
		Character component = collision.GetComponent<Character>();
		if (component == null)
		{
			return;
		}
		if (component.type != Character.Type.Player)
		{
			return;
		}
		this.DisableCameraZone();
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000031BE File Offset: 0x000013BE
	private void EnableCameraZone()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = this._cameraTrackSpeed;
		Scene<GameBase>.instance.cameraController.StartTrack(this._cameraTarget);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000031EA File Offset: 0x000013EA
	private void DisableCameraZone()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = this._trackSpeedCached;
		Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003224 File Offset: 0x00001424
	private void OnDestroy()
	{
		if (Scene<GameBase>.instance)
		{
			this.DisableCameraZone();
		}
	}

	// Token: 0x0400003A RID: 58
	[SerializeField]
	private Transform _cameraTarget;

	// Token: 0x0400003B RID: 59
	[SerializeField]
	private float _cameraTrackSpeed = 3f;

	// Token: 0x0400003C RID: 60
	[GetComponent]
	[SerializeField]
	private BoxCollider2D _startTrigger;

	// Token: 0x0400003D RID: 61
	private float _trackSpeedCached;

	// Token: 0x0400003E RID: 62
	private Coroutine _returnTrackSpeedModifier;
}
