using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    public interface IDocumentViewModel : INotifyPropertyChanged
    {
        string Title { get; }
        bool IsActive { get; set; }
    }
}
