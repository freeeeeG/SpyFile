using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C4 RID: 708
	public class All1TextureOffsetOverTime : MonoBehaviour
	{
		// Token: 0x06001148 RID: 4424 RVA: 0x00031654 File Offset: 0x0002F854
		private void Start()
		{
			if (this.mat == null)
			{
				SpriteRenderer component = base.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					this.mat = component.material;
				}
				else
				{
					Image component2 = base.GetComponent<Image>();
					if (component2 != null)
					{
						this.mat = component2.material;
					}
				}
			}
			if (this.mat == null)
			{
				this.DestroyComponentAndLogError(base.gameObject.name + " has no valid Material, deleting All1TextureOffsetOverTIme component");
				return;
			}
			if (this.mat.HasProperty(this.texturePropertyName))
			{
				this.textureShaderId = Shader.PropertyToID(this.texturePropertyName);
				return;
			}
			this.DestroyComponentAndLogError(base.gameObject.name + "'s Material doesn't have a " + this.texturePropertyName + " property");
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00031720 File Offset: 0x0002F920
		private void Update()
		{
			this.currOffset.x = this.currOffset.x + this.offsetSpeed.x * Time.deltaTime;
			this.currOffset.y = this.currOffset.y + this.offsetSpeed.y * Time.deltaTime;
			this.mat.SetTextureOffset(this.textureShaderId, this.currOffset);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00031784 File Offset: 0x0002F984
		private void DestroyComponentAndLogError(string logError)
		{
			Debug.LogError(logError);
			Object.Destroy(this);
		}

		// Token: 0x04000994 RID: 2452
		[SerializeField]
		private string texturePropertyName = "_MainTex";

		// Token: 0x04000995 RID: 2453
		[SerializeField]
		private Vector2 offsetSpeed;

		// Token: 0x04000996 RID: 2454
		[SerializeField]
		[Header("If missing will search object Sprite Renderer or UI Image")]
		private Material mat;

		// Token: 0x04000997 RID: 2455
		private int textureShaderId;

		// Token: 0x04000998 RID: 2456
		private Vector2 currOffset = Vector2.zero;
	}
}
