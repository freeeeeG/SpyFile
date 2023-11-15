using System;

namespace UnityEngine.UI
{
	// Token: 0x0200035F RID: 863
	[AddComponentMenu("AnchorGridLayoutGroup")]
	public class AnchorGridLayoutGroup : LayoutGroup
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x0005F159 File Offset: 0x0005D559
		protected AnchorGridLayoutGroup()
		{
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0005F188 File Offset: 0x0005D588
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x0005F190 File Offset: 0x0005D590
		public AnchorGridLayoutGroup.Corner startCorner
		{
			get
			{
				return this.m_StartCorner;
			}
			set
			{
				base.SetProperty<AnchorGridLayoutGroup.Corner>(ref this.m_StartCorner, value);
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0005F19F File Offset: 0x0005D59F
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x0005F1A7 File Offset: 0x0005D5A7
		public AnchorGridLayoutGroup.Axis startAxis
		{
			get
			{
				return this.m_StartAxis;
			}
			set
			{
				base.SetProperty<AnchorGridLayoutGroup.Axis>(ref this.m_StartAxis, value);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x0005F1B6 File Offset: 0x0005D5B6
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x0005F1BE File Offset: 0x0005D5BE
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0005F1CD File Offset: 0x0005D5CD
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x0005F1D5 File Offset: 0x0005D5D5
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

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0005F1E4 File Offset: 0x0005D5E4
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x0005F1EC File Offset: 0x0005D5EC
		public AnchorGridLayoutGroup.Constraint constraint
		{
			get
			{
				return this.m_Constraint;
			}
			set
			{
				base.SetProperty<AnchorGridLayoutGroup.Constraint>(ref this.m_Constraint, value);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0005F1FB File Offset: 0x0005D5FB
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x0005F203 File Offset: 0x0005D603
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

		// Token: 0x0600108B RID: 4235 RVA: 0x0005F218 File Offset: 0x0005D618
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			int num2;
			int num;
			if (this.m_Constraint == AnchorGridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = (num2 = this.m_ConstraintCount);
			}
			else if (this.m_Constraint == AnchorGridLayoutGroup.Constraint.FixedRowCount)
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

		// Token: 0x0600108C RID: 4236 RVA: 0x0005F320 File Offset: 0x0005D720
		public override void CalculateLayoutInputVertical()
		{
			int num;
			if (this.m_Constraint == AnchorGridLayoutGroup.Constraint.FixedColumnCount)
			{
				num = Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.m_ConstraintCount - 0.001f);
			}
			else if (this.m_Constraint == AnchorGridLayoutGroup.Constraint.FixedRowCount)
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

		// Token: 0x0600108D RID: 4237 RVA: 0x0005F44C File Offset: 0x0005D84C
		public override void SetLayoutHorizontal()
		{
			this.SetCellsAlongAxis(0);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0005F455 File Offset: 0x0005D855
		public override void SetLayoutVertical()
		{
			this.SetCellsAlongAxis(1);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0005F460 File Offset: 0x0005D860
		private void SetCellsAlongAxis(int axis)
		{
			if (axis == 0)
			{
				for (int i = 0; i < base.rectChildren.Count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = this.cellSize;
					rectTransform.sizeDelta = Vector2.zero;
				}
				return;
			}
			float num = 1f;
			float num2 = 1f;
			float x = base.rectTransform.rect.size.x;
			float y = base.rectTransform.rect.size.y;
			int num3;
			int num4;
			if (this.m_Constraint == AnchorGridLayoutGroup.Constraint.FixedColumnCount)
			{
				num3 = this.m_ConstraintCount;
				num4 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num3 - 0.001f);
			}
			else if (this.m_Constraint == AnchorGridLayoutGroup.Constraint.FixedRowCount)
			{
				num4 = this.m_ConstraintCount;
				num3 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num4 - 0.001f);
			}
			else
			{
				if (this.cellSize.x + this.spacing.x <= 0f)
				{
					num3 = int.MaxValue;
				}
				else
				{
					num3 = Mathf.Max(1, Mathf.FloorToInt((num - (float)base.padding.horizontal + this.spacing.x + 0.001f) / (this.cellSize.x + this.spacing.x)));
				}
				if (this.cellSize.y + this.spacing.y <= 0f)
				{
					num4 = int.MaxValue;
				}
				else
				{
					num4 = Mathf.Max(1, Mathf.FloorToInt((num2 - (float)base.padding.vertical + this.spacing.y + 0.001f) / (this.cellSize.y + this.spacing.y)));
				}
			}
			int num5 = (int)(this.startCorner % AnchorGridLayoutGroup.Corner.LowerLeft);
			int num6 = (int)(this.startCorner / AnchorGridLayoutGroup.Corner.LowerLeft);
			int num7;
			int num8;
			int num9;
			if (this.startAxis == AnchorGridLayoutGroup.Axis.Horizontal)
			{
				num7 = num3;
				num8 = Mathf.Clamp(num3, 1, base.rectChildren.Count);
				num9 = Mathf.Clamp(num4, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num7));
			}
			else
			{
				num7 = num4;
				num9 = Mathf.Clamp(num4, 1, base.rectChildren.Count);
				num8 = Mathf.Clamp(num3, 1, Mathf.CeilToInt((float)base.rectChildren.Count / (float)num7));
			}
			Vector2 vector = new Vector2((float)num8 * this.cellSize.x + (float)(num8 - 1) * this.spacing.x, (float)num9 * this.cellSize.y + (float)(num9 - 1) * this.spacing.y);
			Vector2 vector2 = new Vector2(base.GetStartOffset(0, x * vector.x) / x, base.GetStartOffset(1, y * vector.y) / y);
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				int num10;
				int num11;
				if (this.startAxis == AnchorGridLayoutGroup.Axis.Horizontal)
				{
					num10 = j % num7;
					num11 = j / num7;
				}
				else
				{
					num10 = j / num7;
					num11 = j % num7;
				}
				if (num5 == 1)
				{
					num10 = num8 - 1 - num10;
				}
				if (num6 == 1)
				{
					num11 = num9 - 1 - num11;
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, x * (vector2.x + (this.cellSize[0] + this.spacing[0]) * (float)num10), x * this.cellSize[0]);
				base.SetChildAlongAxis(base.rectChildren[j], 1, y * (vector2.y + (this.cellSize[1] + this.spacing[1]) * (float)num11), y * this.cellSize[1]);
			}
		}

		// Token: 0x04000CB0 RID: 3248
		[SerializeField]
		protected TextAnchor m_anchorPoint;

		// Token: 0x04000CB1 RID: 3249
		[SerializeField]
		protected AnchorGridLayoutGroup.Corner m_StartCorner;

		// Token: 0x04000CB2 RID: 3250
		[SerializeField]
		protected AnchorGridLayoutGroup.Axis m_StartAxis;

		// Token: 0x04000CB3 RID: 3251
		[SerializeField]
		protected Vector2 m_CellSize = new Vector2(1f, 1f);

		// Token: 0x04000CB4 RID: 3252
		[SerializeField]
		protected Vector2 m_Spacing = Vector2.zero;

		// Token: 0x04000CB5 RID: 3253
		[SerializeField]
		protected AnchorGridLayoutGroup.Constraint m_Constraint;

		// Token: 0x04000CB6 RID: 3254
		[SerializeField]
		protected int m_ConstraintCount = 2;

		// Token: 0x02000360 RID: 864
		public enum Corner
		{
			// Token: 0x04000CB8 RID: 3256
			UpperLeft,
			// Token: 0x04000CB9 RID: 3257
			UpperRight,
			// Token: 0x04000CBA RID: 3258
			LowerLeft,
			// Token: 0x04000CBB RID: 3259
			LowerRight
		}

		// Token: 0x02000361 RID: 865
		public enum Axis
		{
			// Token: 0x04000CBD RID: 3261
			Horizontal,
			// Token: 0x04000CBE RID: 3262
			Vertical
		}

		// Token: 0x02000362 RID: 866
		public enum Constraint
		{
			// Token: 0x04000CC0 RID: 3264
			Flexible,
			// Token: 0x04000CC1 RID: 3265
			FixedColumnCount,
			// Token: 0x04000CC2 RID: 3266
			FixedRowCount
		}
	}
}
