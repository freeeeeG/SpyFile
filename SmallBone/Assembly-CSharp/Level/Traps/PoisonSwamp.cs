using System;
using Characters;
using Characters.Operations.Attack;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000674 RID: 1652
	[ExecuteAlways]
	public class PoisonSwamp : Trap
	{
		// Token: 0x06002111 RID: 8465 RVA: 0x00063AEC File Offset: 0x00061CEC
		private void SetSize()
		{
			Vector2 size = this._spriteRenderer.size;
			size.x = (float)(this._size * 2);
			this._spriteRenderer.size = size;
			Vector2 size2 = this._collider.size;
			size2.x = (float)(this._size * 2) - 1.2f;
			this._collider.size = size2;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00063B4F File Offset: 0x00061D4F
		private void Awake()
		{
			this.SetSize();
			this._sweepAttack.Initialize();
			this._sweepAttackForPoison.Initialize();
			this._sweepAttack.Run(this._character);
			this._sweepAttackForPoison.Run(this._character);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x04001C28 RID: 7208
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001C29 RID: 7209
		[SerializeField]
		private Character _character;

		// Token: 0x04001C2A RID: 7210
		[SerializeField]
		private BoxCollider2D _collider;

		// Token: 0x04001C2B RID: 7211
		[SerializeField]
		private int _size = 1;

		// Token: 0x04001C2C RID: 7212
		[SerializeField]
		private SweepAttack _sweepAttack;

		// Token: 0x04001C2D RID: 7213
		[SerializeField]
		private SweepAttack _sweepAttackForPoison;
	}
}
