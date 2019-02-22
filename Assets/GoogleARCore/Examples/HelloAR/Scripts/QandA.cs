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

        QandA(int Number, String Question, double Answer, bool Correct)
        {
            questionNo = Number;
            question = Question;
            answer = Answer;
            correct = Correct;
        }

        public void setQuestionNo (int Number)
        {
            questionNo = Number;
        }

        public void setQuestion (String Question)
        {
            question = Question;
        }

        public void setAnswer (double Answer)
        {
            answer = Answer;
        }

        public void setCorrect (bool Correct)
        {
            correct = Correct;
        }

        public int getQuestionNo()
        {
            return (questionNo);
        }

        public String getQuestion()
        {
            return (question);
        }

        public double getAnswer()
        {
            return (answer);
        }

        public bool getCorrect()
        {
            return (correct);
        }
    }
}
