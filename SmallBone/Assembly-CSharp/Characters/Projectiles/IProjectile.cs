using System;
using Characters.Utils;
using UnityEngine;

namespace Characters.Projectiles
{
	// Token: 0x0200075E RID: 1886
	public interface IProjectile : IMonoBehaviour
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060026A1 RID: 9889
		Character owner { get; }

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060026A2 RID: 9890
		Collider2D collider { get; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060026A3 RID: 9891
		float baseDamage { get; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060026A4 RID: 9892
		float speedMultiplier { get; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060026A5 RID: 9893
		Vector2 firedDirection { get; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060026A6 RID: 9894
		// (set) Token: 0x060026A7 RID: 9895
		Vector2 direction { get; set; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060026A8 RID: 9896
		float speed { get; }

		// Token: 0x060026A9 RID: 9897
		void Despawn();

		// Token: 0x060026AA RID: 9898
		void DetectCollision(Vector2 origin, Vector2 direction, float speed);

		// Token: 0x060026AB RID: 9899
		void ClearHitHistroy();

		// Token: 0x060026AC RID: 9900
		void Fire(Character owner, float attackDamage, float direction, bool flipX = false, bool flipY = false, float speedMultiplier = 1f, HitHistoryManager hitHistoryManager = null, float delay = 0f);
	}
}
