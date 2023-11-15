using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class SpriteFadeOut : MonoBehaviour
{
	// Token: 0x060002A7 RID: 679 RVA: 0x0000A8EB File Offset: 0x00008AEB
	private void OnEnable()
	{
		base.StartCoroutine(this.CFadeOut());
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000A8FA File Offset: 0x00008AFA
	private IEnumerator CFadeOut()
	{
		Color color = Color.white;
		EasingFunction.Function easeFunction = EasingFunction.GetEasingFunction(this._easingMehtod);
		float t = 0f;
		while (t <= this._duration)
		{
			t += Time.deltaTime;
			color.a = easeFunction(1f, 0f, t);
			this._spriteRenderer.color = color;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000234 RID: 564
	[GetComponent]
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	// Token: 0x04000235 RID: 565
	[SerializeField]
	private float _duration;

	// Token: 0x04000236 RID: 566
	[SerializeField]
	private EasingFunction.Method _easingMehtod;
}
