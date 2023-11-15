using System;
using FX;
using UnityEngine;

// Token: 0x02000046 RID: 70
[ExecuteInEditMode]
public class GridLayoutGenerator : MonoBehaviour
{
	// Token: 0x0600013C RID: 316 RVA: 0x00006E50 File Offset: 0x00005050
	public void Generate()
	{
		this.RemoveAll();
		float num = this._distanceX * (float)(this._width - 1) / 2f;
		GameObject original = this._prefab;
		for (int i = 0; i < this._height; i++)
		{
			for (int j = 0; j < this._width; j++)
			{
				if (this._prefabs != null && this._prefabs.Length != 0)
				{
					original = this._prefabs.Random<GameObject>();
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, base.transform);
				gameObject.transform.position = new Vector2(base.transform.position.x + this._distanceX * (float)j - num, base.transform.position.y + this._distanceY * (float)i);
				gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, this._rotationValue.value);
				Vector3 localScale = Vector3.one * this._scaleValue.value;
				localScale.x *= this._scaleXValue.value;
				localScale.y *= this._scaleYValue.value;
				gameObject.transform.localScale = localScale;
			}
		}
		this.Noise();
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00006FA0 File Offset: 0x000051A0
	private void RemoveAll()
	{
		for (int i = base.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00006FDC File Offset: 0x000051DC
	public void Shuffle()
	{
		foreach (object obj in base.transform)
		{
			((Transform)obj).SetSiblingIndex(UnityEngine.Random.Range(0, base.transform.childCount - 1));
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00007048 File Offset: 0x00005248
	public void Noise()
	{
		foreach (object obj in base.transform)
		{
			((Transform)obj).transform.position += this._positionNoise.Evaluate();
		}
	}

	// Token: 0x04000111 RID: 273
	[SerializeField]
	private int _width;

	// Token: 0x04000112 RID: 274
	[SerializeField]
	private int _height;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private float _distanceX;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	private float _distanceY;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	private GameObject _prefab;

	// Token: 0x04000116 RID: 278
	[Tooltip("여러개 중 랜덤한 것 생성")]
	[SerializeField]
	private GameObject[] _prefabs;

	// Token: 0x04000117 RID: 279
	[SerializeField]
	private PositionNoise _positionNoise;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	private CustomFloat _rotationValue;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private CustomFloat _scaleValue = new CustomFloat(1f);

	// Token: 0x0400011A RID: 282
	[SerializeField]
	private CustomFloat _scaleXValue = new CustomFloat(1f);

	// Token: 0x0400011B RID: 283
	[SerializeField]
	private CustomFloat _scaleYValue = new CustomFloat(1f);
}
