using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001AD RID: 429
public class UI_SoundOnMouseOver : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	// Token: 0x06000B7B RID: 2939 RVA: 0x0002D082 File Offset: 0x0002B282
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundManager.PlaySound(this.soundDataName, this.soundKey, -1f, -1f, -1f);
	}

	// Token: 0x0400092C RID: 2348
	[SerializeField]
	private string soundDataName;

	// Token: 0x0400092D RID: 2349
	[SerializeField]
	private string soundKey;
}
