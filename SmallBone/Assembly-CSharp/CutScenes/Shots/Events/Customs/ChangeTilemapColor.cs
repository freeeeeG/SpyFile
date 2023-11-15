using System;
using System.Collections;
using System.Collections.Generic;
using Runnables;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CutScenes.Shots.Events.Customs
{
	// Token: 0x0200021A RID: 538
	public class ChangeTilemapColor : CRunnable
	{
		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001CF00 File Offset: 0x0001B100
		private static void StopSameChanges(Tilemap tilemap)
		{
			ChangeTilemapColor changeTilemapColor;
			if (!ChangeTilemapColor._changes.TryGetValue(tilemap, out changeTilemapColor))
			{
				return;
			}
			ChangeTilemapColor._changes.Remove(tilemap);
			changeTilemapColor.Stop();
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001CF2F File Offset: 0x0001B12F
		private static void StartChange(Tilemap tilemap, ChangeTilemapColor changeTilemapColor)
		{
			ChangeTilemapColor._changes.Add(tilemap, changeTilemapColor);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001CF3D File Offset: 0x0001B13D
		public void Stop()
		{
			this._interrupt = true;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001CF46 File Offset: 0x0001B146
		public override IEnumerator CRun()
		{
			ChangeTilemapColor.StopSameChanges(this._tilemap);
			ChangeTilemapColor.StartChange(this._tilemap, this);
			Color start = this._tilemap.color;
			float elapsed = 0f;
			this._interrupt = false;
			while (elapsed < this._curve.duration && !this._interrupt)
			{
				elapsed += Chronometer.global.deltaTime;
				this._tilemap.color = Color.Lerp(start, this._color, this._curve.Evaluate(elapsed));
				yield return null;
			}
			this._tilemap.color = this._color;
			yield break;
		}

		// Token: 0x040008AF RID: 2223
		[SerializeField]
		private Tilemap _tilemap;

		// Token: 0x040008B0 RID: 2224
		[SerializeField]
		private Color _color;

		// Token: 0x040008B1 RID: 2225
		[SerializeField]
		private Curve _curve;

		// Token: 0x040008B2 RID: 2226
		private static Dictionary<Tilemap, ChangeTilemapColor> _changes = new Dictionary<Tilemap, ChangeTilemapColor>();

		// Token: 0x040008B3 RID: 2227
		private bool _interrupt;
	}
}
