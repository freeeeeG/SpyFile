using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001FA RID: 506
	public sealed class InvokeUnityEventOnEnd : Sequence
	{
		// Token: 0x06000A5C RID: 2652 RVA: 0x0001C8BF File Offset: 0x0001AABF
		public override IEnumerator CRun()
		{
			yield return this._sequences.CRun();
			UnityEvent @event = this._event;
			if (@event != null)
			{
				@event.Invoke();
			}
			yield break;
		}

		// Token: 0x04000877 RID: 2167
		[Sequence.SubcomponentAttribute]
		[SerializeField]
		private Sequence.Subcomponents _sequences;

		// Token: 0x04000878 RID: 2168
		[SerializeField]
		private UnityEvent _event;
	}
}
