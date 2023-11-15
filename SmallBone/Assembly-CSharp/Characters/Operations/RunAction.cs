using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DFC RID: 3580
	public class RunAction : CharacterOperation
	{
		// Token: 0x060047A0 RID: 18336 RVA: 0x000D0401 File Offset: 0x000CE601
		public override void Run(Character owner)
		{
			this._action.TryStart();
		}

		// Token: 0x040036A5 RID: 13989
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
