using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x020000A2 RID: 162
	[AddComponentMenu("Modular Options/Selectable UI Sound")]
	[RequireComponent(typeof(Selectable), typeof(AudioSource))]
	public class SelectableUISound : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x06000233 RID: 563 RVA: 0x000090C0 File Offset: 0x000072C0
		private void Awake()
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000090CE File Offset: 0x000072CE
		public void OnPointerClick(PointerEventData _eventData)
		{
			this.PlayIfNotNull(this.soundData.submitSound);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000090E1 File Offset: 0x000072E1
		public void OnPointerEnter(PointerEventData _eventData)
		{
			this.PlayIfNotNull(this.soundData.selectionSound);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000090F4 File Offset: 0x000072F4
		public void OnPointerExit(PointerEventData _eventData)
		{
			this.PlayIfNotNull(this.soundData.deselectionSound);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009107 File Offset: 0x00007307
		public void OnSubmit(BaseEventData _eventData)
		{
			this.PlayIfNotNull(this.soundData.submitSound);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000911A File Offset: 0x0000731A
		public void OnSelect(BaseEventData _eventData)
		{
			this.PlayIfNotNull(this.soundData.selectionSound);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000912D File Offset: 0x0000732D
		public void OnDeselect(BaseEventData _eventData)
		{
			this.PlayIfNotNull(this.soundData.deselectionSound);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009140 File Offset: 0x00007340
		private void PlayIfNotNull(AudioClip _clip)
		{
			if (_clip != null)
			{
				this.audioSource.PlayOneShot(_clip);
			}
		}

		// Token: 0x040001E5 RID: 485
		[Tooltip("Reference to ScriptableObject containing sound data. Create a new one by right-clicking in the Project-window and clicking DataContainer/UI/SelectableSound")]
		public SelectableUISoundData soundData;

		// Token: 0x040001E6 RID: 486
		private AudioSource audioSource;
	}
}
