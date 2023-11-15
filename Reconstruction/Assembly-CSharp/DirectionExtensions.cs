using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
public static class DirectionExtensions
{
	// Token: 0x060004CC RID: 1228 RVA: 0x0000D0A2 File Offset: 0x0000B2A2
	public static Vector3 GetHalfVector(this Direction direction)
	{
		return DirectionExtensions.halfVectors[(int)direction];
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0000D0AF File Offset: 0x0000B2AF
	public static DirectionChange GetDirectionChangeTo(this Direction current, Direction next)
	{
		if (current == next)
		{
			return DirectionChange.None;
		}
		if (current + 1 == next || current - 3 == next)
		{
			return DirectionChange.TurnRight;
		}
		if (current - 1 == next || current + 3 == next)
		{
			return DirectionChange.TurnLeft;
		}
		return DirectionChange.TurnAround;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
	public static float GetAngle(this Direction direction)
	{
		switch (direction)
		{
		case Direction.up:
			return 0f;
		case Direction.right:
			return 270f;
		case Direction.down:
			return 180f;
		case Direction.left:
			return 90f;
		default:
			return 0f;
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0000D10B File Offset: 0x0000B30B
	public static Vector2 GetDirectionPos(this Direction direction)
	{
		return DirectionExtensions.NormalizeDistance[(int)direction] * Singleton<StaticData>.Instance.TileSize;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0000D127 File Offset: 0x0000B327
	public static Quaternion GetRandomRotation()
	{
		return DirectionExtensions.Rotations[Random.Range(0, DirectionExtensions.Rotations.Length)];
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0000D140 File Offset: 0x0000B340
	public static Quaternion GetRotation(this Direction direction)
	{
		return DirectionExtensions.Rotations[(int)direction];
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0000D14D File Offset: 0x0000B34D
	public static Direction GetDirection(int index)
	{
		return (Direction)index;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0000D150 File Offset: 0x0000B350
	public static Direction GetDirection(Vector3 center, Vector3 exit)
	{
		if (exit.x > center.x + 0.2f)
		{
			return Direction.right;
		}
		if (exit.x + 0.2f < center.x)
		{
			return Direction.left;
		}
		if (exit.y + 0.2f < center.y)
		{
			return Direction.down;
		}
		return Direction.up;
	}

	// Token: 0x040001DA RID: 474
	public static readonly Vector2[] NormalizeDistance = new Vector2[]
	{
		new Vector2(0f, 1f),
		new Vector2(1f, 0f),
		new Vector2(0f, -1f),
		new Vector2(-1f, 0f)
	};

	// Token: 0x040001DB RID: 475
	private static Quaternion[] Rotations = new Quaternion[]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f)
	};

	// Token: 0x040001DC RID: 476
	private static Vector3[] halfVectors = new Vector3[]
	{
		Vector3.up * 0.5f,
		Vector3.right * 0.5f,
		Vector3.down * 0.5f,
		Vector3.left * 0.5f
	};
}
