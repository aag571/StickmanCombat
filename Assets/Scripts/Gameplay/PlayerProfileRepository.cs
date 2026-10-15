using System;
using System.Collections.Generic;
using UnityEngine;

namespace StickmanCombat.Gameplay
{
    [Serializable]
    public sealed class PlayerProfile
    {
        public RankProgress rank = new RankProgress();
        public bool tutorialCompleted;
        public int endlessBest;
        public List<string> pendingUnlocks = new List<string>();
    }

    public sealed class PlayerProfileRepository
    {
        private const string SaveKey = "WebPlayerProfile.v1";
        public PlayerProfile Profile { get; }

        public PlayerProfileRepository()
        {
            try { Profile = JsonUtility.FromJson<PlayerProfile>(PlayerPrefs.GetString(SaveKey, "")); }
            catch (ArgumentException) { Profile = null; }
            if (Profile == null) Profile = new PlayerProfile();
            if (Profile.rank == null) Profile.rank = new RankProgress();
            if (Profile.pendingUnlocks == null) Profile.pendingUnlocks = new List<string>();
            Profile.rank.Normalize();
            PlayerPrefs.DeleteKey("missionTypes");
            PlayerPrefs.DeleteKey("missionValues");
            PlayerPrefs.DeleteKey("BeltExam");
            Save();
        }

        public void Save()
        {
            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(Profile));
            PlayerPrefs.Save();
        }

        public void QueueUnlock(string name)
        {
            if (!Profile.pendingUnlocks.Contains(name)) Profile.pendingUnlocks.Add(name);
        }

        public string ConsumeUnlocks()
        {
            string message = string.Join("、", Profile.pendingUnlocks);
            Profile.pendingUnlocks.Clear();
            Save();
            return message;
        }
    }
}
