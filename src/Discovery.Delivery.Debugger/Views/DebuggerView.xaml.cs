using Discovery.Delivery.Debugger.Converters;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discovery.Delivery.Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DebuggerView : Window
    {
        public DebuggerView()
        {
            InitializeComponent();
        }

        private void PropertyGrid_PreparePropertyItem(object sender, Xceed.Wpf.Toolkit.PropertyGrid.PropertyItemEventArgs e)
        {
            const int maxStringLength = 50;
            var propertyValue = e.PropertyItem.GetValue(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem.ValueProperty);
            if (propertyValue is string str && str.Length > maxStringLength)
            {
                e.PropertyItem.SetValue(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem.ValueProperty, str.Substring(0, maxStringLength) + " …");
                e.PropertyItem.SetValue(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem.IsReadOnlyProperty, true);
                e.PropertyItem.ToolTip = str;
            }
        }
    }
}