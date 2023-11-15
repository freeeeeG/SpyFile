using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200022D RID: 557
	[RequireComponent(typeof(CanvasGroup))]
	public class Panel : MonoBehaviour
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0002D5CB File Offset: 0x0002B7CB
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0002D5D8 File Offset: 0x0002B7D8
		public bool interactable
		{
			get
			{
				return this.canvasGroup.interactable;
			}
			set
			{
				this.canvasGroup.interactable = value;
				this.canvasGroup.blocksRaycasts = value;
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0002D5F2 File Offset: 0x0002B7F2
		protected virtual void Start()
		{
			this.canvasGroup = base.GetComponent<CanvasGroup>();
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			base.StartCoroutine(this.DelayStartCR());
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0002D628 File Offset: 0x0002B828
		public virtual void Show()
		{
			this.canvasGroup.interactable = true;
			this.canvasGroup.blocksRaycasts = true;
			UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Show();
			}
			if (Gamepad.current != null)
			{
				Selectable selectable = this.gamepadDefaultSelectable;
				if (selectable == null)
				{
					return;
				}
				selectable.Select();
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0002D684 File Offset: 0x0002B884
		public virtual void Hide()
		{
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Hide();
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0002D6C6 File Offset: 0x0002B8C6
		public void SelectDefault()
		{
			Selectable selectable = this.gamepadDefaultSelectable;
			if (selectable == null)
			{
				return;
			}
			selectable.Select();
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0002D6D8 File Offset: 0x0002B8D8
		private IEnumerator DelayStartCR()
		{
			yield return null;
			UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetOff();
			}
			yield break;
		}

		// Token: 0x0400089C RID: 2204
		[SerializeField]
		private Selectable gamepadDefaultSelectable;

		// Token: 0x0400089D RID: 2205
		private CanvasGroup canvasGroup;
	}
}
