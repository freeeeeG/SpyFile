using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000A2A RID: 2602
[AddComponentMenu("KMonoBehaviour/scripts/WaterCubes")]
public class WaterCubes : KMonoBehaviour
{
	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x001B50C4 File Offset: 0x001B32C4
	// (set) Token: 0x06004DF7 RID: 19959 RVA: 0x001B50CB File Offset: 0x001B32CB
	public static WaterCubes Instance { get; private set; }

	// Token: 0x06004DF8 RID: 19960 RVA: 0x001B50D3 File Offset: 0x001B32D3
	public static void DestroyInstance()
	{
		WaterCubes.Instance = null;
	}

	// Token: 0x06004DF9 RID: 19961 RVA: 0x001B50DB File Offset: 0x001B32DB
	protected override void OnPrefabInit()
	{
		WaterCubes.Instance = this;
	}

	// Token: 0x06004DFA RID: 19962 RVA: 0x001B50E4 File Offset: 0x001B32E4
	public void Init()
	{
		this.cubes = Util.NewGameObject(base.gameObject, "WaterCubes");
		GameObject gameObject = new GameObject();
		gameObject.name = "WaterCubesMesh";
		gameObject.transform.parent = this.cubes.transform;
		this.material.renderQueue = RenderQueues.Liquid;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = this.material;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRenderer.sharedMaterial.SetTexture("_MainTex2", this.waveTexture);
		meshFilter.sharedMesh = this.CreateNewMesh();
		meshRenderer.gameObject.layer = 0;
		meshRenderer.gameObject.transform.parent = base.transform;
		meshRenderer.gameObject.transform.SetPosition(new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Liquid)));
	}

	// Token: 0x06004DFB RID: 19963 RVA: 0x001B51DC File Offset: 0x001B33DC
	private Mesh CreateNewMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "WaterCubes";
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] array = new Vector2[num];
		Vector3[] normals = new Vector3[num];
		Vector4[] tangents = new Vector4[num];
		int[] triangles = new int[6];
		float layerZ = Grid.GetLayerZ(Grid.SceneLayer.Liquid);
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, layerZ),
			new Vector3((float)Grid.WidthInCells, 0f, layerZ),
			new Vector3(0f, Grid.HeightInMeters, layerZ),
			new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, layerZ)
		};
		array = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		normals = new Vector3[]
		{
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f)
		};
		tangents = new Vector4[]
		{
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = array;
		mesh.uv2 = array;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f));
		return mesh;
	}

	// Token: 0x040032DB RID: 13019
	public Material material;

	// Token: 0x040032DC RID: 13020
	public Texture2D waveTexture;

	// Token: 0x040032DD RID: 13021
	private GameObject cubes;
}
