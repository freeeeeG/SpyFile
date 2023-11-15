using System;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000212 RID: 530
	public sealed class RunSequences : Event
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x0001CDED File Offset: 0x0001AFED
		public override void Run()
		{
			base.StartCoroutine(this._sequences.CRun());
		}

		// Token: 0x040008A3 RID: 2211
		[Sequence.SubcomponentAttribute]
		[SerializeField]
		private Sequence.Subcomponents _sequences;
	}
}
