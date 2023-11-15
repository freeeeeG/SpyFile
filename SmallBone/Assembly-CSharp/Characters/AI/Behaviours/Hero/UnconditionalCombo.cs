using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Hero;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013CF RID: 5071
	public abstract class UnconditionalCombo : Behaviour, IComboable, IEntryCombo
	{
		// Token: 0x060063F2 RID: 25586 RVA: 0x00122235 File Offset: 0x00120435
		public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
		{
			yield return this.CRun(controller);
			yield return comboSystem.CNext(controller);
			yield break;
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x00122252 File Offset: 0x00120452
		public IEnumerator CTryEntryCombo(AIController controller, ComboSystem comboSystem)
		{
			this._startAction.TryStart();
			while (this._startAction.running)
			{
				yield return null;
			}
			comboSystem.Start();
			yield return this.CTryContinuedCombo(controller, comboSystem);
			yield break;
		}

		// Token: 0x04005092 RID: 20626
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _startAction;
	}
}
