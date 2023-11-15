using System;
using System.Collections.Generic;
using ProcGen;
using TUNING;

// Token: 0x02000CA2 RID: 3234
public class BestFit
{
	// Token: 0x060066F9 RID: 26361 RVA: 0x00266BFC File Offset: 0x00264DFC
	public static Vector2I BestFitWorlds(List<WorldPlacement> worldsToArrange, bool ignoreBestFitY = false)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		Vector2I vector2I = default(Vector2I);
		List<WorldPlacement> list2 = new List<WorldPlacement>(worldsToArrange);
		list2.Sort((WorldPlacement a, WorldPlacement b) => b.height.CompareTo(a.height));
		int height = list2[0].height;
		foreach (WorldPlacement worldPlacement in list2)
		{
			Vector2I vector2I2 = default(Vector2I);
			while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height), list))
			{
				if (ignoreBestFitY)
				{
					vector2I2.x++;
				}
				else if (vector2I2.y + worldPlacement.height >= height + 32)
				{
					vector2I2.y = 0;
					vector2I2.x++;
				}
				else
				{
					vector2I2.y++;
				}
			}
			vector2I.x = Math.Max(worldPlacement.width + vector2I2.x, vector2I.x);
			vector2I.y = Math.Max(worldPlacement.height + vector2I2.y, vector2I.y);
			list.Add(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height));
			worldPlacement.SetPosition(vector2I2);
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			vector2I.x += 136;
			vector2I.y = Math.Max(vector2I.y, 136);
		}
		return vector2I;
	}

	// Token: 0x060066FA RID: 26362 RVA: 0x00266DB8 File Offset: 0x00264FB8
	private static bool UnoccupiedSpace(BestFit.Rect RectA, List<BestFit.Rect> placed)
	{
		foreach (BestFit.Rect rect in placed)
		{
			if (RectA.X1 < rect.X2 && RectA.X2 > rect.X1 && RectA.Y1 < rect.Y2 && RectA.Y2 > rect.Y1)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060066FB RID: 26363 RVA: 0x00266E48 File Offset: 0x00265048
	public static Vector2I GetGridOffset(IList<WorldContainer> existingWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I result = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I.x, vector2I.y, newWorldSize.x, newWorldSize.y), list))
		{
			if (vector2I.x + newWorldSize.x >= widthInCells)
			{
				vector2I.x = 0;
				vector2I.y++;
			}
			else
			{
				vector2I.x++;
			}
		}
		Debug.Assert(vector2I.x + newWorldSize.x <= Grid.WidthInCells, "BestFit is trying to expand the grid width, this is unsupported and will break the SIM.");
		result.y = Math.Max(newWorldSize.y + vector2I.y, Grid.HeightInCells);
		newWorldOffset = vector2I;
		return result;
	}

	// Token: 0x060066FC RID: 26364 RVA: 0x00266F8C File Offset: 0x0026518C
	public static int CountRocketInteriors(IList<WorldContainer> existingWorlds)
	{
		int num = 0;
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		while (BestFit.PlaceWorld(list, rocket_INTERIOR_SIZE, out vector2I))
		{
			num++;
			list.Add(new BestFit.Rect(vector2I.x, vector2I.y, rocket_INTERIOR_SIZE.x, rocket_INTERIOR_SIZE.y));
		}
		return num;
	}

	// Token: 0x060066FD RID: 26365 RVA: 0x00267054 File Offset: 0x00265254
	private static bool PlaceWorld(List<BestFit.Rect> placedWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		Vector2I vector2I = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I2 = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, newWorldSize.x, newWorldSize.y), placedWorlds))
		{
			if (vector2I2.x + newWorldSize.x >= widthInCells)
			{
				vector2I2.x = 0;
				vector2I2.y++;
			}
			else
			{
				vector2I2.x++;
			}
		}
		vector2I.y = Math.Max(newWorldSize.y + vector2I2.y, Grid.HeightInCells);
		newWorldOffset = vector2I2;
		return vector2I2.x + newWorldSize.x <= Grid.WidthInCells && vector2I2.y + newWorldSize.y <= Grid.HeightInCells;
	}

	// Token: 0x02001BCA RID: 7114
	private struct Rect
	{
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06009AFB RID: 39675 RVA: 0x00348087 File Offset: 0x00346287
		public int X1
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06009AFC RID: 39676 RVA: 0x0034808F File Offset: 0x0034628F
		public int X2
		{
			get
			{
				return this.x + this.width + 2;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06009AFD RID: 39677 RVA: 0x003480A0 File Offset: 0x003462A0
		public int Y1
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06009AFE RID: 39678 RVA: 0x003480A8 File Offset: 0x003462A8
		public int Y2
		{
			get
			{
				return this.y + this.height + 2;
			}
		}

		// Token: 0x06009AFF RID: 39679 RVA: 0x003480B9 File Offset: 0x003462B9
		public Rect(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x04007DF2 RID: 32242
		private int x;

		// Token: 0x04007DF3 RID: 32243
		private int y;

		// Token: 0x04007DF4 RID: 32244
		private int width;

		// Token: 0x04007DF5 RID: 32245
		private int height;
	}
}
