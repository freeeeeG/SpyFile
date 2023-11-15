using System;
using System.Collections;
using Runnables;
using UnityEditor;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000128 RID: 296
	public class SkulStory : Runnable
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x00011081 File Offset: 0x0000F281
		public override void Run()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00011090 File Offset: 0x0000F290
		private IEnumerator CRun()
		{
			this._onStart.Run();
			yield return this._sequence.CRun();
			this._onEnd.Run();
			yield break;
		}

		// Token: 0x0400045E RID: 1118
		[SerializeField]
		[Event.SubcomponentAttribute]
		private Event.Subcomponents _onStart;

		// Token: 0x0400045F RID: 1119
		[UnityEditor.Subcomponent(typeof(SequenceInfo))]
		[SerializeField]
		private SequenceInfo.Subcomponents _sequence;

		// Token: 0x04000460 RID: 1120
		[Event.SubcomponentAttribute]
		[SerializeField]
		private Event.Subcomponents _onEnd;
	}
}
