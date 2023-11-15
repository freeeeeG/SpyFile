using System;
using Characters;

namespace BT.Conditions
{
	// Token: 0x02001430 RID: 5168
	public sealed class HasTarget : Condition
	{
		// Token: 0x0600656B RID: 25963 RVA: 0x0012586C File Offset: 0x00123A6C
		protected override bool Check(Context context)
		{
			return context.Get<Character>(Key.Target) != null;
		}
	}
}
