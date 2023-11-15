using System;
using System.Collections;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001C7 RID: 455
	public sealed class SequenceInfos : Shot
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x0001B22D File Offset: 0x0001942D
		public override void Run()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001B23C File Offset: 0x0001943C
		private IEnumerator CRun()
		{
			yield return this._sequences.CRun();
			if (this._next != null)
			{
				this._next.Run();
			}
			yield break;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0001B24B File Offset: 0x0001944B
		public override void SetNext(Shot next)
		{
			this._next = next;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x040007D6 RID: 2006
		[Sequence.SubcomponentAttribute]
		[SerializeField]
		private Sequence.Subcomponents _sequences;

		// Token: 0x040007D7 RID: 2007
		private Shot _next;
	}
}
