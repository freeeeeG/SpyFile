using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AA8 RID: 2728
public struct AsteroidDescriptor
{
	// Token: 0x06005341 RID: 21313 RVA: 0x001DD51F File Offset: 0x001DB71F
	public AsteroidDescriptor(string text, string tooltip, Color associatedColor, List<global::Tuple<string, Color, float>> bands = null, string associatedIcon = null)
	{
		this.text = text;
		this.tooltip = tooltip;
		this.associatedColor = associatedColor;
		this.bands = bands;
		this.associatedIcon = associatedIcon;
	}

	// Token: 0x04003770 RID: 14192
	public string text;

	// Token: 0x04003771 RID: 14193
	public string tooltip;

	// Token: 0x04003772 RID: 14194
	public List<global::Tuple<string, Color, float>> bands;

	// Token: 0x04003773 RID: 14195
	public Color associatedColor;

	// Token: 0x04003774 RID: 14196
	public string associatedIcon;
}
