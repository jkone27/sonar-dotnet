﻿/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2024 SonarSource SA
 * mailto: contact AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

namespace SonarAnalyzer.CSharp.Core.Syntax.Extensions;

public static class MemberAccessExpressionSyntaxExtensions
{
    public static bool IsMemberAccessOnKnownType(this MemberAccessExpressionSyntax memberAccess, string name, KnownType knownType, SemanticModel semanticModel) =>
        memberAccess.NameIs(name)
        && semanticModel.GetSymbolInfo(memberAccess).Symbol is {} symbol
        && symbol.ContainingType.DerivesFrom(knownType);

    public static bool IsPropertyInvocation(this MemberAccessExpressionSyntax expression, ImmutableArray<KnownType> types, string propertyName, SemanticModel semanticModel) =>
        expression.NameIs(propertyName) &&
        semanticModel.GetSymbolInfo(expression).Symbol is IPropertySymbol propertySymbol &&
        propertySymbol.IsInType(types);
}