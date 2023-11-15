using System;
using System.Collections.Generic;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class MoveSpawnMessage : Serialisable
{
	// Token: 0x06001CB7 RID: 7351 RVA: 0x0008C520 File Offset: 0x0008A920
	public void Initialise(KeyValuePair<GameObject, Transform>[] _spawns, Transform[] _spawnPoints)
	{
		MoveSpawnMessage.<Initialise>c__AnonStorey0 <Initialise>c__AnonStorey = new MoveSpawnMessage.<Initialise>c__AnonStorey0();
		<Initialise>c__AnonStorey._spawns = _spawns;
		this.m_players = new GameObject[<Initialise>c__AnonStorey._spawns.Length];
		this.m_indexes = new int[<Initialise>c__AnonStorey._spawns.Length];
		int i;
		for (i = 0; i < <Initialise>c__AnonStorey._spawns.Length; i++)
		{
			this.m_players[i] = <Initialise>c__AnonStorey._spawns[i].Key;
			this.m_indexes[i] = _spawnPoints.FindIndex_Predicate((Transform x) => x == <Initialise>c__AnonStorey._spawns[i].Value);
		}
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x0008C5DC File Offset: 0x0008A9DC
	public KeyValuePair<GameObject, Transform>[] ExtractSpawnMap(Transform[] _spawnPoints)
	{
		int num = this.m_players.Length;
		KeyValuePair<GameObject, Transform>[] array = new KeyValuePair<GameObject, Transform>[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new KeyValuePair<GameObject, Transform>(this.m_players[i], _spawnPoints[this.m_indexes[i]]);
		}
		return array;
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x0008C630 File Offset: 0x0008AA30
	public void Serialise(BitStreamWriter _writer)
	{
		_writer.Write((uint)this.m_players.Length, 3);
		for (int i = 0; i < this.m_players.Length; i++)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_players[i]);
			entry.m_Header.Serialise(_writer);
			_writer.Write((uint)this.m_indexes[i], 6);
		}
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x0008C690 File Offset: 0x0008AA90
	public bool Deserialise(BitStreamReader _reader)
	{
		int num = (int)_reader.ReadUInt32(3);
		this.m_players = new GameObject[num];
		this.m_indexes = new int[num];
		for (int i = 0; i < num; i++)
		{
			this.m_entityHeader.Deserialise(_reader);
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_entityHeader.m_uEntityID);
			this.m_players[i] = entry.m_GameObject;
			this.m_indexes[i] = (int)_reader.ReadUInt32(6);
		}
		return true;
	}

	// Token: 0x04001666 RID: 5734
	private const int c_BitsPerMapLength = 3;

	// Token: 0x04001667 RID: 5735
	private const int c_BitsPerSpawnPoint = 6;

	// Token: 0x04001668 RID: 5736
	private GameObject[] m_players;

	// Token: 0x04001669 RID: 5737
	private int[] m_indexes;

	// Token: 0x0400166A RID: 5738
	private EntityMessageHeader m_entityHeader = new EntityMessageHeader();
}
