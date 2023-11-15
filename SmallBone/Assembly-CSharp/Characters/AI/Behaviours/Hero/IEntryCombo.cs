using System;
using System.Collections;
using Characters.AI.Hero;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013C8 RID: 5064
	public interface IEntryCombo
	{
		// Token: 0x060063D1 RID: 25553
		IEnumerator CTryEntryCombo(AIController controller, ComboSystem comboSystem);
	}
}
