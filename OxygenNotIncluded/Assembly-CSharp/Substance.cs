using System;
using FMODUnity;
using Klei;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020009F7 RID: 2551
[Serializable]
public class Substance
{
	// Token: 0x06004C49 RID: 19529 RVA: 0x001AC178 File Offset: 0x001AA378
	public GameObject SpawnResource(Vector3 position, float mass, float temperature, byte disease_idx, int disease_count, bool prevent_merge = false, bool forceTemperature = false, bool manual_activation = false)
	{
		GameObject gameObject = null;
		PrimaryElement primaryElement = null;
		if (!prevent_merge)
		{
			int cell = Grid.PosToCell(position);
			GameObject gameObject2 = Grid.Objects[cell, 3];
			if (gameObject2 != null)
			{
				Pickupable component = gameObject2.GetComponent<Pickupable>();
				if (component != null)
				{
					Tag b = GameTagExtensions.Create(this.elementID);
					for (ObjectLayerListItem objectLayerListItem = component.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
					{
						KPrefabID component2 = objectLayerListItem.gameObject.GetComponent<KPrefabID>();
						if (component2.PrefabTag == b)
						{
							PrimaryElement component3 = component2.GetComponent<PrimaryElement>();
							if (component3.Mass + mass <= PrimaryElement.MAX_MASS)
							{
								gameObject = component2.gameObject;
								primaryElement = component3;
								temperature = SimUtil.CalculateFinalTemperature(primaryElement.Mass, primaryElement.Temperature, mass, temperature);
								position = gameObject.transform.GetPosition();
								break;
							}
						}
					}
				}
			}
		}
		if (gameObject == null)
		{
			gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.nameTag), Grid.SceneLayer.Ore, null, 0);
			primaryElement = gameObject.GetComponent<PrimaryElement>();
			primaryElement.Mass = mass;
		}
		else
		{
			global::Debug.Assert(primaryElement != null);
			Pickupable component4 = primaryElement.GetComponent<Pickupable>();
			if (component4 != null)
			{
				component4.TotalAmount += mass / primaryElement.MassPerUnit;
			}
			else
			{
				primaryElement.Mass += mass;
			}
		}
		primaryElement.InternalTemperature = temperature;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		gameObject.transform.SetPosition(position);
		if (!manual_activation)
		{
			this.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
		}
		return gameObject;
	}

	// Token: 0x06004C4A RID: 19530 RVA: 0x001AC2F4 File Offset: 0x001AA4F4
	public void ActivateSubstanceGameObject(GameObject obj, byte disease_idx, int disease_count)
	{
		obj.SetActive(true);
		obj.GetComponent<PrimaryElement>().AddDisease(disease_idx, disease_count, "Substances.SpawnResource");
	}

	// Token: 0x06004C4B RID: 19531 RVA: 0x001AC310 File Offset: 0x001AA510
	private void SetTexture(MaterialPropertyBlock block, string texture_name)
	{
		Texture texture = this.material.GetTexture(texture_name);
		if (texture != null)
		{
			this.propertyBlock.SetTexture(texture_name, texture);
		}
	}

	// Token: 0x06004C4C RID: 19532 RVA: 0x001AC340 File Offset: 0x001AA540
	public void RefreshPropertyBlock()
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		if (this.material != null)
		{
			this.SetTexture(this.propertyBlock, "_MainTex");
			float @float = this.material.GetFloat("_WorldUVScale");
			this.propertyBlock.SetFloat("_WorldUVScale", @float);
			if (ElementLoader.FindElementByHash(this.elementID).IsSolid)
			{
				this.SetTexture(this.propertyBlock, "_MainTex2");
				this.SetTexture(this.propertyBlock, "_HeightTex2");
				this.propertyBlock.SetFloat("_Frequency", this.material.GetFloat("_Frequency"));
				this.propertyBlock.SetColor("_ShineColour", this.material.GetColor("_ShineColour"));
				this.propertyBlock.SetColor("_ColourTint", this.material.GetColor("_ColourTint"));
			}
		}
	}

	// Token: 0x06004C4D RID: 19533 RVA: 0x001AC43B File Offset: 0x001AA63B
	internal AmbienceType GetAmbience()
	{
		if (this.audioConfig == null)
		{
			return AmbienceType.None;
		}
		return this.audioConfig.ambienceType;
	}

	// Token: 0x06004C4E RID: 19534 RVA: 0x001AC452 File Offset: 0x001AA652
	internal SolidAmbienceType GetSolidAmbience()
	{
		if (this.audioConfig == null)
		{
			return SolidAmbienceType.None;
		}
		return this.audioConfig.solidAmbienceType;
	}

	// Token: 0x06004C4F RID: 19535 RVA: 0x001AC469 File Offset: 0x001AA669
	internal string GetMiningSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningSound;
	}

	// Token: 0x06004C50 RID: 19536 RVA: 0x001AC484 File Offset: 0x001AA684
	internal string GetMiningBreakSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningBreakSound;
	}

	// Token: 0x06004C51 RID: 19537 RVA: 0x001AC49F File Offset: 0x001AA69F
	internal string GetOreBumpSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.oreBumpSound;
	}

	// Token: 0x06004C52 RID: 19538 RVA: 0x001AC4BA File Offset: 0x001AA6BA
	internal string GetFloorEventAudioCategory()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.floorEventAudioCategory;
	}

	// Token: 0x06004C53 RID: 19539 RVA: 0x001AC4D5 File Offset: 0x001AA6D5
	internal string GetCreatureChewSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.creatureChewSound;
	}

	// Token: 0x040031C2 RID: 12738
	public string name;

	// Token: 0x040031C3 RID: 12739
	public SimHashes elementID;

	// Token: 0x040031C4 RID: 12740
	internal Tag nameTag;

	// Token: 0x040031C5 RID: 12741
	public Color32 colour;

	// Token: 0x040031C6 RID: 12742
	[FormerlySerializedAs("debugColour")]
	public Color32 uiColour;

	// Token: 0x040031C7 RID: 12743
	[FormerlySerializedAs("overlayColour")]
	public Color32 conduitColour = Color.white;

	// Token: 0x040031C8 RID: 12744
	[NonSerialized]
	internal bool renderedByWorld;

	// Token: 0x040031C9 RID: 12745
	[NonSerialized]
	internal int idx;

	// Token: 0x040031CA RID: 12746
	public Material material;

	// Token: 0x040031CB RID: 12747
	public KAnimFile anim;

	// Token: 0x040031CC RID: 12748
	[SerializeField]
	internal bool showInEditor = true;

	// Token: 0x040031CD RID: 12749
	[NonSerialized]
	internal KAnimFile[] anims;

	// Token: 0x040031CE RID: 12750
	[NonSerialized]
	internal ElementsAudio.ElementAudioConfig audioConfig;

	// Token: 0x040031CF RID: 12751
	[NonSerialized]
	internal MaterialPropertyBlock propertyBlock;

	// Token: 0x040031D0 RID: 12752
	public EventReference fallingStartSound;

	// Token: 0x040031D1 RID: 12753
	public EventReference fallingStopSound;
}
