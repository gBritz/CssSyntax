using System;
using System.Collections.Generic;

namespace CssWalker.SyntaxTree
{
    public class MediaSyntax
    {
        public MediaSyntax()
        {
            Selectors = new List<SelectorSyntax>();
            Comments = new List<CommentarySyntax>();
        }

        public String Selector { get; set; }

        public Position StartAt { get; set; }

        public Position EndAt { get; set; }

        public IList<SelectorSyntax> Selectors { get; private set; }

        public IList<CommentarySyntax> Comments { get; private set; }
    }
}