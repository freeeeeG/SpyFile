using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000556 RID: 1366
public abstract class DevToolEntityTarget
{
	// Token: 0x06002148 RID: 8520
	public abstract string GetTag();

	// Token: 0x06002149 RID: 8521
	[return: TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})]
	public abstract Option<ValueTuple<Vector2, Vector2>> GetScreenRect();

	// Token: 0x0600214A RID: 8522 RVA: 0x000B2732 File Offset: 0x000B0932
	public string GetDebugName()
	{
		return "[" + this.GetTag() + "] " + this.ToString();
	}

	// Token: 0x020011EF RID: 4591
	public class ForUIGameObject : DevToolEntityTarget
	{
		// Token: 0x06007B38 RID: 31544 RVA: 0x002DD84D File Offset: 0x002DBA4D
		public ForUIGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x002DD85C File Offset: 0x002DBA5C
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			RectTransform component = this.gameObject.GetComponent<RectTransform>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			if (!componentInParent.worldCamera.IsNullOrDestroyed())
			{
				DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.camera = componentInParent.worldCamera;
				Vector3[] array = new Vector3[4];
				component.GetWorldCorners(array);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[0]), ref CS$<>8__locals1), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[2]), ref CS$<>8__locals1));
			}
			if (componentInParent.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				Vector3[] array2 = new Vector3[4];
				component.GetWorldCorners(array2);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[0]), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[2]));
			}
			return Option.None;
		}

		// Token: 0x06007B3A RID: 31546 RVA: 0x002DD97F File Offset: 0x002DBB7F
		public override string GetTag()
		{
			return "UI";
		}

		// Token: 0x06007B3B RID: 31547 RVA: 0x002DD986 File Offset: 0x002DBB86
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x06007B3C RID: 31548 RVA: 0x002DD993 File Offset: 0x002DBB93
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x06007B3D RID: 31549 RVA: 0x002DD9B3 File Offset: 0x002DBBB3
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_1(Vector2 coord)
		{
			return new Vector2(coord.x, (float)Screen.height - coord.y);
		}

		// Token: 0x04005E03 RID: 24067
		public GameObject gameObject;
	}

	// Token: 0x020011F0 RID: 4592
	public class ForWorldGameObject : DevToolEntityTarget
	{
		// Token: 0x06007B3E RID: 31550 RVA: 0x002DD9CD File Offset: 0x002DBBCD
		public ForWorldGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x06007B3F RID: 31551 RVA: 0x002DD9DC File Offset: 0x002DBBDC
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			KCollider2D component = this.gameObject.GetComponent<KCollider2D>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.min), ref CS$<>8__locals1), DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.max), ref CS$<>8__locals1));
		}

		// Token: 0x06007B40 RID: 31552 RVA: 0x002DDA98 File Offset: 0x002DBC98
		public override string GetTag()
		{
			return "World";
		}

		// Token: 0x06007B41 RID: 31553 RVA: 0x002DDA9F File Offset: 0x002DBC9F
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x06007B42 RID: 31554 RVA: 0x002DDAAC File Offset: 0x002DBCAC
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x04005E04 RID: 24068
		public GameObject gameObject;
	}

	// Token: 0x020011F1 RID: 4593
	public class ForSimCell : DevToolEntityTarget
	{
		// Token: 0x06007B43 RID: 31555 RVA: 0x002DDACC File Offset: 0x002DBCCC
		public ForSimCell(int cellIndex)
		{
			this.cellIndex = cellIndex;
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x002DDADC File Offset: 0x002DBCDC
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Vector2 a = Grid.CellToPosCCC(this.cellIndex, Grid.SceneLayer.Background);
			Vector2 b = Grid.HalfCellSizeInMeters * Vector2.one;
			Vector2 v = a - b;
			Vector2 v2 = a + b;
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v), ref CS$<>8__locals1), DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v2), ref CS$<>8__locals1));
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x002DDB81 File Offset: 0x002DBD81
		public override string GetTag()
		{
			return "Sim Cell";
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x002DDB88 File Offset: 0x002DBD88
		public override string ToString()
		{
			return this.cellIndex.ToString();
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x002DDB95 File Offset: 0x002DBD95
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x04005E05 RID: 24069
		public int cellIndex;
	}
}
