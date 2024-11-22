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

namespace SonarAnalyzer.Rules.VisualBasic;

[DiagnosticAnalyzer(LanguageNames.VisualBasic)]
public sealed class DebuggerDisplayUsesExistingMembers : DebuggerDisplayUsesExistingMembersBase<SimpleArgumentSyntax, SyntaxKind>
{
    protected override ILanguageFacade<SyntaxKind> Language => VisualBasicFacade.Instance;

    protected override SyntaxNode AttributeTarget(SimpleArgumentSyntax attribute) =>
        attribute.GetAncestor<AttributeListSyntax>()?.Parent;

    protected override ImmutableArray<SyntaxNode> ResolvableIdentifiers(SyntaxNode expression) =>
        expression is IdentifierNameSyntax identifierName
            ? ImmutableArray.Create<SyntaxNode>(identifierName)
            : ImmutableArray<SyntaxNode>.Empty;
}
