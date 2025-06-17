using UnityEngine;

public class Parallax : MonoBehaviour
{

    private MeshRenderer meshRenderer;

    public float animationSpeed = 0.5f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }
}
