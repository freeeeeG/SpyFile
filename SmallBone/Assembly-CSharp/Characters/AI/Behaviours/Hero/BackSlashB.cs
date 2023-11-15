using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013A9 RID: 5033
	public class BackSlashB : Behaviour
	{
		// Token: 0x06006342 RID: 25410 RVA: 0x00120CB6 File Offset: 0x0011EEB6
		public override IEnumerator CRun(AIController controller)
		{
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400500B RID: 20491
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _attackAction;
	}
}
