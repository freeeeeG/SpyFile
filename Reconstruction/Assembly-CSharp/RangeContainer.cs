using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class RangeContainer : MonoBehaviour
{
	// Token: 0x06000986 RID: 2438 RVA: 0x00019288 File Offset: 0x00017488
	public void Initialize()
	{
		for (int i = 0; i < 2000; i++)
		{
			RangeIndicator rangeIndicator = Object.Instantiate<RangeIndicator>(Singleton<StaticData>.Instance.RangeIndicatorPrefab, base.transform);
			rangeIndicator.ShowSprite(false);
			this.rangeIndicators.Add(rangeIndicator);
		}
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x000192D0 File Offset: 0x000174D0
	public void SetRange(ConcreteContent concrete, bool value)
	{
		if (value)
		{
			RangeContainer.ShowingConcrete = concrete;
			switch (concrete.Strategy.RangeType)
			{
			case RangeType.Circle:
				this.m_Points = StaticData.GetCirclePoints(concrete.Strategy.FinalRange, 0);
				break;
			case RangeType.HalfCircle:
				this.m_Points = StaticData.GetHalfCirclePoints(concrete.Strategy.FinalRange, 0);
				break;
			case RangeType.Line:
				this.m_Points = StaticData.GetLinePoints(concrete.Strategy.FinalRange, 0);
				break;
			}
			base.transform.SetParent(concrete.transform);
			base.transform.position = concrete.transform.position;
			base.transform.rotation = concrete.m_GameTile.transform.rotation;
		}
		else
		{
			RangeContainer.ShowingConcrete = null;
		}
		this.ShowRange(value);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x000193A4 File Offset: 0x000175A4
	private void ShowRange(bool show)
	{
		this.ShowingRange = show;
		if (show)
		{
			for (int i = 0; i < this.rangeIndicators.Count; i++)
			{
				if (i < this.m_Points.Count)
				{
					this.rangeIndicators[i].transform.localPosition = (Vector3Int)this.m_Points[i];
					this.rangeIndicators[i].ShowSprite(true);
				}
				else
				{
					this.rangeIndicators[i].ShowSprite(false);
				}
			}
			return;
		}
		this.HideRanges();
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00019438 File Offset: 0x00017638
	private void HideRanges()
	{
		foreach (RangeIndicator rangeIndicator in this.rangeIndicators)
		{
			rangeIndicator.ShowSprite(false);
		}
	}

	// Token: 0x040004DE RID: 1246
	private bool ShowingRange;

	// Token: 0x040004DF RID: 1247
	private List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();

	// Token: 0x040004E0 RID: 1248
	private List<Vector2Int> m_Points;

	// Token: 0x040004E1 RID: 1249
	public static ConcreteContent ShowingConcrete;
}
