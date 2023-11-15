using System;
using System.Collections;
using Characters.Controllers;
using Data;
using GameResources;
using Level;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UserInput;

namespace Characters.Player
{
	// Token: 0x020007EA RID: 2026
	public class HandlePlayerDeath : MonoBehaviour
	{
		// Token: 0x06002901 RID: 10497 RVA: 0x0007C9A7 File Offset: 0x0007ABA7
		private void Awake()
		{
			this._player = base.GetComponent<Character>();
			this._gameBase = Scene<GameBase>.instance;
			this._player.health.onDiedTryCatch += this.OnPlayerDied;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0007C9DC File Offset: 0x0007ABDC
		private void OnPlayerDied()
		{
			GameData.Progress.deaths++;
			GameData.Save.instance.hasSave = false;
			GameData.Progress.SaveAll();
			GameData.Save.instance.SaveAll();
			GameData.Currency.SaveAll();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
			PlayerDieHeadParts component = CommonResource.instance.playerDieHeadParts.parts.poolObject.Spawn(true).GetComponent<PlayerDieHeadParts>();
			component.transform.parent = Map.Instance.transform;
			DroppedParts parts = component.parts;
			component.transform.position = this._player.transform.position;
			component.sprite = this._player.playerComponents.inventory.weapon.polymorphOrCurrent.icon;
			parts.Initialize(this._player.movement.push, 1f, true);
			this._gameBase.cameraController.StartTrack(component.transform);
			this._gameBase.cameraController.Zoom(0.8f, 1f);
			CoroutineProxy.instance.StartCoroutine(this.CWaitForGameResult());
			this._slowMotionReference = CoroutineProxy.instance.StartCoroutineWithReference(this.CSlowMotion());
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0007CB0B File Offset: 0x0007AD0B
		private IEnumerator CWaitForGameResult()
		{
			this._gameBase.uiManager.pauseEventSystem.PushEmpty();
			PlayerInput.blocked.Attach(this);
			yield return new WaitForSecondsRealtime(2f);
			this._gameBase.uiManager.gameResult.Show();
			while (!this._gameBase.uiManager.gameResult.animationFinished || (!KeyMapper.Map.Attack.WasPressed && !KeyMapper.Map.Submit.WasPressed))
			{
				yield return null;
			}
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			this._gameBase.uiManager.gameResult.Hide();
			PlayerInput.blocked.Detach(this);
			this._gameBase.uiManager.pauseEventSystem.PopEvent();
			this._slowMotionReference.Stop();
			Chronometer.global.DetachTimeScale(this);
			if (GameData.HardmodeProgress.hardmode)
			{
				Achievement.Type.NewBeginning.Set();
			}
			Singleton<Service>.Instance.levelManager.ResetGame();
			yield break;
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0007CB1A File Offset: 0x0007AD1A
		private IEnumerator CSlowMotion()
		{
			Chronometer.global.AttachTimeScale(this, 0.1f);
			yield return Chronometer.global.WaitForSeconds(0.2f);
			Chronometer.global.AttachTimeScale(this, 0.3f);
			yield return Chronometer.global.WaitForSeconds(0.2f);
			Chronometer.global.AttachTimeScale(this, 0.5f);
			yield return Chronometer.global.WaitForSeconds(0.2f);
			Chronometer.global.AttachTimeScale(this, 0.7f);
			yield return Chronometer.global.WaitForSeconds(0.2f);
			Chronometer.global.AttachTimeScale(this, 0.9f);
			yield return Chronometer.global.WaitForSeconds(0.2f);
			Chronometer.global.DetachTimeScale(this);
			yield break;
		}

		// Token: 0x0400236B RID: 9067
		private Character _player;

		// Token: 0x0400236C RID: 9068
		private GameBase _gameBase;

		// Token: 0x0400236D RID: 9069
		private CoroutineReference _slowMotionReference;
	}
}
