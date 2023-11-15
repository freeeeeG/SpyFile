using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D7 RID: 471
	public class ToggleSummonShootTowardsCursorAction : Action
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0002894C File Offset: 0x00026B4C
		public override void Activate(GameObject target)
		{
			foreach (ShootingSummon shootingSummon in PlayerController.Instance.GetComponentsInChildren<ShootingSummon>(true))
			{
				if (shootingSummon.SummonTypeID == this.SummonTypeID)
				{
					shootingSummon.targetMouse = this.shootTowardCursor;
				}
			}
		}

		// Token: 0x0400076B RID: 1899
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x0400076C RID: 1900
		[SerializeField]
		private bool shootTowardCursor;
	}
}
