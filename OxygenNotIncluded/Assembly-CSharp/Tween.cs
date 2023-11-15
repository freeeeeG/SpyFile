using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C7F RID: 3199
public class Tween : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06006606 RID: 26118 RVA: 0x0026124C File Offset: 0x0025F44C
	private void Awake()
	{
		this.Selectable = base.GetComponent<Selectable>();
	}

	// Token: 0x06006607 RID: 26119 RVA: 0x0026125A File Offset: 0x0025F45A
	public void OnPointerEnter(PointerEventData data)
	{
		this.Direction = 1f;
	}

	// Token: 0x06006608 RID: 26120 RVA: 0x00261267 File Offset: 0x0025F467
	public void OnPointerExit(PointerEventData data)
	{
		this.Direction = -1f;
	}

	// Token: 0x06006609 RID: 26121 RVA: 0x00261274 File Offset: 0x0025F474
	private void Update()
	{
		if (this.Selectable.interactable)
		{
			float x = base.transform.localScale.x;
			float num = x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed;
			num = Mathf.Min(num, Tween.Scale);
			num = Mathf.Max(num, 1f);
			if (num != x)
			{
				base.transform.localScale = new Vector3(num, num, 1f);
			}
		}
	}

	// Token: 0x04004643 RID: 17987
	private static float Scale = 1.025f;

	// Token: 0x04004644 RID: 17988
	private static float ScaleSpeed = 0.5f;

	// Token: 0x04004645 RID: 17989
	private Selectable Selectable;

	// Token: 0x04004646 RID: 17990
	private float Direction = -1f;
}
