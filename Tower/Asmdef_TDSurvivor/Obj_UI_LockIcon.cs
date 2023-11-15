using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000140 RID: 320
public class Obj_UI_LockIcon : MonoBehaviour
{
	// Token: 0x06000849 RID: 2121 RVA: 0x0001F689 File Offset: 0x0001D889
	public void ToggleLockIcon(bool isOn)
	{
		this.image_LockIcon.enabled = isOn;
	}

	// Token: 0x040006B1 RID: 1713
	[SerializeField]
	private Image image_LockIcon;
}
