using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
	// Token: 0x06001B48 RID: 6984 RVA: 0x00092A2B File Offset: 0x00090C2B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("Facing", 35);
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x00092A48 File Offset: 0x00090C48
	public void Face(float target_x)
	{
		float x = base.transform.GetLocalPosition().x;
		if (target_x < x)
		{
			this.facingLeft = true;
			this.UpdateMirror();
			return;
		}
		if (target_x > x)
		{
			this.facingLeft = false;
			this.UpdateMirror();
		}
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x00092A8C File Offset: 0x00090C8C
	public void Face(Vector3 target_pos)
	{
		int num = Grid.CellColumn(Grid.PosToCell(base.transform.GetLocalPosition()));
		int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
		if (num > num2)
		{
			this.facingLeft = true;
			this.UpdateMirror();
			return;
		}
		if (num2 > num)
		{
			this.facingLeft = false;
			this.UpdateMirror();
		}
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x00092ADE File Offset: 0x00090CDE
	[ContextMenu("Flip")]
	public void SwapFacing()
	{
		this.facingLeft = !this.facingLeft;
		this.UpdateMirror();
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x00092AF5 File Offset: 0x00090CF5
	private void UpdateMirror()
	{
		if (this.kanimController != null && this.kanimController.FlipX != this.facingLeft)
		{
			this.kanimController.FlipX = this.facingLeft;
			bool flag = this.facingLeft;
		}
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x00092B30 File Offset: 0x00090D30
	public bool GetFacing()
	{
		return this.facingLeft;
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x00092B38 File Offset: 0x00090D38
	public void SetFacing(bool mirror_x)
	{
		this.facingLeft = mirror_x;
		this.UpdateMirror();
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x00092B48 File Offset: 0x00090D48
	public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x00092B74 File Offset: 0x00090D74
	public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x04000F2B RID: 3883
	[MyCmpGet]
	private KAnimControllerBase kanimController;

	// Token: 0x04000F2C RID: 3884
	private LoggerFS log;

	// Token: 0x04000F2D RID: 3885
	private bool facingLeft;
}
