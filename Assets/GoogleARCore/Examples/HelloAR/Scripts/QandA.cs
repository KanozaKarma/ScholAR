using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleARCore.Examples.HelloAR
{
    class QandA
    {
        private int questionNo;

        private String question;

        private double answer;

        private bool correct;

        public QandA(int Number, String Question, double Answer, bool Correct)
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

        public void SetQuestion (String Question)
        {
            question = Question;
        }

        public void SetAnswer (double Answer)
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

        public String GetQuestion()
        {
            return (question);
        }

        public double GetAnswer()
        {
            return (answer);
        }

        public bool GetCorrect()
        {
            return (correct);
        }
    }
}
