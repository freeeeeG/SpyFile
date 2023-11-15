using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AC8 RID: 2760
public class CodexImage : CodexWidget<CodexImage>
{
	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x0600550A RID: 21770 RVA: 0x001EF168 File Offset: 0x001ED368
	// (set) Token: 0x0600550B RID: 21771 RVA: 0x001EF170 File Offset: 0x001ED370
	public Sprite sprite { get; set; }

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x0600550C RID: 21772 RVA: 0x001EF179 File Offset: 0x001ED379
	// (set) Token: 0x0600550D RID: 21773 RVA: 0x001EF181 File Offset: 0x001ED381
	public Color color { get; set; }

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x0600550F RID: 21775 RVA: 0x001EF19D File Offset: 0x001ED39D
	// (set) Token: 0x0600550E RID: 21774 RVA: 0x001EF18A File Offset: 0x001ED38A
	public string spriteName
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			this.sprite = Assets.GetSprite(value);
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06005511 RID: 21777 RVA: 0x001EF230 File Offset: 0x001ED430
	// (set) Token: 0x06005510 RID: 21776 RVA: 0x001EF1CC File Offset: 0x001ED3CC
	public string batchedAnimPrefabSourceID
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			GameObject prefab = Assets.GetPrefab(value);
			KBatchedAnimController kbatchedAnimController = (prefab != null) ? prefab.GetComponent<KBatchedAnimController>() : null;
			KAnimFile kanimFile = (kbatchedAnimController != null) ? kbatchedAnimController.AnimFiles[0] : null;
			this.sprite = ((kanimFile != null) ? Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "") : null);
		}
	}

	// Token: 0x06005512 RID: 21778 RVA: 0x001EF25C File Offset: 0x001ED45C
	public CodexImage()
	{
		this.color = Color.white;
	}

	// Token: 0x06005513 RID: 21779 RVA: 0x001EF26F File Offset: 0x001ED46F
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color) : base(preferredWidth, preferredHeight)
	{
		this.sprite = sprite;
		this.color = color;
	}

	// Token: 0x06005514 RID: 21780 RVA: 0x001EF288 File Offset: 0x001ED488
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite) : this(preferredWidth, preferredHeight, sprite, Color.white)
	{
	}

	// Token: 0x06005515 RID: 21781 RVA: 0x001EF298 File Offset: 0x001ED498
	public CodexImage(int preferredWidth, int preferredHeight, global::Tuple<Sprite, Color> coloredSprite) : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
	{
	}

	// Token: 0x06005516 RID: 21782 RVA: 0x001EF2AE File Offset: 0x001ED4AE
	public CodexImage(global::Tuple<Sprite, Color> coloredSprite) : this(-1, -1, coloredSprite)
	{
	}

	// Token: 0x06005517 RID: 21783 RVA: 0x001EF2B9 File Offset: 0x001ED4B9
	public void ConfigureImage(Image image)
	{
		image.sprite = this.sprite;
		image.color = this.color;
	}

	// Token: 0x06005518 RID: 21784 RVA: 0x001EF2D3 File Offset: 0x001ED4D3
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureImage(contentGameObject.GetComponent<Image>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
