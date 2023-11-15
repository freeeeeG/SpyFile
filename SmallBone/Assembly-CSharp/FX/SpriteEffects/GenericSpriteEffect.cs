using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x0200026D RID: 621
	public sealed class GenericSpriteEffect : SpriteEffect
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x0002166C File Offset: 0x0001F86C
		public GenericSpriteEffect(int priority, float duration, float speed, GenericSpriteEffect.ColorOverlay colorOverlay, GenericSpriteEffect.ColorBlend colorBlend, GenericSpriteEffect.Outline outline, GenericSpriteEffect.GrayScale grayScale) : base(priority)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._duration = duration;
			this._speed = speed;
			this._colorOverlay = colorOverlay;
			this._colorBlend = colorBlend;
			this._outline = outline;
			this._grayScale = grayScale;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000216B9 File Offset: 0x0001F8B9
		internal void Reset()
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._time = 0f;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000216D4 File Offset: 0x0001F8D4
		internal override void Apply(Renderer renderer)
		{
			renderer.GetPropertyBlock(this._propertyBlock);
			this._colorOverlay.Apply(renderer, this._propertyBlock, this._time);
			this._colorBlend.Apply(renderer, this._propertyBlock, this._time);
			this._outline.Apply(renderer, this._propertyBlock, this._time);
			this._grayScale.Apply(renderer, this._propertyBlock, this._time);
			renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00021759 File Offset: 0x0001F959
		internal override bool Update(float deltaTime)
		{
			this._time += deltaTime * this._speed;
			return this._time < this._duration;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0002177E File Offset: 0x0001F97E
		internal override void Expire()
		{
			this._time = this._duration;
		}

		// Token: 0x04000A25 RID: 2597
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x04000A26 RID: 2598
		private readonly GenericSpriteEffect.ColorOverlay _colorOverlay;

		// Token: 0x04000A27 RID: 2599
		private readonly GenericSpriteEffect.ColorBlend _colorBlend;

		// Token: 0x04000A28 RID: 2600
		private readonly GenericSpriteEffect.Outline _outline;

		// Token: 0x04000A29 RID: 2601
		private readonly GenericSpriteEffect.GrayScale _grayScale;

		// Token: 0x04000A2A RID: 2602
		private readonly float _duration;

		// Token: 0x04000A2B RID: 2603
		private readonly float _speed;

		// Token: 0x04000A2C RID: 2604
		private float _time;

		// Token: 0x0200026E RID: 622
		[Serializable]
		public class ColorOverlay
		{
			// Token: 0x1700029A RID: 666
			// (get) Token: 0x06000C21 RID: 3105 RVA: 0x0002178C File Offset: 0x0001F98C
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
			}

			// Token: 0x1700029B RID: 667
			// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00021794 File Offset: 0x0001F994
			public Color startColor
			{
				get
				{
					return this._startColor;
				}
			}

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0002179C File Offset: 0x0001F99C
			public Color endColor
			{
				get
				{
					return this._endColor;
				}
			}

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x06000C24 RID: 3108 RVA: 0x000217A4 File Offset: 0x0001F9A4
			public Curve curve
			{
				get
				{
					return this._curve;
				}
			}

			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000C25 RID: 3109 RVA: 0x000217AC File Offset: 0x0001F9AC
			public float duration
			{
				get
				{
					return this._curve.duration;
				}
			}

			// Token: 0x06000C26 RID: 3110 RVA: 0x000217B9 File Offset: 0x0001F9B9
			internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
			{
				if (!this._enabled)
				{
					return;
				}
				propertyBlock.SetColor(SpriteEffect._overlayColor, Color.LerpUnclamped(this._startColor, this._endColor, this._curve.Evaluate(time)));
			}

			// Token: 0x04000A2D RID: 2605
			[SerializeField]
			private bool _enabled;

			// Token: 0x04000A2E RID: 2606
			[SerializeField]
			private Color _startColor = Color.white;

			// Token: 0x04000A2F RID: 2607
			[SerializeField]
			private Color _endColor = new Color(1f, 1f, 1f, 0f);

			// Token: 0x04000A30 RID: 2608
			[SerializeField]
			private Curve _curve;
		}

		// Token: 0x0200026F RID: 623
		[Serializable]
		public class ColorBlend
		{
			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0002181E File Offset: 0x0001FA1E
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
			}

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00021826 File Offset: 0x0001FA26
			public Color startColor
			{
				get
				{
					return this._startColor;
				}
			}

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0002182E File Offset: 0x0001FA2E
			public Color endColor
			{
				get
				{
					return this._endColor;
				}
			}

			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00021836 File Offset: 0x0001FA36
			public Curve curve
			{
				get
				{
					return this._curve;
				}
			}

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0002183E File Offset: 0x0001FA3E
			public float duration
			{
				get
				{
					return this._curve.duration;
				}
			}

			// Token: 0x06000C2D RID: 3117 RVA: 0x0002184B File Offset: 0x0001FA4B
			internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
			{
				if (!this._enabled)
				{
					return;
				}
				propertyBlock.SetColor(SpriteEffect._baseColor, Color.LerpUnclamped(this._startColor, this._endColor, this._curve.Evaluate(time)));
			}

			// Token: 0x04000A31 RID: 2609
			[SerializeField]
			private bool _enabled;

			// Token: 0x04000A32 RID: 2610
			[SerializeField]
			private Color _startColor = Color.white;

			// Token: 0x04000A33 RID: 2611
			[SerializeField]
			private Color _endColor = new Color(1f, 1f, 1f, 0f);

			// Token: 0x04000A34 RID: 2612
			[SerializeField]
			private Curve _curve;
		}

		// Token: 0x02000270 RID: 624
		[Serializable]
		public class Outline
		{
			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06000C2F RID: 3119 RVA: 0x000218B0 File Offset: 0x0001FAB0
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
			}

			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x06000C30 RID: 3120 RVA: 0x000218B8 File Offset: 0x0001FAB8
			public Color color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x06000C31 RID: 3121 RVA: 0x000218C0 File Offset: 0x0001FAC0
			public float brightness
			{
				get
				{
					return this._brightness;
				}
			}

			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000218C8 File Offset: 0x0001FAC8
			public int width
			{
				get
				{
					return this._width;
				}
			}

			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x06000C33 RID: 3123 RVA: 0x000218D0 File Offset: 0x0001FAD0
			public float duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x06000C34 RID: 3124 RVA: 0x000218D8 File Offset: 0x0001FAD8
			internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
			{
				if (!this._enabled)
				{
					return;
				}
				propertyBlock.SetFloat(SpriteEffect._outlineEnabled, 1f);
				Color color = this._color * this._brightness;
				color.a = this._color.a;
				if (this._colorChange)
				{
					propertyBlock.SetColor(SpriteEffect._outlineColor, Color.LerpUnclamped(color, this._endColor, time / this.duration));
				}
				else
				{
					propertyBlock.SetColor(SpriteEffect._outlineColor, color);
				}
				propertyBlock.SetFloat(SpriteEffect._outlineSize, (float)this._width);
				propertyBlock.SetFloat(SpriteEffect._alphaThreshold, 0.01f);
			}

			// Token: 0x04000A35 RID: 2613
			[SerializeField]
			private bool _enabled;

			// Token: 0x04000A36 RID: 2614
			[SerializeField]
			private Color _color = Color.white;

			// Token: 0x04000A37 RID: 2615
			[SerializeField]
			private bool _colorChange;

			// Token: 0x04000A38 RID: 2616
			[SerializeField]
			private Color _endColor;

			// Token: 0x04000A39 RID: 2617
			[SerializeField]
			[Range(1f, 10f)]
			private float _brightness = 2f;

			// Token: 0x04000A3A RID: 2618
			[Range(1f, 6f)]
			[SerializeField]
			private int _width = 1;

			// Token: 0x04000A3B RID: 2619
			[FrameTime]
			[SerializeField]
			private float _duration;
		}

		// Token: 0x02000271 RID: 625
		[Serializable]
		public class GrayScale
		{
			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0002199E File Offset: 0x0001FB9E
			public bool enabled
			{
				get
				{
					return this._enabled;
				}
			}

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x06000C37 RID: 3127 RVA: 0x000219A6 File Offset: 0x0001FBA6
			public float startAmount
			{
				get
				{
					return this._startAmount;
				}
			}

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06000C38 RID: 3128 RVA: 0x000219AE File Offset: 0x0001FBAE
			public float endAmount
			{
				get
				{
					return this._endAmount;
				}
			}

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06000C39 RID: 3129 RVA: 0x000219B6 File Offset: 0x0001FBB6
			public Curve curve
			{
				get
				{
					return this._curve;
				}
			}

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06000C3A RID: 3130 RVA: 0x000219BE File Offset: 0x0001FBBE
			public float duration
			{
				get
				{
					return this._curve.duration;
				}
			}

			// Token: 0x06000C3B RID: 3131 RVA: 0x000219CB File Offset: 0x0001FBCB
			internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
			{
				if (!this._enabled)
				{
					return;
				}
				propertyBlock.SetFloat(SpriteEffect._grayScaleLerp, Mathf.Lerp(this._startAmount, this._endAmount, this._curve.Evaluate(time)));
			}

			// Token: 0x04000A3C RID: 2620
			[SerializeField]
			private bool _enabled;

			// Token: 0x04000A3D RID: 2621
			[SerializeField]
			[Range(0f, 1f)]
			private float _startAmount;

			// Token: 0x04000A3E RID: 2622
			[SerializeField]
			[Range(0f, 1f)]
			private float _endAmount;

			// Token: 0x04000A3F RID: 2623
			[SerializeField]
			private Curve _curve;
		}
	}
}
