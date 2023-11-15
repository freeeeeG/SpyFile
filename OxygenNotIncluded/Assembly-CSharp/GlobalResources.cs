using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000A4A RID: 2634
public class GlobalResources : ScriptableObject
{
	// Token: 0x06004F53 RID: 20307 RVA: 0x001C0648 File Offset: 0x001BE848
	public static GlobalResources Instance()
	{
		if (GlobalResources._Instance == null)
		{
			GlobalResources._Instance = Resources.Load<GlobalResources>("GlobalResources");
		}
		return GlobalResources._Instance;
	}

	// Token: 0x040033DA RID: 13274
	public Material AnimMaterial;

	// Token: 0x040033DB RID: 13275
	public Material AnimUIMaterial;

	// Token: 0x040033DC RID: 13276
	public Material AnimPlaceMaterial;

	// Token: 0x040033DD RID: 13277
	public Material AnimMaterialUIDesaturated;

	// Token: 0x040033DE RID: 13278
	public Material AnimSimpleMaterial;

	// Token: 0x040033DF RID: 13279
	public Material AnimOverlayMaterial;

	// Token: 0x040033E0 RID: 13280
	public Texture2D WhiteTexture;

	// Token: 0x040033E1 RID: 13281
	public EventReference ConduitOverlaySoundLiquid;

	// Token: 0x040033E2 RID: 13282
	public EventReference ConduitOverlaySoundGas;

	// Token: 0x040033E3 RID: 13283
	public EventReference ConduitOverlaySoundSolid;

	// Token: 0x040033E4 RID: 13284
	public EventReference AcousticDisturbanceSound;

	// Token: 0x040033E5 RID: 13285
	public EventReference AcousticDisturbanceBubbleSound;

	// Token: 0x040033E6 RID: 13286
	public EventReference WallDamageLayerSound;

	// Token: 0x040033E7 RID: 13287
	public Sprite sadDupeAudio;

	// Token: 0x040033E8 RID: 13288
	public Sprite sadDupe;

	// Token: 0x040033E9 RID: 13289
	public Sprite baseGameLogoSmall;

	// Token: 0x040033EA RID: 13290
	public Sprite expansion1LogoSmall;

	// Token: 0x040033EB RID: 13291
	private static GlobalResources _Instance;
}
