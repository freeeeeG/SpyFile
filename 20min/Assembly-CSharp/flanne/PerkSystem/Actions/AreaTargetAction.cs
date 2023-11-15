using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A7 RID: 423
	public class AreaTargetAction : Action
	{
		// Token: 0x060009EC RID: 2540 RVA: 0x000274A5 File Offset: 0x000256A5
		public override void Init()
		{
			this.action.Init();
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000274B4 File Offset: 0x000256B4
		public override void Activate(GameObject target)
		{
			foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(target.transform.position, this.range, 1 << TagLayerUtil.Enemy))
			{
				this.action.Activate(collider2D.gameObject);
			}
		}

		// Token: 0x04000705 RID: 1797
		[SerializeField]
		private float range;

		// Token: 0x04000706 RID: 1798
		[SerializeReference]
		private Action action;
	}
}
