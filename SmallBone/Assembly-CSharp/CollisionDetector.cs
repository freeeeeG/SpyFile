using System;
using Characters;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

// Token: 0x02000024 RID: 36
[Serializable]
public class CollisionDetector
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000075 RID: 117 RVA: 0x00003B20 File Offset: 0x00001D20
	// (remove) Token: 0x06000076 RID: 118 RVA: 0x00003B58 File Offset: 0x00001D58
	public event CollisionDetector.onTerrainHitDelegate onTerrainHit;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000077 RID: 119 RVA: 0x00003B90 File Offset: 0x00001D90
	// (remove) Token: 0x06000078 RID: 120 RVA: 0x00003BC8 File Offset: 0x00001DC8
	public event CollisionDetector.onTargetHitDelegate onHit;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000079 RID: 121 RVA: 0x00003C00 File Offset: 0x00001E00
	// (remove) Token: 0x0600007A RID: 122 RVA: 0x00003C38 File Offset: 0x00001E38
	public event Action onStop;

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600007B RID: 123 RVA: 0x00003C6D File Offset: 0x00001E6D
	// (set) Token: 0x0600007C RID: 124 RVA: 0x00003C7A File Offset: 0x00001E7A
	public LayerMask layerMask
	{
		get
		{
			return this._filter.layerMask;
		}
		set
		{
			this._filter.layerMask = value;
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00003C98 File Offset: 0x00001E98
	internal void Initialize()
	{
		this._hits.Clear();
		this._propPenetratingHits = 0;
		if (this._collider != null)
		{
			this._collider.enabled = false;
		}
		if (this._maxHitsPerUnit == 0)
		{
			this._maxHitsPerUnit = int.MaxValue;
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00003CE4 File Offset: 0x00001EE4
	internal void Detect(Vector2 origin, Vector2 distance)
	{
		this.Detect(origin, distance.normalized, distance.magnitude);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00003CFC File Offset: 0x00001EFC
	internal void Detect(Vector2 origin, Vector2 direction, float distance)
	{
		CollisionDetector._caster.contactFilter.SetLayerMask(this._filter.layerMask);
		CollisionDetector._caster.RayCast(origin, direction, distance);
		if (this._collider)
		{
			this._collider.enabled = true;
			CollisionDetector._caster.ColliderCast(this._collider, direction, distance);
			this._collider.enabled = false;
		}
		else
		{
			CollisionDetector._caster.RayCast(origin, direction, distance);
		}
		int i = 0;
		while (i < CollisionDetector._caster.results.Count)
		{
			if (this._terrainLayer.Contains(CollisionDetector._caster.results[i].collider.gameObject.layer))
			{
				this.onTerrainHit(this._collider, origin, direction, distance, CollisionDetector._caster.results[i]);
				goto IL_1BB;
			}
			Target component = CollisionDetector._caster.results[i].collider.GetComponent<Target>();
			if (this._hits.CanAttack(component, this._maxHits, this._maxHitsPerUnit, this._hitIntervalPerUnit))
			{
				if (component.character != null)
				{
					if (component.character.liveAndActive)
					{
						this.onHit(this._collider, origin, direction, distance, CollisionDetector._caster.results[i], component);
						this._hits.AddOrUpdate(component);
						goto IL_1BB;
					}
				}
				else
				{
					if (component.damageable != null)
					{
						this.onHit(this._collider, origin, direction, distance, CollisionDetector._caster.results[i], component);
						if (!component.damageable.blockCast)
						{
							this._propPenetratingHits++;
						}
						this._hits.AddOrUpdate(component);
						goto IL_1BB;
					}
					goto IL_1BB;
				}
			}
			IL_1E6:
			i++;
			continue;
			IL_1BB:
			if (this._hits.Count - this._propPenetratingHits < this._maxHits)
			{
				goto IL_1E6;
			}
			Action action = this.onStop;
			if (action == null)
			{
				goto IL_1E6;
			}
			action();
			goto IL_1E6;
		}
	}

	// Token: 0x0400006E RID: 110
	private const int maxHits = 99;

	// Token: 0x04000072 RID: 114
	private GameObject _owner;

	// Token: 0x04000073 RID: 115
	[SerializeField]
	private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

	// Token: 0x04000074 RID: 116
	[SerializeField]
	private LayerMask _terrainLayer = Layers.groundMask;

	// Token: 0x04000075 RID: 117
	[SerializeField]
	private Collider2D _collider;

	// Token: 0x04000076 RID: 118
	[Range(1f, 99f)]
	[SerializeField]
	private int _maxHits = 15;

	// Token: 0x04000077 RID: 119
	[SerializeField]
	private int _maxHitsPerUnit = 1;

	// Token: 0x04000078 RID: 120
	[SerializeField]
	private float _hitIntervalPerUnit = 0.5f;

	// Token: 0x04000079 RID: 121
	private HitHistoryManager _hits = new HitHistoryManager(99);

	// Token: 0x0400007A RID: 122
	private int _propPenetratingHits;

	// Token: 0x0400007B RID: 123
	private ContactFilter2D _filter;

	// Token: 0x0400007C RID: 124
	private static readonly NonAllocCaster _caster = new NonAllocCaster(99);

	// Token: 0x02000025 RID: 37
	// (Invoke) Token: 0x06000083 RID: 131
	public delegate void onTerrainHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit);

	// Token: 0x02000026 RID: 38
	// (Invoke) Token: 0x06000087 RID: 135
	public delegate void onTargetHitDelegate(Collider2D collider, Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target);
}
