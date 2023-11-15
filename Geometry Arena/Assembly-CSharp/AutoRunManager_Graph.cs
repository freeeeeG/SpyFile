using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000D6 RID: 214
public class AutoRunManager_Graph : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000779 RID: 1913 RVA: 0x00029D60 File Offset: 0x00027F60
	private void Update()
	{
		if (this.above)
		{
			this.manager.MouseOn();
		}
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00029D75 File Offset: 0x00027F75
	private void OnDisable()
	{
		this.above = false;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00029D7E File Offset: 0x00027F7E
	public void OnPointerDown(PointerEventData eventData)
	{
		this.above = true;
		this.manager.GetPosX();
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00029D75 File Offset: 0x00027F75
	public void OnPointerUp(PointerEventData eventData)
	{
		this.above = false;
	}

	// Token: 0x04000645 RID: 1605
	[SerializeField]
	private AutoRunManager manager;

	// Token: 0x04000646 RID: 1606
	[SerializeField]
	private bool above;
}
