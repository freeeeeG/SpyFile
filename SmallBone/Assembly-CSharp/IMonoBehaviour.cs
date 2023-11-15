using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000048 RID: 72
public interface IMonoBehaviour
{
	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000144 RID: 324
	string name { get; }

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000145 RID: 325
	GameObject gameObject { get; }

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000146 RID: 326
	Transform transform { get; }

	// Token: 0x06000147 RID: 327
	T GetComponent<T>();

	// Token: 0x06000148 RID: 328
	T GetComponentInChildren<T>(bool includeInactive = false);

	// Token: 0x06000149 RID: 329
	T GetComponentInChildren<T>();

	// Token: 0x0600014A RID: 330
	T GetComponentInParent<T>();

	// Token: 0x0600014B RID: 331
	T[] GetComponents<T>();

	// Token: 0x0600014C RID: 332
	void GetComponents<T>(List<T> results);

	// Token: 0x0600014D RID: 333
	void GetComponentsInChildren<T>(List<T> results);

	// Token: 0x0600014E RID: 334
	T[] GetComponentsInChildren<T>(bool includeInactive);

	// Token: 0x0600014F RID: 335
	void GetComponentsInChildren<T>(bool includeInactive, List<T> result);

	// Token: 0x06000150 RID: 336
	T[] GetComponentsInChildren<T>();

	// Token: 0x06000151 RID: 337
	T[] GetComponentsInParent<T>();

	// Token: 0x06000152 RID: 338
	T[] GetComponentsInParent<T>(bool includeInactive);

	// Token: 0x06000153 RID: 339
	void GetComponentsInParent<T>(bool includeInactive, List<T> results);

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000154 RID: 340
	// (set) Token: 0x06000155 RID: 341
	bool enabled { get; set; }

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000156 RID: 342
	bool isActiveAndEnabled { get; }

	// Token: 0x06000157 RID: 343
	Coroutine StartCoroutine(IEnumerator routine);

	// Token: 0x06000158 RID: 344
	void StopAllCoroutines();

	// Token: 0x06000159 RID: 345
	void StopCoroutine(IEnumerator routine);

	// Token: 0x0600015A RID: 346
	void StopCoroutine(Coroutine routine);
}
