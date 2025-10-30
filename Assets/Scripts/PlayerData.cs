using UnityEngine;

namespace VintageBeef
{
    /// <summary>
    /// Player data that persists across scenes
    /// </summary>
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance { get; private set; }

        public string PlayerName { get; set; } = "Player";
        public Profession SelectedProfession { get; set; }
        public int ProfessionIndex { get; set; } = -1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetProfession(int professionIndex)
        {
            ProfessionIndex = professionIndex;
            if (ProfessionManager.Instance != null)
            {
                SelectedProfession = ProfessionManager.Instance.GetProfession(professionIndex);
            }
        }
    }
}
