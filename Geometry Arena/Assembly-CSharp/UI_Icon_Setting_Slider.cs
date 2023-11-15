using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000097 RID: 151
public class UI_Icon_Setting_Slider : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x06000549 RID: 1353 RVA: 0x0001EEF5 File Offset: 0x0001D0F5
	public void OnPointerUp(PointerEventData eventData)
	{
		this.ifDraging = false;
		this.ApplySetting();
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0001EF04 File Offset: 0x0001D104
	public void OnPointerDown(PointerEventData eventData)
	{
		this.ifDraging = true;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0001EF0D File Offset: 0x0001D10D
	private void Awake()
	{
		if (this.slider == null)
		{
			this.slider = base.gameObject.GetComponent<Slider>();
		}
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0001EF2E File Offset: 0x0001D12E
	private void Update()
	{
		if (this.ifDraging)
		{
			this.ApplySetting();
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0001EF3E File Offset: 0x0001D13E
	private void OnEnable()
	{
		this.InitWithIndex();
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001EF46 File Offset: 0x0001D146
	private void InitWithIndex()
	{
		this.slider.value = (float)Setting.Inst.setFloats[this.index];
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0001EF68 File Offset: 0x0001D168
	private void ApplySetting()
	{
		Setting.Inst.setFloats[this.index] = (double)this.slider.value;
		int num = this.index;
	}

	// Token: 0x04000458 RID: 1112
	public Slider slider;

	// Token: 0x04000459 RID: 1113
	public int index;

	// Token: 0x0400045A RID: 1114
	private bool ifDraging;
}
