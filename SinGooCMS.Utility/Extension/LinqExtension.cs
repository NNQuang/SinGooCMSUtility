using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace SinGooCMS.Utility.Extension
{
    /// <summary>
    /// linq��չ����
    /// </summary>
    public static class LinqExtension
    {
        #region order by �ַ���ת linq

        /// <summary>
        /// ���OrderBy�ö��Ÿ�����exp:"Sort asc,AutoID desc"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrderByBatch<T>(this IEnumerable<T> query, string orderBy)
        {
            var index = 0;
            var arr = orderBy.Split(',');
            foreach (var item in arr)
            {
                var m = index++ > 0 ? "ThenBy" : "OrderBy";
                if (item.ToLower().Contains("desc"))
                {
                    m += "Descending";
                    orderBy = item.Replace("desc", "").Replace("DESC", "").Replace("Desc", "");
                }
                else
                {
                    orderBy = item.Replace("asc", "").Replace("ASC", "").Replace("Asc", "");
                }
                orderBy = orderBy.Trim();

                var propInfo = GetPropertyInfo(typeof(T), orderBy);
                var expr = GetOrderExpression(typeof(T), propInfo);
                var method = typeof(Enumerable).GetMethods().FirstOrDefault(mt => mt.Name == m && mt.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                query = (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
            }
            return query;
        }

        /// <summary>
        /// ���OrderBy�ö��Ÿ�����exp:"Sort asc,AutoID desc"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByBatch<T>(this IQueryable<T> query, string orderBy)
        {
            var index = 0;
            var arr = orderBy.Split(',');
            foreach (var item in arr)
            {
                var m = index++ > 0 ? "ThenBy" : "OrderBy";
                if (item.ToLower().Contains("desc"))
                {
                    m += "Descending";
                    orderBy = item.Replace("desc", "").Replace("DESC", "").Replace("Desc", "");
                }
                else
                {
                    orderBy = item.Replace("asc", "").Replace("ASC", "").Replace("Asc", "");
                }
                orderBy = orderBy.Trim();

                var propInfo = GetPropertyInfo(typeof(T), orderBy);
                var expr = GetOrderExpression(typeof(T), propInfo);
                var method = typeof(Queryable).GetMethods().FirstOrDefault(mt => mt.Name == m && mt.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                query = (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
            }
            return query;
        }

        /// <summary>
        /// �������򵥸�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string orderBy)
        {
            var propInfo = GetPropertyInfo(typeof(T), orderBy);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }
        /// <summary>
        /// �������򵥸������׸���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string orderBy)
        {
            var propInfo = GetPropertyInfo(typeof(T), orderBy);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }
        /// <summary>
        /// �������򵥸�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string orderBy)
        {
            var propInfo = GetPropertyInfo(typeof(T), orderBy);
            var expr = GetOrderExpression(typeof(T), propInfo);
            var metMethods = typeof(Queryable).GetMethods();
            var method = metMethods.FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }
        /// <summary>
        /// �������򵥸������׸���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<T> ThenByDescending<T>(this IQueryable<T> query, string orderBy)
        {
            var propInfo = GetPropertyInfo(typeof(T), orderBy);
            var expr = GetOrderExpression(typeof(T), propInfo);
            var metMethods = typeof(Queryable).GetMethods();
            var method = metMethods.FirstOrDefault(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
            {
                throw new ArgumentException("name");
            }

            return matchedProperty;
        }
        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        #endregion

        #region ������չ����

        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="left">������</param>
        /// <param name="right">������</param>
        /// <returns>�±��ʽ</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return CombineLambdas(left, right, ExpressionType.AndAlso);
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="left">������</param>
        /// <param name="right">������</param>
        /// <returns>�±��ʽ</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return CombineLambdas(left, right, ExpressionType.OrElse);
        }

        private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType expressionType)
        {
            var visitor = new SubstituteParameterVisitor
            {
                Sub =
                {
                    [right.Parameters[0]] = left.Parameters[0]
                }
            };

            Expression body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, left.Parameters[0]);
        }

        private static bool IsExpressionBodyConstant<T>(Expression<Func<T, bool>> left)
        {
            return left.Body.NodeType == ExpressionType.Constant;
        }

        /// <summary>
        /// ȡ���ֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) => source.Select(selector).OrderByDescending(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ���ֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source) => source.OrderByDescending(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ���ֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) => source.Select(selector).OrderByDescending(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ���ֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source) => source.OrderByDescending(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ��Сֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) => source.Select(selector).OrderBy(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ��Сֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source) => source.OrderBy(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ��Сֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) => source.Select(selector).OrderBy(_ => _).FirstOrDefault();

        /// <summary>
        /// ȡ��Сֵ
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source) => source.OrderBy(_ => _).FirstOrDefault();

        #endregion
    }

    internal class SubstituteParameterVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> Sub = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return Sub.TryGetValue(node, out var newValue) ? newValue : node;
        }
    }
}