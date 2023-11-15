using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A4F RID: 2639
public class UIPool<T> where T : MonoBehaviour
{
	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x06004F7B RID: 20347 RVA: 0x001C11C4 File Offset: 0x001BF3C4
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06004F7C RID: 20348 RVA: 0x001C11D1 File Offset: 0x001BF3D1
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06004F7D RID: 20349 RVA: 0x001C11DE File Offset: 0x001BF3DE
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x06004F7E RID: 20350 RVA: 0x001C11ED File Offset: 0x001BF3ED
	public UIPool(T prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<T>();
		this.activeElements = new List<T>();
	}

	// Token: 0x06004F7F RID: 20351 RVA: 0x001C1228 File Offset: 0x001BF428
	public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			T t = this.freeElements[0];
			this.activeElements.Add(t);
			if (t.transform.parent != instantiateParent)
			{
				t.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		T t2 = this.activeElements[this.activeElements.Count - 1];
		if (t2.gameObject.activeInHierarchy != forceActive)
		{
			t2.gameObject.SetActive(forceActive);
		}
		return t2;
	}

	// Token: 0x06004F80 RID: 20352 RVA: 0x001C12F8 File Offset: 0x001BF4F8
	public void ClearElement(T element)
	{
		if (!this.activeElements.Contains(element))
		{
			global::Debug.LogError(this.freeElements.Contains(element) ? "The element provided is already inactive" : "The element provided does not belong to this pool");
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.gameObject.transform.SetParent(this.disabledElementParent);
		}
		element.gameObject.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x06004F81 RID: 20353 RVA: 0x001C1388 File Offset: 0x001BF588
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].gameObject.transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].gameObject.SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x06004F82 RID: 20354 RVA: 0x001C141B File Offset: 0x001BF61B
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06004F83 RID: 20355 RVA: 0x001C1429 File Offset: 0x001BF629
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06004F84 RID: 20356 RVA: 0x001C1460 File Offset: 0x001BF660
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06004F85 RID: 20357 RVA: 0x001C1497 File Offset: 0x001BF697
	public void ForEachActiveElement(Action<T> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06004F86 RID: 20358 RVA: 0x001C14A5 File Offset: 0x001BF6A5
	public void ForEachFreeElement(Action<T> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04003404 RID: 13316
	private T prefab;

	// Token: 0x04003405 RID: 13317
	private List<T> freeElements = new List<T>();

	// Token: 0x04003406 RID: 13318
	private List<T> activeElements = new List<T>();

	// Token: 0x04003407 RID: 13319
	public Transform disabledElementParent;
}
