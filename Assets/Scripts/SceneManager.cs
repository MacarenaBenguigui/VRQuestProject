using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class SceneManager : MonoBehaviour
{
    public enum Scene {
        SCENE1,
        SCENE2,
        SCENE3
    }

    [Header("Escena 1")]
    public GameObject ghostToyGhost;
    public GameObject potionBottle;
    public GameObject wooden;
    public AudioClip placeClip;
    public XRSocketInteractor ghostToyGhostSocket;
    public XRSocketInteractor potionBottleSocket;
    public XRSocketInteractor woodenSocket;

    private bool cubePlaced = false;
    private bool spherePlaced = false;
    private bool cylinderPlaced = false;
    private bool step1Completed = false;

    [Header("Escena 2")]
    public GameObject pumpking;
    public AudioClip pumpkingClip;
    private string animationStateName = "CapsuleAnimation";



    [System.Serializable]
    public class QuizData {
        public string question;
        public string[] answers = new string[4];
        public int correctIndex;
    }

    [Header("Escena 3")]
    public TextMeshProUGUI questionText;
    public GameObject canvasQuiz;
    public Button[] answerButtons; 
    public QuizData[] quizzes;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private int correctAnswer;
    private bool answered;


    [Header("Fade settings")]
    public RawImage fadeImage;
    public Material fadeMaterial;
    public float fadeDuration = 1f;        
    public GameObject completedPanel;       
    public TextMeshProUGUI completedPanelText;       
    public GameObject nextLevelButton;
    public GameObject restartButton;


    public AudioSource mainAudioSource;
    public Scene currentScene = Scene.SCENE1;
    public GameObject scene1Objects;
    public GameObject scene2Objects;
    public GameObject scene3Objects;


    void Start() {

        fadeMaterial.SetFloat("_Fade", 0f);
        scene1Objects.SetActive(true);
        scene2Objects.SetActive(false);
        scene3Objects.SetActive(false);
        currentScene = Scene.SCENE1;

        ghostToyGhostSocket.selectEntered.AddListener(ObjectPlaced);
        potionBottleSocket.selectEntered.AddListener(ObjectPlaced);
        woodenSocket.selectEntered.AddListener(ObjectPlaced);

        int randomIndex = Random.Range(0, quizzes.Length);
        QuizData q = quizzes[randomIndex];

        questionText.text = q.question;
        correctAnswer = q.correctIndex;

        for (int i = 0; i < answerButtons.Length; i++) {
            int index = i; 
            answerButtons[i].transform.Find("Text").GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }

    }

  

    public IEnumerator FadeAndSwitch() {
        yield return StartCoroutine(Fade(0f, 0.9f));
        completedPanel.SetActive(true);
        completedPanel.GetComponent<RectTransform>().DOScale(1f, 0.5f);
        nextLevelButton.SetActive(true);
        nextLevelButton.GetComponent<RectTransform>().DOScale(1f, 0.5f);
        if (currentScene == Scene.SCENE1) {
            completedPanelText.text = "¡Primera prueba completada!";
        } else if (currentScene == Scene.SCENE2) {
            completedPanelText.text = "¡Segunda prueba completada!";
            scene1Objects.SetActive(false);
            scene2Objects.SetActive(false);
            scene3Objects.SetActive(true);
            currentScene = Scene.SCENE3;
        } else if (currentScene == Scene.SCENE3) {
            completedPanelText.text = "¡Felicidades! Has completado todas las pruebas";
            nextLevelButton.SetActive(false);
            restartButton.SetActive(true);
            restartButton.GetComponent<RectTransform>().DOScale(1f, 0.5f);

        }
    }


    public IEnumerator NextLevel() {
        completedPanel.GetComponent<RectTransform>().DOScale(0f, 0.5f);
        nextLevelButton.GetComponent<RectTransform>().DOScale(0f, 0.5f);
        yield return StartCoroutine(Fade(0.9f, 0f));
        if (currentScene == Scene.SCENE1) {
            scene1Objects.SetActive(false);
            scene2Objects.SetActive(true);
            scene3Objects.SetActive(false);
            currentScene = Scene.SCENE2;
        } else if (currentScene == Scene.SCENE3) {
            canvasQuiz.GetComponent<RectTransform>().DOScale(0.002f, 0.6f);
        }

    }

    private IEnumerator Fade(float from, float to) {
        float elapsed = 0f;
        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float value = Mathf.Lerp(from, to, t);
            fadeMaterial.SetFloat("_Fade", value);
            yield return null;
        }
        fadeMaterial.SetFloat("_Fade", to);
    }

    public void NextLevelButton() {
        StartCoroutine(NextLevel());
    }

    public void RestartExperience() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    #region Scene1
    private void ObjectPlaced(SelectEnterEventArgs args) {
        if (args.interactableObject.transform.gameObject == ghostToyGhost) {
            cubePlaced = true;
            ghostToyGhost.transform.position = ghostToyGhostSocket.transform.position + new Vector3(0f, 0.25f, 0f);
            ghostToyGhost.transform.rotation = ghostToyGhostSocket.transform.rotation;
            ghostToyGhostSocket.transform.GetChild(0).gameObject.SetActive(false);
            ghostToyGhostSocket.transform.GetComponent<FadeMaterial>().StopBlink();
            ghostToyGhost.GetComponent<XRGrabInteractable>().enabled = false;
            FadeMaterial.instance.StopAllCoroutines();
        } else if(args.interactableObject.transform.gameObject == wooden) {
            cylinderPlaced = true;
            wooden.transform.position = woodenSocket.transform.position;
            wooden.transform.rotation = woodenSocket.transform.rotation;
            woodenSocket.transform.GetChild(0).gameObject.SetActive(false);
            woodenSocket.transform.GetComponent<FadeMaterial>().StopBlink();
            wooden.GetComponent<XRGrabInteractable>().enabled = false;
            FadeMaterial.instance.StopAllCoroutines();
        } else if(args.interactableObject.transform.gameObject == potionBottle) {
            spherePlaced = true;
            potionBottle.transform.position = potionBottleSocket.transform.position;
            potionBottle.transform.rotation = potionBottleSocket.transform.rotation;
            potionBottleSocket.transform.GetChild(0).gameObject.SetActive(false);
            potionBottleSocket.transform.GetComponent<FadeMaterial>().StopBlink();
            potionBottle.GetComponent<XRGrabInteractable>().enabled = false;
            FadeMaterial.instance.StopAllCoroutines();
        }
        mainAudioSource.clip = placeClip;
        mainAudioSource.Play();
        CheckAllPlaced();
    }

    private void CheckAllPlaced() {
        if (!step1Completed && cubePlaced && spherePlaced && cylinderPlaced) {
            step1Completed = true;
            StartCoroutine(FadeAndSwitch());
        }
    }
    #endregion


    #region Scene2
    public void OnSelected() {
        pumpking.GetComponent<Animator>().SetTrigger("Start");
        mainAudioSource.clip = pumpkingClip;
        mainAudioSource.Play();
        StartCoroutine(WaitForAnimationThenSwitch());
    }

    private IEnumerator WaitForAnimationThenSwitch() {
        AnimatorStateInfo stateInfo = pumpking.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(animationStateName)) {
            yield return null;
            stateInfo = pumpking.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        }
        yield return new WaitForSeconds(stateInfo.length);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FadeAndSwitch());
    }

    #endregion

    #region Scene3

    void SelectAnswer(int index) {
        if (answered) {
            return;
        }
        answered = true;

        bool correct = index == correctAnswer;
        questionText.text = correct ? "<color=green>¡Respuesta correcta!</color>" : "<color=red>!Respuesta incorrecta!</color>";

        answerButtons[index].GetComponent<Image>().color = correct ? Color.green : Color.red;
        if (!correct) {
            answerButtons[correctAnswer].GetComponent<Image>().color = Color.green;
        }

        mainAudioSource.clip = correct ? correctSound : wrongSound;
        mainAudioSource.Play();
        foreach (Button buttonSelected in answerButtons) {
            buttonSelected.interactable = false;
        }
        canvasQuiz.GetComponent<RectTransform>().DOScale(0f, 0.5f).SetDelay(3).OnComplete(() => {
                   StartCoroutine(FadeAndSwitch());
        });
    }

    #endregion

}

