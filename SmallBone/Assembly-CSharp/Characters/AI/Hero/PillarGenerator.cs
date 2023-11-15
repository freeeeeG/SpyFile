using System;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001267 RID: 4711
	public class PillarGenerator : MonoBehaviour
	{
		// Token: 0x04004AF8 RID: 19192
		[SerializeField]
		private PillarOfLightContainer _containerPrefab;

		// Token: 0x04004AF9 RID: 19193
		[SerializeField]
		private string _pillarPrefabPath;

		// Token: 0x04004AFA RID: 19194
		[SerializeField]
		private PillarOfLight _pillarPrefab;

		// Token: 0x04004AFB RID: 19195
		[SerializeField]
		private float _distance = 2.1f;

		// Token: 0x04004AFC RID: 19196
		[SerializeField]
		[MinMaxSlider(0f, 90f)]
		private Vector2 _angleRange;

		// Token: 0x04004AFD RID: 19197
		[SerializeField]
		private int _count = 9;
	}
}
