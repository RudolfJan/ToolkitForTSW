using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolkitForTSW
    {
    /// <summary>
    /// Interaction logic for FormInputMapperManager.xaml
    /// </summary>
    public partial class FormInputMapperManager
        {
        public CInputMapperList InputMapperList { get; set; }

        public FormInputMapperManager(CInputMapperList MyInputMapperList)
            {
            InitializeComponent();
            Contract.Assert(MyInputMapperList != null);
            InputMapperList = MyInputMapperList;
            DataContext = InputMapperList;
            }

        private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
            {
            Close();
            }
        }
    }
