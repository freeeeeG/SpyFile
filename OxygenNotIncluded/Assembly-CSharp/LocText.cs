using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000B4F RID: 2895
public class LocText : TextMeshProUGUI
{
	// Token: 0x06005957 RID: 22871 RVA: 0x0020AE54 File Offset: 0x00209054
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06005958 RID: 22872 RVA: 0x0020AE5C File Offset: 0x0020905C
	// (set) Token: 0x06005959 RID: 22873 RVA: 0x0020AE64 File Offset: 0x00209064
	public bool AllowLinks
	{
		get
		{
			return this.allowLinksInternal;
		}
		set
		{
			this.allowLinksInternal = value;
			this.RefreshLinkHandler();
			this.raycastTarget = (this.raycastTarget || this.allowLinksInternal);
		}
	}

	// Token: 0x0600595A RID: 22874 RVA: 0x0020AE8C File Offset: 0x0020908C
	[ContextMenu("Apply Settings")]
	public void ApplySettings()
	{
		if (this.key != "" && Application.isPlaying)
		{
			StringKey key = new StringKey(this.key);
			this.text = Strings.Get(key);
		}
		if (this.textStyleSetting != null)
		{
			SetTextStyleSetting.ApplyStyle(this, this.textStyleSetting);
		}
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x0020AEEC File Offset: 0x002090EC
	private new void Awake()
	{
		base.Awake();
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = Localization.IsRightToLeft;
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshText));
		SetTextStyleSetting setTextStyleSetting = base.gameObject.GetComponent<SetTextStyleSetting>();
		if (setTextStyleSetting == null)
		{
			setTextStyleSetting = base.gameObject.AddComponent<SetTextStyleSetting>();
		}
		if (!this.allowOverride)
		{
			setTextStyleSetting.SetStyle(this.textStyleSetting);
		}
		this.textLinkHandler = base.GetComponent<TextLinkHandler>();
	}

	// Token: 0x0600595C RID: 22876 RVA: 0x0020AFA9 File Offset: 0x002091A9
	private new void Start()
	{
		base.Start();
		this.RefreshLinkHandler();
	}

	// Token: 0x0600595D RID: 22877 RVA: 0x0020AFB7 File Offset: 0x002091B7
	private new void OnDestroy()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshText));
		base.OnDestroy();
	}

	// Token: 0x0600595E RID: 22878 RVA: 0x0020AFD5 File Offset: 0x002091D5
	public override void SetLayoutDirty()
	{
		if (this.staticLayout)
		{
			return;
		}
		base.SetLayoutDirty();
	}

	// Token: 0x0600595F RID: 22879 RVA: 0x0020AFE6 File Offset: 0x002091E6
	public void SetLinkOverrideAction(Func<string, bool> action)
	{
		this.RefreshLinkHandler();
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.overrideLinkAction = action;
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06005960 RID: 22880 RVA: 0x0020B008 File Offset: 0x00209208
	// (set) Token: 0x06005961 RID: 22881 RVA: 0x0020B010 File Offset: 0x00209210
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			base.text = this.FilterInput(value);
		}
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x0020B01F File Offset: 0x0020921F
	public override void SetText(string text)
	{
		text = this.FilterInput(text);
		base.SetText(text);
	}

	// Token: 0x06005963 RID: 22883 RVA: 0x0020B031 File Offset: 0x00209231
	private string FilterInput(string input)
	{
		if (input != null)
		{
			string text = LocText.ParseText(input);
			if (text != input)
			{
				this.originalString = input;
			}
			else
			{
				this.originalString = string.Empty;
			}
			input = text;
		}
		if (this.AllowLinks)
		{
			return LocText.ModifyLinkStrings(input);
		}
		return input;
	}

	// Token: 0x06005964 RID: 22884 RVA: 0x0020B06C File Offset: 0x0020926C
	public static string ParseText(string input)
	{
		string pattern = "\\{Hotkey/(\\w+)\\}";
		string input2 = Regex.Replace(input, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			global::Action action;
			if (LocText.ActionLookup.TryGetValue(value, out action))
			{
				return GameUtil.GetHotkeyString(action);
			}
			return m.Value;
		});
		pattern = "\\(ClickType/(\\w+)\\)";
		return Regex.Replace(input2, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			Pair<LocString, LocString> pair;
			if (!LocText.ClickLookup.TryGetValue(value, out pair))
			{
				return m.Value;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return pair.first.ToString();
			}
			return pair.second.ToString();
		});
	}

	// Token: 0x06005965 RID: 22885 RVA: 0x0020B0D0 File Offset: 0x002092D0
	private void RefreshText()
	{
		if (this.originalString != string.Empty)
		{
			this.SetText(this.originalString);
		}
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x0020B0F0 File Offset: 0x002092F0
	protected override void GenerateTextMesh()
	{
		base.GenerateTextMesh();
	}

	// Token: 0x06005967 RID: 22887 RVA: 0x0020B0F8 File Offset: 0x002092F8
	internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
	{
		base.font = font;
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = isRightToLeft;
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x0020B154 File Offset: 0x00209354
	private static string ModifyLinkStrings(string input)
	{
		if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
		{
			return input;
		}
		StringBuilder stringBuilder = new StringBuilder(input);
		stringBuilder.Replace("<link=\"", LocText.combinedPrefix);
		stringBuilder.Replace("</link>", LocText.combinedSuffix);
		return stringBuilder.ToString();
	}

	// Token: 0x06005969 RID: 22889 RVA: 0x0020B1A4 File Offset: 0x002093A4
	private void RefreshLinkHandler()
	{
		if (this.textLinkHandler == null && this.allowLinksInternal)
		{
			this.textLinkHandler = base.GetComponent<TextLinkHandler>();
			if (this.textLinkHandler == null)
			{
				this.textLinkHandler = base.gameObject.AddComponent<TextLinkHandler>();
			}
		}
		else if (!this.allowLinksInternal && this.textLinkHandler != null)
		{
			UnityEngine.Object.Destroy(this.textLinkHandler);
			this.textLinkHandler = null;
		}
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.CheckMouseOver();
		}
	}

	// Token: 0x04003C64 RID: 15460
	public string key;

	// Token: 0x04003C65 RID: 15461
	public TextStyleSetting textStyleSetting;

	// Token: 0x04003C66 RID: 15462
	public bool allowOverride;

	// Token: 0x04003C67 RID: 15463
	public bool staticLayout;

	// Token: 0x04003C68 RID: 15464
	private TextLinkHandler textLinkHandler;

	// Token: 0x04003C69 RID: 15465
	private string originalString = string.Empty;

	// Token: 0x04003C6A RID: 15466
	[SerializeField]
	private bool allowLinksInternal;

	// Token: 0x04003C6B RID: 15467
	private static readonly Dictionary<string, global::Action> ActionLookup = Enum.GetNames(typeof(global::Action)).ToDictionary((string x) => x, (string x) => (global::Action)Enum.Parse(typeof(global::Action), x), StringComparer.OrdinalIgnoreCase);

	// Token: 0x04003C6C RID: 15468
	private static readonly Dictionary<string, Pair<LocString, LocString>> ClickLookup = new Dictionary<string, Pair<LocString, LocString>>
	{
		{
			UI.ClickType.Click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESS, UI.CONTROLS.CLICK)
		},
		{
			UI.ClickType.Clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLE, UI.CONTROLS.CLICKABLE)
		},
		{
			UI.ClickType.Clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSED, UI.CONTROLS.CLICKED)
		},
		{
			UI.ClickType.Clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSING, UI.CONTROLS.CLICKING)
		},
		{
			UI.ClickType.Clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSES, UI.CONTROLS.CLICKS)
		},
		{
			UI.ClickType.click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSLOWER, UI.CONTROLS.CLICKLOWER)
		},
		{
			UI.ClickType.clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLELOWER, UI.CONTROLS.CLICKABLELOWER)
		},
		{
			UI.ClickType.clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDLOWER, UI.CONTROLS.CLICKEDLOWER)
		},
		{
			UI.ClickType.clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGLOWER, UI.CONTROLS.CLICKINGLOWER)
		},
		{
			UI.ClickType.clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESLOWER, UI.CONTROLS.CLICKSLOWER)
		},
		{
			UI.ClickType.CLICK.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSUPPER, UI.CONTROLS.CLICKUPPER)
		},
		{
			UI.ClickType.CLICKABLE.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLEUPPER, UI.CONTROLS.CLICKABLEUPPER)
		},
		{
			UI.ClickType.CLICKED.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDUPPER, UI.CONTROLS.CLICKEDUPPER)
		},
		{
			UI.ClickType.CLICKING.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGUPPER, UI.CONTROLS.CLICKINGUPPER)
		},
		{
			UI.ClickType.CLICKS.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESUPPER, UI.CONTROLS.CLICKSUPPER)
		}
	};

	// Token: 0x04003C6D RID: 15469
	private const string linkPrefix_open = "<link=\"";

	// Token: 0x04003C6E RID: 15470
	private const string linkSuffix = "</link>";

	// Token: 0x04003C6F RID: 15471
	private const string linkColorPrefix = "<b><style=\"KLink\">";

	// Token: 0x04003C70 RID: 15472
	private const string linkColorSuffix = "</style></b>";

	// Token: 0x04003C71 RID: 15473
	private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";

	// Token: 0x04003C72 RID: 15474
	private static readonly string combinedSuffix = "</style></b></link>";
}
