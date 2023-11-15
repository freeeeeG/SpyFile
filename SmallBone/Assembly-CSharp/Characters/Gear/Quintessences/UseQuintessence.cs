using System;
using UnityEngine;

namespace Characters.Gear.Quintessences
{
	// Token: 0x020008E2 RID: 2274
	public abstract class UseQuintessence : MonoBehaviour
	{
		// Token: 0x0600309D RID: 12445 RVA: 0x00091AF3 File Offset: 0x0008FCF3
		protected virtual void Awake()
		{
			this._quintessence.onUse += this.OnUse;
		}

		// Token: 0x0600309E RID: 12446
		protected abstract void OnUse();

		// Token: 0x04002822 RID: 10274
		[SerializeField]
		protected Quintessence _quintessence;
	}
}
