using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000136 RID: 310
[Serializable]
public class SpriteGroupInfo
{
	// Token: 0x0400067E RID: 1662
	[Tooltip("Set the colour for this sprite group.")]
	public Color colour = Color.white;

	// Token: 0x0400067F RID: 1663
	[Tooltip("Set the sorting layer for this sprite group.")]
	public string sortingLayer;

	// Token: 0x04000680 RID: 1664
	[Tooltip("Set the sorting order for this sprite group.")]
	public int sortingOrder;

	// Token: 0x04000681 RID: 1665
	[Tooltip("The sprite group: Drag game objects that have sprite renderers attached from the hierarchy into this list.")]
	public List<SpriteRenderer> spriteRendererList;

	// Token: 0x04000682 RID: 1666
	[Tooltip("Do you want to update the colour of this sprite group?")]
	public bool updateColour;

	// Token: 0x04000683 RID: 1667
	[Tooltip("Do you want to update the sorting layer of this sprite group?")]
	public bool updateSortingLayer;

	// Token: 0x04000684 RID: 1668
	[Tooltip("Do you want to update the sorting order of this sprite group?")]
	public bool updateSortingOrder;
}
