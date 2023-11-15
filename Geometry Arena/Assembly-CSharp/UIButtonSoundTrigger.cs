using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000E5 RID: 229
public class UIButtonSoundTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
	// Token: 0x06000808 RID: 2056 RVA: 0x0002DF43 File Offset: 0x0002C143
	public void OnPointerClick(PointerEventData eventData)
	{
		SoundEffects.Inst.ui_ButtonClick.PlayRandom();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x0002DF54 File Offset: 0x0002C154
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundEffects.Inst.ui_ButtonEnter.PlayRandom();
	}
}
