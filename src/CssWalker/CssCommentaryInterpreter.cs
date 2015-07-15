using System;
using System.Text;

namespace CssWalker
{
    public class CssCommentaryInterpreter
    {
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

            if (buffer[index] == '/' && buffer[index + 1] == '*')
            {
                IsOpen = true;
                IsCommentary = true;
                index = result = index + 2;

                onOpen(currentLine, currentColumn);
            }
            else if (buffer[index] == '*' && buffer[index + 1] == '/')
            {
                IsClose = true;
                IsCommentary = false;
                result = index + 2;

                onClose(content.ToString(), currentLine, currentColumn);
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