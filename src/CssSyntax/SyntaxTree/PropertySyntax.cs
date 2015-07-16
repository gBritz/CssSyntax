using System;

namespace CssSyntax.SyntaxTree
{
    public class PropertySyntax : ISyntax
    {
        public String Name { get; set; }

        public String Value { get; set; }

        public Position StartAt { get; set; }

        public Position EndAt { get; set; }

        public String Content
        {
            get { return Name + ":" + Value; }
        }
    }
}