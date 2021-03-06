using System;
using System.Collections;
using UnityEngine;

namespace WowStaticData
{
	public class CurrencyTypesDB
	{
		private Hashtable m_records;

		public CurrencyTypesRec GetRecord(int id)
		{
			return (CurrencyTypesRec)this.m_records.get_Item(id);
		}

		public void EnumRecords(Predicate<CurrencyTypesRec> callback)
		{
			IEnumerator enumerator = this.m_records.get_Values().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CurrencyTypesRec currencyTypesRec = (CurrencyTypesRec)enumerator.get_Current();
					if (!callback.Invoke(currencyTypesRec))
					{
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public bool Load(string path, AssetBundle nonLocalizedBundle, AssetBundle localizedBundle, string locale)
		{
			string text = string.Concat(new string[]
			{
				path,
				locale,
				"/CurrencyTypes_",
				locale,
				".txt"
			});
			if (this.m_records != null)
			{
				Debug.Log("Already loaded static db " + text);
				return false;
			}
			TextAsset textAsset = localizedBundle.LoadAsset<TextAsset>(text);
			if (textAsset == null)
			{
				Debug.Log("Unable to load static db " + text);
				return false;
			}
			string text2 = textAsset.ToString();
			this.m_records = new Hashtable();
			int num = 0;
			int num2;
			do
			{
				num2 = text2.IndexOf('\n', num);
				if (num2 >= 0)
				{
					string valueLine = text2.Substring(num, num2 - num + 1).Trim();
					CurrencyTypesRec currencyTypesRec = new CurrencyTypesRec();
					currencyTypesRec.Deserialize(valueLine);
					this.m_records.Add(currencyTypesRec.ID, currencyTypesRec);
					num = num2 + 1;
				}
			}
			while (num2 > 0);
			return true;
		}
	}
}
