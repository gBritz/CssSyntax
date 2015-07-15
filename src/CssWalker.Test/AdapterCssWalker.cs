using System;

namespace CssWalker.Test
{
    public class AdapterCssWalker : CssWalker
    {
        public Action<String, Int32, Int32> OnBeginSelector { get; set; }

        public Action<String, Int32, Int32> OnEndSelector { get; set; }

        protected override void VisitBeginSelector(string selector, int line, int column)
        {
            if (OnBeginSelector != null)
                OnBeginSelector(selector, line, column);
        }

        protected override void VisitEndSelector(string selector, int line, int column)
        {
            if (OnEndSelector != null)
                OnEndSelector(selector, line, column);
        }
    }
}