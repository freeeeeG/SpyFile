using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000261 RID: 609
public class InfoBtn : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000F4A RID: 3914 RVA: 0x00028AA2 File Offset: 0x00026CA2
	public void SetContent(string content)
	{
		this.content = content;
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00028AAC File Offset: 0x00026CAC
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.content == "")
		{
			return;
		}
		Vector2 a = RectTransformUtility.WorldToScreenPoint(Camera.main, base.transform.position);
		Singleton<TipsManager>.Instance.ShowTempTips(this.content, a + this.offset);
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x00028B03 File Offset: 0x00026D03
	public void OnPointerExit(PointerEventData eventData)
	{
		Singleton<TipsManager>.Instance.HideTempTips();
	}

	// Token: 0x040007AD RID: 1965
	[TextArea(2, 3)]
	[SerializeField]
	private string content;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	private Vector2 offset;
}
