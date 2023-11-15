using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200058F RID: 1423
public class ArtifactSelector : KMonoBehaviour
{
	// Token: 0x17000191 RID: 401
	// (get) Token: 0x0600226A RID: 8810 RVA: 0x000BD193 File Offset: 0x000BB393
	public int AnalyzedArtifactCount
	{
		get
		{
			return this.analyzedArtifactCount;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x0600226B RID: 8811 RVA: 0x000BD19B File Offset: 0x000BB39B
	public int AnalyzedSpaceArtifactCount
	{
		get
		{
			return this.analyzedSpaceArtifactCount;
		}
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x000BD1A3 File Offset: 0x000BB3A3
	public List<string> GetAnalyzedArtifactIDs()
	{
		return this.analyzedArtifatIDs;
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x000BD1AC File Offset: 0x000BB3AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ArtifactSelector.Instance = this;
		this.placedArtifacts.Add(ArtifactType.Terrestrial, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Space, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Any, new List<string>());
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000BD1F8 File Offset: 0x000BB3F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = 0;
		int num2 = 0;
		foreach (string artifactID in this.analyzedArtifatIDs)
		{
			ArtifactType artifactType = this.GetArtifactType(artifactID);
			if (artifactType != ArtifactType.Space)
			{
				if (artifactType == ArtifactType.Terrestrial)
				{
					num++;
				}
			}
			else
			{
				num2++;
			}
		}
		if (num > this.analyzedArtifactCount)
		{
			this.analyzedArtifactCount = num;
		}
		if (num2 > this.analyzedSpaceArtifactCount)
		{
			this.analyzedSpaceArtifactCount = num2;
		}
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x000BD28C File Offset: 0x000BB48C
	public bool RecordArtifactAnalyzed(string id)
	{
		if (this.analyzedArtifatIDs.Contains(id))
		{
			return false;
		}
		this.analyzedArtifatIDs.Add(id);
		return true;
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000BD2AB File Offset: 0x000BB4AB
	public void IncrementAnalyzedTerrestrialArtifacts()
	{
		this.analyzedArtifactCount++;
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x000BD2BB File Offset: 0x000BB4BB
	public void IncrementAnalyzedSpaceArtifacts()
	{
		this.analyzedSpaceArtifactCount++;
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x000BD2CC File Offset: 0x000BB4CC
	public string GetUniqueArtifactID(ArtifactType artifactType = ArtifactType.Any)
	{
		List<string> list = new List<string>();
		foreach (string item in ArtifactConfig.artifactItems[artifactType])
		{
			if (!this.placedArtifacts[artifactType].Contains(item))
			{
				list.Add(item);
			}
		}
		string text = "artifact_officemug";
		if (list.Count == 0 && artifactType != ArtifactType.Any)
		{
			foreach (string item2 in ArtifactConfig.artifactItems[ArtifactType.Any])
			{
				if (!this.placedArtifacts[ArtifactType.Any].Contains(item2))
				{
					list.Add(item2);
					artifactType = ArtifactType.Any;
				}
			}
		}
		if (list.Count != 0)
		{
			text = list[UnityEngine.Random.Range(0, list.Count)];
		}
		this.placedArtifacts[artifactType].Add(text);
		return text;
	}

	// Token: 0x06002273 RID: 8819 RVA: 0x000BD3E0 File Offset: 0x000BB5E0
	public void ReserveArtifactID(string artifactID, ArtifactType artifactType = ArtifactType.Any)
	{
		if (this.placedArtifacts[artifactType].Contains(artifactID))
		{
			DebugUtil.Assert(true, string.Format("Tried to add {0} to placedArtifacts but it already exists in the list!", artifactID));
		}
		this.placedArtifacts[artifactType].Add(artifactID);
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x000BD419 File Offset: 0x000BB619
	public ArtifactType GetArtifactType(string artifactID)
	{
		if (this.placedArtifacts[ArtifactType.Terrestrial].Contains(artifactID))
		{
			return ArtifactType.Terrestrial;
		}
		if (this.placedArtifacts[ArtifactType.Space].Contains(artifactID))
		{
			return ArtifactType.Space;
		}
		return ArtifactType.Any;
	}

	// Token: 0x040013A7 RID: 5031
	public static ArtifactSelector Instance;

	// Token: 0x040013A8 RID: 5032
	[Serialize]
	private Dictionary<ArtifactType, List<string>> placedArtifacts = new Dictionary<ArtifactType, List<string>>();

	// Token: 0x040013A9 RID: 5033
	[Serialize]
	private int analyzedArtifactCount;

	// Token: 0x040013AA RID: 5034
	[Serialize]
	private int analyzedSpaceArtifactCount;

	// Token: 0x040013AB RID: 5035
	[Serialize]
	private List<string> analyzedArtifatIDs = new List<string>();

	// Token: 0x040013AC RID: 5036
	private const string DEFAULT_ARTIFACT_ID = "artifact_officemug";
}
