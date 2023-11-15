using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B5C RID: 2908
[RequireComponent(typeof(GridLayoutGroup))]
public class GridElementSnap : MonoBehaviour
{
	// Token: 0x06003B0F RID: 15119 RVA: 0x00119664 File Offset: 0x00117A64
	private void Awake()
	{
		this.m_GridLayout = base.GetComponent<GridLayoutGroup>();
		this.m_RectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x06003B10 RID: 15120 RVA: 0x00119680 File Offset: 0x00117A80
	private void Update()
	{
		if (this.m_bButtonScrolling)
		{
			if (this.m_bButtonScrollThisFrame)
			{
				Vector3 zero = Vector3.zero;
				if (this.m_bScrollingLeft)
				{
					zero = new Vector3(-this.GetCellWidth(), 0f, 0f);
				}
				else if (this.m_bScrollingRight)
				{
					zero = new Vector3(this.GetCellWidth(), 0f, 0f);
				}
				Vector3 b = Vector3.Lerp(this.m_RectTransform.localPosition, this.m_RectTransform.localPosition + zero, Mathf.Min(1f, Time.deltaTime * this.m_ScrollSpeed)) - this.m_RectTransform.localPosition;
				this.m_RectTransform.localPosition += b;
				this.m_bButtonScrollThisFrame = false;
				this.m_bButtonScrollJustReleased = true;
			}
			else
			{
				if (this.m_bButtonScrollJustReleased)
				{
					this.ConsiderSnapToCurrentPosition();
					this.m_bButtonScrollJustReleased = false;
				}
				this.m_RectTransform.localPosition = Vector3.Lerp(this.m_RectTransform.localPosition, this.m_TargetSnapPosition, Mathf.Min(1f, Time.deltaTime * this.m_NoControlRestingSpeed));
				if (Vector3.Distance(this.m_RectTransform.localPosition, this.m_TargetSnapPosition) < 1f)
				{
					this.m_RectTransform.localPosition = this.m_TargetSnapPosition;
					this.m_bButtonScrolling = false;
					this.m_bScrollingLeft = false;
					this.m_bScrollingRight = false;
					this.m_OriginSnappedPosition = this.m_TargetSnapPosition;
					this.m_TargetSnapPosition = Vector3.zero;
				}
			}
			return;
		}
	}

	// Token: 0x06003B11 RID: 15121 RVA: 0x00119814 File Offset: 0x00117C14
	public void ConsiderSnapToCurrentPosition()
	{
		Vector3 vector = this.CalculateCurrentSnappedPosition();
		if (vector != this.m_OriginSnappedPosition && this.m_TargetSnapPosition != vector)
		{
			this.m_TargetSnapPosition = vector;
		}
	}

	// Token: 0x06003B12 RID: 15122 RVA: 0x00119854 File Offset: 0x00117C54
	public void SnapToNearest(ScrollRect scrollingRect)
	{
		bool flag = scrollingRect.velocity.x < 0f;
		this.m_TargetSnapPosition.x = this.CalculateCurrentSnappedPosition().x;
		this.m_bButtonScrolling = true;
		this.m_bScrollingLeft = flag;
		this.m_bScrollingRight = !flag;
		this.m_bButtonScrollThisFrame = true;
		this.m_bButtonScrollJustReleased = false;
		scrollingRect.velocity = Vector2.zero;
		scrollingRect = null;
	}

	// Token: 0x06003B13 RID: 15123 RVA: 0x001198C4 File Offset: 0x00117CC4
	public Vector3 CalculateCurrentSnappedPosition()
	{
		float cellWidth = this.GetCellWidth();
		float num = cellWidth / 2f;
		float num2 = this.m_RectTransform.localPosition.x - num;
		num2 = Mathf.Round(num2 / cellWidth) * cellWidth;
		num2 += num;
		return new Vector3(num2, 0f, 0f);
	}

	// Token: 0x06003B14 RID: 15124 RVA: 0x00119918 File Offset: 0x00117D18
	public float GetCellWidth()
	{
		return this.m_GridLayout.cellSize.x + this.m_GridLayout.spacing.x;
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x0011994C File Offset: 0x00117D4C
	public void ForceTargetPosition(Vector3 position)
	{
		this.m_OriginSnappedPosition = position;
		this.m_RectTransform.localPosition = this.m_OriginSnappedPosition;
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x00119968 File Offset: 0x00117D68
	public void SetTargetElement(float xDif)
	{
		if (!this.m_bButtonScrolling)
		{
			this.m_TargetSnapPosition.x = this.CalculateCurrentSnappedPosition().x + xDif;
			this.m_bButtonScrolling = true;
			this.m_bScrollingLeft = (xDif > 0f);
			this.m_bScrollingRight = (xDif <= 0f);
			this.m_bButtonScrollThisFrame = true;
			this.m_bButtonScrollJustReleased = false;
		}
	}

	// Token: 0x06003B17 RID: 15127 RVA: 0x001199D0 File Offset: 0x00117DD0
	public void ButtonScroll(bool scrollLeft, bool isNewInput)
	{
		bool flag = (this.m_bButtonScrolling && this.m_bScrollingLeft != scrollLeft) || isNewInput;
		if (!this.m_bButtonScrolling || flag)
		{
			this.m_OriginSnappedPosition = this.CalculateCurrentSnappedPosition();
			if (scrollLeft)
			{
				this.m_TargetSnapPosition.x = this.m_OriginSnappedPosition.x - this.GetCellWidth();
			}
			else
			{
				this.m_TargetSnapPosition.x = this.m_OriginSnappedPosition.x + this.GetCellWidth();
			}
		}
		this.m_bButtonScrolling = true;
		this.m_bScrollingLeft = scrollLeft;
		this.m_bScrollingRight = !scrollLeft;
		this.m_bButtonScrollThisFrame = true;
		this.m_bButtonScrollJustReleased = false;
	}

	// Token: 0x0400300C RID: 12300
	private GridLayoutGroup m_GridLayout;

	// Token: 0x0400300D RID: 12301
	private RectTransform m_RectTransform;

	// Token: 0x0400300E RID: 12302
	private bool m_bButtonScrolling;

	// Token: 0x0400300F RID: 12303
	private bool m_bScrollingLeft;

	// Token: 0x04003010 RID: 12304
	private bool m_bScrollingRight;

	// Token: 0x04003011 RID: 12305
	private bool m_bButtonScrollThisFrame;

	// Token: 0x04003012 RID: 12306
	private bool m_bButtonScrollJustReleased;

	// Token: 0x04003013 RID: 12307
	private Vector3 m_OriginSnappedPosition = Vector3.zero;

	// Token: 0x04003014 RID: 12308
	private Vector3 m_TargetSnapPosition = Vector3.zero;

	// Token: 0x04003015 RID: 12309
	public float m_ScrollSpeed = 4.5f;

	// Token: 0x04003016 RID: 12310
	public float m_NoControlRestingSpeed = 8f;

	// Token: 0x04003017 RID: 12311
	public float m_SnapStartVelocity = 100f;
}
