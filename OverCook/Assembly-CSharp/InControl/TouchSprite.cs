using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002CE RID: 718
	[Serializable]
	public class TouchSprite
	{
		// Token: 0x06000ED0 RID: 3792 RVA: 0x00047B84 File Offset: 0x00045F84
		public TouchSprite()
		{
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00047BF4 File Offset: 0x00045FF4
		public TouchSprite(float size)
		{
			this.size = Vector2.one * size;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x00047C72 File Offset: 0x00046072
		// (set) Token: 0x06000ED3 RID: 3795 RVA: 0x00047C7A File Offset: 0x0004607A
		public bool Dirty { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00047C83 File Offset: 0x00046083
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x00047C8B File Offset: 0x0004608B
		public bool Ready { get; set; }

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00047C94 File Offset: 0x00046094
		public void Create(string gameObjectName, Transform parentTransform, int sortingOrder)
		{
			this.spriteGameObject = this.CreateSpriteGameObject(gameObjectName, parentTransform);
			this.spriteRenderer = this.CreateSpriteRenderer(this.spriteGameObject, this.idleSprite, sortingOrder);
			this.spriteRenderer.color = this.idleColor;
			this.Ready = true;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00047CE0 File Offset: 0x000460E0
		public void Delete()
		{
			this.Ready = false;
			UnityEngine.Object.Destroy(this.spriteGameObject);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00047CF4 File Offset: 0x000460F4
		public void Update()
		{
			this.Update(false);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00047D00 File Offset: 0x00046100
		public void Update(bool forceUpdate)
		{
			if (this.Dirty || forceUpdate)
			{
				if (this.spriteRenderer != null)
				{
					this.spriteRenderer.sprite = ((!this.State) ? this.idleSprite : this.busySprite);
				}
				if (this.sizeUnitType == TouchUnitType.Pixels)
				{
					Vector2 a = TouchUtility.RoundVector(this.size);
					this.ScaleSpriteInPixels(this.spriteGameObject, this.spriteRenderer, a);
					this.worldSize = a * TouchManager.PixelToWorld;
				}
				else
				{
					this.ScaleSpriteInPercent(this.spriteGameObject, this.spriteRenderer, this.size);
					if (this.lockAspectRatio)
					{
						this.worldSize = this.size * TouchManager.PercentToWorld;
					}
					else
					{
						this.worldSize = Vector2.Scale(this.size, TouchManager.ViewSize);
					}
				}
				this.Dirty = false;
			}
			if (this.spriteRenderer != null)
			{
				Color color = (!this.State) ? this.idleColor : this.busyColor;
				if (this.spriteRenderer.color != color)
				{
					this.spriteRenderer.color = Utility.MoveColorTowards(this.spriteRenderer.color, color, 5f * Time.deltaTime);
				}
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00047E64 File Offset: 0x00046264
		private GameObject CreateSpriteGameObject(string name, Transform parentTransform)
		{
			return new GameObject(name)
			{
				transform = 
				{
					parent = parentTransform,
					localPosition = Vector3.zero,
					localScale = Vector3.one
				},
				layer = parentTransform.gameObject.layer
			};
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00047EB8 File Offset: 0x000462B8
		private SpriteRenderer CreateSpriteRenderer(GameObject spriteGameObject, Sprite sprite, int sortingOrder)
		{
			SpriteRenderer spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
			spriteRenderer.sortingOrder = sortingOrder;
			spriteRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
			spriteRenderer.sharedMaterial.SetFloat("PixelSnap", 1f);
			return spriteRenderer;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00047F08 File Offset: 0x00046308
		private void ScaleSpriteInPixels(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			float num = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.bounds.size.x;
			float num2 = TouchManager.PixelToWorld * num;
			float x = num2 * size.x / spriteRenderer.sprite.rect.width;
			float y = num2 * size.y / spriteRenderer.sprite.rect.height;
			spriteGameObject.transform.localScale = new Vector3(x, y);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00047FD0 File Offset: 0x000463D0
		private void ScaleSpriteInPercent(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			if (this.lockAspectRatio)
			{
				float num = Mathf.Min(TouchManager.ViewSize.x, TouchManager.ViewSize.y);
				float x = num * size.x / spriteRenderer.sprite.bounds.size.x;
				float y = num * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(x, y);
			}
			else
			{
				float x2 = TouchManager.ViewSize.x * size.x / spriteRenderer.sprite.bounds.size.x;
				float y2 = TouchManager.ViewSize.y * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(x2, y2);
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0004811C File Offset: 0x0004651C
		public bool Contains(Vector2 testWorldPoint)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				float num = (testWorldPoint.x - this.Position.x) / this.worldSize.x;
				float num2 = (testWorldPoint.y - this.Position.y) / this.worldSize.y;
				return num * num + num2 * num2 < 0.25f;
			}
			float num3 = Utility.Abs(testWorldPoint.x - this.Position.x) * 2f;
			float num4 = Utility.Abs(testWorldPoint.y - this.Position.y) * 2f;
			return num3 <= this.worldSize.x && num4 <= this.worldSize.y;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000481F8 File Offset: 0x000465F8
		public bool Contains(Touch touch)
		{
			return this.Contains(TouchManager.ScreenToWorldPoint(touch.position));
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00048210 File Offset: 0x00046610
		public void DrawGizmos(Vector3 position, Color color)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				Utility.DrawOvalGizmo(position, this.WorldSize, color);
			}
			else
			{
				Utility.DrawRectGizmo(position, this.WorldSize, color);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x00048246 File Offset: 0x00046646
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x0004824E File Offset: 0x0004664E
		public bool State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					this.state = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0004826A File Offset: 0x0004666A
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x00048272 File Offset: 0x00046672
		public Sprite BusySprite
		{
			get
			{
				return this.busySprite;
			}
			set
			{
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00048293 File Offset: 0x00046693
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x0004829B File Offset: 0x0004669B
		public Sprite IdleSprite
		{
			get
			{
				return this.idleSprite;
			}
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x000482BC File Offset: 0x000466BC
		public Sprite Sprite
		{
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x000482FC File Offset: 0x000466FC
		// (set) Token: 0x06000EE9 RID: 3817 RVA: 0x00048304 File Offset: 0x00046704
		public Color BusyColor
		{
			get
			{
				return this.busyColor;
			}
			set
			{
				if (this.busyColor != value)
				{
					this.busyColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x00048325 File Offset: 0x00046725
		// (set) Token: 0x06000EEB RID: 3819 RVA: 0x0004832D File Offset: 0x0004672D
		public Color IdleColor
		{
			get
			{
				return this.idleColor;
			}
			set
			{
				if (this.idleColor != value)
				{
					this.idleColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x0004834E File Offset: 0x0004674E
		// (set) Token: 0x06000EED RID: 3821 RVA: 0x00048356 File Offset: 0x00046756
		public TouchSpriteShape Shape
		{
			get
			{
				return this.shape;
			}
			set
			{
				if (this.shape != value)
				{
					this.shape = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x00048372 File Offset: 0x00046772
		// (set) Token: 0x06000EEF RID: 3823 RVA: 0x0004837A File Offset: 0x0004677A
		public TouchUnitType SizeUnitType
		{
			get
			{
				return this.sizeUnitType;
			}
			set
			{
				if (this.sizeUnitType != value)
				{
					this.sizeUnitType = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x00048396 File Offset: 0x00046796
		// (set) Token: 0x06000EF1 RID: 3825 RVA: 0x0004839E File Offset: 0x0004679E
		public Vector2 Size
		{
			get
			{
				return this.size;
			}
			set
			{
				if (this.size != value)
				{
					this.size = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x000483BF File Offset: 0x000467BF
		public Vector2 WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x000483C7 File Offset: 0x000467C7
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x000483F3 File Offset: 0x000467F3
		public Vector3 Position
		{
			get
			{
				return (!this.spriteGameObject) ? Vector3.zero : this.spriteGameObject.transform.position;
			}
			set
			{
				if (this.spriteGameObject)
				{
					this.spriteGameObject.transform.position = value;
				}
			}
		}

		// Token: 0x04000BA3 RID: 2979
		[SerializeField]
		private Sprite idleSprite;

		// Token: 0x04000BA4 RID: 2980
		[SerializeField]
		private Sprite busySprite;

		// Token: 0x04000BA5 RID: 2981
		[SerializeField]
		private Color idleColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04000BA6 RID: 2982
		[SerializeField]
		private Color busyColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000BA7 RID: 2983
		[SerializeField]
		private TouchSpriteShape shape;

		// Token: 0x04000BA8 RID: 2984
		[SerializeField]
		private TouchUnitType sizeUnitType;

		// Token: 0x04000BA9 RID: 2985
		[SerializeField]
		private Vector2 size = new Vector2(10f, 10f);

		// Token: 0x04000BAA RID: 2986
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x04000BAB RID: 2987
		[SerializeField]
		[HideInInspector]
		private Vector2 worldSize;

		// Token: 0x04000BAC RID: 2988
		private Transform spriteParentTransform;

		// Token: 0x04000BAD RID: 2989
		private GameObject spriteGameObject;

		// Token: 0x04000BAE RID: 2990
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000BAF RID: 2991
		private bool state;
	}
}
