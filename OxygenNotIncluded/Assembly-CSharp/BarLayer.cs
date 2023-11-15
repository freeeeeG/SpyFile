using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class BarLayer : GraphLayer
{
	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06005747 RID: 22343 RVA: 0x001FE9F3 File Offset: 0x001FCBF3
	public int bar_count
	{
		get
		{
			return this.bars.Count;
		}
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x001FEA00 File Offset: 0x001FCC00
	public void NewBar(int[] values, float x_position, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
		if (ID == "")
		{
			ID = this.bars.Count.ToString();
		}
		gameObject.name = string.Format("bar_{0}", ID);
		GraphedBar component = gameObject.GetComponent<GraphedBar>();
		component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
		int[] array = new int[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = (int)(base.graph.rectTransform().rect.height * base.graph.GetRelativeSize(new Vector2(0f, (float)values[i])).y);
		}
		component.SetValues(array, base.graph.GetRelativePosition(new Vector2(x_position, 0f)).x);
		this.bars.Add(component);
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x001FEB00 File Offset: 0x001FCD00
	public void ClearBars()
	{
		foreach (GraphedBar graphedBar in this.bars)
		{
			if (graphedBar != null && graphedBar.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedBar.gameObject);
			}
		}
		this.bars.Clear();
	}

	// Token: 0x04003AED RID: 15085
	public GameObject bar_container;

	// Token: 0x04003AEE RID: 15086
	public GameObject prefab_bar;

	// Token: 0x04003AEF RID: 15087
	public GraphedBarFormatting[] bar_formats;

	// Token: 0x04003AF0 RID: 15088
	private List<GraphedBar> bars = new List<GraphedBar>();
}
