using System;
using System.Collections;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Usables
{
	// Token: 0x0200074F RID: 1871
	public sealed class EffectZone : MonoBehaviour
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x00072E10 File Offset: 0x00071010
		// (set) Token: 0x0600260D RID: 9741 RVA: 0x00072E18 File Offset: 0x00071018
		public Vector2 sizeRange
		{
			get
			{
				return this._horizontalRange;
			}
			set
			{
				this._horizontalRange = value;
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x00072E21 File Offset: 0x00071021
		static EffectZone()
		{
			EffectZone._terrainFindSize = new Vector2(1f, 0.1f);
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x00072E44 File Offset: 0x00071044
		public bool MatchKey(string[] targetKeys)
		{
			foreach (string value in targetKeys)
			{
				string[] keys = this._keys;
				for (int j = 0; j < keys.Length; j++)
				{
					if (keys[j].Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x00072E8E File Offset: 0x0007108E
		private void OnEnable()
		{
			base.StartCoroutine(this.COnEnable());
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00072E9D File Offset: 0x0007109D
		private IEnumerator COnEnable()
		{
			this._renderer.enabled = false;
			yield return null;
			this.Create();
			this._renderer.enabled = true;
			yield break;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x00072EAC File Offset: 0x000710AC
		public void Create()
		{
			RaycastHit2D raycastHit2D = default(RaycastHit2D);
			if (!TargetFinder.BoxCast(this._root.position, EffectZone._terrainFindSize, 0f, Vector2.down, 100f, this._terrainLayerMask, ref raycastHit2D))
			{
				this.SetEffectZoneScale(1f);
				return;
			}
			this.SetPosition(raycastHit2D.collider);
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x00072F10 File Offset: 0x00071110
		private void SetPosition(Collider2D platform)
		{
			Bounds bounds = platform.bounds;
			float x = this._horizontalRange.x;
			float y = this._horizontalRange.y;
			if (bounds.size.x < x)
			{
				this.SetPositionToCenter(bounds);
				this.SetEffectZoneScale(x);
				return;
			}
			if (bounds.size.x > y)
			{
				this.SetPositionToClosestSide(bounds);
				this.SetEffectZoneScale(y);
				return;
			}
			this.SetPositionToCenter(bounds);
			this.SetEffectZoneScale(bounds.size.x);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00072F91 File Offset: 0x00071191
		private void SetPositionToCenter(Bounds bounds)
		{
			this._root.position = new Vector2(bounds.center.x, bounds.max.y);
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x00072FC0 File Offset: 0x000711C0
		private void SetPositionToClosestSide(Bounds bounds)
		{
			float num = this._horizontalRange.y / 2f;
			float num2 = bounds.max.x - this._root.position.x;
			float num3 = this._root.position.x - bounds.min.x;
			float num4 = num - num2;
			float num5 = num - num3;
			if (num4 > 0f)
			{
				this._root.position = new Vector2(bounds.max.x - num, bounds.max.y);
				return;
			}
			if (num5 > 0f)
			{
				this._root.position = new Vector2(bounds.min.x + num, bounds.max.y);
			}
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x00073090 File Offset: 0x00071290
		private void SetEffectZoneScale(float sizeOfAOE)
		{
			this._renderer.transform.localScale = new Vector3(sizeOfAOE * this._sizeMultiplier, 1f, 1f);
		}

		// Token: 0x040020BE RID: 8382
		[SerializeField]
		private Transform _root;

		// Token: 0x040020BF RID: 8383
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _horizontalRange;

		// Token: 0x040020C0 RID: 8384
		[SerializeField]
		private LayerMask _terrainLayerMask;

		// Token: 0x040020C1 RID: 8385
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x040020C2 RID: 8386
		[SerializeField]
		private float _sizeMultiplier = 1f;

		// Token: 0x040020C3 RID: 8387
		[SerializeField]
		private string[] _keys;

		// Token: 0x040020C4 RID: 8388
		private static Vector2 _terrainFindSize;

		// Token: 0x040020C5 RID: 8389
		private static NonAllocCaster _caster = new NonAllocCaster(1);
	}
}
