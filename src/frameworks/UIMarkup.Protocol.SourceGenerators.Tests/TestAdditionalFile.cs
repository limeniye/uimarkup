﻿using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;

namespace UIMarkup.Protocol.SourceGenerators.Tests;

public class TestAdditionalFile : AdditionalText
{
	private readonly string _text;
	public override string Path { get; }

	public TestAdditionalFile(string path, string text)
	{
		Path = path;
		_text = text;
	}

	public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken())
	{
		return SourceText.From(_text);
	}
}
