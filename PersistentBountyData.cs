using System;
using System.Collections;
using WowJamMessages.MobileClientJSON;

public class PersistentBountyData
{
	private static PersistentBountyData s_instance;

	private Hashtable m_bountyDictionary;

	private bool m_bountiesAreVisible;

	private static PersistentBountyData instance
	{
		get
		{
			if (PersistentBountyData.s_instance == null)
			{
				PersistentBountyData.s_instance = new PersistentBountyData();
				PersistentBountyData.s_instance.m_bountyDictionary = new Hashtable();
				PersistentBountyData.s_instance.m_bountiesAreVisible = false;
			}
			return PersistentBountyData.s_instance;
		}
	}

	public static Hashtable bountyDictionary
	{
		get
		{
			return PersistentBountyData.instance.m_bountyDictionary;
		}
	}

	public static void SetBountiesVisible(bool visible)
	{
		PersistentBountyData.s_instance.m_bountiesAreVisible = visible;
	}

	public static bool BountiesAreVisible()
	{
		return PersistentBountyData.s_instance.m_bountiesAreVisible;
	}

	public static void AddOrUpdateBounty(MobileWorldQuestBounty bounty)
	{
		if (PersistentBountyData.instance.m_bountyDictionary.ContainsKey(bounty.QuestID))
		{
			PersistentBountyData.instance.m_bountyDictionary.Remove(bounty.QuestID);
		}
		PersistentBountyData.instance.m_bountyDictionary.Add(bounty.QuestID, bounty);
	}

	public static void ClearData()
	{
		PersistentBountyData.instance.m_bountyDictionary.Clear();
	}
}
