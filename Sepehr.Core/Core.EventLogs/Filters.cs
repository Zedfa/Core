using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventLogs
{
    class Filters
    {
        public string ConvertToWhereClause(CriteriaOperator criteria)
        {
          
            var whereClause = CreateWhereClauseComplexOperator(criteria);

            return whereClause;
        }
        private string CreateWhereClauseComplexOperator(CriteriaOperator criteria)
        {
            var whereClause = string.Empty;
                      
            if (criteria.GetType() == typeof(GroupOperator))
            {
                var operands = ((GroupOperator)criteria).Operands;

                var operatorType = ((GroupOperator)criteria).OperatorType;

                whereClause += CreateWhereClauseGroupOperator((GroupOperator)criteria, operands, operatorType);
            }

            else
            {
                whereClause += CreateWhereClauseCriteriaOperator(criteria);

            }
            return whereClause;
        }
        private string CreateWhereClauseGroupOperator(GroupOperator filter, CriteriaOperatorCollection operands, GroupOperatorType operatorType)
        {
            var whereClause = string.Empty;

            // var operands = ((GroupOperator)eventLogGrid.FilterCriteria).Operands;

            // var operatorType = ((GroupOperator)eventLogGrid.FilterCriteria).OperatorType.ToString();


            for (int i = 0; i < operands.Count; i++)
            {
                if (operands[i].GetType() == typeof(GroupOperator))
                {
                    whereClause += CreateWhereClauseGroupOperator((GroupOperator)operands[i], operands, operatorType);
                }
                else
                {
                    whereClause += CreateWhereClauseCriteriaOperator(operands[i]);
                    if (i + 1 != operands.Count)
                        whereClause += $" {operatorType} ";
                }
            }

            return whereClause;
        }

        private string CreateWhereClauseCriteriaOperator(CriteriaOperator oprator)
        {

            var whereClause = string.Empty;

            if (oprator.GetType() == typeof(FunctionOperator))
            {
                whereClause = CreateWhereClauseFunctionOperator((FunctionOperator)oprator);

            }
            else
            {
                whereClause = CreateWhereClauseBinaryOperator((BinaryOperator)oprator);

            }
            return whereClause;
        }

        private string CreateWhereClauseFunctionOperator(FunctionOperator oprator)
        {
            var operands = oprator.Operands;

            var operatorType = $"ToLower().{oprator.OperatorType}" ;

            var propertyName = ((OperandProperty)operands[0]).PropertyName;

            var value = ((OperandValue)operands[1]).Value;

            return $"{propertyName}.{operatorType}(\"{value}\") ";

        }

        private string CreateWhereClauseBinaryOperator(BinaryOperator oprator)
        {
            string whereClause = string.Empty;

            var property = oprator.LeftOperand;

            var propertyName = ((OperandProperty)property).PropertyName;

            var operatorType = oprator.OperatorType.ToString().ToLower();

            if (operatorType == "equal") operatorType = "==";

            else if (operatorType == "greater") operatorType = ">";

            else if (operatorType == "less") operatorType = "<";

            else if (operatorType == "greaterorequal") operatorType = ">=";

            else if (operatorType == "lessorequal") operatorType = "<=";

            else throw new NotImplementedException();

            var value = ((OperandValue)(oprator).RightOperand).Value;

            if (value.GetType().Equals(typeof(DateTime)))
            {
                var convertedValue = (DateTime)value;
                whereClause = $"{propertyName} {operatorType} DateTime({convertedValue.Year},{convertedValue.Month},{convertedValue.Day},{convertedValue.Hour},{convertedValue.Minute},{convertedValue.Second}) ";

            }
            else if (value.GetType().Equals(typeof(string)))
            {

                whereClause = $"{propertyName} {operatorType} \"{value}\" ";
            }
            else if (value.GetType().Equals(typeof(int)))
            {
                whereClause = $"{propertyName} {operatorType} {value} ";

            }

            return whereClause;
        }





    }
}
