using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B5D RID: 2909
public class PointerDelegate : Selectable
{
	// Token: 0x06003B19 RID: 15129 RVA: 0x00119A89 File Offset: 0x00117E89
	protected override void Awake()
	{
		base.Awake();
		if (this.PointerEnter == null)
		{
			this.PointerEnter = new UnityEvent();
		}
		if (this.PointerExit == null)
		{
			this.PointerExit = new UnityEvent();
		}
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x00119ABD File Offset: 0x00117EBD
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.PointerEnter != null)
		{
			this.PointerEnter.Invoke();
		}
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x00119ADC File Offset: 0x00117EDC
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (this.PointerExit != null)
		{
			this.PointerExit.Invoke();
		}
	}

	// Token: 0x04003018 RID: 12312
	public UnityEvent PointerEnter;

	// Token: 0x04003019 RID: 12313
	public UnityEvent PointerExit;
}
