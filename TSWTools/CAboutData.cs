using System;

namespace TSWTools
	{
	public class CAboutData
		{
		public String Company => AssemblyInfoProvider.AssemblyCompany;

		public String Product => AssemblyInfoProvider.AssemblyProduct;

		public String Copyright => AssemblyInfoProvider.AssemblyCopyright;

		public String Description => AssemblyInfoProvider.AssemblyDescription;

		public String Version => "0.51 alfa";

		public Uri DownloadLocation => new Uri("https://www.hollandhiking.nl/trainsimulator");
		}
	}
