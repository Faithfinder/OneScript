﻿/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptEngine.Compiler
{
    class ParseIterator
    {
        private int _index;
        private int _startPosition;
        private char _currentSymbol;
        private readonly string _code;
        private int _lineCounter = 1;
        private readonly Dictionary<int, int> _lineBounds;

        public ParseIterator(string code)
        {
            _code = code;
            _startPosition = 0;
            _lineBounds = new Dictionary<int, int>();

            _lineCounter = 1;
            _lineBounds.Add(_lineCounter, 0);

            _index = -1;
            if (!MoveNext())
                _currentSymbol = '\0';
        }

        public char CurrentSymbol
        {
            get
            {
                return _currentSymbol;
            }
        }

        public Word GetContents()
        {
            return GetContents(0, 0);
        }

        public int CurrentLine
        {
            get
            {
                return _lineCounter;
            }
        }

        public string GetCodeLine(int index)
        {
            var idx = GetCodeIndexer();
            return idx.GetCodeLine(index);
        }

        public SourceCodeIndexer GetCodeIndexer()
        {
            return new SourceCodeIndexer(_code, _lineBounds);
        }

        public CodePositionInfo GetPositionInfo(int lineNumber)
        {
            var cp = new CodePositionInfo();
            cp.LineNumber = lineNumber;
            cp.Code = GetCodeLine(lineNumber);
            return cp;
        }

        public Word GetContents(int padLeft, int padRight)
        {
            int len;

            if (_startPosition == _index && _startPosition < _code.Length)
            {
                len = 1;
            }
            else if (_startPosition < _index)
            {
                len = _index - _startPosition;
            }
            else
            {
                return new Word() { start = -1 };
            }

            var contents = _code.Substring(_startPosition + padLeft, len - padRight);
            var word = new Word() { start = _startPosition, content = contents };

            _startPosition = _index + 1;

            return word;
        }

        public bool MoveNext()
        {
            _index++;
            if (_index < _code.Length)
            {
                _currentSymbol = _code[_index];
                if (_currentSymbol == '\n')
                {
                    _lineCounter++;
                    _lineBounds[_lineCounter] = _index + 1;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveBack()
        {
            _index--;
            if (_index >= 0)
            {
                _currentSymbol = _code[_index];
                if (_currentSymbol == '\n')
                {
                    _lineCounter--;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveToContent()
        {
            if (SkipSpaces())
            {
                _startPosition = _index;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SkipSpaces()
        {
            while (Char.IsWhiteSpace(_currentSymbol))
            {
                if (!MoveNext())
                {
                    return false;
                }
            }

            if (_index >= _code.Length)
            {
                return false;
            }

            return true;
        }
    }
}