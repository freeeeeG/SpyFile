using System;
using Characters;

namespace Runnables
{
	// Token: 0x02000320 RID: 800
	public interface IHitEvent
	{
		// Token: 0x06000F6E RID: 3950
		void OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt);
	}
}
