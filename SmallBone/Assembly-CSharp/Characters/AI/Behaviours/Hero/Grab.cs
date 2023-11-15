using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Hero;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013B8 RID: 5048
	public class Grab : Behaviour, IFinish, IComboable
	{
		// Token: 0x0600638A RID: 25482 RVA: 0x00121697 File Offset: 0x0011F897
		public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
		{
			comboSystem.Clear();
			yield return this.CCombat(controller);
			yield break;
		}

		// Token: 0x0600638B RID: 25483 RVA: 0x001216B4 File Offset: 0x0011F8B4
		public override IEnumerator CRun(AIController controller)
		{
			this._startAction.TryStart();
			while (this._startAction.running)
			{
				yield return null;
			}
			yield return this.CCombat(controller);
			yield break;
		}

		// Token: 0x0600638C RID: 25484 RVA: 0x001216CA File Offset: 0x0011F8CA
		private IEnumerator CCombat(AIController controller)
		{
			Character character = controller.character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.CheckHit));
			this._attackAction.TryStart();
			while (this._attackAction.running)
			{
				yield return null;
			}
			Character character2 = controller.character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(this.CheckHit));
			if (this._canUseSlashCombo)
			{
				yield return this._throwing.CRun(controller);
			}
			else
			{
				this._grapFailAction.TryStart();
				while (this._grapFailAction.running)
				{
					yield return null;
				}
			}
			this._canUseSlashCombo = false;
			yield return this._skipableIdle.CRun(controller);
			yield break;
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x001216E0 File Offset: 0x0011F8E0
		private void CheckHit(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			this._canUseSlashCombo = true;
		}

		// Token: 0x04005043 RID: 20547
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _startAction;

		// Token: 0x04005044 RID: 20548
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04005045 RID: 20549
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _grapFailAction;

		// Token: 0x04005046 RID: 20550
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(SkipableIdle))]
		private SkipableIdle _skipableIdle;

		// Token: 0x04005047 RID: 20551
		[UnityEditor.Subcomponent(typeof(Throwing))]
		[SerializeField]
		private Throwing _throwing;

		// Token: 0x04005048 RID: 20552
		private bool _canUseSlashCombo;
	}
}
