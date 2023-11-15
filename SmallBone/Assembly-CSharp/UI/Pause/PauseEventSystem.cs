using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Pause
{
	// Token: 0x02000422 RID: 1058
	public class PauseEventSystem : MonoBehaviour
	{
		// Token: 0x06001408 RID: 5128 RVA: 0x0003D3AE File Offset: 0x0003B5AE
		private void Awake()
		{
			this._events = new Stack<PauseEvent>();
			this._empty = base.gameObject.AddComponent<Empty>();
			this._events.Push(this._baseEvent);
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0003D3DD File Offset: 0x0003B5DD
		public void Run()
		{
			if (this._events.Count == 0)
			{
				Debug.LogError("Panel이 없습니다.");
				return;
			}
			this._events.Peek().Invoke();
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0003D407 File Offset: 0x0003B607
		public void PushEvent(PauseEvent type)
		{
			this._events.Push(type);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0003D415 File Offset: 0x0003B615
		public void PopEvent()
		{
			this._events.Pop();
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0003D423 File Offset: 0x0003B623
		public void PushEmpty()
		{
			this.PushEvent(this._empty);
		}

		// Token: 0x04001103 RID: 4355
		[PauseEvent.SubcomponentAttribute]
		[SerializeField]
		private PauseEvent _baseEvent;

		// Token: 0x04001104 RID: 4356
		private PauseEvent _empty;

		// Token: 0x04001105 RID: 4357
		private Stack<PauseEvent> _events;
	}
}
