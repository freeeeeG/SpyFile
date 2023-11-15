using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D4 RID: 468
	public class TeleportTowardsCursorAction : Action
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x00028672 File Offset: 0x00026872
		public override void Init()
		{
			this.player = PlayerController.Instance;
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0002868C File Offset: 0x0002688C
		public override void Activate(GameObject target)
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
			Vector2 vector = this.player.transform.position;
			Vector2 a2 = a - vector;
			Vector2 position = vector + a2 * this.distance;
			this.player.SetPosition(position);
		}

		// Token: 0x0400075E RID: 1886
		[SerializeField]
		private float distance;

		// Token: 0x0400075F RID: 1887
		private PlayerController player;

		// Token: 0x04000760 RID: 1888
		private ShootingCursor SC;
	}
}
