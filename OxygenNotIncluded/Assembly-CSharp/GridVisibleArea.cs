using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007ED RID: 2029
public class GridVisibleArea
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x060039B4 RID: 14772 RVA: 0x00141F44 File Offset: 0x00140144
	public GridArea CurrentArea
	{
		get
		{
			return this.Areas[0];
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x060039B5 RID: 14773 RVA: 0x00141F52 File Offset: 0x00140152
	public GridArea PreviousArea
	{
		get
		{
			return this.Areas[1];
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x060039B6 RID: 14774 RVA: 0x00141F60 File Offset: 0x00140160
	public GridArea PreviousPreviousArea
	{
		get
		{
			return this.Areas[2];
		}
	}

	// Token: 0x060039B7 RID: 14775 RVA: 0x00141F70 File Offset: 0x00140170
	public void Update()
	{
		this.Areas[2] = this.Areas[1];
		this.Areas[1] = this.Areas[0];
		this.Areas[0] = GridVisibleArea.GetVisibleArea();
		foreach (GridVisibleArea.Callback callback in this.Callbacks)
		{
			callback.OnUpdate();
		}
	}

	// Token: 0x060039B8 RID: 14776 RVA: 0x00142008 File Offset: 0x00140208
	public void AddCallback(string name, System.Action on_update)
	{
		GridVisibleArea.Callback item = new GridVisibleArea.Callback
		{
			Name = name,
			OnUpdate = on_update
		};
		this.Callbacks.Add(item);
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x0014203C File Offset: 0x0014023C
	public void Run(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentArea.Run(in_view);
		}
	}

	// Token: 0x060039BA RID: 14778 RVA: 0x0014205C File Offset: 0x0014025C
	public void Run(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
		}
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x001420B4 File Offset: 0x001402B4
	public void RunIfVisible(int cell, Action<int> action)
	{
		this.CurrentArea.RunIfInside(cell, action);
	}

	// Token: 0x060039BC RID: 14780 RVA: 0x001420D4 File Offset: 0x001402D4
	public static GridArea GetVisibleArea()
	{
		GridArea result = default(GridArea);
		Camera mainCamera = Game.MainCamera;
		if (mainCamera != null)
		{
			Vector3 vector = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.transform.GetPosition().z));
			Vector3 vector2 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.transform.GetPosition().z));
			if (CameraController.Instance != null)
			{
				Vector2I vector2I;
				Vector2I vector2I2;
				CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), vector2I.x), Math.Max((int)(vector2.y - 0.5f), vector2I.y), Math.Min((int)(vector.x + 1.5f), vector2I2.x + vector2I.x), Math.Min((int)(vector.y + 1.5f), vector2I2.y + vector2I.y));
			}
			else
			{
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), 0), Math.Max((int)(vector2.y - 0.5f), 0), Math.Min((int)(vector.x + 1.5f), Grid.WidthInCells), Math.Min((int)(vector.y + 1.5f), Grid.HeightInCells));
			}
		}
		return result;
	}

	// Token: 0x04002677 RID: 9847
	private GridArea[] Areas = new GridArea[3];

	// Token: 0x04002678 RID: 9848
	private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();

	// Token: 0x020015CC RID: 5580
	public struct Callback
	{
		// Token: 0x0400698A RID: 27018
		public System.Action OnUpdate;

		// Token: 0x0400698B RID: 27019
		public string Name;
	}
}
