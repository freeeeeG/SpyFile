using System;
using UnityEngine;

// Token: 0x02000851 RID: 2129
public class LogicPortVisualizer : ILogicUIElement, IUniformGridObject
{
	// Token: 0x06003E3B RID: 15931 RVA: 0x0015A164 File Offset: 0x00158364
	public LogicPortVisualizer(int cell, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.spriteType = sprite_type;
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x0015A17A File Offset: 0x0015837A
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x0015A182 File Offset: 0x00158382
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x0015A194 File Offset: 0x00158394
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x0015A1A6 File Offset: 0x001583A6
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x04002868 RID: 10344
	private int cell;

	// Token: 0x04002869 RID: 10345
	private LogicPortSpriteType spriteType;
}
