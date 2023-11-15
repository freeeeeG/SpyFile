using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C93 RID: 3219
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownListRow")]
public class BreakdownListRow : KMonoBehaviour
{
	// Token: 0x06006688 RID: 26248 RVA: 0x00263B10 File Offset: 0x00261D10
	public void ShowData(string name, string value)
	{
		base.gameObject.transform.localScale = Vector3.one;
		this.nameLabel.text = name;
		this.valueLabel.text = value;
		this.dotOutlineImage.gameObject.SetActive(true);
		Vector2 vector = Vector2.one * 0.6f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.dotInsideImage.gameObject.SetActive(true);
		this.dotInsideImage.color = BreakdownListRow.statusColour[0];
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetHighlighted(false);
		this.SetImportant(false);
	}

	// Token: 0x06006689 RID: 26249 RVA: 0x00263BEC File Offset: 0x00261DEC
	public void ShowStatusData(string name, string value, BreakdownListRow.Status dotColor)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetStatusColor(dotColor);
	}

	// Token: 0x0600668A RID: 26250 RVA: 0x00263C4C File Offset: 0x00261E4C
	public void SetStatusColor(BreakdownListRow.Status dotColor)
	{
		this.checkmarkImage.gameObject.SetActive(dotColor > BreakdownListRow.Status.Default);
		this.checkmarkImage.color = BreakdownListRow.statusColour[(int)dotColor];
		switch (dotColor)
		{
		case BreakdownListRow.Status.Red:
			this.checkmarkImage.sprite = this.statusFailureIcon;
			return;
		case BreakdownListRow.Status.Green:
			this.checkmarkImage.sprite = this.statusSuccessIcon;
			return;
		case BreakdownListRow.Status.Yellow:
			this.checkmarkImage.sprite = this.statusWarningIcon;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600668B RID: 26251 RVA: 0x00263CD0 File Offset: 0x00261ED0
	public void ShowCheckmarkData(string name, string value, BreakdownListRow.Status status)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.SetStatusColor(status);
	}

	// Token: 0x0600668C RID: 26252 RVA: 0x00263D34 File Offset: 0x00261F34
	public void ShowIconData(string name, string value, Sprite sprite)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(true);
		this.checkmarkImage.gameObject.SetActive(false);
		this.iconImage.sprite = sprite;
		this.iconImage.color = Color.white;
	}

	// Token: 0x0600668D RID: 26253 RVA: 0x00263DA9 File Offset: 0x00261FA9
	public void ShowIconData(string name, string value, Sprite sprite, Color spriteColor)
	{
		this.ShowIconData(name, value, sprite);
		this.iconImage.color = spriteColor;
	}

	// Token: 0x0600668E RID: 26254 RVA: 0x00263DC4 File Offset: 0x00261FC4
	public void SetHighlighted(bool highlighted)
	{
		this.isHighlighted = highlighted;
		Vector2 vector = Vector2.one * 0.8f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.nameLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
		this.valueLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
	}

	// Token: 0x0600668F RID: 26255 RVA: 0x00263E50 File Offset: 0x00262050
	public void SetDisabled(bool disabled)
	{
		this.isDisabled = disabled;
		this.nameLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
		this.valueLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
	}

	// Token: 0x06006690 RID: 26256 RVA: 0x00263EA4 File Offset: 0x002620A4
	public void SetImportant(bool important)
	{
		this.isImportant = important;
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.nameLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.valueLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.nameLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
		this.valueLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
	}

	// Token: 0x06006691 RID: 26257 RVA: 0x00263F3C File Offset: 0x0026213C
	public void HideIcon()
	{
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
	}

	// Token: 0x06006692 RID: 26258 RVA: 0x00263F8D File Offset: 0x0026218D
	public void AddTooltip(string tooltipText)
	{
		if (this.tooltip == null)
		{
			this.tooltip = base.gameObject.AddComponent<ToolTip>();
		}
		this.tooltip.SetSimpleTooltip(tooltipText);
	}

	// Token: 0x06006693 RID: 26259 RVA: 0x00263FBA File Offset: 0x002621BA
	public void ClearTooltip()
	{
		if (this.tooltip != null)
		{
			this.tooltip.ClearMultiStringTooltip();
		}
	}

	// Token: 0x06006694 RID: 26260 RVA: 0x00263FD5 File Offset: 0x002621D5
	public void SetValue(string value)
	{
		this.valueLabel.text = value;
	}

	// Token: 0x040046B2 RID: 18098
	private static Color[] statusColour = new Color[]
	{
		new Color(0.34117648f, 0.36862746f, 0.45882353f, 1f),
		new Color(0.72156864f, 0.38431373f, 0f, 1f),
		new Color(0.38431373f, 0.72156864f, 0f, 1f),
		new Color(0.72156864f, 0.72156864f, 0f, 1f)
	};

	// Token: 0x040046B3 RID: 18099
	public Image dotOutlineImage;

	// Token: 0x040046B4 RID: 18100
	public Image dotInsideImage;

	// Token: 0x040046B5 RID: 18101
	public Image iconImage;

	// Token: 0x040046B6 RID: 18102
	public Image checkmarkImage;

	// Token: 0x040046B7 RID: 18103
	public LocText nameLabel;

	// Token: 0x040046B8 RID: 18104
	public LocText valueLabel;

	// Token: 0x040046B9 RID: 18105
	private bool isHighlighted;

	// Token: 0x040046BA RID: 18106
	private bool isDisabled;

	// Token: 0x040046BB RID: 18107
	private bool isImportant;

	// Token: 0x040046BC RID: 18108
	private ToolTip tooltip;

	// Token: 0x040046BD RID: 18109
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x040046BE RID: 18110
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x040046BF RID: 18111
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x02001BB9 RID: 7097
	public enum Status
	{
		// Token: 0x04007DAE RID: 32174
		Default,
		// Token: 0x04007DAF RID: 32175
		Red,
		// Token: 0x04007DB0 RID: 32176
		Green,
		// Token: 0x04007DB1 RID: 32177
		Yellow
	}
}
