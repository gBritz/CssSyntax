using System;

namespace CssSyntax.SyntaxTree
{
    public class CommentarySyntax : ISyntax
    {
        public String Content { get; set; }

        public Position StartAt { get; set; }

        public Position EndAt { get; set; }
    }
}