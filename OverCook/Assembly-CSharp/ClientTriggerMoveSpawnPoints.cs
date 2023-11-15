using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
public class ClientTriggerMoveSpawnPoints : ClientSynchroniserBase
{
	// Token: 0x06001CC4 RID: 7364 RVA: 0x0008CA3F File Offset: 0x0008AE3F
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerMoveSpawn;
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x0008CA43 File Offset: 0x0008AE43
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_changeRespawn = (TriggerMoveSpawnPoints)synchronisedObject;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x0008CA58 File Offset: 0x0008AE58
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		MoveSpawnMessage moveSpawnMessage = (MoveSpawnMessage)serialisable;
		KeyValuePair<GameObject, Transform>[] array = moveSpawnMessage.ExtractSpawnMap(this.m_changeRespawn.m_spawnPoints);
		for (int i = 0; i < array.Length; i++)
		{
			this.ApplySpawnChange(array[i].Key, array[i].Value);
		}
		if (this.m_changeRespawn.m_movePlayersImmediately)
		{
			for (int j = 0; j < array.Length; j++)
			{
				IEnumerator item = this.MovePlayer(array[j].Key, array[j].Value);
				this.m_moveRoutines.Add(item);
			}
		}
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x0008CB01 File Offset: 0x0008AF01
	private bool ProcessRoutine(IEnumerator _routine)
	{
		return _routine == null || !_routine.MoveNext();
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x0008CB15 File Offset: 0x0008AF15
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_moveRoutines.Count > 0)
		{
			this.m_moveRoutines.RemoveAll(new Predicate<IEnumerator>(this.ProcessRoutine));
		}
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x0008CB48 File Offset: 0x0008AF48
	private void ApplySpawnChange(GameObject _player, Transform _spawn)
	{
		ClientPlayerRespawnBehaviour clientPlayerRespawnBehaviour = _player.RequireComponent<ClientPlayerRespawnBehaviour>();
		clientPlayerRespawnBehaviour.MoveRespawnPoint(_spawn.localPosition, _spawn.parent);
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x0008CB70 File Offset: 0x0008AF70
	private IEnumerator MovePlayer(GameObject _player, Transform _target)
	{
		PlayerControls controls = _player.RequireComponent<PlayerControls>();
		controls.enabled = false;
		controls.Motion.SetKinematic(true);
		DynamicLandscapeParenting dynamicParenting = _player.RequestComponent<DynamicLandscapeParenting>();
		if (dynamicParenting != null)
		{
			dynamicParenting.enabled = false;
		}
		ParticleSystem particles = _player.RequestComponentRecursive<ParticleSystem>();
		if (particles != null)
		{
			particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
		ClientWorldObjectSynchroniser synchroniser = _player.RequestComponent<ClientWorldObjectSynchroniser>();
		if (synchroniser != null)
		{
			synchroniser.Pause();
		}
		Transform transform = _player.transform;
		transform.SetPositionAndRotation(_target.position, transform.rotation);
		transform.SetParent(_target);
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = _player.RequestComponent<ServerWorldObjectSynchroniser>();
			if (serverWorldObjectSynchroniser != null)
			{
				serverWorldObjectSynchroniser.ResumeAllClients(true);
			}
		}
		yield return null;
		if (synchroniser != null)
		{
			while (!synchroniser.IsReadyToResume())
			{
				yield return null;
			}
			synchroniser.Resume();
		}
		if (particles != null)
		{
			particles.Play();
		}
		if (dynamicParenting != null)
		{
			dynamicParenting.enabled = true;
		}
		controls.enabled = true;
		controls.Motion.SetKinematic(false);
		yield break;
	}

	// Token: 0x0400166D RID: 5741
	private TriggerMoveSpawnPoints m_changeRespawn;

	// Token: 0x0400166E RID: 5742
	private List<IEnumerator> m_moveRoutines = new List<IEnumerator>();
}
