using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B33 RID: 2867
public class KleiInventoryUISubcategory : KMonoBehaviour
{
	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x0600588E RID: 22670 RVA: 0x0020791B File Offset: 0x00205B1B
	public bool IsOpen
	{
		get
		{
			return this.stateExpanded;
		}
	}

	// Token: 0x0600588F RID: 22671 RVA: 0x00207923 File Offset: 0x00205B23
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.expandButton.onClick = delegate()
		{
			this.ToggleOpen(!this.stateExpanded);
		};
	}

	// Token: 0x06005890 RID: 22672 RVA: 0x00207942 File Offset: 0x00205B42
	public void SetIdentity(string label, Sprite icon)
	{
		this.label.SetText(label);
		this.icon.sprite = icon;
	}

	// Token: 0x06005891 RID: 22673 RVA: 0x0020795C File Offset: 0x00205B5C
	public void RefreshDisplay()
	{
		foreach (GameObject gameObject in this.dummyItems)
		{
			gameObject.SetActive(false);
		}
		int num = 0;
		for (int i = 0; i < this.gridLayout.transform.childCount; i++)
		{
			if (this.gridLayout.transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		base.gameObject.SetActive(num != 0);
		int j = 0;
		int num2 = num % this.gridLayout.constraintCount;
		if (num2 > 0)
		{
			j = this.gridLayout.constraintCount - num2;
		}
		while (j > this.dummyItems.Count)
		{
			this.dummyItems.Add(Util.KInstantiateUI(this.dummyPrefab, this.gridLayout.gameObject, false));
		}
		for (int k = 0; k < j; k++)
		{
			this.dummyItems[k].SetActive(true);
			this.dummyItems[k].transform.SetAsLastSibling();
		}
		this.headerLayout.minWidth = base.transform.parent.rectTransform().rect.width - 8f;
	}

	// Token: 0x06005892 RID: 22674 RVA: 0x00207ABC File Offset: 0x00205CBC
	public void ToggleOpen(bool open)
	{
		this.gridLayout.gameObject.SetActive(open);
		this.stateExpanded = open;
		this.expandButton.ChangeState(this.stateExpanded ? 1 : 0);
	}

	// Token: 0x04003BE6 RID: 15334
	[SerializeField]
	private GameObject dummyPrefab;

	// Token: 0x04003BE7 RID: 15335
	public string subcategoryID;

	// Token: 0x04003BE8 RID: 15336
	public GridLayoutGroup gridLayout;

	// Token: 0x04003BE9 RID: 15337
	public List<GameObject> dummyItems;

	// Token: 0x04003BEA RID: 15338
	[SerializeField]
	private LayoutElement headerLayout;

	// Token: 0x04003BEB RID: 15339
	[SerializeField]
	private Image icon;

	// Token: 0x04003BEC RID: 15340
	[SerializeField]
	private LocText label;

	// Token: 0x04003BED RID: 15341
	[SerializeField]
	private MultiToggle expandButton;

	// Token: 0x04003BEE RID: 15342
	private bool stateExpanded = true;
}
