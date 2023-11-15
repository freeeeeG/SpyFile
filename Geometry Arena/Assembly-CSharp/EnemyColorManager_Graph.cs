using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000DA RID: 218
public class EnemyColorManager_Graph : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x0600078F RID: 1935 RVA: 0x0002A250 File Offset: 0x00028450
	private void Update()
	{
		if (this.above)
		{
			this.manager.MouseOn();
		}
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0002A265 File Offset: 0x00028465
	private void OnDisable()
	{
		this.above = false;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0002A26E File Offset: 0x0002846E
	public void OnPointerDown(PointerEventData eventData)
	{
		this.above = true;
		this.manager.GetPosX();
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0002A265 File Offset: 0x00028465
	public void OnPointerUp(PointerEventData eventData)
	{
		this.above = false;
	}

	// Token: 0x0400065D RID: 1629
	[SerializeField]
	private EnemyColorManager manager;

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	private bool above;
}
