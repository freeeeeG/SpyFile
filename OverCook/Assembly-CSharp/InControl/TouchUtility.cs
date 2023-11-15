using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D0 RID: 720
	public static class TouchUtility
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x00048418 File Offset: 0x00046818
		public static Vector2 AnchorToViewPoint(TouchControlAnchor touchControlAnchor)
		{
			switch (touchControlAnchor)
			{
			case TouchControlAnchor.TopLeft:
				return new Vector2(0f, 1f);
			case TouchControlAnchor.CenterLeft:
				return new Vector2(0f, 0.5f);
			case TouchControlAnchor.BottomLeft:
				return new Vector2(0f, 0f);
			case TouchControlAnchor.TopCenter:
				return new Vector2(0.5f, 1f);
			case TouchControlAnchor.Center:
				return new Vector2(0.5f, 0.5f);
			case TouchControlAnchor.BottomCenter:
				return new Vector2(0.5f, 0f);
			case TouchControlAnchor.TopRight:
				return new Vector2(1f, 1f);
			case TouchControlAnchor.CenterRight:
				return new Vector2(1f, 0.5f);
			case TouchControlAnchor.BottomRight:
				return new Vector2(1f, 0f);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000484E9 File Offset: 0x000468E9
		public static Vector2 RoundVector(Vector2 vector)
		{
			return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
		}
	}
}
