using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Compatibility
{

	public class GtkExpressionSearch : IExpressionSearch
	{

		public List<T> FindObjects<T>(Expression expression) where T : class
		{
			throw new System.NotImplementedException();
		}

	}

}