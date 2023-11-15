using System;

// Token: 0x02000A1B RID: 2587
public static class UtilityConnectionsExtensions
{
	// Token: 0x06004D63 RID: 19811 RVA: 0x001B2144 File Offset: 0x001B0344
	public static UtilityConnections InverseDirection(this UtilityConnections direction)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return UtilityConnections.Right;
		case UtilityConnections.Right:
			return UtilityConnections.Left;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return UtilityConnections.Down;
		default:
			if (direction == UtilityConnections.Down)
			{
				return UtilityConnections.Up;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x001B2198 File Offset: 0x001B0398
	public static UtilityConnections LeftDirection(this UtilityConnections direction)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return UtilityConnections.Down;
		case UtilityConnections.Right:
			return UtilityConnections.Up;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return UtilityConnections.Left;
		default:
			if (direction == UtilityConnections.Down)
			{
				return UtilityConnections.Right;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001B21EC File Offset: 0x001B03EC
	public static UtilityConnections RightDirection(this UtilityConnections direction)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return UtilityConnections.Up;
		case UtilityConnections.Right:
			return UtilityConnections.Down;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return UtilityConnections.Right;
		default:
			if (direction == UtilityConnections.Down)
			{
				return UtilityConnections.Left;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001B2240 File Offset: 0x001B0440
	public static int CellInDirection(this UtilityConnections direction, int from_cell)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return from_cell - 1;
		case UtilityConnections.Right:
			return from_cell + 1;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return from_cell + Grid.WidthInCells;
		default:
			if (direction == UtilityConnections.Down)
			{
				return from_cell - Grid.WidthInCells;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x001B22A4 File Offset: 0x001B04A4
	public static UtilityConnections DirectionFromToCell(int from_cell, int to_cell)
	{
		if (to_cell == from_cell - 1)
		{
			return UtilityConnections.Left;
		}
		if (to_cell == from_cell + 1)
		{
			return UtilityConnections.Right;
		}
		if (to_cell == from_cell + Grid.WidthInCells)
		{
			return UtilityConnections.Up;
		}
		if (to_cell == from_cell - Grid.WidthInCells)
		{
			return UtilityConnections.Down;
		}
		return (UtilityConnections)0;
	}
}
