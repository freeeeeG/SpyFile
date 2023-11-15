using System;
using System.Collections;
using InControl;
using Platforms;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace Scenes
{
	// Token: 0x02000142 RID: 322
	public class Main : Scene<Main>
	{
		// Token: 0x0600066C RID: 1644 RVA: 0x00012E5A File Offset: 0x0001105A
		private void Awake()
		{
			this._loading.SetActive(false);
			this._gameLogo.SetActive(false);
			this._pressAnyKey.SetActive(false);
			this._username.text = PersistentSingleton<PlatformManager>.Instance.platform.userName;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00012E9A File Offset: 0x0001109A
		private void Start()
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(this._backgroundMusic, 1f, true, true, false);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00012EB4 File Offset: 0x000110B4
		public void LogoAnimationsCompleted()
		{
			base.StartCoroutine(this.CStartGameOnReady());
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00012EC3 File Offset: 0x000110C3
		private IEnumerator CStartGameOnReady()
		{
			yield return this.CWaitForBackgroundFadeIn();
			yield return this.CWaitForInput();
			this._loading.SetActive(true);
			this.StartGame();
			yield break;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00012ED2 File Offset: 0x000110D2
		private IEnumerator CWaitForBackgroundFadeIn()
		{
			this._gameLogo.SetActive(true);
			this._username.gameObject.SetActive(true);
			this._versionName.SetActive(true);
			yield return new WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00012EE1 File Offset: 0x000110E1
		private IEnumerator CWaitForInput()
		{
			GameResourceLoader.instance.WaitForStrings();
			yield return null;
			this._pressAnyKey.SetActive(true);
			while (!Input.anyKey && !InputManager.ActiveDevice.AnyButtonIsPressed)
			{
				yield return null;
			}
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			this._pressAnyKey.SetActive(false);
			yield break;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00012EF0 File Offset: 0x000110F0
		private void StartGame()
		{
			Singleton<Service>.Instance.fadeInOut.ShowLoadingIcon();
			GameResourceLoader.instance.WaitForCompletion();
			Singleton<Service>.Instance.levelManager.LoadGame();
			UnityEngine.Object.Destroy(this._container);
		}

		// Token: 0x040004BF RID: 1215
		[SerializeField]
		private GameObject _container;

		// Token: 0x040004C0 RID: 1216
		[SerializeField]
		private AudioClip _backgroundMusic;

		// Token: 0x040004C1 RID: 1217
		[SerializeField]
		private GameObject _loading;

		// Token: 0x040004C2 RID: 1218
		[SerializeField]
		private GameObject _gameLogo;

		// Token: 0x040004C3 RID: 1219
		[SerializeField]
		private GameObject _pressAnyKey;

		// Token: 0x040004C4 RID: 1220
		[SerializeField]
		private TMP_Text _username;

		// Token: 0x040004C5 RID: 1221
		[SerializeField]
		private GameObject _versionName;

		// Token: 0x040004C6 RID: 1222
		[SerializeField]
		private int _gameBaseSceneNumber;
	}
}
