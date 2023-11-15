using System;
using UnityEngine;

namespace FX
{
	// Token: 0x02000227 RID: 551
	[RequireComponent(typeof(LineRenderer))]
	public class BezierCurve : MonoBehaviour
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0001D95C File Offset: 0x0001BB5C
		public int count
		{
			get
			{
				return this._points.Length;
			}
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0001D966 File Offset: 0x0001BB66
		public void SetVector(int index, Vector2 vector)
		{
			this._points[index] = vector;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0001D975 File Offset: 0x0001BB75
		public void SetStart(Vector2 vector)
		{
			this.SetVector(0, vector);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0001D97F File Offset: 0x0001BB7F
		public void SetEnd(Vector2 vector)
		{
			this.SetVector(this.count - 1, vector);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0001D990 File Offset: 0x0001BB90
		public void UpdateCurve()
		{
			for (int i = 0; i < this._lineRenderer.positionCount; i++)
			{
				float time = (float)i / (float)(this._lineRenderer.positionCount - 1);
				this._lineRenderer.SetPosition(i, MMMaths.BezierCurve(this._points, time));
			}
		}

		// Token: 0x040008F1 RID: 2289
		[SerializeField]
		private LineRenderer _lineRenderer;

		// Token: 0x040008F2 RID: 2290
		[SerializeField]
		private Vector2[] _points = new Vector2[4];
	}
}
