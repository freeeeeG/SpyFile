using System;
using Scenes;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class CastleParallax : MonoBehaviour
{
	// Token: 0x06000048 RID: 72 RVA: 0x000034C4 File Offset: 0x000016C4
	private void Awake()
	{
		CastleParallax.Element[] values = this._elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].Initialize();
		}
		this._cameraController = Scene<GameBase>.instance.cameraController;
		this.UpdateElements(this._cameraController.transform.position - this._origin.position, 0f);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x0000352E File Offset: 0x0000172E
	private void Update()
	{
		this.UpdateElements(this._cameraController.delta, Chronometer.global.deltaTime);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x0000354C File Offset: 0x0000174C
	private void UpdateElements(Vector3 delta, float deltaTime)
	{
		CastleParallax.Element[] values = this._elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].Update(delta, deltaTime);
		}
	}

	// Token: 0x0400004D RID: 77
	[SerializeField]
	private Transform _origin;

	// Token: 0x0400004E RID: 78
	[SerializeField]
	private CastleParallax.Element.Reorderable _elements;

	// Token: 0x0400004F RID: 79
	private CameraController _cameraController;

	// Token: 0x02000015 RID: 21
	[Serializable]
	private class Element
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003580 File Offset: 0x00001780
		internal void Initialize()
		{
			if (this._hotizontalAutoScroll == 0f)
			{
				return;
			}
			this._spriteRenderer.drawMode = SpriteDrawMode.Tiled;
			this._spriteRenderer.tileMode = SpriteTileMode.Continuous;
			this._spriteSize = this._spriteRenderer.sprite.bounds.size;
			this._spriteSize.x = this._spriteSize.x * 2f;
			if (this._hotizontalAutoScroll >= 0f)
			{
				this._spriteRenderer.size = this._spriteSize;
				return;
			}
			this._spriteRenderer.size = -this._spriteSize;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003620 File Offset: 0x00001820
		internal void Update(Vector3 delta, float deltaTime)
		{
			this._spriteRenderer.transform.Translate(0f, this._verticalScroll * delta.y, 0f);
			if (this._hotizontalAutoScroll == 0f)
			{
				return;
			}
			Vector2 size = this._spriteRenderer.size;
			size.x += this._hotizontalAutoScroll * deltaTime;
			if (this._hotizontalAutoScroll > 0f && size.x >= this._spriteSize.x * 2f)
			{
				size.x = this._spriteSize.x;
			}
			if (this._hotizontalAutoScroll < 0f && size.x <= this._spriteSize.x * 2f)
			{
				size.x = this._spriteSize.x * 2f * 2f;
			}
			this._spriteRenderer.size = size;
		}

		// Token: 0x04000050 RID: 80
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private float _verticalScroll;

		// Token: 0x04000052 RID: 82
		[SerializeField]
		private float _hotizontalAutoScroll;

		// Token: 0x04000053 RID: 83
		private Vector2 _spriteSize;

		// Token: 0x02000016 RID: 22
		[Serializable]
		internal class Reorderable : ReorderableArray<CastleParallax.Element>
		{
		}
	}
}
