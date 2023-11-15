using System;

namespace UnityEngine.UI
{
	// Token: 0x02000B6C RID: 2924
	[AddComponentMenu("T17_UI/Layout/Grid Layout Group", 152)]
	public class T17GridLayoutGroup : LayoutGroup
	{
		// Token: 0x06003B75 RID: 15221 RVA: 0x0011AD34 File Offset: 0x00119134
		protected T17GridLayoutGroup()
		{
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06003B76 RID: 15222 RVA: 0x0011AD8E File Offset: 0x0011918E
		// (set) Token: 0x06003B77 RID: 15223 RVA: 0x0011AD96 File Offset: 0x00119196
		public RectOffset ourPadding
		{
			get
			{
				return this.m_OurPadding;
			}
			set
			{
				base.SetProperty<RectOffset>(ref this.m_OurPadding, value);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06003B78 RID: 15224 RVA: 0x0011ADA5 File Offset: 0x001191A5
		// (set) Token: 0x06003B79 RID: 15225 RVA: 0x0011ADAD File Offset: 0x001191AD
		public Vector2 scalingFactor
		{
			get
			{
				return this.m_ScalingFactor;
			}
			set
			{
				base.SetProperty<Vector2>(ref this.m_ScalingFactor, value);
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06003B7A RID: 15226 RVA: 0x0011ADBC File Offset: 0x001191BC
		// (set) Token: 0x06003B7B RID: 15227 RVA: 0x0011ADC4 File Offset: 0x001191C4
		public bool scaleAspect
		{
			get
			{
				return this.m_ScaleAspect;
			}
			set
			{
				base.SetProperty<bool>(ref this.m_ScaleAspect, value);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06003B7C RID: 15228 RVA: 0x0011ADD3 File Offset: 0x001191D3
		// (set) Token: 0x06003B7D RID: 15229 RVA: 0x0011ADDB File Offset: 0x001191DB
		public Vector2 cellSize
		{
			get
			{
				return this.m_CellSize;
			}
			set
			{
				base.SetProperty<Vector2>(ref this.m_CellSize, value);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06003B7E RID: 15230 RVA: 0x0011ADEA File Offset: 0x001191EA
		// (set) Token: 0x06003B7F RID: 15231 RVA: 0x0011ADF2 File Offset: 0x001191F2
		public Vector2 spacing
		{
			get
			{
				return this.m_Spacing;
			}
			set
			{
				base.SetProperty<Vector2>(ref this.m_Spacing, value);
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06003B80 RID: 15232 RVA: 0x0011AE01 File Offset: 0x00119201
		// (set) Token: 0x06003B81 RID: 15233 RVA: 0x0011AE09 File Offset: 0x00119209
		public T17GridLayoutGroup.Corner startCorner
		{
			get
			{
				return this.m_StartCorner;
			}
			set
			{
				base.SetProperty<T17GridLayoutGroup.Corner>(ref this.m_StartCorner, value);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06003B82 RID: 15234 RVA: 0x0011AE18 File Offset: 0x00119218
		// (set) Token: 0x06003B83 RID: 15235 RVA: 0x0011AE20 File Offset: 0x00119220
		public T17GridLayoutGroup.Axis startAxis
		{
			get
			{
				return this.m_StartAxis;
			}
			set
			{
				base.SetProperty<T17GridLayoutGroup.Axis>(ref this.m_StartAxis, value);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x0011AE2F File Offset: 0x0011922F
		// (set) Token: 0x06003B85 RID: 15237 RVA: 0x0011AE37 File Offset: 0x00119237
		public T17GridLayoutGroup.Constraint constraint
		{
			get
			{
				return this.m_Constraint;
			}
			set
			{
				base.SetProperty<T17GridLayoutGroup.Constraint>(ref this.m_Constraint, value);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x0011AE46 File Offset: 0x00119246
		// (set) Token: 0x06003B87 RID: 15239 RVA: 0x0011AE4E File Offset: 0x0011924E
		public int constraintCount
		{
			get
			{
				return this.m_ConstraintCount;
			}
			set
			{
				base.SetProperty<int>(ref this.m_ConstraintCount, Mathf.Max(1, value));
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x0011AE63 File Offset: 0x00119263
		// (set) Token: 0x06003B89 RID: 15241 RVA: 0x0011AE6B File Offset: 0x0011926B
		public bool resizeToFitChildren
		{
			get
			{
				return this.m_ResizeToFitChildren;
			}
			set
			{
				base.SetProperty<bool>(ref this.m_ResizeToFitChildren, value);
			}
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0011AE7A File Offset: 0x0011927A
		public void ForceRefresh()
		{
			base.SetDirty();
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x0011AE84 File Offset: 0x00119284
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			int num2;
			int num;
			if (this.m_Constraint == T17GridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = (num2 = this.m_ConstraintCount);
			}
			else if (this.m_Constraint == T17GridLayoutGroup.Constraint.FixedRowCount)
			{
				num = (num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f));
			}
			else
			{
				num2 = 1;
				num = Mathf.CeilToInt(Mathf.Sqrt((float)base.rectChildren.Count));
			}
			base.SetLayoutInputForAxis((float)base.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float)num2 - this.spacing.x, (float)base.padding.horizontal + (this.cellSize.x + this.spacing.x) * (float)num - this.spacing.x, -1f, 0);
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0011AF8C File Offset: 0x0011938C
		public override void CalculateLayoutInputVertical()
		{
			int num;
			if (this.m_Constraint == T17GridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f);
			}
			else if (this.m_Constraint == T17GridLayoutGroup.Constraint.FixedRowCount)
			{
				num = this.m_ConstraintCount;
			}
			else
			{
				float x = base.rectTransform.rect.size.x;
				int num2 = Mathf.Max(1, Mathf.FloorToInt((x - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num2);
			}
			float num3 = (float)base.padding.vertical + (this.cellSize.y + this.spacing.y) * (float)num - this.spacing.y;
			base.SetLayoutInputForAxis(num3, num3, -1f, 1);
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0011B0B8 File Offset: 0x001194B8
		public override void SetLayoutHorizontal()
		{
			this.SetCellsAlongAxis(0);
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x0011B0C1 File Offset: 0x001194C1
		public override void SetLayoutVertical()
		{
			this.SetCellsAlongAxis(1);
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x0011B0CC File Offset: 0x001194CC
		private void SetCellsAlongAxis(int axis)
		{
			if (axis == 0)
			{
				for (int i = 0; i < base.rectChildren.Count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
					rectTransform.anchorMin = Vector2.up;
					rectTransform.anchorMax = Vector2.up;
					rectTransform.sizeDelta = ((this.m_Constraint != T17GridLayoutGroup.Constraint.Flexible) ? this.m_NewCellSize : this.cellSize);
				}
				return;
			}
			float x = base.rectTransform.rect.size.x;
			float y = base.rectTransform.rect.size.y;
			int num;
			int num2;
			if (this.m_Constraint == T17GridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = this.m_ConstraintCount;
				num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num - 0.001f);
			}
			else if (this.m_Constraint == T17GridLayoutGroup.Constraint.FixedRowCount)
			{
				num2 = this.m_ConstraintCount;
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num2 - 0.001f);
			}
			else
			{
				if (this.cellSize.x + this.spacing.x <= 0f)
				{
					num = int.MaxValue;
				}
				else
				{
					num = Mathf.Max(1, Mathf.FloorToInt((x - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
				}
				if (this.cellSize.y + this.spacing.y <= 0f)
				{
					num2 = int.MaxValue;
				}
				else
				{
					num2 = Mathf.Max(1, Mathf.FloorToInt((y - (float)base.padding.vertical + this.spacing.y + 0.001f) / (this.cellSize.y + this.spacing.y)));
				}
			}
			int num3 = (int)(this.startCorner % T17GridLayoutGroup.Corner.LowerLeft);
			int num4 = (int)(this.startCorner / T17GridLayoutGroup.Corner.LowerLeft);
			int num5;
			int num6;
			int num7;
			if (this.startAxis == T17GridLayoutGroup.Axis.Horizontal)
			{
				num5 = num;
				num6 = Mathf.Clamp(num, 1, base.rectChildren.Count);
				num7 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num5));
			}
			else
			{
				num5 = num2;
				num7 = Mathf.Clamp(num2, 1, base.rectChildren.Count);
				num6 = Mathf.Clamp(num, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num5));
			}
			this.m_CellCountX = num6;
			this.m_CellCountY = num7;
			Vector2 vector = new Vector2((float)num6 * this.cellSize.x + (float)(num6 - 1) * this.spacing.x, (float)num7 * this.cellSize.y + (float)(num7 - 1) * this.spacing.y);
			if (this.m_Constraint != T17GridLayoutGroup.Constraint.Flexible)
			{
				float num8 = Mathf.Clamp((x - (float)this.m_OurPadding.horizontal) / vector.x, this.m_ScalingFactor.x, this.m_ScalingFactor.y);
				float num9 = Mathf.Clamp((y - (float)this.m_OurPadding.vertical) / vector.y, this.m_ScalingFactor.x, this.m_ScalingFactor.y);
				if (this.m_ScaleAspect)
				{
					float num10;
					if (this.constraint == T17GridLayoutGroup.Constraint.FixedColumnCount)
					{
						num10 = Mathf.Max(new float[]
						{
							num8
						});
					}
					else if (this.constraint == T17GridLayoutGroup.Constraint.FixedRowCount)
					{
						num10 = Mathf.Max(new float[]
						{
							num9
						});
					}
					else
					{
						num10 = Mathf.Max(num8, num9);
					}
					this.m_CurrentSizeFactor = new Vector2(num10, num10);
				}
				else
				{
					this.m_CurrentSizeFactor = new Vector2(num8, num9);
				}
				this.m_NewCellSize.x = this.m_CurrentSizeFactor.x * this.cellSize.x;
				this.m_NewCellSize.y = this.m_CurrentSizeFactor.y * this.cellSize.y;
				this.m_NewSpacing.x = this.m_CurrentSizeFactor.x * this.spacing.x;
				this.m_NewSpacing.y = this.m_CurrentSizeFactor.y * this.spacing.y;
				base.padding.left = Mathf.FloorToInt((float)this.m_OurPadding.left * this.m_CurrentSizeFactor.x);
				base.padding.right = Mathf.FloorToInt((float)this.m_OurPadding.right * this.m_CurrentSizeFactor.x);
				base.padding.top = Mathf.FloorToInt((float)this.m_OurPadding.top * this.m_CurrentSizeFactor.y);
				base.padding.bottom = Mathf.FloorToInt((float)this.m_OurPadding.bottom * this.m_CurrentSizeFactor.y);
				vector = new Vector2((float)num6 * this.m_NewCellSize.x + (float)(num6 - 1) * this.m_NewSpacing.x, (float)num7 * this.m_NewCellSize.y + (float)(num7 - 1) * this.m_NewSpacing.y);
			}
			Vector2 vector2 = new Vector2(base.GetStartOffset(0, vector.x), base.GetStartOffset(1, vector.y));
			Vector2 vector3 = (this.m_Constraint != T17GridLayoutGroup.Constraint.Flexible) ? this.m_NewCellSize : this.cellSize;
			Vector2 vector4 = (this.m_Constraint != T17GridLayoutGroup.Constraint.Flexible) ? this.m_NewSpacing : this.spacing;
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				int num11;
				int num12;
				if (this.startAxis == T17GridLayoutGroup.Axis.Horizontal)
				{
					num11 = j % num5;
					num12 = j / num5;
				}
				else
				{
					num11 = j / num5;
					num12 = j % num5;
				}
				if (num3 == 1)
				{
					num11 = num6 - 1 - num11;
				}
				if (num4 == 1)
				{
					num12 = num7 - 1 - num12;
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, vector2.x + (vector3[0] + vector4[0]) * (float)num11, vector3[0]);
				base.SetChildAlongAxis(base.rectChildren[j], 1, vector2.y + (vector3[1] + vector4[1]) * (float)num12, vector3[1]);
			}
			if (this.m_ResizeToFitChildren)
			{
				if (this.constraint == T17GridLayoutGroup.Constraint.FixedColumnCount)
				{
					this.m_Tracker.Add(this, base.rectTransform, DrivenTransformProperties.SizeDeltaY);
					base.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector.y + (float)base.padding.top + (float)base.padding.bottom);
				}
				else if (this.constraint == T17GridLayoutGroup.Constraint.FixedRowCount)
				{
					this.m_Tracker.Add(this, base.rectTransform, DrivenTransformProperties.SizeDeltaX);
					base.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector.x + (float)base.padding.left + (float)base.padding.right);
				}
			}
		}

		// Token: 0x04003052 RID: 12370
		[SerializeField]
		protected RectOffset m_OurPadding;

		// Token: 0x04003053 RID: 12371
		[SerializeField]
		public Vector2 m_ScalingFactor = new Vector2(1f, 1f);

		// Token: 0x04003054 RID: 12372
		[SerializeField]
		public bool m_ScaleAspect;

		// Token: 0x04003055 RID: 12373
		[SerializeField]
		protected Vector2 m_CellSize = new Vector2(100f, 100f);

		// Token: 0x04003056 RID: 12374
		[SerializeField]
		protected Vector2 m_Spacing = Vector2.zero;

		// Token: 0x04003057 RID: 12375
		[SerializeField]
		protected T17GridLayoutGroup.Corner m_StartCorner;

		// Token: 0x04003058 RID: 12376
		[SerializeField]
		protected T17GridLayoutGroup.Axis m_StartAxis;

		// Token: 0x04003059 RID: 12377
		[SerializeField]
		protected T17GridLayoutGroup.Constraint m_Constraint;

		// Token: 0x0400305A RID: 12378
		[SerializeField]
		protected int m_ConstraintCount = 2;

		// Token: 0x0400305B RID: 12379
		[SerializeField]
		public Vector2 m_CurrentSizeFactor = Vector2.one;

		// Token: 0x0400305C RID: 12380
		[SerializeField]
		protected bool m_ResizeToFitChildren;

		// Token: 0x0400305D RID: 12381
		[SerializeField]
		public int m_CellCountX;

		// Token: 0x0400305E RID: 12382
		[SerializeField]
		public int m_CellCountY;

		// Token: 0x0400305F RID: 12383
		private Vector2 m_NewCellSize;

		// Token: 0x04003060 RID: 12384
		private Vector2 m_NewSpacing;

		// Token: 0x02000B6D RID: 2925
		public enum Corner
		{
			// Token: 0x04003062 RID: 12386
			UpperLeft,
			// Token: 0x04003063 RID: 12387
			UpperRight,
			// Token: 0x04003064 RID: 12388
			LowerLeft,
			// Token: 0x04003065 RID: 12389
			LowerRight
		}

		// Token: 0x02000B6E RID: 2926
		public enum Axis
		{
			// Token: 0x04003067 RID: 12391
			Horizontal,
			// Token: 0x04003068 RID: 12392
			Vertical
		}

		// Token: 0x02000B6F RID: 2927
		public enum Constraint
		{
			// Token: 0x0400306A RID: 12394
			Flexible,
			// Token: 0x0400306B RID: 12395
			FixedColumnCount,
			// Token: 0x0400306C RID: 12396
			FixedRowCount
		}
	}
}
