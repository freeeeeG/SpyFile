using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000D8 RID: 216
public class BulletAlphaManager_Graph : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000784 RID: 1924 RVA: 0x00029FF0 File Offset: 0x000281F0
	private void Update()
	{
		if (this.above)
		{
			this.manager.MouseOn();
		}
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0002A005 File Offset: 0x00028205
	private void OnDisable()
	{
		this.above = false;
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0002A00E File Offset: 0x0002820E
	public void OnPointerDown(PointerEventData eventData)
	{
		this.above = true;
		this.manager.GetPosX();
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0002A005 File Offset: 0x00028205
	public void OnPointerUp(PointerEventData eventData)
	{
		this.above = false;
	}

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private BulletAlphaManager manager;

	// Token: 0x04000653 RID: 1619
	[SerializeField]
	private bool above;
}
