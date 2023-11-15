using System;

// Token: 0x02000825 RID: 2085
[Serializable]
public class PlayerGameInput
{
	// Token: 0x060027FC RID: 10236 RVA: 0x000BB7E8 File Offset: 0x000B9BE8
	public PlayerGameInput(ControlPadInput.PadNum _pad, PadSide _side, AmbiControlsMappingData _mappingData)
	{
		this.Pad = _pad;
		this.Side = _side;
		this.AmbiControlsMapping = _mappingData;
	}

	// Token: 0x04001F89 RID: 8073
	public ControlPadInput.PadNum Pad;

	// Token: 0x04001F8A RID: 8074
	public PadSide Side = PadSide.Both;

	// Token: 0x04001F8B RID: 8075
	public AmbiControlsMappingData AmbiControlsMapping;
}
