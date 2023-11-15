using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000A1 RID: 161
public class ButtonMouseDetector : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000599 RID: 1433 RVA: 0x00020310 File Offset: 0x0001E510
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			Player.inst.mouseOnButton = true;
		}
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002032A File Offset: 0x0001E52A
	public void OnPointerExit(PointerEventData eventData)
	{
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			Player.inst.mouseOnButton = false;
		}
	}
}
