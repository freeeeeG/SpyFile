using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class FreeParallax : MonoBehaviour
{
	// Token: 0x060000EA RID: 234 RVA: 0x00005079 File Offset: 0x00003279
	private void Awake()
	{
		this._spriteRenderers = base.GetComponentsInChildren<SpriteRenderer>();
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005088 File Offset: 0x00003288
	private void SetFadeAlpha(float t)
	{
		foreach (SpriteRenderer spriteRenderer in this._spriteRenderers)
		{
			Color color = spriteRenderer.color;
			Color color2 = new Color(color.r, color.g, color.b, t);
			spriteRenderer.color = color2;
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000050D4 File Offset: 0x000032D4
	public void FadeIn()
	{
		base.StartCoroutine(this.CFadeIn());
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000050E3 File Offset: 0x000032E3
	public IEnumerator CFadeIn()
	{
		float t = 0f;
		this.SetFadeAlpha(1f);
		yield return null;
		while (t < 1f)
		{
			this.SetFadeAlpha(1f - t);
			yield return null;
			t += Time.unscaledDeltaTime * 0.3f;
		}
		this.SetFadeAlpha(0f);
		yield break;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000050F2 File Offset: 0x000032F2
	public void FadeOut()
	{
		base.StartCoroutine(this.CFadeOut());
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00005101 File Offset: 0x00003301
	public IEnumerator CFadeOut()
	{
		float t = 0f;
		this.SetFadeAlpha(0f);
		yield return null;
		while (t < 1f)
		{
			this.SetFadeAlpha(t);
			yield return null;
			t += Time.unscaledDeltaTime * 0.3f;
		}
		this.SetFadeAlpha(1f);
		yield break;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00005110 File Offset: 0x00003310
	private void SetupElementAtIndex(int i)
	{
		FreeParallaxElement freeParallaxElement = this.Elements[i];
		if (freeParallaxElement.GameObjects == null || freeParallaxElement.GameObjects.Count == 0)
		{
			Debug.LogError("No game objects found at element index " + i.ToString() + ", be sure to set at least one game object for each element in the parallax");
			return;
		}
		using (List<GameObject>.Enumerator enumerator = freeParallaxElement.GameObjects.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == null)
				{
					Debug.LogError("Null game object found at element index " + i.ToString());
					return;
				}
			}
		}
		freeParallaxElement.SetupState(this, this.parallaxCamera, i);
		freeParallaxElement.SetupScale(this, this.parallaxCamera, i);
		freeParallaxElement.SetupPosition(this, this.parallaxCamera, i);
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000051E4 File Offset: 0x000033E4
	public void Reset()
	{
		this.SetupElements(false);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000051F0 File Offset: 0x000033F0
	public void SetupElements(bool randomize)
	{
		if (this.parallaxCamera == null)
		{
			this.parallaxCamera = Camera.main;
			if (this.parallaxCamera == null)
			{
				Debug.LogError("Cannot run parallax without a camera");
				return;
			}
		}
		if (this.Elements == null || this.Elements.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.Elements.Count; i++)
		{
			this.SetupElementAtIndex(i);
			if (randomize)
			{
				this.Elements[i].Randomize(this, this.parallaxCamera);
			}
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00005280 File Offset: 0x00003480
	public void AddElement(FreeParallaxElement e)
	{
		if (this.Elements == null)
		{
			this.Elements = new List<FreeParallaxElement>();
		}
		int count = this.Elements.Count;
		this.Elements.Add(e);
		this.SetupElementAtIndex(count);
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000052C0 File Offset: 0x000034C0
	public static void SetPosition(GameObject obj, Renderer r, float x, float y)
	{
		Vector3 position = new Vector3(x, y, obj.transform.position.z);
		obj.transform.position = position;
		float num = r.bounds.min.x - obj.transform.position.x;
		if (num != 0f)
		{
			position.x -= num;
			obj.transform.position = position;
		}
		float num2 = r.bounds.min.y - obj.transform.position.y;
		if (num2 != 0f)
		{
			position.y -= num2;
			obj.transform.position = position;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000537C File Offset: 0x0000357C
	public void Initialize(float originHeight)
	{
		this._originHeight = originHeight;
		this.SetupElements(this.randomize);
		this.Translate(new Vector2(0f, -originHeight));
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000053A4 File Offset: 0x000035A4
	public void Translate(Vector2 delta)
	{
		for (int i = 0; i < this.Elements.Count; i++)
		{
			this.Elements[i].Update(this, delta, this.parallaxCamera);
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000053E0 File Offset: 0x000035E0
	private void Randomize()
	{
		for (int i = 0; i < this.Elements.Count; i++)
		{
			this.Elements[i].Randomize(this, this.parallaxCamera);
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000541C File Offset: 0x0000361C
	private void Update()
	{
		Vector3 delta = this.cameraController.delta;
		delta.x *= this.Speed.x;
		delta.y *= this.Speed.y;
		foreach (FreeParallaxElement freeParallaxElement in this.Elements)
		{
			freeParallaxElement.Update(this, delta, this.parallaxCamera);
		}
	}

	// Token: 0x040000CE RID: 206
	[Tooltip("Camera to use for the parallax. Defaults to main camera.")]
	public Camera parallaxCamera;

	// Token: 0x040000CF RID: 207
	[Tooltip("The speed at which the parallax moves, which will likely be opposite from the speed at which your character moves. Elements can be set to move as a percentage of this value.")]
	public Vector2 Speed;

	// Token: 0x040000D0 RID: 208
	[Tooltip("Randomize position on initialize")]
	public bool randomize = true;

	// Token: 0x040000D1 RID: 209
	[Tooltip("The elements in the parallax.")]
	public List<FreeParallaxElement> Elements;

	// Token: 0x040000D2 RID: 210
	[Tooltip("Whether the parallax moves horizontally or vertically. Horizontal moves left and right, vertical moves up and down.")]
	public bool IsHorizontal = true;

	// Token: 0x040000D3 RID: 211
	[Tooltip("The overlap in world units for wrapping elements. This can help fix rare one pixel gaps.")]
	public float WrapOverlap;

	// Token: 0x040000D4 RID: 212
	public CameraController cameraController;

	// Token: 0x040000D5 RID: 213
	private float _originHeight;

	// Token: 0x040000D6 RID: 214
	private SpriteRenderer[] _spriteRenderers;
}
