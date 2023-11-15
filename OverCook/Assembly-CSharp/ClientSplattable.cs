using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000815 RID: 2069
public class ClientSplattable : ClientSynchroniserBase
{
	// Token: 0x060027A1 RID: 10145 RVA: 0x000BA464 File Offset: 0x000B8864
	private void Awake()
	{
		this.m_splattable = base.gameObject.RequireComponent<Splattable>();
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_splattable.m_splatPrefab[this.m_splattable.m_prefabIndex], new VoidGeneric<GameObject>(this.OnHazardSpawned));
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x000BA4B0 File Offset: 0x000B88B0
	private void OnHazardSpawned(GameObject _object)
	{
		_object.AddComponent<StaticGridLocation>();
		GameUtils.TriggerAudio(GameOneShotAudioTag.Splat, base.gameObject.layer);
	}

	// Token: 0x04001F26 RID: 7974
	private Splattable m_splattable;
}
