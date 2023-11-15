using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C92 RID: 3218
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownList")]
public class BreakdownList : KMonoBehaviour
{
	// Token: 0x06006681 RID: 26241 RVA: 0x0026391C File Offset: 0x00261B1C
	public BreakdownListRow AddRow()
	{
		BreakdownListRow breakdownListRow;
		if (this.unusedListRows.Count > 0)
		{
			breakdownListRow = this.unusedListRows[0];
			this.unusedListRows.RemoveAt(0);
		}
		else
		{
			breakdownListRow = UnityEngine.Object.Instantiate<BreakdownListRow>(this.listRowTemplate);
		}
		breakdownListRow.gameObject.transform.SetParent(base.transform);
		breakdownListRow.gameObject.transform.SetAsLastSibling();
		this.listRows.Add(breakdownListRow);
		breakdownListRow.gameObject.SetActive(true);
		return breakdownListRow;
	}

	// Token: 0x06006682 RID: 26242 RVA: 0x0026399D File Offset: 0x00261B9D
	public GameObject AddCustomRow(GameObject newRow)
	{
		newRow.transform.SetParent(base.transform);
		newRow.gameObject.transform.SetAsLastSibling();
		this.customRows.Add(newRow);
		newRow.SetActive(true);
		return newRow;
	}

	// Token: 0x06006683 RID: 26243 RVA: 0x002639D4 File Offset: 0x00261BD4
	public void ClearRows()
	{
		foreach (BreakdownListRow breakdownListRow in this.listRows)
		{
			this.unusedListRows.Add(breakdownListRow);
			breakdownListRow.gameObject.SetActive(false);
			breakdownListRow.ClearTooltip();
		}
		this.listRows.Clear();
		foreach (GameObject gameObject in this.customRows)
		{
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06006684 RID: 26244 RVA: 0x00263A8C File Offset: 0x00261C8C
	public void SetTitle(string title)
	{
		this.headerTitle.text = title;
	}

	// Token: 0x06006685 RID: 26245 RVA: 0x00263A9A File Offset: 0x00261C9A
	public void SetDescription(string description)
	{
		if (description != null && description.Length >= 0)
		{
			this.infoTextLabel.gameObject.SetActive(true);
			this.infoTextLabel.text = description;
			return;
		}
		this.infoTextLabel.gameObject.SetActive(false);
	}

	// Token: 0x06006686 RID: 26246 RVA: 0x00263AD7 File Offset: 0x00261CD7
	public void SetIcon(Sprite icon)
	{
		this.headerIcon.sprite = icon;
	}

	// Token: 0x040046A8 RID: 18088
	public Image headerIcon;

	// Token: 0x040046A9 RID: 18089
	public Sprite headerIconSprite;

	// Token: 0x040046AA RID: 18090
	public Image headerBar;

	// Token: 0x040046AB RID: 18091
	public LocText headerTitle;

	// Token: 0x040046AC RID: 18092
	public LocText headerValue;

	// Token: 0x040046AD RID: 18093
	public LocText infoTextLabel;

	// Token: 0x040046AE RID: 18094
	public BreakdownListRow listRowTemplate;

	// Token: 0x040046AF RID: 18095
	private List<BreakdownListRow> listRows = new List<BreakdownListRow>();

	// Token: 0x040046B0 RID: 18096
	private List<BreakdownListRow> unusedListRows = new List<BreakdownListRow>();

	// Token: 0x040046B1 RID: 18097
	private List<GameObject> customRows = new List<GameObject>();
}
