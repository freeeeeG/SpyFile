using System;
using System.Collections.Generic;
using Characters;
using GameResources;
using Singletons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FX
{
	// Token: 0x02000229 RID: 553
	[Serializable]
	public class EffectInfo : IDisposable
	{
		// Token: 0x06000ADD RID: 2781 RVA: 0x0001DA04 File Offset: 0x0001BC04
		public EffectInfo()
		{
			this.attachInfo = new EffectInfo.AttachInfo();
			this.scale = new CustomFloat(1f);
			this.scaleX = new CustomFloat(1f);
			this.scaleY = new CustomFloat(1f);
			this.angle = new CustomAngle(0f);
			this.noise = new PositionNoise();
			this.color = Color.white;
			this.sortingLayerId = int.MinValue;
			this.fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this.flipDirectionByOwnerDirection = false;
			this.flipXByOwnerDirection = true;
			this.flipYByOwnerDirection = false;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0001DAFC File Offset: 0x0001BCFC
		public EffectInfo(PoolObject effect)
		{
			this.effect = effect;
			this.attachInfo = new EffectInfo.AttachInfo();
			this.scale = new CustomFloat(1f);
			this.scaleX = new CustomFloat(1f);
			this.scaleY = new CustomFloat(1f);
			this.angle = new CustomAngle(0f);
			this.noise = new PositionNoise();
			this.color = Color.white;
			this.sortingLayerId = SortingLayer.NameToID("Effect");
			this.fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this.flipDirectionByOwnerDirection = false;
			this.flipXByOwnerDirection = true;
			this.flipYByOwnerDirection = false;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0001DC00 File Offset: 0x0001BE00
		public EffectInfo(RuntimeAnimatorController animation)
		{
			this.animation = animation;
			this.animationBySize = new EffectInfo.SizeForEffectAndAnimatorArray();
			for (int i = 0; i < this.animationBySize.Array.Length; i++)
			{
				this.animationBySize.Array[i] = animation;
			}
			this.attachInfo = new EffectInfo.AttachInfo();
			this.scale = new CustomFloat(1f);
			this.scaleX = new CustomFloat(1f);
			this.scaleY = new CustomFloat(1f);
			this.angle = new CustomAngle(0f);
			this.noise = new PositionNoise();
			this.color = Color.white;
			this.sortingLayerId = SortingLayer.NameToID("Effect");
			this.fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this.flipDirectionByOwnerDirection = false;
			this.flipXByOwnerDirection = true;
			this.flipYByOwnerDirection = false;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0001DD34 File Offset: 0x0001BF34
		public EffectInfo(RuntimeAnimatorController animation, EffectInfo.SizeForEffectAndAnimatorArray animationBySize)
		{
			this.animation = animation;
			this.animationBySize = animationBySize;
			this.attachInfo = new EffectInfo.AttachInfo();
			this.scale = new CustomFloat(1f);
			this.scaleX = new CustomFloat(1f);
			this.scaleY = new CustomFloat(1f);
			this.angle = new CustomAngle(0f);
			this.noise = new PositionNoise();
			this.color = Color.white;
			this.sortingLayerId = SortingLayer.NameToID("Effect");
			this.fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			this.flipDirectionByOwnerDirection = false;
			this.flipXByOwnerDirection = true;
			this.flipYByOwnerDirection = false;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0001DE40 File Offset: 0x0001C040
		~EffectInfo()
		{
			this.Dispose();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0001DE6C File Offset: 0x0001C06C
		public void Dispose()
		{
			this.effect = null;
			this.animation = null;
			if (this._animationAssetHandle.IsValid())
			{
				Addressables.Release<RuntimeAnimatorController>(this._animationAssetHandle);
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0001DE94 File Offset: 0x0001C094
		private void SetTransform(EffectPoolInstance effect, Vector3 position, float extraAngle, float extraScale, bool flip = false)
		{
			float num = this.angle.value + extraAngle;
			if (flip && this.flipDirectionByOwnerDirection)
			{
				num = (180f - num) % 360f;
			}
			effect.transform.SetPositionAndRotation(position + this.noise.Evaluate(), Quaternion.Euler(0f, 0f, num));
			Vector3 localScale = Vector3.one * extraScale * this.scale.value;
			float value = this.scaleX.value;
			if (value > 0f)
			{
				localScale.x *= value;
			}
			float value2 = this.scaleY.value;
			if (value2 > 0f)
			{
				localScale.y *= value2;
			}
			effect.renderer.flipX = this.flipX;
			effect.renderer.flipY = this.flipY;
			if (flip)
			{
				if (this.flipXByOwnerDirection)
				{
					localScale.x *= -1f;
				}
				if (this.flipYByOwnerDirection)
				{
					localScale.y *= -1f;
				}
			}
			effect.transform.localScale = localScale;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		public void LoadReference()
		{
			if (this.animationReference == null || !this.animationReference.RuntimeKeyIsValid())
			{
				return;
			}
			this._animationAssetHandle = this.animationReference.LoadAssetAsync<RuntimeAnimatorController>();
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0001DFDD File Offset: 0x0001C1DD
		public EffectPoolInstance Spawn(Vector3 position, float extraAngle = 0f, float extraScale = 1f)
		{
			if (this.effect != null)
			{
				return this.Spawn(position, this.effect.GetComponent<Animator>().runtimeAnimatorController, extraAngle, extraScale, false);
			}
			return this.Spawn(position, this.animation, extraAngle, extraScale, false);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0001E018 File Offset: 0x0001C218
		public EffectPoolInstance Spawn(Vector3 position, RuntimeAnimatorController animation, float extraAngle = 0f, float extraScale = 1f, bool flip = false)
		{
			EffectInfo.<>c__DisplayClass45_0 CS$<>8__locals1 = new EffectInfo.<>c__DisplayClass45_0();
			CS$<>8__locals1.<>4__this = this;
			if (animation == null)
			{
				if (!this._animationAssetHandle.IsValid())
				{
					return null;
				}
				animation = this._animationAssetHandle.WaitForCompletion();
				if (animation == null)
				{
					return null;
				}
			}
			CS$<>8__locals1.effect = Singleton<EffectPool>.Instance.Play(animation, this.delay, this.duration, this.loop, this.fadeOutCurve, this.fadeOutDuration);
			this.SetTransform(CS$<>8__locals1.effect, position, extraAngle, extraScale, flip);
			CS$<>8__locals1.effect.hue = this.hue;
			CS$<>8__locals1.effect.renderer.sortingOrder = (int)(this.autoLayerOrder ? Effects.GetSortingOrderAndIncrease() : this.sortingLayerOrder);
			if (SortingLayer.IsValid(this.sortingLayerId))
			{
				if (CS$<>8__locals1.effect.renderer.sortingLayerID != this.sortingLayerId)
				{
					CS$<>8__locals1.effect.renderer.sortingLayerID = this.sortingLayerId;
				}
			}
			else
			{
				int num = SortingLayer.NameToID("Effect");
				Debug.LogError(string.Format("The sorting layer id of effect is invalid! id : {0}, effect id : {1}", this.sortingLayerId, num));
				CS$<>8__locals1.effect.renderer.sortingLayerID = num;
			}
			Material sharedMaterial;
			switch (this.blend)
			{
			case EffectInfo.Blend.Darken:
				sharedMaterial = MaterialResource.effect_darken;
				break;
			case EffectInfo.Blend.Lighten:
				sharedMaterial = MaterialResource.effect_lighten;
				break;
			case EffectInfo.Blend.LinearBurn:
				sharedMaterial = MaterialResource.effect_linearBurn;
				break;
			case EffectInfo.Blend.LinearDodge:
				sharedMaterial = MaterialResource.effect_linearDodge;
				break;
			default:
				sharedMaterial = MaterialResource.effect;
				break;
			}
			CS$<>8__locals1.effect.renderer.sharedMaterial = sharedMaterial;
			CS$<>8__locals1.effect.chronometer = this.chronometer;
			if (this.trackChildren)
			{
				this._children.Add(CS$<>8__locals1.effect);
				CS$<>8__locals1.effect.OnStop += CS$<>8__locals1.<Spawn>g__RemoveFromList|0;
			}
			CS$<>8__locals1.effect.renderer.enabled = true;
			CS$<>8__locals1.effect.renderer.color = this.color;
			return CS$<>8__locals1.effect;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0001E218 File Offset: 0x0001C418
		public EffectPoolInstance Spawn(Vector3 position, Character target, float extraAngle = 0f, float extraScale = 1f)
		{
			if (this.attachInfo.attach && target.sizeForEffect == Character.SizeForEffect.None)
			{
				return null;
			}
			RuntimeAnimatorController x;
			if (this.effect != null)
			{
				x = this.effect.GetComponent<Animator>().runtimeAnimatorController;
			}
			else
			{
				EffectInfo.SizeForEffectAndAnimatorArray sizeForEffectAndAnimatorArray = this.animationBySize;
				x = ((sizeForEffectAndAnimatorArray != null) ? sizeForEffectAndAnimatorArray[target.sizeForEffect] : null);
				if (x == null)
				{
					x = this.animation;
				}
			}
			EffectPoolInstance effectPoolInstance = this.Spawn(position, x, extraAngle, extraScale, target.lookingDirection == Character.LookingDirection.Left);
			if (effectPoolInstance == null)
			{
				return null;
			}
			effectPoolInstance.chronometer = target.chronometer.effect;
			if (this.attachInfo.attach)
			{
				effectPoolInstance.transform.parent = (this.flipXByOwnerDirection ? target.attachWithFlip.transform : target.attach.transform);
				Vector3 position2 = target.transform.position;
				position2.x += target.collider.offset.x;
				position2.y += target.collider.offset.y;
				Vector3 size = target.collider.bounds.size;
				size.x *= this.attachInfo.pivotValue.x;
				size.y *= this.attachInfo.pivotValue.y;
				effectPoolInstance.transform.position = position2 + size;
			}
			return effectPoolInstance;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0001E394 File Offset: 0x0001C594
		public void DespawnChildren()
		{
			for (int i = this._children.Count - 1; i >= 0; i--)
			{
				this._children[i].Stop();
			}
		}

		// Token: 0x040008F4 RID: 2292
		public static readonly int huePropertyID = Shader.PropertyToID("_Hue");

		// Token: 0x040008F5 RID: 2293
		[SerializeField]
		private bool _fold;

		// Token: 0x040008F6 RID: 2294
		public bool subordinated;

		// Token: 0x040008F7 RID: 2295
		public PoolObject effect;

		// Token: 0x040008F8 RID: 2296
		public RuntimeAnimatorController animation;

		// Token: 0x040008F9 RID: 2297
		public AssetReference animationReference;

		// Token: 0x040008FA RID: 2298
		private AsyncOperationHandle<RuntimeAnimatorController> _animationAssetHandle;

		// Token: 0x040008FB RID: 2299
		public EffectInfo.SizeForEffectAndAnimatorArray animationBySize;

		// Token: 0x040008FC RID: 2300
		public EffectInfo.AttachInfo attachInfo;

		// Token: 0x040008FD RID: 2301
		public CustomFloat scale;

		// Token: 0x040008FE RID: 2302
		public CustomFloat scaleX;

		// Token: 0x040008FF RID: 2303
		public CustomFloat scaleY;

		// Token: 0x04000900 RID: 2304
		public CustomAngle angle;

		// Token: 0x04000901 RID: 2305
		public PositionNoise noise;

		// Token: 0x04000902 RID: 2306
		public Color color = Color.white;

		// Token: 0x04000903 RID: 2307
		public EffectInfo.Blend blend;

		// Token: 0x04000904 RID: 2308
		[Range(-180f, 180f)]
		public int hue;

		// Token: 0x04000905 RID: 2309
		public int sortingLayerId;

		// Token: 0x04000906 RID: 2310
		public bool autoLayerOrder = true;

		// Token: 0x04000907 RID: 2311
		public short sortingLayerOrder;

		// Token: 0x04000908 RID: 2312
		public bool trackChildren;

		// Token: 0x04000909 RID: 2313
		public bool loop;

		// Token: 0x0400090A RID: 2314
		public float delay;

		// Token: 0x0400090B RID: 2315
		public float duration;

		// Token: 0x0400090C RID: 2316
		[Header("Flips")]
		[Tooltip("Owner의 방향에 따라서 각도를 뒤집음")]
		public bool flipDirectionByOwnerDirection;

		// Token: 0x0400090D RID: 2317
		[Tooltip("Owner의 방향에 따라서 X 스케일을 뒤집음")]
		public bool flipXByOwnerDirection = true;

		// Token: 0x0400090E RID: 2318
		[Tooltip("Owner의 방향에 따라서 Y 스케일을 뒤집음")]
		public bool flipYByOwnerDirection;

		// Token: 0x0400090F RID: 2319
		[Tooltip("이미지를 좌우 반전시킴")]
		public bool flipX;

		// Token: 0x04000910 RID: 2320
		[Tooltip("이미지를 상하 반전시킴")]
		public bool flipY;

		// Token: 0x04000911 RID: 2321
		public AnimationCurve fadeOutCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000912 RID: 2322
		public float fadeOutDuration;

		// Token: 0x04000913 RID: 2323
		public Chronometer chronometer;

		// Token: 0x04000914 RID: 2324
		private readonly List<EffectPoolInstance> _children = new List<EffectPoolInstance>();

		// Token: 0x0200022A RID: 554
		[Serializable]
		public class AttachInfo
		{
			// Token: 0x17000258 RID: 600
			// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0001E3DB File Offset: 0x0001C5DB
			public bool attach
			{
				get
				{
					return this._attach;
				}
			}

			// Token: 0x17000259 RID: 601
			// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0001E3E3 File Offset: 0x0001C5E3
			public EffectInfo.AttachInfo.Pivot pivot
			{
				get
				{
					return this._pivot;
				}
			}

			// Token: 0x1700025A RID: 602
			// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0001E3EB File Offset: 0x0001C5EB
			public Vector2 pivotValue
			{
				get
				{
					return this._pivotValue;
				}
			}

			// Token: 0x06000AED RID: 2797 RVA: 0x0001E3F3 File Offset: 0x0001C5F3
			public AttachInfo()
			{
				this._attach = false;
				this._pivot = EffectInfo.AttachInfo.Pivot.Center;
				this._pivotValue = Vector2.zero;
			}

			// Token: 0x06000AEE RID: 2798 RVA: 0x0001E414 File Offset: 0x0001C614
			public AttachInfo(bool attach, bool layerOnly, int layerOrderOffset, EffectInfo.AttachInfo.Pivot pivot)
			{
				this._attach = attach;
				this._pivot = pivot;
				this._pivotValue = EffectInfo.AttachInfo._pivotValues[pivot];
			}

			// Token: 0x04000915 RID: 2325
			private static readonly EnumArray<EffectInfo.AttachInfo.Pivot, Vector2> _pivotValues = new EnumArray<EffectInfo.AttachInfo.Pivot, Vector2>(new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(-0.5f, 0.5f),
				new Vector2(0f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(-0.5f, 0f),
				new Vector2(0f, 0.5f),
				new Vector2(-0.5f, -0.5f),
				new Vector2(0f, -0.5f),
				new Vector2(0.5f, -0.5f),
				new Vector2(0f, 0f)
			});

			// Token: 0x04000916 RID: 2326
			[SerializeField]
			internal bool _attach;

			// Token: 0x04000917 RID: 2327
			[SerializeField]
			private EffectInfo.AttachInfo.Pivot _pivot;

			// Token: 0x04000918 RID: 2328
			[HideInInspector]
			[SerializeField]
			private Vector2 _pivotValue;

			// Token: 0x0200022B RID: 555
			public enum Pivot
			{
				// Token: 0x0400091A RID: 2330
				Center,
				// Token: 0x0400091B RID: 2331
				TopLeft,
				// Token: 0x0400091C RID: 2332
				Top,
				// Token: 0x0400091D RID: 2333
				TopRight,
				// Token: 0x0400091E RID: 2334
				Left,
				// Token: 0x0400091F RID: 2335
				Right,
				// Token: 0x04000920 RID: 2336
				BottomLeft,
				// Token: 0x04000921 RID: 2337
				Bottom,
				// Token: 0x04000922 RID: 2338
				BottomRight,
				// Token: 0x04000923 RID: 2339
				Custom
			}
		}

		// Token: 0x0200022C RID: 556
		[Serializable]
		public class SizeForEffectAndAnimatorArray : EnumArray<Character.SizeForEffect, RuntimeAnimatorController>, IDisposable
		{
			// Token: 0x06000AF0 RID: 2800 RVA: 0x0001E53C File Offset: 0x0001C73C
			public void Dispose()
			{
				for (int i = 0; i < base.Array.Length; i++)
				{
					base.Array[i] = null;
				}
			}
		}

		// Token: 0x0200022D RID: 557
		public enum Blend
		{
			// Token: 0x04000925 RID: 2341
			Normal,
			// Token: 0x04000926 RID: 2342
			Darken,
			// Token: 0x04000927 RID: 2343
			Lighten,
			// Token: 0x04000928 RID: 2344
			LinearBurn,
			// Token: 0x04000929 RID: 2345
			LinearDodge
		}
	}
}
