using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000986 RID: 2438
[AddComponentMenu("KMonoBehaviour/scripts/Sounds")]
public class Sounds : KMonoBehaviour
{
	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x060047F8 RID: 18424 RVA: 0x001964B4 File Offset: 0x001946B4
	// (set) Token: 0x060047F9 RID: 18425 RVA: 0x001964BB File Offset: 0x001946BB
	public static Sounds Instance { get; private set; }

	// Token: 0x060047FA RID: 18426 RVA: 0x001964C3 File Offset: 0x001946C3
	public static void DestroyInstance()
	{
		Sounds.Instance = null;
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x001964CB File Offset: 0x001946CB
	protected override void OnPrefabInit()
	{
		Sounds.Instance = this;
	}

	// Token: 0x04002FA2 RID: 12194
	public FMODAsset BlowUp_Generic;

	// Token: 0x04002FA3 RID: 12195
	public FMODAsset Build_Generic;

	// Token: 0x04002FA4 RID: 12196
	public FMODAsset InUse_Fabricator;

	// Token: 0x04002FA5 RID: 12197
	public FMODAsset InUse_OxygenGenerator;

	// Token: 0x04002FA6 RID: 12198
	public FMODAsset Place_OreOnSite;

	// Token: 0x04002FA7 RID: 12199
	public FMODAsset Footstep_rock;

	// Token: 0x04002FA8 RID: 12200
	public FMODAsset Ice_crack;

	// Token: 0x04002FA9 RID: 12201
	public FMODAsset BuildingPowerOn;

	// Token: 0x04002FAA RID: 12202
	public FMODAsset ElectricGridOverload;

	// Token: 0x04002FAB RID: 12203
	public FMODAsset IngameMusic;

	// Token: 0x04002FAC RID: 12204
	public FMODAsset[] OreSplashSounds;

	// Token: 0x04002FAE RID: 12206
	public EventReference BlowUp_GenericMigrated;

	// Token: 0x04002FAF RID: 12207
	public EventReference Build_GenericMigrated;

	// Token: 0x04002FB0 RID: 12208
	public EventReference InUse_FabricatorMigrated;

	// Token: 0x04002FB1 RID: 12209
	public EventReference InUse_OxygenGeneratorMigrated;

	// Token: 0x04002FB2 RID: 12210
	public EventReference Place_OreOnSiteMigrated;

	// Token: 0x04002FB3 RID: 12211
	public EventReference Footstep_rockMigrated;

	// Token: 0x04002FB4 RID: 12212
	public EventReference Ice_crackMigrated;

	// Token: 0x04002FB5 RID: 12213
	public EventReference BuildingPowerOnMigrated;

	// Token: 0x04002FB6 RID: 12214
	public EventReference ElectricGridOverloadMigrated;

	// Token: 0x04002FB7 RID: 12215
	public EventReference IngameMusicMigrated;

	// Token: 0x04002FB8 RID: 12216
	public EventReference[] OreSplashSoundsMigrated;
}
