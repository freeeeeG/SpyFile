using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009EF RID: 2543
public static class InteractWithItemHelper
{
	// Token: 0x060031A1 RID: 12705 RVA: 0x000E8550 File Offset: 0x000E6950
	private static GridIndex[] GetGridOffsets()
	{
		return InteractWithItemHelper.s_gridOffsetsXZ;
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x000E8558 File Offset: 0x000E6958
	public static Collider GetFacingGridOccupant(Transform _scanner, int _layermask)
	{
		InteractWithItemHelper.s_gridColliderData.Clear();
		int activeCount = GridManager.GetActiveCount();
		for (int i = 0; i < activeCount; i++)
		{
			GridManager active = GridManager.GetActive(i);
			GridIndex unclampedGridLocationFromPos = active.GetUnclampedGridLocationFromPos(_scanner.position);
			GameObject gridOccupant = active.GetGridOccupant(unclampedGridLocationFromPos);
			if (!(gridOccupant != null) || gridOccupant.CompareTag("Hazard") || gridOccupant.CompareTag("Travelator") || gridOccupant.CompareTag("MovingPlatform"))
			{
				GridIndex[] gridOffsets = InteractWithItemHelper.GetGridOffsets();
				for (int j = 0; j < gridOffsets.Length; j++)
				{
					Collider colliderAtGridIndex = InteractWithItemHelper.GetColliderAtGridIndex(active, unclampedGridLocationFromPos + gridOffsets[j], _layermask);
					if (colliderAtGridIndex != null)
					{
						InteractWithItemHelper.GridColliderData item = default(InteractWithItemHelper.GridColliderData);
						item.Manager = active;
						item.Index = gridOffsets[j];
						item.Subject = colliderAtGridIndex;
						InteractWithItemHelper.s_gridColliderData.Add(item);
					}
				}
			}
		}
		InteractWithItemHelper.GridColliderData? gridColliderData = null;
		float num = 0f;
		for (int k = 0; k < InteractWithItemHelper.s_gridColliderData.Count; k++)
		{
			float num2 = InteractWithItemHelper.ScoreFacingGridCollider(InteractWithItemHelper.s_gridColliderData[k], _scanner.forward);
			if (gridColliderData == null || num2 < num)
			{
				gridColliderData = new InteractWithItemHelper.GridColliderData?(InteractWithItemHelper.s_gridColliderData[k]);
				num = num2;
			}
		}
		InteractWithItemHelper.s_gridColliderData.Clear();
		if (gridColliderData != null)
		{
			return gridColliderData.Value.Subject;
		}
		return null;
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x000E870C File Offset: 0x000E6B0C
	private static float ScoreFacingGridCollider(InteractWithItemHelper.GridColliderData _data, Vector3 _forward)
	{
		Vector2 lhs = _data.Manager.transform.InverseTransformDirection(_forward).XZ();
		float num = Vector2.Dot(lhs, new Vector2((float)_data.Index.X, (float)_data.Index.Z));
		if (num < Mathf.Cos(2.3561945f))
		{
			return float.MaxValue;
		}
		return 1f - num;
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x000E8774 File Offset: 0x000E6B74
	private static Collider GetColliderAtGridIndex(GridManager _gridManager, GridIndex _index, int _layermask)
	{
		GameObject gridOccupant = _gridManager.GetGridOccupant(_index);
		if (gridOccupant != null && gridOccupant.GetComponent<Collider>() != null && ((_layermask & 1 << gridOccupant.layer) != 0 || _layermask == -1))
		{
			return gridOccupant.GetComponent<Collider>();
		}
		return null;
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x000E87C7 File Offset: 0x000E6BC7
	public static bool OwnedByGrid(Transform _test)
	{
		return ComponentCache<IGridLocation>.GetComponent(_test.gameObject) != null || (_test.parent != null && InteractWithItemHelper.OwnedByGrid(_test.parent));
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x000E87FC File Offset: 0x000E6BFC
	public static Vector3 GetClosestPointOnSurface(Collider _collider, Vector3 _position)
	{
		Transform transform = _collider.transform;
		SphereCollider sphereCollider = _collider as SphereCollider;
		if (sphereCollider != null)
		{
			Vector3 vector = transform.TransformPoint(sphereCollider.center);
			return vector + (_position - vector).normalized * sphereCollider.radius;
		}
		CapsuleCollider capsuleCollider = _collider as CapsuleCollider;
		if (capsuleCollider != null)
		{
			Vector3 vector2 = transform.InverseTransformPoint(_position) - capsuleCollider.center;
			float num = Mathf.Max(capsuleCollider.height - 2f * capsuleCollider.radius, 0f);
			Vector3 a;
			if (vector2.y >= -0.5f * num && vector2.y <= 0.5f * num)
			{
				a = (vector2.WithY(0f).normalized * capsuleCollider.radius).WithY(vector2.y);
			}
			else if (vector2.y > 0.5f * num)
			{
				Vector3 vector3 = new Vector3(0f, 0.5f * num, 0f);
				a = vector3 + (vector2 - vector3).normalized * capsuleCollider.radius;
			}
			else
			{
				Vector3 vector4 = new Vector3(0f, -0.5f * num, 0f);
				a = vector4 + (vector2 - vector4).normalized * capsuleCollider.radius;
			}
			return transform.TransformPoint(a + capsuleCollider.center);
		}
		return _collider.ClosestPointOnBounds(_position);
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x000E89B0 File Offset: 0x000E6DB0
	public static bool IsColliderInArc(Collider _collider, Vector3 _position, Vector3 _forward, float _radius, float _arc)
	{
		Vector3 closestPointOnSurface = InteractWithItemHelper.GetClosestPointOnSurface(_collider, _position);
		float num = Mathf.Cos(0.5f * _arc);
		Vector3 vector = (closestPointOnSurface - _position).WithY(0f);
		return vector.sqrMagnitude > 0.001f && vector.sqrMagnitude < _radius * _radius && Vector3.Dot(_forward, vector.normalized) >= num;
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x000E8A1C File Offset: 0x000E6E1C
	public static Vector3 GetCollidersInArc(float detectionRadius, float arc, Transform _transform, Collider[] _colliders, int _layerMask, bool _gridSelection)
	{
		Vector3 position = _transform.position;
		Vector3 normalized = _transform.forward.WithY(0f).normalized;
		int num = Physics.OverlapSphereNonAlloc(position, 2f * detectionRadius, _colliders, _layerMask);
		for (int i = 0; i < num; i++)
		{
			Collider collider = _colliders[i];
			if (_transform.gameObject == collider.gameObject)
			{
				_colliders[i] = null;
			}
			else if (!InteractWithItemHelper.IsColliderInArc(collider, position, normalized, detectionRadius, arc))
			{
				_colliders[i] = null;
			}
			else if (_gridSelection && InteractWithItemHelper.OwnedByGrid(collider.transform))
			{
				_colliders[i] = null;
			}
		}
		if (_gridSelection)
		{
			Collider facingGridOccupant = InteractWithItemHelper.GetFacingGridOccupant(_transform, _layerMask);
			if (facingGridOccupant != null)
			{
				bool flag = false;
				for (int j = 0; j < _colliders.Length; j++)
				{
					Collider x = _colliders[j];
					if (x == null)
					{
						_colliders[j] = facingGridOccupant;
						flag = true;
						if (j >= num)
						{
							num = j;
						}
						break;
					}
				}
				if (!flag)
				{
				}
			}
		}
		for (int k = num; k < _colliders.Length; k++)
		{
			_colliders[k] = null;
		}
		return position + 0.5f * detectionRadius * normalized;
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x000E8B70 File Offset: 0x000E6F70
	public static GameObject ScanForObject(Collider[] _colliders, Vector3 _targetPosition, InteractWithItemHelper.ScanCondition _condition = null)
	{
		float num = float.PositiveInfinity;
		GameObject result = null;
		foreach (Collider collider in _colliders)
		{
			if (collider != null)
			{
				GameObject gameObject = collider.gameObject;
				bool flag = _condition == null || _condition(gameObject);
				if (gameObject.activeInHierarchy && flag)
				{
					float sqrMagnitude = (_targetPosition - gameObject.transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = gameObject;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x000E8C04 File Offset: 0x000E7004
	public static ObjectType ScanForComponent<ObjectType>(Collider[] _colliders, Vector3 _targetPosition, InteractWithItemHelper.ScanCondition<ObjectType> _condition = null) where ObjectType : MonoBehaviour
	{
		ObjectType result = (ObjectType)((object)null);
		float num = float.PositiveInfinity;
		foreach (Collider collider in _colliders)
		{
			if (collider != null)
			{
				foreach (ObjectType objectType in ComponentCache<ObjectType>.GetComponents(collider.gameObject))
				{
					if (objectType.enabled && (_condition == null || _condition(objectType)))
					{
						float sqrMagnitude = (_targetPosition - objectType.transform.position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							result = objectType;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x040027E4 RID: 10212
	private static GridIndex[] s_gridOffsetsXZ = new GridIndex[]
	{
		new GridIndex(1, 0, 0),
		new GridIndex(-1, 0, 0),
		new GridIndex(0, 0, 1),
		new GridIndex(0, 0, -1)
	};

	// Token: 0x040027E5 RID: 10213
	private const int c_defaultGridColliderSize = 16;

	// Token: 0x040027E6 RID: 10214
	private static List<InteractWithItemHelper.GridColliderData> s_gridColliderData = new List<InteractWithItemHelper.GridColliderData>(16);

	// Token: 0x020009F0 RID: 2544
	// (Invoke) Token: 0x060031AD RID: 12717
	public delegate bool ScanCondition(GameObject _testObject);

	// Token: 0x020009F1 RID: 2545
	// (Invoke) Token: 0x060031B1 RID: 12721
	public delegate bool ScanCondition<ObjectType>(ObjectType _testObject) where ObjectType : MonoBehaviour;

	// Token: 0x020009F2 RID: 2546
	private struct GridColliderData
	{
		// Token: 0x040027E7 RID: 10215
		public Collider Subject;

		// Token: 0x040027E8 RID: 10216
		public GridIndex Index;

		// Token: 0x040027E9 RID: 10217
		public GridManager Manager;
	}
}
