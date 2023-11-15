using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A4E RID: 2638
public class UIGameObjectPool
{
	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06004F6F RID: 20335 RVA: 0x001C0ED9 File Offset: 0x001BF0D9
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06004F70 RID: 20336 RVA: 0x001C0EE6 File Offset: 0x001BF0E6
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x06004F71 RID: 20337 RVA: 0x001C0EF3 File Offset: 0x001BF0F3
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06004F72 RID: 20338 RVA: 0x001C0F02 File Offset: 0x001BF102
	public UIGameObjectPool(GameObject prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<GameObject>();
		this.activeElements = new List<GameObject>();
	}

	// Token: 0x06004F73 RID: 20339 RVA: 0x001C0F40 File Offset: 0x001BF140
	public GameObject GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			GameObject gameObject = this.freeElements[0];
			this.activeElements.Add(gameObject);
			if (gameObject.transform.parent != instantiateParent)
			{
				gameObject.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		GameObject gameObject2 = this.activeElements[this.activeElements.Count - 1];
		if (gameObject2.gameObject.activeInHierarchy != forceActive)
		{
			gameObject2.gameObject.SetActive(forceActive);
		}
		return gameObject2;
	}

	// Token: 0x06004F74 RID: 20340 RVA: 0x001C0FF8 File Offset: 0x001BF1F8
	public void ClearElement(GameObject element)
	{
		if (!this.activeElements.Contains(element))
		{
			object obj = this.freeElements.Contains(element) ? (element.name + ": The element provided is already inactive") : (element.name + ": The element provided does not belong to this pool");
			element.SetActive(false);
			if (this.disabledElementParent != null)
			{
				element.transform.SetParent(this.disabledElementParent);
			}
			global::Debug.LogError(obj);
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.transform.SetParent(this.disabledElementParent);
		}
		element.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06004F75 RID: 20341 RVA: 0x001C10B0 File Offset: 0x001BF2B0
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06004F76 RID: 20342 RVA: 0x001C112C File Offset: 0x001BF32C
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06004F77 RID: 20343 RVA: 0x001C113A File Offset: 0x001BF33A
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06004F78 RID: 20344 RVA: 0x001C1171 File Offset: 0x001BF371
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06004F79 RID: 20345 RVA: 0x001C11A8 File Offset: 0x001BF3A8
	public void ForEachActiveElement(Action<GameObject> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06004F7A RID: 20346 RVA: 0x001C11B6 File Offset: 0x001BF3B6
	public void ForEachFreeElement(Action<GameObject> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04003400 RID: 13312
	private GameObject prefab;

	// Token: 0x04003401 RID: 13313
	private List<GameObject> freeElements = new List<GameObject>();

	// Token: 0x04003402 RID: 13314
	private List<GameObject> activeElements = new List<GameObject>();

	// Token: 0x04003403 RID: 13315
	public Transform disabledElementParent;
}
