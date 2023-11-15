using System;
using System.Collections;
using Characters.AI.Pope;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200136C RID: 4972
	public class MoveHandler : MonoBehaviour
	{
		// Token: 0x060061EC RID: 25068 RVA: 0x0011E05F File Offset: 0x0011C25F
		public IEnumerator CMove(AIController controller)
		{
			if (MMMaths.Chance(this._chance))
			{
				this._move.SetDestination(this._destination);
				yield return this._move.CRun(controller);
			}
			yield break;
		}

		// Token: 0x04004EFC RID: 20220
		[SerializeField]
		private Point.Tag _destination;

		// Token: 0x04004EFD RID: 20221
		[SerializeField]
		[Range(0f, 1f)]
		private float _chance = 1f;

		// Token: 0x04004EFE RID: 20222
		[SerializeField]
		private Move _move;
	}
}
