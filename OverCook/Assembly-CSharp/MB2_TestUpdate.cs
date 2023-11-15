using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class MB2_TestUpdate : MonoBehaviour
{
	// Token: 0x06000007 RID: 7 RVA: 0x000020F0 File Offset: 0x000004F0
	private void Start()
	{
		this.meshbaker.AddDeleteGameObjects(this.objsToMove, null, true);
		this.meshbaker.AddDeleteGameObjects(new GameObject[]
		{
			this.objWithChangingUVs
		}, null, true);
		MeshFilter component = this.objWithChangingUVs.GetComponent<MeshFilter>();
		this.m = component.sharedMesh;
		this.uvs = this.m.uv;
		this.meshbaker.Apply(null);
		this.multiMeshBaker.AddDeleteGameObjects(this.objsToMove, null, true);
		this.multiMeshBaker.AddDeleteGameObjects(new GameObject[]
		{
			this.objWithChangingUVs
		}, null, true);
		component = this.objWithChangingUVs.GetComponent<MeshFilter>();
		this.m = component.sharedMesh;
		this.uvs = this.m.uv;
		this.multiMeshBaker.Apply(null);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000021CC File Offset: 0x000005CC
	private void LateUpdate()
	{
		this.meshbaker.UpdateGameObjects(this.objsToMove, false, true, true, true, false, false, false, false, false);
		Vector2[] uv = this.m.uv;
		for (int i = 0; i < uv.Length; i++)
		{
			uv[i] = Mathf.Sin(Time.time) * this.uvs[i];
		}
		this.m.uv = uv;
		this.meshbaker.UpdateGameObjects(new GameObject[]
		{
			this.objWithChangingUVs
		}, true, true, true, true, true, false, false, false, false);
		this.meshbaker.Apply(false, true, true, true, true, false, false, false, false, false, false, null);
		this.multiMeshBaker.UpdateGameObjects(this.objsToMove, false, true, true, true, false, false, false, false, false);
		uv = this.m.uv;
		for (int j = 0; j < uv.Length; j++)
		{
			uv[j] = Mathf.Sin(Time.time) * this.uvs[j];
		}
		this.m.uv = uv;
		this.multiMeshBaker.UpdateGameObjects(new GameObject[]
		{
			this.objWithChangingUVs
		}, true, true, true, true, true, false, false, false, false);
		this.multiMeshBaker.Apply(false, true, true, true, true, false, false, false, false, false, false, null);
	}

	// Token: 0x04000018 RID: 24
	public MB3_MeshBaker meshbaker;

	// Token: 0x04000019 RID: 25
	public MB3_MultiMeshBaker multiMeshBaker;

	// Token: 0x0400001A RID: 26
	public GameObject[] objsToMove;

	// Token: 0x0400001B RID: 27
	public GameObject objWithChangingUVs;

	// Token: 0x0400001C RID: 28
	private Vector2[] uvs;

	// Token: 0x0400001D RID: 29
	private Mesh m;
}
