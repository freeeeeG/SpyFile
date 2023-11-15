using System;
using UnityEngine;

namespace RefiTools
{
	// Token: 0x020001B5 RID: 437
	public abstract class TweenBase : MonoBehaviour
	{
		// Token: 0x06000B9A RID: 2970 RVA: 0x0002DBB7 File Offset: 0x0002BDB7
		protected virtual void Start()
		{
			if (this.isStartAtRandomTime)
			{
				this.tweenT = Random.Range(0f, this.duration);
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002DBD7 File Offset: 0x0002BDD7
		protected virtual void Reset()
		{
			this.curve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002DC18 File Offset: 0x0002BE18
		protected virtual void OnValidate()
		{
			if (this.duration < 0f)
			{
				this.duration = 0f;
			}
			if (this.curve != null && this.curve.keys != null)
			{
				for (int i = 0; i < this.curve.keys.Length; i++)
				{
					this.curve.keys[i].time = Mathf.Clamp(this.curve.keys[i].time, 0f, 1f);
				}
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0002DCA8 File Offset: 0x0002BEA8
		protected void Update()
		{
			if (this.isTweenOn && this.duration > 0f)
			{
				if (this.isAffectedByTimeScale)
				{
					this.tweenT += Time.deltaTime * this.sign / this.duration;
				}
				else
				{
					this.tweenT += Time.unscaledDeltaTime * this.sign / this.duration;
				}
				if (this.tweenT > 1f || this.tweenT < 0f)
				{
					this.MakeSureTweenInRange();
				}
				this.UpdateTween();
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002DD3A File Offset: 0x0002BF3A
		public void SetDuration(float _duration)
		{
			this.duration = Mathf.Max(0f, _duration);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002DD50 File Offset: 0x0002BF50
		private void MakeSureTweenInRange()
		{
			if (this.tweenT <= 1f)
			{
				if (this.tweenT < 0f)
				{
					switch (this.loopType)
					{
					case TweenBase.eTweenLoopType.NONE:
						this.ToggleTween(false);
						this.RestartTween();
						return;
					case TweenBase.eTweenLoopType.LOOP:
						this.tweenT = 0f;
						return;
					case TweenBase.eTweenLoopType.PINGPONG:
						this.tweenT = -1f * this.tweenT;
						this.sign *= -1f;
						break;
					default:
						return;
					}
				}
				return;
			}
			switch (this.loopType)
			{
			case TweenBase.eTweenLoopType.NONE:
				this.ToggleTween(false);
				this.RestartTween();
				return;
			case TweenBase.eTweenLoopType.LOOP:
				this.tweenT -= 1f;
				return;
			case TweenBase.eTweenLoopType.PINGPONG:
				this.tweenT = 2f - this.tweenT;
				this.sign *= -1f;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000BA0 RID: 2976
		protected abstract void UpdateTween();

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002DE2F File Offset: 0x0002C02F
		public void ToggleTween(bool isOn)
		{
			if (this.isTweenOn == isOn)
			{
				return;
			}
			if (isOn && this.isStartAtRandomTime)
			{
				this.tweenT = Random.Range(0f, this.duration);
			}
			this.isTweenOn = isOn;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002DE63 File Offset: 0x0002C063
		public void RestartTween()
		{
			this.tweenT = 0f;
			this.UpdateTween();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002DE76 File Offset: 0x0002C076
		public void SetLoopType(TweenBase.eTweenLoopType _loopType)
		{
			this.loopType = _loopType;
		}

		// Token: 0x04000941 RID: 2369
		[SerializeField]
		[Header("持續時間")]
		protected float duration = 1f;

		// Token: 0x04000942 RID: 2370
		[SerializeField]
		[Header("重複播放的類型")]
		protected TweenBase.eTweenLoopType loopType;

		// Token: 0x04000943 RID: 2371
		[SerializeField]
		[Header("是否受Timescale影響")]
		protected bool isAffectedByTimeScale = true;

		// Token: 0x04000944 RID: 2372
		[SerializeField]
		[Header("動畫曲線")]
		protected AnimationCurve curve;

		// Token: 0x04000945 RID: 2373
		[SerializeField]
		[Header("是否從隨機時間點開始")]
		private bool isStartAtRandomTime;

		// Token: 0x04000946 RID: 2374
		protected float tweenT;

		// Token: 0x04000947 RID: 2375
		protected float sign = 1f;

		// Token: 0x04000948 RID: 2376
		[SerializeField]
		protected bool isTweenOn;

		// Token: 0x020002C4 RID: 708
		public enum eTweenLoopType
		{
			// Token: 0x04000CE7 RID: 3303
			NONE,
			// Token: 0x04000CE8 RID: 3304
			LOOP,
			// Token: 0x04000CE9 RID: 3305
			PINGPONG
		}
	}
}
