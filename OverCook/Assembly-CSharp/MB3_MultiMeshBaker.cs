using System;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class MB3_MultiMeshBaker : MB3_MeshBakerCommon
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600029B RID: 667 RVA: 0x0001C41B File Offset: 0x0001A81B
	public override MB3_MeshCombiner meshCombiner
	{
		get
		{
			return this._meshCombiner;
		}
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0001C424 File Offset: 0x0001A824
	public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource)
	{
		if (this._meshCombiner.resultSceneObject == null)
		{
			this._meshCombiner.resultSceneObject = new GameObject("CombinedMesh-" + base.name);
		}
		this.meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjects(gos, deleteGOs, disableRendererInSource);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0001C490 File Offset: 0x0001A890
	public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOs, bool disableRendererInSource)
	{
		if (this._meshCombiner.resultSceneObject == null)
		{
			this._meshCombiner.resultSceneObject = new GameObject("CombinedMesh-" + base.name);
		}
		this.meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjectsByID(gos, deleteGOs, disableRendererInSource);
	}

	// Token: 0x040001AE RID: 430
	[SerializeField]
	protected MB3_MultiMeshCombiner _meshCombiner = new MB3_MultiMeshCombiner();
}
