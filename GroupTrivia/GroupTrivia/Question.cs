using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupTrivia
{
    public class Question
    {
        private string _question;

        private List<string> _choices;

        public string question
        {
            get
            {
                return _question;
            }
        }

        public List<string> choices
        {
            get
            {
                return _choices;
            }
        }

        public Question(string question, List<string> options)
        {
            this._question = question;
            this._choices = options;
        }
    }
}