using System;
using System.Collections.Generic;
using UnityEngine;

namespace VintageBeef
{
    /// <summary>
    /// Represents a profession that players can choose
    /// </summary>
    [Serializable]
    public class Profession
    {
        public string professionName;
        public string description;
        public Sprite icon;
        public Color themeColor;
        
        public Profession(string name, string desc)
        {
            professionName = name;
            description = desc;
            themeColor = Color.white;
        }
    }

    /// <summary>
    /// Manages all available professions in the game
    /// </summary>
    public class ProfessionManager : MonoBehaviour
    {
        public static ProfessionManager Instance { get; private set; }

        [SerializeField] private List<Profession> availableProfessions = new List<Profession>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeProfessions();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeProfessions()
        {
            // Initialize the 12 professions based on Vintage Story mechanics
            if (availableProfessions.Count == 0)
            {
                availableProfessions = new List<Profession>
                {
                    new Profession("Farmer", "Grow crops and tend to animals"),
                    new Profession("Blacksmith", "Forge tools and weapons"),
                    new Profession("Builder", "Construct buildings and structures"),
                    new Profession("Miner", "Extract resources from the earth"),
                    new Profession("Hunter", "Track and hunt wildlife"),
                    new Profession("Cook", "Prepare food and meals"),
                    new Profession("Tailor", "Craft clothing and armor"),
                    new Profession("Merchant", "Trade goods and resources"),
                    new Profession("Explorer", "Discover new lands and dungeons"),
                    new Profession("Engineer", "Build machines and contraptions"),
                    new Profession("Alchemist", "Create potions and elixirs"),
                    new Profession("Woodworker", "Craft items from wood")
                };
            }
        }

        public List<Profession> GetAllProfessions()
        {
            return new List<Profession>(availableProfessions);
        }

        public Profession GetProfession(int index)
        {
            if (index >= 0 && index < availableProfessions.Count)
            {
                return availableProfessions[index];
            }
            return null;
        }
    }
}
