using System;
using Characters.Controllers;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Ending
{
	// Token: 0x020003E4 RID: 996
	public class Panel : MonoBehaviour
	{
		// Token: 0x06001295 RID: 4757 RVA: 0x000378FC File Offset: 0x00035AFC
		private void Awake()
		{
			this._tumblbug.onClick.AddListener(delegate
			{
				Application.OpenURL("https://tumblbug.com/skul");
			});
			this._twitter.onClick.AddListener(delegate
			{
				Application.OpenURL("https://twitter.com/Skul_game");
			});
			this._openTrade.onClick.AddListener(delegate
			{
				Application.OpenURL("https://otrade.co/funding/334");
			});
			this._tumblr.onClick.AddListener(delegate
			{
				Application.OpenURL("https://skulthegame.tumblr.com/");
			});
			this._newGame.onClick.AddListener(delegate
			{
				base.gameObject.SetActive(false);
				Singleton<Service>.Instance.ResetGameScene();
			});
			this._quit.onClick.AddListener(new UnityAction(Application.Quit));
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x000379FD File Offset: 0x00035BFD
		private void OnEnable()
		{
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
			EventSystem.current.SetSelectedGameObject(this._tumblbug.gameObject);
			this._tumblbug.Select();
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00031B1A File Offset: 0x0002FD1A
		private void OnDisable()
		{
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00037A3C File Offset: 0x00035C3C
		private void Update()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			Transform transform = (currentSelectedGameObject != null) ? currentSelectedGameObject.transform : null;
			if (transform == null)
			{
				return;
			}
			RectTransform component = transform.GetComponent<RectTransform>();
			this._focus.rectTransform.position = transform.position;
			Vector2 sizeDelta = component.sizeDelta;
			sizeDelta.x /= this._focus.transform.localScale.x;
			sizeDelta.y /= this._focus.transform.localScale.y;
			sizeDelta.x -= 6f;
			sizeDelta.y -= 6f;
			this._focus.rectTransform.sizeDelta = sizeDelta;
		}

		// Token: 0x04000F91 RID: 3985
		[SerializeField]
		private Image _focus;

		// Token: 0x04000F92 RID: 3986
		[SerializeField]
		private UnityEngine.UI.Button _tumblbug;

		// Token: 0x04000F93 RID: 3987
		[SerializeField]
		private UnityEngine.UI.Button _twitter;

		// Token: 0x04000F94 RID: 3988
		[SerializeField]
		private UnityEngine.UI.Button _openTrade;

		// Token: 0x04000F95 RID: 3989
		[SerializeField]
		private UnityEngine.UI.Button _tumblr;

		// Token: 0x04000F96 RID: 3990
		[SerializeField]
		private UnityEngine.UI.Button _newGame;

		// Token: 0x04000F97 RID: 3991
		[SerializeField]
		private UnityEngine.UI.Button _quit;
	}
}
