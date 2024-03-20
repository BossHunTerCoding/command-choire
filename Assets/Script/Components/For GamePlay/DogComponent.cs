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
    [SerializeField] float smoothMovement = 3f;
    DataGamePlay dataThisGame = new();

    void Start()
    {
        startPosition = transform.position;
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
        if (indexMovement >= dirMovement.Count) indexMovement = 0;
        Vector3 targetMove = new(dirMovement[indexMovement].transform.position.x, dirMovement[indexMovement].transform.position.y, transform.position.z);
        while (transform.position != targetMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMove, smoothMovement * Time.deltaTime);
            yield return null;
        }
        indexMovement++;
    }

    public void ResetGame()
    {
        indexMovement = 0;
        indexNavigator = 0;
        transform.position = startPosition;
        stepFootWalk = StartCoroutine(ShowNavigatorFootWalk());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(StaticText.TagPlayer))
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            player.TakeDamage();
        }
    }
}
