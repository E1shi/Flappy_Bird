using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    [System.Serializable]
    public class SkinData
    {
        public string skinName;
        public Sprite[] animationFrames; // [downflap, midflap, upflap]
    }

    public SkinData[] allSkins = new SkinData[3]; // Lebih terstruktur
    public Player player;
    public GameObject skinPopup; // Reference ke popup panel
    public GameObject menuPlayer;  // Player yang di menu (drag di Inspector)
    public static SkinManager Instance;
    public GameObject touchBlocker; // Drag panel TouchBlocker ke sini


    private void Awake()
    {
        PlayerPrefs.DeleteKey("SelectedSkin");
        PlayerPrefs.Save();

        Instance = this; // Inisialisasi singleton
        CloseSkinPopup();

        // PASTIKAN DEFAULT SKIN SELALU VALID
        if (PlayerPrefs.GetInt("SelectedSkin", -1) >= allSkins.Length)
        {
            PlayerPrefs.SetInt("SelectedSkin", 0); // Reset ke default
        }
    }
    void Start()
    {
        // Load skin terakhir yang dipilih
        int savedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        SelectSkin(savedSkinIndex);
    }

    public void SelectSkin(int skinIndex)
    {
        // CLAMP INDEX BIAR AMAN
        int safeIndex = Mathf.Clamp(skinIndex, 0, allSkins.Length - 1);
        
        PlayerPrefs.SetInt("SelectedSkin", safeIndex);
        player.ChangeSkin(allSkins[safeIndex].animationFrames);
        menuPlayer.GetComponent<SpriteRenderer>().sprite = allSkins[safeIndex].animationFrames[0];
    }

    public void OpenSkinPopup()
    {
        skinPopup.SetActive(true);
        touchBlocker.SetActive(true); // Aktifkan touch blocker
    }

    public void CloseSkinPopup()
    {
        skinPopup.SetActive(false);
        touchBlocker.SetActive(false); // Nonaktifkan touch blocker
    }
}