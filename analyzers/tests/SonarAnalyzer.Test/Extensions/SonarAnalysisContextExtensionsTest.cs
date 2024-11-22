﻿/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2014-2024 SonarSource SA
 * mailto:info AT sonarsource DOT com
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the Sonar Source-Available License Version 1, as published by SonarSource SA.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the Sonar Source-Available License for more details.
 *
 * You should have received a copy of the Sonar Source-Available License
 * along with this program; if not, see https://sonarsource.com/license/ssal/
 */

using NSubstitute;
using SonarAnalyzer.AnalysisContext;
using ExtensionsCS = SonarAnalyzer.CSharp.Core.Extensions.SonarAnalysisContextExtensions;
using ExtensionsVB = SonarAnalyzer.VisualBasic.Core.Extensions.SonarAnalysisContextExtensions;

namespace SonarAnalyzer.Test.Extensions;

[TestClass]
public class SonarAnalysisContextExtensions
{
    private static readonly DiagnosticDescriptor DummyMainDescriptor = AnalysisScaffolding.CreateDescriptorMain();

    [DataTestMethod]
    [DataRow("// <auto-generated/>", false)]
    [DataRow("// any random comment", true)]
    public void ReportIssue_SonarSymbolAnalysisContext_CS(string comment, bool expected)
    {
        var (tree, model) = TestHelper.CompileCS($$"""
                {{comment}}
                public class Sample {}
                """);
        var wasReported = false;
        var symbolContext = new SymbolAnalysisContext(Substitute.For<ISymbol>(), model.Compilation, AnalysisScaffolding.CreateOptions(), _ => wasReported = true, _ => true, default);
        var context = new SonarSymbolReportingContext(AnalysisScaffolding.CreateSonarAnalysisContext(), symbolContext);
        ExtensionsCS.ReportIssue(context, DummyMainDescriptor, tree.GetRoot());

        wasReported.Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow("' <auto-generated/>", false)]
    [DataRow("' any random comment", true)]
    public void ReportIssue_SonarSymbolAnalysisContext_VB(string comment, bool expected)
    {
        var (tree, model) = TestHelper.CompileVB($"""
                {comment}
                Public Class Sample
                End Class
                """);
        var wasReported = false;
        var symbolContext = new SymbolAnalysisContext(Substitute.For<ISymbol>(), model.Compilation, AnalysisScaffolding.CreateOptions(), _ => wasReported = true, _ => true, default);
        var context = new SonarSymbolReportingContext(AnalysisScaffolding.CreateSonarAnalysisContext(), symbolContext);

        ExtensionsVB.ReportIssue(context, DummyMainDescriptor, tree.GetRoot());
        wasReported.Should().Be(expected);

        wasReported = false;
        ExtensionsVB.ReportIssue(context, DummyMainDescriptor, tree.GetRoot().GetFirstToken());
        wasReported.Should().Be(expected);

        wasReported = false;
        ExtensionsVB.ReportIssue(context, DummyMainDescriptor, tree.GetRoot().GetLocation());
        wasReported.Should().Be(expected);
    }
}
