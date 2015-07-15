using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CssWalker.Test
{
    [TestClass]
    public class CssWalkerTest
    {
        [TestMethod]
        public void GivenOneEmptyCssClassButton_ClassNameShouldBeButton()
        {
            var reader = new StringReader(@" .button {}");

            var walker = new AdapterCssWalker
            {
                OnBeginSelector = (selector, line, column) => selector.Should().Be(".button ")
            };

            walker.Visit(reader);
        }

        [TestMethod]
        public void GivenOneCssClassButtonWithTwoProperties_ClassNameShouldBeButton()
        {
            var reader = new StringReader(@"
                .button {
                    margin-top: 10px;
                    background-image: url(../img/bk.jpg);
                }
            ");

            var walker = new AdapterCssWalker
            {
                OnBeginSelector = (selector, line, column) => selector.Should().Be(".button ")
            };

            walker.Visit(reader);
        }
    }
}