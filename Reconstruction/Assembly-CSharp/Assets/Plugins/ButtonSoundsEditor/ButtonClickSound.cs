using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Plugins.ButtonSoundsEditor
{
	// Token: 0x020002C2 RID: 706
	public class ButtonClickSound : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600113E RID: 4414 RVA: 0x0003125C File Offset: 0x0002F45C
		public void OnPointerClick(PointerEventData eventData)
		{
			this.PlayClickSound();
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x00031264 File Offset: 0x0002F464
		private void PlayClickSound()
		{
			Singleton<Sound>.Instance.PlayEffect(this.ClickSound);
		}

		// Token: 0x04000988 RID: 2440
		public AudioSource AudioSource;

		// Token: 0x04000989 RID: 2441
		public AudioClip ClickSound;
	}
}
