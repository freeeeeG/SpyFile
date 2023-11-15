using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C5 RID: 709
	public class AllIn1ScrollProperty : MonoBehaviour
	{
		// Token: 0x0600114C RID: 4428 RVA: 0x000317B0 File Offset: 0x0002F9B0
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
			if (this.mat.HasProperty(this.numericPropertyName))
			{
				this.propertyShaderID = Shader.PropertyToID(this.numericPropertyName);
			}
			else
			{
				this.DestroyComponentAndLogError(base.gameObject.name + "'s Material doesn't have a " + this.numericPropertyName + " property");
			}
			this.currValue = this.mat.GetFloat(this.propertyShaderID);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00031894 File Offset: 0x0002FA94
		private void Update()
		{
			this.currValue += this.scrollSpeed * Time.deltaTime;
			if (this.applyModulo)
			{
				this.currValue %= this.modulo;
			}
			this.mat.SetFloat(this.propertyShaderID, this.currValue);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000318EC File Offset: 0x0002FAEC
		private void DestroyComponentAndLogError(string logError)
		{
			Debug.LogError(logError);
			Object.Destroy(this);
		}

		// Token: 0x04000999 RID: 2457
		[SerializeField]
		private string numericPropertyName = "_RotateUvAmount";

		// Token: 0x0400099A RID: 2458
		[SerializeField]
		private float scrollSpeed;

		// Token: 0x0400099B RID: 2459
		[Space]
		[SerializeField]
		private bool applyModulo;

		// Token: 0x0400099C RID: 2460
		[SerializeField]
		private float modulo = 1f;

		// Token: 0x0400099D RID: 2461
		[Space]
		[SerializeField]
		[Header("If missing will search object Sprite Renderer or UI Image")]
		private Material mat;

		// Token: 0x0400099E RID: 2462
		private int propertyShaderID;

		// Token: 0x0400099F RID: 2463
		private float currValue;
	}
}
