using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAndDeath.Models
{

    public class LifeRootobject
    {
        public string copyright { get; set; }
        public Dataset[] dataset { get; set; }
        public Attribute[] attribute { get; set; }
        public Dimension[] dimension { get; set; }
        public Fact[] fact { get; set; }
    }

    public class Dataset
    {
        public string label { get; set; }
        public string display { get; set; }
    }

    public class Attribute
    {
        public string label { get; set; }
        public string display { get; set; }
    }

    public class Dimension
    {
        public string label { get; set; }
        public string display { get; set; }
        public bool isMeasure { get; set; }
        public Code[] code { get; set; }
    }

    public class Code
    {
        public string label { get; set; }
        public string display { get; set; }
        public int display_sequence { get; set; }
        public string url { get; set; }
        public Attr[] attr { get; set; }
    }

    public class Attr
    {
        public string category { get; set; }
        public string value { get; set; }
    }

    public class Fact
    {
        public int fact_id { get; set; }
        public string dataset { get; set; }
        public string effective_date { get; set; }
        public string end_date { get; set; }
        public bool published { get; set; }
        public Dim[] Dim { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public string display { get; set; }
        public float numeric { get; set; }
        public object low { get; set; }
        public object high { get; set; }
        public object stderr { get; set; }
        public object stddev { get; set; }
    }

    public class Dim
    {
        public string category { get; set; }
        public string code { get; set; }
    }

}
