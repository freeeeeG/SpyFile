using System;
using UnityEngine;

// Token: 0x02000A48 RID: 2632
public class BuildingCellVisualizerResources : ScriptableObject
{
	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06004F30 RID: 20272 RVA: 0x001C028C File Offset: 0x001BE48C
	// (set) Token: 0x06004F31 RID: 20273 RVA: 0x001C0294 File Offset: 0x001BE494
	public Material backgroundMaterial { get; set; }

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06004F32 RID: 20274 RVA: 0x001C029D File Offset: 0x001BE49D
	// (set) Token: 0x06004F33 RID: 20275 RVA: 0x001C02A5 File Offset: 0x001BE4A5
	public Material iconBackgroundMaterial { get; set; }

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06004F34 RID: 20276 RVA: 0x001C02AE File Offset: 0x001BE4AE
	// (set) Token: 0x06004F35 RID: 20277 RVA: 0x001C02B6 File Offset: 0x001BE4B6
	public Material powerInputMaterial { get; set; }

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06004F36 RID: 20278 RVA: 0x001C02BF File Offset: 0x001BE4BF
	// (set) Token: 0x06004F37 RID: 20279 RVA: 0x001C02C7 File Offset: 0x001BE4C7
	public Material powerOutputMaterial { get; set; }

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06004F38 RID: 20280 RVA: 0x001C02D0 File Offset: 0x001BE4D0
	// (set) Token: 0x06004F39 RID: 20281 RVA: 0x001C02D8 File Offset: 0x001BE4D8
	public Material liquidInputMaterial { get; set; }

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06004F3A RID: 20282 RVA: 0x001C02E1 File Offset: 0x001BE4E1
	// (set) Token: 0x06004F3B RID: 20283 RVA: 0x001C02E9 File Offset: 0x001BE4E9
	public Material liquidOutputMaterial { get; set; }

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06004F3C RID: 20284 RVA: 0x001C02F2 File Offset: 0x001BE4F2
	// (set) Token: 0x06004F3D RID: 20285 RVA: 0x001C02FA File Offset: 0x001BE4FA
	public Material gasInputMaterial { get; set; }

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06004F3E RID: 20286 RVA: 0x001C0303 File Offset: 0x001BE503
	// (set) Token: 0x06004F3F RID: 20287 RVA: 0x001C030B File Offset: 0x001BE50B
	public Material gasOutputMaterial { get; set; }

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06004F40 RID: 20288 RVA: 0x001C0314 File Offset: 0x001BE514
	// (set) Token: 0x06004F41 RID: 20289 RVA: 0x001C031C File Offset: 0x001BE51C
	public Material highEnergyParticleInputMaterial { get; set; }

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x06004F42 RID: 20290 RVA: 0x001C0325 File Offset: 0x001BE525
	// (set) Token: 0x06004F43 RID: 20291 RVA: 0x001C032D File Offset: 0x001BE52D
	public Material highEnergyParticleOutputMaterial { get; set; }

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06004F44 RID: 20292 RVA: 0x001C0336 File Offset: 0x001BE536
	// (set) Token: 0x06004F45 RID: 20293 RVA: 0x001C033E File Offset: 0x001BE53E
	public Mesh backgroundMesh { get; set; }

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06004F46 RID: 20294 RVA: 0x001C0347 File Offset: 0x001BE547
	// (set) Token: 0x06004F47 RID: 20295 RVA: 0x001C034F File Offset: 0x001BE54F
	public Mesh iconMesh { get; set; }

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06004F48 RID: 20296 RVA: 0x001C0358 File Offset: 0x001BE558
	// (set) Token: 0x06004F49 RID: 20297 RVA: 0x001C0360 File Offset: 0x001BE560
	public int backgroundLayer { get; set; }

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06004F4A RID: 20298 RVA: 0x001C0369 File Offset: 0x001BE569
	// (set) Token: 0x06004F4B RID: 20299 RVA: 0x001C0371 File Offset: 0x001BE571
	public int iconLayer { get; set; }

	// Token: 0x06004F4C RID: 20300 RVA: 0x001C037A File Offset: 0x001BE57A
	public static void DestroyInstance()
	{
		BuildingCellVisualizerResources._Instance = null;
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001C0382 File Offset: 0x001BE582
	public static BuildingCellVisualizerResources Instance()
	{
		if (BuildingCellVisualizerResources._Instance == null)
		{
			BuildingCellVisualizerResources._Instance = Resources.Load<BuildingCellVisualizerResources>("BuildingCellVisualizerResources");
			BuildingCellVisualizerResources._Instance.Initialize();
		}
		return BuildingCellVisualizerResources._Instance;
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001C03B0 File Offset: 0x001BE5B0
	private void Initialize()
	{
		Shader shader = Shader.Find("Klei/BuildingCell");
		this.backgroundMaterial = new Material(shader);
		this.backgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.iconBackgroundMaterial = new Material(shader);
		this.iconBackgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.powerInputMaterial = new Material(shader);
		this.powerOutputMaterial = new Material(shader);
		this.liquidInputMaterial = new Material(shader);
		this.liquidOutputMaterial = new Material(shader);
		this.gasInputMaterial = new Material(shader);
		this.gasOutputMaterial = new Material(shader);
		this.highEnergyParticleInputMaterial = new Material(shader);
		this.highEnergyParticleOutputMaterial = new Material(shader);
		this.backgroundMesh = this.CreateMesh("BuildingCellVisualizer", Vector2.zero, 0.5f);
		float num = 0.5f;
		this.iconMesh = this.CreateMesh("BuildingCellVisualizerIcon", Vector2.zero, num * 0.5f);
		this.backgroundLayer = LayerMask.NameToLayer("Default");
		this.iconLayer = LayerMask.NameToLayer("Place");
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001C04C8 File Offset: 0x001BE6C8
	private Mesh CreateMesh(string name, Vector2 base_offset, float half_size)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = new Vector3[]
		{
			new Vector3(-half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(-half_size + base_offset.x, half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, half_size + base_offset.y, 0f)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x040033B4 RID: 13236
	[Header("Electricity")]
	public Color electricityInputColor;

	// Token: 0x040033B5 RID: 13237
	public Color electricityOutputColor;

	// Token: 0x040033B6 RID: 13238
	public Sprite electricityInputIcon;

	// Token: 0x040033B7 RID: 13239
	public Sprite electricityOutputIcon;

	// Token: 0x040033B8 RID: 13240
	public Sprite electricityConnectedIcon;

	// Token: 0x040033B9 RID: 13241
	public Sprite electricityBridgeIcon;

	// Token: 0x040033BA RID: 13242
	public Sprite electricityBridgeConnectedIcon;

	// Token: 0x040033BB RID: 13243
	public Sprite electricityArrowIcon;

	// Token: 0x040033BC RID: 13244
	public Sprite switchIcon;

	// Token: 0x040033BD RID: 13245
	public Color32 switchColor;

	// Token: 0x040033BE RID: 13246
	public Color32 switchOffColor = Color.red;

	// Token: 0x040033BF RID: 13247
	[Header("Gas")]
	public Sprite gasInputIcon;

	// Token: 0x040033C0 RID: 13248
	public Sprite gasOutputIcon;

	// Token: 0x040033C1 RID: 13249
	public BuildingCellVisualizerResources.IOColours gasIOColours;

	// Token: 0x040033C2 RID: 13250
	[Header("Liquid")]
	public Sprite liquidInputIcon;

	// Token: 0x040033C3 RID: 13251
	public Sprite liquidOutputIcon;

	// Token: 0x040033C4 RID: 13252
	public BuildingCellVisualizerResources.IOColours liquidIOColours;

	// Token: 0x040033C5 RID: 13253
	[Header("High Energy Particle")]
	public Sprite highEnergyParticleInputIcon;

	// Token: 0x040033C6 RID: 13254
	public Sprite[] highEnergyParticleOutputIcons;

	// Token: 0x040033C7 RID: 13255
	public Color highEnergyParticleInputColour;

	// Token: 0x040033C8 RID: 13256
	public Color highEnergyParticleOutputColour;

	// Token: 0x040033D7 RID: 13271
	private static BuildingCellVisualizerResources _Instance;

	// Token: 0x020018DA RID: 6362
	[Serializable]
	public struct ConnectedDisconnectedColours
	{
		// Token: 0x04007328 RID: 29480
		public Color32 connected;

		// Token: 0x04007329 RID: 29481
		public Color32 disconnected;
	}

	// Token: 0x020018DB RID: 6363
	[Serializable]
	public struct IOColours
	{
		// Token: 0x0400732A RID: 29482
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours input;

		// Token: 0x0400732B RID: 29483
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours output;
	}
}
