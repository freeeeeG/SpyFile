using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000E09 RID: 3593
	public class EmoteStep
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06006E3C RID: 28220 RVA: 0x002B66DB File Offset: 0x002B48DB
		public int Id
		{
			get
			{
				return this.anim.HashValue;
			}
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x002B66E8 File Offset: 0x002B48E8
		public HandleVector<EmoteStep.Callbacks>.Handle RegisterCallbacks(Action<GameObject> startedCb, Action<GameObject> finishedCb)
		{
			if (startedCb == null && finishedCb == null)
			{
				return HandleVector<EmoteStep.Callbacks>.InvalidHandle;
			}
			EmoteStep.Callbacks item = new EmoteStep.Callbacks
			{
				StartedCb = startedCb,
				FinishedCb = finishedCb
			};
			return this.callbacks.Add(item);
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x002B6727 File Offset: 0x002B4927
		public void UnregisterCallbacks(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle)
		{
			this.callbacks.Release(callbackHandle);
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x002B6736 File Offset: 0x002B4936
		public void UnregisterAllCallbacks()
		{
			this.callbacks = new HandleVector<EmoteStep.Callbacks>(64);
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x002B6748 File Offset: 0x002B4948
		public void OnStepStarted(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.StartedCb != null)
			{
				item.StartedCb(parameter);
			}
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x002B6784 File Offset: 0x002B4984
		public void OnStepFinished(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.FinishedCb != null)
			{
				item.FinishedCb(parameter);
			}
		}

		// Token: 0x04005294 RID: 21140
		public HashedString anim = HashedString.Invalid;

		// Token: 0x04005295 RID: 21141
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;

		// Token: 0x04005296 RID: 21142
		public float timeout = -1f;

		// Token: 0x04005297 RID: 21143
		private HandleVector<EmoteStep.Callbacks> callbacks = new HandleVector<EmoteStep.Callbacks>(64);

		// Token: 0x02001F8A RID: 8074
		public struct Callbacks
		{
			// Token: 0x04008E6C RID: 36460
			public Action<GameObject> StartedCb;

			// Token: 0x04008E6D RID: 36461
			public Action<GameObject> FinishedCb;
		}
	}
}
