using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProcGen;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
public class Grid
{
	// Token: 0x06003935 RID: 14645 RVA: 0x0014008E File Offset: 0x0013E28E
	private static void UpdateBuildMask(int i, Grid.BuildFlags flag, bool state)
	{
		if (state)
		{
			Grid.BuildMasks[i] |= flag;
			return;
		}
		Grid.BuildMasks[i] &= ~flag;
	}

	// Token: 0x06003936 RID: 14646 RVA: 0x001400B6 File Offset: 0x0013E2B6
	public static void SetSolid(int cell, bool solid, CellSolidEvent ev)
	{
		Grid.UpdateBuildMask(cell, Grid.BuildFlags.Solid, solid);
	}

	// Token: 0x06003937 RID: 14647 RVA: 0x001400C2 File Offset: 0x0013E2C2
	private static void UpdateVisMask(int i, Grid.VisFlags flag, bool state)
	{
		if (state)
		{
			Grid.VisMasks[i] |= flag;
			return;
		}
		Grid.VisMasks[i] &= ~flag;
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x001400EA File Offset: 0x0013E2EA
	private static void UpdateNavValidatorMask(int i, Grid.NavValidatorFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavValidatorMasks[i] |= flag;
			return;
		}
		Grid.NavValidatorMasks[i] &= ~flag;
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x00140112 File Offset: 0x0013E312
	private static void UpdateNavMask(int i, Grid.NavFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavMasks[i] |= flag;
			return;
		}
		Grid.NavMasks[i] &= ~flag;
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x0014013A File Offset: 0x0013E33A
	public static void ResetNavMasksAndDetails()
	{
		Grid.NavMasks = null;
		Grid.tubeEntrances.Clear();
		Grid.restrictions.Clear();
		Grid.suitMarkers.Clear();
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x00140160 File Offset: 0x0013E360
	public static bool DEBUG_GetRestrictions(int cell, out Grid.Restriction restriction)
	{
		return Grid.restrictions.TryGetValue(cell, out restriction);
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x00140170 File Offset: 0x0013E370
	public static void RegisterRestriction(int cell, Grid.Restriction.Orientation orientation)
	{
		Grid.HasAccessDoor[cell] = true;
		Grid.restrictions[cell] = new Grid.Restriction
		{
			DirectionMasksForMinionInstanceID = new Dictionary<int, Grid.Restriction.Directions>(),
			orientation = orientation
		};
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x001401B1 File Offset: 0x0013E3B1
	public static void UnregisterRestriction(int cell)
	{
		Grid.restrictions.Remove(cell);
		Grid.HasAccessDoor[cell] = false;
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x001401CB File Offset: 0x0013E3CB
	public static void SetRestriction(int cell, int minionInstanceID, Grid.Restriction.Directions directions)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID[minionInstanceID] = directions;
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x001401E4 File Offset: 0x0013E3E4
	public static void ClearRestriction(int cell, int minionInstanceID)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID.Remove(minionInstanceID);
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x00140200 File Offset: 0x0013E400
	public static bool HasPermission(int cell, int minionInstanceID, int fromCell, NavType fromNavType)
	{
		if (!Grid.HasAccessDoor[cell])
		{
			return true;
		}
		Grid.Restriction restriction = Grid.restrictions[cell];
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = Grid.CellToXY(fromCell);
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		int num = vector2I.x - vector2I2.x;
		int num2 = vector2I.y - vector2I2.y;
		switch (restriction.orientation)
		{
		case Grid.Restriction.Orientation.Vertical:
			if (num < 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num > 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.Horizontal:
			if (num2 > 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num2 < 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.SingleCell:
			if (Math.Abs(num) != 1 && Math.Abs(num2) != 1 && fromNavType != NavType.Teleport)
			{
				directions |= Grid.Restriction.Directions.Teleport;
			}
			break;
		}
		Grid.Restriction.Directions directions2 = (Grid.Restriction.Directions)0;
		return (!restriction.DirectionMasksForMinionInstanceID.TryGetValue(minionInstanceID, out directions2) && !restriction.DirectionMasksForMinionInstanceID.TryGetValue(-1, out directions2)) || (directions2 & directions) == (Grid.Restriction.Directions)0;
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x001402E0 File Offset: 0x0013E4E0
	public static void RegisterTubeEntrance(int cell, int reservationCapacity)
	{
		DebugUtil.Assert(!Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = true;
		Grid.tubeEntrances[cell] = new Grid.TubeEntrance
		{
			reservationCapacity = reservationCapacity,
			reservedInstanceIDs = new HashSet<int>()
		};
	}

	// Token: 0x06003942 RID: 14658 RVA: 0x00140334 File Offset: 0x0013E534
	public static void UnregisterTubeEntrance(int cell)
	{
		DebugUtil.Assert(Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = false;
		Grid.tubeEntrances.Remove(cell);
	}

	// Token: 0x06003943 RID: 14659 RVA: 0x00140360 File Offset: 0x0013E560
	public static bool ReserveTubeEntrance(int cell, int minionInstanceID, bool reserve)
	{
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		if (!reserve)
		{
			return reservedInstanceIDs.Remove(minionInstanceID);
		}
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		if (reservedInstanceIDs.Count == tubeEntrance.reservationCapacity)
		{
			return false;
		}
		DebugUtil.Assert(reservedInstanceIDs.Add(minionInstanceID));
		return true;
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x001403B8 File Offset: 0x0013E5B8
	public static void SetTubeEntranceReservationCapacity(int cell, int newReservationCapacity)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.reservationCapacity = newReservationCapacity;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x001403F8 File Offset: 0x0013E5F8
	public static bool HasUsableTubeEntrance(int cell, int minionInstanceID)
	{
		if (!Grid.HasTubeEntrance[cell])
		{
			return false;
		}
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		if (!tubeEntrance.operational)
		{
			return false;
		}
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		return reservedInstanceIDs.Count < tubeEntrance.reservationCapacity || reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x00140448 File Offset: 0x0013E648
	public static bool HasReservedTubeEntrance(int cell, int minionInstanceID)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		return Grid.tubeEntrances[cell].reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x00140470 File Offset: 0x0013E670
	public static void SetTubeEntranceOperational(int cell, bool operational)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.operational = operational;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x001404B0 File Offset: 0x0013E6B0
	public static void RegisterSuitMarker(int cell)
	{
		DebugUtil.Assert(!Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = true;
		Grid.suitMarkers[cell] = new Grid.SuitMarker
		{
			suitCount = 0,
			lockerCount = 0,
			flags = Grid.SuitMarker.Flags.Operational,
			minionIDsWithSuitReservations = new HashSet<int>(),
			minionIDsWithEmptyLockerReservations = new HashSet<int>()
		};
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x00140520 File Offset: 0x0013E720
	public static void UnregisterSuitMarker(int cell)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = false;
		Grid.suitMarkers.Remove(cell);
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x0014054C File Offset: 0x0013E74C
	public static bool ReserveSuit(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithSuitReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag);
			return flag;
		}
		if (minionIDsWithSuitReservations.Count >= suitMarker.suitCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithSuitReservations.Add(minionInstanceID));
		return true;
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x001405AC File Offset: 0x0013E7AC
	public static bool ReserveEmptyLocker(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithEmptyLockerReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag);
			return flag;
		}
		if (minionIDsWithEmptyLockerReservations.Count >= suitMarker.emptyLockerCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithEmptyLockerReservations.Add(minionInstanceID));
		return true;
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x0014060C File Offset: 0x0013E80C
	public static void UpdateSuitMarker(int cell, int fullLockerCount, int emptyLockerCount, Grid.SuitMarker.Flags flags, PathFinder.PotentialPath.Flags pathFlags)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker value = Grid.suitMarkers[cell];
		value.suitCount = fullLockerCount;
		value.lockerCount = fullLockerCount + emptyLockerCount;
		value.flags = flags;
		value.pathFlags = pathFlags;
		Grid.suitMarkers[cell] = value;
	}

	// Token: 0x0600394D RID: 14669 RVA: 0x00140664 File Offset: 0x0013E864
	public static bool TryGetSuitMarkerFlags(int cell, out Grid.SuitMarker.Flags flags, out PathFinder.PotentialPath.Flags pathFlags)
	{
		if (Grid.HasSuitMarker[cell])
		{
			flags = Grid.suitMarkers[cell].flags;
			pathFlags = Grid.suitMarkers[cell].pathFlags;
			return true;
		}
		flags = (Grid.SuitMarker.Flags)0;
		pathFlags = PathFinder.PotentialPath.Flags.None;
		return false;
	}

	// Token: 0x0600394E RID: 14670 RVA: 0x001406A0 File Offset: 0x0013E8A0
	public static bool HasSuit(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		return minionIDsWithSuitReservations.Count < suitMarker.suitCount || minionIDsWithSuitReservations.Contains(minionInstanceID);
	}

	// Token: 0x0600394F RID: 14671 RVA: 0x001406E8 File Offset: 0x0013E8E8
	public static bool HasEmptyLocker(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		return minionIDsWithEmptyLockerReservations.Count < suitMarker.emptyLockerCount || minionIDsWithEmptyLockerReservations.Contains(minionInstanceID);
	}

	// Token: 0x06003950 RID: 14672 RVA: 0x00140730 File Offset: 0x0013E930
	public unsafe static void InitializeCells()
	{
		for (int num = 0; num != Grid.WidthInCells * Grid.HeightInCells; num++)
		{
			ushort index = Grid.elementIdx[num];
			Element element = ElementLoader.elements[(int)index];
			Grid.Element[num] = element;
			if (element.IsSolid)
			{
				Grid.BuildMasks[num] |= Grid.BuildFlags.Solid;
			}
			else
			{
				Grid.BuildMasks[num] &= ~Grid.BuildFlags.Solid;
			}
			Grid.RenderedByWorld[num] = (element.substance != null && element.substance.renderedByWorld && Grid.Objects[num, 9] == null);
		}
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x001407DD File Offset: 0x0013E9DD
	public static bool IsInitialized()
	{
		return Grid.mass != null;
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x001407EC File Offset: 0x0013E9EC
	public static int GetCellInDirection(int cell, Direction d)
	{
		switch (d)
		{
		case Direction.Up:
			return Grid.CellAbove(cell);
		case Direction.Right:
			return Grid.CellRight(cell);
		case Direction.Down:
			return Grid.CellBelow(cell);
		case Direction.Left:
			return Grid.CellLeft(cell);
		case Direction.None:
			return cell;
		}
		return -1;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x00140838 File Offset: 0x0013EA38
	public static bool Raycast(int cell, Vector2I direction, out int hitDistance, int maxDistance = 100, Grid.BuildFlags layerMask = Grid.BuildFlags.Any)
	{
		bool flag = false;
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = vector2I + direction * maxDistance;
		int num = cell;
		int num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
		int num3 = 0;
		int num4 = 0;
		float num5 = (float)maxDistance * 0.5f;
		while ((float)num3 < num5)
		{
			if (!Grid.IsValidCell(num) || (Grid.BuildMasks[num] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				flag = true;
				break;
			}
			if (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				num4 = maxDistance - num3;
			}
			vector2I += direction;
			vector2I2 -= direction;
			num = Grid.XYToCell(vector2I.x, vector2I.y);
			num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
			num3++;
		}
		if (!flag && maxDistance % 2 == 0)
		{
			flag = (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		hitDistance = (flag ? num3 : ((num4 > 0) ? num4 : maxDistance));
		return flag | hitDistance == num4;
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x00140939 File Offset: 0x0013EB39
	public static int CellAbove(int cell)
	{
		return cell + Grid.WidthInCells;
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x00140942 File Offset: 0x0013EB42
	public static int CellBelow(int cell)
	{
		return cell - Grid.WidthInCells;
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x0014094B File Offset: 0x0013EB4B
	public static int CellLeft(int cell)
	{
		if (cell % Grid.WidthInCells <= 0)
		{
			return -1;
		}
		return cell - 1;
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x0014095C File Offset: 0x0013EB5C
	public static int CellRight(int cell)
	{
		if (cell % Grid.WidthInCells >= Grid.WidthInCells - 1)
		{
			return -1;
		}
		return cell + 1;
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x00140974 File Offset: 0x0013EB74
	public static CellOffset GetOffset(int cell)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return new CellOffset(x, y);
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x00140998 File Offset: 0x0013EB98
	public static int CellUpLeft(int cell)
	{
		int result = -1;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x001409CC File Offset: 0x0013EBCC
	public static int CellUpRight(int cell)
	{
		int result = -1;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x00140A08 File Offset: 0x0013EC08
	public static int CellDownLeft(int cell)
	{
		int result = -1;
		if (cell > Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x00140A34 File Offset: 0x0013EC34
	public static int CellDownRight(int cell)
	{
		int result = -1;
		if (cell >= Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x00140A66 File Offset: 0x0013EC66
	public static bool IsCellLeftOf(int cell, int other_cell)
	{
		return Grid.CellColumn(cell) < Grid.CellColumn(other_cell);
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x00140A78 File Offset: 0x0013EC78
	public static bool IsCellOffsetOf(int cell, int target_cell, CellOffset[] target_offsets)
	{
		int num = target_offsets.Length;
		for (int i = 0; i < num; i++)
		{
			if (cell == Grid.OffsetCell(target_cell, target_offsets[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x00140AA8 File Offset: 0x0013ECA8
	public static bool IsCellOffsetOf(int cell, GameObject target, CellOffset[] target_offsets)
	{
		int target_cell = Grid.PosToCell(target);
		return Grid.IsCellOffsetOf(cell, target_cell, target_offsets);
	}

	// Token: 0x06003960 RID: 14688 RVA: 0x00140AC4 File Offset: 0x0013ECC4
	public static int GetCellDistance(int cell_a, int cell_b)
	{
		CellOffset offset = Grid.GetOffset(cell_a, cell_b);
		return Math.Abs(offset.x) + Math.Abs(offset.y);
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x00140AF0 File Offset: 0x0013ECF0
	public static int GetCellRange(int cell_a, int cell_b)
	{
		CellOffset offset = Grid.GetOffset(cell_a, cell_b);
		return Math.Max(Math.Abs(offset.x), Math.Abs(offset.y));
	}

	// Token: 0x06003962 RID: 14690 RVA: 0x00140B20 File Offset: 0x0013ED20
	public static CellOffset GetOffset(int base_cell, int offset_cell)
	{
		int num;
		int num2;
		Grid.CellToXY(base_cell, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(offset_cell, out num3, out num4);
		return new CellOffset(num3 - num, num4 - num2);
	}

	// Token: 0x06003963 RID: 14691 RVA: 0x00140B4C File Offset: 0x0013ED4C
	public static CellOffset GetCellOffsetDirection(int base_cell, int offset_cell)
	{
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		offset.x = Mathf.Clamp(offset.x, -1, 1);
		offset.y = Mathf.Clamp(offset.y, -1, 1);
		return offset;
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x00140B8A File Offset: 0x0013ED8A
	public static int OffsetCell(int cell, CellOffset offset)
	{
		return cell + offset.x + offset.y * Grid.WidthInCells;
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x00140BA1 File Offset: 0x0013EDA1
	public static int OffsetCell(int cell, int x, int y)
	{
		return cell + x + y * Grid.WidthInCells;
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x00140BB0 File Offset: 0x0013EDB0
	public static bool IsCellOffsetValid(int cell, int x, int y)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return num + x >= 0 && num + x < Grid.WidthInCells && num2 + y >= 0 && num2 + y < Grid.HeightInCells;
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x00140BEB File Offset: 0x0013EDEB
	public static bool IsCellOffsetValid(int cell, CellOffset offset)
	{
		return Grid.IsCellOffsetValid(cell, offset.x, offset.y);
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x00140BFF File Offset: 0x0013EDFF
	public static int PosToCell(StateMachine.Instance smi)
	{
		return Grid.PosToCell(smi.transform.GetPosition());
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x00140C11 File Offset: 0x0013EE11
	public static int PosToCell(GameObject go)
	{
		return Grid.PosToCell(go.transform.GetPosition());
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x00140C23 File Offset: 0x0013EE23
	public static int PosToCell(KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x00140C38 File Offset: 0x0013EE38
	public static bool IsValidBuildingCell(int cell)
	{
		if (!Grid.IsWorldValidCell(cell))
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
		if (world == null)
		{
			return false;
		}
		Vector2I vector2I = Grid.CellToXY(cell);
		return (float)vector2I.x >= world.minimumBounds.x && (float)vector2I.x <= world.maximumBounds.x && (float)vector2I.y >= world.minimumBounds.y && (float)vector2I.y <= world.maximumBounds.y - (float)Grid.TopBorderHeight;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x00140CCF File Offset: 0x0013EECF
	public static bool IsWorldValidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue;
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x00140CEC File Offset: 0x0013EEEC
	public static bool IsValidCell(int cell)
	{
		return cell >= 0 && cell < Grid.CellCount;
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x00140CFC File Offset: 0x0013EEFC
	public static bool IsValidCellInWorld(int cell, int world)
	{
		return cell >= 0 && cell < Grid.CellCount && (int)Grid.WorldIdx[cell] == world;
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x00140D16 File Offset: 0x0013EF16
	public static bool IsActiveWorld(int cell)
	{
		return ClusterManager.Instance != null && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[cell];
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x00140D3A File Offset: 0x0013EF3A
	public static bool AreCellsInSameWorld(int cell, int world_cell)
	{
		return Grid.IsValidCell(cell) && Grid.IsValidCell(world_cell) && Grid.WorldIdx[cell] == Grid.WorldIdx[world_cell];
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x00140D5E File Offset: 0x0013EF5E
	public static bool IsCellOpenToSpace(int cell)
	{
		return !Grid.IsSolidCell(cell) && !(Grid.Objects[cell, 2] != null) && global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x00140D94 File Offset: 0x0013EF94
	public static int PosToCell(Vector2 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x00140DC0 File Offset: 0x0013EFC0
	public static int PosToCell(Vector3 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x00140DEC File Offset: 0x0013EFEC
	public static void PosToXY(Vector3 pos, out int x, out int y)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out x, out y);
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x00140DFB File Offset: 0x0013EFFB
	public static void PosToXY(Vector3 pos, out Vector2I xy)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out xy.x, out xy.y);
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x00140E14 File Offset: 0x0013F014
	public static Vector2I PosToXY(Vector3 pos)
	{
		Vector2I result;
		Grid.CellToXY(Grid.PosToCell(pos), out result.x, out result.y);
		return result;
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x00140E3B File Offset: 0x0013F03B
	public static int XYToCell(int x, int y)
	{
		return x + y * Grid.WidthInCells;
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x00140E46 File Offset: 0x0013F046
	public static void CellToXY(int cell, out int x, out int y)
	{
		x = Grid.CellColumn(cell);
		y = Grid.CellRow(cell);
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x00140E58 File Offset: 0x0013F058
	public static Vector2I CellToXY(int cell)
	{
		return new Vector2I(Grid.CellColumn(cell), Grid.CellRow(cell));
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x00140E6C File Offset: 0x0013F06C
	public static Vector3 CellToPos(int cell, float x_offset, float y_offset, float z_offset)
	{
		int widthInCells = Grid.WidthInCells;
		float num = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float num2 = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(num + x_offset, num2 + y_offset, z_offset);
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x00140EA0 File Offset: 0x0013F0A0
	public static Vector3 CellToPos(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(x, y, 0f);
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x00140ED4 File Offset: 0x0013F0D4
	public static Vector3 CellToPos2D(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector2(x, y);
	}

	// Token: 0x0600397D RID: 14717 RVA: 0x00140F07 File Offset: 0x0013F107
	public static int CellRow(int cell)
	{
		return cell / Grid.WidthInCells;
	}

	// Token: 0x0600397E RID: 14718 RVA: 0x00140F10 File Offset: 0x0013F110
	public static int CellColumn(int cell)
	{
		return cell % Grid.WidthInCells;
	}

	// Token: 0x0600397F RID: 14719 RVA: 0x00140F19 File Offset: 0x0013F119
	public static int ClampX(int x)
	{
		return Math.Min(Math.Max(x, 0), Grid.WidthInCells - 1);
	}

	// Token: 0x06003980 RID: 14720 RVA: 0x00140F2E File Offset: 0x0013F12E
	public static int ClampY(int y)
	{
		return Math.Min(Math.Max(y, 0), Grid.HeightInCells - 1);
	}

	// Token: 0x06003981 RID: 14721 RVA: 0x00140F44 File Offset: 0x0013F144
	public static Vector2I Constrain(Vector2I val)
	{
		val.x = Mathf.Max(0, Mathf.Min(val.x, Grid.WidthInCells - 1));
		val.y = Mathf.Max(0, Mathf.Min(val.y, Grid.HeightInCells - 1));
		return val;
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x00140F90 File Offset: 0x0013F190
	public static void Reveal(int cell, byte visibility = 255, bool forceReveal = false)
	{
		bool flag = Grid.Spawnable[cell] == 0 && visibility > 0;
		Grid.Spawnable[cell] = Math.Max(visibility, Grid.Visible[cell]);
		if (forceReveal || !Grid.PreventFogOfWarReveal[cell])
		{
			Grid.Visible[cell] = Math.Max(visibility, Grid.Visible[cell]);
		}
		if (flag && Grid.OnReveal != null)
		{
			Grid.OnReveal(cell);
		}
	}

	// Token: 0x06003983 RID: 14723 RVA: 0x00140FF9 File Offset: 0x0013F1F9
	public static ObjectLayer GetObjectLayerForConduitType(ConduitType conduit_type)
	{
		switch (conduit_type)
		{
		case ConduitType.Gas:
			return ObjectLayer.GasConduitConnection;
		case ConduitType.Liquid:
			return ObjectLayer.LiquidConduitConnection;
		case ConduitType.Solid:
			return ObjectLayer.SolidConduitConnection;
		default:
			throw new ArgumentException("Invalid value.", "conduit_type");
		}
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x0014102C File Offset: 0x0013F22C
	public static Vector3 CellToPos(int cell, CellAlignment alignment, Grid.SceneLayer layer)
	{
		switch (alignment)
		{
		case CellAlignment.Bottom:
			return Grid.CellToPosCBC(cell, layer);
		case CellAlignment.Top:
			return Grid.CellToPosCTC(cell, layer);
		case CellAlignment.Left:
			return Grid.CellToPosLCC(cell, layer);
		case CellAlignment.Right:
			return Grid.CellToPosRCC(cell, layer);
		case CellAlignment.RandomInternal:
		{
			Vector3 b = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0f, 0f);
			return Grid.CellToPosCCC(cell, layer) + b;
		}
		}
		return Grid.CellToPosCCC(cell, layer);
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x001410AE File Offset: 0x0013F2AE
	public static float GetLayerZ(Grid.SceneLayer layer)
	{
		return -Grid.HalfCellSizeInMeters - Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier;
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x001410C5 File Offset: 0x0013F2C5
	public static Vector3 CellToPosCCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x001410DD File Offset: 0x0013F2DD
	public static Vector3 CellToPosCBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x06003988 RID: 14728 RVA: 0x001410F5 File Offset: 0x0013F2F5
	public static Vector3 CellToPosCCF(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, -Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier);
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x00141116 File Offset: 0x0013F316
	public static Vector3 CellToPosLCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600398A RID: 14730 RVA: 0x0014112E File Offset: 0x0013F32E
	public static Vector3 CellToPosRCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x0014114C File Offset: 0x0013F34C
	public static Vector3 CellToPosCTC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.CellSizeInMeters - 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600398C RID: 14732 RVA: 0x0014116A File Offset: 0x0013F36A
	public static bool IsSolidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x00141184 File Offset: 0x0013F384
	public unsafe static bool IsSubstantialLiquid(int cell, float threshold = 0.35f)
	{
		if (Grid.IsValidCell(cell))
		{
			ushort num = Grid.elementIdx[cell];
			if ((int)num < ElementLoader.elements.Count)
			{
				Element element = ElementLoader.elements[(int)num];
				if (element.IsLiquid && Grid.mass[cell] >= element.defaultValues.mass * threshold)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x001411E4 File Offset: 0x0013F3E4
	public static bool IsVisiblyInLiquid(Vector2 pos)
	{
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && Grid.IsLiquid(num))
		{
			int cell = Grid.CellAbove(num);
			if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
			{
				return true;
			}
			float num2 = Grid.Mass[num];
			float num3 = (float)((int)pos.y) - pos.y;
			if (num2 / 1000f <= num3)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x00141248 File Offset: 0x0013F448
	public static bool IsLiquid(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsLiquid;
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x00141269 File Offset: 0x0013F469
	public static bool IsGas(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsGas;
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x0014128C File Offset: 0x0013F48C
	public static void GetVisibleExtents(out int min_x, out int min_y, out int max_x, out int max_y)
	{
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		min_y = (int)vector2.y;
		max_y = (int)(vector.y + 0.5f);
		min_x = (int)vector2.x;
		max_x = (int)(vector.x + 0.5f);
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x00141325 File Offset: 0x0013F525
	public static void GetVisibleExtents(out Vector2I min, out Vector2I max)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x00141344 File Offset: 0x0013F544
	public static bool IsVisible(int cell)
	{
		return Grid.Visible[cell] > 0 || !PropertyTextures.IsFogOfWarEnabled;
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x0014135A File Offset: 0x0013F55A
	public static bool VisibleBlockingCB(int cell)
	{
		return !Grid.Transparent[cell] && Grid.IsSolidCell(cell);
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x00141371 File Offset: 0x0013F571
	public static bool VisibilityTest(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.TestLineOfSight(x, y, x2, y2, Grid.VisibleBlockingDelegate, blocking_tile_visible, false);
	}

	// Token: 0x06003996 RID: 14742 RVA: 0x00141384 File Offset: 0x0013F584
	public static bool VisibilityTest(int cell, int target_cell, bool blocking_tile_visible = false)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		int x2 = 0;
		int y2 = 0;
		Grid.CellToXY(target_cell, out x2, out y2);
		return Grid.VisibilityTest(x, y, x2, y2, blocking_tile_visible);
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x001413B7 File Offset: 0x0013F5B7
	public static bool PhysicalBlockingCB(int cell)
	{
		return Grid.Solid[cell];
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x001413C4 File Offset: 0x0013F5C4
	public static bool IsPhysicallyAccessible(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.TestLineOfSight(x, y, x2, y2, Grid.PhysicalBlockingDelegate, blocking_tile_visible, false);
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x001413D8 File Offset: 0x0013F5D8
	public static void CollectCellsInLine(int startCell, int endCell, HashSet<int> outputCells)
	{
		int num = 2;
		int cellDistance = Grid.GetCellDistance(startCell, endCell);
		Vector2 a = (Grid.CellToPos(endCell) - Grid.CellToPos(startCell)).normalized;
		for (float num2 = 0f; num2 < (float)cellDistance; num2 = Mathf.Min(num2 + 1f / (float)num, (float)cellDistance))
		{
			int num3 = Grid.PosToCell(Grid.CellToPos(startCell) + a * num2);
			if (Grid.GetCellDistance(startCell, num3) <= cellDistance)
			{
				outputCells.Add(num3);
			}
		}
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x00141464 File Offset: 0x0013F664
	public static bool IsRangeExposedToSunlight(int cell, int scanRadius, CellOffset scanShape, out int cellsClear, int clearThreshold = 1)
	{
		cellsClear = 0;
		if (Grid.IsValidCell(cell) && (int)Grid.ExposedToSunlight[cell] >= clearThreshold)
		{
			cellsClear++;
		}
		bool flag = true;
		bool flag2 = true;
		int num = 1;
		while (num <= scanRadius && (flag || flag2))
		{
			int num2 = Grid.OffsetCell(cell, scanShape.x * num, scanShape.y * num);
			int num3 = Grid.OffsetCell(cell, -scanShape.x * num, scanShape.y * num);
			if (Grid.IsValidCell(num2) && (int)Grid.ExposedToSunlight[num2] >= clearThreshold)
			{
				cellsClear++;
			}
			if (Grid.IsValidCell(num3) && (int)Grid.ExposedToSunlight[num3] >= clearThreshold)
			{
				cellsClear++;
			}
			num++;
		}
		return cellsClear > 0;
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x00141518 File Offset: 0x0013F718
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible = false, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x00141634 File Offset: 0x0013F834
	public static bool GetFreeGridSpace(Vector2I size, out Vector2I offset)
	{
		Vector2I gridOffset = BestFit.GetGridOffset(ClusterManager.Instance.WorldContainers, size, out offset);
		if (gridOffset.X <= Grid.WidthInCells && gridOffset.Y <= Grid.HeightInCells)
		{
			SimMessages.SimDataResizeGridAndInitializeVacuumCells(gridOffset, size.x, size.y, offset.x, offset.y);
			Game.Instance.roomProber.Refresh();
			return true;
		}
		return false;
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x001416A0 File Offset: 0x0013F8A0
	public static void FreeGridSpace(Vector2I size, Vector2I offset)
	{
		SimMessages.SimDataFreeCells(size.x, size.y, offset.x, offset.y);
		for (int i = offset.y; i < size.y + offset.y + 1; i++)
		{
			for (int j = offset.x - 1; j < size.x + offset.x + 1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num))
				{
					Grid.Element[num] = ElementLoader.FindElementByHash(SimHashes.Vacuum);
				}
			}
		}
		Game.Instance.roomProber.Refresh();
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x0014173A File Offset: 0x0013F93A
	[Conditional("UNITY_EDITOR")]
	public static void DrawBoxOnCell(int cell, Color color, float offset = 0f)
	{
		Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0f);
	}

	// Token: 0x04002621 RID: 9761
	public static readonly CellOffset[] DefaultOffset = new CellOffset[1];

	// Token: 0x04002622 RID: 9762
	public static float WidthInMeters;

	// Token: 0x04002623 RID: 9763
	public static float HeightInMeters;

	// Token: 0x04002624 RID: 9764
	public static int WidthInCells;

	// Token: 0x04002625 RID: 9765
	public static int HeightInCells;

	// Token: 0x04002626 RID: 9766
	public static float CellSizeInMeters;

	// Token: 0x04002627 RID: 9767
	public static float InverseCellSizeInMeters;

	// Token: 0x04002628 RID: 9768
	public static float HalfCellSizeInMeters;

	// Token: 0x04002629 RID: 9769
	public static int CellCount;

	// Token: 0x0400262A RID: 9770
	public static int InvalidCell = -1;

	// Token: 0x0400262B RID: 9771
	public static int TopBorderHeight = 2;

	// Token: 0x0400262C RID: 9772
	public static Dictionary<int, GameObject>[] ObjectLayers;

	// Token: 0x0400262D RID: 9773
	public static Action<int> OnReveal;

	// Token: 0x0400262E RID: 9774
	public static Grid.BuildFlags[] BuildMasks;

	// Token: 0x0400262F RID: 9775
	public static Grid.BuildFlagsFoundationIndexer Foundation;

	// Token: 0x04002630 RID: 9776
	public static Grid.BuildFlagsSolidIndexer Solid;

	// Token: 0x04002631 RID: 9777
	public static Grid.BuildFlagsDupeImpassableIndexer DupeImpassable;

	// Token: 0x04002632 RID: 9778
	public static Grid.BuildFlagsFakeFloorIndexer FakeFloor;

	// Token: 0x04002633 RID: 9779
	public static Grid.BuildFlagsDupePassableIndexer DupePassable;

	// Token: 0x04002634 RID: 9780
	public static Grid.BuildFlagsImpassableIndexer CritterImpassable;

	// Token: 0x04002635 RID: 9781
	public static Grid.BuildFlagsDoorIndexer HasDoor;

	// Token: 0x04002636 RID: 9782
	public static Grid.VisFlags[] VisMasks;

	// Token: 0x04002637 RID: 9783
	public static Grid.VisFlagsRevealedIndexer Revealed;

	// Token: 0x04002638 RID: 9784
	public static Grid.VisFlagsPreventFogOfWarRevealIndexer PreventFogOfWarReveal;

	// Token: 0x04002639 RID: 9785
	public static Grid.VisFlagsRenderedByWorldIndexer RenderedByWorld;

	// Token: 0x0400263A RID: 9786
	public static Grid.VisFlagsAllowPathfindingIndexer AllowPathfinding;

	// Token: 0x0400263B RID: 9787
	public static Grid.NavValidatorFlags[] NavValidatorMasks;

	// Token: 0x0400263C RID: 9788
	public static Grid.NavValidatorFlagsLadderIndexer HasLadder;

	// Token: 0x0400263D RID: 9789
	public static Grid.NavValidatorFlagsPoleIndexer HasPole;

	// Token: 0x0400263E RID: 9790
	public static Grid.NavValidatorFlagsTubeIndexer HasTube;

	// Token: 0x0400263F RID: 9791
	public static Grid.NavValidatorFlagsNavTeleporterIndexer HasNavTeleporter;

	// Token: 0x04002640 RID: 9792
	public static Grid.NavValidatorFlagsUnderConstructionIndexer IsTileUnderConstruction;

	// Token: 0x04002641 RID: 9793
	public static Grid.NavFlags[] NavMasks;

	// Token: 0x04002642 RID: 9794
	private static Grid.NavFlagsAccessDoorIndexer HasAccessDoor;

	// Token: 0x04002643 RID: 9795
	public static Grid.NavFlagsTubeEntranceIndexer HasTubeEntrance;

	// Token: 0x04002644 RID: 9796
	public static Grid.NavFlagsPreventIdleTraversalIndexer PreventIdleTraversal;

	// Token: 0x04002645 RID: 9797
	public static Grid.NavFlagsReservedIndexer Reserved;

	// Token: 0x04002646 RID: 9798
	public static Grid.NavFlagsSuitMarkerIndexer HasSuitMarker;

	// Token: 0x04002647 RID: 9799
	private static Dictionary<int, Grid.Restriction> restrictions = new Dictionary<int, Grid.Restriction>();

	// Token: 0x04002648 RID: 9800
	private static Dictionary<int, Grid.TubeEntrance> tubeEntrances = new Dictionary<int, Grid.TubeEntrance>();

	// Token: 0x04002649 RID: 9801
	private static Dictionary<int, Grid.SuitMarker> suitMarkers = new Dictionary<int, Grid.SuitMarker>();

	// Token: 0x0400264A RID: 9802
	public unsafe static ushort* elementIdx;

	// Token: 0x0400264B RID: 9803
	public unsafe static float* temperature;

	// Token: 0x0400264C RID: 9804
	public unsafe static float* radiation;

	// Token: 0x0400264D RID: 9805
	public unsafe static float* mass;

	// Token: 0x0400264E RID: 9806
	public unsafe static byte* properties;

	// Token: 0x0400264F RID: 9807
	public unsafe static byte* strengthInfo;

	// Token: 0x04002650 RID: 9808
	public unsafe static byte* insulation;

	// Token: 0x04002651 RID: 9809
	public unsafe static byte* diseaseIdx;

	// Token: 0x04002652 RID: 9810
	public unsafe static int* diseaseCount;

	// Token: 0x04002653 RID: 9811
	public unsafe static byte* exposedToSunlight;

	// Token: 0x04002654 RID: 9812
	public unsafe static float* AccumulatedFlowValues = null;

	// Token: 0x04002655 RID: 9813
	public static byte[] Visible;

	// Token: 0x04002656 RID: 9814
	public static byte[] Spawnable;

	// Token: 0x04002657 RID: 9815
	public static float[] Damage;

	// Token: 0x04002658 RID: 9816
	public static float[] Decor;

	// Token: 0x04002659 RID: 9817
	public static bool[] GravitasFacility;

	// Token: 0x0400265A RID: 9818
	public static byte[] WorldIdx;

	// Token: 0x0400265B RID: 9819
	public static float[] Loudness;

	// Token: 0x0400265C RID: 9820
	public static Element[] Element;

	// Token: 0x0400265D RID: 9821
	public static int[] LightCount;

	// Token: 0x0400265E RID: 9822
	public static Grid.PressureIndexer Pressure;

	// Token: 0x0400265F RID: 9823
	public static Grid.TransparentIndexer Transparent;

	// Token: 0x04002660 RID: 9824
	public static Grid.ElementIdxIndexer ElementIdx;

	// Token: 0x04002661 RID: 9825
	public static Grid.TemperatureIndexer Temperature;

	// Token: 0x04002662 RID: 9826
	public static Grid.RadiationIndexer Radiation;

	// Token: 0x04002663 RID: 9827
	public static Grid.MassIndexer Mass;

	// Token: 0x04002664 RID: 9828
	public static Grid.PropertiesIndexer Properties;

	// Token: 0x04002665 RID: 9829
	public static Grid.ExposedToSunlightIndexer ExposedToSunlight;

	// Token: 0x04002666 RID: 9830
	public static Grid.StrengthInfoIndexer StrengthInfo;

	// Token: 0x04002667 RID: 9831
	public static Grid.Insulationndexer Insulation;

	// Token: 0x04002668 RID: 9832
	public static Grid.DiseaseIdxIndexer DiseaseIdx;

	// Token: 0x04002669 RID: 9833
	public static Grid.DiseaseCountIndexer DiseaseCount;

	// Token: 0x0400266A RID: 9834
	public static Grid.LightIntensityIndexer LightIntensity;

	// Token: 0x0400266B RID: 9835
	public static Grid.AccumulatedFlowIndexer AccumulatedFlow;

	// Token: 0x0400266C RID: 9836
	public static Grid.ObjectLayerIndexer Objects;

	// Token: 0x0400266D RID: 9837
	public static float LayerMultiplier = 1f;

	// Token: 0x0400266E RID: 9838
	private static readonly Func<int, bool> VisibleBlockingDelegate = (int cell) => Grid.VisibleBlockingCB(cell);

	// Token: 0x0400266F RID: 9839
	private static readonly Func<int, bool> PhysicalBlockingDelegate = (int cell) => Grid.PhysicalBlockingCB(cell);

	// Token: 0x0200159F RID: 5535
	[Flags]
	public enum BuildFlags : byte
	{
		// Token: 0x0400693E RID: 26942
		Solid = 1,
		// Token: 0x0400693F RID: 26943
		Foundation = 2,
		// Token: 0x04006940 RID: 26944
		Door = 4,
		// Token: 0x04006941 RID: 26945
		DupePassable = 8,
		// Token: 0x04006942 RID: 26946
		DupeImpassable = 16,
		// Token: 0x04006943 RID: 26947
		CritterImpassable = 32,
		// Token: 0x04006944 RID: 26948
		FakeFloor = 192,
		// Token: 0x04006945 RID: 26949
		Any = 255
	}

	// Token: 0x020015A0 RID: 5536
	public struct BuildFlagsFoundationIndexer
	{
		// Token: 0x170008F4 RID: 2292
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Foundation) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Foundation, value);
			}
		}
	}

	// Token: 0x020015A1 RID: 5537
	public struct BuildFlagsSolidIndexer
	{
		// Token: 0x170008F5 RID: 2293
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Solid) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}
	}

	// Token: 0x020015A2 RID: 5538
	public struct BuildFlagsDupeImpassableIndexer
	{
		// Token: 0x170008F6 RID: 2294
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupeImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupeImpassable, value);
			}
		}
	}

	// Token: 0x020015A3 RID: 5539
	public struct BuildFlagsFakeFloorIndexer
	{
		// Token: 0x170008F7 RID: 2295
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.FakeFloor) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}

		// Token: 0x06008837 RID: 34871 RVA: 0x0030E044 File Offset: 0x0030C244
		public void Add(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) + 1);
			num = Math.Min(num, 3);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}

		// Token: 0x06008838 RID: 34872 RVA: 0x0030E084 File Offset: 0x0030C284
		public void Remove(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) - Grid.BuildFlags.Solid);
			num = Math.Max(num, 0);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}
	}

	// Token: 0x020015A4 RID: 5540
	public struct BuildFlagsDupePassableIndexer
	{
		// Token: 0x170008F8 RID: 2296
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupePassable, value);
			}
		}
	}

	// Token: 0x020015A5 RID: 5541
	public struct BuildFlagsImpassableIndexer
	{
		// Token: 0x170008F9 RID: 2297
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.CritterImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.CritterImpassable, value);
			}
		}
	}

	// Token: 0x020015A6 RID: 5542
	public struct BuildFlagsDoorIndexer
	{
		// Token: 0x170008FA RID: 2298
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Door) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Door, value);
			}
		}
	}

	// Token: 0x020015A7 RID: 5543
	[Flags]
	public enum VisFlags : byte
	{
		// Token: 0x04006947 RID: 26951
		Revealed = 1,
		// Token: 0x04006948 RID: 26952
		PreventFogOfWarReveal = 2,
		// Token: 0x04006949 RID: 26953
		RenderedByWorld = 4,
		// Token: 0x0400694A RID: 26954
		AllowPathfinding = 8
	}

	// Token: 0x020015A8 RID: 5544
	public struct VisFlagsRevealedIndexer
	{
		// Token: 0x170008FB RID: 2299
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.Revealed) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.Revealed, value);
			}
		}
	}

	// Token: 0x020015A9 RID: 5545
	public struct VisFlagsPreventFogOfWarRevealIndexer
	{
		// Token: 0x170008FC RID: 2300
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.PreventFogOfWarReveal) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.PreventFogOfWarReveal, value);
			}
		}
	}

	// Token: 0x020015AA RID: 5546
	public struct VisFlagsRenderedByWorldIndexer
	{
		// Token: 0x170008FD RID: 2301
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.RenderedByWorld) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.RenderedByWorld, value);
			}
		}
	}

	// Token: 0x020015AB RID: 5547
	public struct VisFlagsAllowPathfindingIndexer
	{
		// Token: 0x170008FE RID: 2302
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.AllowPathfinding) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.AllowPathfinding, value);
			}
		}
	}

	// Token: 0x020015AC RID: 5548
	[Flags]
	public enum NavValidatorFlags : byte
	{
		// Token: 0x0400694C RID: 26956
		Ladder = 1,
		// Token: 0x0400694D RID: 26957
		Pole = 2,
		// Token: 0x0400694E RID: 26958
		Tube = 4,
		// Token: 0x0400694F RID: 26959
		NavTeleporter = 8,
		// Token: 0x04006950 RID: 26960
		UnderConstruction = 16
	}

	// Token: 0x020015AD RID: 5549
	public struct NavValidatorFlagsLadderIndexer
	{
		// Token: 0x170008FF RID: 2303
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Ladder) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Ladder, value);
			}
		}
	}

	// Token: 0x020015AE RID: 5550
	public struct NavValidatorFlagsPoleIndexer
	{
		// Token: 0x17000900 RID: 2304
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Pole) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Pole, value);
			}
		}
	}

	// Token: 0x020015AF RID: 5551
	public struct NavValidatorFlagsTubeIndexer
	{
		// Token: 0x17000901 RID: 2305
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Tube) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Tube, value);
			}
		}
	}

	// Token: 0x020015B0 RID: 5552
	public struct NavValidatorFlagsNavTeleporterIndexer
	{
		// Token: 0x17000902 RID: 2306
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.NavTeleporter) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.NavTeleporter, value);
			}
		}
	}

	// Token: 0x020015B1 RID: 5553
	public struct NavValidatorFlagsUnderConstructionIndexer
	{
		// Token: 0x17000903 RID: 2307
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.UnderConstruction) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.UnderConstruction, value);
			}
		}
	}

	// Token: 0x020015B2 RID: 5554
	[Flags]
	public enum NavFlags : byte
	{
		// Token: 0x04006952 RID: 26962
		AccessDoor = 1,
		// Token: 0x04006953 RID: 26963
		TubeEntrance = 2,
		// Token: 0x04006954 RID: 26964
		PreventIdleTraversal = 4,
		// Token: 0x04006955 RID: 26965
		Reserved = 8,
		// Token: 0x04006956 RID: 26966
		SuitMarker = 16
	}

	// Token: 0x020015B3 RID: 5555
	public struct NavFlagsAccessDoorIndexer
	{
		// Token: 0x17000904 RID: 2308
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.AccessDoor) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.AccessDoor, value);
			}
		}
	}

	// Token: 0x020015B4 RID: 5556
	public struct NavFlagsTubeEntranceIndexer
	{
		// Token: 0x17000905 RID: 2309
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.TubeEntrance) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.TubeEntrance, value);
			}
		}
	}

	// Token: 0x020015B5 RID: 5557
	public struct NavFlagsPreventIdleTraversalIndexer
	{
		// Token: 0x17000906 RID: 2310
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.PreventIdleTraversal) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.PreventIdleTraversal, value);
			}
		}
	}

	// Token: 0x020015B6 RID: 5558
	public struct NavFlagsReservedIndexer
	{
		// Token: 0x17000907 RID: 2311
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.Reserved) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.Reserved, value);
			}
		}
	}

	// Token: 0x020015B7 RID: 5559
	public struct NavFlagsSuitMarkerIndexer
	{
		// Token: 0x17000908 RID: 2312
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.SuitMarker) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.SuitMarker, value);
			}
		}
	}

	// Token: 0x020015B8 RID: 5560
	public struct Restriction
	{
		// Token: 0x04006957 RID: 26967
		public const int DefaultID = -1;

		// Token: 0x04006958 RID: 26968
		public Dictionary<int, Grid.Restriction.Directions> DirectionMasksForMinionInstanceID;

		// Token: 0x04006959 RID: 26969
		public Grid.Restriction.Orientation orientation;

		// Token: 0x0200218C RID: 8588
		[Flags]
		public enum Directions : byte
		{
			// Token: 0x04009642 RID: 38466
			Left = 1,
			// Token: 0x04009643 RID: 38467
			Right = 2,
			// Token: 0x04009644 RID: 38468
			Teleport = 4
		}

		// Token: 0x0200218D RID: 8589
		public enum Orientation : byte
		{
			// Token: 0x04009646 RID: 38470
			Vertical,
			// Token: 0x04009647 RID: 38471
			Horizontal,
			// Token: 0x04009648 RID: 38472
			SingleCell
		}
	}

	// Token: 0x020015B9 RID: 5561
	private struct TubeEntrance
	{
		// Token: 0x0400695A RID: 26970
		public bool operational;

		// Token: 0x0400695B RID: 26971
		public int reservationCapacity;

		// Token: 0x0400695C RID: 26972
		public HashSet<int> reservedInstanceIDs;
	}

	// Token: 0x020015BA RID: 5562
	public struct SuitMarker
	{
		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600885B RID: 34907 RVA: 0x0030E261 File Offset: 0x0030C461
		public int emptyLockerCount
		{
			get
			{
				return this.lockerCount - this.suitCount;
			}
		}

		// Token: 0x0400695D RID: 26973
		public int suitCount;

		// Token: 0x0400695E RID: 26974
		public int lockerCount;

		// Token: 0x0400695F RID: 26975
		public Grid.SuitMarker.Flags flags;

		// Token: 0x04006960 RID: 26976
		public PathFinder.PotentialPath.Flags pathFlags;

		// Token: 0x04006961 RID: 26977
		public HashSet<int> minionIDsWithSuitReservations;

		// Token: 0x04006962 RID: 26978
		public HashSet<int> minionIDsWithEmptyLockerReservations;

		// Token: 0x0200218E RID: 8590
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x0400964A RID: 38474
			OnlyTraverseIfUnequipAvailable = 1,
			// Token: 0x0400964B RID: 38475
			Operational = 2,
			// Token: 0x0400964C RID: 38476
			Rotated = 4
		}
	}

	// Token: 0x020015BB RID: 5563
	public struct ObjectLayerIndexer
	{
		// Token: 0x1700090A RID: 2314
		public GameObject this[int cell, int layer]
		{
			get
			{
				GameObject result = null;
				Grid.ObjectLayers[layer].TryGetValue(cell, out result);
				return result;
			}
			set
			{
				if (value == null)
				{
					Grid.ObjectLayers[layer].Remove(cell);
				}
				else
				{
					Grid.ObjectLayers[layer][cell] = value;
				}
				GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.objectLayers[layer], value);
			}
		}
	}

	// Token: 0x020015BC RID: 5564
	public struct PressureIndexer
	{
		// Token: 0x1700090B RID: 2315
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i] * 101.3f;
			}
		}
	}

	// Token: 0x020015BD RID: 5565
	public struct TransparentIndexer
	{
		// Token: 0x1700090C RID: 2316
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 16) > 0;
			}
		}
	}

	// Token: 0x020015BE RID: 5566
	public struct ElementIdxIndexer
	{
		// Token: 0x1700090D RID: 2317
		public unsafe ushort this[int i]
		{
			get
			{
				return Grid.elementIdx[i];
			}
		}
	}

	// Token: 0x020015BF RID: 5567
	public struct TemperatureIndexer
	{
		// Token: 0x1700090E RID: 2318
		public unsafe float this[int i]
		{
			get
			{
				return Grid.temperature[i];
			}
		}
	}

	// Token: 0x020015C0 RID: 5568
	public struct RadiationIndexer
	{
		// Token: 0x1700090F RID: 2319
		public unsafe float this[int i]
		{
			get
			{
				return Grid.radiation[i];
			}
		}
	}

	// Token: 0x020015C1 RID: 5569
	public struct MassIndexer
	{
		// Token: 0x17000910 RID: 2320
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i];
			}
		}
	}

	// Token: 0x020015C2 RID: 5570
	public struct PropertiesIndexer
	{
		// Token: 0x17000911 RID: 2321
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.properties[i];
			}
		}
	}

	// Token: 0x020015C3 RID: 5571
	public struct ExposedToSunlightIndexer
	{
		// Token: 0x17000912 RID: 2322
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.exposedToSunlight[i];
			}
		}
	}

	// Token: 0x020015C4 RID: 5572
	public struct StrengthInfoIndexer
	{
		// Token: 0x17000913 RID: 2323
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.strengthInfo[i];
			}
		}
	}

	// Token: 0x020015C5 RID: 5573
	public struct Insulationndexer
	{
		// Token: 0x17000914 RID: 2324
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.insulation[i];
			}
		}
	}

	// Token: 0x020015C6 RID: 5574
	public struct DiseaseIdxIndexer
	{
		// Token: 0x17000915 RID: 2325
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.diseaseIdx[i];
			}
		}
	}

	// Token: 0x020015C7 RID: 5575
	public struct DiseaseCountIndexer
	{
		// Token: 0x17000916 RID: 2326
		public unsafe int this[int i]
		{
			get
			{
				return Grid.diseaseCount[i];
			}
		}
	}

	// Token: 0x020015C8 RID: 5576
	public struct AccumulatedFlowIndexer
	{
		// Token: 0x17000917 RID: 2327
		public unsafe float this[int i]
		{
			get
			{
				return Grid.AccumulatedFlowValues[i];
			}
		}
	}

	// Token: 0x020015C9 RID: 5577
	public struct LightIntensityIndexer
	{
		// Token: 0x17000918 RID: 2328
		public unsafe int this[int i]
		{
			get
			{
				float num = Game.Instance.currentFallbackSunlightIntensity;
				WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[i]);
				if (world != null)
				{
					num = world.currentSunlightIntensity;
				}
				int num2 = (int)((float)Grid.exposedToSunlight[i] / 255f * num);
				int num3 = Grid.LightCount[i];
				return num2 + num3;
			}
		}
	}

	// Token: 0x020015CA RID: 5578
	public enum SceneLayer
	{
		// Token: 0x04006964 RID: 26980
		WorldSelection = -3,
		// Token: 0x04006965 RID: 26981
		NoLayer,
		// Token: 0x04006966 RID: 26982
		Background,
		// Token: 0x04006967 RID: 26983
		Backwall = 1,
		// Token: 0x04006968 RID: 26984
		Gas,
		// Token: 0x04006969 RID: 26985
		GasConduits,
		// Token: 0x0400696A RID: 26986
		GasConduitBridges,
		// Token: 0x0400696B RID: 26987
		LiquidConduits,
		// Token: 0x0400696C RID: 26988
		LiquidConduitBridges,
		// Token: 0x0400696D RID: 26989
		SolidConduits,
		// Token: 0x0400696E RID: 26990
		SolidConduitContents,
		// Token: 0x0400696F RID: 26991
		SolidConduitBridges,
		// Token: 0x04006970 RID: 26992
		Wires,
		// Token: 0x04006971 RID: 26993
		WireBridges,
		// Token: 0x04006972 RID: 26994
		WireBridgesFront,
		// Token: 0x04006973 RID: 26995
		LogicWires,
		// Token: 0x04006974 RID: 26996
		LogicGates,
		// Token: 0x04006975 RID: 26997
		LogicGatesFront,
		// Token: 0x04006976 RID: 26998
		InteriorWall,
		// Token: 0x04006977 RID: 26999
		GasFront,
		// Token: 0x04006978 RID: 27000
		BuildingBack,
		// Token: 0x04006979 RID: 27001
		Building,
		// Token: 0x0400697A RID: 27002
		BuildingUse,
		// Token: 0x0400697B RID: 27003
		BuildingFront,
		// Token: 0x0400697C RID: 27004
		TransferArm,
		// Token: 0x0400697D RID: 27005
		Ore,
		// Token: 0x0400697E RID: 27006
		Creatures,
		// Token: 0x0400697F RID: 27007
		Move,
		// Token: 0x04006980 RID: 27008
		Front,
		// Token: 0x04006981 RID: 27009
		GlassTile,
		// Token: 0x04006982 RID: 27010
		Liquid,
		// Token: 0x04006983 RID: 27011
		Ground,
		// Token: 0x04006984 RID: 27012
		TileMain,
		// Token: 0x04006985 RID: 27013
		TileFront,
		// Token: 0x04006986 RID: 27014
		FXFront,
		// Token: 0x04006987 RID: 27015
		FXFront2,
		// Token: 0x04006988 RID: 27016
		SceneMAX
	}
}
