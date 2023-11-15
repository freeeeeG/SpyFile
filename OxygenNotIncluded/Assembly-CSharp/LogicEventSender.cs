using System;
using UnityEngine;

// Token: 0x0200084D RID: 2125
internal class LogicEventSender : ILogicEventSender, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06003DF6 RID: 15862 RVA: 0x0015886E File Offset: 0x00156A6E
	public LogicEventSender(HashedString id, int cell, Action<int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.id = id;
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0015889B File Offset: 0x00156A9B
	public HashedString ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x001588A3 File Offset: 0x00156AA3
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x001588AB File Offset: 0x00156AAB
	public int GetLogicValue()
	{
		return this.logicValue;
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x001588B3 File Offset: 0x00156AB3
	public int GetLogicUICell()
	{
		return this.GetLogicCell();
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x001588BB File Offset: 0x00156ABB
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06003DFC RID: 15868 RVA: 0x001588C3 File Offset: 0x00156AC3
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06003DFD RID: 15869 RVA: 0x001588D5 File Offset: 0x00156AD5
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06003DFE RID: 15870 RVA: 0x001588E7 File Offset: 0x00156AE7
	public void SetValue(int value)
	{
		this.logicValue = value;
		this.onValueChanged(value);
	}

	// Token: 0x06003DFF RID: 15871 RVA: 0x001588FC File Offset: 0x00156AFC
	public void LogicTick()
	{
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x001588FE File Offset: 0x00156AFE
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x04002846 RID: 10310
	private HashedString id;

	// Token: 0x04002847 RID: 10311
	private int cell;

	// Token: 0x04002848 RID: 10312
	private int logicValue;

	// Token: 0x04002849 RID: 10313
	private Action<int> onValueChanged;

	// Token: 0x0400284A RID: 10314
	private Action<int, bool> onConnectionChanged;

	// Token: 0x0400284B RID: 10315
	private LogicPortSpriteType spriteType;
}
