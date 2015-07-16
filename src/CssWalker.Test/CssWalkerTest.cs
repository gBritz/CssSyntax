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
            selectors[0].Content.Should().Be(".btn ");
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
            selectors[0].Content.Should().Be(".button ");
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

        [TestMethod]
        public void CommentaryShouldHavePositionOneLineAndColumn20()
        {
            var comments = @"/* Testing 123... */".ToComments();

            comments.Should().HaveCount(1);
            comments[0].StartAt.Should().Be(new Position(1, 1));
            comments[0].EndAt.Should().Be(new Position(1, 21));
        }

        [TestMethod]
        public void CommentaryShouldHavePositionMultiLineAndColumn20()
        {
            var comments = @"/* Testing 123... 

*/".ToComments();

            comments.Should().HaveCount(1);
            comments[0].StartAt.Should().Be(new Position(1, 1));
            comments[0].EndAt.Should().Be(new Position(3, 3));
        }

        [TestMethod]
        public void ClassNameShouldBe8Comments()
        {
            var selectors = @"
.el-custom-scrollbar.ps-container .ps-scrollbar-y-rail .ps-scrollbar-y {
    -webkit-box-shadow: 0 0 2px #c09251;
    -moz-box-shadow: 0 0 2px #c09251;
    box-shadow: 0 0 2px #c09251;
    border: 1px solid rgba(192, 146, 81, 0.7);
    background: #ffe100;
    /* Old browsers */
    
    background: -moz-linear-gradient(top, #ffe100 0%, #fdc700 100%);
    /* FF3.6+ */
    
    background: -webkit-gradient(linear, left top, left bottom, color-stop(0%, #ffe100), color-stop(100%, #fdc700));
    /* Chrome,Safari4+ */
    
    background: -webkit-linear-gradient(top, #ffe100 0%, #fdc700 100%);
    /* Chrome10+,Safari5.1+ */
    
    background: -o-linear-gradient(top, #ffe100 0%, #fdc700 100%);
    /* Opera 11.10+ */
    
    background: -ms-linear-gradient(top, #ffe100 0%, #fdc700 100%);
    /* IE10+ */
    
    background: linear-gradient(to bottom, #ffe100 0%, #fdc700 100%);
    /* W3C */
    
    filter: progid: DXImageTransform.Microsoft.gradient(startColorstr='#ffe100', endColorstr='#fdc700', GradientType=0);
    /* IE6-9 */
}".ToSelectors();

            selectors.Should().HaveCount(1);
            
            var commnets = selectors[0].Comments;
            commnets.Should().HaveCount(8);
            commnets[0].Content.Should().Be(" Old browsers ");
            commnets[1].Content.Should().Be(" FF3.6+ ");
            commnets[2].Content.Should().Be(" Chrome,Safari4+ ");
            commnets[3].Content.Should().Be(" Chrome10+,Safari5.1+ ");
            commnets[4].Content.Should().Be(" Opera 11.10+ ");
            commnets[5].Content.Should().Be(" IE10+ ");
            commnets[6].Content.Should().Be(" W3C ");
            commnets[7].Content.Should().Be(" IE6-9 ");
        }
    }
}