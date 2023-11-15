using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace Level
{
	// Token: 0x020004FF RID: 1279
	public class ParallaxBackground : MonoBehaviour
	{
		// Token: 0x06001936 RID: 6454 RVA: 0x0004F130 File Offset: 0x0004D330
		private void Awake()
		{
			ParallaxBackground.Element[] values = this._elements.values;
			for (int i = 0; i < values.Length; i++)
			{
				values[i].Initialize();
			}
			this._cameraController = Scene<GameBase>.instance.cameraController;
			this.UpdateElements(this._cameraController.delta, Chronometer.global.deltaTime);
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0004F18A File Offset: 0x0004D38A
		private void Update()
		{
			this.UpdateElements(this._cameraController.delta, Chronometer.global.deltaTime);
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0004F1A7 File Offset: 0x0004D3A7
		public void Initialize(float originHeight)
		{
			this.UpdateElements(new Vector3(0f, originHeight, 0f), 0f);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0004F1C4 File Offset: 0x0004D3C4
		private void UpdateElements(Vector3 delta, float deltaTime)
		{
			ParallaxBackground.Element[] values = this._elements.values;
			for (int i = 0; i < values.Length; i++)
			{
				values[i].Update(delta, deltaTime);
			}
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0004F1FC File Offset: 0x0004D3FC
		private void SetFadeAlpha(float alpha)
		{
			ParallaxBackground.Element[] values = this._elements.values;
			for (int i = 0; i < values.Length; i++)
			{
				values[i].SetAlpha(alpha);
			}
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0004F22C File Offset: 0x0004D42C
		public void FadeIn()
		{
			base.StartCoroutine(this.CFadeIn());
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0004F23B File Offset: 0x0004D43B
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

		// Token: 0x0600193D RID: 6461 RVA: 0x0004F24A File Offset: 0x0004D44A
		public void FadeOut()
		{
			base.StartCoroutine(this.CFadeOut());
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0004F259 File Offset: 0x0004D459
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

		// Token: 0x040015FC RID: 5628
		private const float _screenWidth = 2560f;

		// Token: 0x040015FD RID: 5629
		private const float _pixelPerUnit = 32f;

		// Token: 0x040015FE RID: 5630
		private const float _pixel = 0.03125f;

		// Token: 0x040015FF RID: 5631
		[SerializeField]
		private ParallaxBackground.Element.Reorderable _elements;

		// Token: 0x04001600 RID: 5632
		private CameraController _cameraController;

		// Token: 0x02000500 RID: 1280
		[Serializable]
		private class Element
		{
			// Token: 0x06001940 RID: 6464 RVA: 0x0004F268 File Offset: 0x0004D468
			internal void Initialize()
			{
				this._spriteSize = this._spriteRenderer.sprite.bounds.size;
				int num = Mathf.CeilToInt(80f / this._spriteSize.x);
				this._origin = new Vector2[num];
				this._rendererInstances = new SpriteRenderer[num];
				this._rendererInstances[0] = this._spriteRenderer;
				for (int i = 1; i < this._rendererInstances.Length; i++)
				{
					this._rendererInstances[i] = UnityEngine.Object.Instantiate<SpriteRenderer>(this._spriteRenderer, this._spriteRenderer.transform.parent);
				}
				this._mostRight = (float)(this._rendererInstances.Length - 1) * this._spriteSize.x / 2f;
				this._mostLeft = -this._mostRight;
				for (int j = 0; j < this._rendererInstances.Length; j++)
				{
					this._origin[j] = new Vector2(this._mostLeft + this._spriteSize.x * (float)j - 0.03125f * (float)j, this._spriteSize.y / 2f - Camera.main.orthographicSize);
					this._rendererInstances[j].transform.localPosition = this._origin[j];
				}
				if (this._randomize)
				{
					this._translated.x = this._translated.x + UnityEngine.Random.Range(0f, this._spriteSize.x);
				}
			}

			// Token: 0x06001941 RID: 6465 RVA: 0x0004F3E8 File Offset: 0x0004D5E8
			internal void Update(Vector2 delta, float deltaTime)
			{
				delta.x += this._hotizontalAutoScroll * deltaTime;
				delta = Vector2.Scale(delta, this._distance);
				this._translated -= delta;
				if (this._translated.x < this._mostLeft)
				{
					this._translated.x = this._mostRight;
				}
				if (this._translated.x > this._mostRight)
				{
					this._translated.x = this._mostLeft;
				}
				for (int i = 0; i < this._rendererInstances.Length; i++)
				{
					Component component = this._rendererInstances[i];
					Vector2 v = this._origin[i] + this._translated;
					component.transform.localPosition = v;
				}
			}

			// Token: 0x06001942 RID: 6466 RVA: 0x0004F4B4 File Offset: 0x0004D6B4
			internal void SetAlpha(float alpha)
			{
				foreach (SpriteRenderer spriteRenderer in this._rendererInstances)
				{
					Color color = spriteRenderer.color;
					Color color2 = new Color(color.r, color.g, color.b, alpha);
					spriteRenderer.color = color2;
				}
			}

			// Token: 0x04001601 RID: 5633
			[SerializeField]
			private SpriteRenderer _spriteRenderer;

			// Token: 0x04001602 RID: 5634
			[SerializeField]
			private bool _randomize = true;

			// Token: 0x04001603 RID: 5635
			[SerializeField]
			private Vector2 _distance;

			// Token: 0x04001604 RID: 5636
			[SerializeField]
			private float _hotizontalAutoScroll;

			// Token: 0x04001605 RID: 5637
			private Vector2 _spriteSize;

			// Token: 0x04001606 RID: 5638
			private SpriteRenderer[] _rendererInstances;

			// Token: 0x04001607 RID: 5639
			private Vector2[] _origin;

			// Token: 0x04001608 RID: 5640
			private Vector2 _translated;

			// Token: 0x04001609 RID: 5641
			private float _mostLeft;

			// Token: 0x0400160A RID: 5642
			private float _mostRight;

			// Token: 0x02000501 RID: 1281
			[Serializable]
			internal class Reorderable : ReorderableArray<ParallaxBackground.Element>
			{
			}
		}
	}
}
