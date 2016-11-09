using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace GroupTrivia
{
    public class TriviaHub : Hub
    {
        private static int points;

        private static System.Timers.Timer pointTimer = new System.Timers.Timer(1000);

        private static List<Question> questionList = new List<Question>();

        private Question GetRandomQuestion()
        {
            Random rand = new Random();
            int index = rand.Next(0, questionList.Count);
            return questionList[index];
        }

        /// <summary>
        /// Read the question list from the trivia text file and put it into a list of questions
        /// </summary>
        private void BuildQuestionList()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"assets\trivia.txt");
            while((line = file.ReadLine()) != null)
            {
                //parse question and choices
                string[] tokens = line.Split(',');

                //need at least 3 tokens (a question and 2 options) to be a valid trivia question
                if (tokens.Length > 3)
                {
                    string question = tokens[0];
                    List<string> options = new List<string>();
                    for (var i = 0; i < tokens.Length; i++)
                    {
                        options.Add(tokens[i]);
                    }

                    Question q = new Question(question, options);
                    questionList.Add(q);
                }
            }
            file.Close();
        }

        /// <summary>
        /// Get a random question and send it to clients. 
        /// This will be private after we don't test it by calling it from client "ask" button
        /// </summary>
        public void GiveQuestion()
        {
            BuildQuestionList();
            Question q = GetRandomQuestion();
            Clients.All.displayQuestion(q.question, q.choices);
            PointsCountdown();
        }

        private void PointsCountdown()
        {
            points = 1000;
            pointTimer.Elapsed += handleTimer;
            pointTimer.Start();
        }

        private void handleTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            if(points > 0)
            {
                points -= 50;

                Clients.All.updatePoints(points);
            }
            else
            {
                pointTimer.Stop();
                pointTimer.Elapsed -= handleTimer;
            }
        }

        public void Hello()
        {
            Clients.All.hello();
        }
    }
}