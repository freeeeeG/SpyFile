using System;
using System.Collections;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001D3 RID: 467
	public class CharacterMoveTo : Sequence
	{
		// Token: 0x060009B4 RID: 2484 RVA: 0x0001B643 File Offset: 0x00019843
		public override IEnumerator CRun()
		{
			for (;;)
			{
				float num = this._point.position.x - this._target.character.transform.position.x;
				if (Mathf.Abs(num) < this._epsilon)
				{
					break;
				}
				Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
				this._target.character.movement.move = move;
				yield return null;
			}
			yield break;
		}

		// Token: 0x040007ED RID: 2029
		[SerializeField]
		private Target _target;

		// Token: 0x040007EE RID: 2030
		[SerializeField]
		private Transform _point;

		// Token: 0x040007EF RID: 2031
		[SerializeField]
		private float _epsilon = 0.1f;
	}
}
