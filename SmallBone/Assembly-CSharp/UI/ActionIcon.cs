using System;
using Characters.Actions;
using GameResources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x02000386 RID: 902
	public class ActionIcon : IconWithCooldown
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x00030BDC File Offset: 0x0002EDDC
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x00030BE4 File Offset: 0x0002EDE4
		public Characters.Actions.Action action { get; set; }

		// Token: 0x0600107C RID: 4220 RVA: 0x00030BF0 File Offset: 0x0002EDF0
		protected override void Update()
		{
			if (this.action == null)
			{
				return;
			}
			base.Update();
			base.icon.material = (this.action.canUse ? null : MaterialResource.ui_grayScale);
			if (this.action.owner.silence.value)
			{
				if (!this._silenceMask.gameObject.activeInHierarchy)
				{
					this._silenceMask.gameObject.SetActive(true);
				}
				base.icon.material = MaterialResource.ui_grayScale;
				return;
			}
			if (this._silenceMask.gameObject.activeInHierarchy)
			{
				this._silenceMask.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000D88 RID: 3464
		[SerializeField]
		private Image _silenceMask;
	}
}
