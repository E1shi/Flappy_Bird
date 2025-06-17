using System.Net.Http.Headers;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Sprite[] currentSkinSprites; // Untuk menyimpan sprite skin aktif
    private bool isUsingCustomSkin = false;

    public AudioSource flySfx;
    public AudioSource deadSfx;

    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Load skin yang dipilih terakhir kali
        int savedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", -1);
        
        // Validasi index sebelum digunakan
        if (savedSkinIndex != -1 && savedSkinIndex < SkinManager.Instance.allSkins.Length)
        {
            ChangeSkin(SkinManager.Instance.allSkins[savedSkinIndex].animationFrames);
        }
        
        // JALANKAN ANIMASI APAPUN KEADAANNYA
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);

        /*
        // Load skin yang dipilih terakhir kali
        int savedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", -1);
        if (savedSkinIndex != -1)
        {
            // Implementasikan logic untuk load skin sesuai savedSkinIndex
            // Contoh: ChangeSkin(availableSkins[savedSkinIndex]);
        }
        else
        {
             InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        //Input gerakan buat pc
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                direction = Vector3.up * strength;

                flySfx.Play();
            }

        //Input gerakan buat mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
                flySfx.Play();
            }
        }

        //Biar burungnya ketarik kebawah terus
        direction.y += gravity * Time.deltaTime;

        //Biar posisi terakhir burung ditambah direction
        transform.position += direction * Time.deltaTime;
        //transform.position = transform.position + direction * Time.deltaTime;
    }


    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            FindAnyObjectByType<GameManager>().GameOver();
            deadSfx.Play();
        }
        else if (other.gameObject.tag == "Score")
        {
            FindAnyObjectByType<GameManager>().IncreaseScore();
        }
    }


    public void ChangeSkin(Sprite[] skinSprites)
    {
        currentSkinSprites = skinSprites;
        isUsingCustomSkin = true;
        PlayerPrefs.SetInt("SelectedSkin", System.Array.IndexOf(sprites, skinSprites[0])); // Simpan index skin pertama
    }
    
    private void AnimateSprite()
    {
        if (isUsingCustomSkin)
        {
            // Gunakan animasi dari skin custom
            spriteIndex++;
            if (spriteIndex >= currentSkinSprites.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = currentSkinSprites[spriteIndex];
        }
        else
        {
            // Gunakan animasi default
            spriteIndex++;
            if (spriteIndex >= sprites.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }
}
