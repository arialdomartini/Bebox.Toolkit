using System.Security.Principal;

namespace Bebox.Toolkit.Impersonation
{
	public class ImpersonatorSession : IImpersonatorSession
	{
		private WindowsImpersonationContext _impersonationContext = null;

		public ImpersonatorSession(WindowsImpersonationContext impersonationContext)
		{
			_impersonationContext = impersonationContext;
		}

		public void Dispose()
		{
			if (_impersonationContext != null)
			{
				_impersonationContext.Undo();
			}
		}
	}
}