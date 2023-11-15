using System;
using UnityEngine;

// Token: 0x02000772 RID: 1906
public class Dream : Resource
{
	// Token: 0x060034B2 RID: 13490 RVA: 0x0011B018 File Offset: 0x00119218
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names) : base(id, parent, null)
	{
		this.Icons = new Sprite[icons_sprite_names.Length];
		this.BackgroundAnim = background;
		for (int i = 0; i < icons_sprite_names.Length; i++)
		{
			this.Icons[i] = Assets.GetSprite(icons_sprite_names[i]);
		}
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x0011B074 File Offset: 0x00119274
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names, float durationPerImage) : this(id, parent, background, icons_sprite_names)
	{
		this.secondPerImage = durationPerImage;
	}

	// Token: 0x04001FCD RID: 8141
	public string BackgroundAnim;

	// Token: 0x04001FCE RID: 8142
	public Sprite[] Icons;

	// Token: 0x04001FCF RID: 8143
	public float secondPerImage = 2.4f;
}
