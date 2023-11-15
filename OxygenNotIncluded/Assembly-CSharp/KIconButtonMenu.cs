using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B2B RID: 2859
public class KIconButtonMenu : KScreen
{
	// Token: 0x0600581A RID: 22554 RVA: 0x00204AAC File Offset: 0x00202CAC
	protected override void OnActivate()
	{
		base.OnActivate();
		this.RefreshButtons();
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x00204ABA File Offset: 0x00202CBA
	public void SetButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x0600581C RID: 22556 RVA: 0x00204AD4 File Offset: 0x00202CD4
	public void RefreshButtonTooltip()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (buttonInfo.buttonGo == null || buttonInfo == null)
			{
				return;
			}
			ToolTip componentInChildren = buttonInfo.buttonGo.GetComponentInChildren<ToolTip>();
			if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = buttonInfo.GetTooltipText();
				LocText componentInChildren2 = buttonInfo.buttonGo.GetComponentInChildren<LocText>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.text = buttonInfo.text;
				}
			}
		}
	}

	// Token: 0x0600581D RID: 22557 RVA: 0x00204B78 File Offset: 0x00202D78
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
		if (this.buttons == null || this.buttons.Count == 0)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[j];
			if (buttonInfo != null)
			{
				GameObject binstance = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
				buttonInfo.buttonGo = binstance;
				this.buttonObjects[j] = binstance;
				Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
				binstance.transform.SetParent(parent, false);
				binstance.SetActive(true);
				binstance.name = buttonInfo.text + "Button";
				KButton component = binstance.GetComponent<KButton>();
				if (component != null && buttonInfo.onClick != null)
				{
					component.onClick += buttonInfo.onClick;
				}
				Image image = null;
				if (component)
				{
					image = component.fgImage;
				}
				if (image != null)
				{
					image.gameObject.SetActive(false);
					foreach (Sprite sprite in this.icons)
					{
						if (sprite != null && sprite.name == buttonInfo.iconName)
						{
							image.sprite = sprite;
							image.gameObject.SetActive(true);
							break;
						}
					}
				}
				if (buttonInfo.texture != null)
				{
					RawImage componentInChildren = binstance.GetComponentInChildren<RawImage>();
					if (componentInChildren != null)
					{
						componentInChildren.gameObject.SetActive(true);
						componentInChildren.texture = buttonInfo.texture;
					}
				}
				ToolTip componentInChildren2 = binstance.GetComponentInChildren<ToolTip>();
				if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren2 != null)
				{
					componentInChildren2.toolTip = buttonInfo.GetTooltipText();
					LocText componentInChildren3 = binstance.GetComponentInChildren<LocText>();
					if (componentInChildren3 != null)
					{
						componentInChildren3.text = buttonInfo.text;
					}
				}
				if (buttonInfo.onToolTip != null)
				{
					componentInChildren2.OnToolTip = buttonInfo.onToolTip;
				}
				KIconButtonMenu screen = this;
				System.Action onClick = buttonInfo.onClick;
				System.Action value = delegate()
				{
					onClick.Signal();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
					if (binstance != null)
					{
						KToggle component3 = binstance.GetComponent<KToggle>();
						if (component3 != null)
						{
							this.SelectToggle(component3);
						}
					}
				};
				KToggle componentInChildren4 = binstance.GetComponentInChildren<KToggle>();
				if (componentInChildren4 != null)
				{
					ToggleGroup component2 = base.GetComponent<ToggleGroup>();
					if (component2 == null)
					{
						component2 = this.externalToggleGroup;
					}
					componentInChildren4.group = component2;
					componentInChildren4.onClick += value;
					Navigation navigation = componentInChildren4.navigation;
					navigation.mode = (this.automaticNavigation ? Navigation.Mode.Automatic : Navigation.Mode.None);
					componentInChildren4.navigation = navigation;
				}
				else
				{
					KBasicToggle componentInChildren5 = binstance.GetComponentInChildren<KBasicToggle>();
					if (componentInChildren5 != null)
					{
						componentInChildren5.onClick += value;
					}
				}
				if (component != null)
				{
					component.isInteractable = buttonInfo.isInteractable;
				}
				buttonInfo.onCreate.Signal(buttonInfo);
			}
		}
		this.Update();
	}

	// Token: 0x0600581E RID: 22558 RVA: 0x00204EE8 File Offset: 0x002030E8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		if (!base.gameObject.activeSelf || !base.enabled)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600581F RID: 22559 RVA: 0x00204F77 File Offset: 0x00203177
	protected override void OnPrefabInit()
	{
		base.Subscribe<KIconButtonMenu>(315865555, KIconButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x06005820 RID: 22560 RVA: 0x00204F8A File Offset: 0x0020318A
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x06005821 RID: 22561 RVA: 0x00204FA0 File Offset: 0x002031A0
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

	// Token: 0x06005822 RID: 22562 RVA: 0x0020505C File Offset: 0x0020325C
	protected void SelectToggle(KToggle selectedToggle)
	{
		if (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.enabled)
		{
			return;
		}
		if (this.currentlySelectedToggle == selectedToggle)
		{
			this.currentlySelectedToggle = null;
		}
		else
		{
			this.currentlySelectedToggle = selectedToggle;
		}
		GameObject[] array = this.buttonObjects;
		for (int i = 0; i < array.Length; i++)
		{
			KToggle component = array[i].GetComponent<KToggle>();
			if (component != null)
			{
				if (component == this.currentlySelectedToggle)
				{
					component.Select();
					component.isOn = true;
				}
				else
				{
					component.Deselect();
					component.isOn = false;
				}
			}
		}
	}

	// Token: 0x06005823 RID: 22563 RVA: 0x002050F4 File Offset: 0x002032F4
	public void ClearSelection()
	{
		foreach (GameObject gameObject in this.buttonObjects)
		{
			KToggle component = gameObject.GetComponent<KToggle>();
			if (component != null)
			{
				component.Deselect();
				component.isOn = false;
			}
			else
			{
				KBasicToggle component2 = gameObject.GetComponent<KBasicToggle>();
				if (component2 != null)
				{
					component2.isOn = false;
				}
			}
			ImageToggleState component3 = gameObject.GetComponent<ImageToggleState>();
			if (component3.GetIsActive())
			{
				component3.SetInactive();
			}
		}
		ToggleGroup component4 = base.GetComponent<ToggleGroup>();
		if (component4 != null)
		{
			component4.SetAllTogglesOff(true);
		}
		this.SelectToggle(null);
	}

	// Token: 0x04003B94 RID: 15252
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x04003B95 RID: 15253
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x04003B96 RID: 15254
	[SerializeField]
	protected bool automaticNavigation = true;

	// Token: 0x04003B97 RID: 15255
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x04003B98 RID: 15256
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04003B99 RID: 15257
	[SerializeField]
	protected Sprite[] icons;

	// Token: 0x04003B9A RID: 15258
	[SerializeField]
	private ToggleGroup externalToggleGroup;

	// Token: 0x04003B9B RID: 15259
	protected KToggle currentlySelectedToggle;

	// Token: 0x04003B9C RID: 15260
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x04003B9D RID: 15261
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04003B9E RID: 15262
	private UnityAction inputChangeReceiver;

	// Token: 0x04003B9F RID: 15263
	protected GameObject go;

	// Token: 0x04003BA0 RID: 15264
	protected IList<KIconButtonMenu.ButtonInfo> buttons;

	// Token: 0x04003BA1 RID: 15265
	private static readonly global::EventSystem.IntraObjectHandler<KIconButtonMenu> OnSetActivatorDelegate = new global::EventSystem.IntraObjectHandler<KIconButtonMenu>(delegate(KIconButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001A37 RID: 6711
	public class ButtonInfo
	{
		// Token: 0x06009665 RID: 38501 RVA: 0x0033C778 File Offset: 0x0033A978
		public ButtonInfo(string iconName = "", string text = "", System.Action on_click = null, global::Action shortcutKey = global::Action.NumActions, Action<GameObject> on_refresh = null, Action<KIconButtonMenu.ButtonInfo> on_create = null, Texture texture = null, string tooltipText = "", bool is_interactable = true)
		{
			this.iconName = iconName;
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = on_click;
			this.onCreate = on_create;
			this.texture = texture;
			this.tooltipText = tooltipText;
			this.isInteractable = is_interactable;
		}

		// Token: 0x06009666 RID: 38502 RVA: 0x0033C7C8 File Offset: 0x0033A9C8
		public string GetTooltipText()
		{
			string text = (this.tooltipText == "") ? this.text : this.tooltipText;
			if (this.shortcutKey != global::Action.NumActions)
			{
				text = GameUtil.ReplaceHotkeyString(text, this.shortcutKey);
			}
			return text;
		}

		// Token: 0x040078DF RID: 30943
		public string iconName;

		// Token: 0x040078E0 RID: 30944
		public string text;

		// Token: 0x040078E1 RID: 30945
		public string tooltipText;

		// Token: 0x040078E2 RID: 30946
		public string[] multiText;

		// Token: 0x040078E3 RID: 30947
		public global::Action shortcutKey;

		// Token: 0x040078E4 RID: 30948
		public bool isInteractable;

		// Token: 0x040078E5 RID: 30949
		public Action<KIconButtonMenu.ButtonInfo> onCreate;

		// Token: 0x040078E6 RID: 30950
		public System.Action onClick;

		// Token: 0x040078E7 RID: 30951
		public Func<string> onToolTip;

		// Token: 0x040078E8 RID: 30952
		public GameObject buttonGo;

		// Token: 0x040078E9 RID: 30953
		public object userData;

		// Token: 0x040078EA RID: 30954
		public Texture texture;

		// Token: 0x02002231 RID: 8753
		// (Invoke) Token: 0x0600AD0B RID: 44299
		public delegate void Callback();
	}
}
