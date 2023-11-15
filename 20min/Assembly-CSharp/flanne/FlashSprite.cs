using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000A9 RID: 169
	public class FlashSprite : MonoBehaviour
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x0001AD98 File Offset: 0x00018F98
		private void Start()
		{
			if (this.sprite != null)
			{
				this.spriteRenderer = this.sprite;
			}
			else
			{
				this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			this.originalMaterial = this.spriteRenderer.sharedMaterial;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001ADD3 File Offset: 0x00018FD3
		private void OnDisable()
		{
			this.StopFlash();
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001ADDB File Offset: 0x00018FDB
		public void Flash()
		{
			if (this.flashRoutine != null)
			{
				base.StopCoroutine(this.flashRoutine);
			}
			if (base.gameObject.activeSelf)
			{
				this.flashRoutine = base.StartCoroutine(this.FlashRoutine());
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001AE10 File Offset: 0x00019010
		public void StopFlash()
		{
			if (this.flashRoutine != null)
			{
				base.StopCoroutine(this.flashRoutine);
				this.spriteRenderer.material = this.originalMaterial;
				this.flashRoutine = null;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001AE3E File Offset: 0x0001903E
		private IEnumerator FlashRoutine()
		{
			if (this.spriteRenderer != null)
			{
				int num;
				for (int i = 0; i < this.numRepeats; i = num + 1)
				{
					this.spriteRenderer.material = this.flashMaterial;
					yield return new WaitForSeconds(this.duration);
					this.spriteRenderer.material = this.originalMaterial;
					yield return new WaitForSeconds(this.duration);
					num = i;
				}
			}
			this.flashRoutine = null;
			yield break;
		}

		// Token: 0x04000387 RID: 903
		[Tooltip("Defaults to sprite on this gameObject if left null.")]
		[SerializeField]
		private SpriteRenderer sprite;

		// Token: 0x04000388 RID: 904
		[Tooltip("Material to switch to during the flash.")]
		[SerializeField]
		private Material flashMaterial;

		// Token: 0x04000389 RID: 905
		[Tooltip("Duration of a flash.")]
		[SerializeField]
		private float duration;

		// Token: 0x0400038A RID: 906
		[Tooltip("Times to repeat flash.")]
		[SerializeField]
		private int numRepeats = 1;

		// Token: 0x0400038B RID: 907
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400038C RID: 908
		private Material originalMaterial;

		// Token: 0x0400038D RID: 909
		private Coroutine flashRoutine;
	}
}
