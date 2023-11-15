using System;
using System.Collections.Generic;

// Token: 0x02000145 RID: 325
[Serializable]
public class GameBehaviorCollection
{
	// Token: 0x0600089F RID: 2207 RVA: 0x00017453 File Offset: 0x00015653
	public void Add(IGameBehavior behavior)
	{
		this.behaviors.Add(behavior);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00017461 File Offset: 0x00015661
	public void Remove(IGameBehavior behavior)
	{
		this.behaviors.Remove(behavior);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00017470 File Offset: 0x00015670
	public void RemoveAll()
	{
		foreach (IGameBehavior gameBehavior in this.behaviors)
		{
			Singleton<ObjectPool>.Instance.UnSpawn(gameBehavior as ReusableObject);
		}
		this.behaviors.Clear();
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x000174D8 File Offset: 0x000156D8
	public void GameUpdate()
	{
		for (int i = 0; i < this.behaviors.Count; i++)
		{
			if (!this.behaviors[i].GameUpdate())
			{
				int index = this.behaviors.Count - 1;
				this.behaviors[i] = this.behaviors[index];
				this.behaviors.RemoveAt(index);
				i--;
			}
		}
	}

	// Token: 0x04000481 RID: 1153
	public List<IGameBehavior> behaviors = new List<IGameBehavior>();
}
