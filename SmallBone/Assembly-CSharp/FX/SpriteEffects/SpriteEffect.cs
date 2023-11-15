using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x02000274 RID: 628
	public abstract class SpriteEffect
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x00021BFD File Offset: 0x0001FDFD
		public SpriteEffect(int priority)
		{
			this.priority = priority;
		}

		// Token: 0x06000C47 RID: 3143
		internal abstract void Apply(Renderer renderer);

		// Token: 0x06000C48 RID: 3144
		internal abstract bool Update(float deltaTime);

		// Token: 0x06000C49 RID: 3145
		internal abstract void Expire();

		// Token: 0x04000A4B RID: 2635
		public static readonly SpriteEffect.Default @default = SpriteEffect.Default.instance;

		// Token: 0x04000A4C RID: 2636
		protected static readonly int _baseColor = Shader.PropertyToID("_BaseColor");

		// Token: 0x04000A4D RID: 2637
		protected static readonly int _overlayColor = Shader.PropertyToID("_OverlayColor");

		// Token: 0x04000A4E RID: 2638
		protected static readonly int _outlineEnabled = Shader.PropertyToID("_IsOutlineEnabled");

		// Token: 0x04000A4F RID: 2639
		protected static readonly int _outlineColor = Shader.PropertyToID("_OutlineColor");

		// Token: 0x04000A50 RID: 2640
		protected static readonly int _outlineSize = Shader.PropertyToID("_OutlineSize");

		// Token: 0x04000A51 RID: 2641
		protected static readonly int _alphaThreshold = Shader.PropertyToID("_AlphaThreshold");

		// Token: 0x04000A52 RID: 2642
		protected static readonly int _grayScaleLerp = Shader.PropertyToID("_GrayScaleLerp");

		// Token: 0x04000A53 RID: 2643
		protected const string _outsideMaterialKeyword = "SPRITE_OUTLINE_OUTSIDE";

		// Token: 0x04000A54 RID: 2644
		public readonly int priority;

		// Token: 0x02000275 RID: 629
		public class Default : SpriteEffect
		{
			// Token: 0x06000C4B RID: 3147 RVA: 0x00021C8C File Offset: 0x0001FE8C
			private Default() : base(0)
			{
				this._propertyBlock = new MaterialPropertyBlock();
			}

			// Token: 0x06000C4C RID: 3148 RVA: 0x00021CA0 File Offset: 0x0001FEA0
			internal override void Apply(Renderer renderer)
			{
				renderer.GetPropertyBlock(this._propertyBlock);
				this._propertyBlock.SetColor(SpriteEffect._baseColor, Color.white);
				this._propertyBlock.SetColor(SpriteEffect._overlayColor, Color.clear);
				this._propertyBlock.SetFloat(SpriteEffect._outlineEnabled, 0f);
				this._propertyBlock.SetFloat(SpriteEffect._grayScaleLerp, 0f);
				renderer.SetPropertyBlock(this._propertyBlock);
			}

			// Token: 0x06000C4D RID: 3149 RVA: 0x00002191 File Offset: 0x00000391
			internal override void Expire()
			{
			}

			// Token: 0x06000C4E RID: 3150 RVA: 0x000076D4 File Offset: 0x000058D4
			internal override bool Update(float deltaTime)
			{
				return true;
			}

			// Token: 0x04000A55 RID: 2645
			public static readonly SpriteEffect.Default instance = new SpriteEffect.Default();

			// Token: 0x04000A56 RID: 2646
			private readonly MaterialPropertyBlock _propertyBlock;
		}
	}
}
