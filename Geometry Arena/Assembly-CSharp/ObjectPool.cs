using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class ObjectPool : MonoBehaviour
{
	// Token: 0x060002E4 RID: 740 RVA: 0x00012106 File Offset: 0x00010306
	private void Awake()
	{
		ObjectPool.inst = this;
		this.open = true;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00012118 File Offset: 0x00010318
	private void PoolAdd(GameObject gameObject)
	{
		string name = gameObject.name;
		List<GameObject> list2;
		if (!this.pool.ContainsKey(name))
		{
			List<GameObject> list = new List<GameObject>();
			this.pool.Add(name, list);
			list2 = list;
		}
		else
		{
			list2 = this.pool[name];
		}
		list2.Add(gameObject);
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00012168 File Offset: 0x00010368
	public void Particle_GoPool(GameObject partObj)
	{
		if (!this.open)
		{
			Object.Destroy(partObj);
			return;
		}
		if (partObj.GetComponent<Particle>() == null)
		{
			Debug.LogError("不是粒子，不能进对象池");
			Object.Destroy(partObj);
			return;
		}
		if (partObj.transform.parent == null)
		{
			partObj.transform.parent = this.obj_Container_Particle.transform;
		}
		partObj.SetActive(false);
		this.PoolAdd(partObj.gameObject);
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x000121E0 File Offset: 0x000103E0
	public void Bullet_GoPool(GameObject bulletObj)
	{
		if (!this.open)
		{
			Object.Destroy(bulletObj);
			return;
		}
		Bullet component = bulletObj.GetComponent<Bullet>();
		if (component == null)
		{
			Debug.LogError("不是子弹，不能进入对象池");
			Object.Destroy(bulletObj);
			return;
		}
		bulletObj.SetActive(false);
		bulletObj.transform.localScale = Vector2.one;
		bulletObj.transform.position = new Vector2(198f, 0f);
		base.StartCoroutine(this.BulletGoPoolLater(component.gameObject));
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0001226B File Offset: 0x0001046B
	private IEnumerator BulletGoPoolLater(GameObject obj)
	{
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			yield return 0;
			num = i;
		}
		this.PoolAdd(obj);
		yield break;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00012281 File Offset: 0x00010481
	public void DamageFloatText_GoPool(GameObject dftObj)
	{
		if (!this.open)
		{
			Object.Destroy(dftObj);
			return;
		}
		dftObj.GetComponent<FloatDamageText>();
		dftObj.SetActive(false);
		this.PoolAdd(dftObj);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x000122A7 File Offset: 0x000104A7
	public void BlastWave_GoPool(GameObject obj)
	{
		if (!this.open)
		{
			Object.Destroy(obj);
			return;
		}
		obj.GetComponent<Skill_Player8_Wave>();
		obj.transform.position = new Vector2(198f, 0f);
		this.PoolAdd(obj);
	}

	// Token: 0x060002EB RID: 747 RVA: 0x000122E8 File Offset: 0x000104E8
	private GameObject GetPool(string objName)
	{
		if (!this.open)
		{
			return null;
		}
		if (!this.pool.ContainsKey(objName))
		{
			return null;
		}
		List<GameObject> list = this.pool[objName];
		if (list.Count == 0)
		{
			return null;
		}
		GameObject result = list[0];
		list.RemoveAt(0);
		return result;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00012338 File Offset: 0x00010538
	public Bullet GetPool_Bullet(string objName)
	{
		GameObject gameObject = this.GetPool(objName);
		if (gameObject != null)
		{
			Bullet component = gameObject.GetComponent<Bullet>();
			component.gameObject.SetActive(true);
			component.Awake();
			return component;
		}
		return null;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00012370 File Offset: 0x00010570
	public Particle GetPool_Particle_GetOrNew(GameObject prefab)
	{
		GameObject gameObject = this.GetPool(prefab.GetComponent<Particle>().partName);
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			return gameObject.GetComponent<Particle>();
		}
		return Object.Instantiate<GameObject>(prefab).GetComponent<Particle>();
	}

	// Token: 0x060002EE RID: 750 RVA: 0x000123B4 File Offset: 0x000105B4
	public GameObject GetPool_FloatDamageText_GetOrNew(GameObject prefab)
	{
		GameObject gameObject = this.GetPool("FloatDamageText");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
			return gameObject;
		}
		return Object.Instantiate<GameObject>(prefab);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000123E8 File Offset: 0x000105E8
	public GameObject GetPool_BlastWave(GameObject prefab)
	{
		GameObject gameObject = this.GetPool("BlastWave");
		if (gameObject != null)
		{
			return gameObject;
		}
		return Object.Instantiate<GameObject>(prefab);
	}

	// Token: 0x040002AC RID: 684
	public static ObjectPool inst;

	// Token: 0x040002AD RID: 685
	[SerializeField]
	public GameObject obj_Container_Bullet;

	// Token: 0x040002AE RID: 686
	[SerializeField]
	private GameObject obj_Container_Particle;

	// Token: 0x040002AF RID: 687
	[SerializeField]
	private bool open;

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();
}
