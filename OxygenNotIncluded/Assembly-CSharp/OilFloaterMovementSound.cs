using System;

// Token: 0x0200009B RID: 155
internal class OilFloaterMovementSound : KMonoBehaviour
{
	// Token: 0x060002BC RID: 700 RVA: 0x000159C4 File Offset: 0x00013BC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.sound = GlobalAssets.GetSound(this.sound, false);
		base.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "OilFloaterMovementSound");
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00015A1C File Offset: 0x00013C1C
	private void OnObjectMovementStateChanged(object data)
	{
		GameHashes gameHashes = (GameHashes)data;
		this.isMoving = (gameHashes == GameHashes.ObjectMovementWakeUp);
		this.UpdateSound();
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00015A44 File Offset: 0x00013C44
	private void OnCellChanged()
	{
		this.UpdateSound();
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00015A4C File Offset: 0x00013C4C
	private void UpdateSound()
	{
		bool flag = this.isMoving && base.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
		if (flag == this.isPlayingSound)
		{
			return;
		}
		LoopingSounds component = base.GetComponent<LoopingSounds>();
		if (flag)
		{
			component.StartSound(this.sound);
		}
		else
		{
			component.StopSound(this.sound);
		}
		this.isPlayingSound = flag;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00015AAC File Offset: 0x00013CAC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x040001D4 RID: 468
	public string sound;

	// Token: 0x040001D5 RID: 469
	public bool isPlayingSound;

	// Token: 0x040001D6 RID: 470
	public bool isMoving;

	// Token: 0x040001D7 RID: 471
	private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>(delegate(OilFloaterMovementSound component, object data)
	{
		component.OnObjectMovementStateChanged(data);
	});
}
