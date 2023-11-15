using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x02000399 RID: 921
	public abstract class Dialogue : MonoBehaviour
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00031D92 File Offset: 0x0002FF92
		public static Dialogue GetCurrent()
		{
			if (Dialogue.opened.Count <= 0)
			{
				return null;
			}
			return Dialogue.opened[Dialogue.opened.Count - 1];
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x00031DB9 File Offset: 0x0002FFB9
		public static bool anyDialogueOpened
		{
			get
			{
				return Dialogue.opened.Count > 0;
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00031DC8 File Offset: 0x0002FFC8
		private static bool Focused(Dialogue dialogue)
		{
			return Dialogue.opened.Count != 0 && Dialogue.opened[Dialogue.opened.Count - 1] == dialogue;
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060010DE RID: 4318
		public abstract bool closeWithPauseKey { get; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00031DF4 File Offset: 0x0002FFF4
		public bool focused
		{
			get
			{
				return Dialogue.Focused(this);
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00031DFC File Offset: 0x0002FFFC
		public void Toggle()
		{
			if (base.gameObject.activeSelf)
			{
				this.Close();
				return;
			}
			this.Open();
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00031E18 File Offset: 0x00030018
		public void Open()
		{
			if (base.gameObject.activeSelf)
			{
				return;
			}
			Dialogue.opened.Add(this);
			base.gameObject.SetActive(true);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00031E3F File Offset: 0x0003003F
		protected virtual void OnEnable()
		{
			this.Focus();
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00031E47 File Offset: 0x00030047
		public void Close()
		{
			if (Dialogue.opened.Count >= 2 && this.focused)
			{
				Dialogue.opened[Dialogue.opened.Count - 2].Focus();
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00031E85 File Offset: 0x00030085
		protected virtual void OnDisable()
		{
			InputManager.ClearInputState();
			Dialogue.opened.Remove(this);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00031E98 File Offset: 0x00030098
		public void Focus()
		{
			if (this._defaultFocus == null)
			{
				return;
			}
			this.Focus(this._defaultFocus);
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00031EB5 File Offset: 0x000300B5
		public void Focus(Selectable focus)
		{
			base.StartCoroutine(this.CFocus(focus));
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00031EC5 File Offset: 0x000300C5
		private IEnumerator CFocus(Selectable focus)
		{
			EventSystem.current.SetSelectedGameObject(null);
			yield return null;
			EventSystem.current.SetSelectedGameObject(focus.gameObject);
			focus.Select();
			typeof(Selectable).GetMethod("DoStateTransition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(focus, new object[]
			{
				3,
				true
			});
			yield break;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00031ED4 File Offset: 0x000300D4
		protected virtual void Update()
		{
			if (!this.focused)
			{
				return;
			}
			if (EventSystem.current == null)
			{
				return;
			}
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject == null || currentSelectedGameObject.GetComponentInParent<Dialogue>(true) != this)
			{
				if (this._lastValidFocus == null)
				{
					if (this._defaultFocus == null)
					{
						return;
					}
					this._lastValidFocus = this._defaultFocus.gameObject;
				}
				EventSystem.current.SetSelectedGameObject(this._lastValidFocus);
				return;
			}
			this._lastValidFocus = currentSelectedGameObject;
		}

		// Token: 0x04000DCC RID: 3532
		public static readonly List<Dialogue> opened = new List<Dialogue>();

		// Token: 0x04000DCD RID: 3533
		[SerializeField]
		protected Selectable _defaultFocus;

		// Token: 0x04000DCE RID: 3534
		private GameObject _lastValidFocus;
	}
}
