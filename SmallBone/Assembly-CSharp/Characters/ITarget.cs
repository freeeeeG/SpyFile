using System;
using Level;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000703 RID: 1795
	public interface ITarget
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600244B RID: 9291
		Collider2D collider { get; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600244C RID: 9292
		Transform transform { get; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600244D RID: 9293
		Character character { get; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600244E RID: 9294
		DestructibleObject damageable { get; }
	}
}
