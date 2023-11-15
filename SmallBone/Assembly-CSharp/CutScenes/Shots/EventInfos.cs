using System;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001C1 RID: 449
	public sealed class EventInfos : Shot
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x0001B029 File Offset: 0x00019229
		public override void Run()
		{
			this._events.Run();
			if (this._next != null)
			{
				this._next.Run();
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001B04F File Offset: 0x0001924F
		public override void SetNext(Shot next)
		{
			this._next = next;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x040007CD RID: 1997
		[Event.SubcomponentAttribute]
		[SerializeField]
		private Event.Subcomponents _events;

		// Token: 0x040007CE RID: 1998
		private Shot _next;
	}
}
