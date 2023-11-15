using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000275 RID: 629
public class UITujian_ListHolder : MonoBehaviour
{
	// Token: 0x06000F9E RID: 3998 RVA: 0x00029BC0 File Offset: 0x00027DC0
	public void SetContent(ToggleGroup group)
	{
		this.itemParent = base.transform.Find("ListPanel");
		foreach (ContentAttribute attribute in this.attributes)
		{
			Object.Instantiate<ItemSlot>(this.itemSlotPrefab, this.itemParent).SetContent(attribute, group);
		}
	}

	// Token: 0x040007FE RID: 2046
	[SerializeField]
	private ContentAttribute[] attributes;

	// Token: 0x040007FF RID: 2047
	[SerializeField]
	private ItemSlot itemSlotPrefab;

	// Token: 0x04000800 RID: 2048
	private Transform itemParent;
}
