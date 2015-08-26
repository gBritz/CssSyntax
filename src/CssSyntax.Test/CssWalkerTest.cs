using CssSyntax.SyntaxTree;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace CssSyntax.Test
{
    [TestClass]
    public class CssWalkerTest
    {
        [TestMethod]
        public void SelectShouldHaveOneElement()
        {
            var selectors = @" .btn {}".ToSelectors();

            selectors.Should().HaveCount(1);
            selectors[0].Content.Should().Be(".btn ");
        }

        [TestMethod]
        public void ClassNameShouldBeTwoProperties()
        {
            var selectors = @"
                .button {
                    margin-top: 10px;
                    background-image: url(../img/bk.jpg);
                }
            ".ToSelectors();

            selectors.Should().HaveCount(1);

            var selector = selectors[0];
            selector.StartAt.Should().Be(new Position(2, 26));
            selector.EndAt.Should().Be(new Position(5, 18));

            selector.Properties.Should().HaveCount(2);
            selector.Properties[0].Content.Should().Be("margin-top:10px");
            selector.Properties[1].Content.Should().Be("background-image:url(../img/bk.jpg)");
        }

        [TestMethod]
        public void ClassNameShouldBeTwoSelectors()
        {
            var selectors = @"
                #edit-task { padding-top : 10px; }
                .btn {
                    margin-top: 10px;
                    background-image: url(../img/bk.jpg);
                }
            ".ToSelectors();

            selectors.Should().HaveCount(2);
            selectors[0].Content.Should().Be("#edit-task ");
            selectors[0].Properties.Should().HaveCount(1);
            selectors[0].Properties[0].Content.Should().Be("padding-top:10px");

            selectors[1].Content.Should().Be(".btn ");
            selectors[1].Properties.Should().HaveCount(2);
        }

        [TestMethod]
        public void CommentaryShouldHaveOneAndAtPosition()
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

        [TestMethod]
        public void TextShouldBeEqualsCss()
        {
            var css =
@"img {
  float: right;
}

.clearfix {
  overflow: auto;
  zoom: 1;
}";

            var cssText = new StringBuilder();
            var walker = new CssWalkerMock
            {
                OnVisitText = text => cssText.Append(text)
            };

            walker.Visit(new StringReader(css));

            cssText.ToString().Should()
                .NotBeNullOrEmpty().And
                .Be(css);
        }

        [TestMethod]
        public void TextWithCommentsShouldBeEqualsCss()
        {
            var css =
@"img {
  float: right;
}

/*Css clear fix*/
.clearfix {
  overflow: auto;
  zoom: 1;/*IE6 support*/
}";

            var cssText = new StringBuilder();
            var walker = new CssWalkerMock
            {
                OnVisitText = text => cssText.Append(text)
            };

            walker.Visit(new StringReader(css));

            cssText.ToString().Should()
                .NotBeNullOrEmpty().And
                .Be(css);
        }

        [TestMethod]
        public void SelectShouldNotBeBreakLines()
        {
            var css = @" .btn {}";

            var countBreakLine = 0;
            var walker = new CssWalkerMock
            {
                OnVisitBreakLine = (line, colum) => countBreakLine++
            };

            walker.Visit(new StringReader(css));

            countBreakLine.Should().Be(0);
        }

        [TestMethod]
        public void SelectShouldBeContainsTwoBreakLines()
        {
            var css = @"img {
  float: right;
}";

            var countBreakLine = 0;
            var walker = new CssWalkerMock
            {
                OnVisitBreakLine = (line, colum) => countBreakLine++
            };

            walker.Visit(new StringReader(css));

            countBreakLine.Should().Be(2);
        }

        [TestMethod]
        public void SelectShouldBeContainsTwoConsecutivesBreakLines()
        {
            var css = @"

img { float: right; }";

            var countBreakLine = 0;
            var walker = new CssWalkerMock
            {
                OnVisitBreakLine = (line, colum) => countBreakLine++
            };

            walker.Visit(new StringReader(css));

            countBreakLine.Should().Be(2);
        }
    }
}