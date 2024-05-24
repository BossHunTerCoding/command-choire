using System;
using System.Collections;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommandChoice.Handler
{
    public class LoadSceneManager
    {
        private class LoadingMonoBehaviour : MonoBehaviour { }
        private static Action onLoaderCallback;
        private static AsyncOperation loadingAsyncOperation;

        public static void LoadScene(string sceneName)
        {
            onLoaderCallback = () =>
            {
                GameObject loadingGameObject = new("Loading...");
                loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(sceneName));
            };

            SceneManager.LoadScene(SceneMenu.Loading.ToString());
        }

        private static IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return null;
            loadingAsyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!loadingAsyncOperation.isDone)
            {
                yield return null;
            }
        }

        public static float GetLoadingProgress()
        {
            if (loadingAsyncOperation != null)
            {
                return loadingAsyncOperation.progress;
            }
            else
            {
                return 0f;
            }
        }

        public static void LoaderCallback()
        {
            if (onLoaderCallback != null)
            {
                onLoaderCallback();
                onLoaderCallback = null;
            }
        }
    }
}