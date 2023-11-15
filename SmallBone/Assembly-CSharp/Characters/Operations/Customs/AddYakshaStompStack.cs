using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FC5 RID: 4037
	public class AddYakshaStompStack : Operation
	{
		// Token: 0x06004E29 RID: 20009 RVA: 0x000E9EC7 File Offset: 0x000E80C7
		public override void Run()
		{
			this._yakshaPassive.AddStack();
		}

		// Token: 0x04003E2F RID: 15919
		[SerializeField]
		private YakshaPassiveAttacher _yakshaPassive;
	}
}
