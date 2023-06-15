using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;
    
    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;
    
    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    
    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    Score score;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;
    public bool isQuizComplete;
    
    // Start is called before the first frame update
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        score = FindObjectOfType<Score>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update() {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion) {
            if(progressBar.value == progressBar.maxValue) {
                isQuizComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion){
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index){
        Debug.Log("Clicked" +index);
        
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + score.CalculateScore() + "%";
    }

    private void GetNextQuestion(){
        if(questions.Count > 0){
            SetButtonState(true);
            SetDefaultBtnSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            score.IncrementQuestionSeen();
        }
    }

    void GetRandomQuestion(){
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        
        if(questions.Contains(currentQuestion))
            questions.Remove(currentQuestion);
    }

    private void DisplayQuestion() {
        questionText.text = currentQuestion.GetQuestion();
        TextMeshProUGUI buttonText;

        for(int i = 0; i < answerButtons.Length; i++) {
            buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswers(i);
        }
    }

    private void DisplayAnswer(int index) {
        if(index == currentQuestion.GetCorrectAnswerIdx()){
            questionText.text = "Correct";
            Image buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            score.IncrementCorrectAnswers();
        } else {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIdx();
            questionText.text = "Sorry, that's incorrect. The answer is:\n" + currentQuestion.GetAnswers(correctAnswerIndex);
            Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    private void SetButtonState(bool state){
        foreach (GameObject button in answerButtons)
        {
            Button btn = button.GetComponent<Button>();
            btn.interactable = state;
        }
    }

    private void SetDefaultBtnSprites(){
        foreach(GameObject btn in answerButtons){
            Image btnImage = btn.GetComponent<Image>();
            btnImage.sprite = defaultAnswerSprite;
        }
    }
}
