namespace YoutrackBoard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    class MethodParameterExtractor
    {
        public static object[] GetObjects(Expression expression)
        {
            var l = expression as LambdaExpression;
            var e = l.Body as MethodCallExpression;
            
            if (e == null) throw new InvalidOperationException("expression is not method expression");

            var parameters = new List<object>();

            foreach (var p in e.Arguments)
            {
                Func<object> lambda;
                if (p.Type.IsValueType)
                {
                    lambda = Expression.Lambda<Func<object>>(Expression.Convert(p,typeof(object))).Compile();
                }
                else
                {
                    lambda = Expression.Lambda<Func<object>>(p).Compile();
                    
                }
                
                parameters.Add(lambda());
                
            }
            return parameters.ToArray();
        }

        public static string ToKey(object[] keyParameters)
        {
            return string.Join(",", keyParameters.Select(Convert.ToString));
        }
    }
}