using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x0200007F RID: 127
	[AddComponentMenu("Modular Options/Button/Load Scene")]
	[RequireComponent(typeof(Button))]
	public class LoadSceneButton : MonoBehaviour
	{
		// Token: 0x060001DB RID: 475 RVA: 0x000083FF File Offset: 0x000065FF
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(delegate()
			{
				SceneManager.LoadScene(this.scene);
			});
		}

		// Token: 0x040001B4 RID: 436
		[SceneRef]
		public string scene;
	}
}
