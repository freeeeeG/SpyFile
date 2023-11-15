using System;
using System.Collections.Generic;
using Characters;

// Token: 0x0200005E RID: 94
public class SummonWave : Wave
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060001C0 RID: 448 RVA: 0x000084DF File Offset: 0x000066DF
	public List<Character> characters
	{
		get
		{
			return this._characters;
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00002191 File Offset: 0x00000391
	public override void Initialize()
	{
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x000084E8 File Offset: 0x000066E8
	public void Attach(Character character)
	{
		this._characters.Add(character);
		character.health.onDiedTryCatch += delegate()
		{
			this.Detach(character);
		};
		if (base.state == Wave.State.Spawned)
		{
			return;
		}
		base.state = Wave.State.Spawned;
		Action onSpawn = this._onSpawn;
		if (onSpawn == null)
		{
			return;
		}
		onSpawn();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00008557 File Offset: 0x00006757
	private void Detach(Character character)
	{
		this._characters.Remove(character);
		if (this._characters.Count == 0)
		{
			base.state = Wave.State.Cleared;
			Action onClear = this._onClear;
			if (onClear == null)
			{
				return;
			}
			onClear();
		}
	}

	// Token: 0x0400018F RID: 399
	private List<Character> _characters = new List<Character>();
}
