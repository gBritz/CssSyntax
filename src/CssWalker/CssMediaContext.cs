using System;

namespace CssWalker
{
    public class CssMediaContext : CssContext
    {
        public CssMediaContext(String selector) : base(selector) { }

        public override void Parse(String content) { }
    }
}