using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config.Playerbase
{
    public class BaseModuleCollection : ObservableCollection<BaseModule>
    {
        private int _slots;

        public int Slots 
        {
            get => _slots;
            set
            {
                _slots = value;
                while (Count > _slots)
                {
                    Remove(this.Last());
                }
            }
        }

        protected override void InsertItem(int index, BaseModule item)
        {
            base.InsertItem(index, item);
        }
        protected override void SetItem(int index, BaseModule item)
        {
            base.SetItem(index, item);
        }
    }
}
