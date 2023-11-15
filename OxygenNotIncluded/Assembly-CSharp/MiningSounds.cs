using System;
using FMODUnity;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
	// Token: 0x06001C44 RID: 7236 RVA: 0x0009726F File Offset: 0x0009546F
	protected override void OnPrefabInit()
	{
		base.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
		base.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x00097294 File Offset: 0x00095494
	private void OnStartMiningSound(object data)
	{
		if (this.miningSound == null)
		{
			Element element = data as Element;
			if (element != null)
			{
				string text = element.substance.GetMiningSound();
				if (text == null || text == "")
				{
					return;
				}
				text = "Mine_" + text;
				string sound = GlobalAssets.GetSound(text, false);
				this.miningSoundEvent = RuntimeManager.PathToEventReference(sound);
				if (!this.miningSoundEvent.IsNull)
				{
					this.loopingSounds.StartSound(this.miningSoundEvent);
				}
			}
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x00097315 File Offset: 0x00095515
	private void OnStopMiningSound(object data)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.StopSound(this.miningSoundEvent);
			this.miningSound = null;
		}
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x0009733C File Offset: 0x0009553C
	public void SetPercentComplete(float progress)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
		}
	}

	// Token: 0x04000F97 RID: 3991
	private static HashedString HASH_PERCENTCOMPLETE = "percentComplete";

	// Token: 0x04000F98 RID: 3992
	[MyCmpGet]
	private LoopingSounds loopingSounds;

	// Token: 0x04000F99 RID: 3993
	private FMODAsset miningSound;

	// Token: 0x04000F9A RID: 3994
	private EventReference miningSoundEvent;

	// Token: 0x04000F9B RID: 3995
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStartMiningSound(data);
	});

	// Token: 0x04000F9C RID: 3996
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStopMiningSound(data);
	});
}
