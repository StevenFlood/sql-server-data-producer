﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SQLRepeater.Entities.Generators
{
    public class SmallIntGenerator //: GeneratorBase<IntParameter>
    {
        //public SmallIntGenerator(ValueCreatorDelegate generator, IntParameter param)
        //    : base(generator, param)
        //{

        //}










        //public static System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate> Generators { get; set; }

        //static SmallIntGenerator()
        //{
        //    Generators = new System.Collections.ObjectModel.ObservableCollection<ValueCreatorDelegate>();
        //    Generators.Add(UpCounter);
        //    Generators.Add(DownCounter);
        //    Generators.Add(RandomSmallInt);
        //    Generators.Add(StaticNumber);
        //}

        //public static object UpCounter(int n, object genParameter)
        //{
        //    return (n % short.MaxValue);
        //}

        //public static object DownCounter(int n, object genParameter)
        //{
        //    return (1-(n % short.MaxValue));
        //}

        //public static object RandomSmallInt(int n, object genParameter)
        //{
        //    return (RandomSupplier.Instance.GetNextInt() % short.MaxValue); 
        //}

        //public static object StaticNumber(int n, object param)
        //{
        //    IntParameter p = ObjectToIntParameter(param);
        //    return Wrap(p.MinValue);
        //}

        //private static IntParameter ObjectToIntParameter(object param)
        //{
        //    if (!(param is IntParameter))
        //        throw new ArgumentException("The supplied Parameter is not IntParameter");

        //    return param as IntParameter;
        //}
    }
}
