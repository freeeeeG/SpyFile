using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000177 RID: 375
public class UI_Obj_CardPool : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x060009EF RID: 2543 RVA: 0x0002559D File Offset: 0x0002379D
	private void Start()
	{
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0002559F File Offset: 0x0002379F
	private void Update()
	{
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x000255A4 File Offset: 0x000237A4
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}
		Debug.Log("OnDrop Pool: " + eventData.pointerDrag.name);
		UI_DraggableCard component = eventData.pointerDrag.GetComponent<UI_DraggableCard>();
		if (component != null)
		{
			component.ReturnToCardPool();
		}
	}
}
