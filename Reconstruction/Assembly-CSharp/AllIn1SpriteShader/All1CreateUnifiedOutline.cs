using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
	// Token: 0x020002CC RID: 716
	[ExecuteInEditMode]
	public class All1CreateUnifiedOutline : MonoBehaviour
	{
		// Token: 0x06001164 RID: 4452 RVA: 0x00031E78 File Offset: 0x00030078
		private void Update()
		{
			if (this.createUnifiedOutline)
			{
				if (this.outlineMaterial == null)
				{
					this.createUnifiedOutline = false;
					this.MissingMaterial();
					return;
				}
				List<Transform> list = new List<Transform>();
				this.GetAllChildren(base.transform, ref list);
				foreach (Transform transform in list)
				{
					this.CreateOutlineSpriteDuplicate(transform.gameObject);
				}
				this.CreateOutlineSpriteDuplicate(base.gameObject);
				Object.DestroyImmediate(this);
			}
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00031F18 File Offset: 0x00030118
		private void CreateOutlineSpriteDuplicate(GameObject target)
		{
			bool flag = false;
			SpriteRenderer component = target.GetComponent<SpriteRenderer>();
			Image component2 = target.GetComponent<Image>();
			if (component != null)
			{
				flag = false;
			}
			else if (component2 != null)
			{
				flag = true;
			}
			else if (component == null && component2 == null && !base.transform.Equals(this.outlineParentTransform))
			{
				return;
			}
			GameObject gameObject = new GameObject();
			gameObject.name = target.name + "Outline";
			gameObject.transform.position = target.transform.position;
			gameObject.transform.rotation = target.transform.rotation;
			gameObject.transform.localScale = target.transform.lossyScale;
			if (this.outlineParentTransform == null)
			{
				gameObject.transform.parent = target.transform;
			}
			else
			{
				gameObject.transform.parent = this.outlineParentTransform;
			}
			if (!flag)
			{
				SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = component.sprite;
				spriteRenderer.sortingOrder = this.duplicateOrderInLayer;
				spriteRenderer.sortingLayerName = this.duplicateSortingLayer;
				spriteRenderer.material = this.outlineMaterial;
				spriteRenderer.flipX = component.flipX;
				spriteRenderer.flipY = component.flipY;
				return;
			}
			Image image = gameObject.AddComponent<Image>();
			image.sprite = component2.sprite;
			image.material = this.outlineMaterial;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00032073 File Offset: 0x00030273
		private void MissingMaterial()
		{
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00032078 File Offset: 0x00030278
		private void GetAllChildren(Transform parent, ref List<Transform> transforms)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				transforms.Add(transform);
				this.GetAllChildren(transform, ref transforms);
			}
		}

		// Token: 0x040009B5 RID: 2485
		[SerializeField]
		private Material outlineMaterial;

		// Token: 0x040009B6 RID: 2486
		[SerializeField]
		private Transform outlineParentTransform;

		// Token: 0x040009B7 RID: 2487
		[Space]
		[Header("Only needed if Sprite (ignored if UI)")]
		[SerializeField]
		private int duplicateOrderInLayer = -100;

		// Token: 0x040009B8 RID: 2488
		[SerializeField]
		private string duplicateSortingLayer = "Default";

		// Token: 0x040009B9 RID: 2489
		[Space]
		[Header("This operation will delete the component")]
		[SerializeField]
		private bool createUnifiedOutline;
	}
}
