#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Data.Common;

using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace FluentMigrator.Builders.Execute
{
    /// <summary>
    /// The implementation of the <see cref="IExecuteExpressionRoot"/> interface.
    /// </summary>
    public class ExecuteExpressionRoot : IExecuteExpressionRoot
    {
        private readonly IMigrationContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteExpressionRoot"/> class.
        /// </summary>
        /// <param name="context">The migration context</param>
        public ExecuteExpressionRoot(IMigrationContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public void Sql(string sqlStatement)
        {
            var expression = new ExecuteSqlStatementExpression { SqlStatement = sqlStatement };
            _context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Script(string pathToSqlScript, IDictionary<string, string> parameters)
        {
            var expression = new ExecuteSqlScriptExpression
            {
                SqlScript = pathToSqlScript,
                Parameters = parameters,
            };

            _context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void Script(string pathToSqlScript)
        {
            var expression = new ExecuteSqlScriptExpression { SqlScript = pathToSqlScript };
            _context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void WithConnection(Action<DbConnection, DbTransaction> operation)
        {
            var expression = new PerformDBOperationExpression { Operation = operation };
            _context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void EmbeddedScript(string embeddedSqlScriptName)
        {
            var embeddedResourceProviders = _context.ServiceProvider.GetRequiredService<IEnumerable<IEmbeddedResourceProvider>>();
            var expression = new ExecuteEmbeddedSqlScriptExpression(embeddedResourceProviders) { SqlScript = embeddedSqlScriptName };
            _context.Expressions.Add(expression);
        }

        /// <inheritdoc />
        public void EmbeddedScript(string embeddedSqlScriptName, IDictionary<string, string> parameters)
        {
            var embeddedResourceProviders = _context.ServiceProvider.GetRequiredService<IEnumerable<IEmbeddedResourceProvider>>();
            var expression = new ExecuteEmbeddedSqlScriptExpression(embeddedResourceProviders)
            {
                SqlScript = embeddedSqlScriptName,
                Parameters = parameters,
            };

            _context.Expressions.Add(expression);
        }
    }
}
