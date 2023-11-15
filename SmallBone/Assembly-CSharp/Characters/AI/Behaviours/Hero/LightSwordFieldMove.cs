using System;
using System.Collections;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x0200139D RID: 5021
	public sealed class LightSwordFieldMove : LightMove
	{
		// Token: 0x06006313 RID: 25363 RVA: 0x0012073D File Offset: 0x0011E93D
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._helper.swords == null)
			{
				yield return this._helper.CFire();
			}
			yield return base.CRun(controller);
			yield break;
		}

		// Token: 0x06006314 RID: 25364 RVA: 0x00120754 File Offset: 0x0011E954
		protected override LightSword GetDestination()
		{
			LightSwordFieldMove.Where where = this._where;
			if (where == LightSwordFieldMove.Where.PlayerBehind)
			{
				return this._helper.GetBehindPlayer();
			}
			if (where != LightSwordFieldMove.Where.PlayerClosest)
			{
				return this._helper.GetClosestFromPlayer();
			}
			return this._helper.GetClosestFromPlayer();
		}

		// Token: 0x04004FE6 RID: 20454
		[SerializeField]
		private LightSwordFieldHelper _helper;

		// Token: 0x04004FE7 RID: 20455
		[SerializeField]
		private LightSwordFieldMove.Where _where;

		// Token: 0x0200139E RID: 5022
		private enum Where
		{
			// Token: 0x04004FE9 RID: 20457
			PlayerBehind,
			// Token: 0x04004FEA RID: 20458
			PlayerClosest
		}
	}
}
