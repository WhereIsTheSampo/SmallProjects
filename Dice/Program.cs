using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dice
{
    class Program
    {
        private const String PATTERN = @"(?=[-+*/])|(?<=[-+*/])";
        static void Main(string[] args)
        {
            String argument = "2d6+1-d8";

            String[] tokenized = Regex.Split(argument.ToLower(), PATTERN);

            foreach (String item in tokenized)
                Debug.WriteLine(item);

            foreach (var token in tokenized)
            {
                String[] split = token.Split('d');

                if (split.Length == 2)
                {
                    
                }
            }

        }
    }
}
