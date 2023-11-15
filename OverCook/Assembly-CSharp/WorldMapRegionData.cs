using System;
using UnityEngine;

// Token: 0x02000BBF RID: 3007
[CreateAssetMenu(fileName = "WorldMapRegionDataData", menuName = "Team17/Create World Map Region Data")]
[Serializable]
public class WorldMapRegionData : SerializedSceneData
{
	// Token: 0x0400316D RID: 12653
	[Header("World Map Region Data")]
	[SerializeField]
	public SerializedSceneData.SkyColours SkyColourSettings = new SerializedSceneData.SkyColours();
}
