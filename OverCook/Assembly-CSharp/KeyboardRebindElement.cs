using System;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

// Token: 0x02000AD1 RID: 2769
public abstract class KeyboardRebindElement : MonoBehaviour
{
	// Token: 0x170003CA RID: 970
	// (get) Token: 0x060037F5 RID: 14325 RVA: 0x001073B2 File Offset: 0x001057B2
	public string ActionTag
	{
		get
		{
			return this.m_ActionText.m_LocalizationTag;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x060037F6 RID: 14326 RVA: 0x001073BF File Offset: 0x001057BF
	// (set) Token: 0x060037F7 RID: 14327 RVA: 0x001073C7 File Offset: 0x001057C7
	public KeyboardRebindElementSet ElementSet { get; private set; }

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x060037F8 RID: 14328 RVA: 0x001073D0 File Offset: 0x001057D0
	// (set) Token: 0x060037F9 RID: 14329 RVA: 0x001073D8 File Offset: 0x001057D8
	private protected KeyboardRebindController RebindController { protected get; private set; }

	// Token: 0x060037FA RID: 14330
	public abstract void SetBinding(Key key);

	// Token: 0x060037FB RID: 14331
	public abstract void UnsetBinding(Key key);

	// Token: 0x060037FC RID: 14332
	public abstract bool HasAnyBindings();

	// Token: 0x060037FD RID: 14333
	public abstract void RefreshBindingText();

	// Token: 0x060037FE RID: 14334 RVA: 0x001073E1 File Offset: 0x001057E1
	public void Awake()
	{
		this.ElementSet = base.GetComponentInParent<KeyboardRebindElementSet>();
		this.RebindController = base.GetComponentInParent<KeyboardRebindController>();
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x001073FB File Offset: 0x001057FB
	public void Start()
	{
		this.RefreshBindingText();
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x00107403 File Offset: 0x00105803
	public void OnStartRebind()
	{
		this.RebindController.StartRebind(this);
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x00107414 File Offset: 0x00105814
	protected string KeysToString(List<Key> keys)
	{
		string text = string.Empty;
		keys = keys.Distinct<Key>().ToList<Key>();
		for (int i = 0; i < keys.Count; i++)
		{
			if (text.Length != 0)
			{
				text += "    ";
			}
			string text2 = keys[i].ToString();
			if (text2.Length == 1)
			{
				text = text + "[" + text2.ToUpperInvariant() + "]";
			}
			else
			{
				text = text + "[" + Localization.Get("Text.ControlsMenu." + text2.ToUpperInvariant(), new LocToken[0]) + "]";
			}
		}
		return text;
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x001074D0 File Offset: 0x001058D0
	protected void SetKeyBindingsText(string keys)
	{
		if (this.m_KeyBindingsText != null)
		{
			if (!string.IsNullOrEmpty(keys))
			{
				this.m_KeyBindingsText.color = Color.white;
				this.m_KeyBindingsText.SetNonLocalizedText(keys);
			}
			else
			{
				this.m_KeyBindingsText.color = Color.red;
				this.m_KeyBindingsText.SetLocalisedTextCatchAll("Text.ControlsMenu.MissingBindings");
			}
		}
	}

	// Token: 0x04002CBC RID: 11452
	[SerializeField]
	private T17Text m_ActionText;

	// Token: 0x04002CBD RID: 11453
	[SerializeField]
	private T17Text m_KeyBindingsText;

	// Token: 0x04002CBE RID: 11454
	[SerializeField]
	protected PadSide m_Side;

	// Token: 0x04002CBF RID: 11455
	[SerializeField]
	protected bool m_AllowSecondaryKey;
}
