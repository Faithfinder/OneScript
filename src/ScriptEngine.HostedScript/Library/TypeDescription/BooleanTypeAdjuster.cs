﻿/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;

namespace ScriptEngine.HostedScript.Library
{
	public sealed class BooleanTypeAdjuster : IValueAdjuster
	{
		public IValue Adjust(IValue value)
		{
			if (value?.DataType == DataType.Boolean)
				return value;

			try
			{
				// TODO: вменяемое приведение без Попытки
				return ValueFactory.Create(value.AsBoolean());

			} catch { }

			return ValueFactory.Create(false);
		}
	}
}
