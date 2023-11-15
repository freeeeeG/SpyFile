using System;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x020001FE RID: 510
	public class CancelAction : Event
	{
		// Token: 0x06000A69 RID: 2665 RVA: 0x0001C9C5 File Offset: 0x0001ABC5
		public override void Run()
		{
			this._target.character.CancelAction();
		}

		// Token: 0x0400087E RID: 2174
		[SerializeField]
		private Target _target;
	}
}
