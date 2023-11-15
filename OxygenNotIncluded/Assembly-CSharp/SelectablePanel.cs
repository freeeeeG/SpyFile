using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BEB RID: 3051
public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x06006084 RID: 24708 RVA: 0x0023AC63 File Offset: 0x00238E63
	public void OnDeselect(BaseEventData evt)
	{
		base.gameObject.SetActive(false);
	}
}
