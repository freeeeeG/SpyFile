using System;
using System.Collections;
using Characters;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001D1 RID: 465
	public class CharacterLootAt : Sequence
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x0001B590 File Offset: 0x00019790
		public override IEnumerator CRun()
		{
			Character.LookingDirection lookingDirection = (this._point.position.x - this._target.character.transform.position.x > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left;
			this._target.character.lookingDirection = lookingDirection;
			yield return null;
			yield break;
		}

		// Token: 0x040007E8 RID: 2024
		[SerializeField]
		private Runnables.Target _target;

		// Token: 0x040007E9 RID: 2025
		[SerializeField]
		private Transform _point;
	}
}
