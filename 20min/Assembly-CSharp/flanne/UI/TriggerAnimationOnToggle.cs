using System;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200023A RID: 570
	public class TriggerAnimationOnToggle : MonoBehaviour
	{
		// Token: 0x06000C88 RID: 3208 RVA: 0x0002DEFE File Offset: 0x0002C0FE
		private void Start()
		{
			this.toggle.onValueChanged.AddListener(delegate(bool <p0>)
			{
				this.ToggleValueChanged(this.toggle);
			});
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0002DF1C File Offset: 0x0002C11C
		private void ToggleValueChanged(Toggle change)
		{
			this.TriggerAnimation();
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0002DF24 File Offset: 0x0002C124
		private void TriggerAnimation()
		{
			if (this.toggle.isOn)
			{
				this.animator.ResetTrigger(this.toggleOffTrigger);
				this.animator.SetTrigger(this.toggleOnTrigger);
				return;
			}
			this.animator.ResetTrigger(this.toggleOnTrigger);
			this.animator.SetTrigger(this.toggleOffTrigger);
		}

		// Token: 0x040008CA RID: 2250
		[SerializeField]
		private Toggle toggle;

		// Token: 0x040008CB RID: 2251
		[SerializeField]
		private Animator animator;

		// Token: 0x040008CC RID: 2252
		[SerializeField]
		private string toggleOnTrigger;

		// Token: 0x040008CD RID: 2253
		[SerializeField]
		private string toggleOffTrigger;
	}
}
