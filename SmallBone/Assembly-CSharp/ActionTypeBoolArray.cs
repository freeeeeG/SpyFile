using System;
using Characters.Actions;

// Token: 0x020000B5 RID: 181
[Serializable]
public class ActionTypeBoolArray : EnumArray<Characters.Actions.Action.Type, bool>
{
	// Token: 0x06000389 RID: 905 RVA: 0x0000CEEB File Offset: 0x0000B0EB
	public ActionTypeBoolArray()
	{
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0000CEF3 File Offset: 0x0000B0F3
	public ActionTypeBoolArray(EnumArray<Characters.Actions.Action.Type, bool> defaultValue) : base(defaultValue)
	{
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00002191 File Offset: 0x00000391
	public void GetOrDefault()
	{
	}
}
