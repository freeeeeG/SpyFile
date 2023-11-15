using System;
using Characters;

namespace FX.EffectProperties
{
	// Token: 0x02000288 RID: 648
	public abstract class EffectProperty
	{
		// Token: 0x06000CA7 RID: 3239
		public abstract void Apply(PoolObject spawned, Character owner, Target target);
	}
}
