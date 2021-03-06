﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Bebox.Toolkit.Impersonation
{
	public class Impersonator : IImpersonator
	{
		public IImpersonatorSession Impersonate(string domainName, string userName, string password)
		{
			var windowsImpersonationContext = ImpersonateWithCredential(domainName, userName, password);
			return new ImpersonatorSession(windowsImpersonationContext);
		}

		private WindowsImpersonationContext ImpersonateWithCredential(string domain, string userName, string password)
		{
			var token = IntPtr.Zero;
			var tokenDuplicate = IntPtr.Zero;

			try
			{
				if (RevertToSelf())
				{
					if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
					{
						if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
							return new WindowsIdentity(tokenDuplicate).Impersonate();
						else
							throw new Win32Exception(Marshal.GetLastWin32Error());
					}
					else
						throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				else
					throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			finally
			{
				if (token != IntPtr.Zero)
					CloseHandle(token);

				if (tokenDuplicate != IntPtr.Zero)
					CloseHandle(tokenDuplicate);
			}
		}

		#region P/Invoke

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_PROVIDER_DEFAULT = 0;

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern int LogonUser(string lpszUserName, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);


		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool CloseHandle(IntPtr handle);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool RevertToSelf();

		#endregion
	}
}