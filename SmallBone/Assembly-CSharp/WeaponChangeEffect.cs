using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class WeaponChangeEffect : MonoBehaviour
{
	// Token: 0x060000DC RID: 220 RVA: 0x00004EFC File Offset: 0x000030FC
	private void Start()
	{
		this._isActive = true;
		this.tempMaterial = new Material(Shader.Find("GUI/Text Shader"));
		this.tempMaterial.hideFlags = HideFlags.None;
		this._spriteRenderer.material = this.tempMaterial;
		this._spriteRenderer.color = this._startColor;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00002191 File Offset: 0x00000391
	private static void Initialize()
	{
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00004F53 File Offset: 0x00003153
	private IEnumerator ChangeColor(Chronometer chronometer)
	{
		float t = 0f;
		do
		{
			yield return null;
			this._spriteRenderer.color = Color.Lerp(this._startColor, this._endColor, t / this._duration);
			t += chronometer.deltaTime;
		}
		while (t <= this._duration);
		this._spriteRenderer.material = this.defaultMaterial;
		this._spriteRenderer.color = Color.white;
		this._isActive = false;
		yield break;
	}

	// Token: 0x040000BE RID: 190
	[SerializeField]
	private Color _startColor = Color.white;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private Color _endColor = Color.white;

	// Token: 0x040000C0 RID: 192
	[SerializeField]
	private float _duration = 0.2f;

	// Token: 0x040000C1 RID: 193
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	// Token: 0x040000C2 RID: 194
	[SerializeField]
	private EasingFunction.Method _runEaseMethod = EasingFunction.Method.Linear;

	// Token: 0x040000C3 RID: 195
	private EasingFunction _runEase;

	// Token: 0x040000C4 RID: 196
	private Material tempMaterial;

	// Token: 0x040000C5 RID: 197
	private Material defaultMaterial;

	// Token: 0x040000C6 RID: 198
	private static float _endTime;

	// Token: 0x040000C7 RID: 199
	private bool _isActive;

	// Token: 0x040000C8 RID: 200
	private const string shader = "GUI/Text Shader";
}
