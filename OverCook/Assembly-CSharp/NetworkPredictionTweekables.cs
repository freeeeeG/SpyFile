using System;
using UnityEngine;

// Token: 0x020008EE RID: 2286
[CreateAssetMenu(fileName = "NetworkPredictionTweekables", menuName = "Team17/Create Network Prediction Tweekables")]
public class NetworkPredictionTweekables : ScriptableObject
{
	// Token: 0x040023C5 RID: 9157
	public float ChefRadius = 0.4f;

	// Token: 0x040023C6 RID: 9158
	public float PenetrationAngle = 0.65f;

	// Token: 0x040023C7 RID: 9159
	public float PenitrationMinSpeed = 7.5f;

	// Token: 0x040023C8 RID: 9160
	public float ChefMovingTowardsUsAngle = 0.4f;

	// Token: 0x040023C9 RID: 9161
	public float LerpDistanceFactor = 0.4f;

	// Token: 0x040023CA RID: 9162
	public float LerpDistanceFactorMax = 32f;

	// Token: 0x040023CB RID: 9163
	public float LerpTime = 0.25f;

	// Token: 0x040023CC RID: 9164
	public float LerpFactorMax = 1.1f;

	// Token: 0x040023CD RID: 9165
	public float LerpMinimumSpeed = 2f;

	// Token: 0x040023CE RID: 9166
	public float RotationSpeed = 720f;
}
