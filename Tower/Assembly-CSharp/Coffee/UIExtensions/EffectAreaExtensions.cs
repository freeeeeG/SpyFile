using System;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIExtensions
{
	// Token: 0x020000EA RID: 234
	public static class EffectAreaExtensions
	{
		// Token: 0x06000369 RID: 873 RVA: 0x0000F350 File Offset: 0x0000D550
		public static Rect GetEffectArea(this EffectArea area, VertexHelper vh, Graphic graphic, float aspectRatio = -1f)
		{
			Rect result = default(Rect);
			switch (area)
			{
			case EffectArea.RectTransform:
				result = graphic.rectTransform.rect;
				break;
			case EffectArea.Fit:
			{
				UIVertex uivertex = default(UIVertex);
				result.xMin = (result.yMin = float.MaxValue);
				result.xMax = (result.yMax = float.MinValue);
				for (int i = 0; i < vh.currentVertCount; i++)
				{
					vh.PopulateUIVertex(ref uivertex, i);
					result.xMin = Mathf.Min(result.xMin, uivertex.position.x);
					result.yMin = Mathf.Min(result.yMin, uivertex.position.y);
					result.xMax = Mathf.Max(result.xMax, uivertex.position.x);
					result.yMax = Mathf.Max(result.yMax, uivertex.position.y);
				}
				break;
			}
			case EffectArea.Character:
				result = EffectAreaExtensions.rectForCharacter;
				break;
			default:
				result = graphic.rectTransform.rect;
				break;
			}
			if (0f < aspectRatio)
			{
				if (result.width < result.height)
				{
					result.width = result.height * aspectRatio;
				}
				else
				{
					result.height = result.width / aspectRatio;
				}
			}
			return result;
		}

		// Token: 0x04000342 RID: 834
		private static readonly Rect rectForCharacter = new Rect(0f, 0f, 1f, 1f);
	}
}
