using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class ServerProjectileSpawner : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x0600161C RID: 5660 RVA: 0x00075A78 File Offset: 0x00073E78
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_spawner = (ProjectileSpawner)synchronisedObject;
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_spawner.m_projectilePrefab);
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00075AB4 File Offset: 0x00073EB4
	private void Awake()
	{
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x00075AB8 File Offset: 0x00073EB8
	private bool FindNextTarget(out Vector3 _position)
	{
		if (this.m_spawner.m_randomTargetOrder)
		{
			_position = ((!this.m_spawner.m_bUseTransformPositions) ? this.m_spawner.m_targetPositions.GetRandomElement<Vector3>() : this.m_spawner.m_transformTargetPositions.GetRandomElement<Transform>().position);
			return true;
		}
		int num = this.m_lastTarget + 1;
		if (num >= this.m_spawner.m_targetPositions.Length)
		{
			num = 0;
		}
		if (num >= 0 && num < this.m_spawner.m_targetPositions.Length)
		{
			this.m_lastTarget = num;
			_position = this.m_spawner.m_targetPositions[num];
			return true;
		}
		_position = Vector3.zero;
		return false;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x00075B80 File Offset: 0x00073F80
	private bool FindNextTarget(out Transform _transform)
	{
		if (this.m_spawner.m_randomTargetOrder)
		{
			_transform = ((!this.m_spawner.m_bUseTransformPositions) ? this.m_spawner.m_transformTargetPositions.GetRandomElement<Transform>() : this.m_spawner.m_transformTargetPositions.GetRandomElement<Transform>());
			return true;
		}
		int num = this.m_lastTarget + 1;
		if (num >= this.m_spawner.m_transformTargetPositions.Length)
		{
			num = 0;
		}
		if (num >= 0 && num < this.m_spawner.m_transformTargetPositions.Length)
		{
			this.m_lastTarget = num;
			_transform = this.m_spawner.m_transformTargetPositions[num];
			return true;
		}
		_transform = null;
		return false;
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x00075C2C File Offset: 0x0007402C
	private void SpawnProjectile(Vector3 _targetPos)
	{
		GameObject obj = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_spawner.m_projectilePrefab, this.m_spawner.m_spawnPoint.position, Quaternion.identity);
		Collider collider = obj.RequestComponent<Collider>();
		if (collider != null)
		{
			Collider[] array = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents);
			for (int i = 0; i < array.Length; i++)
			{
				Physics.IgnoreCollision(array[i], collider);
			}
		}
		ServerProjectile serverProjectile = obj.RequireComponent<ServerProjectile>();
		serverProjectile.RegisterReachedTargetCallback(new VoidGeneric<ServerProjectile>(this.ProjectileReachedTarget));
		serverProjectile.RegisterCollidedCallback(new VoidGeneric<ServerProjectile, Collision>(this.ProjectileCollided));
		serverProjectile.SetTargetAndTimeToTarget(_targetPos, this.m_spawner.m_airTime);
		if (this.m_spawner.m_fireMode == ProjectileSpawner.FireMode.Parabolic)
		{
			serverProjectile.SetGravity(Physics.gravity);
		}
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x00075D1C File Offset: 0x0007411C
	private void SpawnProjectile(Transform _targetTransform)
	{
		GameObject obj = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_spawner.m_projectilePrefab, this.m_spawner.m_spawnPoint.position, Quaternion.identity);
		Collider collider = obj.RequestComponent<Collider>();
		if (collider != null)
		{
			Collider[] array = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents);
			for (int i = 0; i < array.Length; i++)
			{
				Physics.IgnoreCollision(array[i], collider);
			}
		}
		ServerProjectile serverProjectile = obj.RequireComponent<ServerProjectile>();
		serverProjectile.RegisterReachedTargetCallback(new VoidGeneric<ServerProjectile>(this.ProjectileReachedTarget));
		serverProjectile.RegisterCollidedCallback(new VoidGeneric<ServerProjectile, Collision>(this.ProjectileCollided));
		serverProjectile.SetTargetAndTimeToTarget(_targetTransform, this.m_spawner.m_airTime);
		ServerFireHazardSpawner component = serverProjectile.GetComponent<ServerFireHazardSpawner>();
		if (component != null)
		{
			component.SetTargetTransformToAttach(_targetTransform);
		}
		if (this.m_spawner.m_fireMode == ProjectileSpawner.FireMode.Parabolic)
		{
			serverProjectile.SetGravity(Physics.gravity);
		}
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x00075E2C File Offset: 0x0007422C
	private void ProjectileReachedTarget(ServerProjectile _projectile)
	{
		if (this.m_spawner.m_reachedTargetTrigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_spawner.m_reachedTargetTrigger);
			_projectile.gameObject.SendTrigger(this.m_spawner.m_reachedTargetTrigger);
		}
		_projectile.enabled = false;
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x00075E88 File Offset: 0x00074288
	private void ProjectileCollided(ServerProjectile _projectile, Collision _collision)
	{
		if (this.m_spawner.m_collidedTrigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_spawner.m_collidedTrigger);
			_projectile.gameObject.SendTrigger(this.m_spawner.m_collidedTrigger);
		}
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x00075EDC File Offset: 0x000742DC
	public void OnTrigger(string _trigger)
	{
		if (this.m_spawner.m_fireTrigger == _trigger)
		{
			Vector3 posFromGridLocation;
			if (this.m_spawner.m_transformTargetPositions.Length > 0)
			{
				Transform targetTransform;
				if (this.FindNextTarget(out targetTransform))
				{
					this.SpawnProjectile(targetTransform);
					GameUtils.TriggerAudio(this.m_spawner.m_spawnAudioTag, base.gameObject.layer);
				}
			}
			else if (this.FindNextTarget(out posFromGridLocation))
			{
				if (this.m_spawner.m_alignTargetsToGrid)
				{
					GridIndex gridLocationFromPos = this.m_gridManager.GetGridLocationFromPos(posFromGridLocation);
					posFromGridLocation = this.m_gridManager.GetPosFromGridLocation(gridLocationFromPos);
				}
				this.SpawnProjectile(posFromGridLocation);
				GameUtils.TriggerAudio(this.m_spawner.m_spawnAudioTag, base.gameObject.layer);
			}
		}
	}

	// Token: 0x040010B1 RID: 4273
	private ProjectileSpawner m_spawner;

	// Token: 0x040010B2 RID: 4274
	private GridManager m_gridManager;

	// Token: 0x040010B3 RID: 4275
	private int m_lastTarget = -1;
}
