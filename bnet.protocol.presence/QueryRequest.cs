using System;
using System.Collections.Generic;
using System.IO;

namespace bnet.protocol.presence
{
	public class QueryRequest : IProtoBuf
	{
		private List<FieldKey> _Key = new List<FieldKey>();

		public EntityId EntityId
		{
			get;
			set;
		}

		public List<FieldKey> Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				this._Key = value;
			}
		}

		public List<FieldKey> KeyList
		{
			get
			{
				return this._Key;
			}
		}

		public int KeyCount
		{
			get
			{
				return this._Key.get_Count();
			}
		}

		public bool IsInitialized
		{
			get
			{
				return true;
			}
		}

		public void Deserialize(Stream stream)
		{
			QueryRequest.Deserialize(stream, this);
		}

		public static QueryRequest Deserialize(Stream stream, QueryRequest instance)
		{
			return QueryRequest.Deserialize(stream, instance, -1L);
		}

		public static QueryRequest DeserializeLengthDelimited(Stream stream)
		{
			QueryRequest queryRequest = new QueryRequest();
			QueryRequest.DeserializeLengthDelimited(stream, queryRequest);
			return queryRequest;
		}

		public static QueryRequest DeserializeLengthDelimited(Stream stream, QueryRequest instance)
		{
			long num = (long)((ulong)ProtocolParser.ReadUInt32(stream));
			num += stream.get_Position();
			return QueryRequest.Deserialize(stream, instance, num);
		}

		public static QueryRequest Deserialize(Stream stream, QueryRequest instance, long limit)
		{
			if (instance.Key == null)
			{
				instance.Key = new List<FieldKey>();
			}
			while (limit < 0L || stream.get_Position() < limit)
			{
				int num = stream.ReadByte();
				if (num == -1)
				{
					if (limit >= 0L)
					{
						throw new EndOfStreamException();
					}
					return instance;
				}
				else
				{
					int num2 = num;
					if (num2 != 10)
					{
						if (num2 != 18)
						{
							Key key = ProtocolParser.ReadKey((byte)num, stream);
							uint field = key.Field;
							if (field == 0u)
							{
								throw new ProtocolBufferException("Invalid field id: 0, something went wrong in the stream");
							}
							ProtocolParser.SkipKey(stream, key);
						}
						else
						{
							instance.Key.Add(FieldKey.DeserializeLengthDelimited(stream));
						}
					}
					else if (instance.EntityId == null)
					{
						instance.EntityId = EntityId.DeserializeLengthDelimited(stream);
					}
					else
					{
						EntityId.DeserializeLengthDelimited(stream, instance.EntityId);
					}
				}
			}
			if (stream.get_Position() == limit)
			{
				return instance;
			}
			throw new ProtocolBufferException("Read past max limit");
		}

		public void Serialize(Stream stream)
		{
			QueryRequest.Serialize(stream, this);
		}

		public static void Serialize(Stream stream, QueryRequest instance)
		{
			if (instance.EntityId == null)
			{
				throw new ArgumentNullException("EntityId", "Required by proto specification.");
			}
			stream.WriteByte(10);
			ProtocolParser.WriteUInt32(stream, instance.EntityId.GetSerializedSize());
			EntityId.Serialize(stream, instance.EntityId);
			if (instance.Key.get_Count() > 0)
			{
				using (List<FieldKey>.Enumerator enumerator = instance.Key.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FieldKey current = enumerator.get_Current();
						stream.WriteByte(18);
						ProtocolParser.WriteUInt32(stream, current.GetSerializedSize());
						FieldKey.Serialize(stream, current);
					}
				}
			}
		}

		public uint GetSerializedSize()
		{
			uint num = 0u;
			uint serializedSize = this.EntityId.GetSerializedSize();
			num += serializedSize + ProtocolParser.SizeOfUInt32(serializedSize);
			if (this.Key.get_Count() > 0)
			{
				using (List<FieldKey>.Enumerator enumerator = this.Key.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FieldKey current = enumerator.get_Current();
						num += 1u;
						uint serializedSize2 = current.GetSerializedSize();
						num += serializedSize2 + ProtocolParser.SizeOfUInt32(serializedSize2);
					}
				}
			}
			num += 1u;
			return num;
		}

		public void SetEntityId(EntityId val)
		{
			this.EntityId = val;
		}

		public void AddKey(FieldKey val)
		{
			this._Key.Add(val);
		}

		public void ClearKey()
		{
			this._Key.Clear();
		}

		public void SetKey(List<FieldKey> val)
		{
			this.Key = val;
		}

		public override int GetHashCode()
		{
			int num = base.GetType().GetHashCode();
			num ^= this.EntityId.GetHashCode();
			using (List<FieldKey>.Enumerator enumerator = this.Key.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FieldKey current = enumerator.get_Current();
					num ^= current.GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			QueryRequest queryRequest = obj as QueryRequest;
			if (queryRequest == null)
			{
				return false;
			}
			if (!this.EntityId.Equals(queryRequest.EntityId))
			{
				return false;
			}
			if (this.Key.get_Count() != queryRequest.Key.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this.Key.get_Count(); i++)
			{
				if (!this.Key.get_Item(i).Equals(queryRequest.Key.get_Item(i)))
				{
					return false;
				}
			}
			return true;
		}

		public static QueryRequest ParseFrom(byte[] bs)
		{
			return ProtobufUtil.ParseFrom<QueryRequest>(bs, 0, -1);
		}
	}
}
