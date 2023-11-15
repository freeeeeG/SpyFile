using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C9D RID: 3229
public class OptionSelector : MonoBehaviour
{
	// Token: 0x060066CF RID: 26319 RVA: 0x00265AD9 File Offset: 0x00263CD9
	private void Start()
	{
		this.selectedItem.GetComponent<KButton>().onBtnClick += this.OnClick;
	}

	// Token: 0x060066D0 RID: 26320 RVA: 0x00265AF7 File Offset: 0x00263CF7
	public void Initialize(object id)
	{
		this.id = id;
	}

	// Token: 0x060066D1 RID: 26321 RVA: 0x00265B00 File Offset: 0x00263D00
	private void OnClick(KKeyCode button)
	{
		if (button == KKeyCode.Mouse0)
		{
			this.OnChangePriority(this.id, 1);
			return;
		}
		if (button != KKeyCode.Mouse1)
		{
			return;
		}
		this.OnChangePriority(this.id, -1);
	}

	// Token: 0x060066D2 RID: 26322 RVA: 0x00265B38 File Offset: 0x00263D38
	public void ConfigureItem(bool disabled, OptionSelector.DisplayOptionInfo display_info)
	{
		HierarchyReferences component = this.selectedItem.GetComponent<HierarchyReferences>();
		KImage kimage = component.GetReference("BG") as KImage;
		if (display_info.bgOptions == null)
		{
			kimage.gameObject.SetActive(false);
		}
		else
		{
			kimage.sprite = display_info.bgOptions[display_info.bgIndex];
		}
		KImage kimage2 = component.GetReference("FG") as KImage;
		if (display_info.fgOptions == null)
		{
			kimage2.gameObject.SetActive(false);
		}
		else
		{
			kimage2.sprite = display_info.fgOptions[display_info.fgIndex];
		}
		KImage kimage3 = component.GetReference("Fill") as KImage;
		if (kimage3 != null)
		{
			kimage3.enabled = !disabled;
			kimage3.color = display_info.fillColour;
		}
		KImage kimage4 = component.GetReference("Outline") as KImage;
		if (kimage4 != null)
		{
			kimage4.enabled = !disabled;
		}
	}

	// Token: 0x0400471D RID: 18205
	private object id;

	// Token: 0x0400471E RID: 18206
	public Action<object, int> OnChangePriority;

	// Token: 0x0400471F RID: 18207
	[SerializeField]
	private KImage selectedItem;

	// Token: 0x04004720 RID: 18208
	[SerializeField]
	private KImage itemTemplate;

	// Token: 0x02001BC5 RID: 7109
	public class DisplayOptionInfo
	{
		// Token: 0x04007DDF RID: 32223
		public IList<Sprite> bgOptions;

		// Token: 0x04007DE0 RID: 32224
		public IList<Sprite> fgOptions;

		// Token: 0x04007DE1 RID: 32225
		public int bgIndex;

		// Token: 0x04007DE2 RID: 32226
		public int fgIndex;

		// Token: 0x04007DE3 RID: 32227
		public Color32 fillColour;
	}
}
