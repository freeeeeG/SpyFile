using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000192 RID: 402
public class TechListPanel : IUserInterface
{
	// Token: 0x06000A2E RID: 2606 RVA: 0x0001BAB8 File Offset: 0x00019CB8
	public void AddTech(Technology tech)
	{
		TechItem techItem = Object.Instantiate<TechItem>(this.techItemPrefab, this.techItemParent);
		techItem.SetTechItem(tech);
		this.techItems.Add(techItem);
		this.ResetPos();
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0001BAF0 File Offset: 0x00019CF0
	private void ResetPos()
	{
		base.StartCoroutine(this.ResetCor());
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0001BAFF File Offset: 0x00019CFF
	private IEnumerator ResetCor()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.techItemParent);
		yield return new WaitForEndOfFrame();
		this.rect.anchoredPosition = new Vector2(0f, -this.rect.sizeDelta.y / 2f + 60f);
		yield break;
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0001BB0E File Offset: 0x00019D0E
	public override void Show()
	{
		base.Show();
		this.ResetPos();
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0001BB1C File Offset: 0x00019D1C
	public void RemoveTech(Technology tech)
	{
		foreach (TechItem techItem in this.techItems.ToList<TechItem>())
		{
			if (techItem.MyTech.TechName == tech.TechName)
			{
				this.techItems.Remove(techItem);
				Object.Destroy(techItem.gameObject);
				this.ResetPos();
				break;
			}
		}
	}

	// Token: 0x04000567 RID: 1383
	[SerializeField]
	private TechItem techItemPrefab;

	// Token: 0x04000568 RID: 1384
	[SerializeField]
	private RectTransform techItemParent;

	// Token: 0x04000569 RID: 1385
	[SerializeField]
	private RectTransform rect;

	// Token: 0x0400056A RID: 1386
	private List<TechItem> techItems = new List<TechItem>();
}
