using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace FX
{
	// Token: 0x02000251 RID: 593
	public class ScreenFlash : MonoBehaviour
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x000200CE File Offset: 0x0001E2CE
		public SpriteRenderer spriteRenderer
		{
			get
			{
				return this._spriteRenderer;
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x000200D8 File Offset: 0x0001E2D8
		private void Awake()
		{
			this._pixelPerUnit = this._spriteRenderer.sprite.pixelsPerUnit;
			this._width = this._spriteRenderer.sprite.rect.width;
			this._height = this._spriteRenderer.sprite.rect.height;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00020138 File Offset: 0x0001E338
		private void FullScreen()
		{
			Vector3 zero = Vector3.zero;
			zero.z = 10f;
			base.transform.localPosition = zero;
			Camera camera = Scene<GameBase>.instance.camera;
			float num = camera.orthographicSize * 2f;
			Vector2 vector = new Vector2(camera.aspect * num, num);
			Vector2 vector2 = Vector2.one;
			if (vector.x >= vector.y)
			{
				vector2 *= vector.x * this._pixelPerUnit / this._width;
			}
			else
			{
				vector2 *= vector.y * this._pixelPerUnit / this._height;
			}
			base.transform.localScale = vector2;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000201F0 File Offset: 0x0001E3F0
		public void Play(ScreenFlash.Info info)
		{
			this.FullScreen();
			this._info = info;
			this._fadedPercent = 0f;
			this._spriteRenderer.sortingLayerID = info.sortingLayer;
			if (info.sortingOrder == ScreenFlash.Info.SortingOrder.Frontmost)
			{
				this._spriteRenderer.sortingOrder = 32767;
			}
			else
			{
				this._spriteRenderer.sortingOrder = -32768;
			}
			this._spriteRenderer.color = info.color;
			base.StartCoroutine(this.CPlay());
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002026E File Offset: 0x0001E46E
		private IEnumerator CPlay()
		{
			yield return this.CFadeIn(this._info.fadeIn, this._info.fadeInDuration);
			yield return Chronometer.global.WaitForSeconds(this._info.duration);
			yield return this.CFadeOut(this._info.fadeOut, this._info.fadeOutDuration);
			yield break;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002027D File Offset: 0x0001E47D
		public void FadeOut()
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.CFadeOut(this._info.fadeOut, this._info.fadeOutDuration));
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x000202A8 File Offset: 0x0001E4A8
		private IEnumerator CFadeIn(AnimationCurve curve, float duration)
		{
			Color color = this._spriteRenderer.color;
			float alpha = this._info.color.a;
			if (duration > 0f)
			{
				while (this._fadedPercent < 1f)
				{
					color.a = alpha * Mathf.LerpUnclamped(0f, 1f, curve.Evaluate(this._fadedPercent));
					this._spriteRenderer.color = color;
					yield return null;
					this._fadedPercent += Chronometer.global.deltaTime / duration;
				}
			}
			this._fadedPercent = 1f;
			color.a = alpha;
			this._spriteRenderer.color = color;
			yield break;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000202C5 File Offset: 0x0001E4C5
		private IEnumerator CFadeOut(AnimationCurve curve, float duration)
		{
			Color color = this._spriteRenderer.color;
			float alpha = this._info.color.a;
			if (duration > 0f)
			{
				while (this._fadedPercent > 0f)
				{
					color.a = alpha * Mathf.LerpUnclamped(0f, 1f, curve.Evaluate(this._fadedPercent));
					this._spriteRenderer.color = color;
					yield return null;
					this._fadedPercent -= Chronometer.global.deltaTime / duration;
				}
			}
			this._fadedPercent = 0f;
			this._poolObject.Despawn();
			yield break;
		}

		// Token: 0x040009A7 RID: 2471
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x040009A8 RID: 2472
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040009A9 RID: 2473
		private float _pixelPerUnit;

		// Token: 0x040009AA RID: 2474
		private float _width;

		// Token: 0x040009AB RID: 2475
		private float _height;

		// Token: 0x040009AC RID: 2476
		private ScreenFlash.Info _info;

		// Token: 0x040009AD RID: 2477
		private float _fadedPercent;

		// Token: 0x02000252 RID: 594
		[Serializable]
		public class Info
		{
			// Token: 0x1700027C RID: 636
			// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x000202E2 File Offset: 0x0001E4E2
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x1700027D RID: 637
			// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x000202EA File Offset: 0x0001E4EA
			public float duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x1700027E RID: 638
			// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x000202F2 File Offset: 0x0001E4F2
			public int sortingLayer
			{
				get
				{
					return this._sortingLayer;
				}
			}

			// Token: 0x1700027F RID: 639
			// (get) Token: 0x06000BAA RID: 2986 RVA: 0x000202FA File Offset: 0x0001E4FA
			public ScreenFlash.Info.SortingOrder sortingOrder
			{
				get
				{
					return this._sortingOrder;
				}
			}

			// Token: 0x17000280 RID: 640
			// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00020302 File Offset: 0x0001E502
			public AnimationCurve fadeIn
			{
				get
				{
					return this._fadeIn;
				}
			}

			// Token: 0x17000281 RID: 641
			// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0002030A File Offset: 0x0001E50A
			public float fadeInDuration
			{
				get
				{
					return this._fadeInDuration;
				}
			}

			// Token: 0x17000282 RID: 642
			// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00020312 File Offset: 0x0001E512
			public AnimationCurve fadeOut
			{
				get
				{
					return this._fadeOut;
				}
			}

			// Token: 0x17000283 RID: 643
			// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0002031A File Offset: 0x0001E51A
			public float fadeOutDuration
			{
				get
				{
					return this._fadeOutDuration;
				}
			}

			// Token: 0x040009AE RID: 2478
			[SerializeField]
			private Color _color = Color.black;

			// Token: 0x040009AF RID: 2479
			[Tooltip("페이드인이 완료된 후 지속될 시간")]
			[SerializeField]
			private float _duration;

			// Token: 0x040009B0 RID: 2480
			[SerializeField]
			[SortingLayer]
			[Space]
			private int _sortingLayer;

			// Token: 0x040009B1 RID: 2481
			[SerializeField]
			private ScreenFlash.Info.SortingOrder _sortingOrder;

			// Token: 0x040009B2 RID: 2482
			[Space]
			[SerializeField]
			private AnimationCurve _fadeIn;

			// Token: 0x040009B3 RID: 2483
			[SerializeField]
			private float _fadeInDuration;

			// Token: 0x040009B4 RID: 2484
			[Space]
			[SerializeField]
			private AnimationCurve _fadeOut;

			// Token: 0x040009B5 RID: 2485
			[SerializeField]
			private float _fadeOutDuration;

			// Token: 0x02000253 RID: 595
			public enum SortingOrder
			{
				// Token: 0x040009B7 RID: 2487
				Frontmost,
				// Token: 0x040009B8 RID: 2488
				Rearmost
			}
		}
	}
}
