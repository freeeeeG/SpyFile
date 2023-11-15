using System;
using UnityEngine;

// Token: 0x02000BB8 RID: 3000
public class LegendEntry
{
	// Token: 0x06005DCE RID: 24014 RVA: 0x002252B0 File Offset: 0x002234B0
	public LegendEntry(string name, string desc, Color colour, string desc_arg = null, Sprite sprite = null, bool displaySprite = true)
	{
		this.name = name;
		this.desc = desc;
		this.colour = colour;
		this.desc_arg = desc_arg;
		this.sprite = ((sprite == null) ? Assets.instance.LegendColourBox : sprite);
		this.displaySprite = displaySprite;
	}

	// Token: 0x04003F29 RID: 16169
	public string name;

	// Token: 0x04003F2A RID: 16170
	public string desc;

	// Token: 0x04003F2B RID: 16171
	public string desc_arg;

	// Token: 0x04003F2C RID: 16172
	public Color colour;

	// Token: 0x04003F2D RID: 16173
	public Sprite sprite;

	// Token: 0x04003F2E RID: 16174
	public bool displaySprite;
}
