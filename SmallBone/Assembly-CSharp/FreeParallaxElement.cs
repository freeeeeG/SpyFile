using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200003F RID: 63
[Serializable]
public class FreeParallaxElement
{
	// Token: 0x06000107 RID: 263 RVA: 0x00005684 File Offset: 0x00003884
	public void SetupState(FreeParallax p, Camera c, int index)
	{
		if (this.RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOffScreen && this.RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen && this.GameObjects.Count == 1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.GameObjects[0]);
			gameObject.transform.parent = this.GameObjects[0].transform.parent;
			gameObject.transform.position = this.GameObjects[0].transform.position;
			this.GameObjects.Add(gameObject);
		}
		if (this.GameObjectRenderers.Count == 0)
		{
			foreach (GameObject gameObject2 in this.GameObjects)
			{
				Renderer component = gameObject2.GetComponent<Renderer>();
				if (component == null)
				{
					Debug.LogError("Null renderer found at element index " + index.ToString() + ", each game object in the parallax must have a renderer");
					break;
				}
				this.GameObjectRenderers.Add(component);
			}
		}
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000057A0 File Offset: 0x000039A0
	public void SetupScale(FreeParallax p, Camera c, int index)
	{
		Vector3 vector = c.ViewportToWorldPoint(Vector3.zero);
		for (int i = 0; i < this.GameObjects.Count; i++)
		{
			GameObject gameObject = this.GameObjects[i];
			Renderer renderer = this.GameObjectRenderers[i];
			Bounds bounds = renderer.bounds;
			if (this.RepositionLogic.ScaleHeight > 0f)
			{
				gameObject.transform.localScale = Vector3.one;
				float num;
				if (p.IsHorizontal)
				{
					Vector3 vector2 = c.WorldToViewportPoint(new Vector3(0f, vector.y + bounds.size.y, 0f));
					num = this.RepositionLogic.ScaleHeight / vector2.y;
				}
				else
				{
					Vector3 vector3 = c.WorldToViewportPoint(new Vector3(vector.x + bounds.size.x, 0f, 0f));
					num = this.RepositionLogic.ScaleHeight / vector3.x;
				}
				gameObject.transform.localScale = new Vector3(num, num, 1f);
				bounds = renderer.bounds;
			}
			if (this.RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOffScreen && this.RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen && this.SpeedRatioX > 0f)
			{
				if (p.IsHorizontal)
				{
					float x = c.WorldToViewportPoint(new Vector3(vector.x + bounds.size.x, 0f, 0f)).x;
					if (x < 1.1f)
					{
						Debug.LogWarning("Game object in element index " + index.ToString() + " did not fit the screen width but was asked to wrap, so it was stretched. This can be fixed by making sure any parallax graphics that wrap are at least 1.1x times the largest width resolution you support.");
						Vector3 localScale = gameObject.transform.localScale;
						if (x != 0f)
						{
							localScale.x = localScale.x * (1f / x) + 0.1f;
						}
						gameObject.transform.localScale = localScale;
					}
				}
				else
				{
					float y = c.WorldToViewportPoint(new Vector3(0f, vector.y + bounds.size.y, 0f)).y;
					if (y < 1.1f)
					{
						Debug.LogWarning("Game object in element index " + index.ToString() + " did not fit the screen height but was asked to wrap, so it was stretched. This can be fixed by making sure any parallax graphics that wrap are at least 1.1x times the largest height resolution you support.");
						Vector3 localScale2 = gameObject.transform.localScale;
						if (y != 0f)
						{
							localScale2.y = localScale2.y * (1f / y) + 0.1f;
						}
						gameObject.transform.localScale = localScale2;
					}
				}
			}
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00005A30 File Offset: 0x00003C30
	public void SetupPosition(FreeParallax p, Camera c, int index)
	{
		Vector3 vector = c.ViewportToWorldPoint(Vector3.zero);
		Vector3 vector2 = c.ViewportToWorldPoint(Vector3.one);
		float num;
		float num2;
		if (p.IsHorizontal)
		{
			num = vector2.y + 1f;
			num2 = (vector.x + vector2.x + this.GameObjectRenderers[0].bounds.size.x) / 2f;
		}
		else
		{
			num = vector2.x + 1f;
			num2 = (vector.y + vector2.y + this.GameObjectRenderers[0].bounds.size.y) / 2f;
		}
		for (int i = 0; i < this.GameObjects.Count; i++)
		{
			GameObject gameObject = this.GameObjects[i];
			Renderer renderer = this.GameObjectRenderers[i];
			if (this.RepositionLogic.SortingOrder != 0)
			{
				renderer.sortingOrder = this.RepositionLogic.SortingOrder;
			}
			if (this.RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOffScreen || this.RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen)
			{
				float x;
				float y;
				if (p.IsHorizontal)
				{
					x = ((this.RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen) ? renderer.bounds.min.x : 0f);
					y = ((this.RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen) ? renderer.bounds.min.y : (num + renderer.bounds.size.y));
				}
				else
				{
					x = ((this.RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen) ? renderer.bounds.min.x : (num + renderer.bounds.size.x));
					y = ((this.RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen) ? renderer.bounds.min.y : 0f);
				}
				FreeParallax.SetPosition(gameObject, renderer, x, y);
			}
			else
			{
				if (p.IsHorizontal)
				{
					num2 -= renderer.bounds.size.x - p.WrapOverlap;
				}
				else
				{
					num2 -= renderer.bounds.size.y - p.WrapOverlap;
				}
				gameObject.transform.rotation = Quaternion.identity;
				if (this.RepositionLogic.PositionMode == FreeParallaxPositionMode.WrapAnchorTop)
				{
					if (p.IsHorizontal)
					{
						Vector3 vector3 = c.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
						FreeParallax.SetPosition(gameObject, renderer, num2, vector3.y - renderer.bounds.size.y);
					}
					else
					{
						Vector3 vector4 = c.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
						FreeParallax.SetPosition(gameObject, renderer, vector4.x - renderer.bounds.size.x, num2 + renderer.bounds.size.y);
					}
				}
				else if (this.RepositionLogic.PositionMode == FreeParallaxPositionMode.WrapAnchorBottom)
				{
					if (p.IsHorizontal)
					{
						FreeParallax.SetPosition(gameObject, renderer, num2, vector.y);
					}
					else
					{
						FreeParallax.SetPosition(gameObject, renderer, vector.x, num2);
					}
				}
				else if (p.IsHorizontal)
				{
					FreeParallax.SetPosition(gameObject, renderer, num2, renderer.bounds.min.y);
				}
				else
				{
					FreeParallax.SetPosition(gameObject, renderer, renderer.bounds.min.x, num2);
				}
				this.GameObjects.RemoveAt(i);
				this.GameObjects.Insert(0, gameObject);
				this.GameObjectRenderers.RemoveAt(i);
				this.GameObjectRenderers.Insert(0, renderer);
			}
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00005E1C File Offset: 0x0000401C
	public void Randomize(FreeParallax p, Camera c)
	{
		if (p.IsHorizontal)
		{
			if (this.SpeedRatioX == 0f)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.GameObjects.Count; i++)
			{
				Bounds bounds = this.GameObjectRenderers[i].bounds;
				num += Math.Abs(bounds.max.x - bounds.min.x);
			}
			this.Update(p, new Vector2(UnityEngine.Random.Range(-num, num) / this.SpeedRatioX, 0f), c);
			return;
		}
		else
		{
			if (this.SpeedRatioY == 0f)
			{
				return;
			}
			float num2 = 0f;
			for (int j = 0; j < this.GameObjects.Count; j++)
			{
				Bounds bounds2 = this.GameObjectRenderers[j].bounds;
				num2 += Math.Abs(bounds2.max.y - bounds2.min.y);
			}
			this.Update(p, new Vector2(0f, UnityEngine.Random.Range(-num2, num2) / this.SpeedRatioY), c);
			return;
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00005F34 File Offset: 0x00004134
	public void Update(FreeParallax p, Vector2 delta, Camera c)
	{
		if (this.GameObjects == null || this.GameObjects.Count == 0 || this.GameObjects.Count != this.GameObjectRenderers.Count)
		{
			return;
		}
		delta.x += this.AutoScrollX;
		this.Translated += delta;
		foreach (GameObject gameObject in this.GameObjects)
		{
			gameObject.transform.Translate(delta.x * this.SpeedRatioX, delta.y * this.SpeedRatioY, 0f);
		}
		bool flag = this.RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOffScreen && this.RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen;
		float num = flag ? 0f : 1f;
		float num2;
		float num3;
		if (p.IsHorizontal)
		{
			num2 = c.rect.x - num;
			num3 = c.rect.width + num;
		}
		else
		{
			num2 = c.rect.y - num;
			num3 = c.rect.height + num;
		}
		int num4 = this.GameObjects.Count;
		for (int i = 0; i < num4; i++)
		{
			GameObject gameObject2 = this.GameObjects[i];
			Renderer renderer = this.GameObjectRenderers[i];
			Bounds bounds = renderer.bounds;
			Vector3 vector = (delta.x > 0f) ? c.WorldToViewportPoint(bounds.min) : c.WorldToViewportPoint(bounds.max);
			float num5 = p.IsHorizontal ? vector.x : vector.y;
			if (flag)
			{
				if (delta.x > 0f && num5 >= num3)
				{
					if (p.IsHorizontal)
					{
						float x = this.GameObjectRenderers[0].bounds.min.x - renderer.bounds.size.x + p.WrapOverlap;
						FreeParallax.SetPosition(gameObject2, renderer, x, renderer.bounds.min.y);
					}
					else
					{
						float y = this.GameObjectRenderers[0].bounds.min.y - renderer.bounds.size.y + p.WrapOverlap;
						FreeParallax.SetPosition(gameObject2, renderer, renderer.bounds.min.x, y);
					}
					this.GameObjects.RemoveAt(i);
					this.GameObjects.Insert(0, gameObject2);
					this.GameObjectRenderers.RemoveAt(i);
					this.GameObjectRenderers.Insert(0, renderer);
				}
				else if (delta.x < 0f && num5 <= num2)
				{
					if (p.IsHorizontal)
					{
						float x2 = this.GameObjectRenderers[this.GameObjects.Count - 1].bounds.max.x - p.WrapOverlap;
						FreeParallax.SetPosition(gameObject2, renderer, x2, renderer.bounds.min.y);
					}
					else
					{
						float y2 = this.GameObjectRenderers[this.GameObjects.Count - 1].bounds.max.y - p.WrapOverlap;
						FreeParallax.SetPosition(gameObject2, renderer, renderer.bounds.min.x, y2);
					}
					this.GameObjects.RemoveAt(i);
					this.GameObjects.Add(gameObject2);
					this.GameObjectRenderers.RemoveAt(i--);
					this.GameObjectRenderers.Add(renderer);
					num4--;
				}
			}
			else if (p.IsHorizontal)
			{
				if (delta.x > 0f && (vector.y >= c.rect.height || num5 >= num3))
				{
					if (this.RepositionLogicFunction != null)
					{
						this.RepositionLogicFunction(p, this, delta.x, gameObject2, renderer);
					}
					else
					{
						Vector3 vector2 = c.ViewportToWorldPoint(Vector3.zero);
						float x3 = UnityEngine.Random.Range(this.RepositionLogic.MinXPercent, this.RepositionLogic.MaxXPercent);
						float y3 = UnityEngine.Random.Range(this.RepositionLogic.MinYPercent, this.RepositionLogic.MaxYPercent);
						Vector3 vector3 = c.ViewportToWorldPoint(new Vector3(x3, y3));
						FreeParallax.SetPosition(gameObject2, renderer, vector2.x - vector3.x, vector3.y);
					}
				}
				else if (delta.x < 0f && (vector.y >= c.rect.height || vector.x < num2))
				{
					if (this.RepositionLogicFunction != null)
					{
						this.RepositionLogicFunction(p, this, delta.x, gameObject2, renderer);
					}
					else
					{
						Vector3 vector4 = c.ViewportToWorldPoint(Vector3.one);
						float x4 = UnityEngine.Random.Range(this.RepositionLogic.MinXPercent, this.RepositionLogic.MaxXPercent);
						float y4 = UnityEngine.Random.Range(this.RepositionLogic.MinYPercent, this.RepositionLogic.MaxYPercent);
						Vector3 vector5 = c.ViewportToWorldPoint(new Vector3(x4, y4));
						FreeParallax.SetPosition(gameObject2, renderer, vector4.x + vector5.x, vector5.y);
					}
				}
			}
			else if (delta.x > 0f && (vector.x >= c.rect.width || num5 >= num3))
			{
				if (this.RepositionLogicFunction != null)
				{
					this.RepositionLogicFunction(p, this, delta.x, gameObject2, renderer);
				}
				else
				{
					Vector3 vector6 = c.ViewportToWorldPoint(Vector3.zero);
					float x5 = UnityEngine.Random.Range(this.RepositionLogic.MinXPercent, this.RepositionLogic.MaxXPercent);
					float y5 = UnityEngine.Random.Range(this.RepositionLogic.MinYPercent, this.RepositionLogic.MaxYPercent);
					Vector3 vector7 = c.ViewportToWorldPoint(new Vector3(x5, y5));
					FreeParallax.SetPosition(gameObject2, renderer, vector7.x, vector6.y - vector7.y);
				}
			}
			else if (delta.x < 0f && (vector.x >= c.rect.width || vector.y < num2))
			{
				if (this.RepositionLogicFunction != null)
				{
					this.RepositionLogicFunction(p, this, delta.x, gameObject2, renderer);
				}
				else
				{
					Vector3 vector8 = c.ViewportToWorldPoint(Vector3.one);
					float x6 = UnityEngine.Random.Range(this.RepositionLogic.MinXPercent, this.RepositionLogic.MaxXPercent);
					float y6 = UnityEngine.Random.Range(this.RepositionLogic.MinYPercent, this.RepositionLogic.MaxYPercent);
					Vector3 vector9 = c.ViewportToWorldPoint(new Vector3(x6, y6));
					FreeParallax.SetPosition(gameObject2, renderer, vector9.x, vector8.y + vector9.y);
				}
			}
		}
	}

	// Token: 0x040000EC RID: 236
	internal readonly List<Renderer> GameObjectRenderers = new List<Renderer>();

	// Token: 0x040000ED RID: 237
	[Tooltip("Game objects to parallax. These will be cycled in sequence, which allows a long rolling background or different individual objects. If there is only one, and the reposition logic specifies to wrap, a second object will be added that is a clone of the first. It is recommended that these all be the same size.")]
	public List<GameObject> GameObjects;

	// Token: 0x040000EE RID: 238
	[Tooltip("The speed at which this object moves in relation to the speed of the parallax.")]
	[Range(-3f, 3f)]
	[FormerlySerializedAs("SpeedRatio")]
	public float SpeedRatioX;

	// Token: 0x040000EF RID: 239
	[Range(-3f, 3f)]
	public float SpeedRatioY;

	// Token: 0x040000F0 RID: 240
	public float AutoScrollX;

	// Token: 0x040000F1 RID: 241
	public Vector2 Translated;

	// Token: 0x040000F2 RID: 242
	[Tooltip("Contains logic on how this object repositions itself when moving off screen.")]
	public FreeParallaxElementRepositionLogic RepositionLogic;

	// Token: 0x040000F3 RID: 243
	[HideInInspector]
	public FreeParallaxElementRepositionLogicFunction RepositionLogicFunction;
}
