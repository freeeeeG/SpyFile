using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000562 RID: 1378
	public sealed class Pin : MonoBehaviour
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x00054545 File Offset: 0x00052745
		public bool spawned
		{
			get
			{
				return this._spawned;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0005454D File Offset: 0x0005274D
		public ICollection<Character> characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00054558 File Offset: 0x00052758
		public ICollection<Character> Load()
		{
			Enemy enemy = this._enemy;
			if (enemy == null)
			{
				Debug.LogError("Enemy is null");
				return null;
			}
			PinEnemySetting component = base.GetComponent<PinEnemySetting>();
			Enemy enemy2 = UnityEngine.Object.Instantiate<Enemy>(enemy, base.transform.position, Quaternion.identity, base.transform.parent);
			foreach (Behavior behavior in enemy2.behaviors)
			{
				behavior.Start();
				behavior.DisableBehavior(true);
			}
			foreach (Character character in enemy2.characters)
			{
				if (this._lookLeft)
				{
					character.gameObject.AddComponent<LookLeft>();
				}
				if (component)
				{
					component.ApplyTo(character);
				}
			}
			this._spawned = true;
			this._characters = enemy2.characters;
			return enemy2.characters;
		}

		// Token: 0x04001751 RID: 5969
		[SerializeField]
		private Key _key;

		// Token: 0x04001752 RID: 5970
		[SerializeField]
		private Enemy _enemy;

		// Token: 0x04001753 RID: 5971
		[SerializeField]
		[ReadOnly(false)]
		private bool _lookLeft;

		// Token: 0x04001754 RID: 5972
		private bool _spawned;

		// Token: 0x04001755 RID: 5973
		private ICollection<Character> _characters;
	}
}
