using System;
using FX;
using GameResources;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class MinimapAgentGenerator : MonoBehaviour
{
	// Token: 0x0600019C RID: 412 RVA: 0x00007AB4 File Offset: 0x00005CB4
	private void Awake()
	{
		if (this._generated)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this._generated = true;
		SpriteRenderer mainRenderer = base.gameObject.GetComponentInParent<SpriteEffectStack>().mainRenderer;
		MinimapAgentGenerator.Generate(base.gameObject, this._collider.bounds, this._color, mainRenderer);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00007B0C File Offset: 0x00005D0C
	public static SpriteRenderer Generate(GameObject gameObject, Bounds bounds, Color color, SpriteRenderer spriteRenderer)
	{
		return MinimapAgentGenerator.Generate(gameObject, bounds, color, spriteRenderer.sortingLayerID, spriteRenderer.sortingOrder);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00007B24 File Offset: 0x00005D24
	public static SpriteRenderer Generate(GameObject gameObject, Bounds bounds, Color color, int sortingLayerID, int sortingOrder)
	{
		Sprite pixelSprite = CommonResource.instance.pixelSprite;
		gameObject.transform.position = bounds.center;
		Vector2 v = new Vector2(bounds.size.x * pixelSprite.pixelsPerUnit / pixelSprite.rect.width, bounds.size.y * pixelSprite.pixelsPerUnit / pixelSprite.rect.height);
		gameObject.transform.localScale = v;
		gameObject.layer = 25;
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sharedMaterial = MaterialResource.minimap;
		spriteRenderer.sprite = pixelSprite;
		spriteRenderer.sortingLayerID = sortingLayerID;
		spriteRenderer.sortingOrder = sortingOrder;
		spriteRenderer.color = color;
		return spriteRenderer;
	}

	// Token: 0x04000162 RID: 354
	[SerializeField]
	private Collider2D _collider;

	// Token: 0x04000163 RID: 355
	[SerializeField]
	private Color _color;

	// Token: 0x04000164 RID: 356
	[SerializeField]
	[HideInInspector]
	private bool _generated;

	// Token: 0x04000165 RID: 357
	private float _pixelPerUnit;

	// Token: 0x04000166 RID: 358
	private float _width;

	// Token: 0x04000167 RID: 359
	private float _height;
}
