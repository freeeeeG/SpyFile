using System;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class ThanksPanel : IUserInterface
{
	// Token: 0x06000F83 RID: 3971 RVA: 0x000296FC File Offset: 0x000278FC
	public override void Show()
	{
		base.Show();
		this.isShowing = true;
		this.txtPanel.anchoredPosition = this.startPos;
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0002971C File Offset: 0x0002791C
	public override void Hide()
	{
		base.Hide();
		this.isShowing = false;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x0002972B File Offset: 0x0002792B
	public override void Update()
	{
		base.Update();
		this.RollPanel();
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0002973C File Offset: 0x0002793C
	private void RollPanel()
	{
		if (this.isShowing)
		{
			this.txtPanel.anchoredPosition = Vector2.MoveTowards(this.txtPanel.anchoredPosition, this.endPos, 100f * Time.deltaTime);
			if (Vector2.SqrMagnitude(this.txtPanel.anchoredPosition - this.endPos) < 0.1f)
			{
				this.txtPanel.anchoredPosition = this.startPos;
			}
		}
	}

	// Token: 0x040007EA RID: 2026
	[SerializeField]
	private RectTransform txtPanel;

	// Token: 0x040007EB RID: 2027
	[SerializeField]
	private Vector2 startPos;

	// Token: 0x040007EC RID: 2028
	[SerializeField]
	private Vector2 endPos;

	// Token: 0x040007ED RID: 2029
	private bool isShowing;
}
