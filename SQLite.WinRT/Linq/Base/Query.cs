﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SQLite.WinRT.Linq.Base
{
    /// <summary>
    ///     A default implementation of IQueryable for use with QueryProvider
    /// </summary>
    public class Query<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>,
        IOrderedQueryable
    {
        private readonly EntityProvider provider;

        public Query(EntityProvider provider, Type staticType)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            this.provider = provider;
            Expression = staticType != null ? Expression.Constant(this, staticType) : Expression.Constant(this);
        }

        public Query(EntityProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("Provider");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof (IQueryable<T>).GetTypeInfo().IsAssignableFrom(expression.Type.GetTypeInfo()))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            Expression = expression;
        }

        public string QueryText
        {
            get { return provider != null ? provider.GetQueryText(Expression) : string.Empty; }
        }

        public Expression Expression { get; private set; }

        public Type ElementType
        {
            get { return typeof (T); }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) provider.Execute(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) provider.Execute(Expression)).GetEnumerator();
        }

        public override string ToString()
        {
            if (Expression.NodeType == ExpressionType.Constant && ((ConstantExpression) Expression).Value == this)
            {
                return "Query(" + typeof (T) + ")";
            }

            return Expression.ToString();
        }
    }
}