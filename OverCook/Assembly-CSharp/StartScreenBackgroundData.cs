using System;
using UnityEngine;

// Token: 0x02000A49 RID: 2633
[CreateAssetMenu(fileName = "StartScreenBackgroundData", menuName = "Team17/Create Start Screen Background Data")]
[Serializable]
public class StartScreenBackgroundData : SerializedSceneData
{
	// Token: 0x040029BB RID: 10683
	[Header("Start Screen Data")]
	[SerializeField]
	public string BackgroundScene;

	// Token: 0x040029BC RID: 10684
	[SerializeField]
	public HatMeshVisibility.VisState ChefHat = HatMeshVisibility.VisState.Fancy;
}
