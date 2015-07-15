using System;

namespace CssWalker
{
    public abstract class CssContext
    {
        private readonly String selector;

        private Int32 beginLine;
        private Int32 beginColumn;
        private Int32 endLine;
        private Int32 endColumn;

        protected CssContext(String selector)
        {
            this.selector = selector;
        }

        public Action<String, Int32, Int32> OnOpen { get; set; }

        public Action<String, Int32, Int32> OnClose { get; set; }

        public CssContext ParentContext { get; private set; }

        public CssContext InnerContext { get; private set; }

        public String Selector
        {
            get { return selector; }
        }

        public void SetGraph(CssContext inner)
        {
            inner.ThrowIfNull("inner");

            InnerContext = inner;
            inner.ParentContext = this;
        }

        public abstract void Parse(String content);

        public virtual void Open(Int32 line, Int32 column)
        {
            beginLine = line;
            beginColumn = column;

            if (OnOpen != null)
                OnOpen(selector, beginLine, beginColumn);
        }

        public virtual void Close(Int32 line, Int32 column)
        {
            endLine = line;
            endColumn = column;

            if (OnClose != null)
                OnClose(selector, endLine, endColumn);
        }
    }
}