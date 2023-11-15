using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace FX
{
	// Token: 0x0200023A RID: 570
	public class GameFadeInOut : MonoBehaviour
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x0001F18C File Offset: 0x0001D38C
		private void Awake()
		{
			this._pixelPerUnit = this._spriteRenderer.sprite.pixelsPerUnit;
			this._width = this._spriteRenderer.sprite.rect.width;
			this._height = this._spriteRenderer.sprite.rect.height;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0001F1EB File Offset: 0x0001D3EB
		public void SetFadeColor(Color color)
		{
			this._color = color;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0001F1F4 File Offset: 0x0001D3F4
		private void SetFadeAlpha(float alpha)
		{
			this._color.a = alpha;
			this._spriteRenderer.color = this._color;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0001F214 File Offset: 0x0001D414
		private void FullScreen()
		{
			Camera camera = Scene<GameBase>.instance.camera;
			float num = camera.orthographicSize * 2f;
			Vector2 vector = new Vector2(camera.aspect * num, num);
			Vector2 vector2 = Vector2.one;
			if (vector.x >= vector.y)
			{
				vector2 *= vector.x * this._pixelPerUnit / this._width;
			}
			else
			{
				vector2 *= vector.y * this._pixelPerUnit / this._height;
			}
			base.transform.localScale = vector2;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0001F2A6 File Offset: 0x0001D4A6
		public void FadeIn(float speed = 1f)
		{
			this.Activate();
			base.StartCoroutine(this.CFadeIn(speed));
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0001913A File Offset: 0x0001733A
		public void Activate()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0001F2BC File Offset: 0x0001D4BC
		public IEnumerator CFadeIn(float speed)
		{
			this.FullScreen();
			for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * speed)
			{
				this.SetFadeAlpha(1f - t);
				yield return null;
			}
			this.SetFadeAlpha(0f);
			yield break;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0001F2D2 File Offset: 0x0001D4D2
		public void FadeOut(float speed = 1f)
		{
			this.Activate();
			base.StartCoroutine(this.CFadeOut(speed));
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0001F2E8 File Offset: 0x0001D4E8
		public IEnumerator CFadeOut(float speed)
		{
			this.FullScreen();
			for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * speed)
			{
				this.SetFadeAlpha(t);
				yield return null;
			}
			this.SetFadeAlpha(1f);
			yield break;
		}

		// Token: 0x0400095E RID: 2398
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x0400095F RID: 2399
		private Color _color = Color.black;

		// Token: 0x04000960 RID: 2400
		private float _pixelPerUnit;

		// Token: 0x04000961 RID: 2401
		private float _width;

		// Token: 0x04000962 RID: 2402
		private float _height;
	}
}
