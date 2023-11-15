using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B29 RID: 2857
public class KButtonMenu : KScreen
{
	// Token: 0x06005807 RID: 22535 RVA: 0x00204351 File Offset: 0x00202551
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = this.ShouldConsumeMouseScroll;
		this.RefreshButtons();
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x00204365 File Offset: 0x00202565
	public void SetButtons(IList<KButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x06005809 RID: 22537 RVA: 0x0020437C File Offset: 0x0020257C
	public virtual void RefreshButtons()
	{
		if (this.buttonObjects != null)
		{
			for (int i = 0; i < this.buttonObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(this.buttonObjects[i]);
			}
			this.buttonObjects = null;
		}
		if (this.buttons == null)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KButtonMenu.ButtonInfo binfo = this.buttons[j];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
			this.buttonObjects[j] = gameObject;
			Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = binfo.text + "Button";
			LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
			if (componentsInChildren != null)
			{
				foreach (LocText locText in componentsInChildren)
				{
					locText.text = ((locText.name == "Hotkey") ? GameUtil.GetActionString(binfo.shortcutKey) : binfo.text);
					locText.color = (binfo.isEnabled ? new Color(1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f));
				}
			}
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (binfo.toolTip != null && binfo.toolTip != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = binfo.toolTip;
			}
			KButtonMenu screen = this;
			KButton button = gameObject.GetComponent<KButton>();
			button.isInteractable = binfo.isEnabled;
			if (binfo.popupOptions == null && binfo.onPopulatePopup == null)
			{
				UnityAction onClick = binfo.onClick;
				System.Action value = delegate()
				{
					onClick();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
				};
				button.onClick += value;
			}
			else
			{
				button.onClick += delegate()
				{
					this.SetupPopupMenu(binfo, button);
				};
			}
			binfo.uibutton = button;
			KButtonMenu.ButtonInfo.HoverCallback onHover = binfo.onHover;
		}
		this.Update();
	}

	// Token: 0x0600580A RID: 22538 RVA: 0x00204630 File Offset: 0x00202830
	protected Button.ButtonClickedEvent SetupPopupMenu(KButtonMenu.ButtonInfo binfo, KButton button)
	{
		Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
		UnityAction unityAction = delegate()
		{
			List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
			if (binfo.onPopulatePopup != null)
			{
				binfo.popupOptions = binfo.onPopulatePopup();
			}
			string[] popupOptions = binfo.popupOptions;
			for (int i = 0; i < popupOptions.Length; i++)
			{
				string delegate_str2 = popupOptions[i];
				string delegate_str = delegate_str2;
				list.Add(new KButtonMenu.ButtonInfo(delegate_str, delegate()
				{
					binfo.onPopupClick(delegate_str);
					if (!this.keepMenuOpen)
					{
						this.Deactivate();
					}
				}, global::Action.NumActions, null, null, null, true, null, null, null));
			}
			KButtonMenu component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject, null, null).GetComponent<KButtonMenu>();
			component.SetButtons(list.ToArray());
			RootMenu.Instance.AddSubMenu(component);
			Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			Vector3 b = default(Vector3);
			if (Util.IsOnLeftSideOfScreen(button.transform.GetPosition()))
			{
				b.x = button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			else
			{
				b.x = -button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			component.transform.SetPosition(button.transform.GetPosition() + b);
		};
		binfo.onClick = unityAction;
		buttonClickedEvent.AddListener(unityAction);
		return buttonClickedEvent;
	}

	// Token: 0x0600580B RID: 22539 RVA: 0x00204680 File Offset: 0x00202880
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x002046F9 File Offset: 0x002028F9
	protected override void OnPrefabInit()
	{
		base.Subscribe<KButtonMenu>(315865555, KButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x0600580D RID: 22541 RVA: 0x0020470C File Offset: 0x0020290C
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x0600580E RID: 22542 RVA: 0x00204720 File Offset: 0x00202920
	protected override void OnDeactivate()
	{
	}

	// Token: 0x0600580F RID: 22543 RVA: 0x00204724 File Offset: 0x00202924
	private void Update()
	{
		if (!this.followGameObject || this.go == null || base.canvas == null)
		{
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(this.go.transform.GetPosition());
		RectTransform component = base.GetComponent<RectTransform>();
		RectTransform component2 = base.canvas.GetComponent<RectTransform>();
		if (component != null)
		{
			component.anchoredPosition = new Vector2(vector.x * component2.sizeDelta.x - component2.sizeDelta.x * 0.5f, vector.y * component2.sizeDelta.y - component2.sizeDelta.y * 0.5f);
		}
	}

	// Token: 0x04003B87 RID: 15239
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x04003B88 RID: 15240
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x04003B89 RID: 15241
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x04003B8A RID: 15242
	public GameObject buttonPrefab;

	// Token: 0x04003B8B RID: 15243
	public bool ShouldConsumeMouseScroll;

	// Token: 0x04003B8C RID: 15244
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x04003B8D RID: 15245
	protected GameObject go;

	// Token: 0x04003B8E RID: 15246
	protected IList<KButtonMenu.ButtonInfo> buttons;

	// Token: 0x04003B8F RID: 15247
	private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KButtonMenu>(delegate(KButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001A30 RID: 6704
	public class ButtonInfo
	{
		// Token: 0x06009656 RID: 38486 RVA: 0x0033C3F0 File Offset: 0x0033A5F0
		public ButtonInfo(string text = null, UnityAction on_click = null, global::Action shortcut_key = global::Action.NumActions, KButtonMenu.ButtonInfo.HoverCallback on_hover = null, string tool_tip = null, GameObject visualizer = null, bool is_enabled = true, string[] popup_options = null, Action<string> on_popup_click = null, Func<string[]> on_populate_popup = null)
		{
			this.text = text;
			this.shortcutKey = shortcut_key;
			this.onClick = on_click;
			this.onHover = on_hover;
			this.visualizer = visualizer;
			this.toolTip = tool_tip;
			this.isEnabled = is_enabled;
			this.uibutton = null;
			this.popupOptions = popup_options;
			this.onPopupClick = on_popup_click;
			this.onPopulatePopup = on_populate_popup;
		}

		// Token: 0x06009657 RID: 38487 RVA: 0x0033C460 File Offset: 0x0033A660
		public ButtonInfo(string text, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.userData = userData;
			this.visualizer = null;
			this.uibutton = null;
		}

		// Token: 0x06009658 RID: 38488 RVA: 0x0033C4B0 File Offset: 0x0033A6B0
		public ButtonInfo(string text, GameObject visualizer, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.visualizer = visualizer;
			this.userData = userData;
			this.uibutton = null;
		}

		// Token: 0x040078C4 RID: 30916
		public string text;

		// Token: 0x040078C5 RID: 30917
		public global::Action shortcutKey;

		// Token: 0x040078C6 RID: 30918
		public GameObject visualizer;

		// Token: 0x040078C7 RID: 30919
		public UnityAction onClick;

		// Token: 0x040078C8 RID: 30920
		public KButtonMenu.ButtonInfo.HoverCallback onHover;

		// Token: 0x040078C9 RID: 30921
		public FMODAsset clickSound;

		// Token: 0x040078CA RID: 30922
		public KButton uibutton;

		// Token: 0x040078CB RID: 30923
		public string toolTip;

		// Token: 0x040078CC RID: 30924
		public bool isEnabled = true;

		// Token: 0x040078CD RID: 30925
		public string[] popupOptions;

		// Token: 0x040078CE RID: 30926
		public Action<string> onPopupClick;

		// Token: 0x040078CF RID: 30927
		public Func<string[]> onPopulatePopup;

		// Token: 0x040078D0 RID: 30928
		public object userData;

		// Token: 0x0200222F RID: 8751
		// (Invoke) Token: 0x0600AD03 RID: 44291
		public delegate void HoverCallback(GameObject hoverTarget);

		// Token: 0x02002230 RID: 8752
		// (Invoke) Token: 0x0600AD07 RID: 44295
		public delegate void Callback();
	}
}
