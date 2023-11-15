using System;
using System.Collections.Generic;

// Token: 0x020001F2 RID: 498
public class PadAssignmentDictionary
{
	// Token: 0x06000843 RID: 2115 RVA: 0x000320B4 File Offset: 0x000304B4
	public PadAssignmentDictionary()
	{
		for (int i = 0; i < 15; i++)
		{
			ControlPadInput.PadNum padNum = (ControlPadInput.PadNum)i;
			this.s_padMapping.Add(padNum, padNum);
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x000320F4 File Offset: 0x000304F4
	public void RemapPad(ControlPadInput.PadNum _oldPadNum, ControlPadInput.PadNum _newPadNum)
	{
		ControlPadInput.PadNum value = this.s_padMapping[_oldPadNum];
		ControlPadInput.PadNum value2 = this.s_padMapping[_newPadNum];
		this.s_padMapping[_oldPadNum] = value2;
		this.s_padMapping[_newPadNum] = value;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00032135 File Offset: 0x00030535
	public ControlPadInput.PadNum GetMappedPad(ControlPadInput.PadNum _requestedPadNum)
	{
		return this.s_padMapping[_requestedPadNum];
	}

	// Token: 0x04000702 RID: 1794
	private Dictionary<ControlPadInput.PadNum, ControlPadInput.PadNum> s_padMapping = new Dictionary<ControlPadInput.PadNum, ControlPadInput.PadNum>();
}
