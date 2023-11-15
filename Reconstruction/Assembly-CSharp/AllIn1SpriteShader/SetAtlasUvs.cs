using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
	// Token: 0x020002CE RID: 718
	[ExecuteInEditMode]
	public class SetAtlasUvs : MonoBehaviour
	{
		// Token: 0x0600116B RID: 4459 RVA: 0x000321D5 File Offset: 0x000303D5
		private void Start()
		{
			this.Setup();
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000321DD File Offset: 0x000303DD
		private void Reset()
		{
			this.Setup();
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x000321E5 File Offset: 0x000303E5
		private void Setup()
		{
			if (this.GetRendererReferencesIfNeeded())
			{
				this.GetAndSetUVs();
			}
			if (!this.updateEveryFrame && Application.isPlaying && this != null)
			{
				base.enabled = false;
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00032214 File Offset: 0x00030414
		private void OnWillRenderObject()
		{
			if (this.updateEveryFrame)
			{
				this.GetAndSetUVs();
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00032224 File Offset: 0x00030424
		public void GetAndSetUVs()
		{
			if (!this.GetRendererReferencesIfNeeded())
			{
				return;
			}
			if (!this.isUI)
			{
				Rect textureRect = this.spriteRender.sprite.textureRect;
				textureRect.x /= (float)this.spriteRender.sprite.texture.width;
				textureRect.width /= (float)this.spriteRender.sprite.texture.width;
				textureRect.y /= (float)this.spriteRender.sprite.texture.height;
				textureRect.height /= (float)this.spriteRender.sprite.texture.height;
				this.render.sharedMaterial.SetFloat("_MinXUV", textureRect.xMin);
				this.render.sharedMaterial.SetFloat("_MaxXUV", textureRect.xMax);
				this.render.sharedMaterial.SetFloat("_MinYUV", textureRect.yMin);
				this.render.sharedMaterial.SetFloat("_MaxYUV", textureRect.yMax);
				return;
			}
			Rect textureRect2 = this.uiImage.sprite.textureRect;
			textureRect2.x /= (float)this.uiImage.sprite.texture.width;
			textureRect2.width /= (float)this.uiImage.sprite.texture.width;
			textureRect2.y /= (float)this.uiImage.sprite.texture.height;
			textureRect2.height /= (float)this.uiImage.sprite.texture.height;
			this.uiImage.material.SetFloat("_MinXUV", textureRect2.xMin);
			this.uiImage.material.SetFloat("_MaxXUV", textureRect2.xMax);
			this.uiImage.material.SetFloat("_MinYUV", textureRect2.yMin);
			this.uiImage.material.SetFloat("_MaxYUV", textureRect2.yMax);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00032468 File Offset: 0x00030668
		public void ResetAtlasUvs()
		{
			if (!this.GetRendererReferencesIfNeeded())
			{
				return;
			}
			if (!this.isUI)
			{
				this.render.sharedMaterial.SetFloat("_MinXUV", 0f);
				this.render.sharedMaterial.SetFloat("_MaxXUV", 1f);
				this.render.sharedMaterial.SetFloat("_MinYUV", 0f);
				this.render.sharedMaterial.SetFloat("_MaxYUV", 1f);
				return;
			}
			this.uiImage.material.SetFloat("_MinXUV", 0f);
			this.uiImage.material.SetFloat("_MaxXUV", 1f);
			this.uiImage.material.SetFloat("_MinYUV", 0f);
			this.uiImage.material.SetFloat("_MaxYUV", 1f);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00032557 File Offset: 0x00030757
		public void UpdateEveryFrame(bool everyFrame)
		{
			this.updateEveryFrame = everyFrame;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00032560 File Offset: 0x00030760
		private bool GetRendererReferencesIfNeeded()
		{
			if (this.spriteRender == null)
			{
				this.spriteRender = base.GetComponent<SpriteRenderer>();
			}
			if (this.spriteRender != null)
			{
				if (this.spriteRender.sprite == null)
				{
					Object.DestroyImmediate(this);
					return false;
				}
				if (this.render == null)
				{
					this.render = base.GetComponent<Renderer>();
				}
				this.isUI = false;
			}
			else
			{
				if (this.uiImage == null)
				{
					this.uiImage = base.GetComponent<Image>();
					if (!(this.uiImage != null))
					{
						Object.DestroyImmediate(this);
						return false;
					}
				}
				if (this.render == null)
				{
					this.render = base.GetComponent<Renderer>();
				}
				this.isUI = true;
			}
			if (this.spriteRender == null && this.uiImage == null)
			{
				Object.DestroyImmediate(this);
				return false;
			}
			return true;
		}

		// Token: 0x040009BA RID: 2490
		[SerializeField]
		private bool updateEveryFrame;

		// Token: 0x040009BB RID: 2491
		private Renderer render;

		// Token: 0x040009BC RID: 2492
		private SpriteRenderer spriteRender;

		// Token: 0x040009BD RID: 2493
		private Image uiImage;

		// Token: 0x040009BE RID: 2494
		private bool isUI;
	}
}
