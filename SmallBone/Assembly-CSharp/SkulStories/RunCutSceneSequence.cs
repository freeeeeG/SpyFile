using System;
using System.Collections;
using CutScenes.Shots;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000114 RID: 276
	public class RunCutSceneSequence : Sequence
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x000109B5 File Offset: 0x0000EBB5
		public override IEnumerator CRun()
		{
			yield return this._sequences.CRun();
			yield break;
		}

		// Token: 0x04000428 RID: 1064
		[Sequence.SubcomponentAttribute]
		[SerializeField]
		private Sequence.Subcomponents _sequences;
	}
}
