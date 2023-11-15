using System;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class RangeHolder : MonoBehaviour
{
	// Token: 0x06000CEE RID: 3310 RVA: 0x0002154B File Offset: 0x0001F74B
	private void Awake()
	{
		this.ConcreteContent = base.transform.root.GetComponentInChildren<ConcreteContent>();
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00021564 File Offset: 0x0001F764
	public void SetRange()
	{
		switch (this.ConcreteContent.Strategy.RangeType)
		{
		case RangeType.Circle:
			this.circleDetectRange.gameObject.SetActive(true);
			this.halfCircleDetectRange.gameObject.SetActive(false);
			this.lineDetectRange.gameObject.SetActive(false);
			this.circleDetectRange.GetComponent<BoxCollider2D>().size = Vector2.one * 2f * ((float)this.ConcreteContent.Strategy.FinalRange + 0.2f) * Mathf.Cos(0.7853982f);
			return;
		case RangeType.HalfCircle:
			this.circleDetectRange.gameObject.SetActive(false);
			this.halfCircleDetectRange.gameObject.SetActive(true);
			this.lineDetectRange.gameObject.SetActive(false);
			this.halfCircleDetectRange.transform.localScale = Vector2.one * ((float)this.ConcreteContent.Strategy.FinalRange + 0.2f);
			return;
		case RangeType.Line:
			this.circleDetectRange.gameObject.SetActive(false);
			this.halfCircleDetectRange.gameObject.SetActive(false);
			this.lineDetectRange.gameObject.SetActive(true);
			this.lineDetectRange.GetComponent<BoxCollider2D>().size = new Vector2(0.7f, (float)this.ConcreteContent.Strategy.FinalRange - 0.3f);
			this.lineDetectRange.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 1f + 0.5f * (float)(this.ConcreteContent.Strategy.FinalRange - 1));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0002171A File Offset: 0x0001F91A
	public void AddTarget(TargetPoint target)
	{
		this.ConcreteContent.AddTarget(target);
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00021728 File Offset: 0x0001F928
	public void RemoveTarget(TargetPoint target)
	{
		this.ConcreteContent.RemoveTarget(target);
	}

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private DetectRange circleDetectRange;

	// Token: 0x0400064A RID: 1610
	[SerializeField]
	private DetectRange halfCircleDetectRange;

	// Token: 0x0400064B RID: 1611
	[SerializeField]
	private DetectRange lineDetectRange;

	// Token: 0x0400064C RID: 1612
	private ConcreteContent ConcreteContent;
}
