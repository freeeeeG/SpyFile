using System;

// Token: 0x0200074B RID: 1867
public enum BuildLocationRule
{
	// Token: 0x04001F05 RID: 7941
	Anywhere,
	// Token: 0x04001F06 RID: 7942
	OnFloor,
	// Token: 0x04001F07 RID: 7943
	OnFloorOverSpace,
	// Token: 0x04001F08 RID: 7944
	OnCeiling,
	// Token: 0x04001F09 RID: 7945
	OnWall,
	// Token: 0x04001F0A RID: 7946
	InCorner,
	// Token: 0x04001F0B RID: 7947
	Tile,
	// Token: 0x04001F0C RID: 7948
	NotInTiles,
	// Token: 0x04001F0D RID: 7949
	Conduit,
	// Token: 0x04001F0E RID: 7950
	LogicBridge,
	// Token: 0x04001F0F RID: 7951
	WireBridge,
	// Token: 0x04001F10 RID: 7952
	HighWattBridgeTile,
	// Token: 0x04001F11 RID: 7953
	BuildingAttachPoint,
	// Token: 0x04001F12 RID: 7954
	OnFloorOrBuildingAttachPoint,
	// Token: 0x04001F13 RID: 7955
	OnFoundationRotatable,
	// Token: 0x04001F14 RID: 7956
	BelowRocketCeiling,
	// Token: 0x04001F15 RID: 7957
	OnRocketEnvelope,
	// Token: 0x04001F16 RID: 7958
	WallFloor,
	// Token: 0x04001F17 RID: 7959
	NoLiquidConduitAtOrigin
}
