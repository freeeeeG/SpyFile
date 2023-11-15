using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
[Serializable]
public class ColorSet
{
	// Token: 0x06000068 RID: 104 RVA: 0x0000408E File Offset: 0x0000228E
	public Color GetColorWithHue(float hue)
	{
		return Color.HSVToRGB(hue, this.saturation / 100f, this.value / 100f);
	}

	// Token: 0x040000B3 RID: 179
	[Range(0f, 100f)]
	public float saturation = 100f;

	// Token: 0x040000B4 RID: 180
	[Range(0f, 100f)]
	public float value = 100f;
}
