using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1B RID: 3099
[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : KMonoBehaviour
{
	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x0600620F RID: 25103 RVA: 0x00243604 File Offset: 0x00241804
	// (set) Token: 0x06006210 RID: 25104 RVA: 0x0024360C File Offset: 0x0024180C
	public new Tag tag { get; private set; }

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06006211 RID: 25105 RVA: 0x00243615 File Offset: 0x00241815
	// (set) Token: 0x06006212 RID: 25106 RVA: 0x0024361D File Offset: 0x0024181D
	public bool isSelected { get; private set; }

	// Token: 0x06006213 RID: 25107 RVA: 0x00243628 File Offset: 0x00241828
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.regularColor = this.outline.color;
		if (this.button != null)
		{
			this.button.onPointerEnter += delegate()
			{
				if (!this.isSelected)
				{
					this.outline.color = this.outlineHighLightColor;
				}
			};
			this.button.onPointerExit += delegate()
			{
				if (!this.isSelected)
				{
					this.outline.color = this.regularColor;
				}
			};
		}
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x00243688 File Offset: 0x00241888
	public void SetTag(Tag tag)
	{
		this.tag = tag;
		this.SetText((tag == GameTags.Void) ? UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION.text : tag.ProperName());
	}

	// Token: 0x06006215 RID: 25109 RVA: 0x002436B6 File Offset: 0x002418B6
	private void SetText(string assignmentStr)
	{
		this.labelText.text = ((!string.IsNullOrEmpty(assignmentStr)) ? assignmentStr : "-");
	}

	// Token: 0x06006216 RID: 25110 RVA: 0x002436D3 File Offset: 0x002418D3
	public void SetSelected(bool selected)
	{
		this.isSelected = selected;
		this.outline.color = (selected ? this.outlineHighLightColor : this.outlineDefaultColor);
		this.BG.color = (selected ? this.BGHighLightColor : Color.white);
	}

	// Token: 0x040042D2 RID: 17106
	[SerializeField]
	private LocText labelText;

	// Token: 0x040042D3 RID: 17107
	[SerializeField]
	private Image BG;

	// Token: 0x040042D4 RID: 17108
	[SerializeField]
	private Image outline;

	// Token: 0x040042D5 RID: 17109
	[SerializeField]
	private Color outlineHighLightColor = new Color32(168, 74, 121, byte.MaxValue);

	// Token: 0x040042D6 RID: 17110
	[SerializeField]
	private Color BGHighLightColor = new Color32(168, 74, 121, 80);

	// Token: 0x040042D7 RID: 17111
	[SerializeField]
	private Color outlineDefaultColor = new Color32(204, 204, 204, byte.MaxValue);

	// Token: 0x040042D8 RID: 17112
	private Color regularColor = Color.white;

	// Token: 0x040042D9 RID: 17113
	[SerializeField]
	public KButton button;
}
