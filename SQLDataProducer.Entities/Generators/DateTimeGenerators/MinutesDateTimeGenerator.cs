﻿// Copyright 2012-2014 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using SQLDataProducer.Entities.DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataProducer.Entities.Generators.DateTimeGenerators
{
    public class MinutesDateTimeGenerator : DateTimeGeneratorBase
    {
        public static readonly string GENERATOR_NAME = "Minutes Series";

        public MinutesDateTimeGenerator(ColumnDataTypeDefinition datatype)
            : base(GENERATOR_NAME, datatype)
        {
            GeneratorParameters.StartDate = new GeneratorParameter("Start Date", DateTime.Now, GeneratorParameterParser.DateTimeParser);
            GeneratorParameters.ShiftDays = new GeneratorParameter("Shift Days", 0, GeneratorParameterParser.IntegerParser);
            GeneratorParameters.ShiftHours = new GeneratorParameter("Shift Hours", 0, GeneratorParameterParser.IntegerParser);
            GeneratorParameters.ShiftMinutes = new GeneratorParameter("Shift Minutes", 0, GeneratorParameterParser.IntegerParser);
            GeneratorParameters.ShiftSeconds = new GeneratorParameter("Shift Seconds", 0, GeneratorParameterParser.IntegerParser);
            GeneratorParameters.ShiftMilliseconds = new GeneratorParameter("Shift Milliseconds", 0, GeneratorParameterParser.IntegerParser);
        }

        protected override object InternalGenerateValue(long n)
        {
            int d = GeneratorParameters.ShiftDays.Value;
            int h = GeneratorParameters.ShiftHours.Value;
            int min = GeneratorParameters.ShiftMinutes.Value;
            int sec = GeneratorParameters.ShiftSeconds.Value;
            int ms = GeneratorParameters.ShiftMilliseconds.Value;
            int a = n.LongToInt();
            var ts = new TimeSpan(d, h, min + a, sec, ms);

            DateTime start = GeneratorParameters.StartDate.Value;

            return start.Add(ts);
        }
    }
}
