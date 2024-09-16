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

using SonarAnalyzer.Helpers.Trackers;

namespace SonarAnalyzer.VisualBasic.Core.Trackers;

public class VisualBasicObjectCreationTracker : ObjectCreationTracker<SyntaxKind>
{
    protected override ILanguageFacade<SyntaxKind> Language => VisualBasicFacade.Instance;

    public override Condition ArgumentAtIndexIsConst(int index) =>
        context => ((ObjectCreationExpressionSyntax)context.Node).ArgumentList  is { } argumentList
                   && argumentList.Arguments.Count > index
                   && argumentList.Arguments[index].GetExpression().HasConstantValue(context.SemanticModel);

    public override object ConstArgumentForParameter(ObjectCreationContext context, string parameterName) =>
        ((ObjectCreationExpressionSyntax)context.Node).ArgumentList is { } argumentList
            && argumentList.ArgumentValuesForParameter(context.SemanticModel, parameterName) is { Length: 1 } values
            && values[0] is ExpressionSyntax valueSyntax
                ? valueSyntax.FindConstantValue(context.SemanticModel)
                : null;
}