using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x0200029D RID: 669
[Serializable]
public struct ModInfo
{
	// Token: 0x040007DE RID: 2014
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.Source source;

	// Token: 0x040007DF RID: 2015
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.ModType type;

	// Token: 0x040007E0 RID: 2016
	public string assetID;

	// Token: 0x040007E1 RID: 2017
	public string assetPath;

	// Token: 0x040007E2 RID: 2018
	public bool enabled;

	// Token: 0x040007E3 RID: 2019
	public bool markedForDelete;

	// Token: 0x040007E4 RID: 2020
	public bool markedForUpdate;

	// Token: 0x040007E5 RID: 2021
	public string description;

	// Token: 0x040007E6 RID: 2022
	public ulong lastModifiedTime;

	// Token: 0x02000F78 RID: 3960
	public enum Source
	{
		// Token: 0x04005601 RID: 22017
		Local,
		// Token: 0x04005602 RID: 22018
		Steam,
		// Token: 0x04005603 RID: 22019
		Rail
	}

	// Token: 0x02000F79 RID: 3961
	public enum ModType
	{
		// Token: 0x04005605 RID: 22021
		WorldGen,
		// Token: 0x04005606 RID: 22022
		Scenario,
		// Token: 0x04005607 RID: 22023
		Mod
	}
}
