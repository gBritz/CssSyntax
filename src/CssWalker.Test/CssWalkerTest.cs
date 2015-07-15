using CssWalker.SyntaxTree;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CssWalker.Test
{
    [TestClass]
    public class CssWalkerTest
    {
        [TestMethod]
        public void GivenOneEmptyCssClassButton_ClassNameShouldBeButton()
        {
            var selectors = @" .btn {}".ToSelectors();

            selectors.Should().HaveCount(1);
            selectors[0].Selector.Should().Be(".btn ");
        }

        [TestMethod]
        public void GivenOneCssClassButtonWithTwoProperties_ClassNameShouldBeButton()
        {
            var selectors = @"
                .button {
                    margin-top: 10px;
                    background-image: url(../img/bk.jpg);
                }
            ".ToSelectors();

            selectors.Should().HaveCount(1);
            selectors[0].Selector.Should().Be(".button ");
        }

        [TestMethod]
        public void ClassNameButtonShouldBeLineOneAndColumn10()
        {
            var selectors = @"
                .button {
                    margin-top: 10px;
                    background-image: url(../img/bk.jpg);
                }
            ".ToSelectors();

            selectors.Should().HaveCount(1);
            selectors[0].StartAt.Should().Be(new Position(2, 26));
            selectors[0].EndAt.Should().Be(new Position(5, 18));
        }

        [TestMethod]
        public void GivenCommentaryOfOneLineBeforeClass_CommentsShouldBeOne()
        {
            var comments = @"
                /* Is a class of god */
                .button {
                    margin-top: 10px;
                    background-image: url(../img/bk.jpg);
                }
            ".ToComments();

            comments.Should().HaveCount(1);
            comments[0].StartAt.Should().Be(new Position(2, 17));
            comments[0].EndAt.Should().Be(new Position(2, 40));
        }
    }
}