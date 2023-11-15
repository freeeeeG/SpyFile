using System;
using System.Collections;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Usables
{
	// Token: 0x02000752 RID: 1874
	[RequireComponent(typeof(PoolObject))]
	public class Liquid : MonoBehaviour
	{
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000731A9 File Offset: 0x000713A9
		public float size
		{
			get
			{
				return this._collider.size.x;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x000731BB File Offset: 0x000713BB
		public int stack
		{
			get
			{
				return this._stack;
			}
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000731C3 File Offset: 0x000713C3
		public void Awake()
		{
			this._renderers = new SpriteRenderer[]
			{
				this._body,
				this._leftSide,
				this._rightSide
			};
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000731EC File Offset: 0x000713EC
		public bool IsSameTerrain(Collider2D terrain)
		{
			return this._terrainHitInfo.collider == terrain;
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x00073200 File Offset: 0x00071400
		public Liquid Spawn(Vector2 position, LiquidMaster master)
		{
			Liquid liquid = this.Spawn(position);
			liquid._master = master;
			liquid._master.Add(liquid);
			return liquid;
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0007322C File Offset: 0x0007142C
		public Liquid Spawn(Vector2 position)
		{
			Liquid component = this._poolObject.Spawn(true).GetComponent<Liquid>();
			component.transform.position = position;
			component._stack = 1;
			component.StartCoroutine("CActivate");
			Singleton<Service>.Instance.levelManager.onMapLoaded -= component.Despawn;
			Singleton<Service>.Instance.levelManager.onMapLoaded += component.Despawn;
			return component;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000732A6 File Offset: 0x000714A6
		private IEnumerator CActivate()
		{
			this._body.enabled = false;
			yield return null;
			this._body.enabled = true;
			Color color = this._body.color;
			this._body.color = new Color(color.r, color.g, color.b, 1f);
			this.Activate();
			yield break;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000732B5 File Offset: 0x000714B5
		private IEnumerator CFadeOut()
		{
			yield return Chronometer.global.WaitForSeconds(this._lifeTime);
			if (this._master != null)
			{
				this._master.Remove(this);
			}
			if (this._fadeOut.length > 0)
			{
				yield return this._poolObject.CFadeOut(this._renderers, Chronometer.global, this._fadeOut, this._fadeOutDuration);
			}
			yield break;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000732C4 File Offset: 0x000714C4
		private void Activate()
		{
			this._collider.size = new Vector2(this._spriteUnitSize, this._collider.size.y);
			this._body.size = new Vector2(this._spriteUnitSize, this._body.size.y);
			this._terrainHitInfo = default(RaycastHit2D);
			this._hit = TargetFinder.BoxCast(base.transform.position, new Vector2(this._collider.size.x, 0.1f), 0f, Vector2.down, 100f, this._terrainMask, ref this._terrainHitInfo);
			NonAllocOverlapper nonAllocOverlapper = new NonAllocOverlapper(99);
			nonAllocOverlapper.contactFilter.SetLayerMask(4096);
			if (nonAllocOverlapper.OverlapCollider(this._collider).GetComponents<Liquid>(true).Count == 0)
			{
				this.MakeDefaultShape();
			}
			else
			{
				this.MakeLiquidShape();
			}
			this._leftSide.color = Color.white;
			this._rightSide.color = Color.white;
			this._leftSide.transform.localPosition = new Vector2(-this._body.size.x / 2f, 0f);
			this._rightSide.transform.localPosition = new Vector2(this._body.size.x / 2f, 0f);
			if (this._lifeTime > 0f)
			{
				base.StartCoroutine(this.CFadeOut());
			}
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x00073460 File Offset: 0x00071660
		private void MakeDefaultShape()
		{
			float spriteUnitSize = this._spriteUnitSize;
			Vector2 v = base.transform.position;
			this.Clamp(base.transform.position, ref spriteUnitSize, ref v);
			base.transform.position = v;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000734B0 File Offset: 0x000716B0
		private void MakeLiquidShape()
		{
			Liquid._sharedOverlapper.contactFilter.SetLayerMask(4096);
			List<Liquid> list = Liquid._sharedOverlapper.OverlapCollider(this._collider).GetComponents<Liquid>(true);
			list = list.FindAll((Liquid target) => target.IsSameTerrain(this._terrainHitInfo.collider));
			if (list.Count == 1)
			{
				if (list[0] == this)
				{
					return;
				}
				this.Expand(list[0]);
				this.MakeLiquidShape();
			}
			if (list.Count >= 2)
			{
				this.Combine(list[0], list[1]);
				this.MakeLiquidShape();
			}
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x00073550 File Offset: 0x00071750
		private void Expand(Liquid liquid)
		{
			float num = this._body.size.x / 2f;
			float num2 = liquid.transform.position.x + num;
			float num3 = liquid.transform.position.x - num;
			float x = liquid.size + this._body.size.x;
			float y = this._body.size.y;
			Vector2 vector = liquid.transform.position;
			if (base.transform.position.x >= num2)
			{
				float num4 = this._body.size.x / 2f;
				vector = new Vector2(liquid.transform.position.x + num4, base.transform.position.y);
			}
			else if (base.transform.position.x <= num3)
			{
				float num5 = this._body.size.x / 2f;
				vector = new Vector2(liquid.transform.position.x - num5, base.transform.position.y);
			}
			this.Clamp(vector, ref x, ref vector);
			base.transform.position = vector;
			this._collider.size = new Vector2(x, y);
			this._body.size = new Vector2(x, y);
			this._stack += liquid.stack;
			liquid.Despawn();
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000736E0 File Offset: 0x000718E0
		private void Combine(Liquid liquid1, Liquid liquid2)
		{
			float x = liquid1.size + liquid2.size + this._spriteUnitSize;
			float y = this._body.size.y;
			Vector2 v = base.transform.position;
			this.Clamp(base.transform.position, ref x, ref v);
			this._collider.size = new Vector2(x, y);
			this._body.size = new Vector2(x, y);
			base.transform.position = v;
			this._stack += liquid1.stack + liquid2.stack;
			liquid1.Despawn();
			liquid2.Despawn();
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0007379C File Offset: 0x0007199C
		private void Clamp(Vector2 originPosition, ref float width, ref Vector2 position)
		{
			if (!this._hit)
			{
				return;
			}
			Bounds bounds = this._terrainHitInfo.collider.bounds;
			if (width >= bounds.size.x)
			{
				width = bounds.size.x;
				position = new Vector2(bounds.center.x, originPosition.y);
				return;
			}
			float num = width / 2f;
			float num2 = position.x - num;
			float num3 = position.x + num;
			if (num3 > bounds.max.x)
			{
				float num4 = num3 - bounds.max.x;
				position = new Vector2(originPosition.x - num4, originPosition.y);
				return;
			}
			if (num2 < bounds.min.x)
			{
				float num5 = bounds.min.x - num2;
				position = new Vector2(originPosition.x + num5, originPosition.y);
				return;
			}
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00073890 File Offset: 0x00071A90
		private void Despawn()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Despawn;
			if (this._master != null)
			{
				this._master.Remove(this);
			}
			this._poolObject.Despawn();
		}

		// Token: 0x040020CC RID: 8396
		[SerializeField]
		[GetComponent]
		private PoolObject _poolObject;

		// Token: 0x040020CD RID: 8397
		[SerializeField]
		private float _spriteUnitSize;

		// Token: 0x040020CE RID: 8398
		[SerializeField]
		private LayerMask _terrainMask;

		// Token: 0x040020CF RID: 8399
		[SerializeField]
		private SpriteRenderer _body;

		// Token: 0x040020D0 RID: 8400
		[SerializeField]
		private SpriteRenderer _leftSide;

		// Token: 0x040020D1 RID: 8401
		[SerializeField]
		private SpriteRenderer _rightSide;

		// Token: 0x040020D2 RID: 8402
		[SerializeField]
		private BoxCollider2D _collider;

		// Token: 0x040020D3 RID: 8403
		[SerializeField]
		private float _lifeTime;

		// Token: 0x040020D4 RID: 8404
		[SerializeField]
		private float _fadeOutDuration;

		// Token: 0x040020D5 RID: 8405
		[SerializeField]
		private AnimationCurve _fadeOut;

		// Token: 0x040020D6 RID: 8406
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

		// Token: 0x040020D7 RID: 8407
		private LiquidMaster _master;

		// Token: 0x040020D8 RID: 8408
		private RaycastHit2D _terrainHitInfo;

		// Token: 0x040020D9 RID: 8409
		private bool _hit;

		// Token: 0x040020DA RID: 8410
		private int _stack = 1;

		// Token: 0x040020DB RID: 8411
		private SpriteRenderer[] _renderers;
	}
}
