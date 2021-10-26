﻿/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2021 SonarSource SA
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarAnalyzer.Rules.CSharp;
using SonarAnalyzer.UnitTest.TestFramework;

namespace SonarAnalyzer.UnitTest.Rules
{
    [TestClass]
    public class MemberShadowsOuterStaticMemberTest
    {
        [TestMethod]
        public void MemberShadowsOuterStaticMember() =>
            Verifier.VerifyAnalyzer(@"TestCases\MemberShadowsOuterStaticMember.cs", new MemberShadowsOuterStaticMember());

#if NET
        [TestMethod]
        public void MemberShadowsOuterStaticMember_CSharp9() =>
            Verifier.VerifyAnalyzerFromCSharp9Library(@"TestCases\MemberShadowsOuterStaticMember.CSharp9.cs", new MemberShadowsOuterStaticMember());

        [TestMethod]
        public void MemberShadowsOuterStaticMember_CSharp10() =>
            Verifier.VerifyAnalyzerFromCSharp10Library(@"TestCases\MemberShadowsOuterStaticMember.CSharp10.cs", new MemberShadowsOuterStaticMember());
#endif
    }
}
