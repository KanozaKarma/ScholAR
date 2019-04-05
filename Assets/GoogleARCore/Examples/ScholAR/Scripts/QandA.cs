using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleARCore.Examples.ScholAR
{
    class QandA
    {
        private int questionNo;

        private string question;

        private string answer;

        private bool correct;

        public QandA(int Number, string Question, string Answer, bool Correct)
        {
            questionNo = Number;
            question = Question;
            answer = Answer;
            correct = Correct;
        }

        public void SetQuestionNo (int Number)
        {
            questionNo = Number;
        }

        public void SetQuestion (string Question)
        {
            question = Question;
        }

        public void SetAnswer (string Answer)
        {
            answer = Answer;
        }

        public void SetCorrect (bool Correct)
        {
            correct = Correct;
        }

        public int GetQuestionNo()
        {
            return (questionNo);
        }

        public string GetQuestion()
        {
            return (question);
        }

        public string GetAnswer()
        {
            return (answer);
        }

        public bool GetCorrect()
        {
            return (correct);
        }
    }
}
