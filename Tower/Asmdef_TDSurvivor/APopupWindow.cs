using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200012A RID: 298
public abstract class APopupWindow : MonoBehaviour
{
	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x0001CC74 File Offset: 0x0001AE74
	// (set) Token: 0x06000795 RID: 1941 RVA: 0x0001CC7C File Offset: 0x0001AE7C
	public bool IsWindowFinished { get; private set; }

	// Token: 0x06000796 RID: 1942 RVA: 0x0001CC88 File Offset: 0x0001AE88
	public static T CreateWindow<T>(APopupWindow.ePopupWindowLayer layer, Transform parent = null, bool setFullScreen = false) where T : APopupWindow
	{
		string text = typeof(T).ToString();
		GameObject gameObject = Resources.Load<GameObject>("UI/" + text);
		if (gameObject == null)
		{
			Debug.LogError("找不到名為 " + text + " 的 prefab");
			return default(T);
		}
		if (parent == null)
		{
			if (GameObject.Find("Canvas") == null)
			{
				Debug.LogError("找不到名為 Canvas 的物件");
				return default(T);
			}
			if (layer != APopupWindow.ePopupWindowLayer.MID)
			{
				if (layer == APopupWindow.ePopupWindowLayer.TOP)
				{
					parent = Singleton<UIManager>.Instance.PopupUIAnchor_TopLevel;
				}
			}
			else
			{
				parent = Singleton<UIManager>.Instance.PopupUIAnchor_MidLevel;
			}
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, parent);
		gameObject2.name = text;
		if (setFullScreen)
		{
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.sizeDelta = Vector2.zero;
		}
		return gameObject2.GetComponent<T>();
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0001CD6E File Offset: 0x0001AF6E
	protected virtual void Start()
	{
		this.ShowWindow();
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0001CD76 File Offset: 0x0001AF76
	public void ShowWindow()
	{
		this.ShowWindowProc();
	}

	// Token: 0x06000799 RID: 1945
	protected abstract void ShowWindowProc();

	// Token: 0x0600079A RID: 1946 RVA: 0x0001CD7E File Offset: 0x0001AF7E
	public void CloseWindow()
	{
		if (this.isClosed)
		{
			return;
		}
		this.isClosed = true;
		this.CloseWindowProc();
		base.StartCoroutine(this.CR_CloseWindow());
	}

	// Token: 0x0600079B RID: 1947
	protected abstract void CloseWindowProc();

	// Token: 0x0600079C RID: 1948 RVA: 0x0001CDA3 File Offset: 0x0001AFA3
	private IEnumerator CR_CloseWindow()
	{
		yield return new WaitForSecondsRealtime(this.waitDestroyTime);
		this.IsWindowFinished = true;
		Action onWindowFinished = this.OnWindowFinished;
		if (onWindowFinished != null)
		{
			onWindowFinished();
		}
		this.DestroyWindow();
		yield break;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0001CDB2 File Offset: 0x0001AFB2
	public virtual void DestroyWindow()
	{
		if (this.isDestroyed)
		{
			return;
		}
		this.isDestroyed = true;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000626 RID: 1574
	[SerializeField]
	protected Animator animator;

	// Token: 0x04000627 RID: 1575
	[SerializeField]
	protected float waitDestroyTime = 1f;

	// Token: 0x04000628 RID: 1576
	private bool isClosed;

	// Token: 0x04000629 RID: 1577
	private bool isDestroyed;

	// Token: 0x0400062B RID: 1579
	public Action OnWindowFinished;

	// Token: 0x02000277 RID: 631
	public enum ePopupWindowLayer
	{
		// Token: 0x04000BBD RID: 3005
		MID,
		// Token: 0x04000BBE RID: 3006
		TOP
	}
}
