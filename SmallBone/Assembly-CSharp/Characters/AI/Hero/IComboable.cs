using System;
using System.Collections;

namespace Characters.AI.Hero
{
	// Token: 0x02001277 RID: 4727
	public interface IComboable
	{
		// Token: 0x06005DB9 RID: 23993
		IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem);
	}
}
