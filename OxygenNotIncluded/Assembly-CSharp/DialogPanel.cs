using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C96 RID: 3222
public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x0600669E RID: 26270 RVA: 0x00264114 File Offset: 0x00262314
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.destroyOnDeselect)
		{
			foreach (object obj in base.transform)
			{
				Util.KDestroyGameObject(((Transform)obj).gameObject);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040046C7 RID: 18119
	public bool destroyOnDeselect = true;
}
