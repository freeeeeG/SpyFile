using System;
using FX;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	// Token: 0x020003C4 RID: 964
	public class PlaySoundOnSelected : EventTrigger
	{
		// Token: 0x060011EC RID: 4588 RVA: 0x00034E40 File Offset: 0x00033040
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (PlaySoundOnSelected._lastSelected == eventData.selectedObject)
			{
				return;
			}
			PlaySoundOnSelected._lastSelected = eventData.selectedObject;
			PersistentSingleton<SoundManager>.Instance.PlaySound(this.soundInfo, Vector3.zero);
		}

		// Token: 0x04000ED4 RID: 3796
		private static GameObject _lastSelected;

		// Token: 0x04000ED5 RID: 3797
		public SoundInfo soundInfo;
	}
}
