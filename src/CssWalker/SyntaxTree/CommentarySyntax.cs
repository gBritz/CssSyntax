using System;

namespace CssWalker.SyntaxTree
{
    public class CommentarySyntax
    {
        public String Content { get; set; }

        public Position StartAt { get; set; }

        public Position EndAt { get; set; }
    }
}