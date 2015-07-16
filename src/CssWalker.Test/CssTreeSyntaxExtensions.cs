using CssSyntax.SyntaxTree;
using System;
using System.IO;

namespace CssSyntax.Test
{
    public static class CssTreeSyntaxExtensions
    {
        public static CssTreeWalker InterpretSyntax(this String css)
        {
            var reader = new StringReader(css);

            var walker = new CssTreeWalker();
            walker.Visit(reader);

            return walker;
        }

        public static SelectorSyntax[] ToSelectors(this String css)
        {
            return css.InterpretSyntax().Selectors;
        }

        public static AtRuleSyntax[] ToAtRules(this String css)
        {
            return css.InterpretSyntax().Medias;
        }

        public static CommentarySyntax[] ToComments(this String css)
        {
            return css.InterpretSyntax().Comments;
        }
    }
}