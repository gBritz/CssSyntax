# CssSyntax (v0.1-alpha.0)

Css interpreter with visitor pattern

[![Build status](https://ci.appveyor.com/api/projects/status/jfy41tn7nf6uvcnf?svg=true)](https://ci.appveyor.com/project/Britz/csssyntax)
[![Coverage Status](https://coveralls.io/repos/gBritz/CssSyntax/badge.svg?branch=develop&service=github)](https://coveralls.io/github/gBritz/CssSyntax?branch=develop)


### Install

Download by package console

    install-package CssSyntax


### How to using this?

Given you want know how many selectors are in your css, implement your own walker class and overriding methods you want to work:

	public class SelectorCounterWalker : CssWalker
	{
		public Int32 CountSelectors { get; private set; }

		protected virtual void VisitBeginSelector(string selector, int line, int column)
		{
			CountSelectors++;
		}
	}

Execute walker:

    var css = @"
	    #edit-task { padding-top : 10px; }
	    .btn {
	        margin-top: 10px;
	        background-image: url(../img/bk.jpg);
	    }
	";

	var reader = new StringReader(css);
	var walker = new SelectorCounterWalker();
	walker.Visit(reader);

	Console.WriteLine(walker.CountSelectors); //print: 2


### I want to contribute!

Great! Feel free to pick one of the issues, or submit a bug/feature you would want to work on.

Please be tidy in your commits. Also, try to touch as small parts of the code as possible. This makes it easier to review and manage pull requests. Make sure your code is covered by tests, and write new ones if needed.

If you plan to do big changes or refactoring, please notify me first, so that we can discuss this in advance.