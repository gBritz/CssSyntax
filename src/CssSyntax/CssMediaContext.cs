using System;

namespace CssSyntax
{
    public class CssMediaContext : CssContext
    {
        public CssMediaContext(String selector) : base(selector) { }

        public override void Parse(String content) { }
    }
}