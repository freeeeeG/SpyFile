using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Hero;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013CC RID: 5068
	public abstract class SequentialCombo : Behaviour, IComboable, IEntryCombo
	{
		// Token: 0x060063E3 RID: 25571 RVA: 0x00122069 File Offset: 0x00120269
		public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
		{
			yield return this.CRun(controller);
			if (comboSystem.TryComboAttack())
			{
				yield return comboSystem.CNext(controller);
				yield break;
			}
			comboSystem.Clear();
			if (this._endAction.TryStart())
			{
				while (this._endAction.running)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060063E4 RID: 25572 RVA: 0x00122086 File Offset: 0x00120286
		public IEnumerator CTryEntryCombo(AIController controller, ComboSystem comboSystem)
		{
			this._startAction.TryStart();
			while (this._startAction.running)
			{
				yield return null;
			}
			yield return this.CTryContinuedCombo(controller, comboSystem);
			yield break;
		}

		// Token: 0x04005086 RID: 20614
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _startAction;

		// Token: 0x04005087 RID: 20615
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _endAction;
	}
}
