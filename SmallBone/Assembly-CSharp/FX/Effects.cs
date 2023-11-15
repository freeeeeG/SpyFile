using System;
using System.Collections;
using GameResources;
using UnityEngine;

namespace FX
{
	// Token: 0x02000232 RID: 562
	public static class Effects
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x0001E8AB File Offset: 0x0001CAAB
		public static short GetSortingOrderAndIncrease()
		{
			short sortingOrder = Effects._sortingOrder;
			Effects._sortingOrder = sortingOrder + 1;
			return sortingOrder;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0001E8BB File Offset: 0x0001CABB
		public static IEnumerator CFadeOut(this PoolObject poolObject, SpriteRenderer spriteRenderer, ChronometerBase chronometer, AnimationCurve curve, float duration)
		{
			float t = 0f;
			Color color = spriteRenderer.color;
			float alpha = color.a;
			float multiplier = 1f / duration;
			while (t < 1f)
			{
				yield return null;
				t += chronometer.DeltaTime() * multiplier;
				color.a = alpha * (1f - curve.Evaluate(t));
				spriteRenderer.color = color;
			}
			poolObject.Despawn();
			yield break;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0001E8E7 File Offset: 0x0001CAE7
		public static IEnumerator CFadeOut(this PoolObject poolObject, SpriteRenderer[] spriteRenderers, ChronometerBase chronometer, AnimationCurve curve, float duration)
		{
			float t = 0f;
			Color[] colorArray = new Color[spriteRenderers.Length];
			float[] alphaArray = new float[spriteRenderers.Length];
			for (int i = 0; i < colorArray.Length; i++)
			{
				if (!(spriteRenderers[i] == null))
				{
					colorArray[i] = spriteRenderers[i].color;
					alphaArray[i] = colorArray[i].a;
				}
			}
			float multiplier = 1f / duration;
			while (t < 1f)
			{
				yield return null;
				t += chronometer.DeltaTime() * multiplier;
				for (int j = 0; j < colorArray.Length; j++)
				{
					colorArray[j].a = alphaArray[j] * (1f - curve.Evaluate(t));
					spriteRenderers[j].color = colorArray[j];
				}
			}
			poolObject.Despawn();
			yield break;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0001E913 File Offset: 0x0001CB13
		public static void FadeOut(this PoolObject poolObject, SpriteRenderer spriteRenderer, ChronometerBase chronometer, AnimationCurve curve, float duration)
		{
			poolObject.StartCoroutine(poolObject.CFadeOut(spriteRenderer, chronometer, curve, duration));
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0001E928 File Offset: 0x0001CB28
		public static void CopyFrom(this SpriteRenderer spriteRenderer, SpriteRenderer from)
		{
			spriteRenderer.sprite = from.sprite;
			spriteRenderer.color = from.color;
			spriteRenderer.flipX = from.flipX;
			spriteRenderer.flipY = from.flipY;
			spriteRenderer.sharedMaterial = from.sharedMaterial;
			spriteRenderer.drawMode = from.drawMode;
			spriteRenderer.sortingLayerID = from.sortingLayerID;
			spriteRenderer.sortingOrder = from.sortingOrder;
			spriteRenderer.maskInteraction = from.maskInteraction;
			spriteRenderer.spriteSortPoint = from.spriteSortPoint;
			spriteRenderer.renderingLayerMask = from.renderingLayerMask;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0001E9BC File Offset: 0x0001CBBC
		public static void CopyFrom(this SpriteRenderer spriteRenderer, SpriteRendererValues from)
		{
			spriteRenderer.sprite = from.sprite;
			spriteRenderer.color = from.color;
			spriteRenderer.flipX = from.flipX;
			spriteRenderer.flipY = from.flipY;
			spriteRenderer.sharedMaterial = from.sharedMaterial;
			spriteRenderer.drawMode = from.drawMode;
			spriteRenderer.sortingLayerID = from.sortingLayerID;
			spriteRenderer.sortingOrder = from.sortingOrder;
			spriteRenderer.maskInteraction = from.maskInteraction;
			spriteRenderer.spriteSortPoint = from.spriteSortPoint;
			spriteRenderer.renderingLayerMask = from.renderingLayerMask;
		}

		// Token: 0x04000937 RID: 2359
		private static short _sortingOrder = short.MinValue;

		// Token: 0x04000938 RID: 2360
		public static readonly Effects.SpritePoolObject sprite = new Effects.SpritePoolObject(CommonResource.instance.emptyEffect);

		// Token: 0x02000233 RID: 563
		public struct SpritePoolObject
		{
			// Token: 0x06000B10 RID: 2832 RVA: 0x0001EA6D File Offset: 0x0001CC6D
			public SpritePoolObject(PoolObject poolObject)
			{
				this.poolObject = poolObject;
				this.spriteRenderer = poolObject.GetComponent<SpriteRenderer>();
			}

			// Token: 0x06000B11 RID: 2833 RVA: 0x0001EA82 File Offset: 0x0001CC82
			public SpritePoolObject(PoolObject poolObject, SpriteRenderer spriteRenderer)
			{
				this.poolObject = poolObject;
				this.spriteRenderer = spriteRenderer;
			}

			// Token: 0x06000B12 RID: 2834 RVA: 0x0001EA92 File Offset: 0x0001CC92
			public Effects.SpritePoolObject Spawn()
			{
				return new Effects.SpritePoolObject(this.poolObject.Spawn(true));
			}

			// Token: 0x06000B13 RID: 2835 RVA: 0x0001EAA5 File Offset: 0x0001CCA5
			public Effects.SpritePoolObject Spawn(SpriteRendererValues spriteRendererValues)
			{
				Effects.SpritePoolObject spritePoolObject = this.Spawn();
				spritePoolObject.spriteRenderer.CopyFrom(this.spriteRenderer);
				return spritePoolObject;
			}

			// Token: 0x06000B14 RID: 2836 RVA: 0x0001EABE File Offset: 0x0001CCBE
			public Effects.SpritePoolObject Spawn(SpriteRenderer spriteRenderer)
			{
				Effects.SpritePoolObject spritePoolObject = this.Spawn();
				spritePoolObject.spriteRenderer.CopyFrom(spriteRenderer);
				return spritePoolObject;
			}

			// Token: 0x06000B15 RID: 2837 RVA: 0x0001EAD2 File Offset: 0x0001CCD2
			public void FadeOut(Chronometer chronometer, AnimationCurve curve, float duration)
			{
				this.poolObject.FadeOut(this.spriteRenderer, chronometer, curve, duration);
			}

			// Token: 0x04000939 RID: 2361
			public PoolObject poolObject;

			// Token: 0x0400093A RID: 2362
			public SpriteRenderer spriteRenderer;
		}
	}
}
