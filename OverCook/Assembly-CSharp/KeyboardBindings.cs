using System;

// Token: 0x020001F4 RID: 500
public class KeyboardBindings
{
	// Token: 0x0600084E RID: 2126 RVA: 0x00032517 File Offset: 0x00030917
	public KeyboardBindings(string name)
	{
		this.m_Name = name;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00032547 File Offset: 0x00030947
	public void CopyFrom(KeyboardBindings original)
	{
		this.m_CombinedKeyboard.CopyFrom(original.m_CombinedKeyboard);
		this.m_SplitKeyboard.CopyFrom(original.m_SplitKeyboard);
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0003256B File Offset: 0x0003096B
	public void Save(GlobalSave saveData)
	{
		this.m_CombinedKeyboard.Save(saveData, this.m_Name + "_Combined");
		this.m_SplitKeyboard.Save(saveData, this.m_Name + "_Split");
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x000325A8 File Offset: 0x000309A8
	public bool Load(GlobalSave saveData)
	{
		bool flag = true;
		flag &= this.m_CombinedKeyboard.Load(saveData, this.m_Name + "_Combined");
		return flag & this.m_SplitKeyboard.Load(saveData, this.m_Name + "_Split");
	}

	// Token: 0x04000706 RID: 1798
	public KeyboardBindingSet m_CombinedKeyboard = new KeyboardBindingSet();

	// Token: 0x04000707 RID: 1799
	public KeyboardBindingSet m_SplitKeyboard = new KeyboardBindingSet();

	// Token: 0x04000708 RID: 1800
	private string m_Name = "UNSET";
}
