using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;

public class TroughComponent : MonoBehaviour
{
    [SerializeField] int countLostMail = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StaticText.TagPlayer))
        {
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            countLostMail = countLostMail > 0 ? countLostMail * -1 : countLostMail;
            player.UpdateMail(-countLostMail);
            CommandManager.TriggerObjects(gameObject.tag);
        }
    }
}
