using System;
using System.Diagnostics;

namespace CssSyntax.SyntaxTree
{
    [DebuggerDisplay("line={line}, colum={column}")]
    public struct Position
    {
        private readonly Int32 line;
        private readonly Int32 column;

        public Position(Int32 line, Int32 column)
        {
            this.line = line;
            this.column = column;
        }

        public Int32 Line
        {
            get { return this.line; }
        }

        public Int32 Column
        {
            get { return this.column; }
        }
    }
}