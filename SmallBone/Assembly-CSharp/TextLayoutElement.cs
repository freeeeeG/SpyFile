using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AE RID: 174
[ExecuteAlways]
public class TextLayoutElement : MonoBehaviour, ILayoutElement
{
	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000372 RID: 882 RVA: 0x0000CC6D File Offset: 0x0000AE6D
	// (set) Token: 0x06000373 RID: 883 RVA: 0x0000CC7A File Offset: 0x0000AE7A
	public string text
	{
		get
		{
			return this._text.text;
		}
		set
		{
			this._text.text = value;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000374 RID: 884 RVA: 0x0000CC88 File Offset: 0x0000AE88
	public float preferredWidth
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000375 RID: 885 RVA: 0x0000CC88 File Offset: 0x0000AE88
	public float flexibleWidth
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000376 RID: 886 RVA: 0x0000CC8F File Offset: 0x0000AE8F
	public float minWidth
	{
		get
		{
			return this._minWidth;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000377 RID: 887 RVA: 0x0000CC97 File Offset: 0x0000AE97
	public float minHeight
	{
		get
		{
			return this._minHeight;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000378 RID: 888 RVA: 0x0000CC88 File Offset: 0x0000AE88
	public float preferredHeight
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000379 RID: 889 RVA: 0x0000CC88 File Offset: 0x0000AE88
	public float flexibleHeight
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600037A RID: 890 RVA: 0x0000CC9F File Offset: 0x0000AE9F
	public int layoutPriority
	{
		get
		{
			return this._layoutPriority;
		}
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
	public void CalculateLayoutInputHorizontal()
	{
		if (this._text == null)
		{
			this._minWidth = -1f;
			return;
		}
		this._minWidth = Mathf.Min(this._text.preferredWidth, this._text.rectTransform.rect.width) + this._padding;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0000CD04 File Offset: 0x0000AF04
	public void CalculateLayoutInputVertical()
	{
		if (this._text == null)
		{
			this._minHeight = -1f;
			return;
		}
		this._minHeight = this._text.preferredHeight + this._padding;
		if (this._userMinHeight > 0f)
		{
			this._minHeight = Mathf.Max(this._userMinHeight, this._minHeight);
		}
		this._minHeight = Mathf.Min(this._maxHeight, this._minHeight);
	}

	// Token: 0x040002CD RID: 717
	[SerializeField]
	private TextMeshProUGUI _text;

	// Token: 0x040002CE RID: 718
	[SerializeField]
	private int _layoutPriority = 1;

	// Token: 0x040002CF RID: 719
	[SerializeField]
	private float _padding;

	// Token: 0x040002D0 RID: 720
	[SerializeField]
	private float _userMinHeight;

	// Token: 0x040002D1 RID: 721
	[SerializeField]
	private float _maxWidth = 100f;

	// Token: 0x040002D2 RID: 722
	[SerializeField]
	private float _maxHeight = float.PositiveInfinity;

	// Token: 0x040002D3 RID: 723
	private float _minWidth = -1f;

	// Token: 0x040002D4 RID: 724
	private float _minHeight = -1f;
}
