using System;
using Data;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platforms
{
	// Token: 0x02000149 RID: 329
	public class PlatformUserChangedHandler : MonoBehaviour
	{
		// Token: 0x0600068F RID: 1679 RVA: 0x000131DB File Offset: 0x000113DB
		private void Start()
		{
			this._platformManager.onUserChanged += this.OnUserChanged;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x000131F4 File Offset: 0x000113F4
		private void OnUserChanged()
		{
			SceneManager.LoadScene("Main");
			Singleton<Service>.Instance.levelManager.DestroyPlayer();
			Singleton<Service>.Instance.levelManager.ClearEvents();
			PoolObject.DespawnAll();
			PoolObject.Clear();
			GameData.Initialize();
			GameResourceLoader.instance.PreloadSavedGear();
		}

		// Token: 0x040004D3 RID: 1235
		private const string _mainSceneName = "Main";

		// Token: 0x040004D4 RID: 1236
		[SerializeField]
		private PlatformManager _platformManager;
	}
}
