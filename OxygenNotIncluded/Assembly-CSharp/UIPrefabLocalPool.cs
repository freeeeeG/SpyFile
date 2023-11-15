using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class UIPrefabLocalPool
{
	// Token: 0x06001322 RID: 4898 RVA: 0x00065040 File Offset: 0x00063240
	public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
	{
		this.sourcePrefab = sourcePrefab;
		this.parent = parent;
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0006506C File Offset: 0x0006326C
	public GameObject Borrow()
	{
		GameObject gameObject;
		if (this.availableInstances.Count == 0)
		{
			gameObject = Util.KInstantiateUI(this.sourcePrefab, this.parent, true);
		}
		else
		{
			gameObject = this.availableInstances.First<KeyValuePair<int, GameObject>>().Value;
			this.availableInstances.Remove(gameObject.GetInstanceID());
		}
		this.checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
		gameObject.SetActive(true);
		gameObject.transform.SetAsLastSibling();
		return gameObject;
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x000650E6 File Offset: 0x000632E6
	public void Return(GameObject instance)
	{
		this.checkedOutInstances.Remove(instance.GetInstanceID());
		this.availableInstances.Add(instance.GetInstanceID(), instance);
		instance.SetActive(false);
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00065114 File Offset: 0x00063314
	public void ReturnAll()
	{
		foreach (KeyValuePair<int, GameObject> self in this.checkedOutInstances)
		{
			int num;
			GameObject gameObject;
			self.Deconstruct(out num, out gameObject);
			int key = num;
			GameObject gameObject2 = gameObject;
			this.availableInstances.Add(key, gameObject2);
			gameObject2.SetActive(false);
		}
		this.checkedOutInstances.Clear();
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x0006518C File Offset: 0x0006338C
	public IEnumerable<GameObject> GetBorrowedObjects()
	{
		return this.checkedOutInstances.Values;
	}

	// Token: 0x04000A4F RID: 2639
	public readonly GameObject sourcePrefab;

	// Token: 0x04000A50 RID: 2640
	public readonly GameObject parent;

	// Token: 0x04000A51 RID: 2641
	private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

	// Token: 0x04000A52 RID: 2642
	private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();
}
