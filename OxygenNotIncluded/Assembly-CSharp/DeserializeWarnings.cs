using System;
using UnityEngine;

// Token: 0x0200074E RID: 1870
[AddComponentMenu("KMonoBehaviour/scripts/DeserializeWarnings")]
public class DeserializeWarnings : KMonoBehaviour
{
	// Token: 0x060033F2 RID: 13298 RVA: 0x00116362 File Offset: 0x00114562
	public static void DestroyInstance()
	{
		DeserializeWarnings.Instance = null;
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x0011636A File Offset: 0x0011456A
	protected override void OnPrefabInit()
	{
		DeserializeWarnings.Instance = this;
	}

	// Token: 0x04001F8F RID: 8079
	public DeserializeWarnings.Warning BuildingTemeperatureIsZeroKelvin;

	// Token: 0x04001F90 RID: 8080
	public DeserializeWarnings.Warning PipeContentsTemperatureIsNan;

	// Token: 0x04001F91 RID: 8081
	public DeserializeWarnings.Warning PrimaryElementTemperatureIsNan;

	// Token: 0x04001F92 RID: 8082
	public DeserializeWarnings.Warning PrimaryElementHasNoElement;

	// Token: 0x04001F93 RID: 8083
	public static DeserializeWarnings Instance;

	// Token: 0x020014E6 RID: 5350
	public struct Warning
	{
		// Token: 0x06008633 RID: 34355 RVA: 0x00308225 File Offset: 0x00306425
		public void Warn(string message, GameObject obj = null)
		{
			if (!this.isSet)
			{
				global::Debug.LogWarning(message, obj);
				this.isSet = true;
			}
		}

		// Token: 0x040066C6 RID: 26310
		private bool isSet;
	}
}
