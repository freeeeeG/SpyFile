using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Helios.GUI
{
	// Token: 0x020000DC RID: 220
	public class GameManager : SingletonPersistent<GameManager>
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0000E5B3 File Offset: 0x0000C7B3
		private void Start()
		{
			this._stScenes.Push(this._goRoot);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000E5C8 File Offset: 0x0000C7C8
		public void LoadGameObject(GameObject go)
		{
			if (SceneManager.GetActiveScene().name.Equals("DemoScene"))
			{
				return;
			}
			if (this._stScenes.Count > 0)
			{
				GameObject gameObject = this._stScenes.Pop();
				gameObject.SetActive(false);
				this._stScenes.Push(gameObject);
			}
			this.LoadPopup(go);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000E624 File Offset: 0x0000C824
		public void LoadPopup(GameObject go)
		{
			if (SceneManager.GetActiveScene().name.Equals("DemoScene"))
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(go);
			gameObject.transform.SetParent(this._tfParent);
			gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(true);
			this._stScenes.Push(gameObject);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
		public void Back()
		{
			if (SceneManager.GetActiveScene().name.Equals("DemoScene"))
			{
				return;
			}
			if (this._stScenes.Count == 0)
			{
				return;
			}
			GameObject obj = this._stScenes.Pop();
			this.TweenFading(obj);
			if (this._stScenes.Count == 0)
			{
				return;
			}
			GameObject gameObject = this._stScenes.Pop();
			gameObject.SetActive(true);
			this._stScenes.Push(gameObject);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000E720 File Offset: 0x0000C920
		private void TweenFading(GameObject obj)
		{
			GameManager.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.obj = obj;
			CanvasGroup canvasGroup;
			if (CS$<>8__locals1.obj.TryGetComponent<CanvasGroup>(out canvasGroup))
			{
				canvasGroup.blocksRaycasts = false;
				canvasGroup.alpha = 0f;
				GameManager.<TweenFading>g__HIdeAndDestroy|7_0(ref CS$<>8__locals1);
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000E75D File Offset: 0x0000C95D
		public void BackHome()
		{
			while (this._stScenes.Count > 1)
			{
				this.Back();
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E788 File Offset: 0x0000C988
		[CompilerGenerated]
		internal static void <TweenFading>g__HIdeAndDestroy|7_0(ref GameManager.<>c__DisplayClass7_0 A_0)
		{
			A_0.obj.SetActive(false);
			Object.Destroy(A_0.obj, 3f);
		}

		// Token: 0x0400030F RID: 783
		[SerializeField]
		private GameObject _goRoot;

		// Token: 0x04000310 RID: 784
		[SerializeField]
		private Transform _tfParent;

		// Token: 0x04000311 RID: 785
		private Stack<GameObject> _stScenes = new Stack<GameObject>();
	}
}
