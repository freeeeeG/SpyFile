using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006EB RID: 1771
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class Comet : KMonoBehaviour, ISim33ms
{
	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06003082 RID: 12418 RVA: 0x00100673 File Offset: 0x000FE873
	public float ExplosionMass
	{
		get
		{
			return this.explosionMass;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06003083 RID: 12419 RVA: 0x0010067B File Offset: 0x000FE87B
	public float AddTileMass
	{
		get
		{
			return this.addTileMass;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06003084 RID: 12420 RVA: 0x00100683 File Offset: 0x000FE883
	public Vector3 TargetPosition
	{
		get
		{
			return this.anim.PositionIncludingOffset;
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06003085 RID: 12421 RVA: 0x00100690 File Offset: 0x000FE890
	// (set) Token: 0x06003086 RID: 12422 RVA: 0x00100698 File Offset: 0x000FE898
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x001006A4 File Offset: 0x000FE8A4
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x001006E2 File Offset: 0x000FE8E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.remainingTileDamage = this.totalTileDamage;
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.RandomizeVelocity();
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x00100720 File Offset: 0x000FE920
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		if (this.spawnWithOffset)
		{
			this.SetupOffset();
		}
		base.OnSpawn();
		this.RandomizeMassAndTemperature();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		this.typeID = base.GetComponent<KPrefabID>().PrefabTag;
		Components.Meteors.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x001007BF File Offset: 0x000FE9BF
	protected override void OnCleanUp()
	{
		Components.Meteors.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x001007D8 File Offset: 0x000FE9D8
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x0010099C File Offset: 0x000FEB9C
	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
		float f = num * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().Rotation = -num - 90f;
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x00100A20 File Offset: 0x000FEC20
	public void RandomizeMassAndTemperature()
	{
		float num = UnityEngine.Random.Range(this.massRange.x, this.massRange.y) * this.GetMassMultiplier();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.Mass = num;
		component.Temperature = UnityEngine.Random.Range(this.temperatureRange.x, this.temperatureRange.y);
		if (this.addTiles > 0)
		{
			float num2 = UnityEngine.Random.Range(0.95f, 0.98f);
			this.explosionMass = num * (1f - num2);
			this.addTileMass = num * num2;
			return;
		}
		this.explosionMass = num;
		this.addTileMass = 0f;
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x00100AC4 File Offset: 0x000FECC4
	public float GetMassMultiplier()
	{
		float num = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
		if (this.affectedByDifficulty && currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (id != null)
			{
				if (!(id == "Infrequent"))
				{
					if (!(id == "Intense"))
					{
						if (id == "Doomed")
						{
							num *= 0.5f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				else
				{
					num *= 1f;
				}
			}
		}
		return num;
	}

	// Token: 0x0600308F RID: 12431 RVA: 0x00100B42 File Offset: 0x000FED42
	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x00100B61 File Offset: 0x000FED61
	public float GetRandomTemperatureForOres()
	{
		return UnityEngine.Random.Range(this.explosionTemperatureRange.x, this.explosionTemperatureRange.y);
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x00100B80 File Offset: 0x000FED80
	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		int world = (int)Grid.WorldIdx[cell3];
		this.PlayImpactSound(pos);
		Vector3 pos2 = pos;
		pos2.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (this.explosionEffectHash != SpawnFXHashes.None)
		{
			Game.Instance.SpawnFX(this.explosionEffectHash, pos2, 0f);
		}
		Substance substance = element.substance;
		int randomNumOres = this.GetRandomNumOres();
		Vector2 vector = -this.velocity.normalized;
		Vector2 a = new Vector2(vector.y, -vector.x);
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x - 3, (int)pos.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
			if (!(gameObject.GetComponent<MinionIdentity>() != null) && !(gameObject.GetComponent<CreatureBrain>() != null) && gameObject.GetDef<RobotAi.Def>() == null)
			{
				Vector2 vector2 = (gameObject.transform.GetPosition() - pos).normalized;
				vector2 += new Vector2(0f, 0.55f);
				vector2 *= 0.5f * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				if (GameComps.Gravities.Has(gameObject))
				{
					GameComps.Gravities.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, vector2);
			}
		}
		pooledList.Recycle();
		int num = this.splashRadius + 1;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				int num2 = Grid.OffsetCell(cell3, j, i);
				if (Grid.IsValidCellInWorld(num2, world) && !this.destroyedCells.Contains(num2))
				{
					float num3 = (1f - (float)Mathf.Abs(j) / (float)num) * (1f - (float)Mathf.Abs(i) / (float)num);
					if (num3 > 0f)
					{
						this.DamageTiles(num2, prev_cell, num3 * this.totalTileDamage * 0.5f);
					}
				}
			}
		}
		float mass = (randomNumOres > 0) ? (this.explosionMass / (float)randomNumOres) : 1f;
		float randomTemperatureForOres = this.GetRandomTemperatureForOres();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		for (int k = 0; k < randomNumOres; k++)
		{
			Vector2 normalized = (vector + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
			Vector3 v = normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
			Vector3 vector3 = normalized.normalized * 0.75f;
			vector3 += new Vector3(0f, 0.55f, 0f);
			vector3 += pos;
			GameObject go = substance.SpawnResource(vector3, mass, randomTemperatureForOres, component.DiseaseIdx, component.DiseaseCount / (randomNumOres + this.addTiles), false, false, false);
			if (GameComps.Fallers.Has(go))
			{
				GameComps.Fallers.Remove(go);
			}
			GameComps.Fallers.Add(go, v);
		}
		if (this.addTiles > 0)
		{
			float depthOfElement = (float)this.GetDepthOfElement(cell3, element, world);
			float num4 = 1f;
			float num5 = (depthOfElement - (float)this.addTilesMinHeight) / (float)(this.addTilesMaxHeight - this.addTilesMinHeight);
			if (!float.IsNaN(num5))
			{
				num4 -= num5;
			}
			int num6 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float)this.addTiles * num4), 1, this.addTiles));
			HashSetPool<int, Comet>.PooledHashSet pooledHashSet = HashSetPool<int, Comet>.Allocate();
			HashSetPool<int, Comet>.PooledHashSet pooledHashSet2 = HashSetPool<int, Comet>.Allocate();
			QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
			int num7 = -1;
			int num8 = 1;
			if (this.velocity.x < 0f)
			{
				num7 *= -1;
				num8 *= -1;
			}
			pooledQueue.Enqueue(new GameUtil.FloodFillInfo
			{
				cell = prev_cell,
				depth = 0
			});
			pooledQueue.Enqueue(new GameUtil.FloodFillInfo
			{
				cell = Grid.OffsetCell(prev_cell, new CellOffset(num7, 0)),
				depth = 0
			});
			pooledQueue.Enqueue(new GameUtil.FloodFillInfo
			{
				cell = Grid.OffsetCell(prev_cell, new CellOffset(num8, 0)),
				depth = 0
			});
			Func<int, bool> condition = (int cell) => Grid.IsValidCellInWorld(cell, world) && !Grid.Solid[cell];
			GameUtil.FloodFillConditional(pooledQueue, condition, pooledHashSet2, pooledHashSet, 10);
			float mass2 = (num6 > 0) ? (this.addTileMass / (float)this.addTiles) : 1f;
			int disease_count = this.addDiseaseCount / num6;
			if (element.HasTag(GameTags.Unstable))
			{
				UnstableGroundManager component2 = World.Instance.GetComponent<UnstableGroundManager>();
				using (HashSet<int>.Enumerator enumerator2 = pooledHashSet.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int cell2 = enumerator2.Current;
						if (num6 <= 0)
						{
							break;
						}
						component2.Spawn(cell2, element, mass2, randomTemperatureForOres, byte.MaxValue, 0);
						num6--;
					}
					goto IL_5D4;
				}
			}
			foreach (int gameCell in pooledHashSet)
			{
				if (num6 <= 0)
				{
					break;
				}
				SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass2, randomTemperatureForOres, this.diseaseIdx, disease_count, true, -1);
				num6--;
			}
			IL_5D4:
			pooledHashSet.Recycle();
			pooledHashSet2.Recycle();
			pooledQueue.Recycle();
		}
		this.SpawnCraterPrefabs();
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x001011DC File Offset: 0x000FF3DC
	protected virtual void SpawnCraterPrefabs()
	{
		if (this.craterPrefabs != null && this.craterPrefabs.Length != 0)
		{
			GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab(this.craterPrefabs[UnityEngine.Random.Range(0, this.craterPrefabs.Length)]), Grid.CellToPos(Grid.PosToCell(this)));
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -19.5f);
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x00101268 File Offset: 0x000FF468
	private int GetDepthOfElement(int cell, Element element, int world)
	{
		int num = 0;
		int num2 = Grid.CellBelow(cell);
		while (Grid.IsValidCellInWorld(num2, world) && Grid.Element[num2] == element)
		{
			num++;
			num2 = Grid.CellBelow(num2);
		}
		return num;
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x001012A0 File Offset: 0x000FF4A0
	[ContextMenu("DamageTiles")]
	private float DamageTiles(int cell, int prev_cell, float input_damage)
	{
		GameObject gameObject = Grid.Objects[cell, 9];
		float num = 1f;
		bool flag = false;
		if (gameObject != null)
		{
			if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Window))
			{
				num = this.windowDamageMultiplier;
			}
			else if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker))
			{
				num = this.bunkerDamageMultiplier;
				if (gameObject.GetComponent<Door>() != null)
				{
					Game.Instance.savedInfo.blockedCometWithBunkerDoor = true;
				}
			}
			SimCellOccupier component = gameObject.GetComponent<SimCellOccupier>();
			if (component != null && !component.doReplaceElement)
			{
				flag = true;
			}
		}
		Element element;
		if (flag)
		{
			element = gameObject.GetComponent<PrimaryElement>().Element;
		}
		else
		{
			element = Grid.Element[cell];
		}
		if (element.strength == 0f)
		{
			return 0f;
		}
		float num2 = input_damage * num / element.strength;
		this.PlayTileDamageSound(element, Grid.CellToPos(cell), gameObject);
		if (num2 == 0f)
		{
			return 0f;
		}
		float num3;
		if (flag)
		{
			BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
			float a = (float)component2.HitPoints / (float)component2.MaxHitPoints;
			float f = num2 * (float)component2.MaxHitPoints;
			component2.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = Mathf.RoundToInt(f),
				source = BUILDINGS.DAMAGESOURCES.COMET,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
			});
			num3 = Mathf.Min(a, num2);
		}
		else
		{
			num3 = WorldDamage.Instance.ApplyDamage(cell, num2, prev_cell, BUILDINGS.DAMAGESOURCES.COMET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);
		}
		this.destroyedCells.Add(cell);
		float num4 = num3 / num2;
		return input_damage * (1f - num4);
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x00101458 File Offset: 0x000FF658
	private void DamageThings(Vector3 pos, int cell, int damage, GameObject ignoreObject = null)
	{
		if (damage == 0 || !Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject != ignoreObject)
		{
			BuildingHP component = gameObject.GetComponent<BuildingHP>();
			Building component2 = gameObject.GetComponent<Building>();
			if (component != null && !this.damagedEntities.Contains(gameObject))
			{
				float f = gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? ((float)damage * this.bunkerDamageMultiplier) : ((float)damage);
				if (component2 != null && component2.Def != null)
				{
					this.PlayBuildingDamageSound(component2.Def, Grid.CellToPos(cell), gameObject);
				}
				component.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
				{
					damage = Mathf.RoundToInt(f),
					source = BUILDINGS.DAMAGESOURCES.COMET,
					popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
				});
				this.damagedEntities.Add(gameObject);
			}
		}
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x, (int)pos.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
			Health component3 = pickupable.GetComponent<Health>();
			if (component3 != null && !this.damagedEntities.Contains(pickupable.gameObject))
			{
				float amount = pickupable.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? ((float)damage * this.bunkerDamageMultiplier) : ((float)damage);
				component3.Damage(amount);
				this.damagedEntities.Add(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x00101644 File Offset: 0x000FF844
	public float GetDistanceFromImpact()
	{
		float num = this.velocity.x / this.velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 b = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + b);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return b.magnitude;
			}
		}
		return 6f;
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x001016ED File Offset: 0x000FF8ED
	public float GetSoundDistance()
	{
		return this.GetDistanceFromImpact();
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x001016F8 File Offset: 0x000FF8F8
	private void PlayTileDamageSound(Element element, Vector3 pos, GameObject tile_go)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			if (element.HasTag(GameTags.RefinedMetal))
			{
				text = "RefinedMetal";
			}
			else if (element.HasTag(GameTags.Metal))
			{
				text = "RawMetal";
			}
			else
			{
				text = "Rock";
			}
		}
		text = "MeteorDamage_" + text;
		text = GlobalAssets.GetSound(text, false);
		if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			float volume = this.GetVolume(tile_go);
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos, false), volume);
		}
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x00101794 File Offset: 0x000FF994
	private void PlayBuildingDamageSound(BuildingDef def, Vector3 pos, GameObject building_go)
	{
		if (def != null)
		{
			string sound = GlobalAssets.GetSound(StringFormatter.Combine("MeteorDamage_Building_", def.AudioCategory), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("MeteorDamage_Building_Metal", false);
			}
			if (sound != null && CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, sound))
			{
				float volume = this.GetVolume(building_go);
				KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos, false), volume);
			}
		}
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x00101810 File Offset: 0x000FFA10
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
		}
		else
		{
			if (this.anim.Offset != Vector3.zero)
			{
				this.anim.Offset = Vector3.zero;
			}
			if (!this.selectable.enabled)
			{
				this.selectable.enabled = true;
			}
			Vector2 vector2 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * -0.1f;
			Vector2 vector3 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector4 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			int num = Grid.PosToCell(vector4);
			this.loopingSounds.UpdateVelocity(this.flyingSound, vector4 - position);
			Element element = ElementLoader.FindElementByHash(this.EXHAUST_ELEMENT);
			if (this.EXHAUST_ELEMENT != SimHashes.Void && Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				SimMessages.EmitMass(num, element.idx, dt * this.EXHAUST_RATE, element.defaultValues.temperature, this.diseaseIdx, Mathf.RoundToInt((float)this.addDiseaseCount * dt), -1);
			}
			if (vector4.x < vector2.x || vector3.x < vector4.x || vector4.y < vector2.y)
			{
				global::Util.KDestroyGameObject(base.gameObject);
			}
			int num2 = Grid.PosToCell(this);
			int num3 = Grid.PosToCell(this.previousPosition);
			if (num2 != num3)
			{
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					this.remainingTileDamage = this.DamageTiles(num2, num3, this.remainingTileDamage);
					if (this.remainingTileDamage <= 0f)
					{
						this.Explode(position, num2, num3, component.Element);
						this.hasExploded = true;
						if (this.destroyOnExplode)
						{
							global::Util.KDestroyGameObject(base.gameObject);
						}
						return;
					}
				}
				else
				{
					GameObject ignoreObject = (this.ignoreObstacleForDamage.Get() == null) ? null : this.ignoreObstacleForDamage.Get().gameObject;
					this.DamageThings(position, num2, this.entityDamage, ignoreObject);
				}
			}
			if (this.canHitDuplicants && this.age > 0.25f && Grid.Objects[Grid.PosToCell(position), 0] != null)
			{
				base.transform.position = Grid.CellToPos(Grid.PosToCell(position));
				this.Explode(position, num2, num3, base.GetComponent<PrimaryElement>().Element);
				if (this.destroyOnExplode)
				{
					global::Util.KDestroyGameObject(base.gameObject);
				}
				return;
			}
			this.previousPosition = position;
			base.transform.SetPosition(vector4);
		}
		this.age += dt;
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x00101B60 File Offset: 0x000FFD60
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x00101C01 File Offset: 0x000FFE01
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x00101C34 File Offset: 0x000FFE34
	public void Explode()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		this.Explode(position, num, num, component.Element);
		this.hasExploded = true;
		if (this.destroyOnExplode)
		{
			global::Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x04001CA6 RID: 7334
	public SimHashes EXHAUST_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x04001CA7 RID: 7335
	public float EXHAUST_RATE = 50f;

	// Token: 0x04001CA8 RID: 7336
	public Vector2 spawnVelocity = new Vector2(12f, 15f);

	// Token: 0x04001CA9 RID: 7337
	public Vector2 spawnAngle = new Vector2(-100f, -80f);

	// Token: 0x04001CAA RID: 7338
	public Vector2 massRange;

	// Token: 0x04001CAB RID: 7339
	public Vector2 temperatureRange;

	// Token: 0x04001CAC RID: 7340
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04001CAD RID: 7341
	public int splashRadius = 1;

	// Token: 0x04001CAE RID: 7342
	public int addTiles;

	// Token: 0x04001CAF RID: 7343
	public int addTilesMinHeight;

	// Token: 0x04001CB0 RID: 7344
	public int addTilesMaxHeight;

	// Token: 0x04001CB1 RID: 7345
	public int entityDamage = 1;

	// Token: 0x04001CB2 RID: 7346
	public float totalTileDamage = 0.2f;

	// Token: 0x04001CB3 RID: 7347
	private float addTileMass;

	// Token: 0x04001CB4 RID: 7348
	public int addDiseaseCount;

	// Token: 0x04001CB5 RID: 7349
	public byte diseaseIdx = byte.MaxValue;

	// Token: 0x04001CB6 RID: 7350
	public Vector2 elementReplaceTileTemperatureRange = new Vector2(800f, 1000f);

	// Token: 0x04001CB7 RID: 7351
	public Vector2I explosionOreCount = new Vector2I(0, 0);

	// Token: 0x04001CB8 RID: 7352
	private float explosionMass;

	// Token: 0x04001CB9 RID: 7353
	public Vector2 explosionTemperatureRange = new Vector2(500f, 700f);

	// Token: 0x04001CBA RID: 7354
	public Vector2 explosionSpeedRange = new Vector2(8f, 14f);

	// Token: 0x04001CBB RID: 7355
	public float windowDamageMultiplier = 5f;

	// Token: 0x04001CBC RID: 7356
	public float bunkerDamageMultiplier;

	// Token: 0x04001CBD RID: 7357
	public string impactSound;

	// Token: 0x04001CBE RID: 7358
	public string flyingSound;

	// Token: 0x04001CBF RID: 7359
	public int flyingSoundID;

	// Token: 0x04001CC0 RID: 7360
	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x04001CC1 RID: 7361
	public bool affectedByDifficulty = true;

	// Token: 0x04001CC2 RID: 7362
	public bool Targeted;

	// Token: 0x04001CC3 RID: 7363
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x04001CC4 RID: 7364
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x04001CC5 RID: 7365
	[Serialize]
	private float remainingTileDamage;

	// Token: 0x04001CC6 RID: 7366
	private Vector3 previousPosition;

	// Token: 0x04001CC7 RID: 7367
	private bool hasExploded;

	// Token: 0x04001CC8 RID: 7368
	public bool canHitDuplicants;

	// Token: 0x04001CC9 RID: 7369
	public string[] craterPrefabs;

	// Token: 0x04001CCA RID: 7370
	public string[] lootOnDestroyedByMissile;

	// Token: 0x04001CCB RID: 7371
	public bool destroyOnExplode = true;

	// Token: 0x04001CCC RID: 7372
	public bool spawnWithOffset;

	// Token: 0x04001CCD RID: 7373
	private float age;

	// Token: 0x04001CCE RID: 7374
	public System.Action OnImpact;

	// Token: 0x04001CCF RID: 7375
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x04001CD0 RID: 7376
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x04001CD1 RID: 7377
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001CD2 RID: 7378
	public Tag typeID;

	// Token: 0x04001CD3 RID: 7379
	private LoopingSounds loopingSounds;

	// Token: 0x04001CD4 RID: 7380
	private List<GameObject> damagedEntities = new List<GameObject>();

	// Token: 0x04001CD5 RID: 7381
	private List<int> destroyedCells = new List<int>();

	// Token: 0x04001CD6 RID: 7382
	private const float MAX_DISTANCE_TEST = 6f;
}
