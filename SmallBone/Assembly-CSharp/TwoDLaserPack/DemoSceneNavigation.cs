using System;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	// Token: 0x0200165B RID: 5723
	public class DemoSceneNavigation : MonoBehaviour
	{
		// Token: 0x06006D12 RID: 27922 RVA: 0x00137D77 File Offset: 0x00135F77
		private void Start()
		{
			this.buttonNextDemo.onClick.AddListener(new UnityAction(this.OnButtonNextDemoClick));
		}

		// Token: 0x06006D13 RID: 27923 RVA: 0x00137D98 File Offset: 0x00135F98
		private void OnButtonNextDemoClick()
		{
			int buildIndex = SceneManager.GetActiveScene().buildIndex;
			if (buildIndex < SceneManager.sceneCount - 1)
			{
				SceneManager.LoadScene(buildIndex + 1);
				return;
			}
			Singleton<Service>.Instance.ResetGameScene();
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x040058D7 RID: 22743
		public Button buttonNextDemo;
	}
}
