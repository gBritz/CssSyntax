using System;
using System.Collections.Generic;
using System.Linq;

namespace CssWalker.SyntaxTree
{
    public class CssTreeWalker : CssWalker
    {
        private readonly IList<AtRuleSyntax> medias = new List<AtRuleSyntax>();
        private readonly IList<SelectorSyntax> selectors = new List<SelectorSyntax>();
        private readonly IList<CommentarySyntax> comments = new List<CommentarySyntax>();

        private Boolean isOpenedMedia;
        private Boolean isOpenedSelector;

        public AtRuleSyntax[] Medias
        {
            get { return medias.ToArray(); }
        }

        public SelectorSyntax[] Selectors
        {
            get { return selectors.ToArray(); }
        }

        public CommentarySyntax[] Comments
        {
            get { return comments.ToArray(); }
        }

        protected override void VisitBeginMedia(string selector, int line, int column)
        {
            medias.Add(new AtRuleSyntax
            {
                Content = selector,
                StartAt = new Position(line, column)
            });

            isOpenedMedia = true;
        }

        protected override void VisitEndMedia(string selector, int line, int column)
        {
            var media = medias.Last();
            media.EndAt = new Position(line, column);
            isOpenedMedia = false;
        }

        protected override void VisitBeginComment(int line, int column)
        {
            var comments = GetCommentsInContext();

            comments.Add(new CommentarySyntax
            {
                StartAt = new Position(line, column)
            });
        }

        protected override void VisitEndComment(string content, int line, int column)
        {
            var comment = GetCommentsInContext().Last();

            comment.Content = content;
            comment.EndAt = new Position(line, column);
        }

        protected override void VisitBeginSelector(string selector, int line, int column)
        {
            var selectorsList = isOpenedMedia ? medias.Last().Selectors : selectors;

            selectorsList.Add(new SelectorSyntax
            {
                Content = selector,
                StartAt = new Position(line, column)
            });

            isOpenedSelector = true;
        }

        protected override void VisitEndSelector(string content, int line, int column)
        {
            var selectorsList = isOpenedMedia ? medias.Last().Selectors : selectors;
            var selector = selectorsList.Last();

            selector.EndAt = new Position(line, column);
            isOpenedSelector = false;
        }

        protected override void VisitProperty(string name, string value)
        {
            var selectorsList = isOpenedMedia ? medias.Last().Selectors : selectors;
            var selector = selectorsList.Last();

            selector.Properties.Add(new PropertySyntax
            {
                Name = name,
                Value = value
            });
        }

        private IList<CommentarySyntax> GetCommentsInContext()
        {
            var comments = this.comments;

            if (isOpenedMedia)
            {
                var media = medias.Last();
                comments = isOpenedSelector ? media.Selectors.Last().Comments : media.Comments;
            }
            else if (isOpenedSelector)
            {
                comments = selectors.Last().Comments;
            }

            return comments;
        }
    }
}