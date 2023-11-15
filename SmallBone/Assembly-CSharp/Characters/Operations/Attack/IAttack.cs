using System;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F88 RID: 3976
	public interface IAttack
	{
		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06004D22 RID: 19746
		// (remove) Token: 0x06004D23 RID: 19747
		event OnAttackHitDelegate onHit;
	}
}
