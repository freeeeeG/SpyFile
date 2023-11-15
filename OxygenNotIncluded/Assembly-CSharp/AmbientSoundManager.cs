using System;
using UnityEngine;

// Token: 0x02000586 RID: 1414
[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06002238 RID: 8760 RVA: 0x000BBFC1 File Offset: 0x000BA1C1
	// (set) Token: 0x06002239 RID: 8761 RVA: 0x000BBFC8 File Offset: 0x000BA1C8
	public static AmbientSoundManager Instance { get; private set; }

	// Token: 0x0600223A RID: 8762 RVA: 0x000BBFD0 File Offset: 0x000BA1D0
	public static void Destroy()
	{
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000BBFD8 File Offset: 0x000BA1D8
	protected override void OnPrefabInit()
	{
		AmbientSoundManager.Instance = this;
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000BBFE0 File Offset: 0x000BA1E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000BBFE8 File Offset: 0x000BA1E8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x0400137A RID: 4986
	[MyCmpAdd]
	private LoopingSounds loopingSounds;
}
