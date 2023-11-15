using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

// Token: 0x02000971 RID: 2417
[AddComponentMenu("KMonoBehaviour/scripts/SimDebugView")]
public class SimDebugView : KMonoBehaviour
{
	// Token: 0x060046E9 RID: 18153 RVA: 0x001901A3 File Offset: 0x0018E3A3
	public static void DestroyInstance()
	{
		SimDebugView.Instance = null;
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x001901AB File Offset: 0x0018E3AB
	protected override void OnPrefabInit()
	{
		SimDebugView.Instance = this;
		this.material = UnityEngine.Object.Instantiate<Material>(this.material);
		this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x001901D8 File Offset: 0x0018E3D8
	protected override void OnSpawn()
	{
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[2].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color3", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[3].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color4", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[4].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color5", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[5].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color6", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[6].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color7", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[7].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[2].colorName));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x00190464 File Offset: 0x0018E664
	public void OnReset()
	{
		this.plane = SimDebugView.CreatePlane("SimDebugView", base.transform);
		this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
		this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.tex;
		this.plane.transform.SetLocalPosition(new Vector3(0f, 0f, -6f));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x00190503 File Offset: 0x0018E703
	public static Texture2D CreateTexture(int width, int height)
	{
		return new Texture2D(width, height)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x00190525 File Offset: 0x0018E725
	public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x0019055C File Offset: 0x0018E75C
	public static Texture2D CreateTexture(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x0019059C File Offset: 0x0018E79C
	public static GameObject CreatePlane(string layer, Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "overlayViewDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		gameObject.transform.SetParent(parent);
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float y = 2f * (float)Grid.HeightInCells;
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, y, 0f),
			new Vector3(Grid.WidthInMeters, y, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 2f),
			new Vector2(1f, 2f)
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
		mesh.uv = uv;
		mesh.triangles = triangles;
		Vector2 vector = new Vector2((float)Grid.WidthInCells, y);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return gameObject;
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x00190774 File Offset: 0x0018E974
	private void Update()
	{
		if (this.plane == null)
		{
			return;
		}
		bool flag = this.mode != global::OverlayModes.None.ID;
		this.plane.SetActive(flag);
		SimDebugViewCompositor.Instance.Toggle(flag && !GameUtil.IsCapturingTimeLapse());
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
		float x = 0f;
		if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
		{
			x = 1f;
		}
		SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0f));
		if (flag)
		{
			this.UpdateData(this.tex, this.texBytes, this.mode, 192);
		}
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x0019089A File Offset: 0x0018EA9A
	private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x060046F3 RID: 18163 RVA: 0x001908CA File Offset: 0x0018EACA
	private static void SetDefaultPoint(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Point;
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x001908FA File Offset: 0x0018EAFA
	private static void SetDisease(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.diseaseMaterial;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x060046F5 RID: 18165 RVA: 0x0019092C File Offset: 0x0018EB2C
	public void UpdateData(Texture2D texture, byte[] textureBytes, HashedString viewMode, byte alpha)
	{
		Action<SimDebugView, Texture> action;
		if (!this.dataUpdateFuncs.TryGetValue(viewMode, out action))
		{
			action = new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
		}
		action(this, texture);
		int x;
		int num;
		int x2;
		int num2;
		Grid.GetVisibleExtents(out x, out num, out x2, out num2);
		this.selectedPathProber = null;
		KSelectable selected = SelectTool.Instance.selected;
		if (selected != null)
		{
			this.selectedPathProber = selected.GetComponent<PathProber>();
		}
		this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
		int num3 = 16;
		for (int i = num; i <= num2; i += num3)
		{
			int y = Math.Min(i + num3 - 1, num2);
			this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(x, i, x2, y));
		}
		this.currentFrame = Time.frameCount;
		this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		GlobalJobManager.Run(this.updateSimViewWorkItems);
		texture.LoadRawTextureData(textureBytes);
		texture.Apply();
	}

	// Token: 0x060046F6 RID: 18166 RVA: 0x00190A27 File Offset: 0x0018EC27
	public void SetGameGridMode(SimDebugView.GameGridMode mode)
	{
		this.gameGridMode = mode;
	}

	// Token: 0x060046F7 RID: 18167 RVA: 0x00190A30 File Offset: 0x0018EC30
	public SimDebugView.GameGridMode GetGameGridMode()
	{
		return this.gameGridMode;
	}

	// Token: 0x060046F8 RID: 18168 RVA: 0x00190A38 File Offset: 0x0018EC38
	public void SetMode(HashedString mode)
	{
		this.mode = mode;
		Game.Instance.gameObject.Trigger(1798162660, mode);
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x00190A5B File Offset: 0x0018EC5B
	public HashedString GetMode()
	{
		return this.mode;
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x00190A64 File Offset: 0x0018EC64
	public static Color TemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, 1f, 1f);
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x00190AB0 File Offset: 0x0018ECB0
	public static Color LiquidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float value = (temperature - minTempExpected) / (maxTempExpected - minTempExpected);
		float num = Mathf.Clamp(value, 0.5f, 1f);
		float s = Mathf.Clamp(value, 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x00190B0C File Offset: 0x0018ED0C
	public static Color SolidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0.5f, 1f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x00190B5C File Offset: 0x0018ED5C
	public static Color GasTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 0.5f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x00190BAC File Offset: 0x0018EDAC
	public Color NormalizedTemperature(float temperature)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.temperatureThresholds.Length; i++)
		{
			if (temperature <= this.temperatureThresholds[i].value)
			{
				num2 = i;
				break;
			}
			num = i;
			num2 = i;
		}
		float num3 = 0f;
		if (num != num2)
		{
			num3 = (temperature - this.temperatureThresholds[num].value) / (this.temperatureThresholds[num2].value - this.temperatureThresholds[num].value);
		}
		num3 = Mathf.Max(num3, 0f);
		num3 = Mathf.Min(num3, 1f);
		return Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num2].colorName), num3);
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x00190C98 File Offset: 0x0018EE98
	public Color NormalizedHeatFlow(int cell)
	{
		int num = 0;
		int num2 = 0;
		float thermalComfort = GameUtil.GetThermalComfort(cell, -0.08368001f);
		for (int i = 0; i < this.heatFlowThresholds.Length; i++)
		{
			if (thermalComfort <= this.heatFlowThresholds[i].value)
			{
				num2 = i;
				break;
			}
			num = i;
			num2 = i;
		}
		float num3 = 0f;
		if (num != num2)
		{
			num3 = (thermalComfort - this.heatFlowThresholds[num].value) / (this.heatFlowThresholds[num2].value - this.heatFlowThresholds[num].value);
		}
		num3 = Mathf.Max(num3, 0f);
		num3 = Mathf.Min(num3, 1f);
		Color result = Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num2].colorName), num3);
		if (Grid.Solid[cell])
		{
			result = Color.black;
		}
		return result;
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x00190DAE File Offset: 0x0018EFAE
	private static bool IsInsulated(int cell)
	{
		return (Grid.Element[cell].state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x00190DC4 File Offset: 0x0018EFC4
	private static Color GetDiseaseColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.DiseaseIdx[cell] != 255)
		{
			Disease disease = Db.Get().Diseases[(int)Grid.DiseaseIdx[cell]];
			result = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
			result.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
		}
		else
		{
			result.a = 0f;
		}
		return result;
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x00190E45 File Offset: 0x0018F045
	private static Color GetHeatFlowColour(SimDebugView instance, int cell)
	{
		return instance.NormalizedHeatFlow(cell);
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x00190E4E File Offset: 0x0018F04E
	private static Color GetBlack(SimDebugView instance, int cell)
	{
		return Color.black;
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x00190E58 File Offset: 0x0018F058
	public static Color GetLightColour(SimDebugView instance, int cell)
	{
		Color result = GlobalAssets.Instance.colorSet.lightOverlay;
		result.a = Mathf.Clamp(Mathf.Sqrt((float)(Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0f, 1f);
		if (Grid.LightIntensity[cell] > 72000)
		{
			float num = ((float)Grid.LightIntensity[cell] + (float)LightGridManager.previewLux[cell] - 72000f) / 8000f;
			num /= 10f;
			result.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, Grid.CellToPos2D(cell).y / 8f + (float)instance.currentFrame / 32f) * num);
		}
		return result;
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x00190F40 File Offset: 0x0018F140
	public static Color GetRadiationColour(SimDebugView instance, int cell)
	{
		float a = Mathf.Clamp(Mathf.Sqrt(Grid.Radiation[cell]) / 30f, 0f, 1f);
		return new Color(0.2f, 0.9f, 0.3f, a);
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x00190F88 File Offset: 0x0018F188
	public static Color GetRoomsColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.IsValidCell(instance.selectedCell))
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				Room room = cavityForCell.room;
				result = GlobalAssets.Instance.colorSet.GetColorByName(room.roomType.category.colorName);
				result.a = 0.45f;
				if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
				{
					result.a += 0.3f;
				}
			}
		}
		return result;
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x00191028 File Offset: 0x0018F228
	public static Color GetJoulesColour(SimDebugView instance, int cell)
	{
		float num = Grid.Element[cell].specificHeatCapacity * Grid.Temperature[cell] * (Grid.Mass[cell] * 1000f);
		float t = 0.5f * num / (ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294f * 1000000f);
		return Color.Lerp(Color.black, Color.red, t);
	}

	// Token: 0x06004708 RID: 18184 RVA: 0x00191094 File Offset: 0x0018F294
	public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
	{
		switch (Game.Instance.temperatureOverlayMode)
		{
		case Game.TemperatureOverlayModes.AbsoluteTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.AdaptiveTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.HeatFlow:
			return SimDebugView.GetHeatFlowColour(instance, cell);
		case Game.TemperatureOverlayModes.StateChange:
			return SimDebugView.GetStateChangeProximityColour(instance, cell);
		default:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		}
	}

	// Token: 0x06004709 RID: 18185 RVA: 0x001910EC File Offset: 0x0018F2EC
	public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
	{
		float temperature = Grid.Temperature[cell];
		Element element = Grid.Element[cell];
		float num = element.lowTemp;
		float num2 = element.highTemp;
		if (element.IsGas)
		{
			num2 = Mathf.Min(num + 150f, num2);
			return SimDebugView.GasTemperatureToColor(temperature, num, num2);
		}
		if (element.IsSolid)
		{
			num = Mathf.Max(num2 - 150f, num);
			return SimDebugView.SolidTemperatureToColor(temperature, num, num2);
		}
		return SimDebugView.TemperatureToColor(temperature, num, num2);
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x00191164 File Offset: 0x0018F364
	public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
	{
		float temperature = Grid.Temperature[cell];
		return instance.NormalizedTemperature(temperature);
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x00191184 File Offset: 0x0018F384
	private static Color GetGameGridColour(SimDebugView instance, int cell)
	{
		Color result = new Color32(0, 0, 0, byte.MaxValue);
		switch (instance.gameGridMode)
		{
		case SimDebugView.GameGridMode.GameSolidMap:
			result = (Grid.Solid[cell] ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.Lighting:
			result = ((Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0) ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.DigAmount:
			if (Grid.Element[cell].IsSolid)
			{
				float num = Grid.Damage[cell] / 255f;
				result = Color.HSVToRGB(1f - num, 1f, 1f);
			}
			break;
		case SimDebugView.GameGridMode.DupePassable:
			result = (Grid.DupePassable[cell] ? Color.white : Color.black);
			break;
		}
		return result;
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x00191265 File Offset: 0x0018F465
	public Color32 GetColourForID(int id)
	{
		return this.networkColours[id % this.networkColours.Length];
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x0019127C File Offset: 0x0018F47C
	private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
	{
		bool flag = SimDebugView.IsInsulated(cell);
		Color black = Color.black;
		float num = instance.maxThermalConductivity - instance.minThermalConductivity;
		if (!flag && num != 0f)
		{
			float num2 = (Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num;
			num2 = Mathf.Max(num2, 0f);
			num2 = Mathf.Min(num2, 1f);
			black = new Color(num2, num2, num2);
		}
		return black;
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x001912E8 File Offset: 0x0018F4E8
	private static Color GetPressureMapColour(SimDebugView instance, int cell)
	{
		Color32 c = Color.black;
		if (Grid.Pressure[cell] > 0f)
		{
			float num = Mathf.Clamp((Grid.Pressure[cell] - instance.minPressureExpected) / (instance.maxPressureExpected - instance.minPressureExpected), 0f, 1f) * 0.9f;
			c = new Color(num, num, num, 1f);
		}
		return c;
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x00191360 File Offset: 0x0018F560
	private static Color GetOxygenMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
		{
			if (Grid.Mass[cell] > SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
			{
				float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0f, 1f);
				result = instance.breathableGradient.Evaluate(time);
			}
			else
			{
				result = instance.unbreathableColour;
			}
		}
		return result;
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00191408 File Offset: 0x0018F608
	private static Color GetTileColour(SimDebugView instance, int cell)
	{
		float num = 0.33f;
		Color result = new Color(num, num, num);
		Element element = Grid.Element[cell];
		bool flag = false;
		foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
		{
			if (element.HasTag(search_tag))
			{
				flag = true;
			}
		}
		if (flag)
		{
			result = element.substance.uiColour;
		}
		return result;
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x00191498 File Offset: 0x0018F698
	private static Color GetTileTypeColour(SimDebugView instance, int cell)
	{
		return Grid.Element[cell].substance.uiColour;
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x001914B0 File Offset: 0x0018F6B0
	private static Color GetStateMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Gas:
			result = Color.yellow;
			break;
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x00191504 File Offset: 0x0018F704
	private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x00191550 File Offset: 0x0018F750
	private static Color GetStateChangeColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		if (!element.IsVacuum)
		{
			float num = Grid.Temperature[cell];
			float num2 = element.lowTemp * 0.05f;
			float a = Mathf.Abs(num - element.lowTemp) / num2;
			float num3 = element.highTemp * 0.05f;
			float b = Mathf.Abs(num - element.highTemp) / num3;
			float t = Mathf.Max(0f, 1f - Mathf.Min(a, b));
			result = Color.Lerp(Color.black, Color.red, t);
		}
		return result;
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x001915E8 File Offset: 0x0018F7E8
	private static Color GetDecorColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.Solid[cell])
		{
			float num = GameUtil.GetDecorAtCell(cell) / 100f;
			if (num > 0f)
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorPositive, Mathf.Abs(num));
			}
			else
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorNegative, Mathf.Abs(num));
			}
		}
		return result;
	}

	// Token: 0x06004716 RID: 18198 RVA: 0x00191688 File Offset: 0x0018F888
	private static Color GetDangerColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
		if (!Grid.Element[cell].IsSolid)
		{
			float num = 0f;
			if (Grid.Temperature[cell] < SimDebugView.minMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
			}
			if (Grid.Temperature[cell] > SimDebugView.maxMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
			}
			if (num > 0f)
			{
				if (num < 10f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryLow;
				}
				else if (num < 30f)
				{
					dangerAmount = SimDebugView.DangerAmount.Low;
				}
				else if (num < 100f)
				{
					dangerAmount = SimDebugView.DangerAmount.Moderate;
				}
				else if (num < 200f)
				{
					dangerAmount = SimDebugView.DangerAmount.High;
				}
				else if (num < 400f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryHigh;
				}
				else if (num > 800f)
				{
					dangerAmount = SimDebugView.DangerAmount.Extreme;
				}
			}
		}
		if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || (Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || Grid.Pressure[cell] < SimDebugView.minMinionPressure))))
		{
			dangerAmount++;
		}
		if (dangerAmount != SimDebugView.DangerAmount.None)
		{
			float num2 = (float)dangerAmount / 6f;
			result = Color.HSVToRGB((80f - num2 * 80f) / 360f, 1f, 1f);
		}
		return result;
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x001917D0 File Offset: 0x0018F9D0
	private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		float num = Grid.Mass[cell];
		float num2 = Grid.Temperature[cell];
		if (float.IsNaN(num) || float.IsNaN(num2) || num > 10000f || num2 > 10000f)
		{
			return Color.red;
		}
		if (element.IsVacuum)
		{
			if (num2 != 0f)
			{
				result = Color.yellow;
			}
			else if (num != 0f)
			{
				result = Color.blue;
			}
			else
			{
				result = Color.gray;
			}
		}
		else if (num2 < 10f)
		{
			result = Color.red;
		}
		else if (Grid.Mass[cell] < 1f && Grid.Pressure[cell] < 1f)
		{
			result = Color.green;
		}
		else if (num2 > element.highTemp + 3f && element.highTempTransition != null)
		{
			result = Color.magenta;
		}
		else if (num2 < element.lowTemp + 3f && element.lowTempTransition != null)
		{
			result = Color.cyan;
		}
		return result;
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x001918D8 File Offset: 0x0018FAD8
	private static Color GetFakeFloorColour(SimDebugView instance, int cell)
	{
		if (!Grid.FakeFloor[cell])
		{
			return Color.black;
		}
		return Color.cyan;
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x001918F2 File Offset: 0x0018FAF2
	private static Color GetFoundationColour(SimDebugView instance, int cell)
	{
		if (!Grid.Foundation[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x0019190C File Offset: 0x0018FB0C
	private static Color GetDupePassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupePassable[cell])
		{
			return Color.black;
		}
		return Color.green;
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x00191926 File Offset: 0x0018FB26
	private static Color GetCritterImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.CritterImpassable[cell])
		{
			return Color.black;
		}
		return Color.yellow;
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x00191940 File Offset: 0x0018FB40
	private static Color GetDupeImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupeImpassable[cell])
		{
			return Color.black;
		}
		return Color.red;
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x0019195A File Offset: 0x0018FB5A
	private static Color GetMinionOccupiedColour(SimDebugView instance, int cell)
	{
		if (!(Grid.Objects[cell, 0] != null))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x0019197B File Offset: 0x0018FB7B
	private static Color GetMinionGroupProberColour(SimDebugView instance, int cell)
	{
		if (!MinionGroupProber.Get().IsReachable(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x00191995 File Offset: 0x0018FB95
	private static Color GetPathProberColour(SimDebugView instance, int cell)
	{
		if (!(instance.selectedPathProber != null) || instance.selectedPathProber.GetCost(cell) == -1)
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x001919BF File Offset: 0x0018FBBF
	private static Color GetReservedColour(SimDebugView instance, int cell)
	{
		if (!Grid.Reserved[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x001919D9 File Offset: 0x0018FBD9
	private static Color GetAllowPathFindingColour(SimDebugView instance, int cell)
	{
		if (!Grid.AllowPathfinding[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x001919F4 File Offset: 0x0018FBF4
	private static Color GetMassColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!SimDebugView.IsInsulated(cell))
		{
			float num = Grid.Mass[cell];
			if (num > 0f)
			{
				float num2 = (num - SimDebugView.Instance.minMassExpected) / (SimDebugView.Instance.maxMassExpected - SimDebugView.Instance.minMassExpected);
				result = Color.HSVToRGB(1f - num2, 1f, 1f);
			}
		}
		return result;
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x00191A5E File Offset: 0x0018FC5E
	public static Color GetScenePartitionerColour(SimDebugView instance, int cell)
	{
		if (!GameScenePartitioner.Instance.DoDebugLayersContainItemsOnCell(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x04002F02 RID: 12034
	[SerializeField]
	public Material material;

	// Token: 0x04002F03 RID: 12035
	public Material diseaseMaterial;

	// Token: 0x04002F04 RID: 12036
	public bool hideFOW;

	// Token: 0x04002F05 RID: 12037
	public const int colourSize = 4;

	// Token: 0x04002F06 RID: 12038
	private byte[] texBytes;

	// Token: 0x04002F07 RID: 12039
	private int currentFrame;

	// Token: 0x04002F08 RID: 12040
	[SerializeField]
	private Texture2D tex;

	// Token: 0x04002F09 RID: 12041
	[SerializeField]
	private GameObject plane;

	// Token: 0x04002F0A RID: 12042
	private HashedString mode = global::OverlayModes.Power.ID;

	// Token: 0x04002F0B RID: 12043
	private SimDebugView.GameGridMode gameGridMode = SimDebugView.GameGridMode.DigAmount;

	// Token: 0x04002F0C RID: 12044
	private PathProber selectedPathProber;

	// Token: 0x04002F0D RID: 12045
	public float minTempExpected = 173.15f;

	// Token: 0x04002F0E RID: 12046
	public float maxTempExpected = 423.15f;

	// Token: 0x04002F0F RID: 12047
	public float minMassExpected = 1.0001f;

	// Token: 0x04002F10 RID: 12048
	public float maxMassExpected = 10000f;

	// Token: 0x04002F11 RID: 12049
	public float minPressureExpected = 1.300003f;

	// Token: 0x04002F12 RID: 12050
	public float maxPressureExpected = 201.3f;

	// Token: 0x04002F13 RID: 12051
	public float minThermalConductivity;

	// Token: 0x04002F14 RID: 12052
	public float maxThermalConductivity = 30f;

	// Token: 0x04002F15 RID: 12053
	public float thresholdRange = 0.001f;

	// Token: 0x04002F16 RID: 12054
	public float thresholdOpacity = 0.8f;

	// Token: 0x04002F17 RID: 12055
	public static float minimumBreathable = 0.05f;

	// Token: 0x04002F18 RID: 12056
	public static float optimallyBreathable = 1f;

	// Token: 0x04002F19 RID: 12057
	public SimDebugView.ColorThreshold[] temperatureThresholds;

	// Token: 0x04002F1A RID: 12058
	public SimDebugView.ColorThreshold[] heatFlowThresholds;

	// Token: 0x04002F1B RID: 12059
	public Color32[] networkColours;

	// Token: 0x04002F1C RID: 12060
	public Gradient breathableGradient = new Gradient();

	// Token: 0x04002F1D RID: 12061
	public Color32 unbreathableColour = new Color(0.5f, 0f, 0f);

	// Token: 0x04002F1E RID: 12062
	public Color32[] toxicColour = new Color32[]
	{
		new Color(0.5f, 0f, 0.5f),
		new Color(1f, 0f, 1f)
	};

	// Token: 0x04002F1F RID: 12063
	public static SimDebugView Instance;

	// Token: 0x04002F20 RID: 12064
	private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems = new WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData>();

	// Token: 0x04002F21 RID: 12065
	private int selectedCell;

	// Token: 0x04002F22 RID: 12066
	private Dictionary<HashedString, Action<SimDebugView, Texture>> dataUpdateFuncs = new Dictionary<HashedString, Action<SimDebugView, Texture>>
	{
		{
			global::OverlayModes.Temperature.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Decor.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint)
		},
		{
			global::OverlayModes.Disease.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDisease)
		}
	};

	// Token: 0x04002F23 RID: 12067
	private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs = new Dictionary<HashedString, Func<SimDebugView, int, Color>>
	{
		{
			global::OverlayModes.ThermalConductivity.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour)
		},
		{
			global::OverlayModes.Temperature.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode)
		},
		{
			global::OverlayModes.Disease.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour)
		},
		{
			global::OverlayModes.Decor.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour)
		},
		{
			global::OverlayModes.Light.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour)
		},
		{
			global::OverlayModes.Radiation.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour)
		},
		{
			global::OverlayModes.Rooms.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour)
		},
		{
			global::OverlayModes.Suit.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Priorities.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Crop.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Harvest.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			SimDebugView.OverlayModes.GameGrid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour)
		},
		{
			SimDebugView.OverlayModes.StateChange,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour)
		},
		{
			SimDebugView.OverlayModes.SimCheckErrorMap,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour)
		},
		{
			SimDebugView.OverlayModes.Foundation,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour)
		},
		{
			SimDebugView.OverlayModes.FakeFloor,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour)
		},
		{
			SimDebugView.OverlayModes.DupePassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour)
		},
		{
			SimDebugView.OverlayModes.DupeImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour)
		},
		{
			SimDebugView.OverlayModes.CritterImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour)
		},
		{
			SimDebugView.OverlayModes.MinionGroupProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour)
		},
		{
			SimDebugView.OverlayModes.PathProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour)
		},
		{
			SimDebugView.OverlayModes.Reserved,
			new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour)
		},
		{
			SimDebugView.OverlayModes.AllowPathFinding,
			new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour)
		},
		{
			SimDebugView.OverlayModes.Danger,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour)
		},
		{
			SimDebugView.OverlayModes.MinionOccupied,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour)
		},
		{
			SimDebugView.OverlayModes.Pressure,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour)
		},
		{
			SimDebugView.OverlayModes.TileType,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour)
		},
		{
			SimDebugView.OverlayModes.State,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour)
		},
		{
			SimDebugView.OverlayModes.SolidLiquid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour)
		},
		{
			SimDebugView.OverlayModes.Mass,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour)
		},
		{
			SimDebugView.OverlayModes.Joules,
			new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour)
		},
		{
			SimDebugView.OverlayModes.ScenePartitioner,
			new Func<SimDebugView, int, Color>(SimDebugView.GetScenePartitionerColour)
		}
	};

	// Token: 0x04002F24 RID: 12068
	public static readonly Color[] dbColours = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 1f, 1f, 0.3f),
		new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
		new Color(0f, 0.3137255f, 1f, 0.3f),
		new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
		new Color(0.078431375f, 1f, 0f, 0.7f),
		new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
		new Color(1f, 0.8235294f, 0f, 0.9f),
		new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
		new Color(1f, 0.41568628f, 0f, 0.9f),
		new Color(1f, 0.7058824f, 0.7058824f, 1f),
		new Color(1f, 0f, 0f, 1f),
		new Color(1f, 0f, 0f, 1f)
	};

	// Token: 0x04002F25 RID: 12069
	private static float minMinionTemperature = 260f;

	// Token: 0x04002F26 RID: 12070
	private static float maxMinionTemperature = 310f;

	// Token: 0x04002F27 RID: 12071
	private static float minMinionPressure = 80f;

	// Token: 0x020017D4 RID: 6100
	public static class OverlayModes
	{
		// Token: 0x04006FFC RID: 28668
		public static readonly HashedString Mass = "Mass";

		// Token: 0x04006FFD RID: 28669
		public static readonly HashedString Pressure = "Pressure";

		// Token: 0x04006FFE RID: 28670
		public static readonly HashedString GameGrid = "GameGrid";

		// Token: 0x04006FFF RID: 28671
		public static readonly HashedString ScenePartitioner = "ScenePartitioner";

		// Token: 0x04007000 RID: 28672
		public static readonly HashedString ConduitUpdates = "ConduitUpdates";

		// Token: 0x04007001 RID: 28673
		public static readonly HashedString Flow = "Flow";

		// Token: 0x04007002 RID: 28674
		public static readonly HashedString StateChange = "StateChange";

		// Token: 0x04007003 RID: 28675
		public static readonly HashedString SimCheckErrorMap = "SimCheckErrorMap";

		// Token: 0x04007004 RID: 28676
		public static readonly HashedString DupePassable = "DupePassable";

		// Token: 0x04007005 RID: 28677
		public static readonly HashedString Foundation = "Foundation";

		// Token: 0x04007006 RID: 28678
		public static readonly HashedString FakeFloor = "FakeFloor";

		// Token: 0x04007007 RID: 28679
		public static readonly HashedString CritterImpassable = "CritterImpassable";

		// Token: 0x04007008 RID: 28680
		public static readonly HashedString DupeImpassable = "DupeImpassable";

		// Token: 0x04007009 RID: 28681
		public static readonly HashedString MinionGroupProber = "MinionGroupProber";

		// Token: 0x0400700A RID: 28682
		public static readonly HashedString PathProber = "PathProber";

		// Token: 0x0400700B RID: 28683
		public static readonly HashedString Reserved = "Reserved";

		// Token: 0x0400700C RID: 28684
		public static readonly HashedString AllowPathFinding = "AllowPathFinding";

		// Token: 0x0400700D RID: 28685
		public static readonly HashedString Danger = "Danger";

		// Token: 0x0400700E RID: 28686
		public static readonly HashedString MinionOccupied = "MinionOccupied";

		// Token: 0x0400700F RID: 28687
		public static readonly HashedString TileType = "TileType";

		// Token: 0x04007010 RID: 28688
		public static readonly HashedString State = "State";

		// Token: 0x04007011 RID: 28689
		public static readonly HashedString SolidLiquid = "SolidLiquid";

		// Token: 0x04007012 RID: 28690
		public static readonly HashedString Joules = "Joules";
	}

	// Token: 0x020017D5 RID: 6101
	public enum GameGridMode
	{
		// Token: 0x04007014 RID: 28692
		GameSolidMap,
		// Token: 0x04007015 RID: 28693
		Lighting,
		// Token: 0x04007016 RID: 28694
		RoomMap,
		// Token: 0x04007017 RID: 28695
		Style,
		// Token: 0x04007018 RID: 28696
		PlantDensity,
		// Token: 0x04007019 RID: 28697
		DigAmount,
		// Token: 0x0400701A RID: 28698
		DupePassable
	}

	// Token: 0x020017D6 RID: 6102
	[Serializable]
	public struct ColorThreshold
	{
		// Token: 0x0400701B RID: 28699
		public string colorName;

		// Token: 0x0400701C RID: 28700
		public float value;
	}

	// Token: 0x020017D7 RID: 6103
	private struct UpdateSimViewSharedData
	{
		// Token: 0x06008F90 RID: 36752 RVA: 0x00322E1E File Offset: 0x0032101E
		public UpdateSimViewSharedData(SimDebugView instance, byte[] texture_bytes, HashedString sim_view_mode, SimDebugView sim_debug_view)
		{
			this.instance = instance;
			this.textureBytes = texture_bytes;
			this.simViewMode = sim_view_mode;
			this.simDebugView = sim_debug_view;
		}

		// Token: 0x0400701D RID: 28701
		public SimDebugView instance;

		// Token: 0x0400701E RID: 28702
		public HashedString simViewMode;

		// Token: 0x0400701F RID: 28703
		public SimDebugView simDebugView;

		// Token: 0x04007020 RID: 28704
		public byte[] textureBytes;
	}

	// Token: 0x020017D8 RID: 6104
	private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
	{
		// Token: 0x06008F91 RID: 36753 RVA: 0x00322E40 File Offset: 0x00321040
		public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
		{
			this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
			this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
			this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
			this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x00322EA0 File Offset: 0x003210A0
		public void Run(SimDebugView.UpdateSimViewSharedData shared_data)
		{
			Func<SimDebugView, int, Color> func;
			if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out func))
			{
				func = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
			}
			for (int i = this.y0; i <= this.y1; i++)
			{
				int num = Grid.XYToCell(this.x0, i);
				int num2 = Grid.XYToCell(this.x1, i);
				for (int j = num; j <= num2; j++)
				{
					int num3 = j * 4;
					if (Grid.IsActiveWorld(j))
					{
						Color color = func(shared_data.instance, j);
						shared_data.textureBytes[num3] = (byte)(Mathf.Min(color.r, 1f) * 255f);
						shared_data.textureBytes[num3 + 1] = (byte)(Mathf.Min(color.g, 1f) * 255f);
						shared_data.textureBytes[num3 + 2] = (byte)(Mathf.Min(color.b, 1f) * 255f);
						shared_data.textureBytes[num3 + 3] = (byte)(Mathf.Min(color.a, 1f) * 255f);
					}
					else
					{
						shared_data.textureBytes[num3] = 0;
						shared_data.textureBytes[num3 + 1] = 0;
						shared_data.textureBytes[num3 + 2] = 0;
						shared_data.textureBytes[num3 + 3] = 0;
					}
				}
			}
		}

		// Token: 0x04007021 RID: 28705
		private int x0;

		// Token: 0x04007022 RID: 28706
		private int y0;

		// Token: 0x04007023 RID: 28707
		private int x1;

		// Token: 0x04007024 RID: 28708
		private int y1;
	}

	// Token: 0x020017D9 RID: 6105
	public enum DangerAmount
	{
		// Token: 0x04007026 RID: 28710
		None,
		// Token: 0x04007027 RID: 28711
		VeryLow,
		// Token: 0x04007028 RID: 28712
		Low,
		// Token: 0x04007029 RID: 28713
		Moderate,
		// Token: 0x0400702A RID: 28714
		High,
		// Token: 0x0400702B RID: 28715
		VeryHigh,
		// Token: 0x0400702C RID: 28716
		Extreme,
		// Token: 0x0400702D RID: 28717
		MAX_DANGERAMOUNT = 6
	}
}
