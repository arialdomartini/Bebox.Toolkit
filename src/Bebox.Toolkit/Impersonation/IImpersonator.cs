namespace Bebox.Toolkit.Impersonation
{
	public interface IImpersonator
	{
		IImpersonatorSession Impersonate(string domainName, string username, string password);
	}
}