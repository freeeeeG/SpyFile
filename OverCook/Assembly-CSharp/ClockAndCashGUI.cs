using System;
using UnityEngine;

// Token: 0x0200099D RID: 2461
public class ClockAndCashGUI : MonoBehaviour
{
	// Token: 0x0600303B RID: 12347 RVA: 0x000E29EB File Offset: 0x000E0DEB
	public void SetTimeProp(float _prop)
	{
		this.m_value = Mathf.Clamp01(_prop);
	}

	// Token: 0x0600303C RID: 12348 RVA: 0x000E29FC File Offset: 0x000E0DFC
	private void Awake()
	{
		this.m_camera = Camera.main;
		this.CreateObject(out this.m_filledMeshObj, out this.m_filledMesh, "FilledMesh", this.m_fullClockSurface);
		this.CreateObject(out this.m_emptiedMeshObj, out this.m_emptiedMesh, "EmptiedMesh", this.m_emptyClockSurface);
		this.CreateObject(out this.m_borderMeshObj, out this.m_borderMesh, "BorderMesh", this.m_borderClockSurface);
	}

	// Token: 0x0600303D RID: 12349 RVA: 0x000E2A6C File Offset: 0x000E0E6C
	private void Update()
	{
		this.m_value %= 1f;
		Vector2 clockLocation = this.m_clockLocation;
		clockLocation.y = 1f - this.m_clockLocation.y;
		this.UpdateClockMesh(this.m_filledMesh, clockLocation, this.m_clockRadius, 0f, this.m_value);
		this.UpdateClockMesh(this.m_emptiedMesh, clockLocation, this.m_clockRadius, this.m_value, 1f);
		this.UpdateAnulusMesh(this.m_borderMesh, clockLocation, this.m_clockRadius, this.m_backgroundRadius);
	}

	// Token: 0x0600303E RID: 12350 RVA: 0x000E2B00 File Offset: 0x000E0F00
	private void UpdateAnulusMesh(Mesh _mesh, Vector2 _centreVS, float _startRadius, float _endRadius)
	{
		Vector3 vector = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS, 1f));
		Vector3 a = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS + Vector2.right * _startRadius, 1f)) - vector;
		Vector3 a2 = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS + Vector2.right * _endRadius, 1f)) - vector;
		Vector3 a3 = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS + Vector2.up * _startRadius, 1f)) - vector;
		Vector3 a4 = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS + Vector2.up * _endRadius, 1f)) - vector;
		a3 = a3.normalized * a.magnitude;
		a4 = a4.normalized * a2.magnitude;
		Vector3[] array = new Vector3[2 * this.m_segments];
		Vector2[] uv = new Vector2[2 * this.m_segments];
		int[] array2 = new int[2 * this.m_segments * 3];
		for (int i = 0; i < this.m_segments; i++)
		{
			float f = MathUtils.ClampedRemap((float)i, 0f, (float)this.m_segments, 0f, 6.2831855f);
			array[2 * i] = vector + a3 * Mathf.Cos(f) + a * Mathf.Sin(f);
			array[2 * i + 1] = vector + a4 * Mathf.Cos(f) + a2 * Mathf.Sin(f);
		}
		for (int j = 0; j < 2 * this.m_segments; j++)
		{
			array2[3 * j] = j;
			array2[3 * j + 1] = (j + 1) % (2 * this.m_segments);
			array2[3 * j + 2] = (j + 2) % (2 * this.m_segments);
		}
		_mesh.Clear();
		_mesh.vertices = array;
		_mesh.uv = uv;
		_mesh.triangles = array2;
		_mesh.RecalculateNormals();
	}

	// Token: 0x0600303F RID: 12351 RVA: 0x000E2D54 File Offset: 0x000E1154
	private void UpdateClockMesh(Mesh _mesh, Vector2 _centreVS, float _radius, float _propStart, float _propEnd)
	{
		Vector3 vector = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS, 1f));
		Vector3 a = this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS + Vector2.right * _radius, 1f)) - vector;
		Vector3 a2 = (this.m_camera.ViewportToWorldPoint(VectorUtils.FromXY(_centreVS + Vector2.up * _radius, 1f)) - vector).normalized * a.magnitude;
		int num = (int)Mathf.Max((float)this.m_segments * (_propEnd - _propStart), 1f);
		Vector3[] array = new Vector3[num + 2];
		Vector2[] array2 = new Vector2[num + 2];
		int[] array3 = new int[num * 3];
		float newA = 6.2831855f * _propStart;
		float newB = 6.2831855f * _propEnd;
		array[0] = vector;
		array2[0] = new Vector2(0.5f, 0f);
		for (int i = 1; i < num + 2; i++)
		{
			float f = MathUtils.ClampedRemap((float)(i - 1), 0f, (float)num, newA, newB);
			array[i] = vector + a2 * Mathf.Cos(f) + a * Mathf.Sin(f);
			float x = MathUtils.ClampedRemap((float)(i - 1), (float)num * 0.25f, (float)num * 0.75f, 0f, 1f);
			float a3 = MathUtils.ClampedRemap((float)(i - 1), 0f, (float)num * 0.25f, 0f, 1f);
			float b = MathUtils.ClampedRemap((float)(i - 1), (float)num * 0.75f, (float)num, 1f, 0f);
			array2[i] = new Vector2(x, Mathf.Min(a3, b));
		}
		for (int j = 0; j < num; j++)
		{
			array3[3 * j] = 0;
			array3[3 * j + 1] = j + 1;
			array3[3 * j + 2] = j + 2;
		}
		_mesh.Clear();
		_mesh.vertices = array;
		_mesh.uv = array2;
		_mesh.triangles = array3;
		_mesh.RecalculateNormals();
	}

	// Token: 0x06003040 RID: 12352 RVA: 0x000E2FA0 File Offset: 0x000E13A0
	private void CreateObject(out GameObject _obj, out Mesh _mesh, string _name, Material _material)
	{
		_obj = new GameObject(_name);
		_obj.transform.SetParent(null);
		_obj.transform.position = Vector3.zero;
		_obj.transform.rotation = Quaternion.identity;
		_obj.transform.localScale = Vector3.one;
		_obj.AddComponent(typeof(MeshFilter));
		_obj.AddComponent(typeof(MeshRenderer));
		_obj.GetComponent<Renderer>().material = _material;
		_mesh = new Mesh();
		_mesh.name = _name;
		_obj.GetComponent<MeshFilter>().mesh = _mesh;
	}

	// Token: 0x040026A8 RID: 9896
	[SerializeField]
	private int m_segments = 20;

	// Token: 0x040026A9 RID: 9897
	[SerializeField]
	private Material m_emptyClockSurface;

	// Token: 0x040026AA RID: 9898
	[SerializeField]
	private Material m_fullClockSurface;

	// Token: 0x040026AB RID: 9899
	[SerializeField]
	private Material m_borderClockSurface;

	// Token: 0x040026AC RID: 9900
	[SerializeField]
	private Vector2 m_clockLocation = new Vector2(0.05f, 0.95f);

	// Token: 0x040026AD RID: 9901
	[SerializeField]
	private float m_clockRadius = 0.01f;

	// Token: 0x040026AE RID: 9902
	[SerializeField]
	private float m_backgroundRadius = 0.011f;

	// Token: 0x040026AF RID: 9903
	private GameObject m_filledMeshObj;

	// Token: 0x040026B0 RID: 9904
	private GameObject m_emptiedMeshObj;

	// Token: 0x040026B1 RID: 9905
	private GameObject m_borderMeshObj;

	// Token: 0x040026B2 RID: 9906
	private Camera m_camera;

	// Token: 0x040026B3 RID: 9907
	private Mesh m_borderMesh;

	// Token: 0x040026B4 RID: 9908
	private Mesh m_emptiedMesh;

	// Token: 0x040026B5 RID: 9909
	private Mesh m_filledMesh;

	// Token: 0x040026B6 RID: 9910
	private float m_value;
}
