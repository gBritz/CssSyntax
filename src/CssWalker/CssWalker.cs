using System;
using System.IO;
using System.Text;

namespace CssWalker
{
    public abstract class CssWalker
    {
        private const Char OpenScope = '{';
        private const Char CloseScope = '}';
        private const Char BreakLine = '\n';
        private readonly StringBuilder sb = new StringBuilder();

        private Int32 currentLine;
        private Int32 currentColumn;
        private CssContext context;
        private readonly CssCommentaryInterpreter commentary;

        public CssWalker()
        {
            commentary = new CssCommentaryInterpreter(VisitBeginComment, VisitEndComment);
        }

        public void Visit(TextReader reader)
        {
            reader.ThrowIfNull("reader");

            var buffer = new Char[4096];
            var readedLength = 0;

            while ((readedLength = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                ReadBuffer(buffer, readedLength);
            }
        }

        protected virtual void VisitBeginSelector(String selector, Int32 line, Int32 column) { }

        protected virtual void VisitEndSelector(String selector, Int32 line, Int32 column) { }

        protected virtual void VisitProperty(String name, String value) { }

        protected virtual void VisitBeginMedia(String media, Int32 line, Int32 column) { }

        protected virtual void VisitEndMedia(String media, Int32 line, Int32 column) { }

        protected virtual void VisitBeginComment(Int32 line, Int32 column) { }

        protected virtual void VisitEndComment(String comment, Int32 line, Int32 column) { }

        private void ReadBuffer(Char[] buffer, Int32 length)
        {
            for (var i = 0; i < length; i++)
            {
                currentColumn++;

                if (buffer[i] == BreakLine)
                {
                    currentLine++;
                    currentColumn = 0;
                    continue;
                }

                i = commentary.Interpret(buffer, i, length, currentLine, currentColumn);
                if (commentary.IsCommentary || commentary.IsOpen || commentary.IsClose)
                {
                    if (commentary.IsClose)
                        commentary.Reset();
                    continue;
                }

                if (i > length)
                    break;

                switch (buffer[i])
                {
                    case OpenScope:
                        var ctx = CreateInstanceCssContext(GetCurrentContent());

                        if (context != null)
                            context.SetGraph(ctx);

                        context = ctx;

                        context.Open(currentLine, currentColumn);
                        break;

                    case CloseScope:
                        context.Parse(GetCurrentContent());
                        context.Close(currentLine, currentColumn);
                        context = context.ParentContext;
                        break;

                    default:
                        sb.Append(buffer[i]);
                        break;
                }
            }
        }

        private String GetCurrentContent()
        {
            var result = sb.ToString();
            sb.Clear();
            return result;
        }

        private CssContext CreateInstanceCssContext(String selector)
        {
            CssContext ctx = null;

            selector = selector.TrimStart();

            if (selector.StartsWith("@")) //@media
            {
                ctx = new CssMediaContext(selector)
                {
                    OnOpen = VisitBeginMedia,
                    OnClose = VisitEndMedia
                };
            }
            else
            {
                ctx = new CssSelectorContext(selector)
                {
                    OnOpen = VisitBeginSelector,
                    OnClose = VisitEndSelector,
                    OnVisitProperty = VisitProperty
                };
            }

            return ctx;
        }
    }
}