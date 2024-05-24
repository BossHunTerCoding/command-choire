using CommandChoice.Handler;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class LoadingScene : MonoBehaviour
    {
        private bool startLoading = true;
        [field: SerializeField] private Scrollbar imageProcess;

        private void Update()
        {
            if (startLoading)
            {
                startLoading = false;
                LoadSceneManager.LoaderCallback();
            }

            imageProcess.size = LoadSceneManager.GetLoadingProgress();
        }
    }
}

