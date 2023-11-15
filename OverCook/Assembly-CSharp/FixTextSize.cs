using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000131 RID: 305
[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class FixTextSize : MonoBehaviour
{
	// Token: 0x06000590 RID: 1424 RVA: 0x0002A747 File Offset: 0x00028B47
	private void Start()
	{
		this.AdjustSize();
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0002A750 File Offset: 0x00028B50
	private void AdjustSize()
	{
		Text text = base.gameObject.RequireComponent<Text>();
		text.fontSize = this.CalculateFontSize();
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0002A778 File Offset: 0x00028B78
	private int CalculateFontSize()
	{
		RectTransform transform = base.transform as RectTransform;
		Canvas overlayCanvas = base.gameObject.RequestComponentUpwardsRecursive<Canvas>();
		return (int)this.GetRectInPixels(transform, overlayCanvas).height;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x0002A7B0 File Offset: 0x00028BB0
	private Rect GetRectInPixels(RectTransform _transform, Canvas _overlayCanvas)
	{
		Vector3[] canvasWorldCorners = new Vector3[4];
		_overlayCanvas.gameObject.RequireComponent<RectTransform>().GetWorldCorners(canvasWorldCorners);
		Vector3[] array = new Vector3[4];
		_transform.GetWorldCorners(array);
		Rect canvasPixelRect = _overlayCanvas.pixelRect;
		Converter<Vector3, Vector2> converter = delegate(Vector3 _worldPos)
		{
			Vector3 lhs = _worldPos - canvasWorldCorners[0];
			Vector3 rhs = canvasWorldCorners[2] - canvasWorldCorners[1];
			Vector3 rhs2 = canvasWorldCorners[1] - canvasWorldCorners[0];
			float num5 = Vector3.Dot(lhs, rhs) / rhs.sqrMagnitude;
			float num6 = Vector3.Dot(lhs, rhs2) / rhs2.sqrMagnitude;
			return new Vector2(canvasPixelRect.width * num5, canvasPixelRect.height * num6);
		};
		Vector2[] array2 = array.ConvertAll(converter);
		float num;
		array2.FindLowestScoring((Vector2 x) => x.x, out num);
		float num2;
		array2.FindHighestScoring((Vector2 x) => x.x, out num2);
		float num3;
		array2.FindLowestScoring((Vector2 x) => x.y, out num3);
		float num4;
		array2.FindHighestScoring((Vector2 x) => x.y, out num4);
		return new Rect(num, num3, num2 - num, num4 - num3);
	}
}
