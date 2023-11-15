using System;
using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200103C RID: 4156
	public class PatrolCat : AIController
	{
		// Token: 0x0600502A RID: 20522 RVA: 0x000F1B4C File Offset: 0x000EFD4C
		protected override void OnEnable()
		{
			if (this._justIdle)
			{
				return;
			}
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x000F1B64 File Offset: 0x000EFD64
		protected override IEnumerator CProcess()
		{
			for (;;)
			{
				yield return this._idle.CRun(this);
				this.SetDestination();
				yield return this._moveToDestination.CRun(this);
			}
			yield break;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x000F1B74 File Offset: 0x000EFD74
		private void SetDestination()
		{
			float num = UnityEngine.Random.Range(this._distanceRange.x, this._distanceRange.y);
			int num2 = MMMaths.RandomBool() ? 1 : -1;
			if (num2 == 1)
			{
				if (this.character.transform.position.x + (float)num2 * num >= this._maxPoint.position.x)
				{
					num2 *= -1;
				}
			}
			else if (num2 == -1 && this.character.transform.position.x + (float)num2 * num <= this._minPoint.position.x)
			{
				num2 *= -1;
			}
			base.destination = new Vector2(this.character.transform.position.x + (float)num2 * num, this.character.transform.position.y);
		}

		// Token: 0x04004077 RID: 16503
		[SerializeField]
		private bool _justIdle;

		// Token: 0x04004078 RID: 16504
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x04004079 RID: 16505
		[Subcomponent(typeof(MoveToDestination))]
		[SerializeField]
		private MoveToDestination _moveToDestination;

		// Token: 0x0400407A RID: 16506
		[SerializeField]
		[MinMaxSlider(5f, 30f)]
		private Vector2 _distanceRange;

		// Token: 0x0400407B RID: 16507
		[SerializeField]
		private Transform _minPoint;

		// Token: 0x0400407C RID: 16508
		[SerializeField]
		private Transform _maxPoint;
	}
}
