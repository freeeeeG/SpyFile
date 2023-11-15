using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A4 RID: 164
	[AddComponentMenu("Modular Options/Misc/Set Cursor Attributes On Awake")]
	public class SetCursorAttributesOnAwake : MonoBehaviour
	{
		// Token: 0x0600023D RID: 573 RVA: 0x00009167 File Offset: 0x00007367
		private void Awake()
		{
			Cursor.visible = this.visible;
			Cursor.lockState = this.lockState;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000917F File Offset: 0x0000737F
		public void SetCursorVisibility(bool _visible)
		{
			Cursor.visible = _visible;
		}

		// Token: 0x040001EA RID: 490
		public bool visible = true;

		// Token: 0x040001EB RID: 491
		public CursorLockMode lockState = CursorLockMode.Confined;
	}
}
