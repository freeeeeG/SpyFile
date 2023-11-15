using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D1 RID: 977
[RequireComponent(typeof(MapAvatarControls))]
[RequireComponent(typeof(MapAvatarGroundCast))]
public class MapAvatarTrail : MonoBehaviour
{
	// Token: 0x06001212 RID: 4626 RVA: 0x00066660 File Offset: 0x00064A60
	protected virtual void Start()
	{
		this.m_controls = base.gameObject.RequireComponent<MapAvatarControls>();
		this.m_groundCast = base.gameObject.RequireComponent<MapAvatarGroundCast>();
		this.m_trails = new MapAvatarTrail.Trail[this.m_configs.Length];
		for (int i = 0; i < this.m_trails.Length; i++)
		{
			this.m_trails[i] = new MapAvatarTrail.Trail();
			this.m_trails[i].m_config = this.m_configs[i];
		}
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x000666E0 File Offset: 0x00064AE0
	protected virtual void Update()
	{
		for (int i = 0; i < this.m_trails.Length; i++)
		{
			this.UpdateTrail(ref this.m_trails[i]);
		}
		for (int j = this.m_killRoutines.Count - 1; j >= 0; j--)
		{
			IEnumerator enumerator = this.m_killRoutines[j];
			if (enumerator == null || !enumerator.MoveNext())
			{
				this.m_killRoutines.RemoveAt(j);
			}
		}
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x00066760 File Offset: 0x00064B60
	private void UpdateTrail(ref MapAvatarTrail.Trail _trail)
	{
		GameObject gameObject = null;
		Vector3 vector = new Vector3(0f, 0f, 0f);
		if (this.m_groundCast != null)
		{
			Vector3 closestPointOnGround = this.m_groundCast.GetClosestPointOnGround();
			Vector3 groundNormal = this.m_groundCast.GetGroundNormal();
			Vector3 point = _trail.m_config.m_transform.position + base.transform.rotation * _trail.m_config.m_offset;
			vector = this.ProjectOntoPlane(point, closestPointOnGround, groundNormal);
			gameObject = this.GetGridOccupantAtPoint(vector);
		}
		if (gameObject != null)
		{
			if (_trail.m_lastHit != gameObject)
			{
				this.ProcessGroundChange(gameObject, vector, ref _trail);
			}
			else if (_trail.m_trailTransform != null)
			{
				if (_trail.m_config.m_rotateWithThis)
				{
					_trail.m_trailTransform.rotation = base.transform.rotation;
				}
				_trail.m_trailTransform.position = vector + base.transform.rotation * _trail.m_config.m_offset;
			}
			_trail.m_lastHit = gameObject;
		}
		else if (_trail.m_lastHit != null)
		{
			_trail.m_lastHit = null;
			MapAvatarTrail.Trail.PointInfo newPointInfo = default(MapAvatarTrail.Trail.PointInfo);
			newPointInfo.m_trailableTerrain = false;
			Vector3 vector2 = _trail.m_config.m_transform.position + base.transform.rotation * _trail.m_config.m_offset;
			Vector3 point2;
			if (this.FindTransitionPoint(_trail.m_lastPoint.Value.m_point, vector2, out point2))
			{
				newPointInfo.m_point = point2;
			}
			else
			{
				newPointInfo.m_point = vector2;
			}
			this.UpdateRenderer(newPointInfo, ref _trail);
		}
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x00066938 File Offset: 0x00064D38
	private void ProcessGroundChange(GameObject _ground, Vector3 _groundPoint, ref MapAvatarTrail.Trail _trail)
	{
		MapAvatarTrail.Trail.PointInfo newPointInfo = default(MapAvatarTrail.Trail.PointInfo);
		newPointInfo.m_trailableTerrain = this.IsTrailableTerrain(_groundPoint, _trail.m_config);
		if (_trail.m_lastPoint != null && newPointInfo.m_trailableTerrain != _trail.m_lastPoint.Value.m_trailableTerrain)
		{
			Vector3 point;
			if (this.FindTransitionPoint(_trail.m_lastPoint.Value.m_point, _groundPoint, out point))
			{
				newPointInfo.m_point = point;
			}
			else
			{
				newPointInfo.m_point = _groundPoint;
			}
		}
		else
		{
			newPointInfo.m_point = _groundPoint;
		}
		this.UpdateRenderer(newPointInfo, ref _trail);
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x000669E0 File Offset: 0x00064DE0
	private void UpdateRenderer(MapAvatarTrail.Trail.PointInfo _newPointInfo, ref MapAvatarTrail.Trail _trail)
	{
		if (!_newPointInfo.m_trailableTerrain && _trail.m_trailTransform != null)
		{
			this.m_killRoutines.Add(this.KillTrailRoutine(_trail.m_trailTransform, _trail.m_currRenderers, _newPointInfo.m_point));
			this.OnTransition(_trail, _newPointInfo);
			_trail.Reset();
		}
		if (_newPointInfo.m_trailableTerrain && (_trail.m_trailTransform == null || _trail.m_lastPoint == null || !_trail.m_lastPoint.Value.m_trailableTerrain))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(_trail.m_config.m_prefab);
			foreach (TrailRenderer trailRenderer in gameObject.RequestComponentsRecursive<TrailRenderer>())
			{
				trailRenderer.Clear();
				trailRenderer.autodestruct = false;
			}
			TrailRenderer[] array;
			_trail.m_currRenderers = array;
			_trail.m_trailTransform = gameObject.transform;
		}
		if (_trail.m_trailTransform != null)
		{
			if (_trail.m_config.m_rotateWithThis)
			{
				_trail.m_trailTransform.rotation = base.transform.rotation;
			}
			_trail.m_trailTransform.position = _newPointInfo.m_point + base.transform.rotation * _trail.m_config.m_offset;
		}
		_trail.m_lastPoint = new MapAvatarTrail.Trail.PointInfo?(_newPointInfo);
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x00066B5C File Offset: 0x00064F5C
	private IEnumerator KillTrailRoutine(Transform _trail, TrailRenderer[] _renderers, Vector3 _endPos)
	{
		for (int i = 0; i < _renderers.Length; i++)
		{
			_renderers[i].autodestruct = true;
		}
		_trail.position = _endPos;
		bool hasRenderer = true;
		while (hasRenderer)
		{
			hasRenderer = false;
			for (int j = 0; j < _renderers.Length; j++)
			{
				if (_renderers[j] != null)
				{
					hasRenderer = true;
				}
			}
			yield return null;
		}
		if (_trail != null && _trail.gameObject != null)
		{
			UnityEngine.Object.Destroy(_trail.gameObject);
		}
		yield break;
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x00066B85 File Offset: 0x00064F85
	protected virtual void OnTransition(MapAvatarTrail.Trail _trail, MapAvatarTrail.Trail.PointInfo _info)
	{
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x00066B88 File Offset: 0x00064F88
	protected virtual bool IsTrailableTerrain(Vector3 _point, MapAvatarTrail.TrailConfig _config)
	{
		GameObject gridOccupantAtPoint = this.GetGridOccupantAtPoint(_point);
		if (gridOccupantAtPoint == null)
		{
			return false;
		}
		WorldMapTileFlip component = gridOccupantAtPoint.GetComponent<WorldMapTileFlip>();
		if (component == null)
		{
			return false;
		}
		MapNode levelMapNode = component.m_flipOwnerData.m_levelMapNode;
		if (levelMapNode != null && !levelMapNode.Unfolded && !levelMapNode.Unfolding)
		{
			return false;
		}
		WorldMapTileOptimizer component2 = gridOccupantAtPoint.GetComponent<WorldMapTileOptimizer>();
		if (component2 == null)
		{
			return false;
		}
		TileCosmetics cosmetics = component2.Cosmetics;
		if (cosmetics == null)
		{
			return false;
		}
		for (int i = 0; i < _config.m_trailableTiles.Length; i++)
		{
			if (cosmetics.m_name == _config.m_trailableTiles[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x00066C50 File Offset: 0x00065050
	private GameObject GetGridOccupantAtPoint(Vector3 _point)
	{
		return this.m_controls.GridManager.GetGridOccupant(this.m_controls.GridManager.GetUnclampedGridLocationFromPos(_point));
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x00066C80 File Offset: 0x00065080
	private bool FindTransitionPoint(Vector3 _start, Vector3 _end, out Vector3 _transitionPoint)
	{
		GameObject gridOccupantAtPoint = this.GetGridOccupantAtPoint(_start);
		HexGridManager hexGridManager = this.m_controls.GridManager as HexGridManager;
		if (gridOccupantAtPoint != null)
		{
			Vector3 a = this.ProjectOntoPlane(_end, gridOccupantAtPoint.transform.position, gridOccupantAtPoint.transform.up);
			Vector3 normalized = (a - gridOccupantAtPoint.transform.position).normalized;
			float hexRadius = hexGridManager.HexRadius;
			float num = Vector3.Angle(Vector3.right, normalized);
			float num2 = num % 60f;
			float d = this.m_sqrtThree * hexRadius / (this.m_sqrtThree * Mathf.Cos(num2 * 0.017453292f) + Mathf.Sin(num2 * 0.017453292f));
			_transitionPoint = gridOccupantAtPoint.transform.position + normalized * d;
			return true;
		}
		_transitionPoint = Vector3.zero;
		return false;
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x00066D64 File Offset: 0x00065164
	private Vector3 ProjectOntoPlane(Vector3 _point, Vector3 _planePoint, Vector3 _planeNormal)
	{
		return _point - Vector3.Dot(_point, _planeNormal) * _planeNormal;
	}

	// Token: 0x04000E25 RID: 3621
	[Header("Shared settings")]
	[SerializeField]
	private LayerMask m_landscapeMask = default(LayerMask);

	// Token: 0x04000E26 RID: 3622
	[Header("Per instance settings")]
	[SerializeField]
	private MapAvatarTrail.TrailConfig[] m_configs = new MapAvatarTrail.TrailConfig[0];

	// Token: 0x04000E27 RID: 3623
	private MapAvatarControls m_controls;

	// Token: 0x04000E28 RID: 3624
	private MapAvatarGroundCast m_groundCast;

	// Token: 0x04000E29 RID: 3625
	private MapAvatarTrail.Trail[] m_trails;

	// Token: 0x04000E2A RID: 3626
	private List<IEnumerator> m_killRoutines = new List<IEnumerator>();

	// Token: 0x04000E2B RID: 3627
	private float m_sqrtThree = Mathf.Sqrt(3f);

	// Token: 0x020003D2 RID: 978
	[Serializable]
	protected class TrailConfig
	{
		// Token: 0x04000E2C RID: 3628
		[SerializeField]
		public GameObject m_prefab;

		// Token: 0x04000E2D RID: 3629
		[SerializeField]
		public Transform m_transform;

		// Token: 0x04000E2E RID: 3630
		[SerializeField]
		public Vector3 m_offset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000E2F RID: 3631
		[SerializeField]
		public bool m_rotateWithThis;

		// Token: 0x04000E30 RID: 3632
		[SerializeField]
		[global::Tooltip("String used to match against a tile's cosmetics name")]
		public string[] m_trailableTiles = new string[]
		{
			"Snow"
		};
	}

	// Token: 0x020003D3 RID: 979
	protected class Trail
	{
		// Token: 0x0600121F RID: 4639 RVA: 0x00066DB8 File Offset: 0x000651B8
		public void Reset()
		{
			this.m_lastPoint = null;
			this.m_lastHit = null;
			this.m_trailTransform = null;
			this.m_currRenderers = null;
		}

		// Token: 0x04000E31 RID: 3633
		public MapAvatarTrail.TrailConfig m_config;

		// Token: 0x04000E32 RID: 3634
		public MapAvatarTrail.Trail.PointInfo? m_lastPoint;

		// Token: 0x04000E33 RID: 3635
		public GameObject m_lastHit;

		// Token: 0x04000E34 RID: 3636
		public Transform m_trailTransform;

		// Token: 0x04000E35 RID: 3637
		public TrailRenderer[] m_currRenderers;

		// Token: 0x020003D4 RID: 980
		public struct PointInfo
		{
			// Token: 0x04000E36 RID: 3638
			public Vector3 m_point;

			// Token: 0x04000E37 RID: 3639
			public bool m_trailableTerrain;
		}
	}
}
