using System.Collections;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class DialogComponent : MonoBehaviour
    {
        public DialogModel dialogDetail;
        [SerializeField] private Image CharacterImage;
        [SerializeField] private Text nameCharacterDialog;
        [SerializeField] private Text textDialog;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button skipButton;
        private bool activeContinue = false;
        private bool skipAnimation = false;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine(NextTextDialog());
            continueButton.onClick.AddListener(() =>
            {
                if (!activeContinue) StartCoroutine(NextTextDialog());
                else skipAnimation = true;
            });
            skipButton.onClick.AddListener(() => { Destroy(gameObject); });
        }

        IEnumerator NextTextDialog()
        {
            activeContinue = true;
            textDialog.text = "";
            float dimBackground = 0f;
            string checkText = "";
            string animationName = "";
            Animator animation = null;
            Coroutine coroutine = null;
            try
            {
                checkText = dialogDetail.DialogDetail[dialogDetail.indexDialog].TextDialog;
                animation = dialogDetail.DialogDetail[dialogDetail.indexDialog].EventAnimation;
                dimBackground = dialogDetail.DialogDetail[dialogDetail.indexDialog].dimBackground;
                animationName = dialogDetail.DialogDetail[dialogDetail.indexDialog].AnimationName;
                CharacterImage.gameObject.SetActive(dialogDetail.DialogDetail[dialogDetail.indexDialog].ShowImageCharacter);
                CharacterImage.rectTransform.anchoredPosition = dialogDetail.GetPositionCharacter(dialogDetail.DialogDetail[dialogDetail.indexDialog].PositionCharacter, CharacterImage.rectTransform.anchoredPosition);
                CharacterImage.sprite = dialogDetail.GetCharacterDetail(dialogDetail.DialogDetail[dialogDetail.indexDialog].ElementCharacter).Item2;
                nameCharacterDialog.text = dialogDetail.GetCharacterDetail(dialogDetail.DialogDetail[dialogDetail.indexDialog].ElementCharacter).Item1;
                textDialog.transform.parent.GetComponent<RectTransform>().anchoredPosition = dialogDetail.GetPositionTextBox(dialogDetail.DialogDetail[dialogDetail.indexDialog].PositionBoxTextDialog, textDialog.rectTransform.anchoredPosition);
                textDialog.transform.parent.gameObject.SetActive(dialogDetail.DialogDetail[dialogDetail.indexDialog].ShowBoxTextDialog);
            }
            catch (System.Exception)
            {
                coroutine = StartCoroutine(PlayAnimation(animator, "Dialog Inactive Animation", true));
            }
            if (coroutine == null)
            {
                Color32 newColor = continueButton.gameObject.GetComponent<Image>().color;
                while (newColor.a != dimBackground)
                {
                    if (newColor.a < dimBackground) newColor.a++;
                    else newColor.a--;
                    continueButton.gameObject.GetComponent<Image>().color = newColor;
                    print($"{newColor.a} : {dimBackground} : {newColor.a != dimBackground}");
                    yield return null;
                }
                if (animation != null) yield return StartCoroutine(PlayAnimation(animation, animationName));
                yield return StartCoroutine(TextAnimation(textDialog, checkText));
                dialogDetail.indexDialog++;
                skipAnimation = false;
                activeContinue = false;
            }
        }

        IEnumerator TextAnimation(Text text, string textDialog)
        {
            foreach (char item in textDialog)
            {
                if (skipAnimation) break;
                text.text += item;
                yield return new WaitForSeconds(0.075f);
            }
            text.text = textDialog;
        }

        IEnumerator PlayAnimation(Animator animator, string animationName, bool destroy = false)
        {
            // Play the animation
            animator.Play(animationName, 0, 0f);

            int animationLength = animator.GetCurrentAnimatorClipInfo(0).Length;
            float timePassed = 0f;
            bool animationEnded = false;

            while (!skipAnimation && !animationEnded && timePassed <= animationLength)
            {
                timePassed += Time.deltaTime;
                if (timePassed >= animationLength)
                {
                    animationEnded = true;
                }

                //print($"{timePassed} : {animationLength} : {animationEnded}");
                yield return new WaitForEndOfFrame();
            }

            // After the wait for end of animation or fulfilled condition to skip, e.g. pressing the button, it should display last frame of animation
            animator.Play(animationName, 0, 1f);

            if ((destroy && animationEnded) || (destroy && skipAnimation)) Destroy(animator.gameObject);
        }

        IEnumerator PlayAnimation(Animation animation, string animationName)
        {
            AnimationClip animationClip = animation.GetClip(animationName);

            // Play the animation
            animation.Play(animationName);

            float timePassed = 0f;
            bool animationEnded = false;

            while (!skipAnimation && !animationEnded && timePassed <= animationClip.length)
            {
                timePassed += Time.deltaTime;
                if (timePassed >= animationClip.length)
                {
                    animationEnded = true;
                }

                yield return new WaitForEndOfFrame();
            }

            // After the wait for end of animation or fulfilled condition to skip, e.g. pressing the button, it should display last frame of animation
            animation[animationName].time = animationClip.length;
        }
    }
}
