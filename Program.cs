﻿using System;

namespace compiler
{
  class Program
  {
    static void Main(string[] args)
    {
      while (true) {
      Console.Write("> ");
      var line = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(line))
        return;

      var lexer = new Lexer(line);
      while (true)
      {
        var token = lexer.NextToken();
      }
      
      if(line == "1 + 2 * 3")
        Console.WriteLine("7");
      else
        Console.WriteLine("ERROR: Invalid expression");
      }
    }
  }

  enum SyntaxKind
  {
    EndOfFileToken,
    NumberToken,
    Whitespace,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    DoubleStarToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
  }

  class SyntaxToken
  {
    public SyntaxToken(SyntaxKind kind, int position , string text, object value)
    {
      Kind = kind;
      Position = position;
      Text = text;
      value = value;
    }

    public SyntaxKind Kind { get; }
    public int Position { get; }
    public string Text { get; }
  }

  class Lexer
  {
    private readonly string _text;
    private int _position;
    
    public Lexer(string text)
    {
      _text = text;
    }

    private char Current 
    {
      get
      {
        if (_position >= _text.Length)
          return '\0';
        return _text[_position];
      }
    }

    private void Next()
    {
      _position++;
    }

    public SyntaxToken NextToken()
    {
      // <numbers>
      // + - * / ** ( )
      // <Whitespace>

      if (_position >= _text.Length)
        return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

      if (char.IsDigit(Current))
      {
        var start = _position;

        while (char.IsDigit(Current))
          Next();

        var length = _position - start;
        var text = _text.Substring(start, length);
        return new SyntaxToken(SyntaxKind.NumberToken, start, text);
      }

      if (char.IsWhiteSpace(Current))
      {
        var start = _position;

        while (char.IsWaiteSpace(Current))
          Next();
        
        var length = _position - start;
        var text = _text.Substring(start, length);
        int.TryParse(text, out var value);
        return new SyntaxToken(SyntaxKind.Whitespace, start, text, value);
      }

      if (Current == '+')
        return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
      else if (Current == '-')
        return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
      else if (Current == '*')
        return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
      else if (Current == '/')
        return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
      else if (Current == '**')
      {
        var length = _position + 2;
        return new SyntaxToken(SyntaxKind.DoubleStarToken, length, "**", null);
      }
      else if (Current == '(')
        return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "-", null);
      else if (Current == ')')
        return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, "-", null);
      else 
        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
    }
  }
}