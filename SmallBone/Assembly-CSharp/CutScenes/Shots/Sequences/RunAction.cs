using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001E5 RID: 485
	public sealed class RunAction : Sequence
	{
		// Token: 0x06000A09 RID: 2569 RVA: 0x0001BFBC File Offset: 0x0001A1BC
		public override IEnumerator CRun()
		{
			if (this._action == null)
			{
				yield break;
			}
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000831 RID: 2097
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
