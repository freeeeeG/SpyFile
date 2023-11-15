using System;
using UnityEngine;

// Token: 0x02000BCB RID: 3019
public class QuickLayout : KMonoBehaviour
{
	// Token: 0x06005ED4 RID: 24276 RVA: 0x0022CFA0 File Offset: 0x0022B1A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ForceUpdate();
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x0022CFAE File Offset: 0x0022B1AE
	private void OnEnable()
	{
		this.ForceUpdate();
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x0022CFB6 File Offset: 0x0022B1B6
	private void LateUpdate()
	{
		this.Run(false);
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x0022CFBF File Offset: 0x0022B1BF
	public void ForceUpdate()
	{
		this.Run(true);
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x0022CFC8 File Offset: 0x0022B1C8
	private void Run(bool forceUpdate = false)
	{
		forceUpdate = (forceUpdate || this._elementSize != this.elementSize);
		forceUpdate = (forceUpdate || this._spacing != this.spacing);
		forceUpdate = (forceUpdate || this._layoutDirection != this.layoutDirection);
		forceUpdate = (forceUpdate || this._offset != this.offset);
		if (forceUpdate)
		{
			this._elementSize = this.elementSize;
			this._spacing = this.spacing;
			this._layoutDirection = this.layoutDirection;
			this._offset = this.offset;
		}
		int num = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		if (num != this.oldActiveChildCount || forceUpdate)
		{
			this.Layout();
			this.oldActiveChildCount = num;
		}
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x0022D0C0 File Offset: 0x0022B2C0
	public void Layout()
	{
		Vector3 vector = this._offset;
		bool flag = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				flag = true;
				base.transform.GetChild(i).rectTransform().anchoredPosition = vector;
				vector += (float)(this._elementSize + this._spacing) * this.GetDirectionVector();
			}
		}
		if (this.driveParentRectSize != null)
		{
			if (!flag)
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(this.driveParentRectSize.sizeDelta.x), 0f);
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(0f, Mathf.Abs(this.driveParentRectSize.sizeDelta.y));
					return;
				}
			}
			else
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(this.driveParentRectSize.sizeDelta.x, Mathf.Abs(vector.y));
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(vector.x), this.driveParentRectSize.sizeDelta.y);
				}
			}
		}
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x0022D258 File Offset: 0x0022B458
	private Vector2 GetDirectionVector()
	{
		Vector2 result = Vector3.zero;
		switch (this._layoutDirection)
		{
		case QuickLayout.LayoutDirection.TopToBottom:
			result = Vector2.down;
			break;
		case QuickLayout.LayoutDirection.BottomToTop:
			result = Vector2.up;
			break;
		case QuickLayout.LayoutDirection.LeftToRight:
			result = Vector2.right;
			break;
		case QuickLayout.LayoutDirection.RightToLeft:
			result = Vector2.left;
			break;
		}
		return result;
	}

	// Token: 0x04004006 RID: 16390
	[Header("Configuration")]
	[SerializeField]
	private int elementSize;

	// Token: 0x04004007 RID: 16391
	[SerializeField]
	private int spacing;

	// Token: 0x04004008 RID: 16392
	[SerializeField]
	private QuickLayout.LayoutDirection layoutDirection;

	// Token: 0x04004009 RID: 16393
	[SerializeField]
	private Vector2 offset;

	// Token: 0x0400400A RID: 16394
	[SerializeField]
	private RectTransform driveParentRectSize;

	// Token: 0x0400400B RID: 16395
	private int _elementSize;

	// Token: 0x0400400C RID: 16396
	private int _spacing;

	// Token: 0x0400400D RID: 16397
	private QuickLayout.LayoutDirection _layoutDirection;

	// Token: 0x0400400E RID: 16398
	private Vector2 _offset;

	// Token: 0x0400400F RID: 16399
	private int oldActiveChildCount;

	// Token: 0x02001B0D RID: 6925
	private enum LayoutDirection
	{
		// Token: 0x04007B90 RID: 31632
		TopToBottom,
		// Token: 0x04007B91 RID: 31633
		BottomToTop,
		// Token: 0x04007B92 RID: 31634
		LeftToRight,
		// Token: 0x04007B93 RID: 31635
		RightToLeft
	}
}
