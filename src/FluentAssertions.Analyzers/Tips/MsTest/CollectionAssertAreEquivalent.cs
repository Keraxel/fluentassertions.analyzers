using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;

namespace FluentAssertions.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CollectionAssertAreEquivalentAnalyzer : MsTestCollectionAssertAnalyzer
    {
        public const string DiagnosticId = Constants.Tips.MsTest.CollectionAssertAreEquivalent;
        public const string Category = Constants.Tips.Category;

        public const string Message = "Use .Should().BeEquivalentTo() instead.";

        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Info, true);
        protected override IEnumerable<FluentAssertionsCSharpSyntaxVisitor> Visitors
        {
            get
            {
                yield return new CollectionAssertAreEquivalentSyntaxVisitor();
            }
        }

		public class CollectionAssertAreEquivalentSyntaxVisitor : FluentAssertionsCSharpSyntaxVisitor
		{
			public CollectionAssertAreEquivalentSyntaxVisitor() : base(new MemberValidator("AreEquivalent"))
			{
			}
		}
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionAssertAreEquivalentCodeFix)), Shared]
    public class CollectionAssertAreEquivalentCodeFix : MsTestCollectionAssertCodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(CollectionAssertAreEquivalentAnalyzer.DiagnosticId);

        protected override ExpressionSyntax GetNewExpression(ExpressionSyntax expression, FluentAssertionsDiagnosticProperties properties)
        {
            return RenameMethodAndReorderActualExpectedAndReplaceWithSubjectShould(expression, "AreEquivalent", "BeEquivalentTo");
        }
    }
}