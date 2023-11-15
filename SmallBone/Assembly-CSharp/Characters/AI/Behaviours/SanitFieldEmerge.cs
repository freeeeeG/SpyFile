using System;
using System.Collections;
using Characters.AI.Hero;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012DC RID: 4828
	public class SanitFieldEmerge : Behaviour
	{
		// Token: 0x06005F80 RID: 24448 RVA: 0x00117BE8 File Offset: 0x00115DE8
		public override IEnumerator CRun(AIController controller)
		{
			yield return this._attack.CRun(controller);
			if (this._field.isStuck)
			{
				yield return this._teleport.CRun(controller);
			}
			yield break;
		}

		// Token: 0x04004CC0 RID: 19648
		[SerializeField]
		private RunAction _attack;

		// Token: 0x04004CC1 RID: 19649
		[SerializeField]
		private RunAction _teleport;

		// Token: 0x04004CC2 RID: 19650
		[SerializeField]
		private SaintField _field;
	}
}
