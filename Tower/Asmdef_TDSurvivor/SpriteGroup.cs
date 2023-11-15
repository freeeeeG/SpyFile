using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000135 RID: 309
[ExecuteInEditMode]
public class SpriteGroup : MonoBehaviour
{
	// Token: 0x06000800 RID: 2048 RVA: 0x0001E8A8 File Offset: 0x0001CAA8
	public void SetColour(int spriteGroupsIndex, Color colour)
	{
		if (this.spriteGroups != null && this.spriteGroups.Count >= spriteGroupsIndex + 1)
		{
			for (int i = 0; i < this.spriteGroups[spriteGroupsIndex].spriteRendererList.Count; i++)
			{
				this.spriteGroups[spriteGroupsIndex].spriteRendererList[i].color = colour;
			}
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0001E90C File Offset: 0x0001CB0C
	public void SetSortingLayer(int spriteGroupsIndex, string sortingLayer)
	{
		if (this.spriteGroups != null && this.spriteGroups.Count >= spriteGroupsIndex + 1)
		{
			for (int i = 0; i < this.spriteGroups[spriteGroupsIndex].spriteRendererList.Count; i++)
			{
				this.spriteGroups[spriteGroupsIndex].spriteRendererList[i].sortingLayerName = sortingLayer;
			}
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0001E970 File Offset: 0x0001CB70
	public void SetSortingOrder(int spriteGroupsIndex, int sortingOrder)
	{
		if (this.spriteGroups != null && this.spriteGroups.Count >= spriteGroupsIndex + 1)
		{
			for (int i = 0; i < this.spriteGroups[spriteGroupsIndex].spriteRendererList.Count; i++)
			{
				this.spriteGroups[spriteGroupsIndex].spriteRendererList[i].sortingOrder = sortingOrder;
			}
		}
	}

	// Token: 0x0400067D RID: 1661
	[Tooltip("To create a new group of sprites that can be edited simultaneously, click the arrow and type the number of groups of sprites you want to create into the field.")]
	public List<SpriteGroupInfo> spriteGroups;
}
