using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New question")]
public class QuestionSO : ScriptableObject
{   
    [TextArea(2,6)]
    [SerializeField] string question = "";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIdx;

    public string GetQuestion() {
        return question;
    }

    public int GetCorrectAnswerIdx() {
        return correctAnswerIdx;
    }

    public string GetAnswers(int index) {
        return answers[index];
    }
}
