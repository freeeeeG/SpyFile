using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETFXPEL
{
	// Token: 0x0200005A RID: 90
	public class ParticleEffectsLibrary : MonoBehaviour
	{
		// Token: 0x0600010D RID: 269 RVA: 0x0000588C File Offset: 0x00003A8C
		private void Awake()
		{
			ParticleEffectsLibrary.GlobalAccess = this;
			this.currentActivePEList = new List<Transform>();
			this.TotalEffects = this.ParticleEffectPrefabs.Length;
			this.CurrentParticleEffectNum = 1;
			if (this.ParticleEffectSpawnOffsets.Length != this.TotalEffects)
			{
				Debug.LogError("ParticleEffectsLibrary-ParticleEffectSpawnOffset: Not all arrays match length, double check counts.");
			}
			if (this.ParticleEffectPrefabs.Length != this.TotalEffects)
			{
				Debug.LogError("ParticleEffectsLibrary-ParticleEffectPrefabs: Not all arrays match length, double check counts.");
			}
			this.effectNameString = string.Concat(new string[]
			{
				this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name,
				" (",
				this.CurrentParticleEffectNum.ToString(),
				" of ",
				this.TotalEffects.ToString(),
				")"
			});
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000594D File Offset: 0x00003B4D
		private void Start()
		{
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005950 File Offset: 0x00003B50
		public string GetCurrentPENameString()
		{
			return string.Concat(new string[]
			{
				this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name,
				" (",
				this.CurrentParticleEffectNum.ToString(),
				" of ",
				this.TotalEffects.ToString(),
				")"
			});
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000059B4 File Offset: 0x00003BB4
		public void PreviousParticleEffect()
		{
			if (this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0f && this.currentActivePEList.Count > 0)
			{
				for (int i = 0; i < this.currentActivePEList.Count; i++)
				{
					if (this.currentActivePEList[i] != null)
					{
						Object.Destroy(this.currentActivePEList[i].gameObject);
					}
				}
				this.currentActivePEList.Clear();
			}
			if (this.CurrentParticleEffectIndex > 0)
			{
				this.CurrentParticleEffectIndex--;
			}
			else
			{
				this.CurrentParticleEffectIndex = this.TotalEffects - 1;
			}
			this.CurrentParticleEffectNum = this.CurrentParticleEffectIndex + 1;
			this.effectNameString = string.Concat(new string[]
			{
				this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name,
				" (",
				this.CurrentParticleEffectNum.ToString(),
				" of ",
				this.TotalEffects.ToString(),
				")"
			});
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public void NextParticleEffect()
		{
			if (this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0f && this.currentActivePEList.Count > 0)
			{
				for (int i = 0; i < this.currentActivePEList.Count; i++)
				{
					if (this.currentActivePEList[i] != null)
					{
						Object.Destroy(this.currentActivePEList[i].gameObject);
					}
				}
				this.currentActivePEList.Clear();
			}
			if (this.CurrentParticleEffectIndex < this.TotalEffects - 1)
			{
				this.CurrentParticleEffectIndex++;
			}
			else
			{
				this.CurrentParticleEffectIndex = 0;
			}
			this.CurrentParticleEffectNum = this.CurrentParticleEffectIndex + 1;
			this.effectNameString = string.Concat(new string[]
			{
				this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name,
				" (",
				this.CurrentParticleEffectNum.ToString(),
				" of ",
				this.TotalEffects.ToString(),
				")"
			});
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005BCC File Offset: 0x00003DCC
		public void SpawnParticleEffect(Vector3 positionInWorldToSpawn)
		{
			this.spawnPosition = positionInWorldToSpawn + this.ParticleEffectSpawnOffsets[this.CurrentParticleEffectIndex];
			GameObject gameObject = Object.Instantiate<GameObject>(this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex], this.spawnPosition, this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].transform.rotation);
			Object @object = gameObject;
			string str = "PE_";
			GameObject gameObject2 = this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex];
			@object.name = str + ((gameObject2 != null) ? gameObject2.ToString() : null);
			if (this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0f)
			{
				this.currentActivePEList.Add(gameObject.transform);
			}
			this.currentActivePEList.Add(gameObject.transform);
			if (this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] != 0f)
			{
				Object.Destroy(gameObject, this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex]);
			}
		}

		// Token: 0x0400010A RID: 266
		public static ParticleEffectsLibrary GlobalAccess;

		// Token: 0x0400010B RID: 267
		public int TotalEffects;

		// Token: 0x0400010C RID: 268
		public int CurrentParticleEffectIndex;

		// Token: 0x0400010D RID: 269
		public int CurrentParticleEffectNum;

		// Token: 0x0400010E RID: 270
		public Vector3[] ParticleEffectSpawnOffsets;

		// Token: 0x0400010F RID: 271
		public float[] ParticleEffectLifetimes;

		// Token: 0x04000110 RID: 272
		public GameObject[] ParticleEffectPrefabs;

		// Token: 0x04000111 RID: 273
		private string effectNameString = "";

		// Token: 0x04000112 RID: 274
		private List<Transform> currentActivePEList;

		// Token: 0x04000113 RID: 275
		private Vector3 spawnPosition = Vector3.zero;
	}
}
