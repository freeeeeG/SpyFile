using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class ParticleEffectsLibrary : MonoBehaviour
{
	// Token: 0x06000044 RID: 68 RVA: 0x00003450 File Offset: 0x00001650
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

	// Token: 0x06000045 RID: 69 RVA: 0x00003511 File Offset: 0x00001711
	private void Start()
	{
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003514 File Offset: 0x00001714
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

	// Token: 0x06000047 RID: 71 RVA: 0x00003578 File Offset: 0x00001778
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

	// Token: 0x06000048 RID: 72 RVA: 0x00003684 File Offset: 0x00001884
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

	// Token: 0x06000049 RID: 73 RVA: 0x00003790 File Offset: 0x00001990
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

	// Token: 0x0400001A RID: 26
	public static ParticleEffectsLibrary GlobalAccess;

	// Token: 0x0400001B RID: 27
	public int TotalEffects;

	// Token: 0x0400001C RID: 28
	public int CurrentParticleEffectIndex;

	// Token: 0x0400001D RID: 29
	public int CurrentParticleEffectNum;

	// Token: 0x0400001E RID: 30
	public Vector3[] ParticleEffectSpawnOffsets;

	// Token: 0x0400001F RID: 31
	public float[] ParticleEffectLifetimes;

	// Token: 0x04000020 RID: 32
	public GameObject[] ParticleEffectPrefabs;

	// Token: 0x04000021 RID: 33
	private string effectNameString = "";

	// Token: 0x04000022 RID: 34
	private List<Transform> currentActivePEList;

	// Token: 0x04000023 RID: 35
	private Vector3 spawnPosition = Vector3.zero;
}
