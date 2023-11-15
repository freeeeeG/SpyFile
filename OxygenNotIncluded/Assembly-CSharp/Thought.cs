using System;
using UnityEngine;

// Token: 0x02000A03 RID: 2563
public class Thought : Resource
{
	// Token: 0x06004CA7 RID: 19623 RVA: 0x001ADF50 File Offset: 0x001AC150
	public Thought(string id, ResourceSet parent, Sprite icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = icon;
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x06004CA8 RID: 19624 RVA: 0x001ADFC0 File Offset: 0x001AC1C0
	public Thought(string id, ResourceSet parent, string icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = Assets.GetSprite(icon);
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x040031FC RID: 12796
	public int priority;

	// Token: 0x040031FD RID: 12797
	public Sprite sprite;

	// Token: 0x040031FE RID: 12798
	public Sprite modeSprite;

	// Token: 0x040031FF RID: 12799
	public string sound;

	// Token: 0x04003200 RID: 12800
	public Sprite bubbleSprite;

	// Token: 0x04003201 RID: 12801
	public string speechPrefix;

	// Token: 0x04003202 RID: 12802
	public LocString hoverText;

	// Token: 0x04003203 RID: 12803
	public bool showImmediately;

	// Token: 0x04003204 RID: 12804
	public float showTime;
}
