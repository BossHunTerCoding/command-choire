using UnityEngine;

public class FootStepWalk : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField] float listTime;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        listTime = sprite.color.a;
    }

    void Update()
    {
        
        if(listTime < 0) Destroy(gameObject);
        else {
            listTime -= Time.deltaTime;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, listTime);
        }
    }

    public void SetColor(Color color){
        sprite.color = color;
    }
}
