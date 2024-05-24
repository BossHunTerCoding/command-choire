using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class PauseGamePanels : MonoBehaviour
    {
        [SerializeField] private GameObject PausePanel;
        [SerializeField] private GameObject ConfirmPanel;

        [SerializeField] private Button ContinueButton;
        [SerializeField] private Button BackButton;
        [SerializeField] private Button ConfirmButton;
        [SerializeField] private Button UnConfirmButton;

        void Start()
        {
            PausePanel.SetActive(true);
            ConfirmPanel.SetActive(false);

            ContinueButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                Destroy(gameObject);
            });
            BackButton.onClick.AddListener(() =>
            {
                PausePanel.SetActive(false);
                ConfirmPanel.SetActive(true);
            });
            ConfirmButton.onClick.AddListener(() => { Time.timeScale = 1; });
            UnConfirmButton.onClick.AddListener(() =>
            {
                PausePanel.SetActive(true);
                ConfirmPanel.SetActive(false);
            });
        }
    }
}
