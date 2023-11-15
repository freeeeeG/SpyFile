using System;

namespace Characters.AI.Behaviours.DarkQuartzGolem
{
	// Token: 0x0200138A RID: 5002
	public interface IPattern
	{
		// Token: 0x060062B8 RID: 25272
		bool CanUse();

		// Token: 0x060062B9 RID: 25273
		bool CanUse(AIController controller);
	}
}
