using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000814 RID: 2068
public class ServerSplattable : ServerSynchroniserBase
{
	// Token: 0x0600279C RID: 10140 RVA: 0x000BA2A8 File Offset: 0x000B86A8
	private void Awake()
	{
		this.m_splattable = base.gameObject.RequireComponent<Splattable>();
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		this.m_landscapeMask = 1 << LayerMask.NameToLayer("Ground");
		this.m_throwable = base.gameObject.RequireInterface<IThrowable>();
		this.m_throwable.RegisterLandedCallback(new GenericVoid<GameObject>(this.OnThrowableLanded));
		this.m_splattable.m_prefabIndex = UnityEngine.Random.Range(0, this.m_splattable.m_splatPrefab.Length - 1);
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_splattable.m_splatPrefab[this.m_splattable.m_prefabIndex]);
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000BA35B File Offset: 0x000B875B
	public override void UpdateSynchronising()
	{
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000BA360 File Offset: 0x000B8760
	private void Splat()
	{
		GridManager gridManager = GameUtils.GetGridManager(base.transform);
		GridIndex unclampedGridLocationFromPos = gridManager.GetUnclampedGridLocationFromPos(base.transform.position);
		GameObject gridOccupant = gridManager.GetGridOccupant(unclampedGridLocationFromPos);
		if (gridOccupant == null || gridOccupant.RequestInterface<HazardBase>() != null)
		{
			GameObject gameObject = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_splattable.m_splatPrefab[this.m_splattable.m_prefabIndex], base.transform.position, Quaternion.identity);
			this.m_splattable.gameObject.AddComponent<StaticGridLocation>();
			if (this.m_splattable.m_alignToGrid)
			{
				gameObject.transform.position = gridManager.GetPosFromGridLocation(unclampedGridLocationFromPos);
			}
		}
		NetworkUtils.DestroyObject(base.gameObject);
	}

	// Token: 0x0600279F RID: 10143 RVA: 0x000BA424 File Offset: 0x000B8824
	private void OnThrowableLanded(GameObject _object)
	{
		bool flag = (this.m_landscapeMask & 1 << _object.layer) != 0;
		if (flag)
		{
			this.Splat();
		}
	}

	// Token: 0x04001F22 RID: 7970
	private Splattable m_splattable;

	// Token: 0x04001F23 RID: 7971
	private LayerMask m_landscapeMask;

	// Token: 0x04001F24 RID: 7972
	private IThrowable m_throwable;

	// Token: 0x04001F25 RID: 7973
	private GridManager m_gridManager;
}
