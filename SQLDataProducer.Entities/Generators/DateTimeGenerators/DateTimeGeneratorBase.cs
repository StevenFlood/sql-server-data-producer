﻿// Copyright 2012-2013 Peter Henell

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
    public abstract class DateTimeGeneratorBase : AbstractValueGenerator
    {
        public DateTimeGeneratorBase(string generatorName, ColumnDataTypeDefinition dataType, bool isTakingValueFromOtherColumn = false)
            : base(generatorName, isTakingValueFromOtherColumn)
        {

        }

        protected override object ApplyGeneratorTypeSpecificLimits(object value)
        {
            if (value is DBNull)
            {
                return value;
            }
            return value;   
        }
    }
}
