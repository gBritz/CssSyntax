﻿using System;
using System.Collections.Generic;

namespace CssSyntax.SyntaxTree
{
    public class AtRuleSyntax : ISyntax
    {
        public AtRuleSyntax()
        {
            Selectors = new List<SelectorSyntax>();
            Comments = new List<CommentarySyntax>();
        }

        public String Content { get; set; }

        public Position StartAt { get; set; }

        public Position EndAt { get; set; }

        public IList<SelectorSyntax> Selectors { get; private set; }

        public IList<CommentarySyntax> Comments { get; private set; }
    }
}