using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public enum FreeParallaxPositionMode
{
	// Token: 0x040000E0 RID: 224
	[Tooltip("Wrap and anchor to the top (or right for a vertical parallax) of the screen")]
	WrapAnchorTop,
	// Token: 0x040000E1 RID: 225
	[Tooltip("Wrap and anchor to the bottom (or left for a vertical parallax) of the screen")]
	WrapAnchorBottom,
	// Token: 0x040000E2 RID: 226
	[Tooltip("No wrap, this is an individual object that starts off screen")]
	IndividualStartOffScreen,
	// Token: 0x040000E3 RID: 227
	[Tooltip("No wrap, this is an individual object that starts on screen")]
	IndividualStartOnScreen,
	// Token: 0x040000E4 RID: 228
	[Tooltip("Wrap and maintain original position")]
	WrapAnchorNone
}
