using System;
using UnityEngine;

// Token: 0x020009B2 RID: 2482
[ExecuteInEditMode]
public class ScrollingListGUI : MonoBehaviour, IScrollingListUI
{
	// Token: 0x0600309F RID: 12447 RVA: 0x000E48B5 File Offset: 0x000E2CB5
	public void SetNames(string[] _names)
	{
		this.m_scrollingListWidget.SetNames(_names);
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x000E48C3 File Offset: 0x000E2CC3
	public void MoveUp()
	{
		this.m_scrollingListWidget.MoveUp();
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000E48D0 File Offset: 0x000E2CD0
	public void MoveDown()
	{
		this.m_scrollingListWidget.MoveDown();
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000E48DD File Offset: 0x000E2CDD
	public int GetSelection()
	{
		return this.m_scrollingListWidget.GetSelection();
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x000E48EA File Offset: 0x000E2CEA
	protected ScrollingListWidgetConfig GetConfig()
	{
		return this.m_scrollingListConfig;
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x000E48F4 File Offset: 0x000E2CF4
	protected virtual void OnGUI()
	{
		if (this.m_worldSpace)
		{
			this.m_scrollingListWidget.Draw(new Vector3?(base.transform.position));
		}
		else
		{
			this.m_scrollingListWidget.Draw(null);
		}
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x000E4940 File Offset: 0x000E2D40
	protected virtual void Awake()
	{
		this.m_scrollingListWidget = new ScrollingListWidget(this.m_scrollingListConfig);
	}

	// Token: 0x0400270D RID: 9997
	[SerializeField]
	private ScrollingListWidgetConfig m_scrollingListConfig;

	// Token: 0x0400270E RID: 9998
	[SerializeField]
	private bool m_worldSpace;

	// Token: 0x0400270F RID: 9999
	private ScrollingListWidget m_scrollingListWidget;

	// Token: 0x04002710 RID: 10000
	private Vector2 m_position;
}
