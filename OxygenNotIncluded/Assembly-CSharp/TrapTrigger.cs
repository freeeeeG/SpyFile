using System;
using UnityEngine;

// Token: 0x02000A09 RID: 2569
public class TrapTrigger : KMonoBehaviour
{
	// Token: 0x06004CDC RID: 19676 RVA: 0x001AF0B4 File Offset: 0x001AD2B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameObject gameObject = base.gameObject;
		this.SetTriggerCell(Grid.PosToCell(gameObject));
		foreach (GameObject gameObject2 in this.storage.items)
		{
			this.SetStoredPosition(gameObject2);
			KBoxCollider2D component = gameObject2.GetComponent<KBoxCollider2D>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x001AF13C File Offset: 0x001AD33C
	public void SetTriggerCell(int cell)
	{
		HandleVector<int>.Handle handle = this.partitionerEntry;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Trap", base.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x001AF194 File Offset: 0x001AD394
	public void SetStoredPosition(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
		if (this.addTrappedAnimationOffset)
		{
			position.x += this.trappedOffset.x - component.Offset.x;
			position.y += this.trappedOffset.y - component.Offset.y;
		}
		else
		{
			position.x += this.trappedOffset.x;
			position.y += this.trappedOffset.y;
		}
		go.transform.SetPosition(position);
		component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
	}

	// Token: 0x06004CDF RID: 19679 RVA: 0x001AF25C File Offset: 0x001AD45C
	public void OnCreatureOnTrap(object data)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.storage.IsEmpty())
		{
			return;
		}
		Trappable trappable = (Trappable)data;
		if (trappable.HasTag(GameTags.Stored))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Trapped))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Creatures.Bagged))
		{
			return;
		}
		bool flag = false;
		foreach (Tag tag in this.trappableCreatures)
		{
			if (trappable.HasTag(tag))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		this.storage.Store(trappable.gameObject, true, false, true, false);
		this.SetStoredPosition(trappable.gameObject);
		base.Trigger(-358342870, trappable.gameObject);
	}

	// Token: 0x06004CE0 RID: 19680 RVA: 0x001AF316 File Offset: 0x001AD516
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04003229 RID: 12841
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400322A RID: 12842
	public Tag[] trappableCreatures;

	// Token: 0x0400322B RID: 12843
	public Vector2 trappedOffset = Vector2.zero;

	// Token: 0x0400322C RID: 12844
	public bool addTrappedAnimationOffset = true;

	// Token: 0x0400322D RID: 12845
	[MyCmpReq]
	private Storage storage;
}
