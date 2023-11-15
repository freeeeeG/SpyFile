﻿using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace ProcGenGame
{
	// Token: 0x02000CAA RID: 3242
	public class River : River, SymbolicMapElement
	{
		// Token: 0x06006752 RID: 26450 RVA: 0x0026C1AB File Offset: 0x0026A3AB
		public River(River other) : base(other, true)
		{
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x0026C1B8 File Offset: 0x0026A3B8
		public void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd)
		{
			Element element = ElementLoader.FindElementByName(base.backgroundElement);
			Sim.PhysicsData defaultValues = element.defaultValues;
			Element element2 = ElementLoader.FindElementByName(base.element);
			Sim.PhysicsData defaultValues2 = element2.defaultValues;
			defaultValues2.temperature = base.temperature;
			Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
			for (int i = 0; i < this.pathElements.Count; i++)
			{
				Segment segment = this.pathElements[i];
				Vector2 vector = segment.e1 - segment.e0;
				Vector2 normalized = new Vector2(-vector.y, vector.x).normalized;
				List<Vector2I> line = ProcGen.Util.GetLine(segment.e0, segment.e1);
				for (int j = 0; j < line.Count; j++)
				{
					for (float num = 0.5f; num <= base.widthCenter; num += 1f)
					{
						Vector2 vector2 = line[j] + normalized * num;
						int num2 = Grid.XYToCell((int)vector2.x, (int)vector2.y);
						if (Grid.IsValidCell(num2))
						{
							SetValues(num2, element2, defaultValues2, invalid);
						}
						Vector2 vector3 = line[j] - normalized * num;
						num2 = Grid.XYToCell((int)vector3.x, (int)vector3.y);
						if (Grid.IsValidCell(num2))
						{
							SetValues(num2, element2, defaultValues2, invalid);
						}
					}
					for (float num3 = 0.5f; num3 <= base.widthBorder; num3 += 1f)
					{
						Vector2 vector4 = line[j] + normalized * (base.widthCenter + num3);
						int num4 = Grid.XYToCell((int)vector4.x, (int)vector4.y);
						if (Grid.IsValidCell(num4))
						{
							defaultValues.temperature = temperatureMin + world.heatOffset[num4] * temperatureRange;
							SetValues(num4, element, defaultValues, invalid);
						}
						Vector2 vector5 = line[j] - normalized * (base.widthCenter + num3);
						num4 = Grid.XYToCell((int)vector5.x, (int)vector5.y);
						if (Grid.IsValidCell(num4))
						{
							defaultValues.temperature = temperatureMin + world.heatOffset[num4] * temperatureRange;
							SetValues(num4, element, defaultValues, invalid);
						}
					}
				}
			}
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x0026C43C File Offset: 0x0026A63C
		public static void ProcessRivers(Chunk world, List<River> rivers, Sim.Cell[] cells, Sim.DiseaseCell[] dcs)
		{
			TerrainCell.SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
			{
				if (Grid.IsValidCell(index))
				{
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
			float temperatureMin = 265f;
			float temperatureRange = 30f;
			for (int i = 0; i < rivers.Count; i++)
			{
				rivers[i].ConvertToMap(world, setValues, temperatureMin, temperatureRange, null);
			}
		}

		// Token: 0x06006755 RID: 26453 RVA: 0x0026C498 File Offset: 0x0026A698
		public static River GetRiverForCell(List<River> rivers, int cell)
		{
			return new River(rivers.Find((River river) => Grid.PosToCell(river.SourcePosition()) == cell || Grid.PosToCell(river.SinkPosition()) == cell));
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x0026C4CC File Offset: 0x0026A6CC
		private static void GetRiverLocation(List<River> rivers, ref GameSpawnData gsd)
		{
			for (int i = 0; i < rivers.Count; i++)
			{
				Vector2 vector = rivers[i].SourcePosition();
				Vector2 vector2 = rivers[i].SinkPosition();
				if (vector.y < vector2.y)
				{
				}
			}
		}
	}
}
