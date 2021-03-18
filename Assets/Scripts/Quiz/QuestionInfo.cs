[System.Serializable]
public class QuestionInfo
{
    public string Question;
    public string[] Answers;
    /// <summary>
    /// Which of the answers is correct. 0-indexed, top-to-bottom in JSON file
    /// </summary>
    public int Correct;
}