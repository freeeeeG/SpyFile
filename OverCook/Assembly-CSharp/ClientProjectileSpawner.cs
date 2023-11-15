using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class ClientProjectileSpawner : ClientSynchroniserBase
{
	// Token: 0x06001626 RID: 5670 RVA: 0x00075FA9 File Offset: 0x000743A9
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_spawner = (ProjectileSpawner)synchronisedObject;
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_spawner.m_projectilePrefab, new VoidGeneric<GameObject>(this.OnProjectileSpawned));
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x00075FE0 File Offset: 0x000743E0
	private void OnProjectileSpawned(GameObject _object)
	{
		ParticleSystem particleSystem = _object.RequestComponentRecursive<ParticleSystem>();
		if (particleSystem != null)
		{
			particleSystem.RestartPFX();
		}
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x00076006 File Offset: 0x00074406
	private void ProjectileReachedTarget(Projectile _projectile)
	{
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00076008 File Offset: 0x00074408
	private void ProjectileCollided(Projectile _projectile, Collision _collision)
	{
	}

	// Token: 0x040010B4 RID: 4276
	private ProjectileSpawner m_spawner;
}
