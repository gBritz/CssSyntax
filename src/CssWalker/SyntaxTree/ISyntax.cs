using System;

namespace CssSyntax.SyntaxTree
{
    public interface ISyntax
    {
        String Content { get; }

        Position StartAt { get; }

        Position EndAt { get; }
    }
}