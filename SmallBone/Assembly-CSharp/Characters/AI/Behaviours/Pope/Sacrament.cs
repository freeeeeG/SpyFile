using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Behaviours.Attacks;
using Characters.AI.Pope;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001369 RID: 4969
	public sealed class Sacrament : Behaviour
	{
		// Token: 0x060061E1 RID: 25057 RVA: 0x0011DF1F File Offset: 0x0011C11F
		private void Start()
		{
			this._sacramentOrbPool.Initialize(this._character);
			this._character.health.onDied += this._sacramentOrbPool.Hide;
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x0011DF53 File Offset: 0x0011C153
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			this._sacramentOrbPool.Run();
			yield return this._attack.CRun(controller);
			this._sacramentOrbPool.Hide();
			this._endMotion.TryStart();
			while (this._endMotion.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004EF3 RID: 20211
		[SerializeField]
		private Character _character;

		// Token: 0x04004EF4 RID: 20212
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004EF5 RID: 20213
		[SerializeField]
		private Characters.Actions.Action _endMotion;

		// Token: 0x04004EF6 RID: 20214
		[SerializeField]
		private SacramentOrbPool _sacramentOrbPool;

		// Token: 0x04004EF7 RID: 20215
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		[SerializeField]
		private MoveHandler _moveHandler;
	}
}
