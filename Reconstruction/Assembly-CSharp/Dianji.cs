using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C8 RID: 456
public class Dianji : MonoBehaviour, ICanvasRaycastFilter
{
	// Token: 0x06000B9F RID: 2975 RVA: 0x0001E45C File Offset: 0x0001C65C
	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		Debug.Log(screenPoint);
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_Canvas.GetComponent<RectTransform>(), screenPoint, eventCamera, out vector);
		string str = "Local=";
		Vector2 vector2 = vector;
		Debug.Log(str + vector2.ToString());
		if (this.onClockButton.GetComponent<RectTransform>().rect.Contains(vector))
		{
			Debug.Log("False");
			return false;
		}
		Debug.Log("True");
		return true;
	}

	// Token: 0x040005CA RID: 1482
	[SerializeField]
	private Canvas m_Canvas;

	// Token: 0x040005CB RID: 1483
	public GameObject onClockButton;

	// Token: 0x040005CC RID: 1484
	private Button btn;
}
