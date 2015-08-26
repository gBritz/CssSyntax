using System;

namespace CssSyntax.Test
{
    public class CssWalkerMock : CssWalker
    {
        public Action<String, Int32, Int32> OnVisitBeginAtRule { get; set; }
        public Action<Int32, Int32> OnVisitBeginComment { get; set; }
        public Action<String, Int32, Int32> OnVisitBeginSelector { get; set; }
        public Action<String, Int32, Int32> OnVisitEndAtRule { get; set; }
        public Action<String, Int32, Int32> OnVisitEndComment { get; set; }
        public Action<String, Int32, Int32> OnVisitEndSelector { get; set; }
        public Action<String, String> OnVisitProperty { get; set; }
        public Action<Int32, Int32> OnVisitBreakLine { get; set; }
        public Action<String> OnVisitText { get; set; }

        protected override void VisitBeginAtRule(string selector, int line, int column)
        {
            if (OnVisitBeginAtRule != null)
                OnVisitBeginAtRule(selector, line, column);
        }

        protected override void VisitBeginComment(int line, int column)
        {
            if (OnVisitBeginComment != null)
                OnVisitBeginComment(line, column);
        }

        protected override void VisitBeginSelector(string selector, int line, int column)
        {
            if (OnVisitBeginSelector != null)
                OnVisitBeginSelector(selector, line, column);
        }

        protected override void VisitEndAtRule(string selector, int line, int column)
        {
            if (OnVisitEndAtRule != null)
                OnVisitEndAtRule(selector, line, column);
        }

        protected override void VisitEndComment(string comment, int line, int column)
        {
            if (OnVisitEndComment != null)
                OnVisitEndComment(comment, line, column);
        }

        protected override void VisitEndSelector(string selector, int line, int column)
        {
            if (OnVisitEndSelector != null)
                OnVisitEndSelector(selector, line, column);
        }

        protected override void VisitProperty(string name, string value)
        {
            if (OnVisitProperty != null)
                OnVisitProperty(name, value);
        }

        protected override void VisitBreakLine(int line, int column)
        {
            if (OnVisitBreakLine != null)
                OnVisitBreakLine(line, column);
        }

        protected override void VisitText(string text)
        {
            if (OnVisitText != null)
                OnVisitText(text);
        }
    }
}