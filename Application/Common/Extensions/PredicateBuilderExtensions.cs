using System.Linq.Expressions;

namespace Application.Common.Extensions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() => param => true;

        public static Expression<Func<T, bool>> False<T>() => param => false;

        public static Expression<Func<T, bool>> Combine<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second,
            Func<Expression, Expression, Expression> merge)
        {
            var parameter = Expression.Parameter(typeof(T));

            var firstBody = new ReplaceParameterVisitor(first.Parameters[0], parameter).Visit(first.Body);
            var secondBody = new ReplaceParameterVisitor(second.Parameters[0], parameter).Visit(second.Body);

            return Expression.Lambda<Func<T, bool>>(merge(firstBody, secondBody), parameter);
        }

        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Combine(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Combine(second, Expression.OrElse);
        }
    }

    internal class ReplaceParameterVisitor(
        ParameterExpression oldParameter,
        ParameterExpression newParameter
        ) : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter = oldParameter;
        private readonly ParameterExpression _newParameter = newParameter;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
