using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class SpriteRendererPreviewer : MonoBehaviour
{
	// Token: 0x060002B3 RID: 691 RVA: 0x0000AC25 File Offset: 0x00008E25
	private void Awake()
	{
		if (this._container != null)
		{
			UnityEngine.Object.Destroy(this._container.gameObject);
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000AC45 File Offset: 0x00008E45
	public void Clear()
	{
		if (this._container == null)
		{
			return;
		}
		UnityEngine.Object.DestroyImmediate(this._container.gameObject);
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000AC68 File Offset: 0x00008E68
	public void Show(SpriteRenderer[] spriteRenderers)
	{
		this.InitializeContainer();
		this.RemoveDuplicatedContainer();
		this.ClampSpriteRenderers(spriteRenderers.Length);
		this._container.gameObject.SetActive(true);
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			this.CopySpriteRenderer(spriteRenderers[i], this._sprites[i]);
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0000ACBE File Offset: 0x00008EBE
	public void Hide()
	{
		if (this._container == null)
		{
			return;
		}
		this._container.gameObject.SetActive(false);
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0000ACE0 File Offset: 0x00008EE0
	public void FlipX(bool flip)
	{
		if (!flip)
		{
			this._container.localScale = Vector3.one;
			return;
		}
		this._container.localScale = new Vector3(-1f, 1f, 1f);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0000AD18 File Offset: 0x00008F18
	private void InitializeContainer()
	{
		if (this._container != null)
		{
			return;
		}
		this._container = new GameObject("Preview").transform;
		this._container.parent = base.transform;
		this._container.localPosition = Vector3.zero;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0000AD6C File Offset: 0x00008F6C
	private void RemoveDuplicatedContainer()
	{
		if (base.transform.childCount <= 0)
		{
			return;
		}
		for (int i = base.transform.childCount - 1; i >= 0; i--)
		{
			Transform child = base.transform.GetChild(i);
			if (child != this._container && child.name.CompareTo("Preview") == 0)
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0000ADD8 File Offset: 0x00008FD8
	private void ClampSpriteRenderers(int count)
	{
		for (int i = this._sprites.Count - 1; i >= 0; i--)
		{
			if (this._sprites[i] == null)
			{
				this._sprites.Remove(this._sprites[i]);
			}
		}
		for (int j = this._sprites.Count; j < count; j++)
		{
			this.CreateNewSpriteRenderer(string.Format("[{0}]", j));
		}
		for (int k = count; k < this._sprites.Count; k++)
		{
			UnityEngine.Object.DestroyImmediate(this._sprites[k].gameObject);
		}
		for (int l = this._container.childCount - 1; l >= 0; l--)
		{
			Transform child = this._container.GetChild(l);
			SpriteRenderer component = child.GetComponent<SpriteRenderer>();
			if (component == null || !this._sprites.Contains(component))
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0000AED8 File Offset: 0x000090D8
	private void CreateNewSpriteRenderer(string name)
	{
		SpriteRenderer spriteRenderer = new GameObject(name).AddComponent<SpriteRenderer>();
		spriteRenderer.transform.parent = this._container;
		spriteRenderer.transform.localScale = Vector3.one;
		this._sprites.Add(spriteRenderer);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0000AF20 File Offset: 0x00009120
	private void CopySpriteRenderer(SpriteRenderer targetInfo, SpriteRenderer sprite)
	{
		sprite.sprite = targetInfo.sprite;
		sprite.color = targetInfo.color;
		sprite.flipX = targetInfo.flipX;
		sprite.sortingLayerID = targetInfo.sortingLayerID;
		sprite.sortingOrder = targetInfo.sortingOrder;
		sprite.transform.localPosition = targetInfo.transform.position;
	}

	// Token: 0x0400024A RID: 586
	[SerializeField]
	private Transform _container;

	// Token: 0x0400024B RID: 587
	[SerializeField]
	private List<SpriteRenderer> _sprites = new List<SpriteRenderer>();

	// Token: 0x0400024C RID: 588
	private const string _containerName = "Preview";
}
