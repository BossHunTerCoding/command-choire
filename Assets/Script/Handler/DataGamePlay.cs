using System.Collections.Generic;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Data
{
    [System.Serializable]
    public class DataGamePlay
    {
        public bool waitCoolDownJump = false;
        public bool playActionCommand = false;
        public bool activeSelectSkipToMode = false;
        public int percentScore = DataGlobal.ScoreDefault;
        [field: SerializeField] public List<GameObject> MailObjects { get; private set; } = new();
        [field: SerializeField] public List<GameObject> EnemyObjects { get; private set; } = new();
        [field: SerializeField] public List<GameObject> buttonGamePlay { get; private set; } = new();

        public DataGamePlay()
        {
            try
            {
                if (GameObject.FindGameObjectsWithTag(StaticText.TagMail).Length > 0)
                {
                    foreach (GameObject item in GameObject.FindGameObjectsWithTag(StaticText.TagMail))
                    {
                        MailObjects.Add(item);
                    };
                }
                if (GameObject.FindGameObjectsWithTag(StaticText.TagEnemy).Length > 0)
                {
                    foreach (GameObject item in GameObject.FindGameObjectsWithTag(StaticText.TagEnemy))
                    {
                        EnemyObjects.Add(item);
                    };
                }
                buttonGamePlay = new() { GameObject.Find("Right-Bottom"), GameObject.Find("Jump"), GameObject.Find("Add Command") };
            }
            catch (System.Exception) {/* Can Play Game */}
        }
    }
}