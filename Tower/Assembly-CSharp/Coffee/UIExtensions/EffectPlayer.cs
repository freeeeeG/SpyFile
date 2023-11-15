using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIExtensions
{
	// Token: 0x020000EB RID: 235
	[Serializable]
	public class EffectPlayer
	{
		// Token: 0x0600036B RID: 875 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		public void OnEnable(Action<float> callback = null)
		{
			if (EffectPlayer.s_UpdateActions == null)
			{
				EffectPlayer.s_UpdateActions = new List<Action>();
				Canvas.willRenderCanvases += delegate()
				{
					int count = EffectPlayer.s_UpdateActions.Count;
					for (int i = 0; i < count; i++)
					{
						EffectPlayer.s_UpdateActions[i]();
					}
				};
			}
			EffectPlayer.s_UpdateActions.Add(new Action(this.OnWillRenderCanvases));
			this._time = 0f;
			this._callback = callback;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000F53A File Offset: 0x0000D73A
		public void OnDisable()
		{
			this._callback = null;
			EffectPlayer.s_UpdateActions.Remove(new Action(this.OnWillRenderCanvases));
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000F55A File Offset: 0x0000D75A
		public void Play(Action<float> callback = null)
		{
			this._time = 0f;
			this.play = true;
			if (callback != null)
			{
				this._callback = callback;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000F578 File Offset: 0x0000D778
		public void Stop()
		{
			this.play = false;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000F584 File Offset: 0x0000D784
		private void OnWillRenderCanvases()
		{
			if (!this.play || !Application.isPlaying || this._callback == null)
			{
				return;
			}
			this._time += ((this.updateMode == AnimatorUpdateMode.UnscaledTime) ? Time.unscaledDeltaTime : Time.deltaTime);
			float obj = this._time / this.duration;
			if (this.duration <= this._time)
			{
				this.play = this.loop;
				this._time = (this.loop ? (-this.loopDelay) : 0f);
			}
			this._callback(obj);
		}

		// Token: 0x04000343 RID: 835
		[Tooltip("Playing.")]
		public bool play;

		// Token: 0x04000344 RID: 836
		[Tooltip("Loop.")]
		public bool loop;

		// Token: 0x04000345 RID: 837
		[Tooltip("Duration.")]
		[Range(0.01f, 10f)]
		public float duration = 1f;

		// Token: 0x04000346 RID: 838
		[Tooltip("Delay before looping.")]
		[Range(0f, 10f)]
		public float loopDelay;

		// Token: 0x04000347 RID: 839
		[Tooltip("Update mode")]
		public AnimatorUpdateMode updateMode;

		// Token: 0x04000348 RID: 840
		private static List<Action> s_UpdateActions;

		// Token: 0x04000349 RID: 841
		private float _time;

		// Token: 0x0400034A RID: 842
		private Action<float> _callback;
	}
}
