﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuizManager : MonoBehaviour
{
    [Tooltip("Reference to the GameManager")]
    public GameManager gameManager;
    [Tooltip("Reference to the SoundManager")]
    public SoundManager soundManager;
    private QuestionContainer questions;

    public List<QuestionInfo> questionsList = new List<QuestionInfo>();

    [Tooltip("The current question being asked")]
    public QuestionInfo currentQuestion;

    [Tooltip("Text for the current question")]
    public TextMeshProUGUI questionText;

    [Tooltip("Where the answers to the question get chosen")]
    public Dropdown answerChoices;

    [Tooltip("Reference to the quiz panel")]
    public GameObject quizPanel;
    
    /// <summary>
    /// How many questions have been asked
    /// </summary>
    private int questionsAsked;

    [Tooltip("How many points a correct question is worth, if it does not revive")]
    public int quizPoints = 10;
    // Start is called before the first frame update
    void Start()
    {
        TextAsset questionData = Resources.Load<TextAsset>("Quiz/questions");
        questions = JsonUtility.FromJson<QuestionContainer>(questionData.text);

        questionsList.AddRange(questions.Questions);

        //Debug.Log(questionsList.Count);
    }
    

    /// <summary>
    /// Returns a set question
    /// </summary>
    /// <param name="questionID">The location of the question to get</param>
    /// <returns>A question</returns>
    private QuestionInfo GetQuestion(int questionID)
    {
        QuestionInfo question = questionsList[questionID];
        questionsList.RemoveAt(questionID);
        return question;
    }

    /// <summary>
    /// Return a random question from the ones possible
    /// </summary>
    /// <returns>A random question</returns>
    public QuestionInfo GetRandomQuestion()
    {
        int rand = Random.Range(0, questionsList.Count);
        return GetQuestion(rand);
    }

    public void StartQuiz()
    {
        currentQuestion = NextQuestion();
        UpdateQuiz(currentQuestion);
    }
    
    public void UpdateQuiz(QuestionInfo question)
    {
        answerChoices.options.Clear();
        List<string> options = new List<string>();
        options.Add("SELECT");
        //questionText.text = question.Question;
        string text = question.Question + "\n";
        char character = 'A';
        for (int i = 0; i < question.Answers.Length; ++i)
        {
            text += $"{character}: {question.Answers[i]}\n";
            options.Add(character.ToString());
            ++character;
        }
        answerChoices.AddOptions(options);
        questionText.text = text;
    }
    
    public QuestionInfo NextQuestion()
    {
        quizPanel.SetActive(true);
        QuestionInfo question = GetQuestion(0);
        UpdateQuiz(question);
        ++questionsAsked;
        return question;
    }
    
    public bool CheckAnswer(int answer)
    {
        return answer == currentQuestion.Correct;
    }

    public void ConfirmSelection()
    {
        string result = "";

        if (CheckAnswer(answerChoices.value))
        {
            result += currentQuestion.PostAnswerTextCorrect;
            FMODUnity.RuntimeManager.PlayOneShot(soundManager.quizAffirmative);
        }
        else
        {
            result += currentQuestion.PostAnswerTextIncorrect;
            FMODUnity.RuntimeManager.PlayOneShot(soundManager.quizNegative);
        }

        //result += currentQuestion.Answers[currentQuestion.Correct];

        questionText.text = result;
    }
    
}
