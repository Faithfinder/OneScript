﻿
Процедура ПриЗагрузкеБиблиотеки(Знач Путь, СтандартнаяОбработка, Отказ)
	
	СтандартнаяОбработка = Ложь;
	ПодключитьВнешнююКомпоненту(ОбъединитьПути(Путь, "Component.dll"));
	
КонецПроцедуры
