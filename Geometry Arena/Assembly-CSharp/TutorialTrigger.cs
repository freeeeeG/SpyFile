using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000DC RID: 220
public class TutorialTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x060007A6 RID: 1958 RVA: 0x0002A9FA File Offset: 0x00028BFA
	private void Update()
	{
		if (this.ifKeyCode && Input.GetKeyDown(this.keyCode))
		{
			this.Trig();
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0002AA17 File Offset: 0x00028C17
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.type == TutorialTrigger.EnumTrigType.CLICK)
		{
			this.Trig();
		}
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0002AA27 File Offset: 0x00028C27
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.type == TutorialTrigger.EnumTrigType.ENTER)
		{
			this.Trig();
		}
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x000051D0 File Offset: 0x000033D0
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0002AA38 File Offset: 0x00028C38
	private void OnDisable()
	{
		if (this.type == TutorialTrigger.EnumTrigType.DISABLE)
		{
			this.Trig();
		}
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0002AA49 File Offset: 0x00028C49
	private void Trig()
	{
		TutorialMaster.inst.TrigID(this.trigID);
	}

	// Token: 0x04000672 RID: 1650
	[SerializeField]
	private int trigID = -1;

	// Token: 0x04000673 RID: 1651
	[SerializeField]
	private TutorialTrigger.EnumTrigType type = TutorialTrigger.EnumTrigType.UNINITED;

	// Token: 0x04000674 RID: 1652
	[SerializeField]
	private bool ifKeyCode;

	// Token: 0x04000675 RID: 1653
	[SerializeField]
	private KeyCode keyCode = KeyCode.G;

	// Token: 0x0200015E RID: 350
	public enum EnumTrigType
	{
		// Token: 0x04000A00 RID: 2560
		UNINITED = -1,
		// Token: 0x04000A01 RID: 2561
		CLICK,
		// Token: 0x04000A02 RID: 2562
		ENTER,
		// Token: 0x04000A03 RID: 2563
		DISABLE
	}
}
