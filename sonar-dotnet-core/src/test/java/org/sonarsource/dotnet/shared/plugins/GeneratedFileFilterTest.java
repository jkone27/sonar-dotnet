/*
 * SonarSource :: .NET :: Core
 * Copyright (C) 2014-2024 SonarSource SA
 * mailto:info AT sonarsource DOT com
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
package org.sonarsource.dotnet.shared.plugins;

import java.nio.file.Paths;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.slf4j.event.Level;
import org.sonar.api.batch.fs.InputFile;
import org.sonar.api.config.PropertyDefinitions;
import org.sonar.api.config.internal.MapSettings;
import org.sonar.api.testfixtures.log.LogTester;
import org.sonar.api.utils.System2;

import static org.assertj.core.api.Assertions.assertThat;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class GeneratedFileFilterTest {

  @Rule
  public LogTester logs = new LogTester();

  private AbstractLanguageConfiguration defaultConfiguration;

  @Before
  public void setUp() {
    logs.setLevel(Level.DEBUG);
    // by default, analyzeGeneratedCode is set to false
    PluginMetadata metadata = mock(PluginMetadata.class);
    when(metadata.languageKey()).thenReturn("cs");
    when(metadata.languageName()).thenReturn("C#");
    when(metadata.fileSuffixesDefaultValue()).thenReturn(".cs");
    AbstractPropertyDefinitions definitions = new AbstractPropertyDefinitions(metadata) {
    };
    MapSettings settings = new MapSettings(new PropertyDefinitions(mock(System2.class), definitions.create()));
    defaultConfiguration = new AbstractLanguageConfiguration(settings.asConfig(), "cs") {
    };
  }

  @Test
  public void accept_returns_false_for_autogenerated_files() {
    // Arrange
    InputFile inputFile = mockInputFile("autogenerated");
    GeneratedFileFilter filter = createFilter(inputFile, defaultConfiguration);

    // Act
    Boolean result = filter.accept(inputFile);

    // Assert
    assertThat(result).isFalse();
    assertThat(logs.logs(Level.DEBUG)).contains("Will ignore generated code");
    assertThat(logs.logs(Level.DEBUG)).contains("Skipping auto generated file: autogenerated");
  }

  @Test
  public void accept_returns_true_for_nonautogenerated_files() {
    // Arrange
    GeneratedFileFilter filter = createFilter(mockInputFile("c:\\autogenerated"), defaultConfiguration);

    // Act
    Boolean result = filter.accept(mockInputFile("File1"));

    // Assert
    assertThat(result).isTrue();
    assertThat(logs.logs(Level.DEBUG)).contains("Will ignore generated code");
  }

  @Test
  public void accept_returns_true_for_autogenerated_files_when_analyzeGeneratedCode_setting_true() {
    // Arrange
    AbstractLanguageConfiguration mockConfiguration = mock(AbstractLanguageConfiguration.class);
    when(mockConfiguration.analyzeGeneratedCode()).thenReturn(true);
    InputFile inputFile = mockInputFile("autogenerated");
    GeneratedFileFilter filter = createFilter(inputFile, mockConfiguration);

    // Act
    Boolean result = filter.accept(inputFile);

    // Assert
    assertThat(result).isTrue();
    assertThat(logs.logs(Level.DEBUG)).contains("Will analyze generated code");
  }

  private InputFile mockInputFile(String path) {
    InputFile file = mock(InputFile.class);
    when(file.uri()).thenReturn(Paths.get(path).toUri());
    when(file.toString()).thenReturn(path);
    return file;
  }

  private GeneratedFileFilter createFilter(InputFile generatedInputFile, AbstractLanguageConfiguration configuration) {
    GlobalProtobufFileProcessor processor = mock(GlobalProtobufFileProcessor.class);
    when(processor.isGenerated(generatedInputFile)).thenReturn(true);
    return new GeneratedFileFilter(processor, configuration);
  }
}
