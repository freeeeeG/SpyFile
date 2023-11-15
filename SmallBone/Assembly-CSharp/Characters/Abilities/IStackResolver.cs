using System;

namespace Characters.Abilities
{
	// Token: 0x02000A13 RID: 2579
	public interface IStackResolver
	{
		// Token: 0x060036AE RID: 13998
		void Initialize();

		// Token: 0x060036AF RID: 13999
		void Attach(Character owner);

		// Token: 0x060036B0 RID: 14000
		void Detach(Character owner);

		// Token: 0x060036B1 RID: 14001
		int GetStack(ref Damage damage);

		// Token: 0x060036B2 RID: 14002
		void UpdateTime(float deltaTime);
	}
}
