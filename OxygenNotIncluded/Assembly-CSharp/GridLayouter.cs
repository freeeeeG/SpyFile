using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B13 RID: 2835
public class GridLayouter
{
	// Token: 0x0600576B RID: 22379 RVA: 0x002001C8 File Offset: 0x001FE3C8
	[Conditional("UNITY_EDITOR")]
	private void ValidateImportantFieldsAreSet()
	{
		global::Debug.Assert(this.minCellSize >= 0f, string.Format("[{0} Error] Minimum cell size is invalid. Given: {1}", "GridLayouter", this.minCellSize));
		global::Debug.Assert(this.maxCellSize >= 0f, string.Format("[{0} Error] Maximum cell size is invalid. Given: {1}", "GridLayouter", this.maxCellSize));
		global::Debug.Assert(this.targetGridLayouts != null, string.Format("[{0} Error] Target grid layout is invalid. Given: {1}", "GridLayouter", this.targetGridLayouts));
	}

	// Token: 0x0600576C RID: 22380 RVA: 0x00200258 File Offset: 0x001FE458
	public void CheckIfShouldResizeGrid()
	{
		Vector2 lhs = new Vector2((float)Screen.width, (float)Screen.height);
		if (lhs != this.oldScreenSize)
		{
			this.RequestGridResize();
		}
		this.oldScreenSize = lhs;
		float @float = KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey);
		if (@float != this.oldScreenScale)
		{
			this.RequestGridResize();
		}
		this.oldScreenScale = @float;
		this.ResizeGridIfRequested();
	}

	// Token: 0x0600576D RID: 22381 RVA: 0x002002BA File Offset: 0x001FE4BA
	public void RequestGridResize()
	{
		this.framesLeftToResizeGrid = 3;
	}

	// Token: 0x0600576E RID: 22382 RVA: 0x002002C3 File Offset: 0x001FE4C3
	private void ResizeGridIfRequested()
	{
		if (this.framesLeftToResizeGrid > 0)
		{
			this.ImmediateSizeGridToScreenResolution();
			this.framesLeftToResizeGrid--;
			if (this.framesLeftToResizeGrid == 0 && this.OnSizeGridComplete != null)
			{
				this.OnSizeGridComplete();
			}
		}
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x00200300 File Offset: 0x001FE500
	public void ImmediateSizeGridToScreenResolution()
	{
		foreach (GridLayoutGroup gridLayoutGroup in this.targetGridLayouts)
		{
			float workingWidth = ((this.overrideParentForSizeReference != null) ? this.overrideParentForSizeReference.rect.size.x : gridLayoutGroup.transform.parent.rectTransform().rect.size.x) - (float)gridLayoutGroup.padding.left - (float)gridLayoutGroup.padding.right;
			float x = gridLayoutGroup.spacing.x;
			int num = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(this.maxCellSize, x, workingWidth) + 1;
			float num2;
			for (num2 = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num); num2 < this.minCellSize; num2 = Mathf.Min(this.maxCellSize, GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num)))
			{
				num--;
				if (num <= 0)
				{
					num = 1;
					num2 = this.minCellSize;
					break;
				}
			}
			gridLayoutGroup.childAlignment = ((num == 1) ? TextAnchor.UpperCenter : TextAnchor.UpperLeft);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = num;
			gridLayoutGroup.cellSize = Vector2.one * num2;
		}
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x00200476 File Offset: 0x001FE676
	[CompilerGenerated]
	internal static float <ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(float workingWidth, float spacingSize, int count)
	{
		return (workingWidth - (spacingSize * (float)count - 1f)) / (float)count;
	}

	// Token: 0x06005772 RID: 22386 RVA: 0x00200488 File Offset: 0x001FE688
	[CompilerGenerated]
	internal static int <ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(float cellSize, float spacingSize, float workingWidth)
	{
		int num = 0;
		for (float num2 = cellSize; num2 < workingWidth; num2 += cellSize + spacingSize)
		{
			num++;
		}
		return num;
	}

	// Token: 0x04003B0C RID: 15116
	public float minCellSize = -1f;

	// Token: 0x04003B0D RID: 15117
	public float maxCellSize = -1f;

	// Token: 0x04003B0E RID: 15118
	public List<GridLayoutGroup> targetGridLayouts;

	// Token: 0x04003B0F RID: 15119
	public RectTransform overrideParentForSizeReference;

	// Token: 0x04003B10 RID: 15120
	public System.Action OnSizeGridComplete;

	// Token: 0x04003B11 RID: 15121
	private Vector2 oldScreenSize;

	// Token: 0x04003B12 RID: 15122
	private float oldScreenScale;

	// Token: 0x04003B13 RID: 15123
	private int framesLeftToResizeGrid;
}
