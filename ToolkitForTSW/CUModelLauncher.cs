using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW
{
	public class CUModelLauncher : Notifier
		{
		#region Properties

		private String _CommandLine;

		public String CommandLine
			{
			get { return _CommandLine; }
			set
				{
				_CommandLine = value;
				OnPropertyChanged("CommandLine");
				}
			}

		private String _PathSettings;
		public String PathSettings
			{
			get { return _PathSettings; }
			set
				{
				_PathSettings = value;
				OnPropertyChanged("PathSettings");
				}
			}

		private String _Package;
		public String Package
			{
			get { return _Package; }
			set
				{
				_Package = value;
				OnPropertyChanged("Package");
				}
			}

		private CTreeItemProvider _FileTree;
		public CTreeItemProvider FileTree
			{
			get => _FileTree;
			set
				{
				_FileTree = value;
				OnPropertyChanged("FileTree");
				}
			}

		private ObservableCollection<CDirTreeItem> _TreeItems;
		public ObservableCollection<CDirTreeItem> TreeItems
			{
			get => _TreeItems;
			set
				{
				_TreeItems = value;
				OnPropertyChanged("TreeItems");
				}
			}

		private String _Results;

		public String Results
			{
			get { return _Results; }
			set
				{
				_Results = value;
				OnPropertyChanged("Results");
				}
			}

		private ObservableCollection<CUModelCommand> _UModelCommandList;

		public ObservableCollection<CUModelCommand> UModelCommandList
			{
			get { return _UModelCommandList; }
			set
				{
				_UModelCommandList = value;
				OnPropertyChanged("UModelCommandList");
				}
			}

		#endregion

		public CUModelLauncher()
			{
			BuildCommandList();
			PathSettings = "-path=" + TextHelper.QuoteFilename(CTSWOptions.UnpackFolder) + " -out=" + TextHelper.QuoteFilename(CTSWOptions.UnpackFolder +
			               "UnpackedAssets\\")+" ";
			FileTree = new CTreeItemProvider();
			TreeItems = FileTree.GetItems(CTSWOptions.UnpackFolder);
		}
		/*


Compatibility options:
    -nomesh disable loading of SkeletalMesh classes in a case of
						unsupported data format
    -noanim disable loading of MeshAnimation classes
    -nostat disable loading of StaticMesh class
    -notex disable loading of Material classes
    -nolightmap disable loading of Lightmap textures
    -sounds allow export of sounds
    -3rdparty allow 3rd party asset export(ScaleForm, FaceFX)
    -lzo|lzx|zlib force compression method for fully-compressed packages

		

*/

		public void BuildPath(String Path)
			{
			Package = System.IO.Path.GetFileName(Path);
			var MyPath = TextHelper.QuoteFilename(System.IO.Path.GetDirectoryName(Path));
			PathSettings = "-path=" + MyPath + " -out=" + TextHelper.QuoteFilename(CTSWOptions.UnpackFolder +
                     "UnpackedAssets")+" ";
		}

		private void BuildCommandList()
			{
			UModelCommandList = new ObservableCollection<CUModelCommand>
				{
				new CUModelCommand("Command", "-view",
					"(default) visualize object; when no<object> specified will load whole package"),
				new CUModelCommand("Command", "-list", "list contents of package"),
				new CUModelCommand("Command", "-export", "export specified object or whole package"),
				new CUModelCommand("Command", "-save", "save specified packages"),
				new CUModelCommand("Help", "-help", "display short help information"),
				new CUModelCommand("Help", "-version", "display umodel version information"),
				new CUModelCommand("Help", "-taglist",
					"list of tags to override game auto detection(for -game= nnn option)"),
				new CUModelCommand("Developer", "-log=<file>", "write log to the specified file"),
				new CUModelCommand("Developer", "-dump", "dump object information to console"),
				new CUModelCommand("Developer", "-pkginfo", "load package and display its information"),
				new CUModelCommand("Options", "-path=<path>",
					" path to game installation directory; if not specified, program will search for packages in current directory"),
				new CUModelCommand("Options", "-game=<tag>",
					"override game autodetection(see -taglist for variants)"),
				new CUModelCommand("Options", "-pkgver=<nnn>",
					"override package version(advanced option!)"),
				new CUModelCommand("Options", "-pkg=<package>",
					"load extra package(in addition to <package>)"),
				new CUModelCommand("Options", "-obj=<object>", "specify object (s) to load"),
				new CUModelCommand("Options", "-gui", "force startup UI to appear"),
				new CUModelCommand("Options", "-aes=<key>",
					"provide AES decryption key for encrypted pak files, key is ASCII or hex string (hex format is 0xAABBCCDD)"),
				new CUModelCommand("Viewer", "-meshes", "view meshes only"),
				new CUModelCommand("Viewer", "-materials", "view materials only(excluding textures)"),
				new CUModelCommand("Viewer", "-anim=<set>",
					"specify AnimSet to automatically attach to mesh"),
				new CUModelCommand("Export", "-out=<path>",
					"export everything into <path> instead of the current directory"),
				new CUModelCommand("Export", "-all",
					"used with -dump, will dump all objects instead of specified one"),
				new CUModelCommand("Export", "-uc", "create unreal script when possible"),
				new CUModelCommand("Export", "-lods", "export all available mesh LOD levels"),
				new CUModelCommand("Export", "-dds", "export textures in DDS format whenever possible"),
				new CUModelCommand("Export", "-notgacomp", "disable TGA compression"),
				new CUModelCommand("Export", "-nooverwrite",
					"prevent existing files from being overwritten(better performance)")
				};
			}
		}
	}
