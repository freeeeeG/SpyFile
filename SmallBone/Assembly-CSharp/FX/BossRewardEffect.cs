using System;
using Characters;
using GameResources;
using Level;
using Singletons;
using UnityEngine;
using UnityEngine.Rendering;

namespace FX
{
	// Token: 0x02000265 RID: 613
	public sealed class BossRewardEffect : MonoBehaviour
	{
		// Token: 0x06000BFF RID: 3071 RVA: 0x00020EA8 File Offset: 0x0001F0A8
		private void Awake()
		{
			this._droppedGear = base.GetComponent<DroppedGear>();
			this._child = new GameObject();
			this._child.transform.parent = base.transform;
			this._child.transform.localPosition = Vector2.zero;
			this._renderer = this._child.AddComponent<SpriteRenderer>();
			this._group = this._droppedGear.gameObject.AddComponent<SortingGroup>();
			this._group.sortingLayerID = this._droppedGear.spriteRenderer.sortingLayerID;
			this._renderer.sortingOrder = this._droppedGear.spriteRenderer.sortingOrder;
			this._renderer.sortingLayerID = this._droppedGear.spriteRenderer.sortingLayerID;
			this._renderer.sortingOrder = this._droppedGear.spriteRenderer.sortingOrder - 1;
			this._animator = this._child.AddComponent<Animator>();
			this._animator.runtimeAnimatorController = CommonResource.instance.bossRewardDeactive;
			this._renderer.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			this.UpdateGearMaterial(1f);
			this._droppedGear.onLoot += this.HandleOnLoot;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00021000 File Offset: 0x0001F200
		private void HandleOnLoot(Character character)
		{
			this.UpdateGearMaterial(0f);
			this._droppedGear.onLoot -= this.HandleOnLoot;
			this._droppedGear.additionalPopupUIOffsetY = 0f;
			this._droppedGear.dropMovement.ResetValue();
			UnityEngine.Object.Destroy(this._child);
			UnityEngine.Object.Destroy(this._group);
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002106C File Offset: 0x0001F26C
		private void Update()
		{
			if (this._droppedGear == null)
			{
				return;
			}
			if (this._droppedGear.popupVisible)
			{
				if (this._cachePopupVisible)
				{
					return;
				}
				this._cachePopupVisible = true;
				this._droppedGear.dropMovement.floating = true;
				this._animator.runtimeAnimatorController = CommonResource.instance.bossRewardActive;
				this._renderer.transform.localScale = new Vector3(0.72f, 0.72f, 1f);
				PersistentSingleton<SoundManager>.Instance.PlaySound(CommonResource.instance.bossRewardActiveSound, this._droppedGear.transform.position, 0.1f);
				this.UpdateGearMaterial(0f);
				return;
			}
			else
			{
				if (!this._cachePopupVisible)
				{
					return;
				}
				this._cachePopupVisible = false;
				this._droppedGear.dropMovement.floating = false;
				this._renderer.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				this._animator.runtimeAnimatorController = CommonResource.instance.bossRewardDeactive;
				this.UpdateGearMaterial(1f);
				return;
			}
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00021190 File Offset: 0x0001F390
		private void UpdateGearMaterial(float grayScale)
		{
			SpriteRenderer spriteRenderer = this._droppedGear.spriteRenderer;
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			spriteRenderer.sharedMaterial = MaterialResource.character;
			spriteRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat(BossRewardEffect.grayScaleID, grayScale);
			spriteRenderer.SetPropertyBlock(materialPropertyBlock);
		}

		// Token: 0x04000A00 RID: 2560
		private DroppedGear _droppedGear;

		// Token: 0x04000A01 RID: 2561
		private static readonly int grayScaleID = Shader.PropertyToID("_GrayScaleLerp");

		// Token: 0x04000A02 RID: 2562
		private bool _cachePopupVisible;

		// Token: 0x04000A03 RID: 2563
		private Renderer _renderer;

		// Token: 0x04000A04 RID: 2564
		private Animator _animator;

		// Token: 0x04000A05 RID: 2565
		private SortingGroup _group;

		// Token: 0x04000A06 RID: 2566
		private GameObject _child;
	}
}
