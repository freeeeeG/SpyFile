using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Gear.Weapons;
using FX.Connections;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FDB RID: 4059
	public class ConnectToTarget : TargetedCharacterOperation
	{
		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x000EB17A File Offset: 0x000E937A
		private MonoBehaviour coroutineReference
		{
			get
			{
				return this._connectionPool;
			}
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x000EB182 File Offset: 0x000E9382
		private void OnDisable()
		{
			this._connectings.Clear();
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x000EB190 File Offset: 0x000E9390
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			this.DisconnectIfConnected(target);
			Connection connection = this._connectionPool.GetConnection();
			Vector2 endOffset = new Vector2(0f, owner.collider.bounds.size.y * 0.5f);
			Vector2 startOffset = new Vector2(0f, target.collider.bounds.size.y * 0.5f);
			connection.Connect(target.transform, startOffset, owner.transform, endOffset);
			ConnectToTarget.ConnectionCoroutine connectionCoroutine = new ConnectToTarget.ConnectionCoroutine(this.coroutineReference, target, connection, this._duration);
			this.AddDisconnectAction(connectionCoroutine);
			this._connectings.Add(connectionCoroutine);
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x000EB254 File Offset: 0x000E9454
		private void DisconnectIfConnected(Character target)
		{
			ConnectToTarget.ConnectionCoroutine connectionCoroutine;
			if (this.TryGetConnectionCoroutine(target, out connectionCoroutine))
			{
				connectionCoroutine.connection.Disconnect();
			}
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x000EB278 File Offset: 0x000E9478
		private void AddDisconnectAction(ConnectToTarget.ConnectionCoroutine connectionCoroutine)
		{
			ConnectToTarget.<>c__DisplayClass10_0 CS$<>8__locals1 = new ConnectToTarget.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.connectionCoroutine = connectionCoroutine;
			CS$<>8__locals1.connectionCoroutine.connection.OnDisconnect += CS$<>8__locals1.<AddDisconnectAction>g__OnDisconnect|0;
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x000EB2B8 File Offset: 0x000E94B8
		private bool TryGetConnectionCoroutine(Character target, out ConnectToTarget.ConnectionCoroutine result)
		{
			foreach (ConnectToTarget.ConnectionCoroutine connectionCoroutine in this._connectings)
			{
				if (connectionCoroutine.target == target)
				{
					result = connectionCoroutine;
					return true;
				}
			}
			result = null;
			return false;
		}

		// Token: 0x04003E8D RID: 16013
		[SerializeField]
		[GetComponentInParent(false)]
		private Weapon _weapon;

		// Token: 0x04003E8E RID: 16014
		[SerializeField]
		private ConnectionPool _connectionPool;

		// Token: 0x04003E8F RID: 16015
		[SerializeField]
		private float _duration;

		// Token: 0x04003E90 RID: 16016
		private List<ConnectToTarget.ConnectionCoroutine> _connectings = new List<ConnectToTarget.ConnectionCoroutine>();

		// Token: 0x02000FDC RID: 4060
		private class ConnectionCoroutine
		{
			// Token: 0x17000F84 RID: 3972
			// (get) Token: 0x06004E7B RID: 20091 RVA: 0x000EB333 File Offset: 0x000E9533
			// (set) Token: 0x06004E7C RID: 20092 RVA: 0x000EB33B File Offset: 0x000E953B
			public Character target { get; private set; }

			// Token: 0x17000F85 RID: 3973
			// (get) Token: 0x06004E7D RID: 20093 RVA: 0x000EB344 File Offset: 0x000E9544
			// (set) Token: 0x06004E7E RID: 20094 RVA: 0x000EB34C File Offset: 0x000E954C
			public Connection connection { get; private set; }

			// Token: 0x17000F86 RID: 3974
			// (get) Token: 0x06004E7F RID: 20095 RVA: 0x000EB355 File Offset: 0x000E9555
			// (set) Token: 0x06004E80 RID: 20096 RVA: 0x000EB35D File Offset: 0x000E955D
			public Coroutine coroutine { get; private set; }

			// Token: 0x06004E81 RID: 20097 RVA: 0x000EB366 File Offset: 0x000E9566
			public ConnectionCoroutine(MonoBehaviour coroutineReference, Character target, Connection connection, float duration)
			{
				this.target = target;
				this.connection = connection;
				this.coroutine = coroutineReference.StartCoroutine(this.CRun(target.chronometer.master, duration));
			}

			// Token: 0x06004E82 RID: 20098 RVA: 0x000EB39B File Offset: 0x000E959B
			private IEnumerator CRun(Chronometer chronometer, float duration)
			{
				float elapsed = 0f;
				while (elapsed < duration && this.connection.connecting && this.target.liveAndActive)
				{
					elapsed += chronometer.deltaTime;
					yield return null;
				}
				if (this.connection.connecting)
				{
					this.connection.Disconnect();
				}
				yield break;
			}
		}
	}
}
