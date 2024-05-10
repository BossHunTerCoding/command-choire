using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;

public class DogComponent : MonoBehaviour
{
    [SerializeField] int indexNavigator = 0;
    [SerializeField] int indexMovement = 0;
    [SerializeField] List<GameObject> dirMovement;
    [SerializeField] Coroutine stepFootWalk;
    GameObject footWalk;
    Vector2 startPosition;
    Quaternion startRotation;
    [SerializeField] bool canRotate = true;
    [SerializeField] int damage = 1;
    [SerializeField] float smoothMovement = 3f;
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        indexMovement = Random.Range(0, dirMovement.Count - 1);
        transform.position = new(dirMovement[indexMovement].transform.position.x, dirMovement[indexMovement].transform.position.y, transform.position.z);
        startPosition = transform.position;
        startRotation = transform.rotation;
        stepFootWalk = StartCoroutine(ShowNavigatorFootWalk());
    }

    public void StopFootWalkDog()
    {
        if (stepFootWalk == null) return;
        StopCoroutine(stepFootWalk);
        if (footWalk != null) Destroy(footWalk);
    }

    IEnumerator ShowNavigatorFootWalk()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (indexNavigator >= dirMovement.Count) indexNavigator = 0;
            footWalk = Instantiate(Resources.Load<GameObject>("GameObject/FootWalk Dog"), GameObject.Find("Grid").transform);
            footWalk.transform.position = new(dirMovement[indexNavigator].transform.position.x, dirMovement[indexNavigator].transform.position.y, transform.position.z + 1f);

            yield return new WaitForSeconds(1f);
            Destroy(footWalk);
            indexNavigator++;
        }
    }

    public IEnumerator Movement()
    {
        indexMovement++;
        if (indexMovement >= dirMovement.Count) indexMovement = 0;
        Vector3 targetMove = new(dirMovement[indexMovement].transform.position.x, dirMovement[indexMovement].transform.position.y, transform.position.z);
        gameObject.GetComponent<SpriteRenderer>().flipX = CheckFlipXEnemy(targetMove);
        if (canRotate) transform.rotation = CheckRotateEnemy(targetMove);
        while (transform.position != targetMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMove, smoothMovement * Time.deltaTime);
            yield return null;
        }
    }

    private bool CheckFlipXEnemy(Vector3 targetMove)
    {
        if (transform.position.x == targetMove.x) return gameObject.GetComponent<SpriteRenderer>().flipX;
        return transform.position.x < targetMove.x;
    }

    private Quaternion CheckRotateEnemy(Vector3 NewTargetMove)
    {
        float newRotate = 0f;
        if (transform.position.y < NewTargetMove.y) newRotate = -90f;
        else if (transform.position.y > NewTargetMove.y) newRotate = 90f;
        return new Quaternion(transform.rotation.x, transform.rotation.y, newRotate, newRotate);
    }

    public void ResetGame()
    {
        indexMovement = 0;
        indexNavigator = 0;
        transform.position = startPosition;
        transform.rotation = startRotation;
        stepFootWalk = StartCoroutine(ShowNavigatorFootWalk());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StaticText.TagPlayer))
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            damage = damage > 0 ? damage * -1 : damage;
            //MusicManagerComponent.PlaySoundTakeDamage(audioSource, player.HP <= 0);
            player.UpdateMail(damage);
            player.UpdateHP(damage);
            CommandManager.TriggerObjects(gameObject.tag);
        }
    }
}
