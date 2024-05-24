using CommandChoice.Data;
using UnityEngine;

namespace CommandChoice.Handler
{
    public class InitData : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DataGlobal.SettingGame.LoadSettings();
        }
    }
}
