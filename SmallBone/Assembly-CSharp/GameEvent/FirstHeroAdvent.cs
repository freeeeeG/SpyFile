using System;
using System.Collections;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace GameEvent
{
	// Token: 0x020000F5 RID: 245
	public class FirstHeroAdvent : MonoBehaviour
	{
		// Token: 0x060004B9 RID: 1209 RVA: 0x0000F312 File Offset: 0x0000D512
		private void Start()
		{
			base.StartCoroutine(this.WaitForAdvent());
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000F321 File Offset: 0x0000D521
		private IEnumerator WaitForAdvent()
		{
			yield return Chronometer.global.WaitForSeconds(this._duration - this._notificationTime);
			PersistentSingleton<SoundManager>.Instance.FadeOutBackgroundMusic(4f);
			yield return Chronometer.global.WaitForSeconds(this._notificationTime);
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._backgroundMusic);
			this._spawned = UnityEngine.Object.Instantiate<GameObject>(this._firstHero);
			yield break;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000F330 File Offset: 0x0000D530
		private void OnDestroy()
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(Singleton<Service>.Instance.levelManager.currentChapter.currentStage.music, 1f, true, true, false);
			UnityEngine.Object.Destroy(this._spawned);
		}

		// Token: 0x0400039F RID: 927
		[SerializeField]
		private float _duration = 120f;

		// Token: 0x040003A0 RID: 928
		[SerializeField]
		private float _notificationTime = 10f;

		// Token: 0x040003A1 RID: 929
		[SerializeField]
		private GameObject _firstHero;

		// Token: 0x040003A2 RID: 930
		[SerializeField]
		private string _notifyKey;

		// Token: 0x040003A3 RID: 931
		[SerializeField]
		private MusicInfo _backgroundMusic;

		// Token: 0x040003A4 RID: 932
		private GameObject _spawned;
	}
}
