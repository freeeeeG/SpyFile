using System;
using Characters;

namespace Runnables
{
	// Token: 0x02000321 RID: 801
	public interface IStatusEvent
	{
		// Token: 0x06000F6F RID: 3951
		void Apply(Character owner, Character target);

		// Token: 0x06000F70 RID: 3952
		void Release(Character owner, Character target);
	}
}
