using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    public bool isMine;
    public bool isRevealed;
    public bool isExploded;
    public bool isMarked;
    public AudioClip[] explosion;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField] private GameObject particleObj;
    BombCounter Count;
    public int adjacentMines;

    float newScale;
    float rdmSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Count = GameObject.Find("BombCount").GetComponent<BombCounter>();

    }

    private void Update()
    {
        float posy = transform.position.y;
        if(posy<-100) Destroy(gameObject);
        if (isExploded)
        {
            AugmentScaleByTime();
        }

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameManager.CellMarked(this);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gameManager.CellClicked(this);
        }
    }

    public void Reveal()
    {
        isRevealed = true;
        spriteRenderer.sprite = gameManager.emptySprite;
        spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void RevealNumber()
    {
        isRevealed = true;
        spriteRenderer.sprite = gameManager.numberSprites[adjacentMines - 1];
        spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void RevealMine()
    {
        isRevealed = true;
        spriteRenderer.sprite = gameManager.mineSprite;
        spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
    }
    public void RevealExplodedMine()
    {
        isRevealed = true;
        audioSource.clip = explosion[Random.Range(0, explosion.Length)];
        audioSource.volume = FindAnyObjectByType<Parameter>().soundVolume;
        audioSource.Play();
        spriteRenderer.sprite = gameManager.loseMineSprite;
        spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void ToggleMark()
    {
        isMarked = !isMarked;
        if (isMarked)
        {
            spriteRenderer.sprite = gameManager.flagSprite;
            Count.Count--;
        }
        else
        {
            spriteRenderer.sprite = gameManager.defaultSprite;
            Count.Count++;
        }
    }


    public void Explode()
    {
        gameObject.layer = 3;
        newScale = transform.localScale.x;
        rdmSpeed = Random.Range(0, 3);
        spriteRenderer.sortingOrder = 2;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5,5),Random.Range(0,5)),ForceMode2D.Impulse);
        isExploded= true;
        //GetComponent<Rigidbody2D>().angularVelocity = 3600;

        audioSource.clip = explosion[Random.Range(0, explosion.Length)];
        audioSource.volume = FindAnyObjectByType<Parameter>().soundVolume;
        audioSource.Play();

        particleObj.GetComponent<ParticleSystem>().Play();
    }
    private void AugmentScaleByTime()
    {
        
            newScale += rdmSpeed * Time.deltaTime;
            transform.localScale = new Vector2(newScale, newScale);

        
        
    }
}
