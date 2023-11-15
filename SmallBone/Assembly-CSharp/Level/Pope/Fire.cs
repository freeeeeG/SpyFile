using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Operations;
using Characters.Operations.Attack;
using Level.Traps;
using UnityEditor;
using UnityEngine;

namespace Level.Pope
{
	// Token: 0x02000644 RID: 1604
	public class Fire : Trap
	{
		// Token: 0x06002038 RID: 8248 RVA: 0x00061A74 File Offset: 0x0005FC74
		private void Awake()
		{
			this._attack.Initialize();
			this._attack.Run(this._character);
			this._onAppear.Initialize();
			this._abilityAttacher.Initialize(this._character);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00061AC4 File Offset: 0x0005FCC4
		public void Appear()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this._onAppear.CRun(this._character));
			this._attack.Run(this._character);
			base.StartCoroutine(this.CAppear());
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00061B13 File Offset: 0x0005FD13
		public void Disappear()
		{
			this._attack.Stop();
			base.StartCoroutine(this.CDisappear());
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x00061B2D File Offset: 0x0005FD2D
		private IEnumerator CAppear()
		{
			int num = 3;
			int time = 6;
			float elapsed = 0f;
			Vector2 start = base.transform.position;
			Vector2 end = new Vector2(start.x, start.y + (float)num);
			while (elapsed < (float)time)
			{
				base.transform.position = Vector2.Lerp(start, end, elapsed / (float)time);
				elapsed += this._character.chronometer.master.deltaTime;
				yield return null;
			}
			base.transform.position = end;
			yield break;
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00061B3C File Offset: 0x0005FD3C
		private IEnumerator CDisappear()
		{
			int num = 4;
			int time = 4;
			float elapsed = 0f;
			Vector2 start = base.transform.position;
			Vector2 end = new Vector2(start.x, start.y - (float)num);
			while (elapsed < (float)time)
			{
				base.transform.position = Vector2.Lerp(start, end, elapsed / (float)time);
				elapsed += this._character.chronometer.master.deltaTime;
				yield return null;
			}
			base.transform.position = end;
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04001B50 RID: 6992
		[SerializeField]
		private Character _character;

		// Token: 0x04001B51 RID: 6993
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onAppear;

		// Token: 0x04001B52 RID: 6994
		[Subcomponent(typeof(SweepAttack))]
		[SerializeField]
		private SweepAttack _attack;

		// Token: 0x04001B53 RID: 6995
		[SerializeField]
		[AbilityAttacher.SubcomponentAttribute]
		private AbilityAttacher _abilityAttacher;
	}
}
