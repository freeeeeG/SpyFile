using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001D6 RID: 470
	public class ThunderNearCursorAction : Action
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x00028900 File Offset: 0x00026B00
		public override void Activate(GameObject target)
		{
			GameObject closestEnemy = EnemyFinder.GetClosestEnemy(Camera.main.ScreenToWorldPoint(ShootingCursor.Instance.cursorPosition));
			if (closestEnemy != null)
			{
				ThunderGenerator.SharedInstance.GenerateAt(closestEnemy, this.baseDamage);
			}
		}

		// Token: 0x0400076A RID: 1898
		[SerializeField]
		private int baseDamage;
	}
}
