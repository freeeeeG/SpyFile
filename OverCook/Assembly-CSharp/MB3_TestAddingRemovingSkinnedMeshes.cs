using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class MB3_TestAddingRemovingSkinnedMeshes : MonoBehaviour
{
	// Token: 0x0600000A RID: 10 RVA: 0x00002339 File Offset: 0x00000739
	private void Start()
	{
		base.StartCoroutine(this.TestScript());
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002348 File Offset: 0x00000748
	private IEnumerator TestScript()
	{
		Debug.Log("Test 1 adding 0,1,2");
		GameObject[] a2 = new GameObject[]
		{
			this.g[0],
			this.g[1],
			this.g[2]
		};
		this.meshBaker.AddDeleteGameObjects(a2, null, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 2 remove 1 and add 3,4,5");
		GameObject[] d = new GameObject[]
		{
			this.g[1]
		};
		a2 = new GameObject[]
		{
			this.g[3],
			this.g[4],
			this.g[5]
		};
		this.meshBaker.AddDeleteGameObjects(a2, d, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 3 remove 0,2,5 and add 1");
		d = new GameObject[]
		{
			this.g[3],
			this.g[4],
			this.g[5]
		};
		a2 = new GameObject[]
		{
			this.g[1]
		};
		this.meshBaker.AddDeleteGameObjects(a2, d, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 3 remove all remaining");
		d = new GameObject[]
		{
			this.g[0],
			this.g[1],
			this.g[2]
		};
		this.meshBaker.AddDeleteGameObjects(null, d, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 3 add all");
		this.meshBaker.AddDeleteGameObjects(this.g, null, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(1f);
		Debug.Log("Done");
		yield break;
	}

	// Token: 0x0400001E RID: 30
	public MB3_MeshBaker meshBaker;

	// Token: 0x0400001F RID: 31
	public GameObject[] g;
}
