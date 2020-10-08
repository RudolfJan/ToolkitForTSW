using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TSWTools
{
    public class CUModelCommand: Notifier
    {
		#region Properties
    private String _Group;
    public String Group
	    {
	    get { return _Group; }
	    set
		    {
		    _Group = value;
		    OnPropertyChanged("Group");
		    }
	    }

    private String _Command;
    public String Command
	    {
	    get { return _Command; }
	    set
		    {
		    _Command = value;
		    OnPropertyChanged("Command");
		    }
	    }

    private String _Description;
    public String Description
	    {
	    get { return _Description; }
	    set
		    {
		    _Description = value;
		    OnPropertyChanged("Description");
		    }
	    }
		#endregion

    public CUModelCommand()
	    {

	    }

    public CUModelCommand(String MyGroup, String MyCommand, String MyDescription="")
	    {
	    Group = MyGroup;
	    Command = MyCommand;
	    Description = MyDescription;
	    }
	}
}
