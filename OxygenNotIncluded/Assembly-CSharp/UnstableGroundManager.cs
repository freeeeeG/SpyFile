using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000A0F RID: 2575
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGroundManager")]
public class UnstableGroundManager : KMonoBehaviour
{
	// Token: 0x06004D0F RID: 19727 RVA: 0x001B0388 File Offset: 0x001AE588
	protected override void OnPrefabInit()
	{
		this.fallingTileOffset = new Vector3(0.5f, 0f, 0f);
		UnstableGroundManager.EffectInfo[] array = this.effects;
		for (int i = 0; i < array.Length; i++)
		{
			UnstableGroundManager.EffectInfo effectInfo = array[i];
			GameObject prefab = effectInfo.prefab;
			prefab.SetActive(false);
			UnstableGroundManager.EffectRuntimeInfo value = default(UnstableGroundManager.EffectRuntimeInfo);
			GameObjectPool pool = new GameObjectPool(() => this.InstantiateObj(prefab), 16);
			value.pool = pool;
			value.releaseFunc = delegate(GameObject go)
			{
				this.ReleaseGO(go);
				pool.ReleaseInstance(go);
			};
			this.runtimeInfo[effectInfo.element] = value;
		}
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x001B044B File Offset: 0x001AE64B
	private void ReleaseGO(GameObject go)
	{
		if (GameComps.Gravities.Has(go))
		{
			GameComps.Gravities.Remove(go);
		}
		go.SetActive(false);
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x001B046C File Offset: 0x001AE66C
	private GameObject InstantiateObj(GameObject prefab)
	{
		GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.SceneLayer.BuildingBack, null, 0);
		gameObject.SetActive(false);
		gameObject.name = "UnstablePool";
		return gameObject;
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x001B048C File Offset: 0x001AE68C
	public void Spawn(int cell, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		Vector3 vector = Grid.CellToPosCCC(cell, Grid.SceneLayer.TileMain);
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			global::Debug.LogError("Tried to spawn unstable ground with NaN temperature");
			temperature = 293f;
		}
		KBatchedAnimController kbatchedAnimController = this.Spawn(vector, element, mass, temperature, disease_idx, disease_count);
		kbatchedAnimController.Play("start", KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.Play("loop", KAnim.PlayMode.Loop, 1f, 0f);
		kbatchedAnimController.gameObject.name = "Falling " + element.name;
		GameComps.Gravities.Add(kbatchedAnimController.gameObject, Vector2.zero, null);
		this.fallingObjects.Add(kbatchedAnimController.gameObject);
		this.SpawnPuff(vector, element, mass, temperature, disease_idx, disease_count);
		Substance substance = element.substance;
		if (substance != null && !substance.fallingStartSound.IsNull && CameraController.Instance.IsAudibleSound(vector, substance.fallingStartSound))
		{
			SoundEvent.PlayOneShot(substance.fallingStartSound, vector, 1f);
		}
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x001B059C File Offset: 0x001AE79C
	private void SpawnOld(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (!element.IsUnstable)
		{
			global::Debug.LogError("Spawning falling ground with a stable element");
		}
		KBatchedAnimController kbatchedAnimController = this.Spawn(pos, element, mass, temperature, disease_idx, disease_count);
		GameComps.Gravities.Add(kbatchedAnimController.gameObject, Vector2.zero, null);
		kbatchedAnimController.Play("loop", KAnim.PlayMode.Loop, 1f, 0f);
		this.fallingObjects.Add(kbatchedAnimController.gameObject);
		kbatchedAnimController.gameObject.name = "SpawnOld " + element.name;
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x001B062C File Offset: 0x001AE82C
	private void SpawnPuff(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (!element.IsUnstable)
		{
			global::Debug.LogError("Spawning sand puff with a stable element");
		}
		KBatchedAnimController kbatchedAnimController = this.Spawn(pos, element, mass, temperature, disease_idx, disease_count);
		kbatchedAnimController.Play("sandPuff", KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.name = "Puff " + element.name;
		kbatchedAnimController.transform.SetPosition(kbatchedAnimController.transform.GetPosition() + this.spawnPuffOffset);
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x001B06B4 File Offset: 0x001AE8B4
	private KBatchedAnimController Spawn(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		UnstableGroundManager.EffectRuntimeInfo effectRuntimeInfo;
		if (!this.runtimeInfo.TryGetValue(element.id, out effectRuntimeInfo))
		{
			global::Debug.LogError(element.id.ToString() + " needs unstable ground info hookup!");
		}
		GameObject instance = effectRuntimeInfo.pool.GetInstance();
		instance.transform.SetPosition(pos);
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			global::Debug.LogError("Tried to spawn unstable ground with NaN temperature");
			temperature = 293f;
		}
		UnstableGround component = instance.GetComponent<UnstableGround>();
		component.element = element.id;
		component.mass = mass;
		component.temperature = temperature;
		component.diseaseIdx = disease_idx;
		component.diseaseCount = disease_count;
		instance.SetActive(true);
		KBatchedAnimController component2 = instance.GetComponent<KBatchedAnimController>();
		component2.onDestroySelf = effectRuntimeInfo.releaseFunc;
		component2.Stop();
		if (element.substance != null)
		{
			component2.TintColour = element.substance.colour;
		}
		return component2;
	}

	// Token: 0x06004D16 RID: 19734 RVA: 0x001B079C File Offset: 0x001AE99C
	public List<int> GetCellsContainingFallingAbove(Vector2I cellXY)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.fallingObjects.Count; i++)
		{
			Vector2I vector2I;
			Grid.PosToXY(this.fallingObjects[i].transform.GetPosition(), out vector2I);
			if (vector2I.x == cellXY.x && vector2I.y >= cellXY.y)
			{
				int item = Grid.PosToCell(vector2I);
				list.Add(item);
			}
		}
		for (int j = 0; j < this.pendingCells.Count; j++)
		{
			Vector2I vector2I2 = Grid.CellToXY(this.pendingCells[j]);
			if (vector2I2.x == cellXY.x && vector2I2.y >= cellXY.y)
			{
				list.Add(this.pendingCells[j]);
			}
		}
		return list;
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x001B0871 File Offset: 0x001AEA71
	private void RemoveFromPending(int cell)
	{
		this.pendingCells.Remove(cell);
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x001B0880 File Offset: 0x001AEA80
	private void Update()
	{
		if (App.isLoading)
		{
			return;
		}
		int i = 0;
		while (i < this.fallingObjects.Count)
		{
			GameObject gameObject = this.fallingObjects[i];
			if (!(gameObject == null))
			{
				Vector3 position = gameObject.transform.GetPosition();
				int cell = Grid.PosToCell(position);
				Grid.CellRight(cell);
				Grid.CellLeft(cell);
				int num = Grid.CellBelow(cell);
				Grid.CellRight(num);
				Grid.CellLeft(num);
				int cell2 = cell;
				if (!Grid.IsValidCell(num) || Grid.Element[num].IsSolid || (Grid.Properties[num] & 4) != 0)
				{
					UnstableGround component = gameObject.GetComponent<UnstableGround>();
					this.pendingCells.Add(cell2);
					HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(delegate()
					{
						this.RemoveFromPending(cell);
					}, false));
					SimMessages.AddRemoveSubstance(cell2, component.element, CellEventLogger.Instance.UnstableGround, component.mass, component.temperature, component.diseaseIdx, component.diseaseCount, true, handle.index);
					ListPool<ScenePartitionerEntry, UnstableGroundManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, UnstableGroundManager>.Allocate();
					Vector2I vector2I = Grid.CellToXY(cell);
					vector2I.x = Mathf.Max(0, vector2I.x - 1);
					vector2I.y = Mathf.Min(Grid.HeightInCells - 1, vector2I.y + 1);
					GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 3, 3, GameScenePartitioner.Instance.collisionLayer, pooledList);
					foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
					{
						if (scenePartitionerEntry.obj is KCollider2D)
						{
							(scenePartitionerEntry.obj as KCollider2D).gameObject.Trigger(-975551167, null);
						}
					}
					pooledList.Recycle();
					Element element = ElementLoader.FindElementByHash(component.element);
					if (element != null && element.substance != null && !element.substance.fallingStopSound.IsNull && CameraController.Instance.IsAudibleSound(position, element.substance.fallingStopSound))
					{
						SoundEvent.PlayOneShot(element.substance.fallingStopSound, position, 1f);
					}
					GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position + this.landEffectOffset, Grid.SceneLayer.Front, null, 0).SetActive(true);
					this.fallingObjects[i] = this.fallingObjects[this.fallingObjects.Count - 1];
					this.fallingObjects.RemoveAt(this.fallingObjects.Count - 1);
					this.ReleaseGO(gameObject);
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001B0B74 File Offset: 0x001AED74
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.fallingObjects.Count > 0)
		{
			this.serializedInfo = new List<UnstableGroundManager.SerializedInfo>();
		}
		foreach (GameObject gameObject in this.fallingObjects)
		{
			UnstableGround component = gameObject.GetComponent<UnstableGround>();
			byte diseaseIdx = component.diseaseIdx;
			int diseaseID = (diseaseIdx != byte.MaxValue) ? Db.Get().Diseases[(int)diseaseIdx].id.HashValue : 0;
			this.serializedInfo.Add(new UnstableGroundManager.SerializedInfo
			{
				position = gameObject.transform.GetPosition(),
				element = component.element,
				mass = component.mass,
				temperature = component.temperature,
				diseaseID = diseaseID,
				diseaseCount = component.diseaseCount
			});
		}
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x001B0C78 File Offset: 0x001AEE78
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedInfo = null;
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x001B0C84 File Offset: 0x001AEE84
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedInfo == null)
		{
			return;
		}
		this.fallingObjects.Clear();
		HashedString id = default(HashedString);
		foreach (UnstableGroundManager.SerializedInfo serializedInfo in this.serializedInfo)
		{
			Element element = ElementLoader.FindElementByHash(serializedInfo.element);
			id.HashValue = serializedInfo.diseaseID;
			byte index = Db.Get().Diseases.GetIndex(id);
			int disease_count = serializedInfo.diseaseCount;
			if (index == 255)
			{
				disease_count = 0;
			}
			this.SpawnOld(serializedInfo.position, element, serializedInfo.mass, serializedInfo.temperature, index, disease_count);
		}
	}

	// Token: 0x04003240 RID: 12864
	[SerializeField]
	private Vector3 spawnPuffOffset;

	// Token: 0x04003241 RID: 12865
	[SerializeField]
	private Vector3 landEffectOffset;

	// Token: 0x04003242 RID: 12866
	private Vector3 fallingTileOffset;

	// Token: 0x04003243 RID: 12867
	[SerializeField]
	private UnstableGroundManager.EffectInfo[] effects;

	// Token: 0x04003244 RID: 12868
	private List<GameObject> fallingObjects = new List<GameObject>();

	// Token: 0x04003245 RID: 12869
	private List<int> pendingCells = new List<int>();

	// Token: 0x04003246 RID: 12870
	private Dictionary<SimHashes, UnstableGroundManager.EffectRuntimeInfo> runtimeInfo = new Dictionary<SimHashes, UnstableGroundManager.EffectRuntimeInfo>();

	// Token: 0x04003247 RID: 12871
	[Serialize]
	private List<UnstableGroundManager.SerializedInfo> serializedInfo;

	// Token: 0x0200189B RID: 6299
	[Serializable]
	private struct EffectInfo
	{
		// Token: 0x04007272 RID: 29298
		[HashedEnum]
		public SimHashes element;

		// Token: 0x04007273 RID: 29299
		public GameObject prefab;
	}

	// Token: 0x0200189C RID: 6300
	private struct EffectRuntimeInfo
	{
		// Token: 0x04007274 RID: 29300
		public GameObjectPool pool;

		// Token: 0x04007275 RID: 29301
		public Action<GameObject> releaseFunc;
	}

	// Token: 0x0200189D RID: 6301
	private struct SerializedInfo
	{
		// Token: 0x04007276 RID: 29302
		public Vector3 position;

		// Token: 0x04007277 RID: 29303
		public SimHashes element;

		// Token: 0x04007278 RID: 29304
		public float mass;

		// Token: 0x04007279 RID: 29305
		public float temperature;

		// Token: 0x0400727A RID: 29306
		public int diseaseID;

		// Token: 0x0400727B RID: 29307
		public int diseaseCount;
	}
}
