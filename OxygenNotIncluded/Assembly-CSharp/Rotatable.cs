﻿using System;
using KSerialization;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Rotatable")]
public class Rotatable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0009F12E File Offset: 0x0009D32E
	public Orientation Orientation
	{
		get
		{
			return this.orientation;
		}
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x0009F138 File Offset: 0x0009D338
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			this.SetSize(component.Def.WidthInCells, component.Def.HeightInCells);
		}
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x0009F190 File Offset: 0x0009D390
	public void SetSize(int width, int height)
	{
		this.width = width;
		this.height = height;
		if (width % 2 == 0)
		{
			this.pivot = new Vector3(-0.5f, 0.5f, 0f);
			this.visualizerOffset = new Vector3(0.5f, 0f, 0f);
			return;
		}
		this.pivot = new Vector3(0f, 0.5f, 0f);
		this.visualizerOffset = Vector3.zero;
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x0009F210 File Offset: 0x0009D410
	public Orientation Rotate()
	{
		switch (this.permittedRotations)
		{
		case PermittedRotations.R90:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.R90 : Orientation.Neutral);
			break;
		case PermittedRotations.R360:
			this.orientation = (this.orientation + 1) % Orientation.NumRotations;
			break;
		case PermittedRotations.FlipH:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipH : Orientation.Neutral);
			break;
		case PermittedRotations.FlipV:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipV : Orientation.Neutral);
			break;
		}
		this.OrientVisualizer(this.orientation);
		return this.orientation;
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x0009F29C File Offset: 0x0009D49C
	public void SetOrientation(Orientation new_orientation)
	{
		this.orientation = new_orientation;
		this.OrientVisualizer(new_orientation);
		this.OrientCollider(new_orientation);
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x0009F2B4 File Offset: 0x0009D4B4
	public void Match(Rotatable other)
	{
		this.pivot = other.pivot;
		this.visualizerOffset = other.visualizerOffset;
		this.permittedRotations = other.permittedRotations;
		this.orientation = other.orientation;
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x0009F30C File Offset: 0x0009D50C
	public float GetVisualizerRotation()
	{
		PermittedRotations permittedRotations = this.permittedRotations;
		if (permittedRotations - PermittedRotations.R90 <= 1)
		{
			return -90f * (float)this.orientation;
		}
		return 0f;
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x0009F339 File Offset: 0x0009D539
	public bool GetVisualizerFlipX()
	{
		return this.orientation == Orientation.FlipH;
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x0009F344 File Offset: 0x0009D544
	public bool GetVisualizerFlipY()
	{
		return this.orientation == Orientation.FlipV;
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x0009F350 File Offset: 0x0009D550
	public Vector3 GetVisualizerPivot()
	{
		Vector3 result = this.pivot;
		Orientation orientation = this.orientation;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
			}
		}
		else
		{
			result.x = -this.pivot.x;
		}
		return result;
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x0009F38C File Offset: 0x0009D58C
	private Vector3 GetVisualizerOffset()
	{
		Orientation orientation = this.orientation;
		Vector3 result;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
				result = this.visualizerOffset;
			}
			else
			{
				result = new Vector3(this.visualizerOffset.x, 1f, this.visualizerOffset.z);
			}
		}
		else
		{
			result = new Vector3(-this.visualizerOffset.x, this.visualizerOffset.y, this.visualizerOffset.z);
		}
		return result;
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x0009F400 File Offset: 0x0009D600
	private void OrientVisualizer(Orientation orientation)
	{
		float visualizerRotation = this.GetVisualizerRotation();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Pivot = this.GetVisualizerPivot();
		component.Rotation = visualizerRotation;
		component.Offset = this.GetVisualizerOffset();
		component.FlipX = this.GetVisualizerFlipX();
		component.FlipY = this.GetVisualizerFlipY();
		base.Trigger(-1643076535, this);
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x0009F45C File Offset: 0x0009D65C
	private void OrientCollider(Orientation orientation)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component == null)
		{
			return;
		}
		float num = 0.5f * (float)((this.width + 1) % 2);
		float num2 = 0f;
		switch (orientation)
		{
		case Orientation.R90:
			num2 = -90f;
			goto IL_11B;
		case Orientation.R180:
			num2 = -180f;
			goto IL_11B;
		case Orientation.R270:
			num2 = -270f;
			goto IL_11B;
		case Orientation.FlipH:
			component.offset = new Vector2(num + (float)(this.width % 2) - 1f, 0.5f * (float)this.height);
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		case Orientation.FlipV:
			component.offset = new Vector2(num, -0.5f * (float)(this.height - 2));
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		}
		component.offset = new Vector2(num, 0.5f * (float)this.height);
		component.size = new Vector2((float)this.width, (float)this.height);
		IL_11B:
		if (num2 != 0f)
		{
			Matrix2x3 n = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n2 = Matrix2x3.Rotate(num2 * 0.017453292f);
			Matrix2x3 matrix2x = Matrix2x3.Translate(this.pivot + new Vector3(num, 0f, 0f)) * n2 * n;
			Vector2 vector = new Vector2(-0.5f * (float)this.width, 0f);
			Vector2 vector2 = new Vector2(0.5f * (float)this.width, (float)this.height);
			Vector2 vector3 = new Vector2(0f, 0.5f * (float)this.height);
			vector = matrix2x.MultiplyPoint(vector);
			vector2 = matrix2x.MultiplyPoint(vector2);
			vector3 = matrix2x.MultiplyPoint(vector3);
			float num3 = Mathf.Min(vector.x, vector2.x);
			float num4 = Mathf.Max(vector.x, vector2.x);
			float num5 = Mathf.Min(vector.y, vector2.y);
			float num6 = Mathf.Max(vector.y, vector2.y);
			component.offset = vector3;
			component.size = new Vector2(num4 - num3, num6 - num5);
		}
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x0009F6E4 File Offset: 0x0009D8E4
	public CellOffset GetRotatedCellOffset(CellOffset offset)
	{
		return Rotatable.GetRotatedCellOffset(offset, this.orientation);
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x0009F6F4 File Offset: 0x0009D8F4
	public static CellOffset GetRotatedCellOffset(CellOffset offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new CellOffset(offset.y, -offset.x);
		case Orientation.R180:
			return new CellOffset(-offset.x, -offset.y);
		case Orientation.R270:
			return new CellOffset(-offset.y, offset.x);
		case Orientation.FlipH:
			return new CellOffset(-offset.x, offset.y);
		case Orientation.FlipV:
			return new CellOffset(offset.x, -offset.y);
		}
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x0009F784 File Offset: 0x0009D984
	public static CellOffset GetRotatedCellOffset(int x, int y, Orientation orientation)
	{
		return Rotatable.GetRotatedCellOffset(new CellOffset(x, y), orientation);
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x0009F793 File Offset: 0x0009D993
	public Vector3 GetRotatedOffset(Vector3 offset)
	{
		return Rotatable.GetRotatedOffset(offset, this.orientation);
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x0009F7A4 File Offset: 0x0009D9A4
	public static Vector3 GetRotatedOffset(Vector3 offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector3(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector3(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector3(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector3(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector3(offset.x, -offset.y);
		}
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x0009F834 File Offset: 0x0009DA34
	public Vector2I GetRotatedOffset(Vector2I offset)
	{
		switch (this.orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector2I(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector2I(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector2I(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector2I(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector2I(offset.x, -offset.y);
		}
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x0009F8CB File Offset: 0x0009DACB
	public Orientation GetOrientation()
	{
		return this.orientation;
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x0009F8D3 File Offset: 0x0009DAD3
	public bool IsRotated
	{
		get
		{
			return this.orientation > Orientation.Neutral;
		}
	}

	// Token: 0x040010AA RID: 4266
	[Serialize]
	[SerializeField]
	private Orientation orientation;

	// Token: 0x040010AB RID: 4267
	[SerializeField]
	private Vector3 pivot = Vector3.zero;

	// Token: 0x040010AC RID: 4268
	[SerializeField]
	private Vector3 visualizerOffset = Vector3.zero;

	// Token: 0x040010AD RID: 4269
	public PermittedRotations permittedRotations;

	// Token: 0x040010AE RID: 4270
	[SerializeField]
	private int width;

	// Token: 0x040010AF RID: 4271
	[SerializeField]
	private int height;
}
