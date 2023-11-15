using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B2C RID: 2860
public class KIconToggleMenu : KScreen
{
	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06005826 RID: 22566 RVA: 0x002051B8 File Offset: 0x002033B8
	// (remove) Token: 0x06005827 RID: 22567 RVA: 0x002051F0 File Offset: 0x002033F0
	public event KIconToggleMenu.OnSelect onSelect;

	// Token: 0x06005828 RID: 22568 RVA: 0x00205225 File Offset: 0x00203425
	public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x00205234 File Offset: 0x00203434
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x0600582A RID: 22570 RVA: 0x0020523C File Offset: 0x0020343C
	protected virtual void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				if (!this.dontDestroyToggles.Contains(ktoggle))
				{
					UnityEngine.Object.Destroy(ktoggle.gameObject);
				}
				else
				{
					ktoggle.ClearOnClick();
				}
			}
		}
		this.toggles.Clear();
		this.dontDestroyToggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform transform = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			KToggle ktoggle2;
			if (toggleInfo.instanceOverride != null)
			{
				ktoggle2 = toggleInfo.instanceOverride;
				this.dontDestroyToggles.Add(ktoggle2);
			}
			else if (toggleInfo.prefabOverride)
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform.gameObject, true);
			}
			else
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform.gameObject, true);
			}
			ktoggle2.Deselect();
			ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
			ktoggle2.group = this.group;
			ktoggle2.onClick += delegate()
			{
				this.OnClick(idx);
			};
			LocText componentInChildren = ktoggle2.transform.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.SetText(toggleInfo.text);
			}
			if (toggleInfo.getSpriteCB != null)
			{
				ktoggle2.fgImage.sprite = toggleInfo.getSpriteCB();
			}
			else if (toggleInfo.icon != null)
			{
				ktoggle2.fgImage.sprite = Assets.GetSprite(toggleInfo.icon);
			}
			toggleInfo.SetToggle(ktoggle2);
			this.toggles.Add(ktoggle2);
		}
	}

	// Token: 0x0600582B RID: 22571 RVA: 0x00205464 File Offset: 0x00203664
	public Sprite GetIcon(string name)
	{
		foreach (Sprite sprite in this.icons)
		{
			if (sprite.name == name)
			{
				return sprite;
			}
		}
		return null;
	}

	// Token: 0x0600582C RID: 22572 RVA: 0x0020549C File Offset: 0x0020369C
	public virtual void ClearSelection()
	{
		if (this.toggles == null)
		{
			return;
		}
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.Deselect();
			ktoggle.ClearAnimState();
		}
		this.selected = -1;
	}

	// Token: 0x0600582D RID: 22573 RVA: 0x00205504 File Offset: 0x00203704
	private void OnClick(int i)
	{
		if (this.onSelect == null)
		{
			return;
		}
		this.selected = i;
		this.onSelect(this.toggleInfo[i]);
		if (!this.toggles[i].isOn)
		{
			this.selected = -1;
		}
		for (int j = 0; j < this.toggles.Count; j++)
		{
			if (j != this.selected)
			{
				this.toggles[j].isOn = false;
			}
		}
	}

	// Token: 0x0600582E RID: 22574 RVA: 0x00205584 File Offset: 0x00203784
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		if (this.toggleInfo == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			if (this.toggles[i].isActiveAndEnabled)
			{
				global::Action hotKey = this.toggleInfo[i].hotKey;
				if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
				{
					if (this.selected != i || this.repeatKeyDownToggles)
					{
						this.toggles[i].Click();
						if (this.selected == i)
						{
							this.toggles[i].Deselect();
						}
						this.selected = i;
						return;
					}
					break;
				}
			}
		}
	}

	// Token: 0x0600582F RID: 22575 RVA: 0x00205636 File Offset: 0x00203836
	public virtual void Close()
	{
		this.ClearSelection();
		this.Show(false);
	}

	// Token: 0x04003BA2 RID: 15266
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04003BA3 RID: 15267
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04003BA4 RID: 15268
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04003BA5 RID: 15269
	[SerializeField]
	private Sprite[] icons;

	// Token: 0x04003BA6 RID: 15270
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04003BA7 RID: 15271
	[SerializeField]
	public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;

	// Token: 0x04003BA8 RID: 15272
	[SerializeField]
	protected bool repeatKeyDownToggles = true;

	// Token: 0x04003BA9 RID: 15273
	protected KToggle currentlySelectedToggle;

	// Token: 0x04003BAB RID: 15275
	protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04003BAC RID: 15276
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04003BAD RID: 15277
	private List<KToggle> dontDestroyToggles = new List<KToggle>();

	// Token: 0x04003BAE RID: 15278
	protected int selected = -1;

	// Token: 0x02001A3A RID: 6714
	// (Invoke) Token: 0x0600966D RID: 38509
	public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001A3B RID: 6715
	public class ToggleInfo
	{
		// Token: 0x06009670 RID: 38512 RVA: 0x0033C8A8 File Offset: 0x0033AAA8
		public ToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "")
		{
			this.text = text;
			this.userData = user_data;
			this.icon = icon;
			this.hotKey = hotkey;
			this.tooltip = tooltip;
			this.tooltipHeader = tooltip_header;
			this.getTooltipText = new ToolTip.ComplexTooltipDelegate(this.DefaultGetTooltipText);
		}

		// Token: 0x06009671 RID: 38513 RVA: 0x0033C8FB File Offset: 0x0033AAFB
		public ToggleInfo(string text, object user_data, global::Action hotkey, Func<Sprite> get_sprite_cb)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotkey;
			this.getSpriteCB = get_sprite_cb;
		}

		// Token: 0x06009672 RID: 38514 RVA: 0x0033C920 File Offset: 0x0033AB20
		public virtual void SetToggle(KToggle toggle)
		{
			this.toggle = toggle;
			toggle.GetComponent<ToolTip>().OnComplexToolTip = this.getTooltipText;
		}

		// Token: 0x06009673 RID: 38515 RVA: 0x0033C93C File Offset: 0x0033AB3C
		protected virtual List<global::Tuple<string, TextStyleSetting>> DefaultGetTooltipText()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (this.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		}

		// Token: 0x040078F0 RID: 30960
		public string text;

		// Token: 0x040078F1 RID: 30961
		public object userData;

		// Token: 0x040078F2 RID: 30962
		public string icon;

		// Token: 0x040078F3 RID: 30963
		public string tooltip;

		// Token: 0x040078F4 RID: 30964
		public string tooltipHeader;

		// Token: 0x040078F5 RID: 30965
		public KToggle toggle;

		// Token: 0x040078F6 RID: 30966
		public global::Action hotKey;

		// Token: 0x040078F7 RID: 30967
		public ToolTip.ComplexTooltipDelegate getTooltipText;

		// Token: 0x040078F8 RID: 30968
		public Func<Sprite> getSpriteCB;

		// Token: 0x040078F9 RID: 30969
		public KToggle prefabOverride;

		// Token: 0x040078FA RID: 30970
		public KToggle instanceOverride;
	}
}
