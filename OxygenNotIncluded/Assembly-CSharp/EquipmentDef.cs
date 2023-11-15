using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x0200079D RID: 1949
public class EquipmentDef : Def
{
	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06003631 RID: 13873 RVA: 0x00125058 File Offset: 0x00123258
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06003632 RID: 13874 RVA: 0x0012507E File Offset: 0x0012327E
	public string GenericName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06003633 RID: 13875 RVA: 0x001250A4 File Offset: 0x001232A4
	public string WornName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_NAME");
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06003634 RID: 13876 RVA: 0x001250CA File Offset: 0x001232CA
	public string WornDesc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_DESC");
		}
	}

	// Token: 0x04002105 RID: 8453
	public string Id;

	// Token: 0x04002106 RID: 8454
	public string Slot;

	// Token: 0x04002107 RID: 8455
	public string FabricatorId;

	// Token: 0x04002108 RID: 8456
	public float FabricationTime;

	// Token: 0x04002109 RID: 8457
	public string RecipeTechUnlock;

	// Token: 0x0400210A RID: 8458
	public SimHashes OutputElement;

	// Token: 0x0400210B RID: 8459
	public Dictionary<string, float> InputElementMassMap;

	// Token: 0x0400210C RID: 8460
	public float Mass;

	// Token: 0x0400210D RID: 8461
	public KAnimFile Anim;

	// Token: 0x0400210E RID: 8462
	public string SnapOn;

	// Token: 0x0400210F RID: 8463
	public string SnapOn1;

	// Token: 0x04002110 RID: 8464
	public KAnimFile BuildOverride;

	// Token: 0x04002111 RID: 8465
	public int BuildOverridePriority;

	// Token: 0x04002112 RID: 8466
	public bool IsBody;

	// Token: 0x04002113 RID: 8467
	public List<AttributeModifier> AttributeModifiers;

	// Token: 0x04002114 RID: 8468
	public string RecipeDescription;

	// Token: 0x04002115 RID: 8469
	public List<Effect> EffectImmunites = new List<Effect>();

	// Token: 0x04002116 RID: 8470
	public Action<Equippable> OnEquipCallBack;

	// Token: 0x04002117 RID: 8471
	public Action<Equippable> OnUnequipCallBack;

	// Token: 0x04002118 RID: 8472
	public EntityTemplates.CollisionShape CollisionShape;

	// Token: 0x04002119 RID: 8473
	public float width;

	// Token: 0x0400211A RID: 8474
	public float height = 0.325f;

	// Token: 0x0400211B RID: 8475
	public Tag[] AdditionalTags;

	// Token: 0x0400211C RID: 8476
	public string wornID;

	// Token: 0x0400211D RID: 8477
	public List<Descriptor> additionalDescriptors = new List<Descriptor>();
}
