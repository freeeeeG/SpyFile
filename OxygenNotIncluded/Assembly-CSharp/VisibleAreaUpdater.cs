using System;

// Token: 0x02000A26 RID: 2598
public class VisibleAreaUpdater
{
	// Token: 0x06004DD8 RID: 19928 RVA: 0x001B492A File Offset: 0x001B2B2A
	public VisibleAreaUpdater(Action<int> outside_view_first_time_cb, Action<int> inside_view_first_time_cb, string name)
	{
		this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
		this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
		this.UpdateCallback = new Action<int>(this.InternalUpdateCell);
		this.Name = name;
	}

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001B4959 File Offset: 0x001B2B59
	public void Update()
	{
		if (CameraController.Instance != null && this.VisibleArea == null)
		{
			this.VisibleArea = CameraController.Instance.VisibleArea;
			this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
		}
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001B4991 File Offset: 0x001B2B91
	private void InternalUpdateCell(int cell)
	{
		this.OutsideViewFirstTimeCallback(cell);
		this.InsideViewFirstTimeCallback(cell);
	}

	// Token: 0x06004DDB RID: 19931 RVA: 0x001B49AB File Offset: 0x001B2BAB
	public void UpdateCell(int cell)
	{
		if (this.VisibleArea != null)
		{
			this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
		}
	}

	// Token: 0x040032BF RID: 12991
	private GridVisibleArea VisibleArea;

	// Token: 0x040032C0 RID: 12992
	private Action<int> OutsideViewFirstTimeCallback;

	// Token: 0x040032C1 RID: 12993
	private Action<int> InsideViewFirstTimeCallback;

	// Token: 0x040032C2 RID: 12994
	private Action<int> UpdateCallback;

	// Token: 0x040032C3 RID: 12995
	private string Name;
}
