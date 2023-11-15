using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B7D RID: 2941
[AddComponentMenu("T17_UI/T17Text", 29)]
[Serializable]
public class T17Text : Text, IT17EventHelper
{
	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06003BFB RID: 15355 RVA: 0x0011F386 File Offset: 0x0011D786
	// (set) Token: 0x06003BFC RID: 15356 RVA: 0x0011F38E File Offset: 0x0011D78E
	public override Color color
	{
		get
		{
			return base.color;
		}
		set
		{
			base.color = value;
		}
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x0011F398 File Offset: 0x0011D798
	protected override void Start()
	{
		base.Start();
		if (Application.isPlaying)
		{
			this.m_PlayerManager = GameUtils.RequireManager<PlayerManager>();
		}
		this.m_TextProcessors = new List<ITextProcessor>(base.GetComponents<ITextProcessor>());
		if (this.material == null)
		{
			this.material = base.font.material;
		}
		if (this.m_bNeedsLocalization)
		{
			this.Convert();
		}
		else if (!this.m_bTagFound && !string.IsNullOrEmpty(this.m_NonLocalizedKeyboardText) && this.ShouldUseKeyboardText())
		{
			this.CheckMarkup(this.m_NonLocalizedKeyboardText);
			this.Apply(this.m_NonLocalizedKeyboardText);
		}
		else
		{
			this.CheckMarkup(this.text);
			this.Apply(this.text);
		}
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x0011F464 File Offset: 0x0011D864
	public void Convert()
	{
		if (string.IsNullOrEmpty(this.m_PlaceholderText))
		{
			this.m_PlaceholderText = this.text;
		}
		Localization.LoadDictionarys(false, null);
		string empty = string.Empty;
		this.m_bTagFound = false;
		if (this.ShouldUseKeyboardText())
		{
			if (this.m_Replacements == null)
			{
				this.m_bTagFound = Localization.Get(this.m_LocalizationTag + "_Keyboard", out empty, new LocToken[0]);
			}
			else
			{
				this.m_bTagFound = Localization.Get(this.m_LocalizationTag + "_Keyboard", out empty, this.m_Replacements);
			}
		}
		if (!this.m_bTagFound)
		{
			if (this.m_Replacements == null)
			{
				this.m_bTagFound = Localization.Get(this.m_LocalizationTag, out empty, new LocToken[0]);
			}
			else
			{
				this.m_bTagFound = Localization.Get(this.m_LocalizationTag, out empty, this.m_Replacements);
			}
		}
		if (this.m_bTagFound)
		{
			this.CheckMarkup(empty);
			this.text = empty;
		}
		else
		{
			this.CheckMarkup(empty);
			this.text = empty;
		}
		this.Apply(this.text);
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x0011F588 File Offset: 0x0011D988
	public void CheckMarkup()
	{
		if (!this.m_bTagFound && !string.IsNullOrEmpty(this.m_NonLocalizedKeyboardText) && this.ShouldUseKeyboardText())
		{
			this.CheckMarkup(this.m_NonLocalizedKeyboardText);
		}
		else
		{
			this.CheckMarkup(this.text);
		}
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x0011F5D8 File Offset: 0x0011D9D8
	private void CheckMarkup(string text)
	{
		if (this == null)
		{
			return;
		}
		this.m_MarkedUpString = text;
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x0011F5FC File Offset: 0x0011D9FC
	private void Apply(string str)
	{
		if (this.m_TextProcessors != null)
		{
			for (int i = 0; i < this.m_TextProcessors.Count; i++)
			{
				this.m_TextProcessors[i].ProcessText(ref str);
			}
		}
		this.m_MarkedUpString = str;
		this.text = str;
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x0011F654 File Offset: 0x0011DA54
	public void SetLocalisedTextCatchAll(string text)
	{
		bool flag;
		if (this.m_bNeedsLocalization)
		{
			flag = (this.m_LocalizationTag != text);
		}
		else
		{
			flag = (this.text != text);
		}
		if (flag)
		{
			this.text = text;
			this.SetNewPlaceHolder(text);
			this.m_bNeedsLocalization = true;
			this.SetNewLocalizationTag(text);
		}
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x0011F6AF File Offset: 0x0011DAAF
	public void SetNewLocalizationTag(string newTag)
	{
		if (this.m_bNeedsLocalization)
		{
			this.m_LocalizationTag = newTag;
			this.Convert();
		}
		else
		{
			this.m_LocalizationTag = string.Empty;
			this.CheckMarkup(newTag);
			this.Apply(newTag);
		}
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x0011F6E7 File Offset: 0x0011DAE7
	public void SetNewPlaceHolder(string newPlaceholder)
	{
		this.m_PlaceholderText = newPlaceholder;
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x0011F6F0 File Offset: 0x0011DAF0
	public void SetNonLocalizedText(string text)
	{
		this.ResetLocalisation();
		this.text = text;
		this.CheckMarkup();
		this.Apply(this.text);
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0011F711 File Offset: 0x0011DB11
	private void ResetLocalisation()
	{
		this.m_bNeedsLocalization = false;
		this.m_LocalizationTag = string.Empty;
		this.m_PlaceholderText = string.Empty;
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x0011F730 File Offset: 0x0011DB30
	private void LanguageChanged()
	{
		if (this.m_bNeedsLocalization)
		{
			this.Convert();
		}
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x0011F744 File Offset: 0x0011DB44
	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		bool flag = false;
		if (this.m_TextProcessors != null)
		{
			for (int i = 0; i < this.m_TextProcessors.Count; i++)
			{
				if (this.m_TextProcessors[i] != null && this.m_TextProcessors[i].HasEmbeddedImages(this.text))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			string text = this.m_Text;
			this.m_Text = this.m_MarkedUpString;
			base.OnPopulateMesh(toFill);
			this.m_Text = text;
			if (Application.isPlaying && this.m_TextProcessors != null)
			{
				for (int j = 0; j < this.m_TextProcessors.Count; j++)
				{
					if (this.m_TextProcessors[j] != null)
					{
						this.m_TextProcessors[j].OnPopulateMesh(toFill);
					}
				}
			}
		}
		else
		{
			base.OnPopulateMesh(toFill);
		}
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x0011F834 File Offset: 0x0011DC34
	private bool ShouldUseKeyboardText()
	{
		if (this.m_PlayerManager != null)
		{
			GamepadUser user = this.m_PlayerManager.GetUser(EngagementSlot.One);
			return user != null && user.ControlType == GamepadUser.ControlTypeEnum.Keyboard;
		}
		return false;
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x0011F879 File Offset: 0x0011DC79
	public T17EventSystem GetDomain()
	{
		return null;
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x0011F87C File Offset: 0x0011DC7C
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x0011F884 File Offset: 0x0011DC84
	public void SetEventSystem(T17EventSystem gamersEventSystem = null)
	{
		if (!this.m_bTagFound && !string.IsNullOrEmpty(this.m_NonLocalizedKeyboardText) && this.ShouldUseKeyboardText())
		{
			this.CheckMarkup(this.m_NonLocalizedKeyboardText);
			this.Apply(this.m_NonLocalizedKeyboardText);
		}
		else
		{
			this.CheckMarkup(this.text);
			this.Apply(this.text);
		}
	}

	// Token: 0x040030BA RID: 12474
	public bool m_bNeedsLocalization = true;

	// Token: 0x040030BB RID: 12475
	public string m_LocalizationTag = "XXX.YYY.ZZZ";

	// Token: 0x040030BC RID: 12476
	public string m_PlaceholderText = string.Empty;

	// Token: 0x040030BD RID: 12477
	public string m_NonLocalizedKeyboardText = string.Empty;

	// Token: 0x040030BE RID: 12478
	public bool m_bTagFound;

	// Token: 0x040030BF RID: 12479
	public LocToken[] m_Replacements;

	// Token: 0x040030C0 RID: 12480
	private static ThaiFontProcessor s_ThaiFontProcessor = new ThaiFontProcessor();

	// Token: 0x040030C1 RID: 12481
	public Func<bool> m_ReleaseOnPointerClickDelegate;

	// Token: 0x040030C2 RID: 12482
	private string m_MarkedUpString = string.Empty;

	// Token: 0x040030C3 RID: 12483
	private PlayerManager m_PlayerManager;

	// Token: 0x040030C4 RID: 12484
	private List<ITextProcessor> m_TextProcessors;

	// Token: 0x02000B7E RID: 2942
	[Serializable]
	public class ImageData
	{
		// Token: 0x040030C5 RID: 12485
		[SerializeField]
		[ReadOnly]
		public string m_OriginalMarkup;

		// Token: 0x040030C6 RID: 12486
		[SerializeField]
		[global::Tooltip("This can be changed in editor BUT will be changed to the correct image at runtime!")]
		public Sprite IconSprite;

		// Token: 0x040030C7 RID: 12487
		[SerializeField]
		[ReadOnly]
		public Vector2 Position = Vector2.zero;

		// Token: 0x040030C8 RID: 12488
		[SerializeField]
		[ReadOnly]
		public bool OutOfBounds;

		// Token: 0x040030C9 RID: 12489
		[SerializeField]
		public Vector2 Offset = Vector2.zero;

		// Token: 0x040030CA RID: 12490
		[SerializeField]
		public Vector2 AlignOffset = new Vector2(0f, 0.2f);

		// Token: 0x040030CB RID: 12491
		[SerializeField]
		public int Size = 34;

		// Token: 0x040030CC RID: 12492
		[SerializeField]
		[HideInInspector]
		public T17Image ImageObj;

		// Token: 0x040030CD RID: 12493
		[SerializeField]
		[HideInInspector]
		public int OriginalQuadIndex;

		// Token: 0x040030CE RID: 12494
		[SerializeField]
		[HideInInspector]
		public int PlaceInString;

		// Token: 0x040030CF RID: 12495
		[SerializeField]
		[HideInInspector]
		public bool NeedsToBeDeleted;
	}
}
