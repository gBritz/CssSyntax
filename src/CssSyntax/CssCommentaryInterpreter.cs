using System;
using System.Text;

namespace CssSyntax
{
    public class CssCommentaryInterpreter
    {
        private static readonly Char CommentPart1 = '/';
        private static readonly Char CommentPart2 = '*';

        private readonly StringBuilder content = new StringBuilder();
        private readonly Action<Int32, Int32> onOpen;
        private readonly Action<String, Int32, Int32> onClose;

        public CssCommentaryInterpreter(Action<Int32, Int32> onOpen, Action<String, Int32, Int32> onClose)
        {
            onOpen.ThrowIfNull("onOpen");
            onClose.ThrowIfNull("onClose");

            this.onOpen = onOpen;
            this.onClose = onClose;
        }

        public Boolean IsOpen { get; private set; }

        public Boolean IsClose { get; private set; }

        public Boolean IsCommentary { get; private set; }

        public Int32 Interpret(Char[] buffer, Int32 index, Int32 length, Int32 currentLine, Int32 currentColumn)
        {
            var result = index;
            IsOpen = false;
            IsClose = false;

            if (buffer[index] == CommentPart1 && buffer[index + 1] == CommentPart2)
            {
                IsOpen = true;
                IsCommentary = true;
                index = result = index + 2;

                onOpen(currentLine, currentColumn - 1);
            }
            else if (buffer[index] == CommentPart2 && buffer[index + 1] == CommentPart1)
            {
                IsClose = true;
                IsCommentary = false;
                result = index + 2;

                onClose(content.ToString(), currentLine, currentColumn + 1);
                content.Clear();
            }

            if (IsCommentary)
            {
                content.Append(buffer[index]);
            }

            return result;
        }

        public void Reset()
        {
            IsCommentary = false;
            IsClose = false;
            IsOpen = false;
        }
    }
}