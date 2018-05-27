using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TraceViewer
{
    public class FilterBaseModel
    {
        private List<string> _formattedFilter = new List<string>();

        public FilterBaseModel()
        {
            var defaultStringOperators = StringOperators.First();

            var defaultDateOperators = NumericOprators.First();

            var defaultNumericOperators = NumericOprators.First(item => item.Equals("=="));

            TraceKey = TraceKey ?? new FilterElements { Operator = defaultStringOperators };

            //SystemTime = SystemTime ?? new FilterElements { Operator = defaultDateOperators, Operand = System.DateTime.Now.ToString("G") };
            StartDate = StartDate ?? new FilterElements { Operator = defaultDateOperators, Operand = System.DateTime.Now.AddMinutes(-5).ToString("G") };

            EndDate = EndDate ?? new FilterElements { Operator = NumericOprators.First(item => item.Equals("<")), Operand = System.DateTime.Now.AddDays(1).ToString("G") };

            Message = Message ?? new FilterElements { Operator = defaultStringOperators };

            Level = Level ?? new FilterElements { Operator = defaultNumericOperators };

            Writer = Writer ?? new FilterElements { Operator = defaultStringOperators };

            Data = new List<KeyValuePair<string, FilterElements>>();
        }

        public List< KeyValuePair< string, FilterElements>> Data
        {
            get;
            set;

        }

        public FilterElements EndDate
        {
            get;
            set;
        }

        public string FormattedFilter
        {
            get
            {
                _formattedFilter.Clear();

                if (TraceKey != null && !string.IsNullOrEmpty(TraceKey.Operand))
                {
                    _formattedFilter.Add($"TraceKey.ToLower().{TraceKey.Operator}(\"{TraceKey.Operand.ToLower()}\") ");

                }

                if (StartDate != null && string.IsNullOrEmpty(StartDate.Operand))
                {
                    StartDate.Operand = System.DateTime.Now.ToString("G");
                }

                if (EndDate != null && string.IsNullOrEmpty(EndDate.Operand))
                {
                    EndDate.Operand = System.DateTime.Now.ToString("G");
                }

                var convertedStartDate = Convert.ToDateTime(StartDate.Operand);

                var convertedEndDate = Convert.ToDateTime(EndDate.Operand);

                var startDatefilterStr = $"SystemTime {StartDate.Operator} DateTime({convertedStartDate.Year},{convertedStartDate.Month},{convertedStartDate.Day},{convertedStartDate.Hour},{convertedStartDate.Minute},{convertedStartDate.Second})";

                

                var endDatefilterStr = $"SystemTime {EndDate.Operator} DateTime({convertedEndDate.Year},{convertedEndDate.Month},{convertedEndDate.Day},{convertedEndDate.Hour},{convertedEndDate.Minute},{convertedEndDate.Second})";

                _formattedFilter.Add($"{startDatefilterStr}  {FilterElements.DefaultLogic }  { endDatefilterStr}");


                if (Message != null && !string.IsNullOrEmpty(Message.Operand))
                {
                    _formattedFilter.Add($"Message.ToLower().{Message.Operator}(\"{Message.Operand.ToLower()}\") ");

                }

                if (Level != null && !string.IsNullOrEmpty(Level.Operand))
                {
                    _formattedFilter.Add($"Level {Level.Operator}({Level.Operand}) ");

                }

                if ( Data.Count > 0) {

                    _formattedFilter.Add($"it.Data != null ");

                    string dataFilter = " (";

                    for (int i = 0; i < Data.Count; i++)
                    
                    {
                        var item = Data.ElementAt(i);
                        dataFilter += $" it.Data.Any(it.Key.ToLower() == \"{item.Key.ToLower()}\" && it.Value !=null && it.Value.ToLower().Contains( \"{item.Value.Operand.ToLower()}\") ) ";

                        if (Data.Count != i+1)
                        {
                            dataFilter += item.Value.Logic;
                        }
                    }
                    dataFilter += ")";
                    _formattedFilter.Add(dataFilter);

                }

                return string.Join($" {FilterElements.DefaultLogic} ", _formattedFilter);
            }
        }

        public FilterElements Level
        {
            get;
            set;
        }

        public FilterElements Message
        {
            get;
            set;
        }

        public List<string> NumericOprators => new List<string> {
            { ">" },
            {"<" },
            {"==" },
            { ">="} ,
            { "<="}
        };

        public FilterElements StartDate
        {
            get;
            set;
        }

        public List<string> StringOperators => new List<string> {
            { "Contains" },
            {"Equals" },
            { "StartsWith"} ,
            { "EndsWith"}
        };
        public FilterElements TraceKey { get; set; }
                
        public FilterElements Writer
        {
            get;
            set;
        }
    }

    public class FilterElements
    {
        public const string DefaultLogic = " && ";
        public FilterElements()
        {
             Logic = DefaultLogic;
        }

        public string Operand
        {
            get; set;
        }

        public string Operator
        {
            get; set;
        }
        public string Logic
        {
            get; set;
        }

    }
    
}
