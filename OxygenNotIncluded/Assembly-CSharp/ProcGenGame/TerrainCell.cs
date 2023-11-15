using System;
using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGen.Map;
using UnityEngine;
using VoronoiTree;

namespace ProcGenGame
{
	// Token: 0x02000CB0 RID: 3248
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TerrainCell
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600676D RID: 26477 RVA: 0x0026DC11 File Offset: 0x0026BE11
		public Polygon poly
		{
			get
			{
				return this.site.poly;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600676E RID: 26478 RVA: 0x0026DC1E File Offset: 0x0026BE1E
		// (set) Token: 0x0600676F RID: 26479 RVA: 0x0026DC26 File Offset: 0x0026BE26
		public Cell node { get; private set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06006770 RID: 26480 RVA: 0x0026DC2F File Offset: 0x0026BE2F
		// (set) Token: 0x06006771 RID: 26481 RVA: 0x0026DC37 File Offset: 0x0026BE37
		public Diagram.Site site { get; private set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06006772 RID: 26482 RVA: 0x0026DC40 File Offset: 0x0026BE40
		// (set) Token: 0x06006773 RID: 26483 RVA: 0x0026DC48 File Offset: 0x0026BE48
		public Dictionary<Tag, int> distancesToTags { get; private set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06006774 RID: 26484 RVA: 0x0026DC51 File Offset: 0x0026BE51
		public bool HasMobs
		{
			get
			{
				return this.mobs != null && this.mobs.Count > 0;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06006775 RID: 26485 RVA: 0x0026DC6B File Offset: 0x0026BE6B
		// (set) Token: 0x06006776 RID: 26486 RVA: 0x0026DC73 File Offset: 0x0026BE73
		public List<KeyValuePair<int, Tag>> mobs { get; private set; }

		// Token: 0x06006777 RID: 26487 RVA: 0x0026DC7C File Offset: 0x0026BE7C
		protected TerrainCell()
		{
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x0026DC8F File Offset: 0x0026BE8F
		protected TerrainCell(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags)
		{
			this.node = node;
			this.site = site;
			this.distancesToTags = distancesToTags;
		}

		// Token: 0x06006779 RID: 26489 RVA: 0x0026DCB7 File Offset: 0x0026BEB7
		public virtual void LogInfo(string evt, string param, float value)
		{
			global::Debug.Log(string.Concat(new string[]
			{
				evt,
				":",
				param,
				"=",
				value.ToString()
			}));
		}

		// Token: 0x0600677A RID: 26490 RVA: 0x0026DCEC File Offset: 0x0026BEEC
		public void InitializeCells(HashSet<int> claimedCells)
		{
			if (this.allCells != null)
			{
				return;
			}
			this.allCells = new List<int>();
			this.availableTerrainPoints = new HashSet<int>();
			this.availableSpawnPoints = new HashSet<int>();
			for (int i = 0; i < Grid.HeightInCells; i++)
			{
				for (int j = 0; j < Grid.WidthInCells; j++)
				{
					if (this.poly.Contains(new Vector2((float)j, (float)i)))
					{
						int item = Grid.XYToCell(j, i);
						this.availableTerrainPoints.Add(item);
						this.availableSpawnPoints.Add(item);
						if (claimedCells.Add(item))
						{
							this.allCells.Add(item);
						}
					}
				}
			}
			this.LogInfo("Initialise cells", "", (float)this.allCells.Count);
		}

		// Token: 0x0600677B RID: 26491 RVA: 0x0026DDAC File Offset: 0x0026BFAC
		public List<int> GetAllCells()
		{
			return new List<int>(this.allCells);
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x0026DDBC File Offset: 0x0026BFBC
		public List<int> GetAvailableSpawnCellsAll()
		{
			List<int> list = new List<int>();
			foreach (int item in this.availableSpawnPoints)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x0026DE18 File Offset: 0x0026C018
		public List<int> GetAvailableSpawnCellsFeature()
		{
			List<int> list = new List<int>();
			HashSet<int> hashSet = new HashSet<int>(this.availableSpawnPoints);
			hashSet.ExceptWith(this.availableTerrainPoints);
			foreach (int item in hashSet)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600677E RID: 26494 RVA: 0x0026DE84 File Offset: 0x0026C084
		public List<int> GetAvailableSpawnCellsBiome()
		{
			List<int> list = new List<int>();
			HashSet<int> hashSet = new HashSet<int>(this.availableSpawnPoints);
			hashSet.ExceptWith(this.featureSpawnPoints);
			foreach (int item in hashSet)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x0026DEF0 File Offset: 0x0026C0F0
		private bool RemoveFromAvailableSpawnCells(int cell)
		{
			return this.availableSpawnPoints.Remove(cell);
		}

		// Token: 0x06006780 RID: 26496 RVA: 0x0026DF00 File Offset: 0x0026C100
		public void AddMobs(IEnumerable<KeyValuePair<int, Tag>> newMobs)
		{
			foreach (KeyValuePair<int, Tag> mob in newMobs)
			{
				this.AddMob(mob);
			}
		}

		// Token: 0x06006781 RID: 26497 RVA: 0x0026DF48 File Offset: 0x0026C148
		private void AddMob(int cellIdx, string tag)
		{
			this.AddMob(new KeyValuePair<int, Tag>(cellIdx, new Tag(tag)));
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x0026DF5C File Offset: 0x0026C15C
		public void AddMob(KeyValuePair<int, Tag> mob)
		{
			if (this.mobs == null)
			{
				this.mobs = new List<KeyValuePair<int, Tag>>();
			}
			this.mobs.Add(mob);
			bool flag = this.RemoveFromAvailableSpawnCells(mob.Key);
			this.LogInfo("\t\t\tRemoveFromAvailableCells", mob.Value.Name + ": " + (flag ? "success" : "failed"), (float)mob.Key);
			if (!flag)
			{
				if (!this.allCells.Contains(mob.Key))
				{
					global::Debug.Assert(false, string.Concat(new string[]
					{
						"Couldnt find cell [",
						mob.Key.ToString(),
						"] we dont own, to remove for mob [",
						mob.Value.Name,
						"]"
					}));
					return;
				}
				global::Debug.Assert(false, string.Concat(new string[]
				{
					"Couldnt find cell [",
					mob.Key.ToString(),
					"] to remove for mob [",
					mob.Value.Name,
					"]"
				}));
			}
		}

		// Token: 0x06006783 RID: 26499 RVA: 0x0026E088 File Offset: 0x0026C288
		protected string GetSubWorldType(WorldGen worldGen)
		{
			Vector2I pos = new Vector2I((int)this.site.poly.Centroid().x, (int)this.site.poly.Centroid().y);
			return worldGen.GetSubWorldType(pos);
		}

		// Token: 0x06006784 RID: 26500 RVA: 0x0026E0D0 File Offset: 0x0026C2D0
		protected Temperature.Range GetTemperatureRange(WorldGen worldGen)
		{
			string subWorldType = this.GetSubWorldType(worldGen);
			if (subWorldType == null)
			{
				return Temperature.Range.Mild;
			}
			if (!worldGen.Settings.HasSubworld(subWorldType))
			{
				return Temperature.Range.Mild;
			}
			return worldGen.Settings.GetSubWorld(subWorldType).temperatureRange;
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x0026E10C File Offset: 0x0026C30C
		protected void GetTemperatureRange(WorldGen worldGen, ref float min, ref float range)
		{
			Temperature.Range temperatureRange = this.GetTemperatureRange(worldGen);
			min = SettingsCache.temperatures[temperatureRange].min;
			range = SettingsCache.temperatures[temperatureRange].max - min;
		}

		// Token: 0x06006786 RID: 26502 RVA: 0x0026E148 File Offset: 0x0026C348
		protected float GetDensityMassForCell(Chunk world, int cellIdx, float mass)
		{
			if (!Grid.IsValidCell(cellIdx))
			{
				return 0f;
			}
			global::Debug.Assert(world.density[cellIdx] >= 0f && world.density[cellIdx] <= 1f, "Density [" + world.density[cellIdx].ToString() + "] out of range [0-1]");
			float num = 0.2f * (world.density[cellIdx] - 0.5f);
			float num2 = mass + mass * num;
			if (num2 > 10000f)
			{
				num2 = 10000f;
			}
			return num2;
		}

		// Token: 0x06006787 RID: 26503 RVA: 0x0026E1D8 File Offset: 0x0026C3D8
		private void HandleSprinkleOfElement(WorldGenSettings settings, Tag targetTag, Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd)
		{
			Element element = ElementLoader.FindElementByName(settings.GetFeature(targetTag.Name).GetOneWeightedSimHash("SprinkleOfElementChoices", rnd).element);
			ProcGen.Room room = null;
			SettingsCache.rooms.TryGetValue(targetTag.Name, out room);
			SampleDescriber sampleDescriber = room;
			Sim.PhysicsData defaultValues = element.defaultValues;
			Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
			for (int i = 0; i < this.terrainPositions.Count; i++)
			{
				if (!(this.terrainPositions[i].Value != targetTag))
				{
					float radius = rnd.RandomRange(sampleDescriber.blobSize.min, sampleDescriber.blobSize.max);
					List<Vector2I> filledCircle = ProcGen.Util.GetFilledCircle(Grid.CellToPos2D(this.terrainPositions[i].Key), radius);
					for (int j = 0; j < filledCircle.Count; j++)
					{
						int num = Grid.XYToCell(filledCircle[j].x, filledCircle[j].y);
						if (Grid.IsValidCell(num))
						{
							defaultValues.mass = this.GetDensityMassForCell(world, num, element.defaultValues.mass);
							defaultValues.temperature = temperatureMin + world.heatOffset[num] * temperatureRange;
							SetValues(num, element, defaultValues, invalid);
						}
					}
				}
			}
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x0026E340 File Offset: 0x0026C540
		private HashSet<Vector2I> DigFeature(ProcGen.Room.Shape shape, float size, List<int> bordersWidths, SeededRandom rnd, out List<Vector2I> featureCenterPoints, out List<List<Vector2I>> featureBorders)
		{
			HashSet<Vector2I> hashSet = new HashSet<Vector2I>();
			featureCenterPoints = new List<Vector2I>();
			featureBorders = new List<List<Vector2I>>();
			if (size < 1f)
			{
				return hashSet;
			}
			Vector2 center = this.site.poly.Centroid();
			this.finalSize = size;
			switch (shape)
			{
			case ProcGen.Room.Shape.Circle:
				featureCenterPoints = ProcGen.Util.GetFilledCircle(center, this.finalSize);
				break;
			case ProcGen.Room.Shape.Blob:
				featureCenterPoints = ProcGen.Util.GetBlob(center, this.finalSize, rnd.RandomSource());
				break;
			case ProcGen.Room.Shape.Square:
				featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize, this.finalSize, rnd, 2f, 2f);
				break;
			case ProcGen.Room.Shape.TallThin:
				featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize / 4f, this.finalSize, rnd, 2f, 2f);
				break;
			case ProcGen.Room.Shape.ShortWide:
				featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize, this.finalSize / 4f, rnd, 2f, 2f);
				break;
			case ProcGen.Room.Shape.Splat:
				featureCenterPoints = ProcGen.Util.GetSplat(center, this.finalSize, rnd.RandomSource());
				break;
			}
			hashSet.UnionWith(featureCenterPoints);
			if (featureCenterPoints.Count == 0)
			{
				global::Debug.LogWarning(string.Concat(new string[]
				{
					"Room has no centerpoints. Terrain Cell [ shape: ",
					shape.ToString(),
					" size: ",
					this.finalSize.ToString(),
					"] [",
					this.node.NodeId.ToString(),
					" ",
					this.node.type,
					" ",
					this.node.position.ToString(),
					"]"
				}));
			}
			else if (bordersWidths != null && bordersWidths.Count > 0 && bordersWidths[0] > 0)
			{
				int num = 0;
				while (num < bordersWidths.Count && bordersWidths[num] > 0)
				{
					featureBorders.Add(ProcGen.Util.GetBorder(hashSet, bordersWidths[num]));
					hashSet.UnionWith(featureBorders[num]);
					num++;
				}
			}
			return hashSet;
		}

		// Token: 0x06006789 RID: 26505 RVA: 0x0026E588 File Offset: 0x0026C788
		public static TerrainCell.ElementOverride GetElementOverride(string element, SampleDescriber.Override overrides)
		{
			global::Debug.Assert(element != null && element.Length > 0);
			TerrainCell.ElementOverride elementOverride = new TerrainCell.ElementOverride
			{
				element = ElementLoader.FindElementByName(element)
			};
			global::Debug.Assert(elementOverride.element != null, "Couldn't find an element called " + element);
			elementOverride.pdelement = elementOverride.element.defaultValues;
			elementOverride.dc = Sim.DiseaseCell.Invalid;
			elementOverride.mass = elementOverride.element.defaultValues.mass;
			elementOverride.temperature = elementOverride.element.defaultValues.temperature;
			if (overrides == null)
			{
				return elementOverride;
			}
			elementOverride.overrideMass = false;
			elementOverride.overrideTemperature = false;
			elementOverride.overrideDiseaseIdx = false;
			elementOverride.overrideDiseaseAmount = false;
			if (overrides.massOverride != null)
			{
				elementOverride.mass = overrides.massOverride.Value;
				elementOverride.overrideMass = true;
			}
			if (overrides.massMultiplier != null)
			{
				elementOverride.mass *= overrides.massMultiplier.Value;
				elementOverride.overrideMass = true;
			}
			if (overrides.temperatureOverride != null)
			{
				elementOverride.temperature = overrides.temperatureOverride.Value;
				elementOverride.overrideTemperature = true;
			}
			if (overrides.temperatureMultiplier != null)
			{
				elementOverride.temperature *= overrides.temperatureMultiplier.Value;
				elementOverride.overrideTemperature = true;
			}
			if (overrides.diseaseOverride != null)
			{
				elementOverride.diseaseIdx = WorldGen.diseaseStats.GetIndex(overrides.diseaseOverride);
				elementOverride.overrideDiseaseIdx = true;
			}
			if (overrides.diseaseAmountOverride != null)
			{
				elementOverride.diseaseAmount = overrides.diseaseAmountOverride.Value;
				elementOverride.overrideDiseaseAmount = true;
			}
			if (elementOverride.overrideTemperature)
			{
				elementOverride.pdelement.temperature = elementOverride.temperature;
			}
			if (elementOverride.overrideMass)
			{
				elementOverride.pdelement.mass = elementOverride.mass;
			}
			if (elementOverride.overrideDiseaseIdx)
			{
				elementOverride.dc.diseaseIdx = elementOverride.diseaseIdx;
			}
			if (elementOverride.overrideDiseaseAmount)
			{
				elementOverride.dc.elementCount = elementOverride.diseaseAmount;
			}
			return elementOverride;
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x0026E7CC File Offset: 0x0026C9CC
		private bool IsFeaturePointContainedInBorder(Vector2 point, WorldGen worldGen)
		{
			if (!this.node.tags.Contains(WorldGenTags.AllowExceedNodeBorders))
			{
				return true;
			}
			if (!this.poly.Contains(point))
			{
				TerrainCell terrainCell = worldGen.TerrainCells.Find((TerrainCell x) => x.poly.Contains(point));
				if (terrainCell != null)
				{
					SubWorld subWorld = worldGen.Settings.GetSubWorld(this.node.GetSubworld());
					SubWorld subWorld2 = worldGen.Settings.GetSubWorld(terrainCell.node.GetSubworld());
					if (subWorld.zoneType != subWorld2.zoneType)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x0026E86C File Offset: 0x0026CA6C
		private void ApplyPlaceElementForRoom(FeatureSettings feature, string group, List<Vector2I> cells, WorldGen worldGen, Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd, HashSet<int> highPriorityClaims)
		{
			if (cells == null || cells.Count == 0)
			{
				return;
			}
			if (!feature.HasGroup(group))
			{
				return;
			}
			switch (feature.ElementChoiceGroups[group].selectionMethod)
			{
			case ProcGen.Room.Selection.Weighted:
			case ProcGen.Room.Selection.WeightedResample:
				for (int i = 0; i < cells.Count; i++)
				{
					int num = Grid.XYToCell(cells[i].x, cells[i].y);
					if (Grid.IsValidCell(num) && !highPriorityClaims.Contains(num) && this.IsFeaturePointContainedInBorder(cells[i], worldGen))
					{
						WeightedSimHash oneWeightedSimHash = feature.GetOneWeightedSimHash(group, rnd);
						TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(oneWeightedSimHash.element, oneWeightedSimHash.overrides);
						if (!elementOverride.overrideTemperature)
						{
							elementOverride.pdelement.temperature = temperatureMin + world.heatOffset[num] * temperatureRange;
						}
						if (!elementOverride.overrideMass)
						{
							elementOverride.pdelement.mass = this.GetDensityMassForCell(world, num, elementOverride.mass);
						}
						SetValues(num, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
					}
				}
				return;
			case ProcGen.Room.Selection.HorizontalSlice:
			{
				int num2 = int.MaxValue;
				int num3 = int.MinValue;
				for (int j = 0; j < cells.Count; j++)
				{
					num2 = Mathf.Min(cells[j].y, num2);
					num3 = Mathf.Max(cells[j].y, num3);
				}
				int num4 = num3 - num2;
				for (int k = 0; k < cells.Count; k++)
				{
					int num5 = Grid.XYToCell(cells[k].x, cells[k].y);
					if (Grid.IsValidCell(num5) && !highPriorityClaims.Contains(num5) && this.IsFeaturePointContainedInBorder(cells[k], worldGen))
					{
						float percentage = 1f - (float)(cells[k].y - num2) / (float)num4;
						WeightedSimHash weightedSimHashAtChoice = feature.GetWeightedSimHashAtChoice(group, percentage);
						TerrainCell.ElementOverride elementOverride2 = TerrainCell.GetElementOverride(weightedSimHashAtChoice.element, weightedSimHashAtChoice.overrides);
						if (!elementOverride2.overrideTemperature)
						{
							elementOverride2.pdelement.temperature = temperatureMin + world.heatOffset[num5] * temperatureRange;
						}
						if (!elementOverride2.overrideMass)
						{
							elementOverride2.pdelement.mass = this.GetDensityMassForCell(world, num5, elementOverride2.mass);
						}
						SetValues(num5, elementOverride2.element, elementOverride2.pdelement, elementOverride2.dc);
					}
				}
				return;
			}
			}
			WeightedSimHash oneWeightedSimHash2 = feature.GetOneWeightedSimHash(group, rnd);
			for (int l = 0; l < cells.Count; l++)
			{
				int num6 = Grid.XYToCell(cells[l].x, cells[l].y);
				if (Grid.IsValidCell(num6) && !highPriorityClaims.Contains(num6) && this.IsFeaturePointContainedInBorder(cells[l], worldGen))
				{
					TerrainCell.ElementOverride elementOverride3 = TerrainCell.GetElementOverride(oneWeightedSimHash2.element, oneWeightedSimHash2.overrides);
					if (!elementOverride3.overrideTemperature)
					{
						elementOverride3.pdelement.temperature = temperatureMin + world.heatOffset[num6] * temperatureRange;
					}
					if (!elementOverride3.overrideMass)
					{
						elementOverride3.pdelement.mass = this.GetDensityMassForCell(world, num6, elementOverride3.mass);
					}
					SetValues(num6, elementOverride3.element, elementOverride3.pdelement, elementOverride3.dc);
				}
			}
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x0026EC14 File Offset: 0x0026CE14
		private int GetIndexForLocation(List<Vector2I> points, Mob.Location location, SeededRandom rnd)
		{
			int num = -1;
			if (points == null || points.Count == 0)
			{
				return num;
			}
			if (location == Mob.Location.Air || location == Mob.Location.Solid)
			{
				return rnd.RandomRange(0, points.Count);
			}
			for (int i = 0; i < points.Count; i++)
			{
				if (Grid.IsValidCell(Grid.XYToCell(points[i].x, points[i].y)))
				{
					if (num == -1)
					{
						num = i;
					}
					else if (location != Mob.Location.Floor)
					{
						if (location == Mob.Location.Ceiling && points[i].y > points[num].y)
						{
							num = i;
						}
					}
					else if (points[i].y < points[num].y)
					{
						num = i;
					}
				}
			}
			return num;
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x0026ECC8 File Offset: 0x0026CEC8
		private void PlaceMobsInRoom(WorldGenSettings settings, List<MobReference> mobTags, List<Vector2I> points, SeededRandom rnd)
		{
			if (points == null)
			{
				return;
			}
			if (this.mobs == null)
			{
				this.mobs = new List<KeyValuePair<int, Tag>>();
			}
			for (int i = 0; i < mobTags.Count; i++)
			{
				if (!settings.HasMob(mobTags[i].type))
				{
					global::Debug.LogError("Missing sample description for tag [" + mobTags[i].type + "]");
				}
				else
				{
					Mob mob = settings.GetMob(mobTags[i].type);
					int num = Mathf.RoundToInt(mobTags[i].count.GetRandomValueWithinRange(rnd));
					for (int j = 0; j < num; j++)
					{
						int indexForLocation = this.GetIndexForLocation(points, mob.location, rnd);
						if (indexForLocation == -1)
						{
							break;
						}
						if (points.Count <= indexForLocation)
						{
							return;
						}
						int cellIdx = Grid.XYToCell(points[indexForLocation].x, points[indexForLocation].y);
						points.RemoveAt(indexForLocation);
						this.AddMob(cellIdx, mobTags[i].type);
					}
				}
			}
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x0026EDDC File Offset: 0x0026CFDC
		private int[] ConvertNoiseToPoints(float[] basenoise, float minThreshold = 0.9f, float maxThreshold = 1f)
		{
			if (basenoise == null)
			{
				return null;
			}
			List<int> list = new List<int>();
			float width = this.site.poly.bounds.width;
			float height = this.site.poly.bounds.height;
			for (float num = this.site.position.y - height / 2f; num < this.site.position.y + height / 2f; num += 1f)
			{
				for (float num2 = this.site.position.x - width / 2f; num2 < this.site.position.x + width / 2f; num2 += 1f)
				{
					int num3 = Grid.PosToCell(new Vector2(num2, num));
					if (this.site.poly.Contains(new Vector2(num2, num)))
					{
						float num4 = (float)((int)basenoise[num3]);
						if (num4 >= minThreshold && num4 <= maxThreshold && !list.Contains(num3))
						{
							list.Add(Grid.PosToCell(new Vector2(num2, num)));
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x0026EF14 File Offset: 0x0026D114
		private void ApplyForeground(WorldGen worldGen, Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd)
		{
			this.LogInfo("Apply foregreound", (this.node.tags != null).ToString(), (float)((this.node.tags != null) ? this.node.tags.Count : 0));
			if (this.node.tags != null)
			{
				FeatureSettings featureSettings = worldGen.Settings.TryGetFeature(this.node.type);
				this.LogInfo("\tFeature?", (featureSettings != null).ToString(), 0f);
				if (featureSettings == null && this.node.tags != null)
				{
					List<Tag> list = new List<Tag>();
					foreach (Tag item in this.node.tags)
					{
						if (worldGen.Settings.HasFeature(item.Name))
						{
							list.Add(item);
						}
					}
					this.LogInfo("\tNo feature, checking possible feature tags, found", "", (float)list.Count);
					if (list.Count > 0)
					{
						Tag tag = list[rnd.RandomSource().Next(list.Count)];
						featureSettings = worldGen.Settings.GetFeature(tag.Name);
						this.LogInfo("\tPicked feature", tag.Name, 0f);
					}
				}
				if (featureSettings != null)
				{
					this.LogInfo("APPLY FOREGROUND", this.node.type, 0f);
					float num = featureSettings.blobSize.GetRandomValueWithinRange(rnd);
					float num2 = this.poly.DistanceToClosestEdge(null);
					if (!this.node.tags.Contains(WorldGenTags.AllowExceedNodeBorders) && num2 < num)
					{
						if (this.debugMode)
						{
							global::Debug.LogWarning(string.Concat(new string[]
							{
								this.node.type,
								" ",
								featureSettings.shape.ToString(),
								"  blob size too large to fit in node. Size reduced. ",
								num.ToString(),
								"->",
								(num2 - 6f).ToString()
							}));
						}
						num = num2 - 6f;
					}
					if (num <= 0f)
					{
						return;
					}
					List<Vector2I> cells;
					List<List<Vector2I>> list2;
					HashSet<Vector2I> hashSet = this.DigFeature(featureSettings.shape, num, featureSettings.borders, rnd, out cells, out list2);
					this.featureSpawnPoints = new HashSet<int>();
					foreach (Vector2I vector2I in hashSet)
					{
						this.featureSpawnPoints.Add(Grid.XYToCell(vector2I.x, vector2I.y));
					}
					this.LogInfo("\t\t", "claimed points", (float)this.featureSpawnPoints.Count);
					this.availableTerrainPoints.ExceptWith(this.featureSpawnPoints);
					this.ApplyPlaceElementForRoom(featureSettings, "RoomCenterElements", cells, worldGen, world, SetValues, temperatureMin, temperatureRange, rnd, worldGen.HighPriorityClaimedCells);
					if (list2 != null)
					{
						for (int i = 0; i < list2.Count; i++)
						{
							this.ApplyPlaceElementForRoom(featureSettings, "RoomBorderChoices" + i.ToString(), list2[i], worldGen, world, SetValues, temperatureMin, temperatureRange, rnd, worldGen.HighPriorityClaimedCells);
						}
					}
					if (featureSettings.tags.Contains(WorldGenTags.HighPriorityFeature.Name))
					{
						worldGen.AddHighPriorityCells(this.featureSpawnPoints);
					}
				}
			}
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x0026F2AC File Offset: 0x0026D4AC
		private void ApplyBackground(WorldGen worldGen, Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd)
		{
			this.LogInfo("Apply Background", this.node.type, 0f);
			float floatSetting = worldGen.Settings.GetFloatSetting("CaveOverrideMaxValue");
			float floatSetting2 = worldGen.Settings.GetFloatSetting("CaveOverrideSliverValue");
			Leaf leafForTerrainCell = worldGen.GetLeafForTerrainCell(this);
			bool flag = leafForTerrainCell.tags.Contains(WorldGenTags.IgnoreCaveOverride);
			bool flag2 = leafForTerrainCell.tags.Contains(WorldGenTags.CaveVoidSliver);
			bool flag3 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroid);
			bool flag4 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroidInv);
			bool flag5 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdge);
			bool flag6 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdgeInv);
			bool flag7 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorder);
			bool flag8 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderWeak);
			bool flag9 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderInv);
			bool flag10 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTop);
			bool flag11 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTopOrSide);
			bool flag12 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointCentroid);
			bool flag13 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointEdge);
			this.LogInfo("Getting Element Bands", this.node.type, 0f);
			ElementBandConfiguration elementBandConfiguration = worldGen.Settings.GetElementBandForBiome(this.node.type);
			if (elementBandConfiguration == null && this.node.biomeSpecificTags != null)
			{
				this.LogInfo("\tType is not a biome, checking tags", "", (float)this.node.tags.Count);
				List<ElementBandConfiguration> list = new List<ElementBandConfiguration>();
				foreach (Tag tag in this.node.biomeSpecificTags)
				{
					ElementBandConfiguration elementBandForBiome = worldGen.Settings.GetElementBandForBiome(tag.Name);
					if (elementBandForBiome != null)
					{
						list.Add(elementBandForBiome);
						this.LogInfo("\tFound biome", tag.Name, 0f);
					}
				}
				if (list.Count > 0)
				{
					int num = rnd.RandomSource().Next(list.Count);
					elementBandConfiguration = list[num];
					this.LogInfo("\tPicked biome", "", (float)num);
				}
			}
			DebugUtil.Assert(elementBandConfiguration != null, "A node didn't get assigned a biome! ", this.node.type);
			foreach (int num2 in this.availableTerrainPoints)
			{
				Vector2I vector2I = Grid.CellToXY(num2);
				if (!worldGen.HighPriorityClaimedCells.Contains(num2))
				{
					float num3 = world.overrides[num2];
					if (!flag && num3 >= 100f)
					{
						if (num3 >= 300f)
						{
							SetValues(num2, WorldGen.voidElement, WorldGen.voidElement.defaultValues, Sim.DiseaseCell.Invalid);
						}
						else if (num3 >= 200f)
						{
							SetValues(num2, WorldGen.unobtaniumElement, WorldGen.unobtaniumElement.defaultValues, Sim.DiseaseCell.Invalid);
						}
						else
						{
							SetValues(num2, WorldGen.katairiteElement, WorldGen.katairiteElement.defaultValues, Sim.DiseaseCell.Invalid);
						}
					}
					else
					{
						float num4 = 1f;
						Vector2 vector = new Vector2((float)vector2I.x, (float)vector2I.y);
						if (flag3 || flag4)
						{
							float num5 = 15f;
							if (flag13)
							{
								float d = 0f;
								MathUtil.Pair<Vector2, Vector2> closestEdge = this.poly.GetClosestEdge(vector, ref d);
								num5 = Vector2.Distance(closestEdge.First + (closestEdge.Second - closestEdge.First) * d, vector);
							}
							num4 = Vector2.Distance(this.poly.Centroid(), vector) / num5;
							num4 = Mathf.Max(0f, Mathf.Min(1f, num4));
							if (flag4)
							{
								num4 = 1f - num4;
							}
						}
						if (flag6 || flag5)
						{
							float d2 = 0f;
							MathUtil.Pair<Vector2, Vector2> closestEdge2 = this.poly.GetClosestEdge(vector, ref d2);
							Vector2 a = closestEdge2.First + (closestEdge2.Second - closestEdge2.First) * d2;
							float num6 = 15f;
							if (flag12)
							{
								num6 = Vector2.Distance(this.poly.Centroid(), vector);
							}
							num4 = Vector2.Distance(a, vector) / num6;
							num4 = Mathf.Max(0f, Mathf.Min(1f, num4));
							if (flag6)
							{
								num4 = 1f - num4;
							}
						}
						if (flag9 || flag7)
						{
							List<Edge> edgesWithTag = worldGen.WorldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
							float num7 = float.MaxValue;
							foreach (Edge edge in edgesWithTag)
							{
								MathUtil.Pair<Vector2, Vector2> segment = new MathUtil.Pair<Vector2, Vector2>(edge.corner0.position, edge.corner1.position);
								float num8 = 0f;
								num7 = Mathf.Min(Mathf.Abs(MathUtil.GetClosestPointBetweenPointAndLineSegment(segment, vector, ref num8)), num7);
							}
							float num9 = flag8 ? 7f : 20f;
							if (flag12)
							{
								num9 = Vector2.Distance(this.poly.Centroid(), vector);
							}
							num4 = num7 / num9;
							num4 = Mathf.Max(0f, Mathf.Min(1f, num4));
							if (flag9)
							{
								num4 = 1f - num4;
							}
						}
						if (flag10)
						{
							float y = (float)worldGen.WorldSize.y;
							float num10 = 38f;
							float num11 = 58f;
							float num12 = y - vector.y;
							if (num12 < num10)
							{
								num4 = 0f;
							}
							else if (num12 < num11)
							{
								num4 = Mathf.Clamp01((num12 - num10) / (num11 - num10));
							}
							else
							{
								num4 = 1f;
							}
						}
						if (flag11)
						{
							float y2 = (float)worldGen.WorldSize.y;
							int x = worldGen.WorldSize.x;
							float num13 = 2f;
							float num14 = 10f;
							float num15 = y2 - vector.y;
							float x2 = vector.x;
							float num16 = (float)x - vector.x;
							float num17 = Mathf.Min(new float[]
							{
								num15,
								x2,
								num16
							});
							if (num17 < num13)
							{
								num4 = 0f;
							}
							else if (num17 < num14)
							{
								num4 = Mathf.Clamp01((num17 - num13) / (num14 - num13));
							}
							else
							{
								num4 = 1f;
							}
						}
						Element element;
						Sim.PhysicsData defaultValues;
						Sim.DiseaseCell dc;
						worldGen.GetElementForBiomePoint(world, elementBandConfiguration, vector2I, out element, out defaultValues, out dc, num4);
						defaultValues.mass += defaultValues.mass * 0.2f * (world.density[vector2I.x + world.size.x * vector2I.y] - 0.5f);
						if (!element.IsVacuum && element.id != SimHashes.Katairite && element.id != SimHashes.Unobtanium)
						{
							float num18 = temperatureMin;
							if (element.lowTempTransition != null && temperatureMin < element.lowTemp)
							{
								num18 = element.lowTemp;
							}
							defaultValues.temperature = num18 + world.heatOffset[num2] * temperatureRange;
						}
						if (element.IsSolid && !flag && num3 > floatSetting && num3 < 100f)
						{
							if (flag2 && num3 > floatSetting2)
							{
								element = WorldGen.voidElement;
							}
							else
							{
								element = WorldGen.vacuumElement;
							}
							defaultValues = element.defaultValues;
						}
						SetValues(num2, element, defaultValues, dc);
					}
				}
			}
			if (this.node.tags.Contains(WorldGenTags.SprinkleOfOxyRock))
			{
				this.HandleSprinkleOfElement(worldGen.Settings, WorldGenTags.SprinkleOfOxyRock, world, SetValues, temperatureMin, temperatureRange, rnd);
			}
			if (this.node.tags.Contains(WorldGenTags.SprinkleOfMetal))
			{
				this.HandleSprinkleOfElement(worldGen.Settings, WorldGenTags.SprinkleOfMetal, world, SetValues, temperatureMin, temperatureRange, rnd);
			}
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x0026FAC8 File Offset: 0x0026DCC8
		private void GenerateActionCells(WorldGenSettings settings, Tag tag, HashSet<int> possiblePoints, SeededRandom rnd)
		{
			ProcGen.Room room = null;
			SettingsCache.rooms.TryGetValue(tag.Name, out room);
			SampleDescriber sampleDescriber = room;
			if (sampleDescriber == null && settings.HasMob(tag.Name))
			{
				sampleDescriber = settings.GetMob(tag.Name);
			}
			if (sampleDescriber == null)
			{
				return;
			}
			HashSet<int> hashSet = new HashSet<int>();
			float randomValueWithinRange = sampleDescriber.density.GetRandomValueWithinRange(rnd);
			SampleDescriber.PointSelectionMethod selectMethod = sampleDescriber.selectMethod;
			List<Vector2> list;
			if (selectMethod != SampleDescriber.PointSelectionMethod.RandomPoints)
			{
				if (selectMethod != SampleDescriber.PointSelectionMethod.Centroid)
				{
				}
				list = new List<Vector2>();
				list.Add(this.node.position);
			}
			else
			{
				list = PointGenerator.GetRandomPoints(this.poly, randomValueWithinRange, 0f, null, sampleDescriber.sampleBehaviour, true, rnd, true, true);
			}
			foreach (Vector2 vector in list)
			{
				int item = Grid.XYToCell((int)vector.x, (int)vector.y);
				if (possiblePoints.Contains(item))
				{
					hashSet.Add(item);
				}
			}
			if (room != null && room.mobselection == ProcGen.Room.Selection.None)
			{
				if (this.terrainPositions == null)
				{
					this.terrainPositions = new List<KeyValuePair<int, Tag>>();
				}
				foreach (int num in hashSet)
				{
					if (Grid.IsValidCell(num))
					{
						this.terrainPositions.Add(new KeyValuePair<int, Tag>(num, tag));
					}
				}
			}
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x0026FC50 File Offset: 0x0026DE50
		private void DoProcess(WorldGen worldGen, Chunk world, TerrainCell.SetValuesFunction SetValues, SeededRandom rnd)
		{
			float temperatureMin = 265f;
			float temperatureRange = 30f;
			this.InitializeCells(worldGen.ClaimedCells);
			this.GetTemperatureRange(worldGen, ref temperatureMin, ref temperatureRange);
			this.ApplyForeground(worldGen, world, SetValues, temperatureMin, temperatureRange, rnd);
			for (int i = 0; i < this.node.tags.Count; i++)
			{
				this.GenerateActionCells(worldGen.Settings, this.node.tags[i], this.availableTerrainPoints, rnd);
			}
			this.ApplyBackground(worldGen, world, SetValues, temperatureMin, temperatureRange, rnd);
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x0026FCDC File Offset: 0x0026DEDC
		public void Process(WorldGen worldGen, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dcs, Chunk world, SeededRandom rnd)
		{
			TerrainCell.SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
			{
				if (Grid.IsValidCell(index))
				{
					if (pd.temperature == 0f || (elem as Element).HasTag(GameTags.Special))
					{
						bgTemp[index] = -1f;
					}
					cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
					dcs[index] = dc;
					return;
				}
				global::Debug.LogError(string.Concat(new string[]
				{
					"Process::SetValuesFunction Index [",
					index.ToString(),
					"] is not valid. cells.Length [",
					cells.Length.ToString(),
					"]"
				}));
			};
			this.DoProcess(worldGen, world, setValues, rnd);
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x0026FD1C File Offset: 0x0026DF1C
		public void Process(WorldGen worldGen, Chunk world, SeededRandom rnd)
		{
			TerrainCell.SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
			{
				SimMessages.ModifyCell(index, (elem as Element).idx, pd.temperature, pd.mass, dc.diseaseIdx, dc.elementCount, SimMessages.ReplaceType.Replace, false, -1);
			};
			this.DoProcess(worldGen, world, setValues, rnd);
		}

		// Token: 0x06006795 RID: 26517 RVA: 0x0026FD54 File Offset: 0x0026DF54
		public int DistanceToTag(Tag tag)
		{
			int result;
			if (!this.distancesToTags.TryGetValue(tag, out result))
			{
				DebugUtil.DevLogError(string.Format("DistanceToTag could not find tag '{0}', did forget to include a start template?", tag));
			}
			return result;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x0026FD87 File Offset: 0x0026DF87
		public bool IsSafeToSpawnPOI(List<TerrainCell> allCells, bool log = true)
		{
			return this.IsSafeToSpawnPOI(allCells, TerrainCell.noPOISpawnTags, TerrainCell.noPOISpawnTagSet, log);
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x0026FD9B File Offset: 0x0026DF9B
		public bool IsSafeToSpawnPOIRelaxed(List<TerrainCell> allCells, bool log = true)
		{
			return this.IsSafeToSpawnPOI(allCells, TerrainCell.relaxedNoPOISpawnTags, TerrainCell.relaxedNoPOISpawnTagSet, log);
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x0026FDAF File Offset: 0x0026DFAF
		private bool IsSafeToSpawnPOI(List<TerrainCell> allCells, Tag[] noSpawnTags, TagSet noSpawnTagSet, bool log)
		{
			return !this.node.tags.ContainsOne(noSpawnTagSet);
		}

		// Token: 0x04004777 RID: 18295
		private const float MASS_VARIATION = 0.2f;

		// Token: 0x0400477C RID: 18300
		public List<KeyValuePair<int, Tag>> terrainPositions;

		// Token: 0x0400477D RID: 18301
		public List<KeyValuePair<int, Tag>> poi;

		// Token: 0x0400477E RID: 18302
		public List<int> neighbourTerrainCells = new List<int>();

		// Token: 0x0400477F RID: 18303
		private float finalSize;

		// Token: 0x04004780 RID: 18304
		private bool debugMode;

		// Token: 0x04004781 RID: 18305
		private List<int> allCells;

		// Token: 0x04004782 RID: 18306
		private HashSet<int> availableTerrainPoints;

		// Token: 0x04004783 RID: 18307
		private HashSet<int> featureSpawnPoints;

		// Token: 0x04004784 RID: 18308
		private HashSet<int> availableSpawnPoints;

		// Token: 0x04004785 RID: 18309
		public const int DONT_SET_TEMPERATURE_DEFAULTS = -1;

		// Token: 0x04004786 RID: 18310
		private static readonly Tag[] noPOISpawnTags = new Tag[]
		{
			WorldGenTags.StartLocation,
			WorldGenTags.AtStart,
			WorldGenTags.NearStartLocation,
			WorldGenTags.POI,
			WorldGenTags.Feature
		};

		// Token: 0x04004787 RID: 18311
		private static readonly TagSet noPOISpawnTagSet = new TagSet(TerrainCell.noPOISpawnTags);

		// Token: 0x04004788 RID: 18312
		private static readonly Tag[] relaxedNoPOISpawnTags = new Tag[]
		{
			WorldGenTags.StartLocation,
			WorldGenTags.AtStart,
			WorldGenTags.NearStartLocation,
			WorldGenTags.POI
		};

		// Token: 0x04004789 RID: 18313
		private static readonly TagSet relaxedNoPOISpawnTagSet = new TagSet(TerrainCell.relaxedNoPOISpawnTags);

		// Token: 0x02001BEC RID: 7148
		// (Invoke) Token: 0x06009B42 RID: 39746
		public delegate void SetValuesFunction(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc);

		// Token: 0x02001BED RID: 7149
		public struct ElementOverride
		{
			// Token: 0x04007E4B RID: 32331
			public Element element;

			// Token: 0x04007E4C RID: 32332
			public Sim.PhysicsData pdelement;

			// Token: 0x04007E4D RID: 32333
			public Sim.DiseaseCell dc;

			// Token: 0x04007E4E RID: 32334
			public float mass;

			// Token: 0x04007E4F RID: 32335
			public float temperature;

			// Token: 0x04007E50 RID: 32336
			public byte diseaseIdx;

			// Token: 0x04007E51 RID: 32337
			public int diseaseAmount;

			// Token: 0x04007E52 RID: 32338
			public bool overrideMass;

			// Token: 0x04007E53 RID: 32339
			public bool overrideTemperature;

			// Token: 0x04007E54 RID: 32340
			public bool overrideDiseaseIdx;

			// Token: 0x04007E55 RID: 32341
			public bool overrideDiseaseAmount;
		}
	}
}
