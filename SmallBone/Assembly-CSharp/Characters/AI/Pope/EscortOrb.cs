using System;
using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011F1 RID: 4593
	public class EscortOrb : MonoBehaviour
	{
		// Token: 0x06005A2F RID: 23087 RVA: 0x0010BF1C File Offset: 0x0010A11C
		private void Awake()
		{
			this._fire.Initialize();
			this._attack.Initialize();
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x0010BF34 File Offset: 0x0010A134
		private void OnEnable()
		{
			this._attack.Initialize();
			this._attack.Run(this._character);
			base.StartCoroutine(this.CStartFireLoop());
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x0010BF5F File Offset: 0x0010A15F
		public void Initialize(float startRadian)
		{
			this._elapsed = startRadian;
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x0010BF68 File Offset: 0x0010A168
		public void Move(float radius)
		{
			Vector3 v = this._pivot.transform.position - this._character.transform.position;
			this._elapsed += this._speed * this._character.chronometer.master.deltaTime;
			this._character.movement.MoveHorizontal(v + new Vector2(Mathf.Cos(this._elapsed), Mathf.Sin(this._elapsed)) * radius);
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x0010C000 File Offset: 0x0010A200
		private IEnumerator CStartFireLoop()
		{
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds(this._fire.duration);
				this._fire.Run(this._character);
			}
			yield break;
		}

		// Token: 0x040048D9 RID: 18649
		[SerializeField]
		private Character _character;

		// Token: 0x040048DA RID: 18650
		[SerializeField]
		[Subcomponent(typeof(SweepAttack))]
		private SweepAttack _attack;

		// Token: 0x040048DB RID: 18651
		[SerializeField]
		private EscortOrb.Fire _fire;

		// Token: 0x040048DC RID: 18652
		[SerializeField]
		private Transform _pivot;

		// Token: 0x040048DD RID: 18653
		[SerializeField]
		private float _speed;

		// Token: 0x040048DE RID: 18654
		private float _elapsed;

		// Token: 0x020011F2 RID: 4594
		[Serializable]
		private class Rotate
		{
			// Token: 0x040048DF RID: 18655
			[SerializeField]
			internal float speed;
		}

		// Token: 0x020011F3 RID: 4595
		[Serializable]
		private class Fire
		{
			// Token: 0x06005A36 RID: 23094 RVA: 0x0010C00F File Offset: 0x0010A20F
			internal void Initialize()
			{
				this.operationInfos.Initialize();
			}

			// Token: 0x06005A37 RID: 23095 RVA: 0x0010C01C File Offset: 0x0010A21C
			internal void Run(Character character)
			{
				this.operationInfos.gameObject.SetActive(true);
				this.operationInfos.Run(character);
			}

			// Token: 0x040048E0 RID: 18656
			[Subcomponent(typeof(OperationInfos))]
			[SerializeField]
			private OperationInfos operationInfos;

			// Token: 0x040048E1 RID: 18657
			[SerializeField]
			internal float duration;
		}
	}
}
