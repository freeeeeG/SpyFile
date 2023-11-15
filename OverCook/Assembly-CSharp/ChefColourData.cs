using System;
using UnityEngine;

// Token: 0x020009D7 RID: 2519
public class ChefColourData : ScriptableObject
{
	// Token: 0x04002781 RID: 10113
	public Material ChefMaterial;

	// Token: 0x04002782 RID: 10114
	[Header("Colour Mask")]
	public Color MaskColour;

	// Token: 0x04002783 RID: 10115
	[Header("UI")]
	public Color UIColour;

	// Token: 0x04002784 RID: 10116
	public Color DarkUIColour;

	// Token: 0x04002785 RID: 10117
	public Color PadUIColour;

	// Token: 0x04002786 RID: 10118
	public Color PadBarColour;

	// Token: 0x04002787 RID: 10119
	[Space]
	public Sprite Background;
}
