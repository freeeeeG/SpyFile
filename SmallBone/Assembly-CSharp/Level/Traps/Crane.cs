using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000659 RID: 1625
	public class Crane : Trap
	{
		// Token: 0x060020AB RID: 8363 RVA: 0x0006293B File Offset: 0x00060B3B
		private void Awake()
		{
			this._onHitOperations.Initialize();
			this._character.health.onDie += this.Run;
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00062964 File Offset: 0x00060B64
		private void Run()
		{
			this._character.health.onDie -= this.Run;
			this._spriteRenderer.sprite = this._wreckage;
			base.StartCoroutine(this._onHitOperations.CRun(this._character));
		}

		// Token: 0x04001BAA RID: 7082
		[SerializeField]
		private Character _character;

		// Token: 0x04001BAB RID: 7083
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001BAC RID: 7084
		[SerializeField]
		private Sprite _wreckage;

		// Token: 0x04001BAD RID: 7085
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onHitOperations;
	}
}
