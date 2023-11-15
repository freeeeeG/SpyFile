using System;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class MB3_MeshBaker : MB3_MeshBakerCommon
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000257 RID: 599 RVA: 0x0001AAB5 File Offset: 0x00018EB5
	public override MB3_MeshCombiner meshCombiner
	{
		get
		{
			return this._meshCombiner;
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0001AABD File Offset: 0x00018EBD
	public void BuildSceneMeshObject()
	{
		this._meshCombiner.BuildSceneMeshObject(null, false);
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0001AACC File Offset: 0x00018ECC
	public virtual bool ShowHide(GameObject[] gos, GameObject[] deleteGOs)
	{
		return this._meshCombiner.ShowHideGameObjects(gos, deleteGOs);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0001AADB File Offset: 0x00018EDB
	public virtual void ApplyShowHide()
	{
		this._meshCombiner.ApplyShowHide();
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0001AAE8 File Offset: 0x00018EE8
	public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource)
	{
		this._meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjects(gos, deleteGOs, disableRendererInSource);
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0001AB13 File Offset: 0x00018F13
	public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource)
	{
		this._meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjectsByID(gos, deleteGOinstanceIDs, disableRendererInSource);
	}

	// Token: 0x04000187 RID: 391
	[SerializeField]
	protected MB3_MeshCombinerSingle _meshCombiner = new MB3_MeshCombinerSingle();
}
