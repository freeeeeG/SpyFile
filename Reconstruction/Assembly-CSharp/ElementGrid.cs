using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023C RID: 572
public class ElementGrid : MonoBehaviour
{
	// Token: 0x06000EB8 RID: 3768 RVA: 0x00026034 File Offset: 0x00024234
	public void SetElement(Composition composition)
	{
		this.element = (ElementType)composition.elementRequirement;
		this.quality = composition.qualityRequeirement;
		TurretAttribute elementAttribute = ConstructHelper.GetElementAttribute(this.element);
		this.Img_Icon.sprite = elementAttribute.TurretLevels[this.quality - 1].TurretIcon;
		this.Txt_ElementName.text = StaticData.FormElementName(this.element, this.quality);
		if (composition.isPerfect)
		{
			this.perfectIcon.SetActive(true);
		}
		else
		{
			this.perfectIcon.SetActive(false);
		}
		if (composition.obtained)
		{
			this.Img_Icon.color = Color.white;
			this.Txt_ElementName.color = Singleton<StaticData>.Instance.HighlightBlue;
			return;
		}
		this.Img_Icon.color = this.UnobtainColor;
		this.Txt_ElementName.color = Singleton<StaticData>.Instance.NormalBlue;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0002611A File Offset: 0x0002431A
	public void SetPreview(bool value, ElementType element, int quality)
	{
		if (!value)
		{
			this.previewGlow.SetActive(false);
			return;
		}
		if (this.element == element && this.quality == quality)
		{
			this.previewGlow.SetActive(true);
			return;
		}
		this.previewGlow.SetActive(false);
	}

	// Token: 0x04000719 RID: 1817
	[SerializeField]
	private Image Img_Icon;

	// Token: 0x0400071A RID: 1818
	[SerializeField]
	private Text Txt_ElementName;

	// Token: 0x0400071B RID: 1819
	[SerializeField]
	private Color UnobtainColor;

	// Token: 0x0400071C RID: 1820
	[SerializeField]
	private GameObject previewGlow;

	// Token: 0x0400071D RID: 1821
	[SerializeField]
	private GameObject perfectIcon;

	// Token: 0x0400071E RID: 1822
	private ElementType element;

	// Token: 0x0400071F RID: 1823
	private int quality;
}
