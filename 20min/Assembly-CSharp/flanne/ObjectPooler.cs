using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000076 RID: 118
	public class ObjectPooler : MonoBehaviour
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x00018870 File Offset: 0x00016A70
		private void Awake()
		{
			ObjectPooler.SharedInstance = this;
			this.itemDictionary = new Dictionary<string, ObjectPoolItem>();
			this.pooledObjectsDictionary = new Dictionary<string, List<GameObject>>();
			this.pooledObjects = new List<GameObject>();
			this.positions = new Dictionary<string, int>();
			for (int i = 0; i < this.itemsToPool.Count; i++)
			{
				this.ObjectPoolItemToPooledObject(i);
				this.itemDictionary.Add(this.itemsToPool[i].tag, this.itemsToPool[i]);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000188F4 File Offset: 0x00016AF4
		public GameObject GetPooledObject(string tag)
		{
			int count = this.pooledObjectsDictionary[tag].Count;
			for (int i = this.positions[tag] + 1; i < this.positions[tag] + this.pooledObjectsDictionary[tag].Count; i++)
			{
				if (!this.pooledObjectsDictionary[tag][i % count].activeInHierarchy)
				{
					this.positions[tag] = i % count;
					return this.pooledObjectsDictionary[tag][i % count];
				}
			}
			if (this.itemDictionary[tag].shouldExpand)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemDictionary[tag].objectToPool);
				gameObject.SetActive(false);
				gameObject.transform.SetParent(base.transform);
				this.pooledObjectsDictionary[tag].Add(gameObject);
				return gameObject;
			}
			return null;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000189DF File Offset: 0x00016BDF
		public List<GameObject> GetAllPooledObjects(string tag)
		{
			return this.pooledObjectsDictionary[tag];
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000189F0 File Offset: 0x00016BF0
		public void AddObject(string tag, GameObject GO, int amt = 3, bool exp = true)
		{
			if (this.pooledObjectsDictionary.ContainsKey(tag))
			{
				return;
			}
			ObjectPoolItem item = new ObjectPoolItem(tag, GO, amt, exp);
			int count = this.itemsToPool.Count;
			this.itemsToPool.Add(item);
			this.ObjectPoolItemToPooledObject(count);
			this.itemDictionary.Add(this.itemsToPool[count].tag, this.itemsToPool[count]);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00018A60 File Offset: 0x00016C60
		private void ObjectPoolItemToPooledObject(int index)
		{
			ObjectPoolItem objectPoolItem = this.itemsToPool[index];
			this.pooledObjects = new List<GameObject>();
			for (int i = 0; i < objectPoolItem.amountToPool; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(objectPoolItem.objectToPool);
				gameObject.SetActive(false);
				gameObject.transform.SetParent(base.transform);
				this.pooledObjects.Add(gameObject);
			}
			this.pooledObjectsDictionary.Add(objectPoolItem.tag, this.pooledObjects);
			this.positions.Add(objectPoolItem.tag, 0);
		}

		// Token: 0x040002D5 RID: 725
		public static ObjectPooler SharedInstance;

		// Token: 0x040002D6 RID: 726
		public List<ObjectPoolItem> itemsToPool;

		// Token: 0x040002D7 RID: 727
		public Dictionary<string, ObjectPoolItem> itemDictionary;

		// Token: 0x040002D8 RID: 728
		public Dictionary<string, List<GameObject>> pooledObjectsDictionary;

		// Token: 0x040002D9 RID: 729
		public List<GameObject> pooledObjects;

		// Token: 0x040002DA RID: 730
		private Dictionary<string, int> positions;
	}
}
