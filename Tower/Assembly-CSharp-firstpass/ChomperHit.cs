using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000009 RID: 9
	public class ChomperHit : SimpleHit
	{
		// Token: 0x06000023 RID: 35 RVA: 0x0000270C File Offset: 0x0000090C
		protected override void Start()
		{
			base.Start();
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002720 File Offset: 0x00000920
		protected override void HitTrigger()
		{
			base.HitTrigger();
			this.animator.SetTrigger("Hit");
		}

		// Token: 0x04000011 RID: 17
		private Animator animator;
	}
}
