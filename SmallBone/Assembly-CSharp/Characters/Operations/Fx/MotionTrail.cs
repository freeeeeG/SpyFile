using System;
using System.Collections;
using FX;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F10 RID: 3856
	public class MotionTrail : CharacterOperation
	{
		// Token: 0x06004B4F RID: 19279 RVA: 0x000DDA86 File Offset: 0x000DBC86
		private void Awake()
		{
			this._propertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x000DDA94 File Offset: 0x000DBC94
		public override void Run(Character owner)
		{
			foreach (CharacterAnimation characterAnimation in owner.animationController.animations)
			{
				if (characterAnimation.gameObject.activeInHierarchy)
				{
					this._cTrail = this.StartCoroutineWithReference(this.CTrail(owner, characterAnimation.spriteRenderer, owner.chronometer.animation));
				}
			}
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x000DDB18 File Offset: 0x000DBD18
		private IEnumerator CTrail(Character owner, SpriteRenderer spriteRenderer, Chronometer chronometer)
		{
			float remainTime = (this._duration == 0f) ? float.PositiveInfinity : this._duration;
			float remainInterval = 0f;
			int sortingOrderCount = 0;
			while (remainTime > 0f)
			{
				if (remainInterval <= 0f)
				{
					Effects.SpritePoolObject spritePoolObject = Effects.sprite.Spawn();
					spritePoolObject.spriteRenderer.CopyFrom(spriteRenderer);
					spritePoolObject.spriteRenderer.sortingLayerID = owner.sortingGroup.sortingLayerID;
					int num = sortingOrderCount;
					sortingOrderCount = num + 1;
					int num2 = owner.sortingGroup.sortingOrder;
					if (this._layer == MotionTrail.Layer.Front)
					{
						num2 += sortingOrderCount;
					}
					else
					{
						num2 -= sortingOrderCount;
					}
					spritePoolObject.spriteRenderer.sortingOrder = num2;
					spritePoolObject.spriteRenderer.color = Color.white;
					spritePoolObject.spriteRenderer.sharedMaterial = MaterialResource.character;
					spritePoolObject.spriteRenderer.GetPropertyBlock(this._propertyBlock);
					if (this._changeColor)
					{
						this._propertyBlock.SetColor(MotionTrail._overlayColor, this._color);
					}
					else
					{
						this._propertyBlock.SetColor(MotionTrail._overlayColor, Color.clear);
					}
					spritePoolObject.spriteRenderer.SetPropertyBlock(this._propertyBlock);
					Transform transform = spritePoolObject.poolObject.transform;
					Transform transform2 = spriteRenderer.transform;
					transform.SetPositionAndRotation(transform2.position, transform2.rotation);
					transform.localScale = transform2.lossyScale;
					transform.rotation = transform2.rotation;
					spritePoolObject.FadeOut(chronometer, this._fadeOutCurve, this._fadeOutDuration);
					remainInterval = this._interval;
				}
				yield return null;
				float num3 = chronometer.DeltaTime();
				remainInterval -= num3;
				remainTime -= num3;
			}
			yield break;
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x000DDB3C File Offset: 0x000DBD3C
		public override void Stop()
		{
			this._cTrail.Stop();
		}

		// Token: 0x04003A7E RID: 14974
		protected static readonly int _overlayColor = Shader.PropertyToID("_OverlayColor");

		// Token: 0x04003A7F RID: 14975
		protected static readonly int _outlineEnabled = Shader.PropertyToID("_IsOutlineEnabled");

		// Token: 0x04003A80 RID: 14976
		protected static readonly int _outlineColor = Shader.PropertyToID("_OutlineColor");

		// Token: 0x04003A81 RID: 14977
		protected static readonly int _outlineSize = Shader.PropertyToID("_OutlineSize");

		// Token: 0x04003A82 RID: 14978
		protected static readonly int _alphaThreshold = Shader.PropertyToID("_AlphaThreshold");

		// Token: 0x04003A83 RID: 14979
		protected const string _outsideMaterialKeyword = "SPRITE_OUTLINE_OUTSIDE";

		// Token: 0x04003A84 RID: 14980
		[SerializeField]
		private MotionTrail.Layer _layer;

		// Token: 0x04003A85 RID: 14981
		[FrameTime]
		[SerializeField]
		[Header("Time")]
		private float _duration;

		// Token: 0x04003A86 RID: 14982
		[FrameTime]
		[SerializeField]
		private float _interval;

		// Token: 0x04003A87 RID: 14983
		[SerializeField]
		[Header("Color")]
		private bool _changeColor = true;

		// Token: 0x04003A88 RID: 14984
		[SerializeField]
		private Color _color;

		// Token: 0x04003A89 RID: 14985
		[Header("Fadeout")]
		[SerializeField]
		private AnimationCurve _fadeOutCurve;

		// Token: 0x04003A8A RID: 14986
		[SerializeField]
		[FrameTime]
		private float _fadeOutDuration;

		// Token: 0x04003A8B RID: 14987
		private CoroutineReference _cTrail;

		// Token: 0x04003A8C RID: 14988
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x02000F11 RID: 3857
		public enum Layer
		{
			// Token: 0x04003A8E RID: 14990
			Behind,
			// Token: 0x04003A8F RID: 14991
			Front
		}
	}
}
