using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x020008F3 RID: 2291
[AddComponentMenu("KMonoBehaviour/scripts/PropertyTextures")]
public class PropertyTextures : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004262 RID: 16994 RVA: 0x00172F48 File Offset: 0x00171148
	public static void DestroyInstance()
	{
		ShaderReloader.Unregister(new System.Action(PropertyTextures.instance.OnShadersReloaded));
		PropertyTextures.externalFlowTex = IntPtr.Zero;
		PropertyTextures.externalLiquidTex = IntPtr.Zero;
		PropertyTextures.externalExposedToSunlight = IntPtr.Zero;
		PropertyTextures.externalSolidDigAmountTex = IntPtr.Zero;
		PropertyTextures.instance = null;
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x00172F98 File Offset: 0x00171198
	protected override void OnPrefabInit()
	{
		PropertyTextures.instance = this;
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06004264 RID: 16996 RVA: 0x00172FB7 File Offset: 0x001711B7
	public static bool IsFogOfWarEnabled
	{
		get
		{
			return PropertyTextures.FogOfWarScale < 1f;
		}
	}

	// Token: 0x06004265 RID: 16997 RVA: 0x00172FC5 File Offset: 0x001711C5
	public Texture GetTexture(PropertyTextures.Property property)
	{
		return this.textureBuffers[(int)property].texture;
	}

	// Token: 0x06004266 RID: 16998 RVA: 0x00172FD4 File Offset: 0x001711D4
	private string GetShaderPropertyName(PropertyTextures.Property property)
	{
		return "_" + property.ToString() + "Tex";
	}

	// Token: 0x06004267 RID: 16999 RVA: 0x00172FF4 File Offset: 0x001711F4
	protected override void OnSpawn()
	{
		if (GenericGameSettings.instance.disableFogOfWar)
		{
			PropertyTextures.FogOfWarScale = 1f;
		}
		this.WorldSizeID = Shader.PropertyToID("_WorldSizeInfo");
		this.ClusterWorldSizeID = Shader.PropertyToID("_ClusterWorldSizeInfo");
		this.FogOfWarScaleID = Shader.PropertyToID("_FogOfWarScale");
		this.PropTexWsToCsID = Shader.PropertyToID("_PropTexWsToCs");
		this.PropTexCsToWsID = Shader.PropertyToID("_PropTexCsToWs");
		this.TopBorderHeightID = Shader.PropertyToID("_TopBorderHeight");
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x00173078 File Offset: 0x00171278
	public void OnReset(object data = null)
	{
		this.lerpers = new TextureLerper[14];
		this.texturePagePool = new TexturePagePool();
		this.textureBuffers = new TextureBuffer[14];
		this.externallyUpdatedTextures = new Texture2D[14];
		for (int i = 0; i < 14; i++)
		{
			PropertyTextures.TextureProperties textureProperties = new PropertyTextures.TextureProperties
			{
				textureFormat = TextureFormat.Alpha8,
				filterMode = FilterMode.Bilinear,
				blend = false,
				blendSpeed = 1f
			};
			for (int j = 0; j < this.textureProperties.Length; j++)
			{
				if (i == (int)this.textureProperties[j].simProperty)
				{
					textureProperties = this.textureProperties[j];
				}
			}
			PropertyTextures.Property property = (PropertyTextures.Property)i;
			textureProperties.name = property.ToString();
			if (this.externallyUpdatedTextures[i] != null)
			{
				UnityEngine.Object.Destroy(this.externallyUpdatedTextures[i]);
				this.externallyUpdatedTextures[i] = null;
			}
			Texture texture;
			if (textureProperties.updatedExternally)
			{
				this.externallyUpdatedTextures[i] = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureUtil.TextureFormatToGraphicsFormat(textureProperties.textureFormat), TextureCreationFlags.None);
				texture = this.externallyUpdatedTextures[i];
			}
			else
			{
				TextureBuffer[] array = this.textureBuffers;
				int num = i;
				property = (PropertyTextures.Property)i;
				array[num] = new TextureBuffer(property.ToString(), Grid.WidthInCells, Grid.HeightInCells, textureProperties.textureFormat, textureProperties.filterMode, this.texturePagePool);
				texture = this.textureBuffers[i].texture;
			}
			if (textureProperties.blend)
			{
				TextureLerper[] array2 = this.lerpers;
				int num2 = i;
				Texture target_texture = texture;
				property = (PropertyTextures.Property)i;
				array2[num2] = new TextureLerper(target_texture, property.ToString(), texture.filterMode, textureProperties.textureFormat);
				this.lerpers[i].Speed = textureProperties.blendSpeed;
			}
			string shaderPropertyName = this.GetShaderPropertyName((PropertyTextures.Property)i);
			texture.name = shaderPropertyName;
			textureProperties.texturePropertyName = shaderPropertyName;
			Shader.SetGlobalTexture(shaderPropertyName, texture);
			this.allTextureProperties.Add(textureProperties);
		}
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x0017325C File Offset: 0x0017145C
	private void OnShadersReloaded()
	{
		for (int i = 0; i < 14; i++)
		{
			TextureLerper textureLerper = this.lerpers[i];
			if (textureLerper != null)
			{
				Shader.SetGlobalTexture(this.allTextureProperties[i].texturePropertyName, textureLerper.Update());
			}
		}
	}

	// Token: 0x0600426A RID: 17002 RVA: 0x001732A0 File Offset: 0x001714A0
	public void Sim200ms(float dt)
	{
		if (this.lerpers == null || this.lerpers.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.lerpers.Length; i++)
		{
			TextureLerper textureLerper = this.lerpers[i];
			if (textureLerper != null)
			{
				textureLerper.LongUpdate(dt);
			}
		}
	}

	// Token: 0x0600426B RID: 17003 RVA: 0x001732E8 File Offset: 0x001714E8
	private void UpdateTextureThreaded(TextureRegion texture_region, int x0, int y0, int x1, int y1, PropertyTextures.WorkItem.Callback update_texture_cb)
	{
		this.workItems.Reset(null);
		int num = 16;
		for (int i = y0; i <= y1; i += num)
		{
			int y2 = Math.Min(i + num - 1, y1);
			this.workItems.Add(new PropertyTextures.WorkItem(texture_region, x0, i, x1, y2, update_texture_cb));
		}
		GlobalJobManager.Run(this.workItems);
	}

	// Token: 0x0600426C RID: 17004 RVA: 0x00173344 File Offset: 0x00171544
	private void UpdateProperty(ref PropertyTextures.TextureProperties p, int x0, int y0, int x1, int y1)
	{
		if (Game.Instance == null || Game.Instance.IsLoading())
		{
			return;
		}
		int simProperty = (int)p.simProperty;
		if (!p.updatedExternally)
		{
			TextureRegion texture_region = this.textureBuffers[simProperty].Lock(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
			switch (p.simProperty)
			{
			case PropertyTextures.Property.StateChange:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateStateChange));
				break;
			case PropertyTextures.Property.GasPressure:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdatePressure));
				break;
			case PropertyTextures.Property.GasColour:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateGasColour));
				break;
			case PropertyTextures.Property.GasDanger:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateDanger));
				break;
			case PropertyTextures.Property.FogOfWar:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFogOfWar));
				break;
			case PropertyTextures.Property.SolidDigAmount:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidDigAmount));
				break;
			case PropertyTextures.Property.SolidLiquidGasMass:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidLiquidGasMass));
				break;
			case PropertyTextures.Property.WorldLight:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateWorldLight));
				break;
			case PropertyTextures.Property.Temperature:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateTemperature));
				break;
			case PropertyTextures.Property.FallingSolid:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFallingSolidChange));
				break;
			case PropertyTextures.Property.Radiation:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateRadiation));
				break;
			}
			texture_region.Unlock();
			return;
		}
		PropertyTextures.Property simProperty2 = p.simProperty;
		if (simProperty2 != PropertyTextures.Property.Flow)
		{
			if (simProperty2 != PropertyTextures.Property.Liquid)
			{
				if (simProperty2 == PropertyTextures.Property.ExposedToSunlight)
				{
					this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalExposedToSunlight, Grid.WidthInCells * Grid.HeightInCells);
				}
			}
			else
			{
				this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalLiquidTex, 4 * Grid.WidthInCells * Grid.HeightInCells);
			}
		}
		else
		{
			this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalFlowTex, 8 * Grid.WidthInCells * Grid.HeightInCells);
		}
		this.externallyUpdatedTextures[simProperty].Apply();
	}

	// Token: 0x0600426D RID: 17005 RVA: 0x001735A8 File Offset: 0x001717A8
	public static Vector4 CalculateClusterWorldSize()
	{
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 zero = Vector4.zero;
		if (DlcManager.IsPureVanilla() || (CameraController.Instance != null && CameraController.Instance.ignoreClusterFX))
		{
			zero = new Vector4((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f, 0f);
		}
		else
		{
			zero = new Vector4((float)worldSize.x, (float)worldSize.y, (float)worldOffset.x, (float)worldOffset.y);
		}
		return zero;
	}

	// Token: 0x0600426E RID: 17006 RVA: 0x00173638 File Offset: 0x00171838
	private void LateUpdate()
	{
		if (!Grid.IsInitialized())
		{
			return;
		}
		Shader.SetGlobalVector(this.WorldSizeID, new Vector4((float)Grid.WidthInCells, (float)Grid.HeightInCells, 1f / (float)Grid.WidthInCells, 1f / (float)Grid.HeightInCells));
		Vector4 value = PropertyTextures.CalculateClusterWorldSize();
		Shader.SetGlobalVector(this.ClusterWorldSizeID, value);
		Shader.SetGlobalVector(this.PropTexWsToCsID, new Vector4(0f, 0f, 1f, 1f));
		Shader.SetGlobalVector(this.PropTexCsToWsID, new Vector4(0f, 0f, 1f, 1f));
		Shader.SetGlobalFloat(this.TopBorderHeightID, ClusterManager.Instance.activeWorld.FullyEnclosedBorder ? 0f : ((float)Grid.TopBorderHeight));
		int x;
		int y;
		int x2;
		int y2;
		this.GetVisibleCellRange(out x, out y, out x2, out y2);
		Shader.SetGlobalFloat(this.FogOfWarScaleID, PropertyTextures.FogOfWarScale);
		int nextPropertyIdx = this.NextPropertyIdx;
		this.NextPropertyIdx = nextPropertyIdx + 1;
		int num = nextPropertyIdx % this.allTextureProperties.Count;
		PropertyTextures.TextureProperties textureProperties = this.allTextureProperties[num];
		while (textureProperties.updateEveryFrame)
		{
			nextPropertyIdx = this.NextPropertyIdx;
			this.NextPropertyIdx = nextPropertyIdx + 1;
			num = nextPropertyIdx % this.allTextureProperties.Count;
			textureProperties = this.allTextureProperties[num];
		}
		for (int i = 0; i < this.allTextureProperties.Count; i++)
		{
			PropertyTextures.TextureProperties textureProperties2 = this.allTextureProperties[i];
			if (num == i || textureProperties2.updateEveryFrame || GameUtil.IsCapturingTimeLapse())
			{
				this.UpdateProperty(ref textureProperties2, x, y, x2, y2);
			}
		}
		for (int j = 0; j < 14; j++)
		{
			TextureLerper textureLerper = this.lerpers[j];
			if (textureLerper != null)
			{
				if (Time.timeScale == 0f)
				{
					textureLerper.LongUpdate(Time.unscaledDeltaTime);
				}
				Shader.SetGlobalTexture(this.allTextureProperties[j].texturePropertyName, textureLerper.Update());
			}
		}
	}

	// Token: 0x0600426F RID: 17007 RVA: 0x00173834 File Offset: 0x00171A34
	private void GetVisibleCellRange(out int x0, out int y0, out int x1, out int y1)
	{
		int num = 16;
		Grid.GetVisibleExtents(out x0, out y0, out x1, out y1);
		int widthInCells = Grid.WidthInCells;
		int heightInCells = Grid.HeightInCells;
		int num2 = 0;
		int num3 = 0;
		x0 = Math.Max(num2, x0 - num);
		y0 = Math.Max(num3, y0 - num);
		x0 = Mathf.Min(x0, widthInCells - 1);
		y0 = Mathf.Min(y0, heightInCells - 1);
		x1 = Mathf.CeilToInt((float)(x1 + num));
		y1 = Mathf.CeilToInt((float)(y1 + num));
		x1 = Mathf.Max(x1, num2);
		y1 = Mathf.Max(y1, num3);
		x1 = Mathf.Min(x1, widthInCells - 1);
		y1 = Mathf.Min(y1, heightInCells - 1);
	}

	// Token: 0x06004270 RID: 17008 RVA: 0x001738DC File Offset: 0x00171ADC
	private static void UpdateFogOfWar(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		byte[] visible = Grid.Visible;
		int y2 = Grid.HeightInCells;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			y2 = activeWorld.WorldSize.y + activeWorld.WorldOffset.y - 1;
		}
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					int num2 = Grid.XYToCell(j, y2);
					if (Grid.IsValidCell(num2))
					{
						region.SetBytes(j, i, visible[num2]);
					}
					else
					{
						region.SetBytes(j, i, 0);
					}
				}
				else
				{
					region.SetBytes(j, i, visible[num]);
				}
			}
		}
	}

	// Token: 0x06004271 RID: 17009 RVA: 0x00173998 File Offset: 0x00171B98
	private static void UpdatePressure(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 pressureRange = PropertyTextures.instance.PressureRange;
		float minPressureVisibility = PropertyTextures.instance.MinPressureVisibility;
		float num = pressureRange.y - pressureRange.x;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num2 = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num2))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num3 = 0f;
					Element element = Grid.Element[num2];
					if (element.IsGas)
					{
						float num4 = Grid.Pressure[num2];
						float b = (num4 > 0f) ? minPressureVisibility : 0f;
						num3 = Mathf.Max(Mathf.Clamp01((num4 - pressureRange.x) / num), b);
					}
					else if (element.IsLiquid)
					{
						int num5 = Grid.CellAbove(num2);
						if (Grid.IsValidCell(num5) && Grid.Element[num5].IsGas)
						{
							float num6 = Grid.Pressure[num5];
							float b2 = (num6 > 0f) ? minPressureVisibility : 0f;
							num3 = Mathf.Max(Mathf.Clamp01((num6 - pressureRange.x) / num), b2);
						}
					}
					region.SetBytes(j, i, (byte)(num3 * 255f));
				}
			}
		}
	}

	// Token: 0x06004272 RID: 17010 RVA: 0x00173AD8 File Offset: 0x00171CD8
	private static void UpdateDanger(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					byte b = (Grid.Element[num].id == SimHashes.Oxygen) ? 0 : byte.MaxValue;
					region.SetBytes(j, i, b);
				}
			}
		}
	}

	// Token: 0x06004273 RID: 17011 RVA: 0x00173B44 File Offset: 0x00171D44
	private static void UpdateStateChange(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		float temperatureStateChangeRange = PropertyTextures.instance.TemperatureStateChangeRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num2 = 0f;
					Element element = Grid.Element[num];
					if (!element.IsVacuum)
					{
						float num3 = Grid.Temperature[num];
						float num4 = element.lowTemp * temperatureStateChangeRange;
						float a = Mathf.Abs(num3 - element.lowTemp) / num4;
						float num5 = element.highTemp * temperatureStateChangeRange;
						float b = Mathf.Abs(num3 - element.highTemp) / num5;
						num2 = Mathf.Max(num2, 1f - Mathf.Min(a, b));
					}
					region.SetBytes(j, i, (byte)(num2 * 255f));
				}
			}
		}
	}

	// Token: 0x06004274 RID: 17012 RVA: 0x00173C2C File Offset: 0x00171E2C
	private static void UpdateFallingSolidChange(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num2 = 0f;
					Element element = Grid.Element[num];
					if (element.id == SimHashes.Mud || element.id == SimHashes.ToxicMud)
					{
						num2 = 0.65f;
					}
					region.SetBytes(j, i, (byte)(num2 * 255f));
				}
			}
		}
	}

	// Token: 0x06004275 RID: 17013 RVA: 0x00173CB0 File Offset: 0x00171EB0
	private static void UpdateGasColour(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0, 0);
				}
				else
				{
					Element element = Grid.Element[num];
					if (element.IsGas)
					{
						region.SetBytes(j, i, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
					}
					else if (element.IsLiquid)
					{
						if (Grid.IsValidCell(Grid.CellAbove(num)))
						{
							region.SetBytes(j, i, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
						}
						else
						{
							region.SetBytes(j, i, 0, 0, 0, 0);
						}
					}
					else
					{
						region.SetBytes(j, i, 0, 0, 0, 0);
					}
				}
			}
		}
	}

	// Token: 0x06004276 RID: 17014 RVA: 0x00173DC8 File Offset: 0x00171FC8
	private static void UpdateLiquid(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = x0; i <= x1; i++)
		{
			int num = Grid.XYToCell(i, y1);
			Element element = Grid.Element[num];
			for (int j = y1; j >= y0; j--)
			{
				int num2 = Grid.XYToCell(i, j);
				if (!Grid.IsActiveWorld(num2))
				{
					region.SetBytes(i, j, 0, 0, 0, 0);
				}
				else
				{
					Element element2 = Grid.Element[num2];
					if (element2.IsLiquid)
					{
						Color32 colour = element2.substance.colour;
						float liquidMaxMass = Lighting.Instance.Settings.LiquidMaxMass;
						float liquidAmountOffset = Lighting.Instance.Settings.LiquidAmountOffset;
						float num3;
						if (element.IsLiquid || element.IsSolid)
						{
							num3 = 1f;
						}
						else
						{
							num3 = liquidAmountOffset + (1f - liquidAmountOffset) * Mathf.Min(Grid.Mass[num2] / liquidMaxMass, 1f);
							num3 = Mathf.Pow(Mathf.Min(Grid.Mass[num2] / liquidMaxMass, 1f), 0.45f);
						}
						region.SetBytes(i, j, (byte)(num3 * 255f), colour.r, colour.g, colour.b);
					}
					else
					{
						region.SetBytes(i, j, 0, 0, 0, 0);
					}
					element = element2;
				}
			}
		}
	}

	// Token: 0x06004277 RID: 17015 RVA: 0x00173F14 File Offset: 0x00172114
	private static void UpdateSolidDigAmount(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Void);
		for (int i = y0; i <= y1; i++)
		{
			int num = Grid.XYToCell(x0, i);
			int num2 = Grid.XYToCell(x1, i);
			int j = num;
			int num3 = x0;
			while (j <= num2)
			{
				byte b = 0;
				byte b2 = 0;
				byte b3 = 0;
				if (Grid.ElementIdx[j] != elementIndex)
				{
					b3 = byte.MaxValue;
				}
				if (Grid.Solid[j])
				{
					b = byte.MaxValue;
					b2 = (byte)(255f * Grid.Damage[j]);
				}
				region.SetBytes(num3, i, b, b2, b3);
				j++;
				num3++;
			}
		}
	}

	// Token: 0x06004278 RID: 17016 RVA: 0x00173FB0 File Offset: 0x001721B0
	private static void UpdateSolidLiquidGasMass(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0, 0);
				}
				else
				{
					Element element = Grid.Element[num];
					byte b = 0;
					byte b2 = 0;
					byte b3 = 0;
					if (element.IsSolid)
					{
						b = byte.MaxValue;
					}
					else if (element.IsLiquid)
					{
						b2 = byte.MaxValue;
					}
					else if (element.IsGas || element.IsVacuum)
					{
						b3 = byte.MaxValue;
					}
					float num2 = Grid.Mass[num];
					float num3 = Mathf.Min(1f, num2 / 2000f);
					if (num2 > 0f)
					{
						num3 = Mathf.Max(0.003921569f, num3);
					}
					region.SetBytes(j, i, b, b2, b3, (byte)(num3 * 255f));
				}
			}
		}
	}

	// Token: 0x06004279 RID: 17017 RVA: 0x001740A0 File Offset: 0x001722A0
	private static void GetTemperatureAlpha(float t, Vector2 cold_range, Vector2 hot_range, out byte cold_alpha, out byte hot_alpha)
	{
		cold_alpha = 0;
		hot_alpha = 0;
		if (t <= cold_range.y)
		{
			float num = Mathf.Clamp01((cold_range.y - t) / (cold_range.y - cold_range.x));
			cold_alpha = (byte)(num * 255f);
			return;
		}
		if (t >= hot_range.x)
		{
			float num2 = Mathf.Clamp01((t - hot_range.x) / (hot_range.y - hot_range.x));
			hot_alpha = (byte)(num2 * 255f);
		}
	}

	// Token: 0x0600427A RID: 17018 RVA: 0x00174114 File Offset: 0x00172314
	private static void UpdateTemperature(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 cold_range = PropertyTextures.instance.coldRange;
		Vector2 hot_range = PropertyTextures.instance.hotRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0);
				}
				else
				{
					float num2 = Grid.Temperature[num];
					byte b;
					byte b2;
					PropertyTextures.GetTemperatureAlpha(num2, cold_range, hot_range, out b, out b2);
					byte b3 = (byte)(255f * Mathf.Pow(Mathf.Clamp(num2 / 1000f, 0f, 1f), 0.45f));
					region.SetBytes(j, i, b, b2, b3);
				}
			}
		}
	}

	// Token: 0x0600427B RID: 17019 RVA: 0x001741CC File Offset: 0x001723CC
	private static void UpdateWorldLight(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		if (!PropertyTextures.instance.ForceLightEverywhere)
		{
			for (int i = y0; i <= y1; i++)
			{
				int num = Grid.XYToCell(x0, i);
				int num2 = Grid.XYToCell(x1, i);
				int j = num;
				int num3 = x0;
				while (j <= num2)
				{
					Color32 color = (Grid.LightCount[j] > 0) ? Lighting.Instance.Settings.LightColour : new Color32(0, 0, 0, byte.MaxValue);
					region.SetBytes(num3, i, color.r, color.g, color.b, (color.r + color.g + color.b > 0) ? byte.MaxValue : 0);
					j++;
					num3++;
				}
			}
			return;
		}
		for (int k = y0; k <= y1; k++)
		{
			for (int l = x0; l <= x1; l++)
			{
				region.SetBytes(l, k, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}
	}

	// Token: 0x0600427C RID: 17020 RVA: 0x001742C4 File Offset: 0x001724C4
	private static void UpdateRadiation(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 vector = PropertyTextures.instance.coldRange;
		Vector2 vector2 = PropertyTextures.instance.hotRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0);
				}
				else
				{
					float v = Grid.Radiation[num];
					region.SetBytes(j, i, v);
				}
			}
		}
	}

	// Token: 0x04002B52 RID: 11090
	[NonSerialized]
	public bool ForceLightEverywhere;

	// Token: 0x04002B53 RID: 11091
	[SerializeField]
	private Vector2 PressureRange = new Vector2(15f, 200f);

	// Token: 0x04002B54 RID: 11092
	[SerializeField]
	private float MinPressureVisibility = 0.1f;

	// Token: 0x04002B55 RID: 11093
	[SerializeField]
	[Range(0f, 1f)]
	private float TemperatureStateChangeRange = 0.05f;

	// Token: 0x04002B56 RID: 11094
	public static PropertyTextures instance;

	// Token: 0x04002B57 RID: 11095
	public static IntPtr externalFlowTex;

	// Token: 0x04002B58 RID: 11096
	public static IntPtr externalLiquidTex;

	// Token: 0x04002B59 RID: 11097
	public static IntPtr externalExposedToSunlight;

	// Token: 0x04002B5A RID: 11098
	public static IntPtr externalSolidDigAmountTex;

	// Token: 0x04002B5B RID: 11099
	[SerializeField]
	private Vector2 coldRange;

	// Token: 0x04002B5C RID: 11100
	[SerializeField]
	private Vector2 hotRange;

	// Token: 0x04002B5D RID: 11101
	public static float FogOfWarScale;

	// Token: 0x04002B5E RID: 11102
	private int WorldSizeID;

	// Token: 0x04002B5F RID: 11103
	private int ClusterWorldSizeID;

	// Token: 0x04002B60 RID: 11104
	private int FogOfWarScaleID;

	// Token: 0x04002B61 RID: 11105
	private int PropTexWsToCsID;

	// Token: 0x04002B62 RID: 11106
	private int PropTexCsToWsID;

	// Token: 0x04002B63 RID: 11107
	private int TopBorderHeightID;

	// Token: 0x04002B64 RID: 11108
	private int NextPropertyIdx;

	// Token: 0x04002B65 RID: 11109
	public TextureBuffer[] textureBuffers;

	// Token: 0x04002B66 RID: 11110
	public TextureLerper[] lerpers;

	// Token: 0x04002B67 RID: 11111
	private TexturePagePool texturePagePool;

	// Token: 0x04002B68 RID: 11112
	[SerializeField]
	private Texture2D[] externallyUpdatedTextures;

	// Token: 0x04002B69 RID: 11113
	private PropertyTextures.TextureProperties[] textureProperties = new PropertyTextures.TextureProperties[]
	{
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Flow,
			textureFormat = TextureFormat.RGFloat,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Liquid,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 1f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.ExposedToSunlight,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidDigAmount,
			textureFormat = TextureFormat.RGB24,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasColour,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasDanger,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasPressure,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.FogOfWar,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.WorldLight,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.StateChange,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.FallingSolid,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidLiquidGasMass,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Temperature,
			textureFormat = TextureFormat.RGB24,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Radiation,
			textureFormat = TextureFormat.RFloat,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		}
	};

	// Token: 0x04002B6A RID: 11114
	private List<PropertyTextures.TextureProperties> allTextureProperties = new List<PropertyTextures.TextureProperties>();

	// Token: 0x04002B6B RID: 11115
	private WorkItemCollection<PropertyTextures.WorkItem, object> workItems = new WorkItemCollection<PropertyTextures.WorkItem, object>();

	// Token: 0x02001748 RID: 5960
	public enum Property
	{
		// Token: 0x04006E2A RID: 28202
		StateChange,
		// Token: 0x04006E2B RID: 28203
		GasPressure,
		// Token: 0x04006E2C RID: 28204
		GasColour,
		// Token: 0x04006E2D RID: 28205
		GasDanger,
		// Token: 0x04006E2E RID: 28206
		FogOfWar,
		// Token: 0x04006E2F RID: 28207
		Flow,
		// Token: 0x04006E30 RID: 28208
		SolidDigAmount,
		// Token: 0x04006E31 RID: 28209
		SolidLiquidGasMass,
		// Token: 0x04006E32 RID: 28210
		WorldLight,
		// Token: 0x04006E33 RID: 28211
		Liquid,
		// Token: 0x04006E34 RID: 28212
		Temperature,
		// Token: 0x04006E35 RID: 28213
		ExposedToSunlight,
		// Token: 0x04006E36 RID: 28214
		FallingSolid,
		// Token: 0x04006E37 RID: 28215
		Radiation,
		// Token: 0x04006E38 RID: 28216
		Num
	}

	// Token: 0x02001749 RID: 5961
	private struct TextureProperties
	{
		// Token: 0x04006E39 RID: 28217
		public string name;

		// Token: 0x04006E3A RID: 28218
		public PropertyTextures.Property simProperty;

		// Token: 0x04006E3B RID: 28219
		public TextureFormat textureFormat;

		// Token: 0x04006E3C RID: 28220
		public FilterMode filterMode;

		// Token: 0x04006E3D RID: 28221
		public bool updateEveryFrame;

		// Token: 0x04006E3E RID: 28222
		public bool updatedExternally;

		// Token: 0x04006E3F RID: 28223
		public bool blend;

		// Token: 0x04006E40 RID: 28224
		public float blendSpeed;

		// Token: 0x04006E41 RID: 28225
		public string texturePropertyName;
	}

	// Token: 0x0200174A RID: 5962
	private struct WorkItem : IWorkItem<object>
	{
		// Token: 0x06008E0D RID: 36365 RVA: 0x0031E674 File Offset: 0x0031C874
		public WorkItem(TextureRegion texture_region, int x0, int y0, int x1, int y1, PropertyTextures.WorkItem.Callback update_texture_cb)
		{
			this.textureRegion = texture_region;
			this.x0 = x0;
			this.y0 = y0;
			this.x1 = x1;
			this.y1 = y1;
			this.updateTextureCb = update_texture_cb;
		}

		// Token: 0x06008E0E RID: 36366 RVA: 0x0031E6A3 File Offset: 0x0031C8A3
		public void Run(object shared_data)
		{
			this.updateTextureCb(this.textureRegion, this.x0, this.y0, this.x1, this.y1);
		}

		// Token: 0x04006E42 RID: 28226
		private int x0;

		// Token: 0x04006E43 RID: 28227
		private int y0;

		// Token: 0x04006E44 RID: 28228
		private int x1;

		// Token: 0x04006E45 RID: 28229
		private int y1;

		// Token: 0x04006E46 RID: 28230
		private TextureRegion textureRegion;

		// Token: 0x04006E47 RID: 28231
		private PropertyTextures.WorkItem.Callback updateTextureCb;

		// Token: 0x020021CE RID: 8654
		// (Invoke) Token: 0x0600ABC3 RID: 43971
		public delegate void Callback(TextureRegion texture_region, int x0, int y0, int x1, int y1);
	}
}
