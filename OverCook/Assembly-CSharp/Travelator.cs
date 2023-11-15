using System;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class Travelator : MonoBehaviour, IMovingSurface
{
	// Token: 0x06001515 RID: 5397 RVA: 0x00072B48 File Offset: 0x00070F48
	private void Start()
	{
		StaticGridLocation staticGridLocation = base.gameObject.RequireComponent<StaticGridLocation>();
		this.m_gridManager = staticGridLocation.AccessGridManager;
		this.m_gridIndex = staticGridLocation.GridIndex;
		this.SetSpeedOnMaterial();
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x00072B80 File Offset: 0x00070F80
	private void SetSpeedOnMaterial()
	{
		MeshRenderer componentInChildren = base.gameObject.GetComponentInChildren<MeshRenderer>();
		Material[] materials = componentInChildren.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			if (componentInChildren.materials[i].HasProperty(this.m_shaderSpeedProp))
			{
				componentInChildren.materials[i].SetFloat(this.m_shaderSpeedProp, this.m_speed * this.m_materialScrollMultiplier);
			}
		}
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x00072BEC File Offset: 0x00070FEC
	public Vector3 CalculateVelocityAtPoint(Vector3 _point, IMovingSurface _prevSurface)
	{
		Vector3 zero = Vector3.zero;
		if (this.CalculateForPrevSurface(_prevSurface, _point, ref zero))
		{
			return zero;
		}
		if (this.CalculateForBorders(_point, ref zero))
		{
			return zero;
		}
		return this.GetSurfaceVelocity();
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x00072C26 File Offset: 0x00071026
	public Quaternion CalculateRotationAtPoint(Vector3 _point, Quaternion _prevRotation)
	{
		return _prevRotation;
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x00072C2C File Offset: 0x0007102C
	private bool CalculateForPrevSurface(IMovingSurface _surface, Vector3 _point, ref Vector3 _velocity)
	{
		if (_surface == null || _surface == this)
		{
			return false;
		}
		Travelator travelator = _surface as Travelator;
		if (travelator == null)
		{
			return false;
		}
		if (travelator.GetNextTravelator() == this)
		{
			Vector3 travelDirection = this.GetTravelDirection();
			Vector3 travelDirection2 = travelator.GetTravelDirection();
			if (Vector3.Dot(travelDirection, travelDirection2) < 0.5f)
			{
				float num = VectorUtils.ProgressUnclamped(travelator.transform.position, base.transform.position, _point);
				if (num >= 0f && num <= 1f)
				{
					_velocity = travelator.GetSurfaceVelocity();
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x00072CD0 File Offset: 0x000710D0
	private bool CalculateForBorders(Vector3 _point, ref Vector3 _velocity)
	{
		Vector3 borderDirection = this.GetBorderDirection(_point);
		if (borderDirection.sqrMagnitude > 0f)
		{
			Travelator adjacentTravelator = this.GetAdjacentTravelator(borderDirection);
			if (adjacentTravelator != null && adjacentTravelator.GetNextTravelator() == this)
			{
				_velocity = adjacentTravelator.GetSurfaceVelocity();
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x00072D2C File Offset: 0x0007112C
	private Vector3 GetBorderDirection(Vector3 _point)
	{
		Vector3 vector = _point - base.transform.position;
		if (vector.x >= this.m_border)
		{
			return Vector3.right;
		}
		if (vector.x <= -this.m_border)
		{
			return Vector3.left;
		}
		if (vector.z >= this.m_border)
		{
			return Vector3.forward;
		}
		if (vector.z <= -this.m_border)
		{
			return Vector3.back;
		}
		return Vector3.zero;
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x00072DB2 File Offset: 0x000711B2
	private Vector3 GetSurfaceVelocity()
	{
		return (!base.enabled) ? Vector3.zero : (this.m_speed * this.GetTravelDirection());
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x00072DDC File Offset: 0x000711DC
	private Travelator GetNextTravelator()
	{
		Vector3 travelDirection = this.GetTravelDirection();
		if (travelDirection.sqrMagnitude > 0f)
		{
			return this.GetAdjacentTravelator(travelDirection);
		}
		return null;
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x00072E0C File Offset: 0x0007120C
	private Travelator GetAdjacentTravelator(Vector3 _directionXZ)
	{
		GridIndex adjacentGridIndex = this.GetAdjacentGridIndex(_directionXZ);
		GameObject gridOccupant = this.m_gridManager.GetGridOccupant(adjacentGridIndex);
		if (gridOccupant != null)
		{
			return gridOccupant.RequestComponent<Travelator>();
		}
		return null;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x00072E44 File Offset: 0x00071244
	private GridIndex GetAdjacentGridIndex(Vector3 _directionXZ)
	{
		int num = (int)Mathf.Round(_directionXZ.x);
		int num2 = (int)Mathf.Round(_directionXZ.z);
		return new GridIndex(this.m_gridIndex.X + num, this.m_gridIndex.Y, this.m_gridIndex.Z + num2);
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x00072E98 File Offset: 0x00071298
	private Vector3 GetTravelDirection()
	{
		Travelator.XZDirection directionXZ = this.m_directionXZ;
		if (directionXZ == Travelator.XZDirection.Leftwards)
		{
			return base.transform.right;
		}
		if (directionXZ != Travelator.XZDirection.Rightwards)
		{
			return Vector3.zero;
		}
		return -base.transform.right;
	}

	// Token: 0x04001033 RID: 4147
	[SerializeField]
	public float m_speed = 1f;

	// Token: 0x04001034 RID: 4148
	[SerializeField]
	private Travelator.XZDirection m_directionXZ = Travelator.XZDirection.Rightwards;

	// Token: 0x04001035 RID: 4149
	[SerializeField]
	private float m_border;

	// Token: 0x04001036 RID: 4150
	[SerializeField]
	public float m_materialScrollMultiplier = 0.3f;

	// Token: 0x04001037 RID: 4151
	private GridManager m_gridManager;

	// Token: 0x04001038 RID: 4152
	private GridIndex m_gridIndex = default(GridIndex);

	// Token: 0x04001039 RID: 4153
	private int m_shaderSpeedProp = Shader.PropertyToID("_speed");

	// Token: 0x02000470 RID: 1136
	public enum XZDirection
	{
		// Token: 0x0400103B RID: 4155
		Leftwards,
		// Token: 0x0400103C RID: 4156
		Rightwards
	}
}
