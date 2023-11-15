using System;
using GameResources;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class MaterialProperty : MonoBehaviour
{
	// Token: 0x060002CD RID: 717 RVA: 0x0000B198 File Offset: 0x00009398
	private void Start()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		this._renderer.sharedMaterial = MaterialResource.effect;
		this._renderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetInt(MaterialProperty._huePropertyID, this.hue);
		this._renderer.SetPropertyBlock(materialPropertyBlock);
	}

	// Token: 0x04000254 RID: 596
	private static readonly int _huePropertyID = Shader.PropertyToID("_Hue");

	// Token: 0x04000255 RID: 597
	[Range(-180f, 180f)]
	public int hue;

	// Token: 0x04000256 RID: 598
	[GetComponent]
	[SerializeField]
	private SpriteRenderer _renderer;
}
