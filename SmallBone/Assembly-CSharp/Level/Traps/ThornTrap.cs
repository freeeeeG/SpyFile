using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000682 RID: 1666
	[ExecuteAlways]
	public class ThornTrap : Trap
	{
		// Token: 0x06002152 RID: 8530 RVA: 0x000643D8 File Offset: 0x000625D8
		private void SetSize()
		{
			Vector2 size = this._spriteRenderer.size;
			size.x = (float)(this._size * 2);
			this._spriteRenderer.size = size;
			Vector2 size2 = this._collider.size;
			size2.x = (float)(this._size * 2) - 1.2f;
			this._collider.size = size2;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0006443B File Offset: 0x0006263B
		private void Awake()
		{
			this.SetSize();
			this._operationInfos.Initialize();
			this._operationInfos.Run(this._character);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x04001C62 RID: 7266
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001C63 RID: 7267
		[SerializeField]
		private Character _character;

		// Token: 0x04001C64 RID: 7268
		[SerializeField]
		private BoxCollider2D _collider;

		// Token: 0x04001C65 RID: 7269
		[SerializeField]
		private int _size = 1;

		// Token: 0x04001C66 RID: 7270
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _operationInfos;
	}
}
