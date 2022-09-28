using System.Collections.Generic;

using App.Models;

namespace App.PumpFactsService.Models
{
    /// <summary>
    /// Вычисление выражений
    /// </summary>
    public class Evaluator
    {
        public delegate ParameterValue GetExpressionValueDelegate(string expression);
        private GetExpressionValueDelegate getExpressionValueDelegate;

        private delegate void EvalFuncDelegate(ParameterValue parameterValue);

        private Dictionary<string, EvalFuncDelegate> methods;

        public Evaluator(GetExpressionValueDelegate _getExpressionValueDelegate)
        {
            getExpressionValueDelegate = _getExpressionValueDelegate; 

            methods = new Dictionary<string, EvalFuncDelegate>();
            registerMethod("D216_minus_D300", method_D216_minus_D300);
            registerMethod("IsPump1TurnedOff", method_IsPump1TurnedOff);
            registerMethod("IsPump2TurnedOff", method_IsPump2TurnedOff);

            registerMethod("IsPump1Running", method_IsPump1Running);
            registerMethod("IsPump1Stopped", method_IsPump1Stopped);
            registerMethod("IsPump2Running", method_IsPump2Running);
            registerMethod("IsPump2Stopped", method_IsPump2Stopped);
        }

        private void registerMethod(string expression, EvalFuncDelegate evalFunc)
        {
            methods.Add(expression.ToUpper(), evalFunc);
        }

        public void evaluate(ParameterValue parameterValue, string expression)
        {
            if (methods.TryGetValue(expression.ToUpper(), out EvalFuncDelegate evalFuncDelegate))
                evalFuncDelegate(parameterValue);
            else
                throw new System.Exception($"Не найден обработчик выражения [{expression}]");
        }

        private void method_D216_minus_D300(ParameterValue parameterValue)
        {
            int a = getExpressionValueDelegate("D216").intValue;
            int b = getExpressionValueDelegate("params.level_above_pump").intValue;
            parameterValue.intValue = (short)(a - b);
            parameterValue.isBoolean = false;
        }

        private void method_IsPump1TurnedOff(ParameterValue parameterValue)
        {
            bool manual = getExpressionValueDelegate("adui.pump1.mode.indication.manual").boolValue;
            bool auto = getExpressionValueDelegate("adui.pump1.mode.indication.auto").boolValue;
            parameterValue.isBoolean = true;
            parameterValue.boolValue = !manual && !auto;
        }

        private void method_IsPump2TurnedOff(ParameterValue parameterValue)
        {
            bool manual = getExpressionValueDelegate("adui.pump2.mode.indication.manual").boolValue;
            bool auto = getExpressionValueDelegate("adui.pump2.mode.indication.auto").boolValue;
            parameterValue.boolValue = !manual && !auto;
            parameterValue.isBoolean = true;
        }

        private void method_IsPump1Running(ParameterValue parameterValue)
        {
            bool pchDriven = getExpressionValueDelegate("adui.pump1.signalling.pch.driven").boolValue;
            bool softStarterDriven = getExpressionValueDelegate("adui.pump1.signalling.soft.starter.driven").boolValue;
            parameterValue.boolValue = pchDriven || softStarterDriven;
            parameterValue.isBoolean = true;
        }

        private void method_IsPump1Stopped(ParameterValue parameterValue)
        {
            bool pchDriven = getExpressionValueDelegate("adui.pump1.signalling.pch.driven").boolValue;
            bool softStarterDriven = getExpressionValueDelegate("adui.pump1.signalling.soft.starter.driven").boolValue;
            parameterValue.boolValue = !(pchDriven || softStarterDriven);
            parameterValue.isBoolean = true;
        }

        private void method_IsPump2Running(ParameterValue parameterValue)
        {
            bool pchDriven = getExpressionValueDelegate("adui.pump2.signalling.pch.driven").boolValue;
            bool softStarterDriven = getExpressionValueDelegate("adui.pump2.signalling.soft.starter.driven").boolValue;
            parameterValue.boolValue = pchDriven || softStarterDriven;
            parameterValue.isBoolean = true;
        }

        private void method_IsPump2Stopped(ParameterValue parameterValue)
        {
            bool pchDriven = getExpressionValueDelegate("adui.pump2.signalling.pch.driven").boolValue;
            bool softStarterDriven = getExpressionValueDelegate("adui.pump2.signalling.soft.starter.driven").boolValue;
            parameterValue.boolValue = !(pchDriven || softStarterDriven);
            parameterValue.isBoolean = true;
        }
    }
}
