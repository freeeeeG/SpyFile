using System;
using System.Collections;
using Characters;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Adventurer
{
	// Token: 0x0200068F RID: 1679
	public class AdventurerCameraZone : MonoBehaviour
	{
		// Token: 0x0600218D RID: 8589 RVA: 0x00064D3F File Offset: 0x00062F3F
		private void Awake()
		{
			this._enemyWave.onClear += this.DisableCameraZone;
			this._trackSpeedCached = Scene<GameBase>.instance.cameraController.trackSpeed;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00064D70 File Offset: 0x00062F70
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

		// Token: 0x0600218F RID: 8591 RVA: 0x00064D9E File Offset: 0x00062F9E
		private void EnableCameraZone()
		{
			Scene<GameBase>.instance.cameraController.trackSpeed = this._cameraTrackSpeed;
			Scene<GameBase>.instance.cameraController.StartTrack(this._cameraTarget);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x00064DCC File Offset: 0x00062FCC
		public void DisableCameraZone()
		{
			Scene<GameBase>.instance.cameraController.trackSpeed = 0.05f;
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
			base.StartCoroutine(this.CReturnTrackSpeed());
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00064E1D File Offset: 0x0006301D
		private IEnumerator CReturnTrackSpeed()
		{
			yield return Chronometer.global.WaitForSeconds(2f);
			Scene<GameBase>.instance.cameraController.trackSpeed = this._trackSpeedCached;
			yield break;
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x00064E2C File Offset: 0x0006302C
		private void OnDestroy()
		{
			if (Scene<GameBase>.instance.cameraController == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.trackSpeed = this._trackSpeedCached;
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return;
			}
			Scene<GameBase>.instance.cameraController.StartTrack(Singleton<Service>.Instance.levelManager.player.transform);
		}

		// Token: 0x04001CA2 RID: 7330
		[SerializeField]
		private Transform _cameraTarget;

		// Token: 0x04001CA3 RID: 7331
		[SerializeField]
		private float _cameraTrackSpeed = 3f;

		// Token: 0x04001CA4 RID: 7332
		[SerializeField]
		private EnemyWave _enemyWave;

		// Token: 0x04001CA5 RID: 7333
		[SerializeField]
		[GetComponent]
		private BoxCollider2D _startTrigger;

		// Token: 0x04001CA6 RID: 7334
		private float _trackSpeedCached;
	}
}
