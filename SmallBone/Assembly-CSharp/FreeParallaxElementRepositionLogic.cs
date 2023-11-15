using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
[Serializable]
public class FreeParallaxElementRepositionLogic
{
	// Token: 0x040000E5 RID: 229
	[Tooltip("Set whether to wrap the object (for a full width (or height for vertical parallax) element, or to use individual elements such as trees, clouds and light rays.")]
	public FreeParallaxPositionMode PositionMode = FreeParallaxPositionMode.WrapAnchorBottom;

	// Token: 0x040000E6 RID: 230
	[Tooltip("Set to a percentage of the screen height (or width for vertical parallax) to scale the element. Set to 0 to leave as the original scale, or 1 to scale to the height (or width for a vertical parallax) of the screen.")]
	public float ScaleHeight;

	// Token: 0x040000E7 RID: 231
	[Tooltip("Sorting order for rendering. Leave as 0 to use original sort order.")]
	public int SortingOrder;

	// Token: 0x040000E8 RID: 232
	[Tooltip("Minimum y percent in viewport space to reposition when object leaves the screen. 0.5 would position it at least half way up the screen for a horizontal parallax, or 5 would position it at least 5 screen heights away for a vertical parallax.")]
	public float MinYPercent;

	// Token: 0x040000E9 RID: 233
	[Tooltip("Maximum y percent in viewport space to reposition when object leaves the screen. 0.75 would position it no more than 3/4 up the screen for a horizontal parallax, or 10 would position it no more than 10 screen heights away for a vertical parallax.")]
	public float MaxYPercent;

	// Token: 0x040000EA RID: 234
	[Tooltip("Minimum x percent in viewport space to reposition when object leaves the screen. 5 would position it at least 5 screen widths away for a horizontal parallax, or 0.5 would position it at least half way across the screen for a vertical parallax.")]
	public float MinXPercent;

	// Token: 0x040000EB RID: 235
	[Tooltip("Maximum x percent in viewport space to reposition when object leaves the screen. 10 would position it no more than 10 screen widths away for a horizontal parallax or 0.75 would position it no more than 3/4 across the screen for a vertical parallax.")]
	public float MaxXPercent;
}
